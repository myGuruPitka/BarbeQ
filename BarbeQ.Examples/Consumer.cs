using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BarbeQ.Examples
{
    class Consumer : IConsumer
    {
        private DateTime m_before;
        private int m_count;
        private string m_name;

        public Consumer(int tag)
        {
            m_name = string.Format("consumer", tag);
            m_count = 0;
            m_before = DateTime.Now;
        }

        public void Consume(IDelivery delivery)
        {
            m_count++;
            var duration = DateTime.Now.Subtract(m_before).TotalMilliseconds;
            m_before = DateTime.Now;
            Console.WriteLine("{0} consumed {1} {2}", m_name, m_count, delivery.Payload);

            Thread.Sleep(TimeSpan.FromMilliseconds(1));

            delivery.Ack();
        }
    }
}
