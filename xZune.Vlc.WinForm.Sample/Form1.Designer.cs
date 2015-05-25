namespace xZune.Vlc.WinForm.Sample
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.vlcPlayer1 = new xZune.Vlc.WinForm.VlcPlayer();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(0, 429);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(545, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Play";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // vlcPlayer1
            // 
            this.vlcPlayer1.AutoSize = true;
            this.vlcPlayer1.BackColor = System.Drawing.Color.White;
            this.vlcPlayer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vlcPlayer1.Length = System.TimeSpan.Parse("00:00:00");
            this.vlcPlayer1.LibVlcPath = "..\\..\\..\\..\\..\\LibVlc";
            this.vlcPlayer1.Location = new System.Drawing.Point(0, 0);
            this.vlcPlayer1.Name = "vlcPlayer1";
            this.vlcPlayer1.Size = new System.Drawing.Size(545, 452);
            this.vlcPlayer1.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 452);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.vlcPlayer1);
            this.Name = "Form1";
            this.Text = "xZune.Vlc for WinForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private VlcPlayer vlcPlayer1;
    }
}

