using System;
using System.Collections.Generic;
using System.Linq;

namespace TransportTycoon.Domain
{
    public abstract class Destination
    {
        protected readonly List<Cargo> _cargoStore;

        protected Destination(List<Cargo> cargoStore)
        {
            _cargoStore = cargoStore;
        }

        public static IDestination FromString(string destination)
        {
            if (string.Equals(destination, "A", StringComparison.OrdinalIgnoreCase))
                return A;

            if (string.Equals("B", destination, StringComparison.OrdinalIgnoreCase))
                return B;

            throw new ArgumentException($"Can't map <{destination}> string to destination.");
        }

        public virtual void StoreCargo(Cargo cargo)
        {
            if (_cargoStore.Any(c => c.Id == cargo.Id))
                return;

            _cargoStore.Add(cargo);
        }

        public virtual Cargo TakeCargo(int cargoId)
        {
            var cargo = _cargoStore.Find(c => c.Id == cargoId);

            if (cargo != null)
                _cargoStore.Remove(cargo);

            return cargo;
        }

        public IEnumerable<Cargo> TakeCargoes(IEnumerable<int> cargoIds)
        {
            var cargoIdList = cargoIds.ToList();

            var cargoes = _cargoStore.Where(cargo => cargoIdList.Contains(cargo.Id)).ToList();

            _cargoStore.RemoveAll(cargo => cargoIdList.Contains(cargo.Id));

            return cargoes;
        }

        public virtual IEnumerable<Cargo> PeekCargoes() => _cargoStore;

        public static bool IsA(IDestination destination) => destination is ADestination;

        public static IDestination A => ADestination.Instance;

        private class ADestination : Destination, IDestination
        {
            private ADestination() : base(new List<Cargo>())
            { }

            public static ADestination Instance { get; } = new ADestination();

            public string Name => "A";
        }

        public static bool IsB(IDestination destination) => destination is BDestination;

        public static IDestination B => BDestination.Instance;

        private class BDestination : Destination, IDestination
        {
            private BDestination() : base(new List<Cargo>())
            { }

            public static BDestination Instance { get; } = new BDestination();

            public string Name => "B";
        }

        public static bool IsFactory(IDestination destination) => destination is FactoryDestination;

        public static IDestination Factory => FactoryDestination.Instance;

        private class FactoryDestination : Destination, IDestination
        {
            private FactoryDestination() : base(new List<Cargo>())
            { }

            public static FactoryDestination Instance { get; } = new FactoryDestination();

            public string Name => "FACTORY";
        }

        public static bool IsPort(IDestination destination) => destination is PortDestination;

        public static IDestination Port => PortDestination.Instance;

        private class PortDestination : Destination, IDestination
        {
            private PortDestination() : base(new List<Cargo>())
            { }

            public static PortDestination Instance { get; } = new PortDestination();

            public string Name => "PORT";
        }
    }
}
