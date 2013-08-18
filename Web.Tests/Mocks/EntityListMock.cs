using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Moq;
using Web.Models;

namespace Web.Tests.Mocks {
	public class EntityListMock<T> : Mock<IDbSet<T>> where T : class {
		public EntityListMock() {
#if EF_5_0
			var collection = new ObservableCollection<T>();
#else
			var collection = new DbLocalView<T>();
#endif
			var queryable = collection.AsQueryable();
			SetupGet(p => p.ElementType).Returns(queryable.ElementType);
			SetupGet(p => p.Expression).Returns(queryable.Expression);
			Setup(p => p.GetEnumerator()).Returns(queryable.GetEnumerator());
			SetupGet(p => p.Provider).Returns(queryable.Provider);
			Setup(p => p.Add(It.IsAny<T>()))
					.Callback((T d) => collection.Add(d));
			Setup(p => p.Remove(It.IsAny<T>())).Callback((T d) => collection.Remove(d));
			SetupGet(p => p.Local).Returns(collection);
		}
	}
}
