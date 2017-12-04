using Newtonsoft.Json;
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
        protected readonly AqaraDeviceConfig config = null;

        readonly Dictionary<string, DeviceState> dicStates = new Dictionary<string, DeviceState>(StringComparer.OrdinalIgnoreCase);

        DateTime latestTimestamp = DateTime.MinValue;
        public string Id { get; private set; }
        public UInt16 ShortId { get; private set; }
        public AqaraDevice(AqaraClient connector, AqaraGateway gateway, string sid, AqaraDeviceConfig config)
        {
            this.connector = connector;
            this.gateway = gateway;
            this.Id = sid;
            this.config = config;
        }

        public string Name
        {
            get
            {
                if (config == null)
                    return name;

                return config.Name;
            }
            set { name = value; }
        }

        public DeviceModel Model => model;

        string name = null;
        DeviceModel model = null;
        string description = null;
        public void Update(string modelName, long short_id)
        {
            this.model = DeviceModel.Parse(modelName);
            if (short_id > UInt16.MaxValue)
                throw new ArgumentOutOfRangeException("short_id 值比 UInt16 大。");
            ShortId = (UInt16)short_id;

            latestTimestamp = DateTime.Now;
        }

        public void UpdateData(string jsonString)
        {
            dynamic data = JsonConvert.DeserializeObject(jsonString);
            foreach (var item in data)
            {
                string key = item.Name;
                string value = item.Value;

                if (!dicStates.ContainsKey(key))
                    dicStates.Add(key, new DeviceState(key));

                var state = dicStates[key];
                state.SetValue(value);
            }

            latestTimestamp = DateTime.Now;

            if (StateChanged != null)
                StateChanged(this, EventArgs.Empty);
        }

        public AqaraGateway Gateway
        {
            get { return gateway; }
        }

        public AqaraDeviceConfig Config => config;

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

        public event EventHandler StateChanged;
    }
}
