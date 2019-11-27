using System.Collections.Generic;
using Newtonsoft.Json;

namespace TransportTycoon.Domain.Events
{
    public class CargoLoadedEvent
    {
        [JsonProperty("event")]
        public string Event => "LOAD";

        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("transport_id")]
        public int TransportId { get; set; }

        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("cargo", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<CargoDetails> Cargo { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}