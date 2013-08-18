using System.Data.Entity;
using Web.Interfaces;

namespace Web.Controllers {
	public class EntityStateSetter: IEntityState {
		public void SetState(ICustomerContext context, object entity, EntityState state) {
			context.Entry(entity).State = state;
		}
	}
}