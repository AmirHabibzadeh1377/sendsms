using Newtonsoft.Json;

using RestSharp;

using SendSms.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace SendSms
{
    public partial class Form1 : Form
    {
        #region Fields

        SmsContext _conection;
        const string SMS_APIKEY = "72f3f7b27d3a48e83f40346ff0cf2ed54a8f94bbac24b266842d8c2dd0ca73cd";

        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _conection = new SmsContext();
            var entities = _conection.SMSAlarmsSendSumms.ToList();
            var smsVms = new List<SmsVm>();
            foreach (var entity in entities)
            {
                var mobiles = new List<string>();
                mobiles.Add(entity.RecipientMobileNo);
                SmsVm sms = new SmsVm
                {
                    //this pram for test
                    Param1 = "amir habibzadeh",
                    Param2 = entity.MessageText,
                    Param3 = "",
                    Message = entity.MessageText,
                    Mobile = entity.RecipientMobileNo,
                    TemplateName = Tools.SmsTemplate.ShowHomeWorkByTeacher
                };
                smsVms.Add(sms);
            }

            foreach (var smsVm in smsVms)
            {
                var messageLengtj = smsVm.Message.Length;
                var messageArray = smsVm.Message.Split('/');
                foreach (var message in messageArray)
                {
                    smsVm.Message = message;
                    var response = SendSMS(smsVm);
                    if (response.result.code == 200)
                    {
                        GetGhasedakStatus(response);
                    }
                }
            }
        }

        private void historyAndDeleteTimer_Tick(object sender, EventArgs e)
        {
            _conection = new SmsContext();
            var lastMonth = DateTime.Now.AddMonths(-1);
            var smsAlarms = _conection.SMSAlarmsSendSumms.Where(x => x.RecievedDate.Value < lastMonth);
            foreach (var smsAlarm in smsAlarms)
            {
                _conection.SMSAlarmsSendSumms.Remove(smsAlarm);
            }

            _conection.SaveChanges();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var conetxt = new SmsContext();
            var run = conetxt.RunForms.FirstOrDefault();
            var flag = run.RunFlag;
        }

        #region Ghasedak Methods

        public GhasedakSendResponseModel SendSMS(SmsVm smsModel)
        {
            try
            {
                _conection = new SmsContext();
                var restClient = new RestClient("https://api.ghasedak.me/v2/sms/send/simple");
                var request = new RestRequest("", Method.Post);
                request.AddHeader("apikey", SMS_APIKEY);
                request.AddParameter("message", smsModel.Message);
                request.AddParameter("receptor", smsModel.Mobile);
                request.AddParameter("linenumber", "90004917");
                RestResponse response = restClient.Execute(request);
                var model = JsonConvert.DeserializeObject<GhasedakSendResponseModel>(response.Content);
                if (model.result.code == 200)
                {
                    var smsAlarm = _conection.SMSAlarmsSendSumms.FirstOrDefault(x => x.RecipientMobileNo == smsModel.Mobile);
                    smsAlarm.SMSSendFlag = true;
                    _conection.SaveChanges();
                }
                return model;
            }
            catch (Exception ex)
            {
                return new GhasedakSendResponseModel();
            }
        }

        public void GetGhasedakStatus(GhasedakSendResponseModel model)
        {
            _conection = new SmsContext();
            var messageId = model.items[0];
            var rest = new RestClient("https://api.ghasedak.me/v2/sms/status");
            var restRequest = new RestRequest();
            restRequest.AddHeader("apikey", SMS_APIKEY);
            restRequest.AddParameter("id", messageId);
            restRequest.AddParameter("type", "1");
            var response = rest.Execute(restRequest);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var statusModel = JsonConvert.DeserializeObject<GhasedakStatusResponseModel>(response.Content);
                var item = statusModel.Items[0];
                var statusMessage = CheckStatusCode(item.StatusCode);

                var smsAlarm = _conection.SMSAlarmsSendSumms.FirstOrDefault(x => x.RecipientMobileNo == item.Receptor);
                smsAlarm.SMSRecievedPanel = statusMessage;
                if (statusMessage != "ارسال با خطا" && statusMessage != "نرسیده به مخابرات" && statusMessage != "شماره گیرنده جزو لیست سیاه است" && statusMessage != "شناسه نا معتبر است")
                {
                    smsAlarm.SMSRecievedFlag = true;
                    smsAlarm.RecievedDate = DateTime.Now;
                    smsAlarm.RecievedTime = DateTime.Now.ToShortTimeString();
                }
                _conection.SaveChanges();
            }
        }

        public string CheckStatusCode(int code)
        {
            var message = "";
            switch (code)
            {
                case 0:
                    message = "تحویل به اپراتور";
                    break;
                case 1:
                    message = "رسیده به گوشی";
                    break;
                case 2:
                    message = "ارسال با خطا";
                    break;
                case 8:
                    message = "رسیده به مخابرات";
                    break;
                case 16:
                    message = "نرسیده به مخابرات";
                    break;
                case 27:
                    message = "شماره گیرنده جزو لیست سیاه است";
                    break;
                case -1:
                    message = "شناسه نا معتبر است";
                    break;
            }

            return message;
        }

        #endregion

        private void form_closing(object sender, FormClosingEventArgs e)
        {
            var conetxt = new SmsContext();
            var run = conetxt.RunForms.FirstOrDefault();
            run.RunFlag = false;
            conetxt.SaveChanges();
        }
    }
}
