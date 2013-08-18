ko.validation.rules.pattern.message = 'Invalid.';

ko.validation.configure({
	registerExtenders: true,
	messagesOnModified: true,
	insertMessages: true,
	parseInputAttributes: true,
	messageTemplate: null
});

window.todoApp.customersViewModel = (function (ko, datacontext) {
	var customers = ko.observableArray(),
    error = ko.observable(),

    addCustomer = function () {
        isAddingNewCustomer(true);
        var customer = datacontext.createCompanyPerson();
        customer.isCompany(isCompany());
        currentCustomer(customer);
        _currentCustomerInList = customer;
        isCompanyEdited(isCompany());
        editVisible(true);
    },

	updateCustomer = function () {
		var errors = ko.validation.group(currentCustomer);
		if (errors().length != 0) {
			if (!(errors().length == 1 && errors()[0] == null)) {
				errors.showAllMessages();
				return;
			}
		}
		applyAction = isCompanyEdited() ? _currentCustomerInList.applyCompany : _currentCustomerInList.applyPerson;
		applyAction(currentCustomer());

		if (_currentCustomerInList.id) {
			saveChangedCustomerAction = isCompany() ? datacontext.saveChangedCompany : datacontext.saveChangedPerson;
			saveChangedCustomerAction(_currentCustomerInList);
		} else {
			//isCompanyEdited here as create new form type can be changed by the user
			saveNewCustomerAction = isCompanyEdited() ? datacontext.saveNewCompany : datacontext.saveNewPerson;
			saveNewCustomerAction(_currentCustomerInList)
				.done(function (result) {
					if (isCompanyEdited() == isCompany()) {
						customers.unshift(_currentCustomerInList);
					}
				});
		}

		editVisible(false);
		$('#editCustomerModal').modal('hide');
	},

	editCustomer = function (customer) {
		isAddingNewCustomer(false);
		isCompanyEdited(isCompany());
		_currentCustomerInList = customer;
		currentCustomer(customer);
		editVisible(true);
	},

	cancelEdit = function () {
		editVisible(false);
		$('#editCustomerModal').modal('hide');
	},

	loadList = function (newIsCompany) {
		newIsCompany = newIsCompany || isCompany();
		customers.removeAll();
		if (newIsCompany) {
			datacontext.getCompanies(customers, error);
		} else {
			datacontext.getPersons(customers, error);
		}
	},

	deleteCustomer = function (customer) {
		deleteAction = isCompany() ? datacontext.deleteCompany : datacontext.deletePerson;

		customers.remove(customer);
		deleteAction(customer)
			.fail(deleteFailed);

		function deleteFailed() {
			list.unshift(customer); // re-show the restored list
		}
	},

	switchToCompanies = function() {
		isCompany(true);
	},

	switchToPersons = function () {
		isCompany(false);
	},

	editVisible = ko.observable(false);
	isCompany = ko.observable(false);
	isCompany.subscribe(function (newIsCompany) {
			loadList(newIsCompany);
	});

	isCompanyEdited = ko.observable(isCompany());
	isAddingNewCustomer = ko.observable();

	toggleNewCompany = function () {
		isCompanyEdited(!isCompanyEdited());
		currentCustomer().isCompany(isCompanyEdited);
	}

	var currentCustomer = ko.observable(null);

	loadList();

	//holds reference to original observable in the list
	var _currentCustomerInList;

	var pageSize = ko.observable(10),
        pageIndex = ko.observable(0),

        previousPage = function () {
            if (pageIndex() > 0) {
                pageIndex(pageIndex() - 1);
            }
        },

        nextPage = function () {
            if (pageIndex() < maxPageIndex()) {
                pageIndex(pageIndex() + 1);
            }
        },

	    maxPageIndex = function () {
	        return Math.ceil(customers().length / pageSize()) - 1;
	    }, 

    pagedRows = ko.dependentObservable(function () {
	    var size = pageSize();
	    var start = pageIndex() * size;
	    return customers().slice(start, start + size);
	}, customers);
    
	return {
		customers: customers,
		loadList: loadList,
		switchToCompanies: switchToCompanies,
		switchToPersons: switchToPersons,
		toggleNewCompany: toggleNewCompany,
		currentCustomer: currentCustomer,
		editVisible: editVisible,
		isCompany: isCompany,
		error: error,
		isAddingNewCustomer: isAddingNewCustomer, 
		addCustomer: addCustomer,
		editCustomer: editCustomer,
		cancelEdit: cancelEdit,
		deleteCustomer: deleteCustomer,
		updateCustomer: updateCustomer,
		previousPage: previousPage,
		nextPage: nextPage,
		pageIndex: pageIndex,
		maxPageIndex: maxPageIndex,
        pagedRows: pagedRows
	};
})(ko, todoApp.datacontext);

// Initiate the Knockout bindings
ko.applyBindings(window.todoApp.customersViewModel);

