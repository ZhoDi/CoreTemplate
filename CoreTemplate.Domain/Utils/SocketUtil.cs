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
    /// Socket相关函数
    /// </summary>
    public static class SocketUtil
    {
        /// <summary>
        /// 支持最大字节长度
        /// </summary>
        private static int mLengthLimit { get; set; } = 3 * 2000;

        /// <summary>
        /// 发送消息
        /// </summary>
        public static void SendMsg(this Socket socket, string msg)
        {
            var msgBuff = Encoding.UTF8.GetBytes(msg);
            if (msgBuff.Length >= mLengthLimit)
                throw (new Exception("Socket数据溢出"));
            Console.WriteLine("Socket发送消息：");
            Console.WriteLine(string.Format("{0} to {1}：", socket.LocalEndPoint, socket.RemoteEndPoint));
            Console.WriteLine(msg);
            socket.Send(msgBuff);
        }

        /// <summary>
        /// 接受消息
        /// </summary>
        public static string ReceiveMsg(this Socket socket)
        {
            var bytes = new byte[mLengthLimit];
            //这里收不到会挂起线程
            var count = socket.Receive(bytes);
            if (count == mLengthLimit)
                throw (new Exception("Socket数据溢出"));
            var msg = Encoding.UTF8.GetString(bytes, 0, count);
            Console.WriteLine("接收到消息：");
            Console.WriteLine(string.Format("{0} to {1}：", socket.RemoteEndPoint, socket.LocalEndPoint));
            Console.WriteLine(msg);
            return msg;
        }
    }
}