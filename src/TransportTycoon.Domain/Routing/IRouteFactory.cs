﻿namespace TransportTycoon.Domain.Routing
{
    public interface IRouteFactory
    {
        Route Create(IDestination start, IDestination end);
    }
}