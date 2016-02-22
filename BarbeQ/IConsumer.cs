namespace BarbeQ
{
    public interface IConsumer
    {
        void Consume(IDelivery delivery);
    }
}