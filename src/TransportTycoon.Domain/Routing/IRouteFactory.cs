using TransportTycoon.Domain.Transport;

namespace TransportTycoon.Domain.Routing
{
    public interface IRouteFactory
    {
        Route Create(IDestination start, IDestination end, TransportKind transportKind, int routeEstimate);

        Route Create(IDestination start, IDestination end);
    }
}