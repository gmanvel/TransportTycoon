using TransportTycoon.Domain.Routing;

namespace TransportTycoon.Domain.Transport
{
    public interface ITransport
    {
        int Id { get; }

        TransportKind Kind { get; }

        bool IsAvailableAt(IDestination destination);

        void Move(int time);

        void Load(Cargo cargo, Route route);

        Cargo Unload();
    }
}