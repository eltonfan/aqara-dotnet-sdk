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
    public partial class AqaraConnector
    {
        static readonly Common.Logging.ILog log = Common.Logging.LogManager.GetLogger(typeof(AqaraConnector));

        const string MULTICAST_ADDRESS = "224.0.0.50";
        const int DISCOVERY_PORT = 4321;
        const int LOCAL_PORT = 9898;
        readonly Dictionary<string, Guid> dicIds = new Dictionary<string, Guid>(StringComparer.OrdinalIgnoreCase);
        readonly Dictionary<string, AqaraGateway> dicGateways = new Dictionary<string, AqaraGateway>(StringComparer.OrdinalIgnoreCase);
        readonly Encoding encoding = Encoding.UTF8;
        public Guid Id { get; private set; }
        protected readonly Dictionary<Guid, AqaraDevice> dicDevices = new Dictionary<Guid, AqaraDevice>();
        public AqaraConnector(Guid id)
        { }

        /// <summary>
        /// AES-CBC 128 初始向量
        /// </summary>
        static readonly byte[] AES_KEY_IV = new byte[] {
            0x17, 0x99, 0x6d, 0x09, 0x3d, 0x28, 0xdd, 0xb3, 0xba, 0x69, 0x5a, 0x2e, 0x6f, 0x58, 0x56, 0x2e
        };

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
                using (ICryptoTransform encryptor = aes.CreateEncryptor(keyBytes, AES_KEY_IV))
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
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(MULTICAST_ADDRESS), DISCOVERY_PORT);

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

        public void Initialize(AqaraConfig config)
        {
            dicGateways.Clear();
            foreach(var gateway in config.Gateways)
            {
                if (dicGateways.ContainsKey(gateway.GatewayMacAddress))
                    continue;

                AqaraGateway entry = new AqaraGateway(gateway.GatewayMacAddress, gateway.Password, gateway.Devices);
                dicGateways.Add(entry.Id, entry);
            }
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

            client.JoinMulticastGroup(IPAddress.Parse(MULTICAST_ADDRESS), 50);

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

                    string jsonString = Encoding.UTF8.GetString(data);
                    log.DebugFormat("Received: {0}", jsonString);
                    ProcessMessage(jsonString, timestamp);
                }
                catch(Exception ex)
                {
                    log.Error("client.ReceiveAsync.", ex);
                }
            }

            client.DropMulticastGroup(IPAddress.Parse(MULTICAST_ADDRESS));

            client.Dispose();
        }

        public void SendWriteCommand(AqaraDevice device, Dictionary<string, string> arguments)
        {
            AqaraGateway gateway = device.Gateway;

            arguments.Add("key", Encrypt(gateway.Password, gateway.Token));
            string dataString = JsonConvert.SerializeObject(arguments);

            dynamic message = new {
                cmd = "write",
                model = device.ModelName,
                sid = device.SystemId,
                short_id = device.ShortId,
                data = dataString,
            };

            string jsonString = JsonConvert.SerializeObject(message);
            this.SendCommand(gateway, jsonString);
        }
    }
}
