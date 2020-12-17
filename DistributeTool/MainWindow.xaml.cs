using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;
using System.Windows.Threading;
using System.Data;
using OctopusV3.Core;
using System.Net.Sockets;
using System.Threading;
using System.Configuration;
using DM.Library;
using Newtonsoft.Json;

namespace DistributeTool
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        TcpClient Client;

        StreamReader Reader;

        StreamWriter Writer;

        NetworkStream stream;

        Thread ReceiveThread;

        bool Connected;

        string ReplaceOriginal = string.Empty;
        string ReplaceTarget = string.Empty;


        public delegate void CallbackEvent(FolderFileHelper helper);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Search_Original_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog browser = new FolderBrowserDialog();
            if (browser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TB_Original_Path.Text = browser.SelectedPath;
            }
        }

        private void Search_Target_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog browser = new FolderBrowserDialog();
            if (browser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TB_Target_Path.Text = browser.SelectedPath;
            }
        }

        public void OriginalEventProc(FolderFileHelper helper)
        {
            DataGrid_Original.ItemsSource = helper.Files.OrderByDescending(x => x.LastWriteTime);
        }

        public void TargetEventProc(FolderFileHelper helper)
        {
            DataGrid_Target.ItemsSource = helper.Files.OrderByDescending(x => x.LastWriteTime);
        }


        private async void Load_Files_Click(object sender, RoutedEventArgs e)
        {
            CallbackEvent origialEvent = new CallbackEvent(OriginalEventProc);
            CallbackEvent targetEvent = new CallbackEvent(TargetEventProc);

            await Task.Factory.StartNew(() =>
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {
                    using (var original = new FolderFileHelper(this))
                    {
                        original.GetAllFiles(TB_Original_Path.Text);
                        origialEvent(original);
                    }
                }));

            });

            await Task.Factory.StartNew(() =>
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {
                    using (var target = new FolderFileHelper(this))
                    {
                        target.GetAllFiles(TB_Target_Path.Text);
                        targetEvent(target);
                    }
                }));
            });
        }

        private void Publish_Click(object sender, RoutedEventArgs e)
        {
            ReplaceOriginal = TB_Original_Replace.Text;
            ReplaceTarget = TB_Target_Replace.Text;


            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                var rows = DataGrid_Original.SelectedItems;
                if (rows != null && rows.Count > 0)
                {
                    ServerCommand command = null;
                    FileInfo file = null;
                    string tmp = string.Empty;
                    foreach(var row in rows)
                    {
                        file = row as FileInfo;
                        command = new ServerCommand()
                        {
                            Command = "COPY",
                            Original = file.FullName.Replace(ReplaceOriginal, ReplaceTarget),
                            Target = ConvertPath(file.FullName).Replace(ReplaceOriginal, ReplaceTarget)
                        };

                        Writer.WriteLine(JsonConvert.SerializeObject(command)); // 보내버리기
                        Writer.Flush();
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("선택된 대상이 없습니다.");
                }
            }));
            

        }

        private void Server_Connect_Click(object sender, RoutedEventArgs e)
        {
            if (Btn_Connect.Content.ToString() == "종료")
            {
                if (Client != null)
                {
                    Client.Close();
                    Client.Dispose();
                    ReceiveThread.Abort();

                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                    {
                        Txt_Connect.Text = "NO Connection";
                        Btn_Connect.Content = "접속";
                    }));
                }
            }
            else
            {
                string IP = TB_Server.Text;
                int port = Convert.ToInt32(TB_Port.Text);
                Client = new TcpClient();
                Client.Connect(IP, port);
                stream = Client.GetStream();
                Connected = true;
                Reader = new StreamReader(stream);
                Writer = new StreamWriter(stream);
                ReceiveThread = new Thread(new ThreadStart(Receive));
                ReceiveThread.Start();
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {
                    Txt_Connect.Text = "Connected";
                    Btn_Connect.Content = "종료";
                }));
            }
        }

        private void Receive()
        {
            while (Connected)
            {
                Thread.Sleep(1);

                try
                {
                    if (stream.CanRead)
                    {
                        string tempStr = Reader.ReadLine();

                        if (tempStr.Length > 0)
                        {

                        }
                    }
                }
                catch
                {
                }
            }
        }

        public string ConvertPath(string path)
        {
            string result = path.Trim();

            if (!string.IsNullOrEmpty(result))
            {
                result = result.Replace(TB_Original_Path.Text, TB_Target_Path.Text);
            }

            return result;
        }
    }
}
