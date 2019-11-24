using System;
using System.Collections.Generic;
using TransportTycoon.Domain.Transport;

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
                yield return _routeFactory.Create(Destination.Factory, Destination.B, TransportKind.Truck, 5);
            }
            else if (end == Destination.A)
            {
                yield return _routeFactory.Create(Destination.Factory, Destination.Port, TransportKind.Truck, 1);
                yield return _routeFactory.Create(Destination.Port, Destination.A, TransportKind.Ship, 4);
            }
            else
                throw new ArgumentException($"Doesn't have routes to deliver to {end.Name}");
        }
    }
}