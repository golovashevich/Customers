(function (ko, datacontext) {
    datacontext.person = person;
    datacontext.company = company;
    datacontext.companyPersonDto = companyPersonDto;
    datacontext.companyPerson = companyPerson;

    function person(data) {
        var self = this;
        data = data || {};

    	// Persisted properties
        initCustomerProperties(data, self);
        initCustomerValidators(self);

        self.ssn = ko.observable(data.ssn).extend({ required: true });
        self.dateOfBirth = ko.observable(data.dateOfBirth);

        self.applyPerson = function (customerObservable) {
        	initCustomerProperties(customerObservable, self, function (first, second) {
        		first(second());
        		return first;
        	});
 
        	self.ssn(customerObservable.ssn());
        	self.dateOfBirth(customerObservable.dateOfBirth());
        };
	};

    function company(data) {
    	var self = this;
    	data = data || {};

    	// Persisted properties
    	initCustomerProperties(data, self);
    	initCustomerValidators(self);

    	self.name = ko.observable(data.name).extend({ required: true });
    	self.employeeCount = ko.observable(data.employeeCount);

    	self.saveChanges = function () {
    		return datacontext.saveChangedCompany(self);
    	};

    	self.applyCompany = function (customerObservable) {
    		initCustomerProperties(customerObservable, self, function (first, second) {
    			first(second());
    			return first;
    		});
    		self.name(customerObservable.name());
    		self.employeeCount(customerObservable.employeeCount());
    	};
    };

    function initCustomerProperties(data, self, action) {
    	action = action || function (target, source) { return ko.observable(source) };
    	self.id = data.id;

    	self.contactPersonFirstName = action(self.contactPersonFirstName, data.contactPersonFirstName);
    	self.contactPersonLastName = action(self.contactPersonLastName, data.contactPersonLastName);
    	self.phoneNumber = action(self.phoneNumber, data.phoneNumber);
    	self.email = action(self.email, data.email);
    	self.collaborationStartDate = action(self.collaborationStartDate, data.collaborationStartDate);
    	self.lastActivityDate = action(self.lastActivityDate, data.lastActivityDate);

    	self.errorMessage = ko.observable();

    	self.toJson = function () { return ko.toJSON(self) };
	}

    function initCustomerValidators(self) {
    	self.contactPersonFirstName.extend({ required: true });
    	self.contactPersonLastName.extend({ required: true });
    	self.phoneNumber.extend({ required: true });
    	self.email.extend({ required: true });
    	self.collaborationStartDate.extend({ required: true });
    	self.lastActivityDate.extend({ required: true });
    }


    function companyPersonDto(customerObservable) {
    	var self = this;

    	initCustomerProperties(customerObservable, self, function (target, source) { return source() });
    	if (customerObservable.name) { self.name = customerObservable.name(); }
    	if (customerObservable.employeeCount) { self.employeeCount = customerObservable.employeeCount(); }

    	if (customerObservable.ssn) { self.ssn = customerObservable.ssn(); }
    	if (customerObservable.dateOfBirth) { self.dateOfBirth = customerObservable.dateOfBirth(); }
	}

	//Compound object to help switching UI between Company and Person
    function companyPerson(companyPersonDto) {
    	companyPersonDto = companyPersonDto || {};
    	var customerPerson = new company(companyPersonDto);

    	initCustomerValidators(customerPerson);
    	customerPerson.isCompany = ko.observable();

    	customerPerson.name.extend({
    		required: {
    			onlyIf: function () {
    				return customerPerson.isCompany();
    			}
    		}
    	});

		//Additional Person properties
    	customerPerson.ssn = ko.observable(companyPersonDto.ssn).extend({
    		required: {
    			onlyIf: function () {
    				return !customerPerson.isCompany();
    			}
    		}
    	});

    	customerPerson.dateOfBirth = ko.observable(companyPersonDto.dateOfBirth);

    	customerPerson.applyPerson = function (companyPersonObservable) {
    		customerPerson.ssn(companyPersonObservable.ssn());
    		customerPerson.dateOfBirth(companyPersonObservable.dateOfBirth());
    	};

    	return customerPerson;
    }

	//TODO: Change to customers
})(ko, todoApp.datacontext);