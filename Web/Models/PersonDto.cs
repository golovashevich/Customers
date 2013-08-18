using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Web.Models {
	/// <summary>
	/// Data transfer object for <see cref="Company"/>
	/// </summary>
	public class PersonDto : CustomerDto {
		public PersonDto() { }

		public PersonDto(Person person) : base(person) {
			SSN = person.SSN;
			DateOfBirth = person.DateOfBirth;
		}

		[Required]
		public string SSN { get; set; }

		public DateTime? DateOfBirth { get; set; }

		public override Customer CreateEntity() {
			return new Person();
		}

		public override Customer ToEntity() {
			var company = (Person)base.ToEntity();
			company.SSN = SSN;
			company.DateOfBirth = DateOfBirth;
			return company;
		}
	}
}
