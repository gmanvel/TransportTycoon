using System;
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

        private ReturnTask _returnTask;

        private Action<int> _currentStep;

        public bool IsCompleted { get; private set; }

        public DeliveryTask(Cargo cargo, Route route, ITransportManager transportManager)
        {
            _cargo = cargo;
            _route = route;
            _transportManager = transportManager;

            _returnTask = null;
            IsCompleted = false;
            _currentStep = FindAvailableTransport;
        }

        public void Execute(int time)
        {
            _currentStep.Invoke(time);
        }

        private void FindAvailableTransport(int time)
        {
            _transport = _transportManager.GetTransportAt(_cargo.CurrentDestination, _route.TransportKind);

            if (_transport != null)
            {
                _returnTask = new ReturnTask(_transport, _route.GetReturnRoute().End);
                _transport.Deliver(new[] { _cargo }, _route);
                _transport.Tick(time);
                _currentStep = Deliver;
            }
        }

        private void Deliver(int time)
        {
            if (_cargo.CurrentDestination == _route.End)
            {
                _currentStep = Return;
            }

            _transport.Tick(time);
        }

        private void Return(int time)
        {
            _returnTask.Execute(time);

            if (_returnTask.Done())
                _currentStep = MarkDeliveryTaskComplete;
        }

        private void MarkDeliveryTaskComplete(int time) => IsCompleted = true;
        

        private class ReturnTask
        {
            private readonly ITransport _transport;
            private readonly IDestination _end;

            public ReturnTask(ITransport transport, IDestination end)
            {
                _transport = transport;
                _end = end;
            }

            public void Execute(int time)
            {
                if (Done())
                    return;

                _transport.Tick(time);
            }

            public bool Done() => _transport.IsAvailableAt(_end);
        }
    }
}