using System;
using System.Collections.Generic;
using TransportTycoon.Domain.Routing;
using Xunit;

namespace TransportTycoon.Domain.Tests
{
    public class RouteTests
    {
        [Fact]
        public void Route_Start_End_Destinations_Should_Differ(IDestination start, IDestination end, bool throws)
        {
            if (throws)
            {
                Assert.Throws<ArgumentException>(() => new Route(start, end, TransportKind.Truck, 1));
            }
            else
            {
                new Route(start, end, TransportKind.Truck, 1);
            }
        }

        public static IEnumerable<object[]> RouteStartEndDestinations()
        {
            yield return new object[]
            {
                Destination.Factory,
                true
            };
        }
    }
}