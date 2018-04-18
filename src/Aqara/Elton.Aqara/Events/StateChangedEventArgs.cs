using System;
using System.Collections.Generic;
using System.Text;

namespace Elton.Aqara
{
    public class StateChangedEventArgs
    {
        public AqaraDevice Device { get; private set; }
        public string StateName { get; private set; }
        public string OldData { get; private set; }
        public string NewData { get; private set; }
        public StateChangedEventArgs(AqaraDevice device, string stateName, string oldData, string newData)
        {
            this.Device = device;
            this.StateName = stateName;
            this.OldData = oldData;
            this.NewData = newData;
        }
    }
}
