using System.Collections.Generic;
using TransportTycoon.Domain.Routing;
using TransportTycoon.Domain.Transport;

namespace TransportTycoon.Domain.Delivery
{
    public class DeliveryManager : IDeliveryManager
    {
        private readonly IPlanRoute _routePlanner;
        private readonly ITransportManager _transportManager;

        private readonly List<DeliveryTask> _deliveryTasks;

        public DeliveryManager(IPlanRoute routePlanner, ITransportManager transportManager)
        {
            _routePlanner = routePlanner;
            _transportManager = transportManager;
            _deliveryTasks = new List<DeliveryTask>();
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
            _deliveryTasks.ForEach(deliveryTask => deliveryTask.Execute(time));
            _deliveryTasks.RemoveAll(deliveryTask => deliveryTask.IsCompleted);
        }
    }
}