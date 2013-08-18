using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web.Interfaces;

namespace Web.Tests.Controllers {
	internal class InvalidIdChecks<TEntity> where TEntity : new() {
		private readonly ICustomerController<TEntity> _controller;
		private readonly string _controllerName;

		public InvalidIdChecks(ICustomerController<TEntity> controller) {
			_controller = controller;
			_controllerName = controller.GetType().Name;
		}

		public void NonExistingIdResultsInNotFound(int id) {
			Delete_NonExistingIdResultsInNotFound(id);
			Edit_NonExistingIdResultsInNotFound(id);
		}

		public void Delete_NonExistingIdResultsInNotFound(int invalidValue) {
			var result = _controller.Delete(invalidValue);
			Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode, 
					"Ivalid id should produce NotFound result");
		}


		public void Edit_NonExistingIdResultsInNotFound(int invalidValue) {
			var result = _controller.PutCustomer(invalidValue, new TEntity());
			Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode,
					"Ivalid id should produce BadRequest result");
		}
	}
}
