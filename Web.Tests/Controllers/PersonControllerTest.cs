using System;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web.Controllers;
using Web.Interfaces;
using Web.Models;
using Web.Tests.Mocks;

namespace Web.Tests.Controllers {
	[TestClass]
	public class PersonControllerTest : CustomerControlerTestBase<Person, PersonDto, ICustomerContext> {
		[ClassInitialize]
		public static void ClassInit(TestContext context) {
			BaseClassInit(context);
		}

		protected override ICustomerController<PersonDto> CreateController() {
			return new PersonController(_db, new EntityStateSetterMock<Person>(_db.Persons), 
					new RequestProxyMock());
		}

		protected override void SetupEntityListMock() {
			_mock.Setup(p => p.Persons).Returns(_entityListMock.Object);
		}

		protected override PersonDto CreateTestEntity(int newId) {
			return new PersonDto() {
				Id = newId,
				ContactPersonFirstName = "ContactPersonFirstName" + newId,
				ContactPersonLastName = "ContactPersonLastName" + newId,
				PhoneNumber = "PhoneNumber" + newId,
				Email = "Email" + newId,
				CollaborationStartDate = DateTime.Now.AddDays(-1),
				LastActivityDate = DateTime.Now,

				SSN = "SSN" + newId,
				DateOfBirth = DateTime.Now.AddYears(-2)
			};
		}

		protected override IDbSet<Person> Entities {
			get { return _db.Persons; }
		}

		protected override void GenerateModelError() {
			_controller.ModelState.AddModelError("Name", new ArgumentOutOfRangeException("Name"));
		}
	}
}
