using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elton.Aqara
{
    /// <summary>
    /// plug 智能插座
    /// </summary>
    public class PlugDevice : AqaraDevice
    {
        public PlugDevice(AqaraClient connector, AqaraGateway gateway, string sid, AqaraDeviceConfig config)
            : base(connector, gateway, sid, config)
        {
        }
        
        public void TurnOn()
        {
            var dic = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            dic.Add("status", "on");
            connector.SendWriteCommand(this, dic);
        }

        public void TurnOff()
        {
            var dic = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            dic.Add("status", "off");
            connector.SendWriteCommand(this, dic);
        }
    }
}
