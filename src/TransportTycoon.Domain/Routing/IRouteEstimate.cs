namespace TransportTycoon.Domain.Routing
{
    public interface IRouteEstimate
    {
        RouteBuilder WithEstimate(int estimate);
    }
}