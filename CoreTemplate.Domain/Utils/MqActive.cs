using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using Newtonsoft.Json.Linq;
using System;

namespace CommonUtils
{
    /// <summary>
    /// ActiveMQ
    /// TODO:断线重连
    /// </summary>
    public class MqActive
    {
        /// <summary>
        /// 连接
        /// </summary>
        private IConnection mConnection;

        /// <summary>
        /// 会话
        /// </summary>
        private ISession mSession;

        /// <summary>
        /// 初始化 ActiveMQ好像不需要账号密码，传与不传一样，RabbitMQ要的！
        /// </summary>
        public MqActive(string url, string user = null, string pwd = null)
        {
            var client = ReflectionUtil.AppName + "@" + DateTime.Now.Stamp();

            //构建MQ工厂
            var factory = new ConnectionFactory(url);
            factory.OnException += Factory_OnException;

            //ActiveMQ不需要账号密码，传与不传一样，RabbitMQ要的！
            if (!string.IsNullOrEmpty(user))
                factory.UserName = user;
            if (!string.IsNullOrEmpty(pwd))
                factory.Password = pwd;

            //通过工厂构建连接
            Console.WriteLine(string.Format("ActiveMQ is trying to connect to {0} with client name {1}.", url, client));
            mConnection = factory.CreateConnection();
            Console.WriteLine(string.Format("ActiveMQ has connectted to {0},waitting for adding listener...", url));
            mConnection.ClientId = client;
            mConnection.ConnectionInterruptedListener += MConnection_ConnectionInterruptedListener;
            mConnection.ConnectionResumedListener += MConnection_ConnectionResumedListener;
            mConnection.ExceptionListener += MConnection_ExceptionListener;

            //通过连接创建一个会话
            mSession = mConnection.CreateSession();
        }

        /// <summary>
        /// 添加监听
        /// </summary>
        public void AddListener(string topic, Action<string> callback = null)
        {
            if (callback == null)
                callback = Console.WriteLine;

            //通过会话创建一个客户，这里就是Queue这种会话类型的监听参数设置
            var consumer = mSession.CreateConsumer(new ActiveMQTopic(topic));
            consumer.Listener += delegate (IMessage message)
            {
                string msg = ((ITextMessage)message).Text;
                callback(msg);
            };
            Console.WriteLine(string.Format("ActiveMQ has listened {0},waitting for start.", topic));
        }

        private bool mStart = false;

        /// <summary>
        /// 启动监听
        /// </summary>
        public void Start()
        {
            //启动连接，监听的话要主动启动连接
            mConnection.Start();
            mStart = true;
            Console.WriteLine("ActiveMQ has started.");
        }

        /// <summary>
        /// 状态
        /// </summary>
        public string State
        {
            get
            {
                JObject state = new JObject();
                state.Add("ActiveMQ.IsStarted", mStart);
                state.Add("ActiveMQ.Connection.IsStarted", mConnection.IsStarted);
                state.Add("ActiveMQ.Session.Transacted", mSession.Transacted);
                return state.ToString();
            }
        }

        #region 异常处理

        private void Factory_OnException(Exception exception)
        {
            throw exception;
        }

        private void MConnection_ExceptionListener(Exception exception)
        {
            throw exception;
        }

        private void MConnection_ConnectionResumedListener()
        {
            throw new Exception("ConnectionResumed");
        }

        private void MConnection_ConnectionInterruptedListener()
        {
            throw new Exception("ConnectionInterrupted");
        }

        #endregion
    }
}
