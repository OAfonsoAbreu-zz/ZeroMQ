using System;
using System.Threading;
using NetMQ;
using NetMQ.Sockets;

namespace ServerZMQ
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                Console.WriteLine();
                Console.WriteLine("Usage: ./{0} HWServer [ServerZMQ]", AppDomain.CurrentDomain.FriendlyName);
                Console.WriteLine();
                Console.WriteLine("    Name   Afonso. Default: World");
                Console.WriteLine();
                args = new string[] { "World" };
            }

            string name = args[0];

            using (var server = new ResponseSocket())
            {
                server.Bind("tcp://*:5555");
                while (true)
                {
                    var msg = server.ReceiveFrameString();

                    if(msg.ToUpper() == "HELLO")
                    {
                        Console.WriteLine("Received: {0}", msg);
                        Thread.Sleep(1);
                        server.SendFrame(name);
                    }
                    else
                    {
                        Console.WriteLine("The message was not recognized");
                        server.SendFrame("-");
                    }
                        
                }
            }
        }
    }
}
