namespace TransportTycoon.Domain
{
    public interface IDeliveryManager
    {
        void PlanDelivery(Cargo cargo);

        void Tick(int time);
    }
}