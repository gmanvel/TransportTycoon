using TransportTycoon.Domain.Routing;
using TransportTycoon.Domain.Transport;
using Xunit;

namespace TransportTycoon.Domain.Tests
{
    public class TransportTests
    {
        [Fact]
        public void Truck_Goes_To_B()
        {
            var truck = new Truck(1);

            var cargo = new Cargo(1, Destination.B);

            var routeFactory = Route.Factory;

            var route = routeFactory.Create(Destination.Factory, Destination.B);

            truck.Deliver(new [] {cargo}, route, 0);

            int time = 0;
            while (!cargo.IsDelivered)
            {
                truck.OnTick(time);

                time++;
            }

            Assert.Equal(route.TimeEstimate, time);
        }

        [Fact]
        public void Transport_Goes_To_Port()
        {
            var truck = new Truck(1);

            var cargo = new Cargo(1, Destination.A);

            var routeFactory = Route.Factory;

            var route = routeFactory.Create(Destination.Factory, Destination.Port);

            truck.Deliver(new[] { cargo }, route, 0);

            int time = 0;
            while (true)
            {
                truck.OnTick(time);

                if (cargo.CurrentDestination == Destination.Port)
                    break;

                time++;
            }
            
            Assert.Equal(route.TimeEstimate, time);
        }

        [Fact]
        public void Ship_Goes_To_A()
        {
            var ship = new Ship(1);

            var cargo = new Cargo(1, Destination.A);

            var routeFactory = Route.Factory;

            var route = routeFactory.Create(Destination.Port, Destination.A);

            ship.Deliver(new[] { cargo }, route, 0);

            int time = 0;
            while (!cargo.IsDelivered)
            {
                ship.OnTick(time);

                time++;
            }

            Assert.Equal(route.TimeEstimate, time);
        }
    }
}