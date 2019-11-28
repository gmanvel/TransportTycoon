using System;
using System.Collections.Generic;
using System.Linq;
using TransportTycoon.Domain.Delivery;
using TransportTycoon.Domain.Infrastructure;
using TransportTycoon.Domain.Routing;
using TransportTycoon.Domain.Transport;
using Xunit;

namespace TransportTycoon.Domain.Tests
{
    public class Simulation
    {
        private readonly DeliveryManager _deliveryManager;

        public Simulation()
        {
            var routeFactory = Route.Factory;

            var routePlanner = new RoutePlanner(routeFactory);

            var transportManager = new TransportManager();

            _deliveryManager = new DeliveryManager(routePlanner, transportManager);
        }

        [Theory]
        //[InlineData("A", 8)]
        //[InlineData("B", 5)]
        //[InlineData("A,B", 8)]
        //[InlineData("B,B", 5)]
        //[InlineData("A,B,B", 8)]
        //[InlineData("A,A,B,A,B,B,A,B", 34)]
        [InlineData("A,B,B,B,A,B,A,A,A,B,B,B", 41)]
        public void Run(string destinationString, int expectedTime)
        {
            var cargoes = new List<Cargo>();

            var destinations = destinationString.Split(",", StringSplitOptions.RemoveEmptyEntries);

            var factory = Destination.Factory;

            foreach (var destinationName in destinations)
            {
                var destination = Destination.FromString(destinationName);

                var cargoId = SequentialIdGenerator.GenerateIdFor(SequentialIdGenerator.Entity.Cargo);

                var cargo = new Cargo(cargoId, destination);

                factory.StoreCargo(cargo);

                cargoes.Add(cargo);
            }

            _deliveryManager.InitialSetup();

            int time = 1;

            while (!cargoes.All(cargo => cargo.IsDelivered))
            {
                _deliveryManager.Tick(time);

                time++;
            }

            Assert.Equal(expectedTime, time - 1);
        }
    }
}