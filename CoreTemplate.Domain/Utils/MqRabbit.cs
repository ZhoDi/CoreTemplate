using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;

namespace CommonUtils
{
    /// <summary>
    /// RabbitMQ
    /// TODO:断线重连
    /// </summary>
    public class MqRabbit
    {
        /// <summary>
        /// 连接
        /// </summary>
        private IConnection mConnection;

        /// <summary>
        /// 会话
        /// </summary>
        private IModel mSession;

        /// <summary>
        /// 初始化 api中的/不能省略
        /// </summary>
        public MqRabbit(string host, int port = 0, string user = null, string pwd = null, string api = null)
        {
            var client = ReflectionUtil.AppName + "@" + DateTime.Now.Stamp();

            //构建MQ工厂
            var factory = new ConnectionFactory();
            factory.HostName = host;
            if (port > 0)
                factory.Port = port;
            if (!string.IsNullOrEmpty(user))
                factory.UserName = user;
            if (!string.IsNullOrEmpty(pwd))
                factory.Password = pwd;
            if (!string.IsNullOrEmpty(api))
                factory.VirtualHost = api;

            //通过工厂构建连接
            Console.WriteLine(string.Format("RabbitMQ is trying to connect to {0} whith client name {1}.", factory.Endpoint, client));
            mConnection = factory.CreateConnection(client);
            Console.WriteLine(string.Format("RabbitMQ has connectted to {0},waitting for adding listener...", factory.Endpoint));

            //通过连接创建一个会话
            mSession = mConnection.CreateModel();
        }

        /// <summary>
        /// 队列名-消费者
        /// </summary>
        private Dictionary<string, EventingBasicConsumer> mQueueNameConsumers = new Dictionary<string, EventingBasicConsumer>();

        /// <summary>
        /// 添加监听
        /// </summary>
        public void AddListener(string queueName, Action<string> callback = null)
        {
            if (callback == null)
                callback = Console.WriteLine;

            var consumer = new EventingBasicConsumer(mSession);
            consumer.Received += delegate (object sender, BasicDeliverEventArgs e)
            {
                callback(e.Body.ToText());
                mSession.BasicAck(e.DeliveryTag, false);
            };
            mQueueNameConsumers.Set(queueName, consumer);
            Console.WriteLine(string.Format("RabbitMQ has listened {0},waitting for start.", queueName));
        }

        private bool mStart = false;

        /// <summary>
        /// 启动监听
        /// </summary>
        public void Start()
        {
            foreach (var queueNameConsumer in mQueueNameConsumers)
                mSession.BasicConsume(queueNameConsumer.Key, false, queueNameConsumer.Value);
            mStart = true;
            Console.WriteLine("RabbitMQ has started.");
        }

        /// <summary>
        /// 状态
        /// </summary>
        public string State
        {
            get
            {
                JObject state = new JObject();
                state.Add("RabbitMQ.IsStarted", mStart);
                state.Add("RabbitMQ.Connection.IsOpened", mConnection.IsOpen);
                state.Add("RabbitMQ.Session.IsOpened", mSession.IsOpen);
                return state.ToString();
            }
        }
    }
}
