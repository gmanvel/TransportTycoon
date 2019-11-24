namespace TransportTycoon.Domain.Transport
{
    public interface ITransportManager
    {
        ITransport GetTransportAt(IDestination destination, TransportKind transportKind);
    }
}