using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sider;

namespace BarbeQ
{
    public class RedisConnection : IConnection
    {
        private TimeSpan heartbeatDuration = TimeSpan.FromMinutes(1);
        private string m_heartbeatKey;
        private string m_queuesKey;
        private bool m_heartbeatStopped;
        Sider.IRedisClient<string> m_redisClient;

        public RedisConnection(string name, string heartbeatKey, string queuesKey, RedisClient<string> redisClient)
        {
            Name = name;
            m_heartbeatKey = heartbeatKey;
            m_queuesKey = queuesKey;
            m_redisClient = redisClient;
        }

        public string Name { get; set; }

        public IEnumerable<string> GetOpenQueues()
        {
            return m_redisClient.SMembers(m_queuesKey);
        }

        public IQueue OpenQueue(string name)
        {
            if (!m_redisClient.SAdd(m_queuesKey, name))
                throw new Exception(string.Format("bbq could not open queue {0}", name));

            return new RedisQueue(name, Name, m_queuesKey, m_redisClient);
        }

        public static IConnection OpenConnection(string tag, string host, int port, int dbIndex)
        {
            var redisClient = new Sider.RedisClient<string>(host, port);
            redisClient.Select(dbIndex);

            return OpenConnectionWithRedisClient(tag, redisClient);
        }

        private static IConnection OpenConnectionWithRedisClient(string tag, RedisClient<string> redisClient)
        {
            var name = string.Format("{0}-{1}", tag, Guid.NewGuid().ToString().Substring(0, 6));
            var heartbeatKey = ConstantKeys.connectionHeartbeatTemplate.Replace(ConstantKeys.phConnection, name);
            var queuesKey = ConstantKeys.connectionQueuesTemplate.Replace(ConstantKeys.phConnection, name);

            return new RedisConnection(name, heartbeatKey, queuesKey, redisClient);
        }
    }
}
