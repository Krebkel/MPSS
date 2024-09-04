let EmployeeManagement = (function () {
    const module = {};
    
    function setupInputMasks() {
        $('#employeePassport').mask('9999 999999', {
            clearIfNotMatch: true,
            onComplete: function(value) {
                $(this).val(value.replace(/\s/g, ''));
            }
        });

        $('#employeePhone').mask('+7(999)999-99-99', {
            clearIfNotMatch: true,
            onComplete: function(value) {
                $(this).val(value);
            }
        });

        $('#employeeINN').mask('999999999999', {
            clearIfNotMatch: true,
            onComplete: function(value) {
                $(this).val(value);
            }
        });

        $('#employeeAccountNumber').mask('99999999999999999999', {
            clearIfNotMatch: true,
            onComplete: function(value) {
                $(this).val(value);
            }
        });

        $('#employeeBIK').mask('999999999', {
            clearIfNotMatch: true,
            onComplete: function(value) {
                $(this).val(value);
            }
        });
    }

    function setupEventListeners() {
        $('#employeeForm').on('submit', function(event) {
            event.preventDefault();
            module.submitEmployeeForm();
        });

        $('#addEmployeeBtn, #addEmployeeBtn2').on('click', function() {
            module.openEmployeeModal();
        });

        $('.nav-link[href="#wages"]').on('click', function() {
            if (typeof ProjectWages !== 'undefined' && typeof ProjectWages.init === 'function') {
                ProjectWages.init();
            }
        });
    }

    module.loadEmployees = function(fullData = false, tableId = 'employeesTable') {
        $.getJSON('/api/employees/base', function(employees) {
            const employeesTableBody = $(`#${tableId} tbody`);
            employeesTableBody.empty();

            employees.forEach((employee, index) => {
                const employeeRow = createEmployeeRow(employee, index, fullData);
                employeesTableBody.append(employeeRow);
            });

            setupRowEventListeners();
        });
    };

    function createEmployeeRow(employee, index, fullData) {
        return `
            <tr class="employee-row" data-employee-id="${employee.id}">
              <td class="shortcol">${index + 1}</td>
              <td>${employee.name}</td>
              <td class="midcol">${employee.phone}</td>
              <td class="midcol">${formatDateForOutput(new Date(employee.dateOfBirth))}</td>
              ${fullData ? '' : `<td class="midcol">${employee.isDriver ? 'Да' : 'Нет'}</td>`}
              ${fullData ? `
                <td class="midcol">${employee.passport ? employee.passport.substring(0, 4) + ' ' + employee.passport.substring(4) : ''}</td>
                <td class="midcol">${employee.inn ? employee.inn : ''}</td>
                <td class="midcol">${employee.accountNumber ? employee.accountNumber : ''}</td>
                <td class="midcol">${employee.bik ? employee.bik : ''}</td>
              ` : ''}
              <td class="btncol">
                <button class="btn delete-btn" data-employee-id="${employee.id}">⛌</button>
              </td>
            </tr>
        `;
    }

    function setupRowEventListeners() {
        $('.delete-btn').off('click').on('click', function(event) {
            event.stopPropagation();
            const employeeId = $(this).data('employee-id');
            module.deleteEmployee(employeeId);
        });

        $('.employee-row').off('click').on('click', function() {
            const employeeId = $(this).data('employee-id');
            module.openEmployeeModal(employeeId);
        });
    }

    module.deleteEmployee = function(employeeId) {
        if (confirm('Вы уверены, что хотите удалить этого сотрудника?')) {
            $.ajax({
                url: `/api/employees/base/${employeeId}`,
                method: 'DELETE',
                success: function() {
                    module.loadEmployees(false, 'employeesTable');
                    module.loadEmployees(true, 'dataTable');
                },
                error: function (xhr) {
                    const errorMessage = xhr.responseText ? xhr.responseText : 'Ошибка при удалении сотрудника';
                    alert(errorMessage);
                }
            });
        }
    };

    module.openEmployeeModal = function(employeeId) {
        if (employeeId) {
            $.ajax({
                url: `/api/employees/base/${employeeId}`,
                type: 'GET',
                success: function(data) {
                    populateEmployeeModal(data, employeeId);
                },
                error: function(err) {
                    console.error(err);
                }
            });
        } else {
            populateEmployeeModal();
        }
    };

    function populateEmployeeModal(data = {}, employeeId = '') {
        $('#modalTitle').text(employeeId ? 'Редактировать сотрудника' : 'Добавить нового сотрудника');
        $('#employeeId').val(employeeId);
        $('#employeeName').val(data.name || '');
        $('#employeePhone').val(data.phone || '');
        $('#employeePassport').val(data.passport ? data.passport.substring(0, 4) + ' ' + data.passport.substring(4) : '');
        $('#employeeDateOfBirth').val(data.dateOfBirth ? formatDateForInput(new Date(data.dateOfBirth)) : new Date().toISOString().split('T')[0]);
        $('#employeeIsDriver').prop('checked', data.isDriver || false);
        $('#employeeINN').val(data.inn || '');
        $('#employeeAccountNumber').val(data.accountNumber || '');
        $('#employeeBIK').val(data.bik || '');
        $('#employeeModal').fadeIn();
    }

    module.submitEmployeeForm = function() {
        const employeeData = {
            Name: $('#employeeName').val(),
            Phone: $('#employeePhone').val(),
            DateOfBirth: $('#employeeDateOfBirth').val(),
            IsDriver: $('#employeeIsDriver').prop('checked'),
            Passport: $('#employeePassport').val().replace(/\s/g, '') || null,
            INN: $('#employeeINN').val() || null,
            AccountNumber: $('#employeeAccountNumber').val() || null,
            BIK: $('#employeeBIK').val() || null
        };
        const employeeId = $('#employeeId').val();
        const url = employeeId ? `/api/employees/base/${employeeId}` : '/api/employees/base';
        const method = employeeId ? 'PUT' : 'POST';

        if (employeeId) {
            employeeData.Id = employeeId;
        }

        $.ajax({
            url: url,
            method: method,
            contentType: 'application/json',
            data: JSON.stringify(employeeData),
            success: function () {
                $('#employeeModal').hide();
                module.loadEmployees(false, 'employeesTable');
                module.loadEmployees(true, 'dataTable');
            },
            error: function (xhr) {
                const errorMessage = xhr.responseText ? xhr.responseText : 'Ошибка при сохранении данных сотрудника';
                alert(errorMessage);
            }
        });
    };

    function formatDateForInput(date) {
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');
        return `${year}-${month}-${day}`;
    }

    function formatDateForOutput(date) {
        const day = String(date.getDate()).padStart(2, '0');
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const year = date.getFullYear();
        return `${day}.${month}.${year}`;
    }

    module.manageAccess = function() {
        AuthManagement.showElementIfHasRole('addProjectBtn', 'Admin');
        AuthManagement.showElementIfHasRole('addProductBtn', 'Admin');
        AuthManagement.showElementIfHasRole('addShiftBtn', 'Admin');
    }

    module.init = function () {
        $(document).ready(function () {
            if (!AuthManagement.checkAuth()) {
                return;
            }
            setupInputMasks();
            setupEventListeners();
            module.loadEmployees(false, 'employeesTable');
            module.loadEmployees(true, 'dataTable');
            module.manageAccess();
            
        });
    };

    return module;
})();

$(document).ready(function() {
    EmployeeManagement.init();
});