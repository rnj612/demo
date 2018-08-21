#region Copyright © 2016 jijinwan.com .All Rights Reserved. 
//   -------------------------------------------------------------------------------------------------
//   SOLUTION     :   wcp
//   PROJECT      :   wcp.Api
//   FILENAME     :   CorsMessageHandler.cs  
//   CREATETIME   :   2016年03月03日
//   AUTHOR       :   王晨刚(wangcg@jijinwan.com)
//   DESCRIPTION  :   
//   ------------------------------------------------------------------------------------------------
// 
#endregion

using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Api
{
    /// <summary>
    /// 跨域处理
    /// </summary>
    public class CorsMessageHandler:DelegatingHandler
    {
        static readonly string domainName = ConfigurationManager.AppSettings["domainName"];

        const string Origin = "Origin";
        const string AccessControlRequestMethod = "Access-Control-Request-Method";
        const string AccessControlRequestHeaders = "Access-Control-Request-Headers";
        const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";
        const string AccessControlAllowMethods = "Access-Control-Allow-Methods";
        const string AccessControlAllowHeaders = "Access-Control-Allow-Headers";

        private static string _allowOrigin = "*";

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return request.Headers.Contains(Origin) ? ProcessCorsRequest(request, ref cancellationToken) : base.SendAsync(request, cancellationToken);
        }

        private Task<HttpResponseMessage> ProcessCorsRequest(HttpRequestMessage request, ref CancellationToken cancellationToken)
        {
            if (request.Method == HttpMethod.Options)
            {
                return Task.Factory.StartNew<HttpResponseMessage>(() =>
                {
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                    AddCorsResponseHeaders(request, response);
                    return response;
                }, cancellationToken);
            }
            else
            {
                return base.SendAsync(request, cancellationToken).ContinueWith<HttpResponseMessage>(task =>
                {
                    HttpResponseMessage resp = task.Result;
                    if (IsAllowUrl(request.RequestUri.Host))
                    {
                        resp.Headers.Add(AccessControlAllowOrigin, request.Headers.GetValues(Origin).First());
                    }
                    else
                    {
                        resp.Headers.Add(AccessControlAllowOrigin, _allowOrigin);
                    }
                    return resp;
                });
            }
        }

        private static void AddCorsResponseHeaders(HttpRequestMessage request, HttpResponseMessage response)
        {
            if (IsAllowUrl(request.RequestUri.Host))
            {
                response.Headers.Add(AccessControlAllowOrigin, request.Headers.GetValues(Origin).First());
            }
            else
            {
                response.Headers.Add(AccessControlAllowOrigin, _allowOrigin);
            }

            string accessControlRequestMethod = request.Headers.GetValues(AccessControlRequestMethod).FirstOrDefault();
            if (accessControlRequestMethod != null)
            {
                response.Headers.Add(AccessControlAllowMethods, accessControlRequestMethod);
            }

            string requestedHeaders = string.Join(", ", request.Headers.GetValues(AccessControlRequestHeaders));
            if (!string.IsNullOrEmpty(requestedHeaders))
            {
                response.Headers.Add(AccessControlAllowHeaders, requestedHeaders);
            }
        }

        private static bool IsAllowUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(domainName)) return true;
            return url.ToLower().EndsWith(domainName);
        }
    }
}