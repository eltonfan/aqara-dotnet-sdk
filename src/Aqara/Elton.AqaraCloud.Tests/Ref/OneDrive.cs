#region License

//   Copyright 2014 Elton FAN (eltonfan@live.cn, http://elton.io)
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License. 

#endregion

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Elton.Utils
{
    /// <summary>
    /// 从 OneDrive 的本地目录中读写数据。
    /// </summary>
    public class OneDrive
    {
        [DllImport("Shell32.dll")]
        static extern int SHGetKnownFolderPath(
            [MarshalAs(UnmanagedType.LPStruct)]Guid rfid, uint dwFlags, IntPtr hToken,
            out IntPtr ppszPath);

        public static string GetOneDrivePath()
        {
            var skyDriveGuid = new Guid("{A52BBA46-E9E1-435F-B3D9-28DAA648C0F6}");
            var flags = 0x00004000; //DontVerify
            var defaultUser = false;

            IntPtr outPath;
            int result = SHGetKnownFolderPath(skyDriveGuid,
                (uint)flags, new IntPtr(defaultUser ? -1 : 0), out outPath);
            if (result >= 0)
            {
                return Marshal.PtrToStringUni(outPath);
            }
            else
            {
                throw new ExternalException("Unable to retrieve the known folder path. It may not "
                    + "be available on this system.", result);
            }
        }

        readonly string configPath = null;
        public OneDrive(string configPath)
        {
            this.configPath = Path.Combine(GetOneDrivePath(), configPath);
        }

        public T ReadConfig<T>(string name)
        {
            var configFile = Path.Combine(configPath, name + ".json");

            if (!File.Exists(configFile))
                return default;

            var jsonString = File.ReadAllText(configFile, Encoding.UTF8);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public void WriteConfig<T>(string name, T value)
        {
            var jsonString = JsonConvert.SerializeObject(value, Formatting.Indented);

            var configFile = Path.Combine(configPath, name + ".json");
            File.WriteAllText(configFile, jsonString, Encoding.UTF8);
        }

        public string ConfigPath => configPath;
    }
}
