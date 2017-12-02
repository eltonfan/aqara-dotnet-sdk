using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elton.Aqara
{
    public class AqaraDevice
    {
        protected readonly AqaraClient connector = null;
        protected readonly AqaraGateway gateway = null;

        readonly Dictionary<string, DeviceState> dicStates = new Dictionary<string, DeviceState>(StringComparer.OrdinalIgnoreCase);

        DateTime latestTimestamp = DateTime.MinValue;
        public string Id { get; private set; }
        public UInt16 ShortId { get; private set; }
        public AqaraDevice(AqaraClient connector, AqaraGateway gateway, string sid)
        {
            this.connector = connector;
            this.gateway = gateway;
            this.Id = sid;
        }

        public string Name
        {
            get
            {
                AqaraDeviceConfig config = gateway.GetDeviceInformation(this.Id);
                if (config == null)
                    return name;

                return config.Name;
            }
            set { name = value; }
        }

        public string ModelName => modelName;

        public string Description
        {
            get
            {
                AqaraDeviceConfig config = gateway.GetDeviceInformation(this.Id);
                if (config == null)
                    return description;

                return config.Model;
            }
            set { description = value; }
        }

        string name = null;
        string modelName = null;
        string description = null;
        public void Update(string modelName, long short_id)
        {
            this.modelName = modelName;
            if (short_id > UInt16.MaxValue)
                throw new ArgumentOutOfRangeException("short_id 值比 UInt16 大。");
            ShortId = (UInt16)short_id;

            latestTimestamp = DateTime.Now;
        }

        public AqaraGateway Gateway
        {
            get { return gateway; }
        }

        /// <summary>
        /// 设备属性。
        /// </summary>
        public Dictionary<string, DeviceState> States
        {
            get { return dicStates; }
        }

        public DateTime LatestTimestamp
        {
            get { return latestTimestamp; }
        }
    }
}
