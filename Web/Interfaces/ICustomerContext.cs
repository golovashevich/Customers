using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Web.Models;

namespace Web.Interfaces {
	public interface ICustomerContext {
		IDbSet<Person> Persons { get; set; }
		IDbSet<Company> Companies { get; set; }

		int SaveChanges();
		DbEntityEntry Entry(object entity);
		void Dispose();
	}
}
