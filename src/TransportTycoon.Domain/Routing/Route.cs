using System;
using System.Collections.Generic;
using TransportTycoon.Domain.Transport;

namespace TransportTycoon.Domain.Routing
{
    public class Route
    {
        public IDestination Start { get; }

        public IDestination End { get; }

        public TransportKind TransportKind { get; }

        public int TimeEstimate { get; }

        private Route(IDestination start, IDestination end, TransportKind transportKind, int timeEstimate)
        {
            Start = start;
            End = end;
            TransportKind = transportKind;
            TimeEstimate = timeEstimate;
        }

        public Route GetReturnRoute() => new Route(End, Start, TransportKind, TimeEstimate);

        public static IRouteFactory Factory { get; } = new RouteFactory();

        public class RouteFactory : IRouteFactory
        {
            private static Dictionary<(IDestination start, IDestination end), Route> DestinationToRouteMap { get; } =
                new Dictionary<(IDestination start, IDestination end), Route>
                {
                    [(Destination.Factory, Destination.B)] = new Route(Destination.Factory, Destination.B, TransportKind.Truck, 5),
                    [(Destination.B, Destination.Factory)] = new Route(Destination.B, Destination.Factory, TransportKind.Truck, 5),

                    [(Destination.Factory, Destination.Port)] = new Route(Destination.Factory, Destination.Port, TransportKind.Truck, 1),
                    [(Destination.Port, Destination.Factory)] = new Route(Destination.Port, Destination.Factory, TransportKind.Truck, 1),

                    [(Destination.Port, Destination.A)] = new Route(Destination.Port, Destination.A, TransportKind.Ship, 6),
                    [(Destination.A, Destination.Port)] = new Route(Destination.A, Destination.Port, TransportKind.Ship, 6)
                };

            public Route Create(IDestination start, IDestination end)
            {
                if (start == end)
                    throw new ArgumentException("Route start and end destinations should be different.");

                var destinationKey = (start, end);

                if (!DestinationToRouteMap.ContainsKey(destinationKey))
                    throw new ArgumentException($"{start.Name} -> {end.Name} is an invalid route.");

                return DestinationToRouteMap[destinationKey];
            }
        }
    }
}