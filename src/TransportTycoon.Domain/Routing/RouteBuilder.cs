using System;

namespace TransportTycoon.Domain.Routing
{
    public class RouteBuilder: IRouteStart, IRouteEnd, IRouteTransport, IRouteEstimate
    {
        private readonly IValidateRoute _routeValidator;
        private IDestination _routeStart;
        private IDestination _routeEnd;
        private TransportKind _transportKind;
        private int _estimate;

        private RouteBuilder(IValidateRoute routeValidator)
        {
            _routeValidator = routeValidator;
        }

        public static IRouteStart Initialize(IValidateRoute routeValidator) => new RouteBuilder(routeValidator);

        public IRouteEnd WithStart(IDestination destination)
        {
            _routeStart = destination;

            return this;
        }

        public IRouteTransport WithEnd(IDestination destination)
        {
            if(_routeStart == _routeEnd)
                throw new ArgumentException("Route start and end destinations should be different.");

            if(!_routeValidator.IsValidRoute(_routeStart, destination))
                throw new ArgumentException($"{_routeStart.Name} -> {_routeEnd.Name} is an invalid route.");

            _routeEnd = destination;

            return this;
        }

        public IRouteEstimate WithTransportKind(TransportKind transportKind)
        {
            if(!_routeValidator.IsValidTransportTypeForRoute(_routeStart, _routeEnd, transportKind))
                throw new ArgumentException("Invalid transport kind for the route.");

            _transportKind = transportKind;

            return this;
        }

        public RouteBuilder WithEstimate(int estimate)
        {
            _estimate = estimate;

            return this;
        }

        public Route Build() => new Route(_routeStart, _routeEnd, _transportKind, _estimate);
    }
}