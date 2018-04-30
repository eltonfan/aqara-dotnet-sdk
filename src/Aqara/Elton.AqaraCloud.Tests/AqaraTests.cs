using System;
using System.IO;
using System.Net;
using System.Threading;
using Elton.OAuth2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Elton.AqaraCloud.Tests
{
    [TestClass]
    public class AqaraTests
    {
        static readonly Common.Logging.ILog log = Common.Logging.LogManager.GetLogger(typeof(AqaraTests));

        public TestContext TestContext { get; set; }

        AqaraConfiguration config = null;
        AqaraClient instance = null;
        [TestInitialize]
        public void Initialize()
        {
            instance = ApiContext.Api;
            config = ApiContext.Config;
        }

        AutoResetEvent resetEvent = new AutoResetEvent(false);
        [TestMethod]
        public void LoginTest()
        {
            resetEvent.Reset();

            StartHttpListener();
            string url = config.GetAuthorizationUrl(
                appId: config.ApplicationId,
                redirectUri: config.RedirectUri,
                state: Guid.NewGuid().ToString("N").ToLower(),
                theme: 0);
            System.Diagnostics.Process.Start(url);

            resetEvent.WaitOne();
        }

        [TestMethod]
        public void GetUserTest()
        {
            var userName = instance.GetUser(ApiContext.Token.OpenId);
            Assert.IsNotNull(userName);
        }

        HttpListener httpListener = new HttpListener();

        void StartHttpListener()
        {
            httpListener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            httpListener.Prefixes.Add("http://localhost:6060/");
            httpListener.Start();
            new Thread(new ThreadStart(delegate
            {
                while (true)
                {
                    HttpListenerContext httpListenerContext = httpListener.GetContext();

                    //成功：GET redirect_uri/?code=authorize_code
                    //失败：GET redirect_uri/?error=access_denied&error_description=The+resource+owner+or+authorization+server+denied+the+request.
                    var queryString = httpListenerContext.Request.QueryString;

                    string title = "";
                    string desc = "";
                    try
                    {
                        if (!string.IsNullOrEmpty(queryString["error"]))
                        {
                            httpListenerContext.Response.StatusCode = 200;
                            title = queryString["error"];
                            desc = queryString["error_description"];
                            continue;
                        }

                        if (string.IsNullOrEmpty(queryString["code"]))
                        {
                            httpListenerContext.Response.StatusCode = 200;
                            title = "Failed";
                            desc = "The code query is empty.";
                            continue;
                        }

                        string authorizeCode = queryString["code"];
                        log.Info("authorizeCode: " + authorizeCode);

                        var api = new ApiClient(config);
                        var token = api.CreateToken(authorizeCode);
                        ApiContext.Default.SaveToken(token);

                        log.Info("AccessToken: " + token.AccessToken);

                        httpListenerContext.Response.StatusCode = 200;
                        title = "Finished";
                        desc = "Well done, you now have an access token which allows you to call Web API on behalf of the user.<br />Please return to the application.";
                    }
                    catch (Exception ex)
                    {
                        title = "Failed";
                        desc = "Failed to create token.<br />" + ex.StackTrace;
                        log.Error("Failed to create token.", ex);
                    }
                    finally
                    {
                        using (var writer = new StreamWriter(httpListenerContext.Response.OutputStream))
                        {
                            WriteHtml(writer, title, desc);
                        }
                    }

                    resetEvent.Set();
                }
            })).Start();
        }

        readonly string templateString = Properties.Resources.HtmlTemplate;
        void WriteHtml(StreamWriter writer, string title, string desc)
        {
            var html = templateString
                .Replace("%title%", title)
                .Replace("%desc%", desc);

            writer.Write(html);
        }
    }
}
