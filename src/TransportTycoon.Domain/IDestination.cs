using System.Collections;
using System.Collections.Generic;

namespace TransportTycoon.Domain
{
    public interface IDestination
    {
        string Name { get; }

        void StoreCargo(Cargo cargo);

        IEnumerable<Cargo> GetCargoes();
    }
}
