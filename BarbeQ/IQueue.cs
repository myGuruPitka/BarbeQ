using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarbeQ
{
    public interface IQueue
    {
        bool Publish(string payload);
        bool StartConsuming(int prefetchLimit, TimeSpan pollDuration);
        bool StopConsuming();
        string AddConsumer(string tag, IConsumer consumer);
    }
}
