namespace Infrastructure.RabbitMq
{
    public interface IRabbitmqService
    {
        void SendMessage(string[] message);
        Task ConsumeMessage();

        void CloseConnection();
    }
}