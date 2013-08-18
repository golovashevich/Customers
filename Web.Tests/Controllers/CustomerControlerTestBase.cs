using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Web.Interfaces;
using Web.Models;
using Web.Tests.Mocks;

namespace Web.Tests.Controllers {
	//Conrete constrains may be avoided with something line IEntityWithId
	public abstract class CustomerControlerTestBase<TEntity, TEntityDto, TDbContext>
				where TEntity : Customer, new()
				where TEntityDto : CustomerDto, new()
				where TDbContext : class, ICustomerContext {
		protected static TDbContext _db;
		protected static ICustomerController<TEntityDto> _controller;
		protected static Mock<IDbSet<TEntity>> _entityListMock;
		protected static Mock<TDbContext> _mock;

		#region Abstraction stuff
		protected abstract ICustomerController<TEntityDto> CreateController();
		protected abstract void SetupEntityListMock();
		protected abstract TEntityDto CreateTestEntity(int newId);
		protected abstract IDbSet<TEntity> Entities { get; }
		protected abstract void GenerateModelError();
		#endregion

		public static void BaseClassInit(TestContext testContext) {
			_mock = new Mock<TDbContext>();
			_entityListMock = new EntityListMock<TEntity>();
			_db = _mock.Object;
		}

		[TestInitialize]
		public virtual void TestInit() {
			SetupEntityListMock();
			_controller = CreateController();
			_controller.ModelState.Clear();

			Entities.Local.Clear();
			for (int i = 0; i < 10; i++) {
				Entities.Add((TEntity)CreateTestEntity(i).ToEntity());
			}
		}

		[TestMethod]
		public void NonExistingIdResultsInNotFound() {
			new InvalidIdChecks<TEntityDto>(_controller).NonExistingIdResultsInNotFound(Int32.MinValue);
		}

		/// <summary>
		/// Creating a valid model
		/// </summary>
		[TestMethod]
		public void Create() {
			int oldCount = Entities.Count();
			int newId = oldCount + 1;
			var entity = CreateTestEntity(newId);

			var result = _controller.Create(entity);
			Assert.AreEqual(HttpStatusCode.Created, result.StatusCode, 
					"Create action with valid model should result in Created");

			Assert.AreEqual(oldCount + 1, Entities.Count(), "Create action should add new entity");
			var savedEntity = Entities.SingleOrDefault(d => d.Id == newId);
			Assert.IsNotNull(savedEntity, "Entity does not seem to be added");
			Assert.IsTrue(entity.ToEntity().Equals(savedEntity), "Original and saved entities differ");
		}

		/// <summary>
		/// Create with invalid model does not add anything and returns to Create view
		/// </summary>
		[TestMethod]
		public void Create_InvalidModel() {
			int oldCount = Entities.Count();
			int newId = oldCount + 1;
			var entity = CreateTestEntity(newId);
			GenerateModelError();

			var result = _controller.Create(entity);
			Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode,
					"Create action with invalid model should result in BadRequest");

			Assert.AreEqual(oldCount, Entities.Count(),
					"Create action with invalid model should not add anything");
		}

		[TestMethod]
		public void Edit() {
			var rand = new Random();
			var newEntity = CreateTestEntity(rand.Next(Int32.MaxValue));
			var id = rand.Next(Entities.Count());
			newEntity.Id = id;

			var result = _controller.PutCustomer(id, newEntity);
			Assert.AreEqual(HttpStatusCode.OK, result.StatusCode,
					"Edit action with valid model should result in OK");

			var updatedEntity = Entities.Single(d => d.Id == id);
			Assert.AreEqual(newEntity.ToEntity(), updatedEntity);
		}

		[TestMethod]
		public void Edit_InvalidModel() {
			int oldCount = Entities.Count();
			int newId = oldCount + 1;
			var newEntity = CreateTestEntity(newId);
			var id = new Random().Next(Entities.Count());
			var oldEntity = Entities.Single(d => d.Id == id);
			GenerateModelError();

			var result = _controller.PutCustomer(id, newEntity);
			Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode,
					"Edit action with invalid model should result in BadRequest");

			var testedEntity = Entities.Single(d => d.Id == id);
			Assert.AreEqual(testedEntity, oldEntity,
					"Edit with invalid model state should not change anything");
		}

		[TestMethod]
		public void Delete() {
			var id = new Random().Next(Entities.Count());

			var result = _controller.Delete(id);
			Assert.AreEqual(HttpStatusCode.OK, result.StatusCode,
					"Delete action with valid id should result in OK");

			Assert.IsNull(Entities.SingleOrDefault(d => d.Id == id),
					"Delete should remove the specified entity from list ({0})", id);
		}
	}
}
