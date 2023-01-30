using RabbitMQ.Client;

namespace Infrastructure.RabbitMq
{
    public interface IRabbitMqConnectionFactory
    {
        IConnection CreateConnection();
    }
}