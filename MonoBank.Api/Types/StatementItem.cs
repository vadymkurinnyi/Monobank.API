using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MonoBank.Api.Types
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class StatementItem
    {
        [JsonProperty(Required = Required.Always)]

        public string Id { get; set; }
        [JsonProperty(Required = Required.Always)]

        public int Time { get; set; }
        [JsonProperty(Required = Required.Always)]

        public string Description { get; set; }
        [JsonProperty(Required = Required.Always)]

        public int Mcc { get; set; }
        [JsonProperty(Required = Required.Always)]

        public bool Hold { get; set; }
        [JsonProperty(Required = Required.Always)]

        public long Amount { get; set; }
        [JsonProperty(Required = Required.Always)]

        public long OperationAmount { get; set; }
        [JsonProperty(Required = Required.Always)]

        public int CurrencyCode { get; set; }
        [JsonProperty(Required = Required.Always)]

        public long CommissionRate { get; set; }
        [JsonProperty(Required = Required.Always)]

        public long CashbackAmount { get; set; }
        [JsonProperty(Required = Required.Always)]

        public long Balance { get; set; }
    }
}
