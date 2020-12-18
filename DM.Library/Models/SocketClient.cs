using OctopusV3.Core;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace DM.Library
{
    public class SocketClient : ISocketClient, ISocketConnection, IDisposable
    {
        private static readonly Lazy<SocketClient> lazy = new Lazy<SocketClient>(() => new SocketClient());

        public static SocketClient Current { get { return lazy.Value; } }


        private bool disposedValue;

        public bool IsConnection { get; set; } = false;
        public IPEndPoint EndPoint { get; set; }

        protected TcpClient Client { get; set; }

        protected StreamReader Reader { get; set; }

        protected StreamWriter Writer { get; set; }

        protected NetworkStream stream { get; set; }

        protected Thread ReceiveThread { get; set; }

        protected string IPaddr { get; set; } = string.Empty;

        protected int Port { get; set; }

        public Action<string> Receive { get; set; }

        public ILogHelper Logger { get; set; }


        public SocketClient()
        {
        }

        public void Setup(string ip, int port, int maximumReady = 0)
        {
            this.IPaddr = ip;
            this.Port = port;
            this.Client = new TcpClient();
        }

        public void Connect(Action Complete = null)
        {
            try
            {
                this.Client.Connect(this.IPaddr, this.Port);
                this.stream = Client.GetStream();
                this.Reader = new StreamReader(stream);
                this.Writer = new StreamWriter(stream);
                this.IsConnection = true;
                this.ReceiveThread = new Thread(new ThreadStart(DataReceive));
                this.ReceiveThread.Start();
                if (Complete != null)
                {
                    Complete();
                }
                if (Logger != null)
                {
                    Logger.Debug("서버 접속");
                }
            }
            catch (Exception ex)
            {
                this.IsConnection = false;
                if (this.Client != null)
                {
                    this.Client.Dispose();
                    this.Client = null;
                }
                if (Logger != null)
                {
                    Logger.Error(ex);
                }
                throw ex;
            }
        }

        public void Send(string msg)
        {
            if (this.IsConnection && this.Writer != null)
            {
                Writer.WriteLine(msg); 
                Writer.Flush();
            }
            else
            {
                throw new Exception("연결되지 않았거나, 전송이 불가능합니다.");
            }
        }

        protected virtual void DataReceive()
        {
            while (this.IsConnection)
            {
                Thread.Sleep(1);

                try
                {
                    if (stream.CanRead)
                    {
                        string msg = Reader.ReadLine();
                        if (!string.IsNullOrWhiteSpace(msg) && this.Receive != null)
                        {
                            Receive(msg);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (Logger != null)
                    {
                        Logger.Error(ex);
                    }
                }
            }
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (this.Client != null)
                    {
                        if (this.Client.Connected)
                        {
                            this.Client.Close();
                        }
                        this.Client.Dispose();
                        this.Client = null;

                        this.Reader.Dispose();
                        this.Reader = null;

                        this.Writer.Dispose();
                        this.Writer = null;

                        this.ReceiveThread.Abort();
                        this.ReceiveThread = null;

                        this.Receive = null;
                    }

                    
                }

                disposedValue = true;
            }
        }

        ~SocketClient()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

    }
}
