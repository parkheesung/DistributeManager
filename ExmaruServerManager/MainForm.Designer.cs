
namespace ExmaruServerManager
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.일반ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.환경설정ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.종료ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.원격제어ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.리눅스서버ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.일반ToolStripMenuItem,
            this.원격제어ToolStripMenuItem,
            this.리눅스서버ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 일반ToolStripMenuItem
            // 
            this.일반ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.환경설정ToolStripMenuItem,
            this.종료ToolStripMenuItem});
            this.일반ToolStripMenuItem.Name = "일반ToolStripMenuItem";
            this.일반ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.일반ToolStripMenuItem.Text = "일반";
            // 
            // 환경설정ToolStripMenuItem
            // 
            this.환경설정ToolStripMenuItem.Name = "환경설정ToolStripMenuItem";
            this.환경설정ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.환경설정ToolStripMenuItem.Text = "환경설정";
            // 
            // 종료ToolStripMenuItem
            // 
            this.종료ToolStripMenuItem.Name = "종료ToolStripMenuItem";
            this.종료ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.종료ToolStripMenuItem.Text = "종료";
            this.종료ToolStripMenuItem.Click += new System.EventHandler(this.종료ToolStripMenuItem_Click);
            // 
            // 원격제어ToolStripMenuItem
            // 
            this.원격제어ToolStripMenuItem.Name = "원격제어ToolStripMenuItem";
            this.원격제어ToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.원격제어ToolStripMenuItem.Text = "윈도우서버";
            this.원격제어ToolStripMenuItem.Click += new System.EventHandler(this.원격제어ToolStripMenuItem_Click);
            // 
            // 리눅스서버ToolStripMenuItem
            // 
            this.리눅스서버ToolStripMenuItem.Name = "리눅스서버ToolStripMenuItem";
            this.리눅스서버ToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.리눅스서버ToolStripMenuItem.Text = "리눅스서버";
            this.리눅스서버ToolStripMenuItem.Click += new System.EventHandler(this.리눅스서버ToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "서버관리자";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 일반ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 환경설정ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 종료ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 원격제어ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 리눅스서버ToolStripMenuItem;
    }
}

