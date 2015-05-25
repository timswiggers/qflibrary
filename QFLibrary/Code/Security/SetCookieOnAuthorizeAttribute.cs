using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using Newtonsoft.Json;

namespace QFLibrary.Code.Security
{
    namespace WebApi
    {
        using System.Web.Http;

        public class SetCookieOnAuthorizeAttribute : AuthorizeAttribute
        {
            public override void OnAuthorization(HttpActionContext actionContext)
            {
                base.OnAuthorization(actionContext);

                var principal = actionContext.RequestContext.Principal;

                var request = actionContext.Request;
                var response = actionContext.Response;

                var cookieExists = new Func<String, bool>(key => request.Headers.GetCookies(CookieAuthorization.CookieSessionKey).Any());
                var setCookie = new Action<string, string>((key, value) => response.Headers.AddCookies(new[] { new CookieHeaderValue(key, value) }));

                CookieAuthorization.SetCookie(principal, cookieExists, setCookie);
            }
        }
    }

    namespace Mvc
    {
        using System.Web.Mvc;

        public class SetCookieOnAuthorizeAttribute : AuthorizeAttribute
        {
            public override void OnAuthorization(AuthorizationContext filterContext)
            {
                base.OnAuthorization(filterContext);

                var principal = filterContext.HttpContext.User;

                var request = filterContext.HttpContext.Request;
                var response = filterContext.HttpContext.Response;

                var cookieExists = new Func<string, bool>(key => request.Cookies[CookieAuthorization.CookieSessionKey] != null);
                var setCookie = new Action<string, string>((key, value) => response.AppendCookie(new HttpCookie(key, value)));

                CookieAuthorization.SetCookie(principal, cookieExists, setCookie);
            }
        }
    }

    internal static class CookieAuthorization
    {
        public const string CookieSessionKey = "qflibrary.session";

        public static void SetCookie(IPrincipal principal, Func<String, bool> cookieExists, Action<String, String> setCookie)
        {
            var windowsIdentity = principal.Identity as WindowsIdentity;
            if (windowsIdentity == null)
            {
                throw new UnauthorizedAccessException("Windows Identity not found");
            }

            var domainAccount = windowsIdentity.Name;

            string domain;
            string account;

            if (!TrySplit(domainAccount, out domain, out account))
            {
                throw new UnauthorizedAccessException("Windows Identity invalid");
            }

            if (cookieExists(CookieSessionKey))
            {
                return;
            }

            var cookieValue = JsonConvert.SerializeObject(new { domain, account });
            var cookieValueEncrypted = Encrypt(cookieValue);

            setCookie(CookieSessionKey, cookieValueEncrypted);
        }

        private static bool TrySplit(string domainAccount, out string domain, out string account)
        {
            domain = String.Empty;
            account = String.Empty;

            if (String.IsNullOrWhiteSpace(domainAccount))
            {
                return false;
            }

            string[] parts;

            if ((parts = domainAccount.Split('\\')).Length != 2)
            {
                return false;
            }

            domain = parts[0];
            account = parts[1];

            return true;
        }

        private static string Encrypt(string value)
        {
            var asBytes = Encoding.ASCII.GetBytes(value);

            return Convert.ToBase64String(asBytes);
        }
    }
}