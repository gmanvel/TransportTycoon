using System;
using System.Collections.Generic;
using TransportTycoon.Domain.Routing;

namespace TransportTycoon.Domain.Transport
{
    public class Ship2: ITransport2
    {
        private Queue<Action<int>> _deliverySteps;

        private readonly List<Cargo> _carryingCargoes;

        private Route _currentRoute = null;

        private IDestination _currentDestination;

        private readonly IDestination _origin;

        public int Id { get; }
        public TransportKind Kind => TransportKind.Ship;

        public Ship2(int id) : this(id, Destination.Factory)
        { }

        public Ship2(int id, IDestination origin)
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
            throw new System.NotImplementedException();
        }

        private bool IsOnRoute() => _currentRoute is null;
    }
}