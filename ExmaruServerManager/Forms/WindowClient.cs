using DM.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OctopusV3.Core;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;

namespace ExmaruServerManager
{
    public partial class WindowClient : Form
    {
        protected MainForm main { get; set; }

        protected delegate void ConsoleDelegate(string msg);

        protected ConsoleDelegate consoleWrite { get; set; }

        public WindowClient(MainForm _main)
        {
            this.main = _main;
            consoleWrite = new ConsoleDelegate(UIConsoleWrite);
            InitializeComponent();
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

                if (string.IsNullOrWhiteSpace(IPv4))
                {
                    MessageBox.Show("IP주소를 입력해 주세요.");
                    TB_IPAddr.Focus();
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
                        SocketClient.Current.Setup(IPv4, Convert.ToInt32(Port));
                        SocketClient.Current.Receive = new Action<string>((msg) =>
                        {
                            if (!string.IsNullOrWhiteSpace(msg))
                            {
                                Invoke(consoleWrite,msg);
                            }
                        });
                        SocketClient.Current.Connect(() => {
                            Btn_Connect.Text = "Disconnect";
                            UIConsoleWrite("Connection Success");
                        });
                    }
                    catch (Exception ex)
                    {
                        UIConsoleWrite(ex.Message);
                    }
                }
            }
            else
            {
                SocketClient.Current.Dispose();
                Btn_Connect.Text = "Connect";
                UIConsoleWrite("Disconnect!");
            }
        }

        private void btn_nowvote_Click(object sender, EventArgs e)
        {
            TB_Command.Text = @"D:\WebService\NowVote_Publish.bat";
        }

        private void btn_run_Click(object sender, EventArgs e)
        {
            string cmdTxt = TB_Command.Text;

            if (string.IsNullOrWhiteSpace(cmdTxt))
            {
                MessageBox.Show("명령어를 입력해 주세요.");
            }
            else
            {
                var command = new ServerCommand()
                {
                    Command = "CMD",
                    Original = cmdTxt,
                    Target = ""
                };
                SocketClient.Current.Send(JsonConvert.SerializeObject(command));
                UIConsoleWrite($"다음 명령이 전달되었습니다. -> {cmdTxt}");
            }

        }
    }
}
