using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.ModelBinding;

namespace Web.Interfaces {
	public interface ICustomerController<TEntityDto> {
		IEnumerable<TEntityDto> Get();
		HttpResponseMessage PutCustomer(int id, TEntityDto customerDto);
		HttpResponseMessage Create(TEntityDto customerDto);
		HttpResponseMessage Delete(int id);

		ModelStateDictionary ModelState { get; }
	}
}
