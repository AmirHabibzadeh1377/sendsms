namespace SendSms.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SMSAlarmsSendSumm")]
    public partial class SMSAlarmsSendSumm
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal AICode { get; set; }

        [Key]
        [Column(TypeName = "numeric")]
        public decimal AICodeASSMS { get; set; }

        public int AlarmFlag { get; set; }

        [Required]
        [StringLength(10)]
        public string SendDate { get; set; }

        [Required]
        [StringLength(10)]
        public string SendTime { get; set; }

        public int SMSDelTime { get; set; }

        [Required]
        [StringLength(50)]
        public string RecipientMobileNo { get; set; }

        [Required]
        public string MessageText { get; set; }

        public bool? SMSSendFlag { get; set; }

        [StringLength(50)]
        public string SMSRecievedPanel { get; set; }

        [StringLength(50)]
        public string SMSRecievedDCenter { get; set; }

        public bool? SMSRecievedFlag { get; set; }

        public DateTime? RecievedDate { get; set; }

        [StringLength(10)]
        public string RecievedTime { get; set; }
    }
}
