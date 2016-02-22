namespace BarbeQ
{
    public interface IDelivery
    {
        string Payload { get; }
        bool Ack { get; }
        bool Reject { get; }
        bool Push { get; }
    }
}