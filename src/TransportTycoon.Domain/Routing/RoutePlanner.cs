using System;
using System.Collections.Generic;

namespace TransportTycoon.Domain.Routing
{
    public class RoutePlanner : IPlanRoute
    {
        private readonly IRouteFactory _routeFactory;

        public RoutePlanner(IRouteFactory routeFactory)
        {
            _routeFactory = routeFactory;
        }

        public IEnumerable<Route> GetDeliveryRoutes(IDestination end)
        {
            if (end == Destination.B)
            {
                yield return _routeFactory.Create(Destination.Factory, Destination.B);
            }
            else if (end == Destination.A)
            {
                yield return _routeFactory.Create(Destination.Factory, Destination.Port);
                yield return _routeFactory.Create(Destination.Port, Destination.A);
            }
            else
                throw new ArgumentException($"Doesn't have routes to deliver to {end.Name}");
        }
    }
}