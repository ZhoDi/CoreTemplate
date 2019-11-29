using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// 邮件发送服务器
    /// </summary>
    public static class MailSmtp
    {
        /// <summary>
        /// 账号
        /// </summary>
        private static string mAccount { get; set; }

        /// <summary>
        /// smtp客户端
        /// </summary>
        private static SmtpClient mSmtpClient { get; set; }

        /// <summary>
        /// Smtp是否有效
        /// </summary>
        public static bool Enable { get; private set; }

        /// <summary>
        /// 初始化
        /// </summary>
        static MailSmtp()
        {
            var host = ConfigUtil.GetString("mail-smtp-host");
            var port = ConfigUtil.GetValue("mail-smtp-port", 25);
            var account = ConfigUtil.GetString("mail-smtp-account");
            var password = ConfigUtil.GetString("mail-smtp-password");
            if (StringUtil.IsNullOrWhiteSpace(host, account, password))
            {
                ConfigUtil.Set("mail-smtp-host", "smtp.xx.xx");
                ConfigUtil.Set("mail-smtp-port", 25);
                ConfigUtil.Set("mail-smtp-account", "xx@xx.xx");
                ConfigUtil.Set("mail-smtp-password", "");
                LogUtil.Log(ConfigUtil.Exception);
                throw ConfigUtil.Exception;
            }
            mAccount = account;
            mSmtpClient = new SmtpClient(host, port);
            mSmtpClient.Credentials = new NetworkCredential(mAccount, password);

            var mail = new Mail("test@test.test");
            Send(mail);
            LogUtil.Log("MailSmtp.Test:" + mail.ResultInfo);
            Enable = mail.SendResult;
        }

        /// <summary>
        /// 发送
        /// </summary>
        public static void Send(this Mail mail)
        {
            try
            {
                mail.ModifyTime = DateTime.Now;
                mail.SenderAddress = mAccount;

                //定义正文
                MailMessage message = new MailMessage();
                //发件人
                message.From = new MailAddress(mail.SenderAddress, mail.SenderName, Encoding.UTF8);
                //收件人
                if (!string.IsNullOrEmpty(mail.ReceiverAddress))
                    message.To.Add(new MailAddress(mail.ReceiverAddress, mail.ReceiverName, Encoding.UTF8));
                foreach (var receiver in mail.Receivers)
                    message.To.Add(new MailAddress(receiver.Key, receiver.Value, Encoding.UTF8));
                //主题
                message.Subject = mail.Title;
                //正文
                message.Body = mail.Content;
                //html支持
                message.IsBodyHtml = true;
                //编码
                message.BodyEncoding = message.HeadersEncoding = message.SubjectEncoding = Encoding.UTF8;
                //发送
                mSmtpClient.Send(message);

                mail.SendResult = true;
                mail.ResultInfo = "发送成功";
            }
            catch (Exception ex)
            {
                mail.SendResult = false;
                mail.ResultInfo = "发送失败：" + ex.Message;
            }
        }

        /// <summary>
        /// 发送/转发
        /// </summary>
        public static void TrySend(this Mail mail)
        {
            if (Enable)
                mail.Send();
            else
                mail.Transfer();
        }
    }
}
