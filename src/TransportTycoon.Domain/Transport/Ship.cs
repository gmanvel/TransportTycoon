using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TransportTycoon.Domain.Events;
using TransportTycoon.Domain.Infrastructure;
using TransportTycoon.Domain.Routing;

namespace TransportTycoon.Domain.Transport
{
    public class Ship : ITransport
    {
        private readonly IDestination _origin;

        private readonly List<Cargo> _carryingCargoes;

        private int _currentRouteEstimate = 0;

        private int _loadUnloadEstimate = 0;

        private Route _currentRoute = null;

        private IDestination _currentDestination;

        private Queue<Action<int>> _deliverySteps;

        public int Id { get; }

        public TransportKind Kind => TransportKind.Ship;

        public Ship(int id) : this(id, Destination.Port)
        { }

        public Ship(int id, IDestination origin)
        {
            Id = id;
            _origin = origin;
            _carryingCargoes = new List<Cargo>();
            _deliverySteps = new Queue<Action<int>>();
        }

        public bool IsAvailableAt(IDestination destination)
        {
            if (IsOnRoute)
                return false;

            return _currentDestination == destination;
        }

        public void Deliver(IEnumerable<Cargo> cargoes, Route route)
        {
            var timeEstimate = route.TimeEstimate;

            // 1 LOAD
            _deliverySteps.Enqueue(time => Load(cargoes, time));

            // 2 Depart
            _deliverySteps.Enqueue(Depart);

            // 3 Move
            foreach (var i in Enumerable.Range(1, timeEstimate))
            {
                _deliverySteps.Enqueue(Move);
            }

            // 4 Arrive
            _deliverySteps.Enqueue(Arrive);
        }

        public void Tick(int time)
        {
            if (!_deliverySteps.TryDequeue(out var step))
                return;

            step?.Invoke(time);
        }

        private void Arrive(int time)
        { }

        public void Move(int time)
        {
            if (_loadUnloadEstimate != 0)
            {
            }




            if (_currentRouteEstimate == 0 && _loadUnloadEstimate == 0)
                return;



            if (_currentRoute.TimeEstimate == _currentRouteEstimate && HasCargo)
                Depart(time);

            _currentRouteEstimate--;

            if (_currentRouteEstimate == 0)
                Arrive(time);
        }

        public bool CanTakeCargo() => _carryingCargoes.Count != 4;

        public void Load(IEnumerable<Cargo> cargoes, int time)
        {

        }

        public void Load(Cargo cargo, Route route)
        {
            _carryingCargoes.Add(cargo);

            _currentRoute = route;

            _currentRouteEstimate = route.TimeEstimate;

            _currentDestination = route.Start;

            _loadUnloadEstimate = 2;
        }

        public Cargo Unload()
        {
            throw new System.NotImplementedException();
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
                Cargo = _carryingCargoes.Select(carryingCargo =>
                    new CargoDetails
                    {
                        CargoId = carryingCargo.Id,
                        Destination = carryingCargo.TargetDestination.Name.ToUpperInvariant(),
                        Origin = carryingCargo.Origin.Name.ToUpperInvariant()
                    })
            };

            Debug.WriteLine(transportDepartedEvent.ToString());
        }

        private bool HasCargo => _carryingCargoes.Any();

        private bool IsOnRoute => _currentRoute is null;
    }
}