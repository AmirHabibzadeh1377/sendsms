using System.Data.Entity;

namespace SendSms.Model
{
    public partial class SmsContext : DbContext
    {
        public SmsContext()
            : base("name=SmsAlarmContext")
        {
        }

        public virtual DbSet<RunForm> RunForms { get; set; }
        public virtual DbSet<SMSAlarmsSendSumm> SMSAlarmsSendSumms { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SMSAlarmsSendSumm>()
                .Property(e => e.AICode)
                .HasPrecision(18, 0);

            modelBuilder.Entity<SMSAlarmsSendSumm>()
                .Property(e => e.AICodeASSMS)
                .HasPrecision(18, 0);
        }
    }
}
