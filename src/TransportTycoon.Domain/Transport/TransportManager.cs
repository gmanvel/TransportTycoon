using System.Collections.Generic;
using System.Linq;
using TransportTycoon.Domain.Infrastructure;

namespace TransportTycoon.Domain.Transport
{
    public class TransportManager : ITransportManager
    {
        private static List<ITransport> _availableTransport;

        public TransportManager()
        {
            SetupTransport();
        }

        private void SetupTransport()
        {
            var truck1Id = SequentialIdGenerator.GenerateIdFor(SequentialIdGenerator.Entity.Truck);
            var truck1 = new Truck(truck1Id);

            var truck2Id = SequentialIdGenerator.GenerateIdFor(SequentialIdGenerator.Entity.Truck);
            var truck2 = new Truck(truck2Id);

            var ship1Id = SequentialIdGenerator.GenerateIdFor(SequentialIdGenerator.Entity.Ship);
            var ship1 = new Ship(ship1Id);

            _availableTransport = new List<ITransport> { truck1, truck2, ship1 };
        }

        public void OnTick(int time) => _availableTransport.ForEach(transport => transport.OnTick(time));

        public ITransport GetTransportAt(IDestination destination, TransportKind transportKind) =>
            _availableTransport.FirstOrDefault(transport =>
            {
                var transportKindMatch = transport.Kind == transportKind;
                var transportAvailableAtMatch = transport.IsAvailableAt(destination);
                return transportKindMatch && transportAvailableAtMatch;
            });
    }
}