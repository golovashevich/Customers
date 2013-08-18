using System;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web.Controllers;
using Web.Interfaces;
using Web.Models;
using Web.Tests.Mocks;

namespace Web.Tests.Controllers {
	[TestClass]
	public class CompanyControllerTest : CustomerControlerTestBase<Company, CompanyDto, ICustomerContext> {
		[ClassInitialize]
		public static void ClassInit(TestContext context) {
			BaseClassInit(context);
		}

		protected override ICustomerController<CompanyDto> CreateController() {
			return new CompanyController(_db,  new EntityStateSetterMock<Company>(_db.Companies), 
					new RequestProxyMock());
		}

		protected override void SetupEntityListMock() {
			_mock.Setup(p => p.Companies).Returns(_entityListMock.Object);
		}

		protected override CompanyDto CreateTestEntity(int newId) {
			return new CompanyDto() {
				Id = newId,
				ContactPersonFirstName = "ContactPersonFirstName" + newId,
				ContactPersonLastName = "ContactPersonLastName" + newId,
				PhoneNumber = "PhoneNumber" + newId,
				Email = "Email" + newId,
				CollaborationStartDate = DateTime.Now.AddMinutes(-10),
				LastActivityDate = DateTime.Now,

				Name = "Name" + newId,
				EmployeeCount = newId
			};
		}

		protected override IDbSet<Company> Entities {
			get { return _db.Companies;  }
		}

		protected override void GenerateModelError() {
			_controller.ModelState.AddModelError("Name", new ArgumentOutOfRangeException("Name"));
		}
	}
}
