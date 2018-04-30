using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Elton.Aqara
{
    partial class AqaraClient
    {
        protected virtual void ProcessMessage(string remoteAddress, string jsonString, DateTime timestamp)
        {
            AqaraGateway gateway = null;
            foreach(var item in dicGateways.Values)
            {
                if(item?.EndPoint?.Address.ToString() == remoteAddress)
                {
                    gateway = item;
                    break;
                }
            }
            
            dynamic content = JsonConvert.DeserializeObject(jsonString);

            string cmd = content.cmd;
            switch (cmd)
            {
                case "iam": ProcessMessage_IAM(gateway, content, timestamp); break;
                case "get_id_list_ack": ProcessMessage_GetIdListAck(gateway, content, timestamp); break;
                case "heartbeat": ProcessMessage_Heartbeat(gateway, content, timestamp); break;
                case "report": ProcessMessage_Report(gateway, content, timestamp); break;
                case "read_ack": ProcessMessage_ReadAck(gateway, content, timestamp); break;
                case "write_ack": break;
            }
        }

        void ProcessMessage_IAM(AqaraGateway gateway, dynamic message, DateTime timestamp)
        {
            string cmd = message.cmd;
            if (cmd != "iam")
                return;
            int port = message.port;
            string sid = message.sid;
            string model = message.model;
            string ip = message.ip;

            if (gateway !=null && gateway?.Id != sid)
                throw new Exception();
            if (!dicGateways.ContainsKey(sid))
                return;
            gateway = dicGateways[sid];
            gateway.UpdateEndPoint(ip, port);

            //{"cmd" : "get_id_list"}
            SendCommand(gateway, "{\"cmd\" : \"get_id_list\"}");
        }

        void ProcessMessage_GetIdListAck(AqaraGateway gateway, dynamic message, DateTime timestamp)
        {
            string cmd = message.cmd;
            if (cmd != "get_id_list_ack")
                return;
            string gateway_sid = message.sid;
            string token = message.token;
            string jsonString = message.data;

            if (gateway != null && gateway?.Id != gateway_sid)
                throw new Exception();
            if (!dicGateways.ContainsKey(gateway_sid))
                return;
            gateway = dicGateways[gateway_sid];
            gateway.UpdateToken(token);

            List<string> list = new List<string>();
            dynamic data = JsonConvert.DeserializeObject(jsonString);
            foreach (string sid in data)
            {
                var deviceId = sid;
                
                if(!gateway.Devices.ContainsKey(deviceId))
                {
                    AqaraDevice device = new AqaraDevice(this, gateway, sid, null);
                    gateway.Devices.Add(deviceId, device);

                    log.Info($"GATEWAY[{device.Gateway.Id}] device added: sid='{device.Id}'.");
                }

                SendCommand(gateway, string.Format("{{\"cmd\" : \"read\", \"sid\": \"{0}\"}}", sid));
            }
        }

        void ProcessMessage_Heartbeat(AqaraGateway gateway, dynamic message, DateTime timestamp)
        {
            string cmd = message.cmd;
            if (cmd != "heartbeat")
                return;
            string sid = message.sid;
            string model = message.model;
            long short_id = message.short_id;
            string jsonString = message.data;
            if (model == "gateway")
            {
                if (gateway != null && gateway?.Id != sid)
                    throw new Exception();
                if (!dicGateways.ContainsKey(sid))
                    return;//如果没有配置密钥，则无需理会
                gateway = dicGateways[sid];

                string token = message.token;
                string ip = null;
                dynamic data = JsonConvert.DeserializeObject(jsonString);
                foreach (var item in data)
                {
                    string key = item.Name;
                    string value = item.Value;
                    if (key == "ip")
                        ip = value;
                }

                gateway.Update(ip, token);
            }
            else
            {//子设备心跳
                UpdateDeviceHeartbeatReceived(gateway, sid, model, short_id, jsonString);
            }
        }

        void UpdateDeviceData(AqaraGateway gateway, string sid, string model, long shortId, string jsonString)
        {
            if (!gateway.Devices.ContainsKey(sid))
                return;
            var device = gateway.Devices[sid];
            device.Update(model, shortId);
            var listChanged = device.UpdateData(jsonString);

            foreach (var args in listChanged)
            {
                if (DeviceStateChanged != null)
                    DeviceStateChanged(device, args);
            }
        }
        void UpdateDeviceHeartbeatReceived(AqaraGateway gateway, string sid, string model, long shortId, string jsonString)
        {
            if (!gateway.Devices.ContainsKey(sid))
                return;
            var device = gateway.Devices[sid];
            device.Update(model, shortId);
            device.UpdateData(jsonString);

            if (HeartbeatReceived != null)
                HeartbeatReceived(device, EventArgs.Empty);
        }

        public event EventHandler<StateChangedEventArgs> DeviceStateChanged;
        public event EventHandler HeartbeatReceived;

        void ProcessMessage_Report(AqaraGateway gateway, dynamic message, DateTime timestamp)
        {
            string cmd = message.cmd;
            if (cmd != "report")
                return;
            string sid = message.sid;
            string model = message.model;
            long short_id = message.short_id;
            string token = message.token;
            string jsonString = message.data;

            UpdateDeviceData(gateway, sid, model, short_id, jsonString);
        }

        void ProcessMessage_ReadAck(AqaraGateway gateway, dynamic message, DateTime timestamp)
        {
            string cmd = message.cmd;
            if (cmd != "read_ack")
                return;
            string sid = message.sid;
            string model = message.model;
            long short_id = message.short_id;
            string jsonString = message.data;

            if (!gateway.Devices.ContainsKey(sid))
                return;
            var deviceId = sid;
            if (!gateway.Devices.ContainsKey(deviceId))
                return;

            AqaraDevice device = gateway.Devices[deviceId] as AqaraDevice;
            device.Update(model, short_id);
            switch (model)
            {
                case "magnet"://a.窗磁传感器
                    break;
                case "motion"://人体传感器
                    break;
                case "switch"://无线开关传感器
                    if (!(device is SwitchDevice))
                    {
                        device = new SwitchDevice(this, device.Gateway, device.Id, device.Config);
                        gateway.Devices[deviceId] = device;
                        device.Update(model, short_id);
                    }
                    break;
                case "plug"://智能插座
                    if (!(device is PlugDevice))
                    {
                        device = new PlugDevice(this, device.Gateway, device.Id, device.Config);
                        gateway.Devices[deviceId] = device;
                        device.Update(model, short_id);
                    }
                    break;
                case "ctrl_neutral1"://单火开关单键
                    break;
                case "ctrl_neutral2"://单火开关双键
                    if (!(device is CtrlNeutral2Device))
                    {
                        device = new CtrlNeutral2Device(this, device.Gateway, device.Id, device.Config);
                        gateway.Devices[deviceId] = device;
                        device.Update(model, short_id);
                    }
                    break;
                case "86sw1"://无线开关单键
                    break;
                case "86sw2"://无线开关双键
                    break;
                case "sensor_ht"://温湿度传感器
                    break;
                case "rgbw_light"://j.LUMI.LIGHT.RGBW
                    break;
                default:
                    device.Update(model, short_id);
                    break;
            }

            log.Info($"GATEWAY[{device.Gateway.Id}] device updated: sid='{device.Id}' model='{device.Model}' shortId='{device.ShortId}'.");

            dynamic data = JsonConvert.DeserializeObject(jsonString);
            foreach (var item in data)
            {
                string key = item.Name;
                string value = item.Value;

                if (!device.States.ContainsKey(key))
                    device.States.Add(key, new DeviceState(key));

                device.States[key].SetValue(value);
            }
        }
    }
}
