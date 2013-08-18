using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Web.Interfaces;

namespace Web.Controllers {
	public class RequestProxy: IRequestProxy {

		public HttpResponseMessage CreateResponse<T>(ApiController controller, HttpStatusCode statusCode, 
				T value) {
			return controller.Request.CreateResponse(statusCode, value);
		}

		public HttpResponseMessage CreateResponse(ApiController controller, HttpStatusCode statusCode) {
			return controller.Request.CreateResponse(statusCode);
		}

		public HttpResponseMessage CreateErrorResponse(ApiController controller, HttpStatusCode statusCode, 
				ModelStateDictionary modelState) {
			return controller.Request.CreateResponse(statusCode, modelState);
		}
	}
}