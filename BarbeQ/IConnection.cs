using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarbeQ
{
    public interface IConnection
    {
        IQueue OpenQueue(string name);
        IEnumerable<string> GetOpenQueues();
    }
}
