using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models {
	public class Person : Customer {
		[Required]
		public string SSN { get; set; }

		public DateTime? DateOfBirth { get; set; }

		public override void Apply(Customer other) {
			var otherPerson = other as Person;
			if (otherPerson == null) {
				return;
			}
			base.Apply(other);
			SSN = otherPerson.SSN;
			DateOfBirth = otherPerson.DateOfBirth;
		}

		public override bool Equals(object obj) {
			var other = obj as Person;
			if (other == null) {
				return false;
			}
			return base.Equals(other) && SSN == other.SSN
				&& DateOfBirth == other.DateOfBirth;
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}
	}
}