using SendSms.Tools;
using System.Collections.Generic;

namespace SendSms.Model
{
public class SmsVm
    {
        public string Message { get; set; }
        public SmsTemplate TemplateName { get; set; }
        public List<string> Receptor { get; set; }
        public ushort Timer { get; set; }
        public string Mobile { get; set; }
        public string Param1 { get; set; }
        public string Param2 { get; set; }
        public string Param3 { get; set; }
        public string Param4 { get; set; }
        public string Param5 { get; set; }
        public string Param6 { get; set; }
        public string Param7 { get; set; }
        public string Param8 { get; set; }
        public string Param9 { get; set; }
        public string Param10 { get; set; }
    }
}