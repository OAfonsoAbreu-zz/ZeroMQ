using System;
using NetMQ;
using NetMQ.Sockets;

namespace PushZMQ
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var sender = new PushSocket())
            using(var sink = new PushSocket())
            {
                //Define porta pela qual irá fazer envio
                sender.Bind("tcp://*:5555");

                //Se conecta ao Coletor
                sink.Connect("tcp://127.0.0.1:5557");

				Console.WriteLine("Press ENTER when the workers are ready...");
				Console.ReadKey(true);
				Console.WriteLine("Sending tasks to workers...");

				// A primeira msg é '0' e indica o inicio do lote
				sink.SendFrame(new byte[] { 0x00 }, 1);

				var rnd = new Random();

				// Envia 100 tarefas
				int i = 0;
				long total_msec = 0;    // Custo Total Utilizado
				for (; i < 100; ++i)
				{
					// Carga de trabalho aleatoria entre 1 e 100ms
					int workload = rnd.Next(100) + 1;
					total_msec += workload;
					byte[] action = BitConverter.GetBytes(workload);

					Console.WriteLine("{0}", workload);
					sender.SendFrame(action, action.Length);
				}

				Console.WriteLine("Total expected cost: {0} ms", total_msec);

			}
        }
    }
}
