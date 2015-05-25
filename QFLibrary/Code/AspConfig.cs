using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using QFLibrary.Code;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(AspConfig), "Configure")]
namespace QFLibrary.Code
{
    public class AspConfig
    {
        private readonly GlobalFilterCollection _mvcFilters;
        private readonly RouteCollection _mvcRoutes;
        private readonly HttpConfiguration _webApiConfig;

        private AspConfig(GlobalFilterCollection mvcFilters, RouteCollection mvcRoutes, HttpConfiguration webApiConfig)
        {
            _mvcFilters = mvcFilters;
            _mvcRoutes = mvcRoutes;
            _webApiConfig = webApiConfig;
        }

        public static void Configure()
        {
            var mvcfilters = GlobalFilters.Filters;
            var mvcRoutes = RouteTable.Routes;

            GlobalConfiguration.Configure(webApiConfig =>
            {
                var config = new AspConfig(mvcfilters, mvcRoutes, webApiConfig);

                config.RegisterFilters();
                config.RegisterRoutes();
            });
        }

        public void RegisterFilters()
        {
            _mvcFilters.Add(new HandleErrorAttribute());
            _mvcFilters.Add(new Security.Mvc.SetCookieOnAuthorizeAttribute());

            var webApiFilters = _webApiConfig.Filters;

            webApiFilters.Add(new Security.WebApi.SetCookieOnAuthorizeAttribute());
        }

        public void RegisterRoutes()
        {
            _webApiConfig.MapHttpAttributeRoutes();

            _webApiConfig.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            _mvcRoutes.MapRoute(
                name: "Default",
                url: "{*anything}",
                defaults: new { controller = "NoneWebApi", action = "ToIndexHtml" }
            );
        }
    }
}
