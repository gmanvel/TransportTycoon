namespace TransportTycoon.Domain
{
    public interface ITransport
    {
        TransportKind Kind { get; }
    }

    public enum TransportKind
    {
        Truck,
        Ship
    }
}