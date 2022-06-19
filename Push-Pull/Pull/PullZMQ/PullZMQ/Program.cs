using System;
using System.Diagnostics;
using NetMQ;
using NetMQ.Sockets;

namespace PullZMQ
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var sink = new PullSocket())
            {
				//Disponibiliza uma porta para receber os PUSHs
                sink.Bind("tcp://127.0.0.1:5557");

				//Espera até receber a primeira msg
                sink.ReceiveFrameBytes();

				//Inicia a contagem do tempo
				var stopwatch = new Stopwatch();
				stopwatch.Start();

				// Processa as 100 confirmacoes
				for (int i = 0; i < 100; ++i)
				{
					sink.ReceiveFrameBytes();

					if ((i / 10) * 10 == i)
						Console.Write(":");
					else
						Console.Write(".");
				}

				// Calcula o tempo gasto para executar todas as tasks
				stopwatch.Stop();
				Console.WriteLine("Total elapsed time: {0} ms", stopwatch.ElapsedMilliseconds);
			}
        }
    }
}
