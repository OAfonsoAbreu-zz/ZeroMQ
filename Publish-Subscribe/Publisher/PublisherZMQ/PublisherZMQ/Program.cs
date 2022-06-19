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
                publisher.Bind(address);

                var rnd = new Random();

                while (true)
                {
                    // Pega CEP e Temperatura do aleatorio
                    int zipcode = rnd.Next(99999);
                    int temperature = rnd.Next(-55, +45);

                    var update = string.Format("{0:D5} {1}", zipcode, temperature); //topic msg

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
