using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sider;
using System.Collections.Concurrent;

namespace BarbeQ
{
    class RedisQueue : IQueue
    {
        private string m_name;
        private string m_connectionName;
        private string m_queuesKey;
        private string m_consumersKey;
        private string m_readKey;
        private string m_rejectedKey;
        private string m_unackedKey;
        private string m_pushKey;
        private bool m_isRunning;
        private int m_prefetchLimit;
        private TimeSpan m_pollDuration;
        private delegate void onDeliveryDelegate(IDelivery delivery);
        private event onDeliveryDelegate OnDelivery;
        private ConcurrentBag<IConsumer> m_consumers;

        private IRedisClient<string> m_redisClient;

        public RedisQueue(string name, string connectionName, string queuesKey, Sider.IRedisClient<string> redisClient)
        {
            m_name = name;
            m_connectionName = connectionName;
            m_queuesKey = queuesKey;
            m_redisClient = redisClient;

            m_consumersKey = ConstantKeys.connectionQueueConsumersTemplate.Replace(ConstantKeys.phConnection, connectionName).Replace(ConstantKeys.phQueue, name);
            m_readKey = ConstantKeys.queueReadyTemplate.Replace(ConstantKeys.phQueue, name);
            m_rejectedKey = ConstantKeys.queueRejectedTemplate.Replace(ConstantKeys.phQueue, name);
            m_unackedKey = ConstantKeys.connectionQueueUnackedTemplate.Replace(ConstantKeys.phConnection, connectionName).Replace(ConstantKeys.phQueue, name);

            m_consumers = new ConcurrentBag<IConsumer>();

            this.OnDelivery += RedisQueue_OnDelivery;

        }

        public string AddConsumer(string tag, IConsumer consumer)
        {
            var name = addCounsumer(tag);

            m_consumers.Add(consumer);

            return name;
        }

        private void RedisQueue_OnDelivery(IDelivery delivery)
        {
            foreach (var consumer in m_consumers)
            {
                consumer.Consume(delivery);
            }
        }

        private string addCounsumer(string tag)
        {
            if (!m_isRunning)
                throw new Exception(string.Format("bbq queue failed to add consumer, call StartConsuming first! {0}", this.ToString()));

            var name = string.Format("{0}-{1}", tag, Guid.NewGuid().ToString().Substring(0, 6));

            var redisClient = new RedisClient();
            redisClient.Select(3);
            redisClient.SAdd(m_consumersKey, name);

            return name;
        }

        public bool Publish(string payload)
        {
            try
            {
                m_redisClient.LPush(m_readKey, payload);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool StartConsuming(int prefetchLimit, TimeSpan pollDuration)
        {
            if (m_isRunning)
                return false; //already consuming

            m_redisClient.SAdd(m_queuesKey, m_name);

            m_prefetchLimit = prefetchLimit;
            m_pollDuration = pollDuration;
            m_isRunning = true;

            Task.Run(() =>
            {
                while (m_isRunning)
                {
                    if (!consume())
                        Task.Delay(m_pollDuration);
                }
            });

            return true;
        }

        bool consume()
        {
            string result;
            try
            {
                result = m_redisClient.RPopLPush(m_readKey, m_unackedKey);
                if (string.IsNullOrEmpty(result))
                    return false;
            }
            catch
            {
                return false;
            }

            if (OnDelivery != null)
            {
                var delivery = new Delivery(result, m_unackedKey, m_rejectedKey, m_pushKey, m_redisClient);
                OnDelivery(delivery);
                return true;
            }

            return true;
        }

        public bool StopConsuming()
        {
            if (!m_isRunning)
                return false; //not consuming

            m_isRunning = false;

            return true;
        }

        public override string ToString()
        {
            return string.Format("[{0} conn: {1}]", m_name, m_connectionName);
        }
    }
}
