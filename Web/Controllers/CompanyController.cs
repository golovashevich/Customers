using System.Data.Entity;
using Web.Interfaces;
using Web.Models;

namespace Web.Controllers {

	public class CompanyController : 
			CustomerControllerBase<Company, CompanyDto>, ICustomerController<CompanyDto> {
		public CompanyController(ICustomerContext db, IEntityState state, IRequestProxy requestProxy) 
				: base(db, state, requestProxy) { }

		protected override CompanyDto CreateEntityDto(Customer customer) {
			return new CompanyDto((Company)customer);
		}

		protected override IDbSet<Company> Customers {
			get { return Db.Companies; }
		}
	}
}