using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Api
{
    public static class ApiUtil
    {

        private static bool isPrivateIP(string ipAddress)
        {
            // http://en.wikipedia.org/wiki/Private_network
            // Private IP Addresses are: 
            //  24-bit block: 10.0.0.0 through 10.255.255.255
            //  20-bit block: 172.16.0.0 through 172.31.255.255
            //  16-bit block: 192.168.0.0 through 192.168.255.255
            //  Link-local addresses: 169.254.0.0 through 169.254.255.255 (http://en.wikipedia.org/wiki/Link-local_address)

            var ip = IPAddress.Parse(ipAddress);
            var octets = ip.GetAddressBytes();

            var is24BitBlock = octets[0] == 10;
            if (is24BitBlock) return true; // Return to prevent further processing

            var is20BitBlock = octets[0] == 172 && octets[1] >= 16 && octets[1] <= 31;
            if (is20BitBlock) return true; // Return to prevent further processing

            var is16BitBlock = octets[0] == 192 && octets[1] == 168;
            if (is16BitBlock) return true; // Return to prevent further processing

            var isLinkLocalAddress = octets[0] == 169 && octets[1] == 254;
            return isLinkLocalAddress;
        }

        public static string GetIPAddressFromAspNet(HttpRequestBase request)
        {
            string szXForwardedFor = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (szXForwardedFor == null)
            {
                return request.UserHostAddress;
            }
            else
            {
                if (szXForwardedFor.IndexOf(",") > 0)
                {
                    string[] arIPs = szXForwardedFor.Split(',');

                    foreach (string item in arIPs)
                    {
                        if (!isPrivateIP(item))
                        {
                            return item;
                        }
                    }
                }
                return szXForwardedFor;
            }
        }

        public static string GetIPAddressFromOwin(IOwinRequest request)
        {
            string szXForwardedFor = request.Headers["HTTP_X_FORWARDED_FOR"];

            if (szXForwardedFor == null)
            {
                return request.RemoteIpAddress;
            }
            else
            {
                if (szXForwardedFor.IndexOf(",") > 0)
                {
                    string[] arIPs = szXForwardedFor.Split(',');

                    foreach (string item in arIPs)
                    {
                        if (!isPrivateIP(item))
                        {
                            return item;
                        }
                    }
                }
            }
            return string.Empty;
        }

        public static string GetIp(HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return GetIPAddressFromAspNet(((HttpContextBase)request.Properties["MS_HttpContext"]).Request);
            }
            if (request.Properties.ContainsKey("MS_OwinContext"))
            {
                return GetIPAddressFromOwin(((OwinContext)request.Properties["MS_OwinContext"]).Request);
            }
            return string.Empty;
            //throw new InvalidOperationException("Client IP Address Not Found in HttpRequest");
        }

        public static async Task<HttpResponseMessage> ExpStream(string str)
        {
            byte[] array = Encoding.UTF8.GetBytes(str);
            MemoryStream Ms = new MemoryStream(array);

            byte[] imgByte = new byte[Ms.Length];
            Ms.Position = 0;
            await Ms.ReadAsync(imgByte, 0, Convert.ToInt32(Ms.Length));
            Ms.Close();

            var resp = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(imgByte)
            };
            resp.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            resp.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = Guid.NewGuid().ToString() + ".xls"
            };
            return resp;
        }
    }
}