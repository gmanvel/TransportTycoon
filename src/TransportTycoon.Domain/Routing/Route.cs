namespace TransportTycoon.Domain.Routing
{
    public class Route
    {
        public IDestination Start { get; }

        public IDestination End { get; }

        public TransportKind TransportKind { get; }

        public int TimeEstimate { get; }

        public Route(IDestination start, IDestination end, TransportKind transportKind, int timeEstimate)
        {
            Start = start;
            End = end;
            TransportKind = transportKind;
            TimeEstimate = timeEstimate;
        }
    }
}