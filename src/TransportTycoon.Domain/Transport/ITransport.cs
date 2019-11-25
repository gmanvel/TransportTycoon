using System.Collections.Generic;
using TransportTycoon.Domain.Routing;

namespace TransportTycoon.Domain.Transport
{
    public interface ITransport
    {
        int Id { get; }

        TransportKind Kind { get; }

        bool IsAvailableAt(IDestination destination);

        void PlanDelivery(IEnumerable<Cargo> cargoes, Route route);

        void Tick(int time);
    }
}