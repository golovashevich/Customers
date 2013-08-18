using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Web.Interfaces;

namespace Web.Controllers {
	public class RequestProxyMock: IRequestProxy {

		public HttpResponseMessage CreateResponse<T>(ApiController controller, HttpStatusCode statusCode, 
				T value) {
					return new HttpResponseMessage(statusCode);
		}

		public HttpResponseMessage CreateResponse(ApiController controller, HttpStatusCode statusCode) {
			var response = new HttpResponseMessage(statusCode);
			response.Headers.Add("location", "location");
			return response;
		}

		public HttpResponseMessage CreateErrorResponse(ApiController controller, HttpStatusCode statusCode, 
				ModelStateDictionary modelState) {
			return new HttpResponseMessage(statusCode);
		}
	}
}