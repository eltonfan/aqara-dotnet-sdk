using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elton.Aqara
{
    public class AqaraDevice
    {
        protected readonly AqaraConnector connector = null;
        protected readonly AqaraGateway gateway = null;

        readonly Dictionary<string, DeviceState> dicStates = new Dictionary<string, DeviceState>(StringComparer.OrdinalIgnoreCase);

        public string SystemId { get; private set; }
        public long ShortId { get; private set; }
        readonly Guid id;
        public AqaraDevice(AqaraConnector connector, Guid id, AqaraGateway gateway, string sid)
        {
            this.id = id;
            this.connector = connector;
            this.gateway = gateway;
            this.SystemId = sid;
        }

        public string Name
        {
            get
            {
                AqaraDeviceConfig config = gateway.GetDeviceInformation(this.SystemId);
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
                AqaraDeviceConfig config = gateway.GetDeviceInformation(this.SystemId);
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
            ShortId = short_id;
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

        public Guid Id => id;

        public string IdString => id.ToString("N").ToLower();
    }
}
