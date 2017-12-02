using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elton.Aqara
{
    public class DeviceState
    {
        readonly string name = null;
        volatile bool isUnknown = true;
        public DeviceState(string name)
        {
            this.name = name;
            this.isUnknown = true;
        }

        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// 数据未知，尚未初始化。
        /// </summary>
        public bool IsUnknown
        {
            get { return this.isUnknown; }
        }

        public string Value { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime LastModified { get; private set; }


        public void SetValue(string value)
        {
            this.Value = value;
            this.isUnknown = false;
            this.LastModified = DateTime.UtcNow;
        }
        public void SetValue(string value, DateTime timestamp)
        {
            this.Value = value;
            this.isUnknown = false;
            this.LastModified = timestamp;
        }
    }
}
