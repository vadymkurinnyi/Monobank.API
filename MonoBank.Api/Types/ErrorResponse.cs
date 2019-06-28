using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MonoBank.Api.Types
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class ErrorResponse
    {
        [JsonProperty(Required = Required.Always)]
        public string ErrorDescription { get; set; }
    }
}
