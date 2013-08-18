using System.Data.Entity;

namespace Web.Interfaces {
	public interface IEntityState {
		void SetState(ICustomerContext context, object entity, EntityState state); 
	}
}