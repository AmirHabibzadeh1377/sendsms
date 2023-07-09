namespace SendSms.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RunForm")]
    public partial class RunForm
    {
        public int Id { get; set; }

        public bool? RunFlag { get; set; }
    }
}
