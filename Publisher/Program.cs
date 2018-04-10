using System;
using RabbitMQ.Client;
using System.Text;
using System.Text.RegularExpressions;

namespace Publisher
{
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                string name = String.Empty;

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

                Console.WriteLine("Enter name: ");
                name = Console.ReadLine();

                bool containsNum = Regex.IsMatch(name, @"\d");

                if (!containsNum && name != null)
                {
                    using (var connection = factory.CreateConnection())
                    {
                        using (var channel = connection.CreateModel())
                        {
                            channel.QueueDeclare(queue: "name",
                                                 durable: false,
                                                 exclusive: false,
                                                 autoDelete: false,
                                                 arguments: null);

                            string message = String.Format("Hello my name is, {0}", name);

                            var body = Encoding.UTF8.GetBytes(message);

                            channel.BasicPublish(exchange: "",
                                         routingKey: "name",
                                         basicProperties: null,
                                         body: body);

                            Console.WriteLine("Message Sent");
                        }

                        Console.Write("Press Enter to exist");
                        Console.ReadLine();
                    }
                }
                else {
                    Console.Write("Couldn't send name, input was invalid!");
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
