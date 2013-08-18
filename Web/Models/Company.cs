using System.ComponentModel.DataAnnotations;

namespace Web.Models {
	public class Company: Customer {
		[Required]
		public string Name { get; set; }

		public int? EmployeeCount { get; set; }

		public override void Apply(Customer other) {
			var otherCompany = other as Company;
			if (otherCompany == null) {
				return;
			}
			base.Apply(other);
			Name = otherCompany.Name;
			EmployeeCount = otherCompany.EmployeeCount;
		}

		public override bool Equals(object obj) {
			var other = obj as Company;
			if (other == null) {
				return false;
			}
			return base.Equals(other) && Name == other.Name	&& EmployeeCount == other.EmployeeCount;
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}
	}
}