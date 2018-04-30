using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elton.Aqara
{
    /// <summary>
    /// switch 无线开关传感器
    /// </summary>
    public class SwitchDevice : AqaraDevice
    {
        public SwitchDevice(AqaraClient connector, AqaraGateway gateway, string sid, AqaraDeviceConfig config)
            : base(connector, gateway, sid, config)
        {
        }
        
        protected void Write(string status)
        {
            //{"cmd":"write","model":"switch","sid":"112316","short_id":4343,"data":"{\"status\":\"click\"}" }

            var dic = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            dic.Add("status", "on");
            connector.SendWriteCommand(this, dic);
        }

        public void Click()
        {
            Write("click");
        }

        public void DoubleClick()
        {
            Write("double_click");
        }

        public void LongClickPress()
        {
            Write("long_click_press");
        }
    }
}
