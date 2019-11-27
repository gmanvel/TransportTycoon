namespace TransportTycoon.Domain.Transport
{
    public interface ITransportManager
    {
        void OnTick(int time);

        ITransport GetTransportAt(IDestination destination, TransportKind transportKind);

        ITransport2 GetTransportAt2(IDestination destination, TransportKind transportKind);
    }
}