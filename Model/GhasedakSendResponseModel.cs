namespace SendSms.Model
{
    public class GhasedakSendResponseModel
    {
        public result result{ get; set; }
        public long[] items { get; set; }
    }

    public class result
    {
        public int code { get; set; }
        public string message{ get; set; }
    }
}
