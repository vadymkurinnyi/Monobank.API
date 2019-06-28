using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MonoBank.Api.Types
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class UserInfo
    {
        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty(Required = Required.Always)]
        public UserAccount[] Accounts { get; set; }
    }
}
