using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarbeQ
{
    class ConstantKeys
    {
        /// <summary>
        /// Set of connection names
        /// </summary>
        public const string connectionsKey = "bbq::connections";
        /// <summary>
        /// expires after {connection} died
        /// </summary>
        public const string connectionHeartbeatTemplate = "bbq::connection::{connection}::heartbeat";
        /// <summary>
        /// Set of queues consumers of {connection} are consuming
        /// </summary>
        public const string connectionQueuesTemplate = "bbq::connection::{connection}::queues";
        /// <summary>
        /// Set of all consumers from {connection} consuming from {queue}
        /// </summary>
        public const string connectionQueueConsumersTemplate = "bbq::connection::{connection}::queue::[{queue}]::consumers";
        /// <summary>
        /// List of deliveries consumers of {connection} are currently consuming
        /// </summary>
        public const string connectionQueueUnackedTemplate = "bbq::connection::{connection}::queue::[{queue}]::unacked";

        /// <summary>
        /// Set of all open queues
        /// </summary>
        public const string queuesKey = "bbq::queues";
        /// <summary>
        /// List of deliveries in that {queue} (right is first and oldest, left is last and youngest)
        /// </summary>
        public const string queueReadyTemplate = "bbq::queue::[{queue}]::ready";
        /// <summary>
        /// List of rejected deliveries from that {queue}
        /// </summary>
        public const string queueRejectedTemplate = "bbq::queue::[{queue}]::rejected";

        /// <summary>
        /// connection name
        /// </summary>
        public const string phConnection = "{connection}";
        /// <summary>
        /// queue name
        /// </summary>
        public const string phQueue = "{queue}";
        /// <summary>
        /// consumer name (consisting of tag and token)
        /// </summary>
        public const string phConsumer = "{consumer}"; 
    }
}
