using System;

namespace TransportTycoon.Domain
{
    public class Cargo
    {
        public int Id { get; }

        public bool IsDelivered { get; private set; }

        public IDestination TargetDestination { get; }

        public IDestination CurrentDestination { get; private set; }

        public IDestination Origin { get; }

        public Cargo(int id, IDestination targetDestination)
        {
            if (!Destination.IsA(targetDestination)
                &&
                !Destination.IsB(targetDestination))
                throw new ArgumentException("Cargo can only be delivered to A or B");

            Id = id;
            Origin = Destination.Factory;
            CurrentDestination = Destination.Factory;
            TargetDestination = targetDestination;
        }

        public void DropAt(IDestination destination)
        {
            CurrentDestination = destination;
            IsDelivered = CurrentDestination == TargetDestination;
        }
    }
}