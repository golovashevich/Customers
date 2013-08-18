using System.Data.Entity;
using System.Linq;
using Web.Interfaces;
using Web.Models;

namespace Web.Tests.Mocks {
	public class EntityStateSetterMock<TEntity> : IEntityState where TEntity : Customer {
		private IDbSet<TEntity> _entities;
		public EntityStateSetterMock(IDbSet<TEntity> entities) {
			_entities = entities;
		}

		public void SetState(ICustomerContext context, object entity, EntityState state) {
			if (state == EntityState.Modified) {
				var customerEntity = entity as TEntity;
				if (customerEntity != null) {
					var entityInList = _entities.SingleOrDefault(p => p.Id == customerEntity.Id);
					entityInList.Apply(customerEntity);
				}
			}
		}
	}
}