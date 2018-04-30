using Elton.AqaraCloud.Models;
using Elton.OAuth2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elton.AqaraCloud
{
    partial class AqaraApi
    {
        public User GetUser(string openId)
        {
            var responseSample = new
            {
                result = new
                {
                    userInfo = new
                    {
                        nickName = "绿藻头"
                    },
                    openId = "xxx"
                },
                code = 0,
                isBytesData = 0,
                requestId = "xxx"
            };

            var response = Post<dynamic>("/user/query",
                headerParams: new[] {
                    new KeyValuePair<string, string>("Appid", config.ApplicationId),
                    new KeyValuePair<string, string>("Appkey", config.ApplicationSecret),
                    new KeyValuePair<string, string>("Openid", openId),
                    new KeyValuePair<string, string>("Access-Token", token),
                },
                contentType: "application/json",
                postBody: JsonConvert.SerializeObject(new { openId, }));

            if (response.code != 0)
                throw new ApiException((int)response.code, (string)response.result);

            return (response.result as Newtonsoft.Json.Linq.JObject).ToObject<User>();
        }
    }
}
