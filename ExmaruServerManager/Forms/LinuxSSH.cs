using DM.Library;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExmaruServerManager
{
    public partial class LinuxSSH : Form
    {
        protected MainForm main { get; set; }

        protected delegate void ConsoleDelegate(string msg);

        protected ConsoleDelegate consoleWrite { get; set; }

        protected SshHandler ssh { get; set; }
        protected SshClient client { get; set; }

        public LinuxSSH(MainForm _main)
        {
            this.main = _main;
            consoleWrite = new ConsoleDelegate(UIConsoleWrite);
            InitializeComponent();
            ConsoleWrite("Ready...");
        }

        public void ConsoleWrite(string msg)
        {
            if (TB_Console.InvokeRequired)
            {
                Invoke(consoleWrite, msg);
            }
            else
            {
                UIConsoleWrite(msg);
            }
        }

        protected void UIConsoleWrite(string msg)
        {
            TB_Console.AppendText(msg);
            TB_Console.AppendText(Environment.NewLine);
        }

        private void Btn_Connect_Click(object sender, EventArgs e)
        {
            if (Btn_Connect.Text.Equals("Connect", StringComparison.OrdinalIgnoreCase))
            {
                string IPv4 = TB_IPAddr.Text;
                string Port = TB_Port.Text;
                string AccountID = TB_ID.Text;
                string Password = TB_Password.Text;

                if (string.IsNullOrWhiteSpace(IPv4))
                {
                    MessageBox.Show("IP주소를 입력해 주세요.");
                    TB_IPAddr.Focus();
                }
                else if (string.IsNullOrWhiteSpace(AccountID))
                {
                    MessageBox.Show("아이디를 입력해 주세요.");
                    TB_ID.Focus();
                }
                else if (string.IsNullOrWhiteSpace(Password))
                {
                    MessageBox.Show("비밀번호를 입력해 주세요.");
                    TB_Password.Focus();
                }
                else if (string.IsNullOrWhiteSpace(Port))
                {
                    MessageBox.Show("포트값을 입력해 주세요.");
                    TB_Port.Focus();
                }
                else
                {
                    try
                    {
                        this.ssh = new SshHandler(IPv4, Convert.ToInt32(Port), AccountID, Password);
                        this.client = new SshClient(ssh.Connector);
                        client.Connect();
                        Btn_Connect.Text = "Disconnect";
                        UIConsoleWrite("Connection Success");
                    }
                    catch (Exception ex)
                    {
                        UIConsoleWrite(ex.Message);
                    }
                }
            }
            else
            {
                client.Disconnect();
                client.Dispose();
                ssh.Dispose();
                Btn_Connect.Text = "Connect";
                UIConsoleWrite("Disconnect!");
            }
        }

        private void btn_httpd_status_Click(object sender, EventArgs e)
        {
            string output = client.RunCommand("systemctl status httpd.service").Result;
            UIConsoleWrite(output);
        }

        private void btn_httpd_restart_Click(object sender, EventArgs e)
        {
            string output = client.RunCommand("systemctl restart httpd.service").Result;
            UIConsoleWrite(output);
        }
    }
}
