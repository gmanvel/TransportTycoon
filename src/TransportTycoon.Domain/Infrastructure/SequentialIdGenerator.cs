using System.Collections.Generic;

namespace TransportTycoon.Domain.Infrastructure
{
    public static class SequentialIdGenerator
    {
        private static Dictionary<Entity, int> LastIdMap { get; }
            = new Dictionary<Entity, int>
            {
                [Entity.Truck] = 0,
                [Entity.Ship] = 0,
                [Entity.Cargo] = 0
            };


        public static int GenerateIdFor(Entity entity)
        {
            var lastGeneratedId = LastIdMap[entity];

            lastGeneratedId++;

            LastIdMap[entity] = lastGeneratedId;

            return lastGeneratedId;
        }

        public enum Entity
        {
            Truck,
            Ship, 
            Cargo
        }

    }
}