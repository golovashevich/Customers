using System.Web;
using System.Web.Http;
using System.Web.Http.Validation.Providers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Web.DependencyResolution;

namespace Web {
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : HttpApplication {
		protected void Application_Start() {
			DependencyResolver.SetResolver(new StructureMapDependencyResolver(IoC.Initialize()));

			AreaRegistration.RegisterAllAreas();

			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			AuthConfig.RegisterAuth();

			//http://stackoverflow.com/a/12518260/2478008
			GlobalConfiguration.Configuration.Services.RemoveAll(
				typeof(System.Web.Http.Validation.ModelValidatorProvider),
				v => v is InvalidModelValidatorProvider);
		}
	}
}