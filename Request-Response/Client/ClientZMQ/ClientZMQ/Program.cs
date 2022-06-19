using System;
using NetMQ.Sockets;
using NetMQ;

namespace ClientZMQ
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                Console.WriteLine();
                Console.WriteLine("Usage: ./{0} HWClient [Endpoint]", AppDomain.CurrentDomain.FriendlyName);
                Console.WriteLine();
                Console.WriteLine("    Endpoint  Where HWClient should connect to.");
                Console.WriteLine("              Default is tcp://127.0.0.1:5555");
                Console.WriteLine();
                args = new string[] { "tcp://127.0.0.1:5555" };
            }

            string endpoint = args[0];

            using (var client = new RequestSocket())
            {
                //Conecta ao endpoint
                client.Connect(endpoint);

                for (int i = 0; i < 10; i++)
                {
                    var text = "Hello";

                    Console.WriteLine("Sending {0} to Server...", text);

                    //Envia um texto para o servidor
                    client.SendFrame(text);

                    //Recebe a resposta do servidor
                    var reply = client.ReceiveFrameString();

                    Console.WriteLine(" Received: {0} {1}!", text, reply);
                    
                }
            }
        }
    }
}
