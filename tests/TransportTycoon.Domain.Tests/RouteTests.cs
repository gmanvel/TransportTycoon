using System;
using System.Collections.Generic;
using TransportTycoon.Domain.Routing;
using Xunit;

namespace TransportTycoon.Domain.Tests
{
    public class RouteTests
    {
        private readonly IRouteFactory _routeFactory;

        public RouteTests()
        {
            _routeFactory = Route.Factory(new RouteValidator());
        }

        [Theory]
        [MemberData(nameof(RouteStartEndDestinations))]
        public void Route_Start_End_Destinations_Should_Differ(IDestination start, IDestination end, bool throws)
        {
            if (throws)
            {
                Assert.Throws<ArgumentException>(() => _routeFactory.Create(start, end, TransportKind.Truck, 1));
            }
            else
            {
                var route = _routeFactory.Create(start, end, TransportKind.Truck, 1);
            }
        }

        public static IEnumerable<object[]> RouteStartEndDestinations()
        {
            yield return new object[]
            {
                Destination.Factory,
                Destination.Factory,
                true
            };

            yield return new object[]
            {
                Destination.Factory,
                Destination.Port,
                false
            };
        }

        [Theory]
        [MemberData(nameof(InvalidRoutes))]
        public void Route_Should_Be_Valid(IDestination start, IDestination end) =>
            Assert.Throws<ArgumentException>(() => _routeFactory.Create(start, end, TransportKind.Truck, 1));

        public static IEnumerable<object[]> InvalidRoutes()
        {
            yield return new object[]
            {
                Destination.Factory,
                Destination.A
            };

            yield return new object[]
            {
                Destination.A,
                Destination.Factory
            };

            yield return new object[]
            {
                Destination.Port,
                Destination.B
            };

            yield return new object[]
            {
                Destination.Port,
                Destination.B
            };
        }

        [Theory]
        [MemberData(nameof(InvalidTransportKindsForRoutes))]
        public void TransportKind_Should_Be_Valid_For_Given_Route(IDestination start, IDestination end, TransportKind transportKind) =>
            Assert.Throws<ArgumentException>(() => _routeFactory.Create(start, end, transportKind, 1));

        public static IEnumerable<object[]> InvalidTransportKindsForRoutes()
        {
            yield return new object[]
            {
                Destination.Factory,
                Destination.Port,
                TransportKind.Ship
            };

            yield return new object[]
            {
                Destination.Port,
                Destination.Factory,
                TransportKind.Ship
            };

            yield return new object[]
            {
                Destination.Factory,
                Destination.B,
                TransportKind.Ship
            };

            yield return new object[]
            {
                Destination.B,
                Destination.Factory,
                TransportKind.Ship
            };

            yield return new object[]
            {
                Destination.Port,
                Destination.A,
                TransportKind.Truck
            };

            yield return new object[]
            {
                Destination.A,
                Destination.Port,
                TransportKind.Truck
            };
        }

        [Fact]
        public void Routes_Return_Route_Swaps_Start_End_Destinations()
        {
            var route = _routeFactory.Create(Destination.Factory, Destination.B, TransportKind.Truck, 5);

            var returnRoute = route.GetReturnRoute();

            Assert.Equal(route.Start, returnRoute.End);
            Assert.Equal(route.End, returnRoute.Start);
            Assert.Equal(route.TransportKind, returnRoute.TransportKind);
            Assert.Equal(route.TimeEstimate, returnRoute.TimeEstimate);
        }
    }
}