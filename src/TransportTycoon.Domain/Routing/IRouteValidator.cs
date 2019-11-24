using System;
using System.Collections.Generic;
using TransportTycoon.Domain.Transport;

namespace TransportTycoon.Domain.Routing
{
    public class RouteValidator: IValidateRoute
    {
        private static Dictionary<(IDestination start, IDestination end), TransportKind> RouteTransportKindMap { get; } =
            new Dictionary<(IDestination start, IDestination end), TransportKind>
            {
                [(Destination.Factory, Destination.B)] = TransportKind.Truck,
                [(Destination.B, Destination.Factory)] = TransportKind.Truck,

                [(Destination.Factory, Destination.Port)] = TransportKind.Truck,
                [(Destination.Port, Destination.Factory)] = TransportKind.Truck,

                [(Destination.Port, Destination.A)] = TransportKind.Ship,
                [(Destination.A, Destination.Port)] = TransportKind.Ship
            };

        public bool IsValidRoute(IDestination start, IDestination end) => RouteTransportKindMap.ContainsKey((start, end));

        public bool IsValidTransportTypeForRoute(IDestination start, IDestination end, TransportKind transportKind)
        {
            if(!IsValidRoute(start, end))
                throw new InvalidOperationException("Can't check transport type of invalid route.");

            var routeKey = (start, end);

            var validTransportKind = RouteTransportKindMap[routeKey];

            return validTransportKind == transportKind;
        }
    }
}