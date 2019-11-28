namespace TransportTycoon.Domain.Delivery
{
    public interface IDeliveryManager
    {
        void InitialSetup();

        void Tick(int time);
    }
}