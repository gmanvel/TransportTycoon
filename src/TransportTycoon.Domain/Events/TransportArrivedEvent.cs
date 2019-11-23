using Newtonsoft.Json;

namespace TransportTycoon.Domain.Events
{
    public class TransportArrivedEvent
    {
        [JsonProperty("event")]
        public string Event => "DEPART";

        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("transport_id")]
        public int TransportId { get; set; }

        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("cargo")]
        public CargoDetails[] Cargo { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}