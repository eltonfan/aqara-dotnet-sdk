using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Elton.Aqara
{
    /// <summary>
    /// 
    /// </summary>
    public class AqaraGateway
    {
        static readonly Common.Logging.ILog log = Common.Logging.LogManager.GetLogger(typeof(AqaraGateway));

        const int REMOTE_PORT = 9898;

        readonly string sid = null;
        readonly string password = null;
        IPEndPoint endpoint = null;
        string token = null;
        DateTime latestTimestamp = DateTime.MinValue;
        readonly Dictionary<string, AqaraDevice> dicDevices = new Dictionary<string, AqaraDevice>(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// 从配置中载入的设备信息，key为device.sid。
        /// </summary>
        readonly Dictionary<string, AqaraDeviceConfig> dicDeviceInformations = new Dictionary<string, AqaraDeviceConfig>(StringComparer.OrdinalIgnoreCase);
        public AqaraGateway(string sid, string password, AqaraDeviceConfig[] devices)
        {
            this.sid = sid;
            this.password = password;
            if(devices != null)
            {
                foreach(var item in devices)
                {
                    string systemid = item.DeviceId;
                    if (systemid.StartsWith("lumi.", StringComparison.OrdinalIgnoreCase))
                        systemid = systemid.Substring(5);

                    if (dicDeviceInformations.ContainsKey(systemid))
                        dicDeviceInformations[systemid] = item;
                    else
                        dicDeviceInformations.Add(systemid, item);
                }
            }
        }

        public void UpdateEndPoint(string remoteIp, int? port = null)
        {
            IPAddress address;
            if (!IPAddress.TryParse(remoteIp, out address))
            {
                log.ErrorFormat("remoteIp format error. (remoteIp='{0}')", remoteIp);
                return;
            }
            if(port != null && port != REMOTE_PORT)
                log.WarnFormat("The remote port is {0}, but the default port is {1} .", port.Value, REMOTE_PORT);

            bool updated = false;
            if (endpoint == null)
            {
                endpoint = new IPEndPoint(address, (port == null) ? 9898 : port.Value);
                updated = true;
            }
            else
            {
                if (endpoint.Address != address)
                {
                    endpoint.Address = address;
                    updated = true;
                }
                if (port != null && endpoint.Port != port.Value)
                {
                    endpoint.Port = port.Value;
                    updated = true;
                }
            }

            if(updated)
                log.InfoFormat("Gateway endpoint was updated, sid='{0}', endpoint='{1}' .", sid, endpoint);
        }
        public void UpdateToken(string token)
        {
            this.token = token;
            latestTimestamp = DateTime.Now;
        }

        public void Update(string remoteIp, string token)
        {
            UpdateEndPoint(remoteIp, null);
            UpdateToken(token);
        }

        public AqaraDeviceConfig GetDeviceInformation(string sid)
        {
            if (!dicDeviceInformations.ContainsKey(sid))
                return null;
            return dicDeviceInformations[sid];
        }

        public string Id
        {
            get { return this.sid; }
        }

        public string Password
        {
            get { return this.password; }
        }

        /// <summary>
        /// Remote IP Address.
        /// </summary>
        public IPEndPoint EndPoint
        {
            get { return endpoint; }
        }

        public string Token
        {
            get { return token; }
        }

        public DateTime LatestTimestamp
        {
            get { return latestTimestamp; }
        }

        public Dictionary<string, AqaraDevice> Devices => dicDevices;

        public override string ToString()
        {
            return string.Format("GATEWAY[{0}] {1}", sid, EndPoint);
        }
    }
}
