using Xunit;

namespace TransportTycoon.Domain.Tests
{
    public class DestinationTests
    {
        [Fact]
        public void Destination_IsA_Returns_True_IF_Destination_Is_A()
        {
            var destinationA = Destination.A;

            Assert.True(Destination.IsA(destinationA));
        }

        [Fact]
        public void Destination_IsB_Returns_True_If_Destination_Is_B()
        {
            var destinationB = Destination.B;

            Assert.True(Destination.IsB(destinationB));
        }

        [Fact]
        public void Destination_IsFactory_Returns_True_If_Destination_Is_Factory()
        {
            var destinationFactory = Destination.Factory;

            Assert.True(Destination.IsFactory(destinationFactory));
        }

        [Fact]
        public void Destination_IsPort_Returns_True_If_Destination_Is_Port()
        {
            var destinationPort = Destination.Port;

            Assert.True(Destination.IsPort(destinationPort));
        }
    }
}
