using System.Text.Json.Serialization;

namespace SendSms.Model
{

    public class GhasedakStatusResponseModel
    {
        [JsonPropertyName("result")]
        public Result Result { get; set; }

        [JsonPropertyName("items")]
        public Items[] Items { get; set; }
    }

    public class Result
    {
        [JsonPropertyName("code")]
        public int Code{ get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
  
    public class Items
    {
        [JsonPropertyName("messageid")]
        public long MessageId { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("status")]
        public int StatusCode { get; set; }

        [JsonPropertyName("price")]
        public int Price { get; set; }

        [JsonPropertyName("sender")]
        public string Sender { get; set; }

        [JsonPropertyName("senddate")]
        public string SenderDate { get; set; }

        [JsonPropertyName("receptor")]
        public string Receptor { get; set; }
    }

}