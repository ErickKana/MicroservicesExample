using PlatformService.Dtos;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBugClient
    {
        private readonly IConfiguration _config;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration config)
        {
            _config = config;
            var factory = new ConnectionFactory()
            {
                HostName = config["RabbitMqHost"],
                Port = config.GetValue<int>("RabbitMqPort")
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: _config["RabbitMqExchange"], type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown!;

                Console.WriteLine("--> Connected to the message bus");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to the message bus: {ex.ToString()}" );   
            }
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ connection shutdown");
        }
        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(
                exchange: _config["RabbitMqExchange"], 
                routingKey: "", 
                basicProperties: null,
                body: body);

            Console.WriteLine($"--> We have sent {message}");
        }

        public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
        {
            var message = JsonSerializer.Serialize(platformPublishedDto);

            if(_connection.IsOpen)
            {
                Console.WriteLine($"--> RabbitMQ connection open, sending message.");
                SendMessage(message);
            }
            else 
            {
                Console.WriteLine($"--> RabbitMQ connection is closed.");
            }
        }

        public void Dispose()
        {
            Console.WriteLine("MessageBus disposed");
            if(_connection.IsOpen )
            {
                _channel.Close();
                _connection.Close();
            }
        }


    }
}
