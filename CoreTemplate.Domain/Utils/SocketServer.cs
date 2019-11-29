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
    /// Socket服务器
    /// </summary>
    public class SocketServer
    {
        /// <summary>
        /// 绑定终端
        /// </summary>
        private IPEndPoint mBind { get; set; }

        /// <summary>
        /// 回调
        /// </summary>
        private Action<string, Socket> mCallback { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public SocketServer(IPEndPoint bind, Action<string, Socket> callback = null)
        {
            //回调函数
            if (callback == null)
                callback = Console.WriteLine;

            Console.WriteLine("SocketServer参数初始化");
            mBind = bind;
            mCallback = callback;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public SocketServer(string remote, Action<string, Socket> callback = null)
        : this(IpUtil.EndPoint(remote), callback)
        {
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public SocketServer(string host, int port, Action<string, Socket> callback = null)
        : this(IpUtil.EndPoint(host, port), callback)
        {
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public SocketServer(int port, Action<string, Socket> callback = null)
        : this("127.0.0.1", port, callback)
        {
        }

        /// <summary>
        /// 主连接
        /// </summary>
        private Socket mMainServer { get; set; }

        /// <summary>
        /// 主监听
        /// </summary>
        private Thread mMainThread { get; set; }

        /// <summary>
        /// 分支
        /// </summary>
        private MapKeyValue<Socket, Thread> mMapSubServerThread { get; set; } = new MapKeyValue<Socket, Thread>();

        /// <summary>
        /// 开启
        /// </summary>
        public void Open()
        {
            try
            {
                LogUtil.Log("SocketServer.Open");
                //创建一个Socket对象,如果用UDP协议,则要用SocketTyype.Dgram类型的套接字
                mMainServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Console.WriteLine("SocketServer绑定终端：" + mBind);
                mMainServer.Bind(mBind);

                //设置挂起连接队列的最大长度,不可省略
                mMainServer.Listen(50000);

                //监听连接
                mMainThread = new Thread(delegate ()
                {
                    try
                    {
                        while (true)
                        {
                            //接受到Client连接,为此连接建立新的Socket,并接受消息
                            var subServer = mMainServer.Accept();
                            Console.WriteLine("新终端接入：" + subServer.RemoteEndPoint);
                            //开启监听新终端
                            Console.WriteLine("开启监听新终端");
                            var subThread = new Thread(delegate ()
                              {
                                  try
                                  {
                                      while (true)
                                      {
                                          var msg = subServer.ReceiveMsg();
                                          try
                                          {
                                              mCallback(msg, subServer);
                                          }
                                          catch (Exception ex)
                                          {
                                              //此处try catch不能与外部合并，以免造subServer不接收消息
                                              LogUtil.Error("SocketSubServer.Callback.Exception", ex);
                                          }
                                      }
                                  }
                                  catch (Exception ex)
                                  {
                                      LogUtil.Log("SocketSubServer断开：" + ex.Message);
                                  }
                              });
                            subThread.Start();
                            //添加到连接表
                            mMapSubServerThread.Add(subServer, subThread);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error("SocketServer.Listener.Exception", ex);
                    }
                });
                LogUtil.Log("SocketServer开启监听");
                mMainThread.Start();
            }
            catch (Exception ex)
            {
                LogUtil.Error("SocketServer.Open异常", ex);
            }
        }

        /// <summary>
        /// 异常重启
        /// </summary>
        private System.Timers.Timer mTimer { get; set; }

        /// <summary>
        /// 开启异常重启
        /// </summary>
        public void StartRebind(int second = 300)
        {
            if (mTimer != null)
                mTimer.Close();

            mTimer = ThreadUtil.TimerDelay(delegate ()
            {
                if (mMainThread == null || !mMainThread.IsAlive)
                {
                    LogUtil.Log("SocketServer.Rebind");
                    Open();
                }
            }, second);
            mTimer.Start();
        }

        /// <summary>
        /// 关闭分支
        /// </summary>
        private void Close(Socket subServer)
        {
            try
            {
                var subThread = mMapSubServerThread.Get(subServer);

                Console.WriteLine("SocketSubServer关闭监听");
                if (subThread != null)
                    subThread.Interrupt();

                Console.WriteLine("SocketSubServer断开链接");
                if (subServer != null)
                    subServer.Close();

                mMapSubServerThread.Remove(subServer);
            }
            catch (Exception ex)
            {
                LogUtil.Error("SocketSubServer.Close异常", ex);
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            try
            {
                LogUtil.Log("SocketServer.Close");
                Console.WriteLine("SocketServer关闭异常重启");
                if (mTimer != null)
                    mTimer.Close();

                Console.WriteLine("SocketServer关闭监听");
                if (mMainThread != null)
                    mMainThread.Interrupt();

                //关闭分支
                foreach (var subServer in mMapSubServerThread.Keys.ToArray())
                    Close(subServer);

                Console.WriteLine("SocketServer断开绑定");
                if (mMainServer != null)
                    mMainServer.Close();
            }
            catch (Exception ex)
            {
                LogUtil.Error("SocketServer.Close异常", ex);
            }
        }

        /// <summary>
        /// 打印状态
        /// </summary>
        public void State()
        {
            try
            {
                Console.WriteLine("SocketServer.OpenRebind:" + (mTimer != null));
                Console.WriteLine("SocketServer.IsBound:" + (mMainServer != null && mMainServer.IsBound));
                Console.WriteLine("SocketServer.Listener.IsAlive:" + (mMainThread != null && mMainThread.IsAlive));
            }
            catch (Exception ex)
            {
                LogUtil.Error("SocketServer.State异常", ex);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        public void Send(Socket subServer, string msg)
        {
            try
            {
                subServer.SendMsg(msg);
            }
            catch
            {
                Close(subServer);
            }
        }

        /// <summary>
        /// 发送一条消息
        /// </summary>
        public void SendOne(string msg)
        {
            foreach (var subServer in mMapSubServerThread.Keys.ToArray())
            {
                try
                {
                    subServer.SendMsg(msg);
                    break;
                }
                catch
                {
                    Close(subServer);
                }
            }
        }

        /// <summary>
        /// 广播消息
        /// </summary>
        public void SendAll(string msg)
        {
            foreach (var subServer in mMapSubServerThread.Keys.ToArray())
            {
                try
                {
                    subServer.SendMsg(msg);
                }
                catch
                {
                    Close(subServer);
                }
            }
        }
    }
}