namespace TransportTycoon.Domain.Routing
{
    public interface IRouteEnd
    {
        IRouteTransport WithEnd(IDestination destination);
    }
}