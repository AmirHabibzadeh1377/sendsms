﻿using Newtonsoft.Json;
using RestSharp;
using SendSms.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;

namespace SendSms
{
    public partial class Form1 : Form
    {
        #region Fields

        /// <summary>
        /// فیلد مر بوط به دیتا کانتکس و ارتباط با دیتابیس
        /// </summary>
        SmsContext _conection;

        /// <summary>
        /// فیلد مربوط به ApiKey دریافتی از پنل اس ام اس
        /// </summary>
        string ghasedakApiKey = ConfigurationManager.AppSettings.Get("GhasedakApiKey");

        /// <summary>
        /// فیلد مربوط به خط خدماتی دریافتی از سامانه پیامک
        /// </summary>
        string lineNumber = ConfigurationManager.AppSettings.Get("LineNumber");

        /// <summary>
        /// کاراکتر تعریف شده برای جداسازی پامک ها(این فیلد ها از قسمت App.Config مقدار دهی میشوند)
        /// </summary>
        string splitSperator = ConfigurationManager.AppSettings.Get("SpliSeperator");

        /// <summary>
        /// این فیلد مشخص میکند که پیام خط به خط ارسال بشود یا تجمیعی که به صورت true یا false در App.config تعریف میشود
        /// </summary>
        string useSplitSperator = ConfigurationManager.AppSettings.Get("UseSplitSeperator");

        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _conection = new SmsContext();
            var entities = _conection.SMSAlarmsSendSumms.Where(x => !x.SMSRecievedFlag.Value).ToList();
            var smsVms = new List<SmsVm>();
            foreach (var entity in entities)
            {
                var mobiles = new List<string>();
                mobiles.Add(entity.RecipientMobileNo);
                SmsVm sms = new SmsVm
                {
                    //this pram for test
                    Param2 = entity.MessageText,
                    Message = entity.MessageText,
                    Mobile = entity.RecipientMobileNo,
                };
                smsVms.Add(sms);
            }

            foreach (var smsVm in smsVms)
            {
                var messageLength = smsVm.Message.Length;
                var messageArray = smsVm.Message.Split(Convert.ToChar(splitSperator)).ToList();

                if (smsVm.Message.Length < 900)
                {
                    var response = SendSMS(smsVm);
                    if (response.result.code == 200)
                    {
                        GetGhasedakStatus(response);
                    }
                }
                else
                {

                    string a = "";
                    foreach (var message in messageArray.ToList())
                    {
                        if (useSplitSperator == "false")
                        {
                            if (!string.IsNullOrWhiteSpace(message))
                            {
                                a += message;
                                messageArray.Remove(message);
                                if (a.Length > 800 && a.Length < 900)
                                {
                                    smsVm.Message = a;
                                    var response = SendSMS(smsVm);
                                    if (response.result.code == 200)
                                    {
                                        GetGhasedakStatus(response);
                                    }
                                    messageLength -= a.Length;
                                    a = "";
                                }
                                else if (messageLength < 800)
                                {
                                    foreach(var messageOfArray in messageArray.ToList())
                                    {
                                        if (!string.IsNullOrWhiteSpace(messageOfArray))
                                        {
                                            a += messageOfArray;
                                        }
                                        messageArray.Remove(messageOfArray);
                                    }
                                    var length = a.Length;
                                    smsVm.Message = a;
                                    var response = SendSMS(smsVm);
                                    if (response.result.code == 200)
                                    {
                                        GetGhasedakStatus(response);
                                    }
                                    a = "";
                                }
                            }
                        }
                        if (messageArray.Count == 0)
                            break;

                        if (useSplitSperator == "true")
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
                request.AddHeader("apikey", ghasedakApiKey);
                request.AddParameter("message", smsModel.Message);
                request.AddParameter("receptor", smsModel.Mobile);
                request.AddParameter("linenumber", lineNumber);
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
            restRequest.AddHeader("apikey", ghasedakApiKey);
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
