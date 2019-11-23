namespace TransportTycoon.Domain.Routing
{
    public interface IRouteTransport
    {
        IRouteEstimate WithTransportKind(TransportKind transportKind);
    }
}