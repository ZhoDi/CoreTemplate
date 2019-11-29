using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CommonUtils
{
    /// <summary>
    /// 邮件中转，突破阿里云的25端口限制
    /// 1、假设服务器不能发送邮件
    /// 2、把邮件上传到服务器存着
    /// 3、肉机从服务器上获取邮件帮忙发送
    /// 4、服务器删除已发送的邮件
    /// 5、此处客户端有三个
    /// 5.1、通过HTTP上传邮件到服务器，用在部署环境无法直接发送邮件的情况
    /// 5.2、通过HTTP获取需要发送的邮件并转发
    /// 5.3、通过Socket获取需要发送的邮件并转发
    /// 6、如果服务器不需要token，那么随便写一个值，通过空判断即可
    /// </summary>
    public static class MailTransferClient
    {
        /// <summary>
        /// 通讯秘钥
        /// </summary>
        private static string mToken { get; set; }

        /// <summary>
        /// 含Token的HTTP头部
        /// </summary>
        private static MapKeyString mHeader { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        static MailTransferClient()
        {
            mToken = ConfigUtil.GetString("mail-transfer-token");
            if (string.IsNullOrWhiteSpace(mToken))
            {
                ConfigUtil.Set("mail-transfer-token", "");
                ConfigUtil.Set("mail-transfer-test", "http://xxx.xxx.xxx.xxx:xxxx/api/test");
                ConfigUtil.Set("mail-transfer-socket", "xxx.xxx.xxx.xxx:xxxx");
                ConfigUtil.Set("mail-transfer-add", "http://xxx.xxx.xxx.xxx:xxxx/api/add");
                ConfigUtil.Set("mail-transfer-waitting", "http://xxx.xxx.xxx.xxx:xxxx/api/waitting");
                ConfigUtil.Set("mail-transfer-finish", "http://xxx.xxx.xxx.xxx:xxxx/api/finish");
                ConfigUtil.Set("mail-transfer-sended", "http://xxx.xxx.xxx.xxx:xxxx/api/sended");
                LogUtil.Log(ConfigUtil.Exception);
                throw ConfigUtil.Exception;
            }
            mHeader = new MapKeyString("token", mToken);
        }

        /// <summary>
        /// API：服务器连接测试
        /// </summary>
        private static string mApiTest { get; set; } = ConfigUtil.GetString("mail-transfer-test");

        /// <summary>
        /// 服务器连接测试
        /// </summary>
        public static bool ServerTest()
        {
            return HttpUtil.GetString(mApiTest, mHeader) == "true";
        }

        /// <summary>
        /// 通讯器
        /// </summary>
        private static SocketClient mSocket;

        /// <summary>
        /// 开启Socket
        /// </summary>
        public static void StartSocket()
        {
            //初始化参数
            mSocket = new SocketClient(ConfigUtil.GetString("mail-transfer-socket"), delegate (string msg, Socket client)
            {
                var request = JsonUtil.Deserialize<MailSocketMessage>(msg);
                if (request == null)
                    throw new Exception("MailTransferClient.Socket.收到了未知消息：" + msg);
                //判断通讯类型
                switch (request.Type)
                {
                    case MailSocketMessage.TypeEnum.List:
                        foreach (var mail in request.Mails)
                            mail.Send();
                        var response = new MailSocketMessage(mToken)
                        {
                            Type = MailSocketMessage.TypeEnum.Finish,
                            Mails = request.Mails
                        };
                        client.SendMsg(response.ToJson());
                        break;
                }
            }, new MailSocketMessage(mToken)
            {
                Type = MailSocketMessage.TypeEnum.Get
            }.ToJson(), delegate ()
            {
                ServerTest();
            });

            ThreadUtil.New(mSocket.Open).Start();
            mSocket.StartReconnect();
        }

        /// <summary>
        /// 关闭Socket
        /// </summary>
        public static void StopSocket()
        {
            mSocket.Close();
        }

        /// <summary>
        /// API：上传邮件
        /// </summary>
        private static string mApiAdd { get; set; } = ConfigUtil.GetString("mail-transfer-add");

        /// <summary>
        /// 邮件中转上传
        /// </summary>
        public static void Transfer(this Mail mail)
        {
            mail.ModifyTime = DateTime.Now;
            mail.ResultInfo = "中转发送：";
            mail.ResultInfo += HttpUtil.PostJson(mApiAdd, mail.ToJson(), Encoding.UTF8, mHeader);
        }

        /// <summary>
        /// API：等待发送的邮件
        /// </summary>
        private static string mApiWaitting { get; set; } = ConfigUtil.GetString("mail-transfer-waitting");


        /// <summary>
        /// 查询等待发送的邮件
        /// </summary>
        public static Mail[] GetWaitting()
        {
            return JsonUtil.Deserialize<Mail[]>(HttpUtil.GetString(mApiWaitting, mHeader));
        }

        /// <summary>
        /// API：标记为已发送
        /// </summary>
        private static string mApiFinish { get; set; } = ConfigUtil.GetString("mail-transfer-finish");

        /// <summary>
        /// 标记邮件已发送
        /// </summary>
        public static void MarkSended(Mail[] mails)
        {
            HttpUtil.PostJson(mApiFinish, mails.ToJson(), Encoding.UTF8, mHeader);
        }

        /// <summary>
        /// 发送等待发送的邮件
        /// </summary>
        public static void SendWaitting()
        {
            var mails = GetWaitting();
            if (mails == null || mails.Length == 0)
                return;
            foreach (var mail in mails)
                mail.Send();
            MarkSended(mails);
        }

        /// <summary>
        /// 用http循环处理
        /// </summary>
        public static void StartHttpLoop()
        {
            new Thread(delegate ()
            {
                while (true)
                {
                    LogUtil.Log("MailTransferClient.HttpLoop Start");
                    SendWaitting();
                    Thread.Sleep(5 * 60 * 1000);
                }
            }).Start();
        }

        /// <summary>
        /// API：已发送的邮件
        /// </summary>
        private static string mApiSended { get; set; } = ConfigUtil.GetString("mail-transfer-sended");

        /// <summary>
        /// 查询已经发送的邮件
        /// </summary>
        public static Mail[] SendedList()
        {
            return JsonUtil.Deserialize<Mail[]>(HttpUtil.GetString(mApiSended, mHeader));
        }
    }
}
