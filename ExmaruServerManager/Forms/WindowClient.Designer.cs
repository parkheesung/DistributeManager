
namespace ExmaruServerManager
{
    partial class WindowClient
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btn_run = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Btn_Connect = new System.Windows.Forms.Button();
            this.TB_Port = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TB_IPAddr = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.TB_Console = new System.Windows.Forms.TextBox();
            this.TB_Command = new System.Windows.Forms.TextBox();
            this.btn_nowvote = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(this.btn_nowvote);
            this.groupBox3.Controls.Add(this.TB_Command);
            this.groupBox3.Controls.Add(this.btn_run);
            this.groupBox3.Location = new System.Drawing.Point(12, 149);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(363, 418);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Command";
            // 
            // btn_run
            // 
            this.btn_run.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_run.Location = new System.Drawing.Point(15, 368);
            this.btn_run.Name = "btn_run";
            this.btn_run.Size = new System.Drawing.Size(333, 31);
            this.btn_run.TabIndex = 0;
            this.btn_run.Text = "명령 실행";
            this.btn_run.UseVisualStyleBackColor = true;
            this.btn_run.Click += new System.EventHandler(this.btn_run_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Btn_Connect);
            this.groupBox2.Controls.Add(this.TB_Port);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.TB_IPAddr);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(363, 131);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Config";
            // 
            // Btn_Connect
            // 
            this.Btn_Connect.Location = new System.Drawing.Point(93, 85);
            this.Btn_Connect.Name = "Btn_Connect";
            this.Btn_Connect.Size = new System.Drawing.Size(255, 30);
            this.Btn_Connect.TabIndex = 8;
            this.Btn_Connect.Text = "Connect";
            this.Btn_Connect.UseVisualStyleBackColor = true;
            this.Btn_Connect.Click += new System.EventHandler(this.Btn_Connect_Click);
            // 
            // TB_Port
            // 
            this.TB_Port.Location = new System.Drawing.Point(93, 58);
            this.TB_Port.MaxLength = 10;
            this.TB_Port.Name = "TB_Port";
            this.TB_Port.Size = new System.Drawing.Size(255, 21);
            this.TB_Port.TabIndex = 3;
            this.TB_Port.Text = "25001";
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
            // TB_IPAddr
            // 
            this.TB_IPAddr.Location = new System.Drawing.Point(93, 31);
            this.TB_IPAddr.MaxLength = 50;
            this.TB_IPAddr.Name = "TB_IPAddr";
            this.TB_IPAddr.Size = new System.Drawing.Size(255, 21);
            this.TB_IPAddr.TabIndex = 1;
            this.TB_IPAddr.Text = "192.168.0.30";
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
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.TB_Console);
            this.groupBox1.Location = new System.Drawing.Point(381, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(481, 561);
            this.groupBox1.TabIndex = 3;
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
            this.TB_Console.Size = new System.Drawing.Size(468, 534);
            this.TB_Console.TabIndex = 0;
            // 
            // TB_Command
            // 
            this.TB_Command.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TB_Command.Location = new System.Drawing.Point(15, 341);
            this.TB_Command.MaxLength = 10;
            this.TB_Command.Name = "TB_Command";
            this.TB_Command.Size = new System.Drawing.Size(333, 21);
            this.TB_Command.TabIndex = 4;
            // 
            // btn_nowvote
            // 
            this.btn_nowvote.Location = new System.Drawing.Point(15, 25);
            this.btn_nowvote.Name = "btn_nowvote";
            this.btn_nowvote.Size = new System.Drawing.Size(333, 31);
            this.btn_nowvote.TabIndex = 5;
            this.btn_nowvote.Text = "이제는 배포";
            this.btn_nowvote.UseVisualStyleBackColor = true;
            this.btn_nowvote.Click += new System.EventHandler(this.btn_nowvote_Click);
            // 
            // WindowClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 585);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "WindowClient";
            this.Text = "WindowClient";
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btn_run;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button Btn_Connect;
        private System.Windows.Forms.TextBox TB_Port;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TB_IPAddr;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox TB_Console;
        private System.Windows.Forms.Button btn_nowvote;
        private System.Windows.Forms.TextBox TB_Command;
    }
}