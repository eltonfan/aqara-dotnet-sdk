using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace Elton.Aqara
{
    /// <summary>
    /// 小米网关模拟器。
    /// 功能：
    /// 1. 网关发现： 监听 4321端口，并单播响应IP信息
    /// 2. 子设备列表、命令等，监听 9898 端口，并单播响应。
    /// 3. 网关心跳、子设备心跳、组播到 224.0.0.50:9898
    /// </summary>
    public class AqaraGatewayServer
    {
        static readonly Common.Logging.ILog log = Common.Logging.LogManager.GetLogger(typeof(AqaraGatewayServer));

        const string MULTICAST_ADDRESS = "224.0.0.50";
        const int DISCOVERY_PORT = 4321;
        const int LOCAL_PORT = 9898;
        readonly Encoding encoding = Encoding.UTF8;

        /// <summary>
        /// 以组播方式发送数据。
        /// </summary>
        /// <param name="jsonString"></param>
        public void SendMulticast(string jsonString)
        {
            var remoteEP = new IPEndPoint(IPAddress.Parse(MULTICAST_ADDRESS), LOCAL_PORT);
            var buffer = encoding.GetBytes(jsonString);
            clientDiscovery.SendAsync(buffer, buffer.Length, remoteEP);
        }

        /// <summary>
        /// 以单播方式响应指令。
        /// </summary>
        /// <param name="remoteEP"></param>
        /// <param name="jsonString"></param>
        public void SendAck(IPEndPoint remoteEP, string jsonString)
        {
            var buffer = encoding.GetBytes(jsonString);
            clientDiscovery.SendAsync(buffer, buffer.Length, remoteEP);
        }


        UdpClient clientGateway = null;
        UdpClient clientDiscovery = null;

        bool cancellationPending = false;
        public bool CancellationPending => cancellationPending;

        public void DoWork(object state)
        {
            this.clientDiscovery = new UdpClient();
            clientDiscovery.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            clientDiscovery.ExclusiveAddressUse = false;
            clientDiscovery.Client.Bind(new IPEndPoint(IPAddress.Any, DISCOVERY_PORT));
            clientDiscovery.JoinMulticastGroup(IPAddress.Parse(MULTICAST_ADDRESS), 50);

            this.clientGateway = new UdpClient();
            clientGateway.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            clientGateway.ExclusiveAddressUse = false;
            clientGateway.Client.Bind(new IPEndPoint(IPAddress.Any, LOCAL_PORT));
            clientGateway.JoinMulticastGroup(IPAddress.Parse(MULTICAST_ADDRESS), 50);

            Task<UdpReceiveResult> taskDiscovery = null;
            Task<UdpReceiveResult> taskGateway = null;
            while (true)
            {
                if (CancellationPending)
                    break;

                try
                {
                    if (taskDiscovery == null || taskDiscovery.Status == TaskStatus.RanToCompletion)
                    {
                        taskDiscovery = clientDiscovery.ReceiveAsync();
                        taskDiscovery.ContinueWith(t =>
                            {
                                var result = t.Result;

                                var remoteEndPoint = result.RemoteEndPoint;
                                Byte[] data = result.Buffer;
                                DateTime timestamp = DateTime.Now;

                                string jsonString = Encoding.UTF8.GetString(data);
                                log.DebugFormat("Received: {0}", jsonString);

                                ProcessMessage(remoteEndPoint, jsonString, timestamp);
                            });
                    }

                    if (taskGateway == null || taskGateway.Status == TaskStatus.RanToCompletion)
                    {
                        taskGateway = clientGateway.ReceiveAsync();
                        taskGateway.ContinueWith(t =>
                        {
                            var result = t.Result;

                            var remoteEndPoint = result.RemoteEndPoint;
                            Byte[] data = result.Buffer;
                            DateTime timestamp = DateTime.Now;

                            string jsonString = Encoding.UTF8.GetString(data);
                            log.DebugFormat("Received: {0}", jsonString);

                            ProcessMessage(remoteEndPoint, jsonString, timestamp);
                        });
                    }
                }
                catch (Exception ex)
                {
                    log.Error("client.ReceiveAsync.", ex);
                }
            }

            clientDiscovery.DropMulticastGroup(IPAddress.Parse(MULTICAST_ADDRESS));

            clientDiscovery.Dispose();
        }

        void ProcessMessage(IPEndPoint remoteEP, string jsonString, DateTime timestamp)
        {
            // "{\"cmd\":\"whois\"}"
            if (jsonString == "{\"cmd\":\"whois\"}")
            {
                dynamic response = new
                {
                    cmd= "iam",
                    sid = "324242",
                    ip = "192.168.0.42",
                    port = "9898",
                    model = "gateway",
                };

                Byte[] buffer = encoding.GetBytes(JsonConvert.SerializeObject(response));
                clientDiscovery.SendAsync(buffer, buffer.Length, remoteEP);
            }
        }

        /*
        public void SendWriteCommand(AqaraDevice device, Dictionary<string, string> arguments)
        {
            AqaraGateway gateway = device.Gateway;

            arguments.Add("key", Encrypt(gateway.Password, gateway.Token));
            string dataString = JsonConvert.SerializeObject(arguments);

            dynamic message = new
            {
                cmd = "write",
                model = device.ModelName,
                sid = device.SystemId,
                short_id = device.ShortId,
                data = dataString,
            };

            string jsonString = JsonConvert.SerializeObject(message);
            this.SendCommand(gateway, jsonString);
        }*/
    }
}
