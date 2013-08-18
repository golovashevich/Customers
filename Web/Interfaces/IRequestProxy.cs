using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Web.Interfaces {
	public interface IRequestProxy {
		HttpResponseMessage CreateResponse<T>(ApiController controller, HttpStatusCode statusCode, T value);
		HttpResponseMessage CreateResponse(ApiController controller, HttpStatusCode statusCode);
		HttpResponseMessage CreateErrorResponse(ApiController controller, HttpStatusCode statusCode, ModelStateDictionary modelState);
	}
}
