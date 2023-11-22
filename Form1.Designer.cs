namespace SendSms
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.sendSmsTimer = new System.Windows.Forms.Timer(this.components);
            this.historyAndDeleteTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // sendSmsTimer
            // 
            this.sendSmsTimer.Enabled = true;
            this.sendSmsTimer.Interval = 1000;
            this.sendSmsTimer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // historyAndDeleteTimer
            // 
            this.historyAndDeleteTimer.Enabled = true;
            this.historyAndDeleteTimer.Interval = 2000;
            this.historyAndDeleteTimer.Tick += new System.EventHandler(this.historyAndDeleteTimer_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 466);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Opacity = 0D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.form_closing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer sendSmsTimer;
        private System.Windows.Forms.Timer historyAndDeleteTimer;
    }
}

