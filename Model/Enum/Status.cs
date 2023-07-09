using System.ComponentModel;

namespace SendSms.Model.Enum
{
    public enum Status
    {
        [Description("تحویل به اپراتور")]
        ReceivedToOperator = 0,

        [Description("رسیده به گوشی")]
        ReceivedToPhone = 2,

        [Description("رسیده به مخابرات")]
        ReceivedToTelecommunications = 8,

        [Description("نرسیده به مخابرات")]
        NotReceivedToTelecommunications = 16,

        [Description("شماره گیرنده جزو لیست سیاه است")]
        TheRecipientsNumberIsBlackListed = 27,

        [Description("شناسه ارسال شده نا معتبر است")]
        IdIsWorng = -1
    }
}