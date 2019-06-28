using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MonoBank.Api.Types
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class CurrencyInfo
    {
        [JsonProperty(Required = Required.Always)]

        public int CurrencyCodeA { get; set; }
        [JsonProperty(Required = Required.Always)]

        public int CurrencyCodeB { get; set; }
        [JsonProperty(Required = Required.Always)]

        public int Date { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public float RateSell { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public float RateBuy { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public float RateCross { get; set; }
    }
}
