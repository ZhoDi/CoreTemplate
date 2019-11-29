using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// 邮件转发服务器
    /// 1、收集不能直接发邮件的客户端发来的邮件
    /// 2、推送或被拉取待发送的邮件，让可以发送邮件的客户端发送
    /// 3、被通知已发送的邮件，记录已发送列表
    /// 4、以上行为可以被HTTP或Socket实现
    /// 5、如果服务器本身就可以直接发送邮件，就没必要让客户端发了，因为部署环境确实不支持发邮件，这一步代码没有写，如果需要可以写在Add接口处，直接发送并放入已发送列表，且不推送
    /// </summary>
    public static class MailTransferServer
    {
        /// <summary>
        /// 验证秘钥
        /// </summary>
        private static string mToken { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        static MailTransferServer()
        {
            mToken = ConfigUtil.GetString("mail-transfer-token");
            if (string.IsNullOrWhiteSpace(mToken))
            {
                ConfigUtil.Set("mail-transfer-token", "");
                ConfigUtil.Set("mail-transfer-socket", "xxx.xxx.xxx.xxx:xxxx");
                LogUtil.Log(ConfigUtil.Exception);
                throw ConfigUtil.Exception;
            }
        }

        /// <summary>
        /// 检查Token
        /// </summary>
        public static bool Check(string token)
        {
            return token == mToken;
        }

        /// <summary>
        /// Socket服务器
        /// </summary>
        private static SocketServer mSocket { get; set; }

        /// <summary>
        /// 开启Socket
        /// </summary>
        public static void StartSocket()
        {
            mSocket = new SocketServer(ConfigUtil.GetString("mail-transfer-socket"), delegate (string msg, Socket subServer)
            {
                var request = JsonUtil.Deserialize<MailSocketMessage>(msg);
                if (request == null)
                    throw new Exception("MailTransferServer.Socket.收到了未知消息：" + msg);

                if (!Check(request.Token))
                    throw new Exception("MailTransferServer.Socket.收到了未知Token：" + request.Token);

                switch (request.Type)
                {
                    case MailSocketMessage.TypeEnum.Get:
                        if (WaittingList.Count == 0)
                            break;
                        MailSocketMessage response = new MailSocketMessage(mToken);
                        response.Type = MailSocketMessage.TypeEnum.List;
                        response.Mails = WaittingList.ToArray();
                        subServer.SendMsg(response.ToJson());
                        break;

                    case MailSocketMessage.TypeEnum.Finish:
                        Finish(request.Mails);
                        break;
                }
            });
            ThreadUtil.New(mSocket.Open).Start();
            mSocket.StartRebind();
        }

        /// <summary>
        /// 关闭Socket
        /// </summary>
        public static void StopSocket()
        {
            mSocket.Close();
        }

        /// <summary>
        /// 等待发送
        /// </summary>
        public static List<Mail> WaittingList { get; private set; } = CacheUtil.GetFromFile("MailTransferServer.WaittingMails", new List<Mail>());

        /// <summary>
        /// 存储等待发送
        /// </summary>
        private static void SaveWaitting()
        {
            CacheUtil.SaveWithFile("MailTransferServer.WaittingMails", WaittingList);
        }

        /// <summary>
        /// 添加到等待发送
        /// </summary>
        public static void Add(Mail mail)
        {
            if (MailSmtp.Enable)
            {
                mail.Send();
                SendedList.Add(mail);
                SaveSended();
                return;
            }

            WaittingList.Add(mail);
            SaveWaitting();
            MailSocketMessage request = new MailSocketMessage(mToken);
            request.Type = MailSocketMessage.TypeEnum.List;
            request.Mails = WaittingList.ToArray();
            mSocket.SendOne(request.ToJson());
        }

        /// <summary>
        /// 已发送
        /// </summary>
        public static List<Mail> SendedList { get; private set; } = CacheUtil.GetFromFile("MailTransferServer.SendedMails", new List<Mail>());

        /// <summary>
        /// 存储已发送
        /// </summary>
        private static void SaveSended()
        {
            CacheUtil.SaveWithFile("MailTransferServer.SendedMails", SendedList);
        }

        /// <summary>
        /// 完成发送
        /// </summary>
        public static void Finish(Mail[] mails)
        {
            foreach (var mail in mails)
            {
                var aimMail = WaittingList.FirstOrDefault(m => m.Id == mail.Id);
                if (aimMail != null)
                    WaittingList.Remove(aimMail);
            }
            SendedList.AddRange(mails);
            SaveWaitting();
            SaveSended();
        }
    }
}