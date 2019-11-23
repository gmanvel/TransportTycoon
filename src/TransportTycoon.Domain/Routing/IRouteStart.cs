namespace TransportTycoon.Domain.Routing
{
    public interface IRouteStart
    {
        IRouteEnd WithStart(IDestination destination);
    }
}