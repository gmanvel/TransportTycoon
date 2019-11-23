using TransportTycoon.Domain.Routing;
using TransportTycoon.Domain.Transport;
using Xunit;

namespace TransportTycoon.Domain.Tests
{
    public class TransportTests
    {
        [Fact]
        public void Transport()
        {
            var truck = new Truck2(1);

            var cargo = new Cargo(1, Destination.B);

            var routeFactory = Route.Factory(new RouteValidator());

            var route = routeFactory.Create(Destination.Factory, Destination.B, TransportKind.Truck, 5);

            truck.Deliver(new [] {cargo}, route);

            int time = 0;
            while (!cargo.IsDelivered)
            {
                truck.Tick(time);

                time++;
            }

            Assert.Equal(5, time);
        }

        [Fact]
        public void Transport2()
        {
            var truck = new Truck2(1);

            var cargo = new Cargo(1, Destination.A);

            var routeFactory = Route.Factory(new RouteValidator());

            var route = routeFactory.Create(Destination.Factory, Destination.Port, TransportKind.Truck, 1);

            truck.Deliver(new[] { cargo }, route);

            int time = 0;
            while (!cargo.IsDelivered)
            {
                truck.Tick(time);

                time++;
            }

            Assert.Equal(1, time);
        }
    }
}