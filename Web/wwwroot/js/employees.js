$(document).ready(function() {

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
    
    function loadEmployees(fullData = false, tableId = 'employeesTable') {
        $.getJSON('/api/employees/base', function(employees) {
            const employeesTableBody = $(`#${tableId} tbody`);
            employeesTableBody.empty();

            employees.forEach((employee, index) => {
                const employeeRow = `
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
                employeesTableBody.append(employeeRow);
            });

            $(`.delete-btn`).off('click').on('click', function(event) {
                event.stopPropagation();
                const employeeId = $(this).data('employee-id');
                deleteEmployee(employeeId);
            });

            $(`.employee-row`).off('click').on('click', function() {
                const employeeId = $(this).data('employee-id');
                openEmployeeModal(employeeId);
            });
        });
    }

    function deleteEmployee(employeeId) {
        if (confirm('Вы уверены, что хотите удалить этого сотрудника?')) {
            $.ajax({
                url: `/api/employees/base/${employeeId}`,
                method: 'DELETE',
                success: function() {
                    alert('Сотрудник успешно удалён');
                    loadEmployees(false, 'employeesTable');
                    loadEmployees(true, 'dataTable');
                },
                error: function (xhr) {
                    const errorMessage = xhr.responseText ? xhr.responseText : 'Ошибка при удалении сотрудника';
                    alert(errorMessage);
                }
            });
        }
    }

    function openEmployeeModal(employeeId) {
        $.ajax({
            url: `/api/employees/base/${employeeId}`,
            type: 'GET',
            success: function(data) {
                $('#modalTitle').text('Редактировать сотрудника');
                $('#employeeId').val(employeeId);
                $('#employeeName').val(data.name);
                $('#employeePhone').val(data.phone);
                $('#employeePassport').val(data.passport ? data.passport.substring(0, 4) + ' ' + data.passport.substring(4) : '');
                $('#employeeDateOfBirth').val(formatDateForInput(new Date(data.dateOfBirth)));
                $('#employeeIsDriver').prop('checked', data.isDriver);
                $('#employeeINN').val(data.inn ? data.inn : '');
                $('#employeeAccountNumber').val(data.accountNumber ? data.accountNumber : '');
                $('#employeeBIK').val(data.bik ? data.bik : '');
                $('#employeeModal').fadeIn();
            },
            error: function(err) {
                console.error();
            }
        });
    }

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

    function submitEmployeeForm() {
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
                loadEmployees(false, 'employeesTable');
                loadEmployees(true, 'dataTable');
                alert(employeeId ? 'Сотрудник успешно обновлен' : 'Новый сотрудник успешно добавлен');
            },
            error: function (xhr) {
                const errorMessage = xhr.responseText ? xhr.responseText : 'Ошибка при сохранении данных сотрудника';
                alert(errorMessage);
            }
        });
    }
    
    $('#employeeForm').on('submit', function(event) {
        event.preventDefault();
        submitEmployeeForm();
    });
    
    $('#addEmployeeBtn, #addEmployeeBtn2').on('click', function() {
        $('#modalTitle').text('Добавить нового сотрудника');
        $('#employeeForm')[0].reset();
        $('#employeeId').val('');
        $('#employeeDateOfBirth').val(new Date().toISOString().split('T')[0]);
        $('#employeeModal').fadeIn();
    });

    loadEmployees(false, 'employeesTable');
    loadEmployees(true, 'dataTable');
});