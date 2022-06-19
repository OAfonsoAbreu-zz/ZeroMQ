using System;
using System.Threading;
using NetMQ;
using NetMQ.Sockets;

namespace PublisherZMQ
{
    class Program
    {
        static void Main(string[] args)
        {
            string address = "tcp://*:5555";
            Console.WriteLine("I: Publisher.Bind'ing on {0}", address);

            using (var publisher = new PublisherSocket())
            {
                //Disponibiliza endpoint
                publisher.Bind(address);

                var rnd = new Random();

                while (true)
                {
                    // Pega CEP e Temperatura do aleatorio
                    int zipcode = rnd.Next(99999);
                    int temperature = rnd.Next(-55, +45);

                    //Criação de msg com Topico + Dados
                    var update = string.Format("{0:D5} {1}", zipcode, temperature); //topic msg

                    //Criação de msg com Topico e Dados separados
                    var topic = string.Format("{0:D5}", zipcode);
                    var msg = string.Format("{0}", temperature); 

                    Console.WriteLine("Sending --> CEP: {0} | Temperatura: {1}°C", topic, msg);

                    //Envia para todos subscribers
                    publisher
                        .SendFrame(update);
                        //.SendMoreFrame(topic)
                        //.SendFrame(msg);


                }
            }
        }
    }
}
