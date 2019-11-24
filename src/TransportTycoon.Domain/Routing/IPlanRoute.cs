using System.Collections.Generic;

namespace TransportTycoon.Domain.Routing
{
    public interface IPlanRoute
    {
        IEnumerable<Route> GetDeliveryRoutes(IDestination end);
    }
}