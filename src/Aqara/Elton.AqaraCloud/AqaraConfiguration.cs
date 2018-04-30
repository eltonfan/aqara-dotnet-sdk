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

using Elton.AqaraCloud.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Elton.AqaraCloud
{
    /// <summary>
    /// 国家（地区码）
    /// http://docs.opencloud.aqara.cn/development/region_code/
    /// </summary>
    public enum CloudRegion
    {
        /// <summary>
        /// 亚太地区	AP	Asia-Pacific regions
        /// </summary>
        AP,
        /// <summary>
        /// 大陆地区	CN	Mainland China
        /// </summary>
        CN,
        /// <summary>
        /// 欧洲地区	EU	European regions
        /// </summary>
        EU,
        /// <summary>
        /// 美洲地区	USA	American regions
        /// </summary>
        USA,
    }

    [DataContract]
    public class AqaraConfiguration : Elton.OAuth2.ApiConfiguration
    {
        [JsonConstructor]
        protected AqaraConfiguration() { }

        public AqaraConfiguration(CloudRegion region = CloudRegion.CN, string appId = default, string appSecret = default, string redirectUri = default)
        {
            string authDomain;
            string apiDomain;
            switch (region)
            {
                case CloudRegion.AP: authDomain = "aiot-oauth2-ap.aqara.com"; apiDomain = "aiot-open-ap.aqara.com"; break;
                case CloudRegion.CN: authDomain = "aiot-oauth2.aqara.cn"; apiDomain = "aiot-open-3rd.aqara.cn"; break;
                case CloudRegion.EU: authDomain = "aiot-oauth2-eu.aqara.com"; apiDomain = "aiot-open-eu.aqara.com"; break;
                case CloudRegion.USA: authDomain = "aiot-oauth2-usa.aqara.com"; apiDomain = "aiot-open-usa.aqara.com"; break;
                default: throw new NotSupportedException();
            }
            Initialize(authDomain, apiDomain);

            this.ApplicationId = appId;
            this.ApplicationSecret = appSecret;
            this.RedirectUri = redirectUri;
        }

        public AqaraConfiguration(string authDomain, string apiDomain, string appId = default, string appSecret = default, string redirectUri = default)
        {
            Initialize(authDomain, apiDomain);

            this.ApplicationId = appId;
            this.ApplicationSecret = appSecret;
            this.RedirectUri = redirectUri;
        }

        void Initialize(string authDomain, string apiDomain)
        {
            BaseUrl = $"https://{apiDomain}/open/";
            AuthorizationUrl = $"https://{authDomain}/authorize";
            RefreshTokenUrl = $"https://{authDomain}/refresh_token";
            AccessTokenUrl = $"https://{authDomain}/access_token";
        }

        public override string GetAuthorizationUrl(string appId = default, string redirectUri = default, string state = default, int theme = default)
        {
            return $"{AuthorizationUrl}?client_id={appId}&response_type=code&redirect_uri={redirectUri}&state={state}&theme={theme}";
        }
    }
}