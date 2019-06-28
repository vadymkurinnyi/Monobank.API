using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MonoBank.Api.Types
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class UserAccount
    {
        [JsonProperty(Required = Required.Always)]
        public string Id { get; set; }
        [JsonProperty(Required = Required.Always)]
        public long Balance { get; set; }
        [JsonProperty(Required = Required.Always)]
        public long CreditLimit { get; set; }
        [JsonProperty(Required = Required.Always)]
        public int CurrencyCode { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string CashbackType { get; set; }
    }
}
