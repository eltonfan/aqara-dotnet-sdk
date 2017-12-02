using System;
using System.Collections.Generic;
using System.Text;

namespace Elton.Aqara
{
    public class DeviceModel
    {
        readonly DeviceModelId id;
        readonly string name;
        readonly string friendlyName;
        readonly string friendlyNameEN;
        public DeviceModel(DeviceModelId id, string name, string friendlyName, string friendlyNameEN)
        {
            this.id = id;
            this.name = name;
            this.friendlyName = friendlyName;
            this.friendlyNameEN = friendlyNameEN;
        }

        public DeviceModelId Id => id;
        public string Name => name;
        public string FriendlyName => friendlyName;

        public override string ToString()
        {
            return $"{friendlyName}({name})";
        }


        static readonly Dictionary<DeviceModelId, DeviceModel> dicModels = new Dictionary<DeviceModelId, DeviceModel>();
        static readonly Dictionary<string, DeviceModel> dicNames = new Dictionary<string, DeviceModel>();
        static void Add(DeviceModelId id, string name, string friendlyName, string friendlyNameEN)
        {
            if (dicModels.ContainsKey(id))
                return;

            var model = new DeviceModel(id, name, friendlyName, friendlyNameEN);
            dicModels.Add(model.id, model);
            dicNames.Add(model.name, model);
        }

        static DeviceModel()
        {
            Add(DeviceModelId.Magnet, "magnet", "门窗传感器", "Xiaomi Door/Window Sensor");
            Add(DeviceModelId.Motion, "motion", "人体传感器", "Xiaomi Mi Motion Sensor");
            Add(DeviceModelId.Switch, "switch", "无线开关传感器", "Xiaomi Mi Wireless Switch");
            Add(DeviceModelId.Plug, "plug", "智能插座", "Xiaomi Mi Smart Socket Plug");
            Add(DeviceModelId.ControlNeutral1, "ctrl_neutral1", "86单火开关单键", "Xiaomi Aqara Wall Switch 1 Button");
            Add(DeviceModelId.ControlNeutral2, "ctrl_neutral2", "86单火开关双键", "Xiaomi Aqara Wall Switch 2 Button");
            Add(DeviceModelId._86Switch1, "86sw1", "86无线开关单键", "Xiaomi Aqara Smart Switch 1 Button");
            Add(DeviceModelId._86Switch2, "86sw2", "86无线开关双键", "Xiaomi Aqara Smart Switch 2 Button");
            Add(DeviceModelId.HT, "sensor_ht", "温湿度传感器", "Xiaomi Mi Temperature & Humidity Sensor");
            Add(DeviceModelId.Cube, "cube", "魔方传感器", "Xiaomi Mi Smart Cube");
            Add(DeviceModelId.Gateway, "gateway", "网关", "Xiaomi Mi Smart Home Gateway");
            Add(DeviceModelId.Ccurtain, "curtain", "窗帘", "Xiaomi Aqara Intelligent Curtain Motor");
            Add(DeviceModelId.ControlLN1, "ctrl_ln1", "86零火墙壁开关单键", "Xiaomi \"zero-fire\" 1 Channel Wall Switch");
            Add(DeviceModelId.ControlLN2, "ctrl_ln2", "86零火墙壁开关双键", "Xiaomi \"zero-fire\" 2 Channel Wall Switch");
            Add(DeviceModelId._86plug, "86plug", "墙壁插座", null);
            Add(DeviceModelId.Natgas, "natgas", "天然气报警器", "Xiaomi Mijia Honeywell Gas Alarm Detector");
            Add(DeviceModelId.Smoke, "smoke", "烟雾报警器", "Xiaomi Mijia Honeywell Fire Alarm Detector");

            Add(DeviceModelId.MagnetAqara2, "sensor_magnet.aq2", "Aqara门窗传感器", "Xiaomi Aqara Door/Window Sensor");
            Add(DeviceModelId.Water, "water", "水浸传感器", "Xiaomi Aqara Water Leak Sensor");
            Add(DeviceModelId.SwitchAqara2, "sensor_switch.aq2", "Aqara无线开关", "Xiaomi Aqara Wireless Switch");
            Add(DeviceModelId.SwitchAqara3, "sensor_switch.aq3", "新版Aqara无线开关", "New Xiaomi Wireless Switch");
            Add(DeviceModelId.WeatherV1, "weather.v1", "Aqara温湿度传感器", "Xiaomi Aqara Temperature, Humidity & Pressure Sensor");
            Add(DeviceModelId.MotionAqara2, "sensor_motion.aq2", "Aqara人体传感器", "Xiaomi Aqara Motion Sensor");
        }

        public static DeviceModel Parse(string name)
        {
            if (dicNames.ContainsKey(name))
                return dicNames[name];

            return null;
        }
        public static string GetFriendlyName(DeviceModelId id)
        {
            if (dicModels.ContainsKey(id))
                return dicModels[id].friendlyName;

            return null;
        }
        public static string GetFriendlyName(string name)
        {
            if (dicNames.ContainsKey(name))
                return dicNames[name].FriendlyName;

            return null;
        }
    }
}
