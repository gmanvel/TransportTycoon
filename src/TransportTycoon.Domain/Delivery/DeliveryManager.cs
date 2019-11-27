using System.Collections.Generic;
using System.Linq;
using TransportTycoon.Domain.Routing;
using TransportTycoon.Domain.Transport;

namespace TransportTycoon.Domain.Delivery
{
    public class DeliveryManager : IDeliveryManager
    {
        private readonly IPlanRoute _routePlanner;
        private readonly ITransportManager _transportManager;
        private readonly List<DeliveryTask> _deliveryTasks;

        public delegate void TickHandler(int time);
        public event TickHandler OnTick;

        public DeliveryManager(IPlanRoute routePlanner, ITransportManager transportManager)
        {
            _routePlanner = routePlanner;
            _transportManager = transportManager;
            _deliveryTasks = new List<DeliveryTask>();

            OnTick += _transportManager.OnTick;
        }

        public void PlanDelivery(Cargo cargo)
        {
            var routes = _routePlanner.GetDeliveryRoutes(cargo.TargetDestination);

            foreach (var route in routes)
            {
                var deliveryTask = new DeliveryTask(cargo, route, _transportManager);
                
                _deliveryTasks.Add(deliveryTask);
            }
        }

        public void Tick(int time)
        {
            _deliveryTasks.ForEach(deliveryTask => deliveryTask.Setup());
            _deliveryTasks.RemoveAll(deliveryTask => deliveryTask.IsCompleted);

            OnTick?.Invoke(time);

            _deliveryTasks.ForEach(deliveryTask => deliveryTask.Setup(time));
            _deliveryTasks.RemoveAll(deliveryTask => deliveryTask.IsCompleted);
        }

        public void InitialSetup()
        {
            DeliverCargoesFromFactory(0);

            DeliverCargoesFromPort(0);
        }

        public void Tick2(int time)
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

                var truck = _transportManager.GetTransportAt2(factory, TransportKind.Truck);

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

            var ship = _transportManager.GetTransportAt2(port, TransportKind.Ship);

            if (ship == null)
                return;

            var cargoes = Destination.Port.TakeCargoes(portCargoes.Select(cargo => cargo.Id));

            var route = Route.Factory(new RouteValidator()).Create(Destination.Port, Destination.A);

            ship.Deliver(cargoes, route, time);
        }
    }
}