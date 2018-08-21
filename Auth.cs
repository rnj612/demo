using System;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.Owin.Security;

namespace Api
{
    public class Auth
    {
        public static string Authenticate(string id)
        {
            var identity = new ClaimsIdentity("Bearer");
            identity.AddClaim(new Claim("id", id));
            var newguid = Guid.NewGuid().ToString().Replace("-", "");
            identity.AddClaim(new Claim("token", newguid));

            AuthenticationTicket ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
            var currentUtc = DateTime.UtcNow;
            ticket.Properties.IssuedUtc = currentUtc;
            ticket.Properties.ExpiresUtc = currentUtc.Add(TimeSpan.FromDays(15));

            // 返回值
            return Startup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);
        }


    }
}