using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elton.Aqara
{
    public class AqaraDeviceConfig
    {
        [JsonProperty("model")]
        public string Model { get; set; }
        [JsonProperty("did")]
        public string DeviceId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class AqaraGatewayConfig
    {
        /// <summary>
        /// 网关的Mac地址
        /// </summary>
        [JsonProperty("mac")]
        public string GatewayMacAddress { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("devices")]
        public AqaraDeviceConfig[] Devices { get; set; }
    }

    public class AqaraConfig
    {
        [JsonProperty("gateways")]
        public AqaraGatewayConfig[] Gateways { get; set; }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
        public static AqaraConfig Parse(string json)
        {
            return JsonConvert.DeserializeObject<AqaraConfig>(json);
        }
    }
}
