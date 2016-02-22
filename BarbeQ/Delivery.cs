using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarbeQ
{
    public class Delivery : IDelivery
    {
        private string m_payload;
        private string m_unackedKey;
        private string m_rejectedKey;
        private string m_pushKey;
        private Sider.IRedisClient<string> m_redisClient;

        public Delivery(string payload, string unackedKey, string rejectedKey, string pushKey, Sider.IRedisClient<string> redisClient)
        {
            m_payload = payload;
            m_unackedKey = unackedKey;
            m_rejectedKey = rejectedKey;
            m_pushKey = pushKey;
            m_redisClient = redisClient;
        }

        public string Payload
        {
            get
            {
                return m_payload;
            }
        }


        public bool Ack()
        {
            var result = m_redisClient.LRem(m_unackedKey, 1, m_payload);

            return result == 1;
        }

        public bool Reject()
        {
            return move(m_rejectedKey);
        }

        public bool Push()
        {
            if (!string.IsNullOrEmpty(m_pushKey))
                return move(m_pushKey);

            return move(m_rejectedKey);
        }

        private bool move(string key)
        {
            try
            {
                m_redisClient.LPush(key, m_payload);
                m_redisClient.LRem(m_unackedKey, 1, m_payload);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            return string.Format("[{0} {1}]", m_payload, m_unackedKey);
        }
    }
}
