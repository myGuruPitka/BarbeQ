namespace BarbeQ
{
    public interface IDelivery
    {
        string Payload { get; }
        bool Ack();
        bool Reject();
        bool Push();
    }
}