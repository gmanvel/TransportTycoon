using System;

namespace TransportTycoon.Domain
{
    public abstract class Destination
    {
        public static IDestination FromString(string destination)
        {
            if (string.Equals(destination, "A", StringComparison.OrdinalIgnoreCase))
                return A;

            if (string.Equals("B", destination, StringComparison.OrdinalIgnoreCase))
                return B;

            throw new ArgumentException($"Can't map <{destination}> string to destination.");
        }

        public static bool IsA(IDestination destination) => destination is ADestination;

        public static IDestination A => ADestination.Instance;

        private class ADestination : IDestination
        {
            private ADestination() { }

            public static ADestination Instance { get; } = new ADestination();

            public string Name => "A";
        }

        public static bool IsB(IDestination destination) => destination is BDestination;

        public static IDestination B => BDestination.Instance;

        private class BDestination : IDestination
        {
            private BDestination() { }

            public static BDestination Instance { get; } = new BDestination();

            public string Name => "B";
        }

        public static bool IsFactory(IDestination destination) => destination is FactoryDestination;

        public static IDestination Factory => FactoryDestination.Instance;

        private class FactoryDestination : IDestination
        {
            private FactoryDestination() { }

            public static FactoryDestination Instance { get; } = new FactoryDestination();

            public string Name => "FACTORY";
        }

        public static bool IsPort(IDestination destination) => destination is PortDestination;

        public static IDestination Port => PortDestination.Instance;

        private class PortDestination : IDestination
        {
            private PortDestination() { }

            public static PortDestination Instance { get; } = new PortDestination();

            public string Name => "PORT";
        }
    }
}
