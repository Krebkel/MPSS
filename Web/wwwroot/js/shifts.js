let ShiftManagement = (function () {
    const module = {};

    let currentProjectId;
    let currentDate;

    module.openShiftModal = function (projectId, date) {
        currentProjectId = projectId;
        currentDate = new Date(date);
        $('#addShiftForm').hide();
        $('#shiftForm')[0].reset();
        $('#shiftForm').removeData('shiftId');
        currentDate.setUTCHours(0, 0, 0, 0);

        $('#shiftForm').attr('data-date', date);
        $('#shiftDate').val(date);

        $('#shiftModal').show();
        module.loadExistingShifts(projectId, currentDate);
        module.loadExistingEmployees();

        module.toggleFutureShiftFields(currentDate);
    };

    module.loadExistingShifts = function (projectId, date) {
        const shiftsTableBody = $('#shiftsTable tbody');
        shiftsTableBody.empty();

        $.ajax({
            url: `/api/employeeShifts/base/byProject/${projectId}`,
            method: 'GET',
            success: function (shifts) {
                const shiftRows = [];
                let currentIndex = 1;

                const employeePromises = shifts.map((shift) => {
                    const shiftDate = toLocal(new Date(shift.date));
                    shiftDate.setHours(0, 0, 0, 0);

                    if (shiftDate.toDateString() === date.toDateString()) {
                        return $.getJSON(`/api/employees/base/${shift.employee}`)
                            .then(employee => {
                                const shiftRow = `
                            <tr class="shift-row" data-shift-id="${shift.id}">
                                <td class="shortcol">${currentIndex++}</td>
                                <td>${employee.name}</td>
                                <td class="btncol">
                                    <button name="deleteShiftBtn" class="btn delete-btn" data-shift-id="${shift.id}">⛌</button>
                                </td>
                            </tr>`;
                                shiftRows.push(shiftRow);
                            });
                    }
                });

                Promise.all(employeePromises).then(() => {
                    shiftsTableBody.html(shiftRows.join(''));
                });
            },
            error: function () {
                alert('Ошибка при загрузке смен');
            }
        });
    };

    module.deleteEmployeeShift = function (employeeShiftId) {
        $.ajax({
            url: `/api/employeeShifts/base/${employeeShiftId}`,
            method: 'DELETE',
            success: function () {},
            error: function () {
                alert('Ошибка при удалении смены из проекта');
            }
        });
    };

    module.editShift = function (shiftId) {
        $.ajax({
            url: `/api/employeeShifts/base/${shiftId}`,
            method: 'GET',
            success: function (shift) {
                const shiftDate = new Date(shift.date);

                $('#addShiftForm').show();
                $('#expenseHeader').text('Редактировать смену');
                $('#employeeSelect').val(shift.employee);
                $('#shiftDate').val(formatDateForInput(shiftDate));
                $('#hoursWorked').val(shift.hoursWorked);

                module.toggleFutureShiftFields(shiftDate);

                if (shift.arrival) {
                    $('#arrivalTime').val(formatTimeForInput(new Date(shift.arrival)));
                } else {
                    $('#arrivalTime').val('');
                }

                if (shift.departure) {
                    $('#departureTime').val(formatTimeForInput(new Date(shift.departure)));
                } else {
                    $('#departureTime').val('');
                }

                $('#travelTime').val(shift.travelTime);
                $('#considerTravel').prop('checked', shift.considerTravel);
                $('#isn').val(shift.isn);

                $('#shiftForm').data('shiftId', shift.id);
            },
            error: function () {
                alert('Ошибка при загрузке данных смены');
            }
        });
    };

    module.toggleFutureShiftFields = function (date) {
        const dateToCheck = toUTC(new Date(date));
        const today = toUTC(new Date());
        dateToCheck.setUTCHours(0, 0, 0, 0);
        today.setUTCHours(0, 0, 0, 0);
        const showFields = dateToCheck <= today;
        $('#futureShiftFields').toggle(showFields);
    };

    module.loadExistingEmployees = function () {
        $.ajax({
            url: '/api/employees/base',
            method: 'GET',
            success: function (employees) {
                const employeeSelect = $('#employeeSelect');
                employeeSelect.empty();
                employeeSelect.append($('<option>').val('').text('Выберите сотрудника'));

                employees.forEach(employee => {
                    employeeSelect.append($('<option>').val(employee.id).text(employee.name));
                });
            },
            error: function () {
                alert('Ошибка при загрузке списка сотрудников');
            }
        });
    }

    module.init = function () {
        $(document).ready(function() {
            if (!AuthManagement.checkAuth()) {
                return;
            }
            $('#addShiftBtn').click(function () {
                $('#addShiftForm').show();
                $('#shiftForm')[0].reset();
                $('#shiftForm').removeData('shiftId');

                $('#arrivalTime').val('08:00');
                $('#departureTime').val('20:00');

                const dateAttr = $('#shiftForm').attr('data-date');
                if (dateAttr) {
                    const shiftDate = new Date(dateAttr);
                    $('#shiftDate').val(shiftDate.toISOString().split('T')[0]);
                } else {
                    $('#shiftDate').val(formatDateForInput(new Date()));
                }

                module.toggleFutureShiftFields(currentDate);
            });

            $('#shiftsTable').off('click', '.shift-row').on('click', '.shift-row', function () {
                const shiftId = $(this).data('shift-id');
                module.editShift(shiftId);
            });

            $('#shiftsTable').on('click', '.delete-btn', function (event) {
                event.stopPropagation();
                const shiftId = $(this).data('shift-id');
                module.deleteEmployeeShift(shiftId);
                ProjectManagement.updateAllShiftCounts();
                $(this).closest('td').remove();
            });

            $('#shiftDate').change(function () {
                const selectedDate = new Date($(this).val());
                module.toggleFutureShiftFields(selectedDate);
            });

            $('#cancelAddShift').click(function () {
                $('#addShiftForm').hide();
                $('#shiftForm')[0].reset().removeData('shiftId');
            });

            $('#shiftForm').submit(function (e) {
                e.preventDefault();
                const shiftData = {
                    project: currentProjectId,
                    employee: $('#employeeSelect').val(),
                    date: toUTC(new Date($('#shiftDate').val())).toISOString(),
                    hoursWorked: parseFloat($('#hoursWorked').val()) || null,
                    arrival: combineDateTime($('#shiftDate').val(), $('#arrivalTime').val()),
                    departure: combineDateTime($('#shiftDate').val(), $('#departureTime').val()),
                    travelTime: parseFloat($('#travelTime').val()) || null,
                    considerTravel: $('#considerTravel').is(':checked'),
                    isn: parseInt($('#isn').val()) || null
                };

                const shiftId = $(this).data('shiftId');
                const url = shiftId ? `/api/employeeShifts/base/${shiftId}` : '/api/employeeShifts/base';
                const method = shiftId ? 'PUT' : 'POST';

                if (shiftId) {
                    shiftData.id = shiftId;
                }

                $.ajax({
                    url: url,
                    method: method,
                    contentType: 'application/json',
                    data: JSON.stringify(shiftData),
                    success: function () {
                        $('#addShiftForm').hide();
                        $('#shiftForm')[0].reset();
                        module.loadExistingShifts(currentProjectId, new Date($('#shiftDate').val()));

                        ProjectManagement.updateShiftCount(currentProjectId, $('#shiftDate').val());
                        },
                    error: function () {
                        alert('Ошибка при сохранении смены');
                    }
                });
            });
        });
    };

    return module;
})();