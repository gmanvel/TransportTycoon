using System.Diagnostics;
using TransportTycoon.Domain.Events;
using TransportTycoon.Domain.Routing;

namespace TransportTycoon.Domain.Transport
{
    public class Truck : ITransport
    {
        private int _currentRouteEstimate = 0;

        private Cargo _carryingCargo = null;

        private Route _currentRoute = null;

        private IDestination _currentDestination;

        private readonly IDestination _origin;

        public int Id { get; }

        public TransportKind Kind => TransportKind.Truck;

        public bool IsAvailableAt(IDestination destination)
        {
            if (IsOnRoute)
                return false;

            return _currentDestination == destination;
        }

        public Truck(int id) : this(id, Destination.Factory)
        { }

        public Truck(int id, IDestination originDestination)
        {
            Id = id;
            _currentDestination = originDestination;
            _origin = originDestination;
        }

        public void Load(Cargo cargo, Route route)
        {
            _currentRouteEstimate = route.TimeEstimate;

            _carryingCargo = cargo;

            _currentRoute = route;
        }

        public Cargo Unload()
        {
            _carryingCargo.DropAt(_currentRoute.End);

            var cargo = _carryingCargo;

            _carryingCargo = null;

            return cargo;
        }

        public void Move(int time)
        {
            if (_currentRouteEstimate == 0)
                return;

            if (_currentRoute.TimeEstimate == _currentRouteEstimate && HasCargo)
                Depart(time);

            _currentRouteEstimate--;

            if (_currentRouteEstimate == 0)
                Arrive(time);
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
                Cargo = new[]
                {
                    new CargoDetails
                    {
                        CargoId = _carryingCargo.Id,
                        Destination = _carryingCargo.TargetDestination.Name.ToUpperInvariant(),
                        Origin = _carryingCargo.Origin.Name.ToUpperInvariant()
                    }
                }
            };

            Debug.WriteLine(transportDepartedEvent.ToString());
        }

        private void Arrive(int time)
        {
            var transportArrivedEvent = new TransportArrivedEvent
            {
                Time = time,
                TransportId = Id,
                Kind = Kind.ToString().ToUpperInvariant(),
                Location = _currentRoute.End.Name.ToUpperInvariant(),
                Cargo = new[]
                {
                    new CargoDetails
                    {
                        CargoId = _carryingCargo.Id,
                        Destination = _carryingCargo.TargetDestination.Name.ToUpperInvariant(),
                        Origin = _carryingCargo.Origin.Name.ToUpperInvariant()
                    }
                }
            };

            Debug.WriteLine(transportArrivedEvent.ToString());

            _currentDestination = _currentRoute.End;

            Unload();

            if (_origin != _currentDestination)
                Return();
        }

        private void Return()
        {
            var returnRoute = _currentRoute.GetReturnRoute();

            _currentRoute = returnRoute;

            _currentRouteEstimate = returnRoute.TimeEstimate;
        }

        private bool HasCargo => _carryingCargo is null;

        private bool IsOnRoute => _currentRoute is null;
    }
}