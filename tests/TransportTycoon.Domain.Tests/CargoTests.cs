using System;
using System.Collections.Generic;
using Xunit;

namespace TransportTycoon.Domain.Tests
{
    public class CargoTests
    {
        [Fact]
        public void Cargo_Origin_Is_Factory()
        {
            var cargo = new Cargo(1, Destination.A);

            Assert.True(Destination.IsFactory(cargo.Origin));
        }

        [Fact]
        public void Cargo_CurrentDestination_Upon_Creation_Is_Factory()
        {
            var cargo = new Cargo(1, Destination.A);

            Assert.True(Destination.IsFactory(cargo.CurrentDestination));
        }

        [Fact]
        public void Cargo_IsDelivered_Upon_Creation_Is_False()
        {
            var cargo = new Cargo(1, Destination.A);

            Assert.False(cargo.IsDelivered);
        }

        public static IEnumerable<object[]> CargoTargetDestinations()
        {
            yield return new object[]
            {
                Destination.Factory,
                true
            };

            yield return new object[]
            {
                Destination.Port,
                true
            };

            yield return new object[]
            {
                Destination.A,
                false
            };

            yield return new object[]
            {
                Destination.B,
                false
            };
        }

        [Theory]
        [MemberData(nameof(CargoTargetDestinations))]
        public void Cargo_TargetDestination_Can_Only_Be_A_Or_B(IDestination targetDestination, bool throws)
        {
            if (throws)
            {
                Assert.Throws<ArgumentException>(() => new Cargo(1, targetDestination));
            }
            else
            {
                var cargo = new Cargo(1, targetDestination);

                Assert.NotNull(cargo);
            }
        }

        [Fact]
        public void Cargo_IsDelivered_Becomes_True_If_Cargo_Dropped_At_Target_Destination()
        {
            var targetDestination = Destination.B;

            var cargo = new Cargo(1, targetDestination);

            cargo.DropAt(targetDestination);

            Assert.True(cargo.IsDelivered);
        }
    }
}