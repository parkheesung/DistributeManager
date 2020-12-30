using System;
using System.Windows.Forms;

namespace ExmaruServerManager
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void 리눅스서버ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool chk = true;

            foreach (System.Windows.Forms.Form theForm in this.MdiChildren)
            {
                if (theForm.Name.Equals("LinuxSSH", StringComparison.OrdinalIgnoreCase))
                {
                    //해당form의 인스턴스가 존재하면 해당 창을 활성시킨다.
                    theForm.BringToFront();
                    theForm.Focus();
                    chk = false;
                }
            }

            if (chk)
            {
                LinuxSSH target = new LinuxSSH(this);
                target.Name = "LinuxSSH";
                target.MdiParent = this;
                target.WindowState = FormWindowState.Maximized;
                target.MinimizeBox = false;
                target.Show();
            }
        }

        private void 원격제어ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool chk = true;

            foreach (System.Windows.Forms.Form theForm in this.MdiChildren)
            {
                if (theForm.Name.Equals("WindowClient", StringComparison.OrdinalIgnoreCase))
                {
                    //해당form의 인스턴스가 존재하면 해당 창을 활성시킨다.
                    theForm.BringToFront();
                    theForm.Focus();
                    chk = false;
                }
            }

            if (chk)
            {
                WindowClient target = new WindowClient(this);
                target.Name = "WindowClient";
                target.MdiParent = this;
                target.WindowState = FormWindowState.Maximized;
                target.MinimizeBox = false;
                target.Show();
            }
        }

        private void 종료ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
