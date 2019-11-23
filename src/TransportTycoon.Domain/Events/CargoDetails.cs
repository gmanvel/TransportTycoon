using Newtonsoft.Json;

namespace TransportTycoon.Domain.Events
{
    public class CargoDetails
    {
        [JsonProperty("cargo_id")]
        public int CargoId { get; set; }

        [JsonProperty("destination")]
        public string Destination { get; set; }

        [JsonProperty("origin")]
        public string Origin { get; set; }
    }
}