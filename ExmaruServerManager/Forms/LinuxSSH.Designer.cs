
namespace ExmaruServerManager
{
    partial class LinuxSSH
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.TB_Console = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TB_IPAddr = new System.Windows.Forms.TextBox();
            this.TB_Port = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TB_Password = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TB_ID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Btn_Connect = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btn_httpd_status = new System.Windows.Forms.Button();
            this.btn_httpd_restart = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.TB_Console);
            this.groupBox1.Location = new System.Drawing.Point(382, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(612, 665);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Console";
            // 
            // TB_Console
            // 
            this.TB_Console.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TB_Console.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.TB_Console.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TB_Console.Font = new System.Drawing.Font("돋움", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TB_Console.ForeColor = System.Drawing.Color.Chartreuse;
            this.TB_Console.Location = new System.Drawing.Point(7, 21);
            this.TB_Console.Margin = new System.Windows.Forms.Padding(0);
            this.TB_Console.MaxLength = 99999999;
            this.TB_Console.Multiline = true;
            this.TB_Console.Name = "TB_Console";
            this.TB_Console.ReadOnly = true;
            this.TB_Console.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TB_Console.Size = new System.Drawing.Size(599, 638);
            this.TB_Console.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Btn_Connect);
            this.groupBox2.Controls.Add(this.TB_Password);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.TB_ID);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.TB_Port);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.TB_IPAddr);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(13, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(363, 187);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Config";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server IP : ";
            // 
            // TB_IPAddr
            // 
            this.TB_IPAddr.Location = new System.Drawing.Point(93, 31);
            this.TB_IPAddr.MaxLength = 50;
            this.TB_IPAddr.Name = "TB_IPAddr";
            this.TB_IPAddr.Size = new System.Drawing.Size(255, 21);
            this.TB_IPAddr.TabIndex = 1;
            this.TB_IPAddr.Text = "211.104.172.29";
            // 
            // TB_Port
            // 
            this.TB_Port.Location = new System.Drawing.Point(93, 58);
            this.TB_Port.MaxLength = 10;
            this.TB_Port.Name = "TB_Port";
            this.TB_Port.Size = new System.Drawing.Size(255, 21);
            this.TB_Port.TabIndex = 3;
            this.TB_Port.Text = "8022";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(47, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port : ";
            // 
            // TB_Password
            // 
            this.TB_Password.Location = new System.Drawing.Point(93, 118);
            this.TB_Password.MaxLength = 100;
            this.TB_Password.Name = "TB_Password";
            this.TB_Password.PasswordChar = '*';
            this.TB_Password.Size = new System.Drawing.Size(255, 21);
            this.TB_Password.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "Password : ";
            // 
            // TB_ID
            // 
            this.TB_ID.Location = new System.Drawing.Point(93, 91);
            this.TB_ID.MaxLength = 50;
            this.TB_ID.Name = "TB_ID";
            this.TB_ID.Size = new System.Drawing.Size(255, 21);
            this.TB_ID.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "Login ID : ";
            // 
            // Btn_Connect
            // 
            this.Btn_Connect.Location = new System.Drawing.Point(93, 146);
            this.Btn_Connect.Name = "Btn_Connect";
            this.Btn_Connect.Size = new System.Drawing.Size(255, 30);
            this.Btn_Connect.TabIndex = 8;
            this.Btn_Connect.Text = "Connect";
            this.Btn_Connect.UseVisualStyleBackColor = true;
            this.Btn_Connect.Click += new System.EventHandler(this.Btn_Connect_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(this.btn_httpd_restart);
            this.groupBox3.Controls.Add(this.btn_httpd_status);
            this.groupBox3.Location = new System.Drawing.Point(13, 207);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(363, 465);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Command";
            // 
            // btn_httpd_status
            // 
            this.btn_httpd_status.Location = new System.Drawing.Point(15, 21);
            this.btn_httpd_status.Name = "btn_httpd_status";
            this.btn_httpd_status.Size = new System.Drawing.Size(333, 31);
            this.btn_httpd_status.TabIndex = 0;
            this.btn_httpd_status.Text = "httpd 상태확인";
            this.btn_httpd_status.UseVisualStyleBackColor = true;
            this.btn_httpd_status.Click += new System.EventHandler(this.btn_httpd_status_Click);
            // 
            // btn_httpd_restart
            // 
            this.btn_httpd_restart.Location = new System.Drawing.Point(15, 58);
            this.btn_httpd_restart.Name = "btn_httpd_restart";
            this.btn_httpd_restart.Size = new System.Drawing.Size(333, 31);
            this.btn_httpd_restart.TabIndex = 1;
            this.btn_httpd_restart.Text = "httpd 재시작";
            this.btn_httpd_restart.UseVisualStyleBackColor = true;
            this.btn_httpd_restart.Click += new System.EventHandler(this.btn_httpd_restart_Click);
            // 
            // LinuxSSH
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1006, 690);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "LinuxSSH";
            this.Text = "LinuxSSH";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox TB_Console;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button Btn_Connect;
        private System.Windows.Forms.TextBox TB_Password;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TB_ID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TB_Port;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TB_IPAddr;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btn_httpd_restart;
        private System.Windows.Forms.Button btn_httpd_status;
    }
}