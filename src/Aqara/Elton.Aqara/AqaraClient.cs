using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Elton.Aqara
{
    public partial class AqaraClient
    {
        static readonly Common.Logging.ILog log = Common.Logging.LogManager.GetLogger(typeof(AqaraClient));

        const int LOCAL_PORT = 9898;
        readonly Encoding encoding = Encoding.UTF8;
        readonly Dictionary<string, AqaraGateway> dicGateways = null;
        public AqaraClient(AqaraConfig config)
        {
            dicGateways = new Dictionary<string, AqaraGateway>(StringComparer.OrdinalIgnoreCase);
            if (config.Gateways != null)
            {
                foreach (var gateway in config.Gateways)
                {
                    if (dicGateways.ContainsKey(gateway.MacAddress))
                        continue;

                    var sid = gateway.MacAddress.Replace(":", "").ToLower();
                    AqaraGateway entry = new AqaraGateway(this, sid, gateway.Password, gateway.Devices);
                    dicGateways.Add(entry.Id, entry);
                }
            }
        }

        /// <summary>
        /// AES-CBC 128 加密。
        /// </summary>
        /// <param name="password"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string Encrypt(string password, string token)
        {
            byte[] keyBytes = Encoding.ASCII.GetBytes(password.PadRight(16));
            byte[] dataBytes = Encoding.ASCII.GetBytes(token.PadRight(16));
            byte[] encryptedBytes = null;

            using (var aes = Aes.Create())
            {
                using (ICryptoTransform encryptor = aes.CreateEncryptor(keyBytes, Consts.AES_KEY_IV))
                {
                    encryptedBytes = new byte[16];
                    int length = encryptor.TransformBlock(dataBytes, 0, dataBytes.Length, encryptedBytes, 0);
                    Debug.Assert(length == 16, "The encrypted result must be 16 bytes.");
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (byte item in encryptedBytes)
                sb.Append(item.ToString("X2"));

            return sb.ToString();
        }

        UdpClient client = null;
        public void SendDiscover(UdpClient client)
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(Consts.MulticastAddress), Consts.DiscoveryPort);

            // {"cmd":"whois"}
            Byte[] buffer = encoding.GetBytes("{\"cmd\":\"whois\"}");
            client.SendAsync(buffer, buffer.Length, remoteEP);
        }

        /// <summary>
        /// 发送指令到网关。
        /// </summary>
        /// <param name="content"></param>
        public void SendCommand(AqaraGateway gateway, string content)
        {
            byte[] data = encoding.GetBytes(content);
            this.client.SendAsync(data, data.Length, gateway.EndPoint);
        }


        bool cancellationPending = false;
        public bool CancellationPending => cancellationPending;

        public void DoWork(object state)
        {
            this.client = new UdpClient();
            client.ExclusiveAddressUse = false;
            IPEndPoint localEp = new IPEndPoint(IPAddress.Any, LOCAL_PORT);

            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            client.ExclusiveAddressUse = false;

            client.Client.Bind(localEp);

            client.JoinMulticastGroup(IPAddress.Parse(Consts.MulticastAddress), 50);

            SendDiscover(client);

            while (true)
            {
                if (CancellationPending)
                    break;

                try
                {
                    UdpReceiveResult result = client.ReceiveAsync().Result;
                    Byte[] data = result.Buffer;
                    DateTime timestamp = DateTime.Now;
                    var remoteAddress = result.RemoteEndPoint.Address.ToString();

                    string jsonString = Encoding.UTF8.GetString(data);
                    log.Debug($"Received: {jsonString}");
                    ProcessMessage(remoteAddress, jsonString, timestamp);
                }
                catch(Exception ex)
                {
                    log.Error("client.ReceiveAsync.", ex);
                }
            }

            client.DropMulticastGroup(IPAddress.Parse(Consts.MulticastAddress));

            client.Dispose();
        }

        public void SendWriteCommand(AqaraDevice device, Dictionary<string, string> arguments)
        {
            AqaraGateway gateway = device.Gateway;

            arguments.Add("key", Encrypt(gateway.Password, gateway.Token));
            string dataString = JsonConvert.SerializeObject(arguments);

            dynamic message = new {
                cmd = "write",
                model = device.Model?.Name,
                sid = device.Id,
                short_id = device.ShortId,
                data = dataString,
            };

            string jsonString = JsonConvert.SerializeObject(message);
            this.SendCommand(gateway, jsonString);
        }

        public Dictionary<string, AqaraGateway> Gateways => dicGateways;
    }
}
