$(document).ready(function() {
    
    function loadEmployees() {
        $.getJSON('/api/employees', function(employees) {
            const employeesTableBody = $('#employeesTable tbody');
            employeesTableBody.empty();

            employees.sort((a, b) => a.Name.localeCompare(b.Name)).forEach((employee, index) => {
                const employeeRow = `
                    <tr class="employee-row" data-employee-id="${employee.Id}">
                        <td class="shortcol">${index + 1}</td>
                        <td class="midcol">${employee.Name}</td>
                        <td class="midcol">${employee.Phone}</td>
                        <td class="midcol">${new Date(employee.DateOfBirth).toLocaleDateString()}</td>
                        <td class="midcol">${employee.IsDriver ? 'Да' : 'Нет'}</td>
                        <td class="midcol">
                            <button class="btn btn-danger delete-employee-btn" data-employee-id="${employee.Id}">Удалить</button>
                        </td>
                    </tr>
                `;
                employeesTableBody.append(employeeRow);
            });

            $('.delete-employee-btn').on('click', function(event) {
                event.stopPropagation();
                const employeeId = $(this).data('employee-id');
                deleteEmployee(employeeId);
            });

            $('.employee-row').on('click', function() {
                const employeeId = $(this).data('employee-id');
                openEmployeeModal(employeeId);
            });
        });
    }

    function loadData() {
        $.getJSON('/api/employees', function(employees) {
            const dataTableBody = $('#dataTable tbody');
            dataTableBody.empty();

            employees.sort((a, b) => a.Name.localeCompare(b.Name)).forEach((employee, index) => {
                const dataRow = `
                    <tr class="data-row" data-employee-id="${employee.Id}">
                        <td class="shortcol">${index + 1}</td>
                        <td class="midcol">${employee.Name}</td>
                        <td class="midcol">${employee.Phone}</td>
                        <td class="midcol">${new Date(employee.DateOfBirth).toLocaleDateString()}</td>
                        <td class="midcol">${employee.Passport ? `${Math.floor(employee.Passport / 10000000)} ${employee.Passport % 10000000}` : ''}</td>
                        <td class="midcol">${employee.INN || ''}</td>
                        <td class="midcol">${employee.AccountNumber || ''}</td>
                        <td class="midcol">${employee.BIK || ''}</td>
                        <td class="midcol">
                            <button class="btn btn-danger delete-data-btn" data-employee-id="${employee.Id}">Удалить</button>
                        </td>
                    </tr>
                `;
                dataTableBody.append(dataRow);
            });

            $('.delete-data-btn').on('click', function(event) {
                event.stopPropagation();
                const employeeId = $(this).data('employee-id');
                deleteEmployee(employeeId);
            });

            $('.data-row').on('click', function() {
                const employeeId = $(this).data('employee-id');
                openDataModal(employeeId);
            });
        });
    }

    function deleteEmployee(employeeId) {
        if (confirm('Вы уверены, что хотите удалить этого сотрудника?')) {
            $.ajax({
                url: `/api/employees/${employeeId}`,
                method: 'DELETE',
                success: function() {
                    alert('Сотрудник успешно удалён');
                    loadEmployees();
                    loadData();
                }
            });
        }
    }

    function openEmployeeModal(employeeId) {
        $.ajax({
            url: `/api/employees/${employeeId}`,
            type: 'GET',
            success: function(data) {
                $('#modalTitle').text('Редактировать сотрудника');
                $('#employeeId').val(data.Id);
                $('#employeeName').val(data.Name);
                $('#employeePhone').val(data.Phone);
                $('#employeeDateOfBirth').val(formatDateForInput(new Date(data.DateOfBirth)));
                $('#employeeIsDriver').prop('checked', data.IsDriver);
                $('#employeeModal').fadeIn();
            },
            error: function(err) {
                console.error('Ошибка при получении данных сотрудника:', err);
            }
        });
    }

    function openDataModal(employeeId) {
        $.ajax({
            url: `/api/employees/${employeeId}`,
            type: 'GET',
            success: function(data) {
                $('#dataModalTitle').text('Редактировать данные сотрудника');
                $('#dataId').val(data.Id);
                $('#dataName').val(data.Name);
                $('#dataPhone').val(data.Phone);
                $('#dataDateOfBirth').val(formatDateForInput(new Date(data.DateOfBirth)));
                $('#dataPassport').val(data.Passport ? `${Math.floor(data.Passport / 10000000)} ${data.Passport % 10000000}` : '');
                $('#dataINN').val(data.INN || '');
                $('#dataAccountNumber').val(data.AccountNumber || '');
                $('#dataBIK').val(data.BIK || '');
                $('#dataModal').fadeIn();
            },
            error: function(err) {
                console.error('Ошибка при получении данных сотрудника:', err);
            }
        });
    }

    function formatDateForInput(date) {
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');
        return `${year}-${month}-${day}`;
    }

    function submitEmployeeForm() {
        const employeeData = {
            Name: $('#employeeName').val(),
            Phone: $('#employeePhone').val(),
            DateOfBirth: $('#employeeDateOfBirth').val(),
            IsDriver: $('#employeeIsDriver').prop('checked')
        };
        const employeeId = $('#employeeId').val();
        const url = employeeId ? `/api/employees/${employeeId}` : '/api/employees';
        const method = employeeId ? 'PUT' : 'POST';

        $.ajax({
            url: url,
            method: method,
            contentType: 'application/json',
            data: JSON.stringify(employeeData),
            success: function () {
                $('#employeeModal').hide();
                loadEmployees();
                alert(employeeId ? 'Сотрудник успешно обновлен' : 'Новый сотрудник успешно добавлен');
            },
            error: function () {
                alert('Ошибка при сохранении данных сотрудника');
            }
        });
    }

    function submitDataForm() {
        const dataData = {
            Name: $('#dataName').val(),
            Phone: $('#dataPhone').val(),
            DateOfBirth: $('#dataDateOfBirth').val(),
            Passport: parsePassport($('#dataPassport').val()),
            INN: $('#dataINN').val(),
            AccountNumber: $('#dataAccountNumber').val(),
            BIK: $('#dataBIK').val()
        };
        const dataId = $('#dataId').val();
        const url = dataId ? '/api/employees/${dataId}' : '/api/employees';
        const method = dataId ? 'PUT' : 'POST';

        $.ajax({
            url: url,
            method: method,
            contentType: 'application/json',
            data: JSON.stringify(dataData),
            success: function () {
                $('#dataModal').hide();
                loadData();
                alert(dataId ? 'Данные сотрудника успешно обновлены' : 'Данные нового сотрудника успешно добавлены');
            },
            error: function () {
                alert('Ошибка при сохранении данных сотрудника');
            }
        });
    }
    
    function parsePassport(passportString) {
        if (!passportString) return null;
        const parts = passportString.split(' ');
        if (parts.length !== 2) return null;
        const series = parseInt(parts[0]);
        const number = parseInt(parts[1]);
        if (isNaN(series) || isNaN(number)) return null;
        return series * 10000000 + number;
    }

    $('.close').on('click', function() {
        $(this).closest('.modal').fadeOut();
    });
    $(window).on('click', function(event) {
        if ($(event.target).hasClass('modal')) {
            $(event.target).fadeOut();
        }
    });
    
    $('#employeeForm').on('submit', function(event) {
        event.preventDefault();
        submitEmployeeForm();
    });
    $('#dataForm').on('submit', function(event) {
        event.preventDefault();
        submitDataForm();
    });
    $('#addEmployeeBtn').on('click', function() {
        $('#modalTitle').text('Добавить нового сотрудника');
        $('#employeeForm')[0].reset();
        $('#employeeId').val('');
        $('#employeeDateOfBirth').val(new Date().toISOString().split('T')[0]);
        $('#employeeModal').fadeIn();
    });
    $('#addDataBtn').on('click', function() {
        $('#dataModalTitle').text('Добавить данные нового сотрудника');
        $('#dataForm')[0].reset();
        $('#dataId').val('');
        $('#dataDateOfBirth').val(new Date().toISOString().split('T')[0]);
        $('#dataModal').fadeIn();
    });
});