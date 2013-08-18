using System.Data.Entity;
using Web.Interfaces;
using Web.Models;

namespace Web.Controllers {
	public class PersonController : CustomerControllerBase<Person, PersonDto>, ICustomerController<PersonDto> {
		public PersonController(ICustomerContext db, IEntityState state, IRequestProxy requestProxy) 
			: base(db, state, requestProxy) { }

		protected override PersonDto CreateEntityDto(Customer customer) {
			return new PersonDto((Person)customer);
		}

		protected override IDbSet<Person> Customers {
			get { return Db.Persons; }
		}
	}
}