using SendSms.Model;
using System;
using System.Linq;
using System.Windows.Forms;

namespace SendSms
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var context = new SmsContext();
            var run = context.RunForms.FirstOrDefault();
            if (run.RunFlag == false)
            {
                run.RunFlag = true;
                context.SaveChanges();
                Application.Run(new Form1());
            }
            else
            {
                Application.Exit();
            }
        }
    }
}