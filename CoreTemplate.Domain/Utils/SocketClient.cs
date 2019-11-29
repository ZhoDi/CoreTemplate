using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CommonUtils
{
    /// <summary>
    /// Socket客户端
    /// </summary>
    public class SocketClient
    {
        /// <summary>
        /// 服务端
        /// </summary>
        private IPEndPoint mRemote { get; set; }

        /// <summary>
        /// 回调函数
        /// </summary>
        private Action<string, Socket> mCallback;

        /// <summary>
        /// 握手信息
        /// </summary>
        private string mGreet { get; set; }

        /// <summary>
        /// 前置函数 连接Socket之前要做的事情
        /// 如果是IIS发布server，只有访问页面才能触发app开启，其他模式不用前置，可直接访问
        /// </summary>
        private Action mBefore;

        /// <summary>
        /// 初始化
        /// </summary>
        public SocketClient(IPEndPoint remote, Action<string, Socket> callback = null, string greet = null, Action before = null)
        {
            //回调函数
            if (callback == null)
                callback = Console.WriteLine;

            Console.WriteLine("SocketClient参数初始化");
            mRemote = remote;
            mCallback = callback;
            mGreet = greet;
            mBefore = before;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public SocketClient(string remote, Action<string, Socket> callback = null, string greet = null, Action before = null)
        : this(IpUtil.EndPoint(remote), callback, greet, before) { }

        /// <summary>
        /// 初始化
        /// </summary>
        public SocketClient(string host, int port, Action<string, Socket> callback = null, string greet = null, Action before = null)
        : this(IpUtil.EndPoint(host, port), callback, greet, before) { }

        /// <summary>
        /// 初始化
        /// </summary>
        public SocketClient(int port, Action<string, Socket> callback = null, string greet = null, Action before = null)
      : this("127.0.0.1", port, callback, greet, before) { }

        /// <summary>
        /// 连接
        /// </summary>
        private Socket mClient { get; set; }

        /// <summary>
        /// 监听
        /// </summary>
        private Thread mThread { get; set; }

        /// <summary>
        /// 开启
        /// </summary>
        public void Open()
        {
            try
            {
                //前置工作
                mBefore?.Invoke();

                LogUtil.Log("SocketClient.Open");
                //创建Socket
                mClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Console.WriteLine("SocketClient连接到服务器:" + mRemote);
                mClient.Connect(mRemote);

                //监听服务器消息
                mThread = new Thread(delegate ()
                {
                    try
                    {
                        while (true)
                        {
                            var msg = mClient.ReceiveMsg();
                            try
                            {
                                mCallback(msg, mClient);
                            }
                            catch (Exception ex)
                            {
                                //此处try catch不能与外部合并，以免造成重连循环
                                LogUtil.Error("SocketClient.Callback.Exception", ex);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Log("SocketClient断开：" + ex.Message);
                    }
                });
                LogUtil.Log("SocketClient开启监听");
                mThread.Start();

                //握手
                if (string.IsNullOrWhiteSpace(mGreet))
                    return;
                Console.WriteLine("SocketClient发送握手消息");
                mClient.SendMsg(mGreet);
            }
            catch (Exception ex)
            {
                LogUtil.Error("SocketClient.Open异常", ex);
            }
        }

        /// <summary>
        /// 断线重连
        /// </summary>
        private System.Timers.Timer mTimer { get; set; }

        /// <summary>
        /// 开启断线重连，开启周期握手？
        /// </summary>
        public void StartReconnect(int second = 300, bool greet = true)
        {
            if (mTimer != null)
                mTimer.Close();

            mTimer = ThreadUtil.TimerDelay(delegate ()
            {
                if (mThread == null || !mThread.IsAlive)
                {
                    LogUtil.Log("SocketClient.Reconnect");
                    Open();
                }
                else
                {
                    if (!greet || string.IsNullOrWhiteSpace(mGreet))
                        return;
                    Console.WriteLine("SocketClient发送握手消息");
                    Send(mGreet);
                }
            }, second);
            mTimer.Start();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            try
            {
                LogUtil.Log("SocketClient.Close");
                Console.WriteLine("SocketClient关闭断线重连");
                if (mTimer != null)
                    mTimer.Close();

                Console.WriteLine("SocketClient关闭监听");
                if (mThread != null)
                    mThread.Interrupt();

                Console.WriteLine("SocketClient断开连接");
                if (mClient != null)
                    mClient.Close();
            }
            catch (Exception ex)
            {
                LogUtil.Error("SocketClient.Close异常", ex);
            }
        }

        /// <summary>
        /// 打印状态
        /// </summary>
        public void State()
        {
            try
            {
                Console.WriteLine("SocketClient.OpenReconnect:" + (mTimer != null));
                Console.WriteLine("SocketClient.Connected:" + (mClient != null && mClient.Connected));
                Console.WriteLine("SocketClient.Listener.IsAlive:" + (mThread != null && mThread.IsAlive));
            }
            catch (Exception ex)
            {
                LogUtil.Error("SocketClient.State异常", ex);
            }
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        public void Send(string msg)
        {
            mClient.SendMsg(msg);
        }
    }
}