using System;
using System.Threading;
using NetMQ;
using NetMQ.Sockets;

namespace WorkerZMQ
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var receiver = new PullSocket())
            using(var sink = new PushSocket())
            {
                receiver.Connect("tcp://127.0.0.1:5555");
                sink.Connect("tcp://127.0.0.1:5557");

                // Processa as tarefas infinitamente
                while (true)
                {
                    var replyBytes = new byte[4];

                    replyBytes = receiver.ReceiveFrameBytes();

                    int workload = BitConverter.ToInt32(replyBytes, 0);
                    Console.WriteLine("{0}.", workload);    //Mostra o progresso

                    Thread.Sleep(workload); // Executa o task

                    sink.SendFrame(new byte[0]);   // Envia o resultado para o Coletor
                }
            }
        }
    }
}
