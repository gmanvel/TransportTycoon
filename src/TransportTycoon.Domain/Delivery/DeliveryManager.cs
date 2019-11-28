using System.Linq;
using TransportTycoon.Domain.Routing;
using TransportTycoon.Domain.Transport;

namespace TransportTycoon.Domain.Delivery
{
    public class DeliveryManager : IDeliveryManager
    {
        private readonly IPlanRoute _routePlanner;
        private readonly ITransportManager _transportManager;

        public delegate void TickHandler(int time);
        public event TickHandler OnTick;

        public DeliveryManager(IPlanRoute routePlanner, ITransportManager transportManager)
        {
            _routePlanner = routePlanner;
            _transportManager = transportManager;

            OnTick += _transportManager.OnTick;
        }

        public void InitialSetup()
        {
            DeliverCargoesFromFactory(0);

            DeliverCargoesFromPort(0);
        }

        public void Tick(int time)
        {
            OnTick?.Invoke(time);

            DeliverCargoesFromFactory(time);

            DeliverCargoesFromPort(time);
        }

        private void DeliverCargoesFromFactory(int time)
        {
            var factory = Destination.Factory;

            var cargoes = factory.PeekCargoes().ToList();

            foreach (var cargo in cargoes)
            {
                var routes = _routePlanner.GetDeliveryRoutes(cargo.TargetDestination).Where(route => route.Start == Destination.Factory);

                var firstRoute = routes.FirstOrDefault();

                var truck = _transportManager.GetTransportAt(factory, TransportKind.Truck);

                if (truck != null)
                {
                    var __cargo = factory.TakeCargo(cargo.Id);
                    truck.Deliver(new[] {__cargo}, firstRoute, time);
                }
            }
        }

        private void DeliverCargoesFromPort(int time)
        {
            var port = Destination.Port;

            var portCargoes = port.PeekCargoes().Take(4).ToList();

            if (portCargoes.Count == 0)
                return;

            var ship = _transportManager.GetTransportAt(port, TransportKind.Ship);

            if (ship == null)
                return;

            var cargoes = Destination.Port.TakeCargoes(portCargoes.Select(cargo => cargo.Id));

            var route = Route.Factory.Create(Destination.Port, Destination.A);

            ship.Deliver(cargoes, route, time);
        }
    }
}