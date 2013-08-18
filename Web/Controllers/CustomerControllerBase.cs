using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Web.Filters;
using Web.Interfaces;
using Web.Models;

namespace Web.Controllers {
	public abstract class CustomerControllerBase<TEntity, TEntityDto> : ApiController 
			where TEntity : Customer
			where TEntityDto : CustomerDto {
		private ICustomerContext _db;
		private IEntityState _entityStateSetter;
		private IRequestProxy _requestProxy;
		protected ICustomerContext Db { get { return _db; } }

		protected abstract IDbSet<TEntity> Customers { get; } 
		protected abstract TEntityDto CreateEntityDto(Customer customer); 

		public CustomerControllerBase(ICustomerContext db, IEntityState entityStateSetter, 
				IRequestProxy requestProxy) {
			_db = db;
			_entityStateSetter = entityStateSetter;
			_requestProxy = requestProxy;
		}

		// GET api/Company
		[ValidateHttpAntiForgeryToken]
		public IEnumerable<TEntityDto> Get() {
			return Customers
				.OrderByDescending(u => u.Id)
				.AsEnumerable()
				.Select(customer => CreateEntityDto(customer));
		}


		// PUT api/Company/5
		[ValidateHttpAntiForgeryToken]
		public HttpResponseMessage PutCustomer(int id, TEntityDto customerDto) {
			if (!ModelState.IsValid) {
				return _requestProxy.CreateErrorResponse(this, HttpStatusCode.BadRequest, ModelState);
			}

			if (id != customerDto.Id) {
				return _requestProxy.CreateResponse(this, HttpStatusCode.BadRequest);
			}

			TEntity customer = (TEntity)customerDto.ToEntity();

			_entityStateSetter.SetState(_db, customer, EntityState.Modified);
			try {
				_db.SaveChanges();
			}
			catch (DbUpdateConcurrencyException) {
				return _requestProxy.CreateResponse(this, HttpStatusCode.InternalServerError);
			}

			return _requestProxy.CreateResponse(this, HttpStatusCode.OK);
		}

		// POST api/Company
		[ValidateHttpAntiForgeryToken]
		public HttpResponseMessage Create(TEntityDto customerDto) {
			if (!ModelState.IsValid) {
				return _requestProxy.CreateErrorResponse(this, HttpStatusCode.BadRequest, ModelState);
			}

			TEntity customer = (TEntity)customerDto.ToEntity();
			Customers.Add(customer);
			_db.SaveChanges();
			customerDto.Id = customer.Id;

			HttpResponseMessage response = _requestProxy.CreateResponse(this, HttpStatusCode.Created, customerDto);
			//response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = customerDto.Id }));
			return response;
		}

		// DELETE api/Company/5
		[ValidateHttpAntiForgeryToken]
		public HttpResponseMessage Delete(int id) {
			var customer = Customers.SingleOrDefault(c => c.Id == id);
			if (customer == null) {
				return _requestProxy.CreateResponse(this, HttpStatusCode.NotFound);
			}

			TEntityDto customerDto = CreateEntityDto(customer);
			Customers.Remove(customer);

			try {
				_db.SaveChanges();
			}
			catch (DbUpdateConcurrencyException) {
				return _requestProxy.CreateResponse(this, HttpStatusCode.InternalServerError);
			}

			return _requestProxy.CreateResponse(this, HttpStatusCode.OK, customerDto);
		}

		protected override void Dispose(bool disposing) {
			_db.Dispose();
			base.Dispose(disposing);
		}
	}
}