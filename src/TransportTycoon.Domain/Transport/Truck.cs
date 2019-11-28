using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TransportTycoon.Domain.Events;
using TransportTycoon.Domain.Infrastructure;
using TransportTycoon.Domain.Routing;

namespace TransportTycoon.Domain.Transport
{
    public class Truck: ITransport
    {
        private readonly IDestination _origin;

        private readonly List<Cargo> _cargoes;

        private readonly Queue<Action<int>> _deliverySteps;

        private Route _currentRoute;

        private IDestination _currentDestination;

        public int Id { get; }

        public TransportKind Kind => TransportKind.Truck;

        public Truck(int id) : this(id, Destination.Factory)
        { }

        public Truck(int id, IDestination origin)
        {
            Id = id;
            _origin = origin;
            _currentDestination = origin;

            _cargoes = new List<Cargo>();
            _currentRoute = null;
            _deliverySteps = new Queue<Action<int>>();
        }

        public bool IsAvailableAt(IDestination destination)
        {
            if (IsOnRoute())
                return false;

            return _currentDestination == destination && _cargoes.Count == 0;
        }

        public void Deliver(IEnumerable<Cargo> cargoes, Route route, int time)
        {
            _currentRoute = route;

            Load(cargoes, time);
            Depart(time);

            PlanDelivery(route);
        }

        private void PlanDelivery(Route route)
        {
            var routeEstimate = route.TimeEstimate;

            for (int i = 1; i <= routeEstimate - 1; i++)
            {
                _deliverySteps.Enqueue(Move);
            }

            _deliverySteps.Enqueue(ArriveAndUnload);
            
            void ArriveAndUnload(int time)
            {
                Arrive(time);
                Unload(time);
            }
        }

        private void Load(IEnumerable<Cargo> cargoes, int time)
        {
            if (cargoes.Count() != 1)
                throw new InvalidOperationException("Truck can carry only 1 cargo at a time.");

            _cargoes.AddRange(cargoes);

            var cargoLoadedEvent = new CargoLoadedEvent
            {
                Time = time,
                TransportId = Id,
                Kind = Kind.ToString().ToUpperInvariant(),
                Location = _currentRoute.End.Name.ToUpperInvariant(),
                Duration = 0,
                Cargo = _cargoes.Select(cargo => new CargoDetails
                {
                    CargoId = cargo.Id,
                    Destination = cargo.TargetDestination.Name.ToUpperInvariant(),
                    Origin = cargo.Origin.Name.ToUpperInvariant()
                })
            };

            Debug.WriteLine(cargoLoadedEvent.ToString());
        }

        private void Depart(int time)
        {
            var transportDepartedEvent = new TransportDepartedEvent
            {
                Time = time,
                TransportId = Id,
                Kind = Kind.ToString().ToUpperInvariant(),
                Location = _currentDestination.Name.ToUpperInvariant(),
                Destination = _currentRoute.End.Name.ToUpperInvariant(),
                Cargo = _cargoes.Select(carryingCargo =>
                    new CargoDetails
                    {
                        CargoId = carryingCargo.Id,
                        Destination = carryingCargo.TargetDestination.Name.ToUpperInvariant(),
                        Origin = carryingCargo.Origin.Name.ToUpperInvariant()
                    })
            };

            Debug.WriteLine(transportDepartedEvent.ToString());
        }

        private void Move(int time) { }

        private void Arrive(int time)
        {
            var transportArrivedEvent = new TransportArrivedEvent
            {
                Time = time,
                TransportId = Id,
                Kind = Kind.ToString().ToUpperInvariant(),
                Location = _currentRoute.End.Name.ToUpperInvariant(),
                Cargo = _cargoes.Select(carryingCargo =>
                    new CargoDetails
                    {
                        CargoId = carryingCargo.Id,
                        Destination = carryingCargo.TargetDestination.Name.ToUpperInvariant(),
                        Origin = carryingCargo.Origin.Name.ToUpperInvariant()
                    })
            };

            Debug.WriteLine(transportArrivedEvent.ToString());

            _currentDestination = _currentRoute.End;

            if (_currentDestination != _origin)
                Return(_currentRoute, time);
            else
                _currentRoute = null;
        }

        private void Unload(int time)
        {
            var cargoUnloadedEvent = new CargoUnloadedEvent
            {
                Time = time,
                TransportId = Id,
                Kind = Kind.ToString().ToUpperInvariant(),
                Location = _currentRoute.End.Name.ToUpperInvariant(),
                Duration = 0,
                Cargo = _cargoes.Select(cargo => new CargoDetails
                {
                    CargoId = cargo.Id,
                    Destination = cargo.TargetDestination.Name.ToUpperInvariant(),
                    Origin = cargo.Origin.Name.ToUpperInvariant()
                })
            };

            Debug.WriteLine(cargoUnloadedEvent.ToString());

            _cargoes.ForEach(cargo => cargo.DropAt(_currentDestination));

            _cargoes.Clear();
        }

        private void Return(Route route, int time)
        {
            var returnRoute = route.GetReturnRoute();

            _currentRoute = returnRoute;

            Depart(time);

            var deliveryEstimate = returnRoute.TimeEstimate;

            // move
            for (int i = 1; i < deliveryEstimate; i++)
            {
                _deliverySteps.Enqueue(Move);
            }

            // arrive
            _deliverySteps.Enqueue(Arrive);
        }

        public void OnTick(int time)
        {
            if (!_deliverySteps.TryDequeue(out var deliveryStep))
                return;

            deliveryStep.Invoke(time);
        }

        private bool IsOnRoute() => _currentRoute != null;
    }
}