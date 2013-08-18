using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models {
	/// <summary>
	/// Data transfer object for <see cref="Customer"/>
	/// </summary>
	public class CustomerDto {
		public CustomerDto() { }

		public CustomerDto(Customer customer) {
			Id = customer.Id;
			ContactPersonFirstName = customer.ContactPersonFirstName;
			ContactPersonLastName = customer.ContactPersonLastName;
			PhoneNumber = customer.PhoneNumber;
			Email = customer.Email;
			CollaborationStartDate = customer.CollaborationStartDate;
			LastActivityDate = customer.LastActivityDate;
		}

		[Key]
		public int Id { get; set; }

		[Required]
		public string ContactPersonFirstName { get; set; }

		[Required]
		public string ContactPersonLastName { get; set; }

		[Required]
		public string PhoneNumber { get; set; }

		[Required]
		public string Email { get; set; }

		[Required]
		public DateTime? CollaborationStartDate { get; set; }

		[Required]
		public DateTime? LastActivityDate { get; set; }

		public virtual Customer CreateEntity() {
			return new Customer();
		}

		public virtual Customer ToEntity() {
			var customer = CreateEntity();
			customer.Id = Id;
			customer.ContactPersonFirstName = ContactPersonFirstName;
			customer.ContactPersonLastName = ContactPersonLastName;
			customer.PhoneNumber = PhoneNumber;
			customer.Email = Email;
			customer.CollaborationStartDate = CollaborationStartDate;
			customer.LastActivityDate = LastActivityDate;
			return customer;
		}
	}
}