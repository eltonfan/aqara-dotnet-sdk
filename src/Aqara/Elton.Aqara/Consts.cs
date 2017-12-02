using System;
using System.Collections.Generic;
using System.Text;

namespace Elton.Aqara
{
    public static class Consts
    {
        public const string MulticastAddress = "224.0.0.50";
        public const int DiscoveryPort = 4321;
        public const int ListeningPort = 9898;

        /// <summary>
        /// AES-CBC 128 初始向量
        /// </summary>
        public static readonly byte[] AES_KEY_IV = new byte[] {
            0x17, 0x99, 0x6d, 0x09, 0x3d, 0x28, 0xdd, 0xb3, 0xba, 0x69, 0x5a, 0x2e, 0x6f, 0x58, 0x56, 0x2e
        };
    }
}
