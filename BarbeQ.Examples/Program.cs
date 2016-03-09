using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarbeQ.Examples
{
    class Program
    {
        const int unackedLimit = 1000;
        const int numConsumers = 10;

        static void Main(string[] args)
        {
            var options = new CommandLineOptions();

            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                if (options.Produce)
                {
                    const int numDeliveries = 1000;
                    const int batchSize = 100;

                    var connection = RedisConnection.OpenConnection("producer-connection", "localhost", 6379, 3);
                    var things = connection.OpenQueue("things");
                    var balls = connection.OpenQueue("balls");

                    var before = DateTime.Now;

                    for (int i = 0; i < numDeliveries; i++)
                    {
                        var delivery = string.Format("delivery {0}", i);
                        things.Publish(delivery);

                        if (i % batchSize == 0)
                        {
                            var duration = DateTime.Now.Subtract(before).TotalMilliseconds;
                            before = DateTime.Now;
                            var perSecond = TimeSpan.FromSeconds(1).TotalMilliseconds / (duration / batchSize);
                            Console.WriteLine(string.Format("produced {0} {1} {2}", i, delivery, perSecond));
                            balls.Publish("ball");
                        }
                    }

                }

                if (options.Consume)
                {
                    var connection = RedisConnection.OpenConnection("consumer-connection", "localhost", 6379, 3);
                    var queue = connection.OpenQueue("things");
                    queue.StartConsuming(unackedLimit, TimeSpan.FromMilliseconds(500));

                    for (int i = 0; i < numConsumers; i++)
                    {
                        var name = "consumer" + i;
                        queue.AddConsumer(name, new Consumer(i));
                    }
                }

            }

            Console.Write("Enter to exit...");
            Console.Read();

        }
    }
}
