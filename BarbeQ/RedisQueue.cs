using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sider;

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
        private bool m_consumingStopped;
        private int m_prefetchLimit;
        private TimeSpan m_pollDuration;

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
        }

        public string AddConsumer(string tag, IConsumer consumer)
        {
            throw new NotImplementedException();
        }

        public bool Publish(string payload)
        {
            throw new NotImplementedException();
        }

        public bool StartConsuming(int prefetchLimit, TimeSpan pollDuration)
        {
            throw new NotImplementedException();
        }

        public bool StopConsuming()
        {
            throw new NotImplementedException();
        }
    }
}
