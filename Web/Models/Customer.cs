using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Web.Models {
	public class Customer {
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

		public virtual void Apply(Customer other) {
			Id = other.Id;
			ContactPersonFirstName = other.ContactPersonFirstName;
			ContactPersonLastName = other.ContactPersonLastName;
			PhoneNumber = other.PhoneNumber;
			Email = other.Email;
			CollaborationStartDate = other.CollaborationStartDate;
			LastActivityDate = other.LastActivityDate;
		}

		public override bool Equals(object obj) {
			var other = obj as Customer;
			if (other == null) {
				return false;
			}
			return Id == other.Id && ContactPersonFirstName == other.ContactPersonFirstName
				&& ContactPersonLastName == other.ContactPersonLastName
				&& PhoneNumber == other.PhoneNumber && Email == other.Email
				&& CollaborationStartDate == other.CollaborationStartDate 
				&& LastActivityDate == other.LastActivityDate;
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}

		public override string ToString() {
			return ContactPersonLastName + " " + ContactPersonFirstName;
		}

	}
}