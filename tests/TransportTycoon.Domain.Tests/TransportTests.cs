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

            var routeFactory = Route.Factory(new RouteValidator());

            var route = routeFactory.Create(Destination.Factory, Destination.B, TransportKind.Truck, 5);

            truck.PlanDelivery(new [] {cargo}, route);

            int time = 0;
            while (!cargo.IsDelivered)
            {
                truck.Tick(time);

                time++;
            }

            Assert.Equal(5, time);
        }

        [Fact]
        public void Transport_Goes_To_Port()
        {
            var truck = new Truck(1);

            var cargo = new Cargo(1, Destination.A);

            var routeFactory = Route.Factory(new RouteValidator());

            var route = routeFactory.Create(Destination.Factory, Destination.Port, TransportKind.Truck, 1);

            truck.PlanDelivery(new[] { cargo }, route);

            int time = 0;
            while (true)
            {
                truck.Tick(time);

                if (cargo.CurrentDestination == Destination.Port)
                    break;

                time++;
            }
            
            Assert.Equal(1, time);
        }

        [Fact]
        public void Ship_Goes_To_A()
        {
            var ship = new Ship(1);

            var cargo = new Cargo(1, Destination.A);

            var routeFactory = Route.Factory(new RouteValidator());

            var route = routeFactory.Create(Destination.Port, Destination.A, TransportKind.Ship, 4);

            ship.PlanDelivery(new[] { cargo }, route);

            int time = 0;
            while (!cargo.IsDelivered)
            {
                ship.Tick(time);

                time++;
            }

            Assert.Equal(4, time);
        }
    }
}