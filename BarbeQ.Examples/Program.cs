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

            if(CommandLine.Parser.Default.ParseArguments(args, options))
            {
                if (options.Produce) {

                    var connection = RedisConnection.OpenConnection("producer", "localhost", 6379, 3);
                    var things = connection.OpenQueue("things");

                    for (int i = 0; i < 10; i++)
                    {
                        var payload = string.Format("delivery {0}", i);
                        things.Publish(payload);
                    }

                }

                if (options.Consume)
                {
                    var connection = RedisConnection.OpenConnection("consumer", "localhost", 6379, 3);
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
