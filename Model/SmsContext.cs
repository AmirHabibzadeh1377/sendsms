using System.Data.Entity;

namespace SendSms.Model
{
    public partial class SmsContext : DbContext
    {
        public SmsContext()
            : base("data source=DESKTOP-CPGCT09;initial catalog=SendSms;user id=sa;password=123;MultipleActiveResultSets=True;App=EntityFramework")
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
