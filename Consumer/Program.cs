using System;
using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;

namespace Consumer
{
    class Program
    {
        public static void Main(string[] args)

        {
            try
            {
                //string name = String.Empty;

                var factory = new ConnectionFactory
                {
                    HostName = "localhost",
                    Port = 5672,
                    UserName = "guest",
                    Password = "guest",
                    VirtualHost = "/",
                    RequestedHeartbeat = 60,
                    Ssl = { }
                };

                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "name",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        var name = message.Split(",")[1].Split('.')[0];
                        Console.WriteLine("Message Received \n{0}", message);
                        Console.WriteLine("Hello {0}, I am your father! \nPress Enter to exit", name);
                    };
                    channel.BasicConsume(queue: "name",
                                         autoAck: true,
                                         consumer: consumer);
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
