using TransportTycoon.Domain.Transport;

namespace TransportTycoon.Domain.Routing
{
    public interface IValidateRoute
    {
        bool IsValidRoute(IDestination start, IDestination end);

        bool IsValidTransportTypeForRoute(IDestination start, IDestination end, TransportKind transportKind);
    }
}