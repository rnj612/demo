using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;

namespace Api
{
    public class Startup
    {
        #region webapi.owin
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter("Bearer"));



        }
        #endregion

        #region oauth
        //private const string issuer = "jjw/";
        //public void Configuration(IAppBuilder app)
        //{
        //    //app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
        //    //{
        //    //    Authority = ConfigurationManager.AppSettings["OAuthHost"],
        //    //    RequiredScopes = new[] { "api1" },
        //    //    ValidationMode = ValidationMode.Local
        //    //});

        //    // OAuth签名cert
        //    var assembly = typeof(Startup).Assembly;
        //    X509Certificate2 cert = null;
        //    using (var stream = assembly.GetManifestResourceStream("Api.sign2048.p12"))
        //    {
        //        if (stream != null)
        //        {
        //            cert = new X509Certificate2(ReadStream(stream), "Iloveyou");
        //        }
        //    }
        //    if (cert == null) throw new ConfigurationErrorsException("Identity Server的签名无法读取");

        //    // 配置token validation
        //    JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();
        //    app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
        //    {
        //        Authority = issuer,
        //        RequiredScopes = new[] { "api1" },
        //        ValidationMode = ValidationMode.Local,
        //        IssuerName = issuer,
        //        SigningCertificate = cert,
        //        EnableValidationResultCache = true,
        //    });
        //}

        //private static byte[] ReadStream(Stream input)
        //{
        //    byte[] buffer = new byte[16 * 1024];
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        int read;
        //        while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
        //        {
        //            ms.Write(buffer, 0, read);
        //        }
        //        return ms.ToArray();
        //    }
        //}

        ///// <summary>
        ///// 去掉字符串结尾的反斜杠'/'
        ///// </summary>
        ///// <param name="s"></param>
        ///// <returns></returns>
        //private string TrimTrailingSlash(string s)
        //{
        //    if (string.IsNullOrEmpty(s)) return s;
        //    int strLen = s.Length;
        //    if (s[strLen - 1] == '/') return s.Substring(0, strLen - 1);
        //    return s;
        //}
        #endregion
    }
}