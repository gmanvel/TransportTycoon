using System;
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

        public static IRouteFactory Factory(IValidateRoute routeValidator) => new RouteFactory(routeValidator);

        public class RouteFactory : IRouteFactory
        {
            private readonly IValidateRoute _routeValidator;

            public RouteFactory(IValidateRoute routeValidator)
            {
                _routeValidator = routeValidator;
            }

            public Route Create(IDestination start, IDestination end, TransportKind transportKind, int routeEstimate)
            {
                if (start == end)
                    throw new ArgumentException("Route start and end destinations should be different.");

                if (!_routeValidator.IsValidRoute(start, end))
                    throw new ArgumentException($"{start.Name} -> {end.Name} is an invalid route.");

                if (!_routeValidator.IsValidTransportTypeForRoute(start, end, transportKind))
                    throw new ArgumentException("Invalid transport kind for the route.");

                return new Route(start, end, transportKind, routeEstimate);
            }
        }
    }
}