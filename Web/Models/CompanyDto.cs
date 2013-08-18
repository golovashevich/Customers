using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models {
	/// <summary>
	/// Data transfer object for <see cref="Company"/>
	/// </summary>
	public class CompanyDto : CustomerDto {
		public CompanyDto() { }

		public CompanyDto(Company company) : base(company) {
			Name = company.Name;
			EmployeeCount = company.EmployeeCount;
		}

		[Required]
		public string Name { get; set; }

		public int? EmployeeCount { get; set; }

		public override Customer CreateEntity() {
			return new Company();
		}

		public override Customer ToEntity() {
			var company = (Company) base.ToEntity();
			company.Name = Name;
			company.EmployeeCount = EmployeeCount;
			return company;
		}
	}
}