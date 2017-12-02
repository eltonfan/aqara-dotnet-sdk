using System;
using System.Collections.Generic;
using System.Text;

namespace Elton.Aqara
{
    public enum DeviceModelId
    {
        /// <summary>
        /// 未知。
        /// </summary>
        Unknown = 0x00,
        /// <summary>
        /// 门窗传感器
        /// </summary>
        Magnet,
        /// <summary>
        /// Aqara门窗传感器
        /// </summary>
        MagnetAqara2,
        /// <summary>
        /// 人体传感器
        /// </summary>
        Motion,
        /// <summary>
        /// Aqara人体传感器
        /// </summary>
        MotionAqara2,
        /// <summary>
        /// 无线开关传感器
        /// </summary>
        Switch,
        /// <summary>
        /// Aqara无线开关
        /// </summary>
        SwitchAqara2,
        /// <summary>
        /// 新版Aqara无线开关
        /// </summary>
        SwitchAqara3,
        /// <summary>
        /// 智能插座
        /// </summary>
        Plug,
        /// <summary>
        /// 86单火开关单键
        /// </summary>
        ControlNeutral1,
        /// <summary>
        /// 86单火开关双键
        /// </summary>
        ControlNeutral2,
        /// <summary>
        /// 86无线开关单键
        /// </summary>
        _86Switch1,
        /// <summary>
        /// 86无线开关双键
        /// </summary>
        _86Switch2,
        /// <summary>
        /// 温湿度传感器
        /// </summary>
        HT,
        /// <summary>
        /// Aqara温湿度传感器
        /// </summary>
        WeatherV1,
        /// <summary>
        /// 魔方传感器
        /// </summary>
        Cube,
        /// <summary>
        /// 网关
        /// </summary>
        Gateway,
        /// <summary>
        /// 窗帘
        /// </summary>
        Ccurtain,
        /// <summary>
        /// 86零火墙壁开关单键
        /// </summary>
        ControlLN1,
        /// <summary>
        /// 86零火墙壁开关双键
        /// </summary>
        ControlLN2,
        /// <summary>
        /// 墙壁插座
        /// </summary>
        _86plug,
        /// <summary>
        /// 天然气报警器
        /// </summary>
        Natgas,
        /// <summary>
        /// 烟雾报警器
        /// </summary>
        Smoke,
        /// <summary>
        /// 水浸传感器
        /// </summary>
        Water,
    }
}
