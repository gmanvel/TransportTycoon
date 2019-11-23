﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TransportTycoon.Domain.Events;
using TransportTycoon.Domain.Infrastructure;
using TransportTycoon.Domain.Routing;

namespace TransportTycoon.Domain.Transport
{
    public interface ITransport2
    {
        int Id { get; }

        TransportKind Kind { get; }

        bool IsAvailableAt(IDestination destination);

        void Deliver(IEnumerable<Cargo> cargoes, Route route);
    }

    public class Truck2: ITransport2
    {
        private Queue<Action<int>> _deliverySteps;

        private readonly List<Cargo> _carryingCargoes;

        private Route _currentRoute = null;

        private IDestination _currentDestination;

        private readonly IDestination _origin;

        public int Id { get; }

        public TransportKind Kind => TransportKind.Truck;

        public Truck2(int id) : this(id, Destination.Factory)
        { }

        public Truck2(int id, IDestination origin)
        {
            Id = id;
            _origin = origin;
            _currentDestination = origin;
            _deliverySteps = new Queue<Action<int>>();
            _carryingCargoes = new List<Cargo>();
        }

        public bool IsAvailableAt(IDestination destination)
        {
            if (IsOnRoute())
                return false;

            return _currentDestination == destination;
        }

        public void Deliver(IEnumerable<Cargo> cargoes, Route route)
        {
            _carryingCargoes.AddRange(cargoes);

            _currentRoute = route;

            var deliveryEstimate = route.TimeEstimate;

            // 1 depart
            _deliverySteps.Enqueue(Depart);

            for (int i = 1; i < deliveryEstimate - 1; i++)
            {
                _deliverySteps.Enqueue(Move);
            }

            // arrive
            _deliverySteps.Enqueue(Arrive);
        }

        public void Tick(int time)
        {
            if (!_deliverySteps.TryDequeue(out var deliveryStep))
                return;

            deliveryStep.Invoke(time);
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

        private void Move(int time)
        {
            Debug.WriteLine($"Move at {time}");
        }

        private void Arrive(int time)
        {
            var transportArrivedEvent = new TransportArrivedEvent
            {
                Time = time,
                TransportId = Id,
                Kind = Kind.ToString().ToUpperInvariant(),
                Location = _currentRoute.End.Name.ToUpperInvariant(),
                Cargo = _carryingCargoes.Select(carryingCargo =>
                    new CargoDetails
                    {
                        CargoId = carryingCargo.Id,
                        Destination = carryingCargo.TargetDestination.Name.ToUpperInvariant(),
                        Origin = carryingCargo.Origin.Name.ToUpperInvariant()
                    })
            };

            Debug.WriteLine(transportArrivedEvent.ToString());

            _currentDestination = _currentRoute.End;

            _carryingCargoes.ForEach(cargo => cargo.DropAt(_currentRoute.End));

            if (_origin != _currentDestination)
            {
                Return();
            }
        }

        private void Return()
        {
            var returnRoute = _currentRoute.GetReturnRoute();

            Deliver(Enumerable.Empty<Cargo>(), returnRoute);
        }

        private bool IsOnRoute() => _currentRoute is null;
    }
}