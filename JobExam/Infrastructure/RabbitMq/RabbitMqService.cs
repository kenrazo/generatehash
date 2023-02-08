using Infrastructure.Repositories;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.RabbitMq
{
    public class RabbitMqService : IRabbitmqService
    {
        private const string Exchange = "hash_exchange";
        private const string Queue = "hash_queue";
        private const string RoutingKey = "hash_routingkey";
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IRabbitMqConnectionFactory _connectionFactory;
        private readonly IHashRepository _hashRepository;
        private readonly HashDbContext _hashDbContext;

        public RabbitMqService(IRabbitMqConnectionFactory connectionFactory, IHashRepository hashRepository, HashDbContext hashDbContext)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(_connectionFactory));
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _hashRepository = hashRepository ?? throw new ArgumentNullException(nameof(_hashRepository));
            _hashDbContext = hashDbContext;
        }

        public void SendMessage(string[] messages)
        {
            _channel.ExchangeDeclare(Exchange, ExchangeType.Fanout);
            _channel.QueueDeclare(Queue, false, false, false, null);
            _channel.QueueBind(Queue, Exchange, "");

            var batches = messages
                .Select((h, i) => new { h, i })
                .GroupBy(x => x.i / 10_000, x => x.h);

            Parallel.ForEach(batches,
            new ParallelOptions { MaxDegreeOfParallelism = 4 },
           batch =>
            {
                using (var channel = _connection.CreateModel())
                {
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    var hashDto = new HashDto(batch.ToList());
                    var json = JsonConvert.SerializeObject(hashDto);

                    var body = Encoding.UTF8.GetBytes(json);
                    channel.BasicPublish(Exchange, RoutingKey, properties, body);
                }
            });
        }

        public async Task ConsumeMessage()
        {
            _channel.QueueDeclare(Queue, false, false, false, null);
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body;
                var message = System.Text.Encoding.UTF8.GetString(body.ToArray());

                var hashDto = JsonConvert.DeserializeObject<HashDto>(message);

                await ProcessMessage(message, hashDto);
            };
            _channel.BasicConsume(queue: Queue,
                                  autoAck: true,
                                  consumer: consumer);
        }

        private async Task ProcessMessage(string message, HashDto? hashDto)
        {
            if (hashDto != null)
            {
                hashDto.Hashes.AsParallel()
                    .WithDegreeOfParallelism(4)
                    .ForAll(async hash =>
                {
                    await _hashRepository.Add(new Hash()
                    {
                        Date = DateTime.Now,
                        Id = Guid.NewGuid(),
                        Sha1 = hash
                    });
                });

                Console.WriteLine("Received message: {0}", message);
            }
        }

        public void CloseConnection()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
