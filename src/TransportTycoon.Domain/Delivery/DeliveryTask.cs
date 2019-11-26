using TransportTycoon.Domain.Routing;
using TransportTycoon.Domain.Transport;

namespace TransportTycoon.Domain.Delivery
{
    internal class DeliveryTask
    {
        private readonly Cargo _cargo;

        private readonly Route _route;

        private readonly ITransportManager _transportManager;

        private ITransport _transport;

        public bool IsCompleted { get; private set; }

        public DeliveryTask(Cargo cargo, Route route, ITransportManager transportManager)
        {
            _cargo = cargo;
            _route = route;
            _transportManager = transportManager;

            IsCompleted = false;
        }

        public void Setup()
        {
            if (IsCompleted)
                return;

            if (_cargo.CurrentDestination == _route.End)
            {
                IsCompleted = true;
                return;
            }

            if (_transport == null)
            {
                _transport = _transportManager.GetTransportAt(_cargo.CurrentDestination, _route.TransportKind);
                _transport?.PlanDelivery(new[] { _cargo }, _route);
            }
        }

        public void Setup(int time)
        {
            if (IsCompleted)
                return;

            if (_cargo.CurrentDestination == _route.End)
            {
                IsCompleted = true;
                return;
            }

            if (_transport == null)
            {
                _transport = _transportManager.GetTransportAt(_cargo.CurrentDestination, _route.TransportKind);
                _transport?.PlanDelivery(new[] { _cargo }, _route, time);
            }
        }
    }
}