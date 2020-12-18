using DM.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;

namespace DistributeTool
{
    public partial class MainWindow : Window
    {
        delegate void AppendTextDelegate(string s);

        string ReplaceOriginal = string.Empty;
        string ReplaceTarget = string.Empty;


        public delegate void CallbackEvent(FolderFileHelper helper);

        private delegate void AddTextDelegate(string strText);

        private ConcurrentQueue<ServerCommand> WorkQueue { get; set; } = new ConcurrentQueue<ServerCommand>();

        private System.Threading.Timer WorkTimer { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        void AppendText(string s)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                Txt_Status.AppendText(s);
                Txt_Status.AppendText(Environment.NewLine);
            }));
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
            string tmp = Txt_Filter.Text;
            if (!string.IsNullOrWhiteSpace(tmp))
            {
                string[] arr = tmp.Split(';');
                DataGrid_Original.ItemsSource = from c in helper.GetFiles
                                                where !(from o in arr select o).Contains(c.Ext)
                                                select c;
            }
            else
            {
                DataGrid_Original.ItemsSource = helper.GetFiles;
            }
        }

        private async void Load_Files_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TB_Original_Path.Text))
            {
                System.Windows.Forms.MessageBox.Show("원본경로를 설정해 주세요.");
            }
            else
            {
                CallbackEvent origialEvent = new CallbackEvent(OriginalEventProc);

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
            }
        }

        private void Publish_Click(object sender, RoutedEventArgs e)
        {
            ReplaceOriginal = TB_Original_Replace.Text;
            ReplaceTarget = TB_Target_Replace.Text;

            if (string.IsNullOrWhiteSpace(TB_Original_Path.Text))
            {
                System.Windows.Forms.MessageBox.Show("원본경로를 설정해 주세요.");
            }
            else if (string.IsNullOrWhiteSpace(TB_Target_Path.Text))
            {
                System.Windows.Forms.MessageBox.Show("대상경로를 설정해 주세요.");
            }
            else
            {

                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {
                    try
                    {
                        var rows = DataGrid_Original.SelectedItems;
                        if (rows != null && rows.Count > 0)
                        {
                            ServerCommand command = null;
                            foreach (GridInfo file in rows)
                            {
                                command = new ServerCommand()
                                {
                                    Command = "COPY",
                                    Original = file.FullPath.Replace(ReplaceOriginal, ReplaceTarget),
                                    Target = ConvertPath(file.FullPath).Replace(ReplaceOriginal, ReplaceTarget)
                                };

                                WorkQueue.Enqueue(command);
                            }
                        }
                        else
                        {
                            AppendText("선택된 대상이 없습니다.");
                        }
                    }
                    catch (Exception ex)
                    {
                        AppendText(ex.Message);
                    }
                }));
            }

        }

        private void Server_Connect_Click(object sender, RoutedEventArgs e)
        {
            if (Btn_Connect.Content.ToString() == "종료")
            {
                SocketClient.Current.Dispose();
                AppendText("접속이 종료됩니다.");
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {
                    Txt_Connect.Text = "NO Connection";
                    Btn_Connect.Content = "접속";
                }));
                this.WorkTimer.Dispose();
                this.WorkTimer = null;
            }
            else
            {
                string IP = TB_Server.Text;
                int port = Convert.ToInt32(TB_Port.Text);
                SocketClient.Current.Setup(IP, port);
                SocketClient.Current.Receive = new Action<string>((msg) =>
                {
                    if (!string.IsNullOrWhiteSpace(msg))
                    {
                        AppendText(msg);
                    }
                });
                SocketClient.Current.Connect(() => {
                    AppendText("서버에 접속되었습니다.");
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                    {
                        Txt_Connect.Text = "Connected";
                        Btn_Connect.Content = "종료";
                    }));
                });

                this.WorkTimer = new System.Threading.Timer(Work_Tick, null, 1000, 1000);
            }
        }

        private void Work_Tick(object state)
        {
            ServerCommand command = null;
            if (WorkQueue.TryDequeue(out command))
            {
                SocketClient.Current.Send(JsonConvert.SerializeObject(command));
                AppendText($"{command.Original} 파일을 복사 요청했습니다.");
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
