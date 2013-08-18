window.todoApp = window.todoApp || {};

window.todoApp.datacontext = (function () {

	//TODO: Refactor to autoselect [company|person] datacontext 
	var datacontext = {
    	getCompanies: getCompanies,
        saveNewCompany: saveNewCompany,
        saveChangedCompany: saveChangedCompany,
        deleteCompany: deleteCompany,

        getPersons: getPersons,
        saveNewPerson: saveNewPerson,
        saveChangedPerson: saveChangedPerson,
		deletePerson: deletePerson, 

		createCompanyPerson: createCompanyPerson,
	};

    return datacontext;

	//TODO: Refactor to use single get method
    function getCompanies(companiesObservable, errorObservable) {
        return ajaxRequest("get", companyUrl())
            .done(getSucceeded)
            .fail(getFailed);

        function getSucceeded(data) {
            var mappedCompanies = $.map(data, function (list) { return new createCompany(list); });
            companiesObservable(mappedCompanies);
        }

        function getFailed(jqXHR) {
            errorObservable("Error retrieving company list.");
        }
    }

    function saveNewCompany(customer) {
    	clearErrorMessage(customer);
    	return ajaxRequest("post", companyUrl(), customer)
            .done(function (result) {
            	customer.id = result.id;
            })
            .fail(function (jqXHR) {
            	customer.errorMessage("Error adding a new customer.");
            });
    }

    function saveChangedCompany(customer) {
    	clearErrorMessage(customer);
    	return ajaxRequest("put", companyUrl(customer.id), customer, "text")
            .fail(function (jqXHR) {
            	customer.errorMessage("Error updating the company.");
            });
    }

    function deleteCompany(customer) {
    	return ajaxRequest("delete", companyUrl(customer.id))
            .fail(function (jqXHR) {
            	company.errorMessage("Error removing customer.");
            });
    }

    function createCompanyPerson(customerDto) {
    	return new datacontext.companyPerson(customerDto);
    }

    function createCompanyPersonDto(customerObservable) {
    	return new datacontext.companyPersonDto(customerObservable);
    }

    function getPersons(personsObservable, errorObservable) {
    	return ajaxRequest("get", personUrl())
            .done(getSucceeded)
            .fail(getFailed);

    	function getSucceeded(data) {
    		var mappedPersons = $.map(data, function (list) { return new createPerson(list); });
    		personsObservable(mappedPersons);
    	}

    	function getFailed(jqXHR) {
    		errorObservable("Error retrieving person list.");
    	}
    }

    function saveNewPerson(customer) {
    	clearErrorMessage(customer);
    	return ajaxRequest("post", personUrl(), customer)
            .done(function (result) {
            	customer.id = result.id;
            })
            .fail(function (jqXHR) {
            	customer.errorMessage("Error adding a new customer.");
            });
    }

    function saveChangedPerson(customer) {
    	clearErrorMessage(customer);
    	return ajaxRequest("put", personUrl(customer.id), customer, "text")
            .fail(function (jqXHR) {
            	customer.errorMessage("Error updating the customer.");
            });
    }

    function deletePerson(customer) {
    	return ajaxRequest("delete", personUrl(customer.id))
            .fail(function (jqXHR) {
            	customer.errorMessage("Error removing customer.");
            });
    }

    // Private
    function clearErrorMessage(entity) { entity.errorMessage(null); }
    function ajaxRequest(type, url, data, dataType) { // Ajax helper
        var options = {
            dataType: dataType || "json",
            contentType: "application/json",
            cache: false,
            type: type,
            data: data ? data.toJson() : null
        };
        var antiForgeryToken = $("#antiForgeryToken").val();
        if (antiForgeryToken) {
            options.headers = {
                'RequestVerificationToken': antiForgeryToken
            }
        }
        return $.ajax(url, options);
    }

	function createCompany(data) {
		return new datacontext.company(data); // company is injected by customers.model.js
	}

	function createPerson(data) {
		return new datacontext.person(data); // person is injected by customers.model.js
	}

	// routes
    function companyUrl(id) { return "/api/company/" + (id || ""); }
    function personUrl(id) { return "/api/person/" + (id || ""); }

})();