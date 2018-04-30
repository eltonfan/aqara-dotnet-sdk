using Elton.AqaraCloud;
using Elton.OAuth2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elton.AqaraCloud.Tests
{
    public class ApiContext
    {
        static ApiContext instance = null;
        public static ApiContext Default
        {
            get
            {
                if (instance == null)
                    instance = new ApiContext();

                return instance;
            }
        }

        readonly Elton.Utils.OneDrive onedrive = new Elton.Utils.OneDrive(@"ApplicationData\ConnectedHome\config");
        readonly AqaraConfiguration appConfig;
        readonly Token tokenConfig;
        readonly AqaraClient api;
        private ApiContext()
        {
            appConfig = onedrive.ReadConfig<AqaraConfiguration>("aqaraCloud");
            tokenConfig = onedrive.ReadConfig<Token>("aqaraCloud.token");

            api = new AqaraClient(appConfig);
            api.SetCredentials(tokenConfig.AccessToken);
        }

        public void SaveToken(IToken token)
        {
            tokenConfig.CopyFrom(token);
            onedrive.WriteConfig("aqaraCloud.token", tokenConfig);
        }

        public static string GetJsonString(string category, string fileName)
        {
            var fullName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "examples", category, fileName);
            if (!File.Exists(fullName))
                throw new FileNotFoundException("Json file is not exists.", fullName);

            return File.ReadAllText(fullName, Encoding.UTF8);
        }

        public static AqaraClient Api => Default.api;
        public static AqaraConfiguration Config => Default.appConfig;
        public static Token Token => Default.tokenConfig;
    }
}
