using System;
using NetMQ;
using NetMQ.Sockets;

namespace SubscriberZMQ
{
    class Program
    {
        static void Main(string[] args)
        {
			if (args == null || args.Length < 2)
			{
				Console.WriteLine();
				Console.WriteLine("Usage: ./{0} WUClient [ZipCode] [Endpoint]", AppDomain.CurrentDomain.FriendlyName);
				Console.WriteLine();
				Console.WriteLine("    ZipCode   The zip code to subscribe. Default is 72622 Nürtingen");
				Console.WriteLine("    Endpoint  Where WUClient should connect to.");
				Console.WriteLine("              Default is tcp://127.0.0.1:5556");
				Console.WriteLine();
				if (args.Length < 1)
					args = new string[] { "72622", "tcp://127.0.0.1:5555" };
				else
					args = new string[] { args[0], "tcp://127.0.0.1:5555" };
			}

			string endpoint = args[1];

			using (var subscriber = new SubscriberSocket())
            {
				Console.WriteLine("I: Connecting to {0}...", endpoint);
				subscriber.Connect(endpoint);

				// Subscribe to zipcode
				string zipCode = args[0];
				Console.WriteLine("I: Subscribing to zip code {0}...", zipCode);
				subscriber.Subscribe(zipCode);

				// Process 10 updates
				int i = 0;
				long total_temperature = 0;
				for (; i < 20; ++i)
				{

					string reply = subscriber.ReceiveFrameString();

					Console.WriteLine(reply);
					total_temperature += Convert.ToInt64(reply.Split(' ')[1]);
					
				}
				Console.WriteLine("Average temperature for zipcode '{0}' was {1}°C", zipCode, (total_temperature / i));

			}
        }
    }
}
