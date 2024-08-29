$(document).ready(function() {
    function createGanttChart(projects) {
        const chartContainer = $('#ganttChart');
        chartContainer.empty();
        const today = new Date();
        const startDate = new Date();
        startDate.setDate(today.getDate() - 3);
        const endDate = new Date();
        endDate.setDate(today.getDate() + 30);

        let monthRow = '<tr><th rowspan="2">Проект</th>';
        let currentMonth = startDate.getMonth();
        let monthSpan = 0;

        for (let d = new Date(startDate); d <= endDate; d.setDate(d.getDate() + 1)) {
            if (d.getMonth() === currentMonth) {
                monthSpan++;
            } else {
                monthRow += `<th colspan="${monthSpan}" class="month-header">${getMonthName(currentMonth)}</th>`;
                currentMonth = d.getMonth();
                monthSpan = 1;
            }
        }
        monthRow += `<th colspan="${monthSpan}" class="month-header">${getMonthName(currentMonth)}</th>`;
        monthRow += '</tr>';

        let dayRow = '<tr>';
        for (let d = new Date(startDate); d <= endDate; d.setDate(d.getDate() + 1)) {
            const dayStyle = (d.getTime() === today.getTime()) ? 'style="background-color: #C6E7D4;"' : '';
            dayRow += `<th class="day-header" ${dayStyle}>${d.getDate().toString().padStart(2, '0')}</th>`;
        }
        dayRow += '</tr>';

        let bodyRows = '';
        projects.forEach((project) => {
            let projectRow = `<tr><td>${project.name}</td>`;
            for (let d = new Date(startDate); d <= endDate; d.setDate(d.getDate() + 1)) {
                const currentDate = new Date(d);
                currentDate.setHours(0, 0, 0, 0);
                const startDateNormalized = new Date(project.startDate);
                startDateNormalized.setHours(0, 0, 0, 0);
                const deadlineDateNormalized = new Date(project.deadlineDate);
                deadlineDateNormalized.setHours(0, 0, 0, 0);
                const isWithinPeriod = currentDate >= startDateNormalized && currentDate <= deadlineDateNormalized;
                let cellColor = '';
                if (isWithinPeriod) {
                    switch (project.projectStatus) {
                        case 'Active': cellColor = '#FABCB2'; break;
                        case 'Standby': cellColor = '#DAD4D2'; break;
                        case 'Done': cellColor = '#C6E7D4'; break;
                        case 'Paid': cellColor = '#B6D1F6'; break;
                        default: cellColor = '';
                    }
                }
                const localDateString = currentDate.toLocaleDateString('en-CA');
                projectRow += `<td class="gantt-cell" data-project-id="${project.id}" data-date="${localDateString}" style="background-color: ${cellColor};">
                <span class="shift-count" style="color: #6F605D;"></span>
            </td>`;
            }
            projectRow += '</tr>';
            bodyRows += projectRow;
        });

        chartContainer.html(`
        <table class="table table-bordered">
            <thead>
                ${monthRow}
                ${dayRow}
            </thead>
            <tbody>
                ${bodyRows}
            </tbody>
        </table>
    `);

        $('.gantt-cell').on('click', function() {
            const projectId = $(this).data('project-id');
            const date = $(this).data('date');
            openShiftModal(projectId, date);
        });

        updateAllShiftCounts();
    }

    function updateAllShiftCounts() {
        $('.gantt-cell').each(function() {
            const projectId = $(this).data('project-id');
            const date = $(this).data('date');
            updateShiftCount(projectId, date);
        });
    }

    function updateShiftCount(projectId, date) {
        $.ajax({
            url: `/api/employeeShifts/base/byProject/${projectId}`,
            method: 'GET',
            success: function(shifts) {
                const shiftCount = shifts.filter(shift => {
                    const shiftDate = toLocal(new Date(shift.date));
                    return shiftDate.toLocaleDateString('en-CA') === date;
                }).length;

                $(`.gantt-cell[data-project-id="${projectId}"][data-date="${date}"] .shift-count`).text(shiftCount > 0 ? shiftCount : '');
            },
            error: function() {
                console.error('Error loading shifts for count update');
            }
        });
    }

    function getMonthName(monthIndex) {
        const monthNames = [
            'Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь',
            'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'
        ];
        return monthNames[monthIndex];
    }

    function loadProjects() {
        $.getJSON('/api/counteragents/base', function(counteragents) {
            const counteragentSelect = $('#projectCounteragent');
            counteragentSelect.empty(); 
            counteragents.forEach(counteragent => {
                counteragentSelect.append(new Option(counteragent.name, counteragent.id));
            });
        });

        $.getJSON('/api/employees/base', function(employees) {
            const employeeSelect = $('#projectResponsibleEmployee');
            employeeSelect.empty();
            employees.forEach(employee => {
                employeeSelect.append(new Option(employee.name, employee.id));
            });
        });

        $.getJSON('/api/projects/base', function(allProjects) {
            const today = new Date();
            const startDate = new Date(today);
            startDate.setDate(today.getDate() - 3);
            const endDate = new Date(today);
            endDate.setDate(today.getDate() + 30);

            const filteredProjects = allProjects.filter(project => {
                const deadlineDate = new Date(project.deadlineDate);
                return deadlineDate >= startDate && deadlineDate <= endDate;
            });

            createGanttChart(filteredProjects);
            const projectsTableBody = $('#projectsTable tbody');
            projectsTableBody.empty();

            filteredProjects.forEach((project, index) => {
                const projectRow = `
                <tr class="project-row" data-project-id="${project.id}">
                    <td class="shortcol">${index + 1}</td>
                    <td>${project.name}</td>
                    <td class="longcol">${project.address}</td>
                    <td class="midcol">${formatDateForOutput(new Date(project.deadlineDate))}</td>
                    <td class="midcol">${translateStatus(project.projectStatus)}</td>
                    <td class="btncol">
                        <button class="btn delete-btn" data-project-id="${project.id}">⛌</button>
                    </td>
                </tr>
            `;
                projectsTableBody.append(projectRow);
            });

            $('.delete-btn').on('click', function(event) {
                event.stopPropagation();
                const projectId = $(this).data('project-id');
                deleteProject(projectId);
            });

            $('.project-row').on('click', function() {
                const projectId = $(this).data('project-id');
                openProjectModal(projectId);
            });
        });
    }
    
    function deleteProject(projectId) {
        if (confirm('Вы уверены, что хотите удалить этот проект?')) {
            $.ajax({
                url: `/api/projects/base/${projectId}`,
                method: 'DELETE',
                success: function() {
                    alert('Проект успешно удален');
                    loadProjects();
                },
                error: function (xhr) {
                    const errorMessage = xhr.responseText ? xhr.responseText : 'Ошибка при удалении проекта';
                    alert(errorMessage);
                }
            });
        }
    }
    
    async function openProjectModal(projectId) {
        const modal = $('#projectModal');
        const form = $('#projectForm')[0];
        form.reset();
        $('#projectProductsTable tbody').empty();

        try {
            const availableProducts = await $.ajax({
                url: `/api/products/base`,
                type: 'GET',
            });
            
            if (projectId) {
                const projectData = await $.ajax({
                    url: `/api/projects/base/${projectId}`,
                    type: 'GET',
                });

                $('#modalTitle').text('Редактировать проект');
                $('#projectId').val(projectData.id);
                $('#projectName').val(projectData.name);
                $('#projectAddress').val(projectData.address);
                $('#projectStartDate').val(formatDateForInput(new Date(projectData.startDate)));
                $('#projectDeadline').val(formatDateForInput(new Date(projectData.deadlineDate)));
                $('#projectCounteragent').val(projectData.counteragent ? projectData.counteragent.id : '');
                $('#projectResponsibleEmployee').val(projectData.responsibleEmployee ? projectData.responsibleEmployee.id : '');
                $('#projectManagerShare').val(projectData.managerShare);
                $('#projectStatus').val(projectData.projectStatus);
                
                const projectProducts = await $.ajax({
                    url: `/api/projectProducts/base/byProject/${projectId}`,
                    type: 'GET',
                });

                projectProducts.forEach(projectProduct => addProjectProductRow(projectProduct, availableProducts));
            } else {
                $('#modalTitle').text('Добавить новый проект');
                $('#projectId').val('');
                }
            
            $('#addProjectProductBtn').on('click', function() {
                addProjectProductRow({}, availableProducts);
            });
            
            modal.fadeIn();
        } catch (error) {
            alert('Ошибка при загрузке данных: ' + (error.responseText || error.statusText));
        }
    }

    function addProjectProductRow(projectProduct = {}, availableProducts = []) {
        const productOptions = availableProducts.map(product =>
            `<option value="${product.id}" ${product.id === (projectProduct.product ? projectProduct.product.id : '') ? 'selected' : ''}>${product.name}</option>`
        ).join('');

        const projectProductRow = `
            <tr class="project-product-row" data-project-product-id="${projectProduct.id || ''}">
                <td class="longcol">
                    <select name="projectProduct[]">${productOptions}</select>
                </td>
                <td class="shortcol">
                    <input type="number" name="projectQuantity[]" value="${projectProduct.quantity || ''}" required min="1" max="999999">
                </td>
                <td class="midcol">
                    <input type="number" name="projectMarkup[]" value="${projectProduct.markup || ''}" required step="0.01" min="0" max="9999999999.99">
                </td>
                <td class="btncol"><button type="button" class="btn delete-btn">⛌</button></td>
            </tr>
        `;

        $('#projectProductsTable tbody').append(projectProductRow);

        $('.delete-btn').off('click').on('click', function() {
            const projectProductId = $(this).closest('.project-product-row').data('project-product-id');
            if (projectProductId) {
                deleteProjectProduct(projectProductId);
            }
            $(this).closest('.project-product-row').remove();
        });
    }

    function deleteProjectProduct(projectProductId) {
        $.ajax({
            url: `/api/projectProducts/base/${projectProductId}`,
            method: 'DELETE',
            success: function() {
                alert('Изделие успешно удалено из проекта');
            },
            error: function() {
                alert('Ошибка при удалении изделия из проекта');
            }
        });
    }
    
    function saveProjectProducts(projectId) {
        const projectProducts = [];
        $('.project-product-row').each(function() {
            const projectProduct = {
                project: projectId,
                product: $(this).find('select[name="projectProduct[]"]').val(),
                quantity: parseInt($(this).find('input[name="projectQuantity[]"]').val()) || null,
                markup: parseFloat($(this).find('input[name="projectMarkup[]"]').val()) || null
            };

            if ($(this).data('project-product-id')) {
                projectProduct.id = $(this).data('project-product-id');
            }

            projectProducts.push(projectProduct);
        });

        $.ajax({
            url: '/api/projectProducts/base',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(projectProducts),
            success: function() {
                $('#projectModal').hide();
                loadProjects();
                alert('Проект и изделия успешно сохранены');
            },
            error: function() {
                alert('Ошибка при сохранении изделий проекта');
            }
        });
    }
    
    function submitProjectForm() {
        const projectData = {
            name: $('#projectName').val(),
            address: $('#projectAddress').val(),
            startDate: $('#projectStartDate').val(),
            deadlineDate: $('#projectDeadline').val(),
            counteragent: $('#projectCounteragent').val() || null,
            responsibleEmployee: $('#projectResponsibleEmployee').val(),
            managerShare: $('#projectManagerShare').val(),
            projectStatus: $('#projectStatus').val(),
        };

        const projectId = $('#projectId').val();
        const url = projectId ? `/api/projects/base/${projectId}` : '/api/projects/base';
        const method = projectId ? 'PUT' : 'POST';

        $.ajax({
            url: url,
            method: method,
            contentType: 'application/json',
            data: JSON.stringify(projectData),
            success: function(response) {
                const newProjectId = projectId || response.id;
                saveProjectProducts(newProjectId);
                $('#projectModal').hide();
                loadProjects();
                alert(projectId ? 'Проект успешно обновлен' : 'Проект успешно добавлен');
            },
            error: function() {
                alert('Ошибка при сохранении проекта');
            }
        });
    }

    $('#projectForm').on('submit', function(event) {
        event.preventDefault();
        submitProjectForm();
    });

    function formatDateForOutput(date) {
        const day = String(date.getDate()).padStart(2, '0');
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const year = date.getFullYear();
        return `${day}.${month}.${year}`;
    }

    function translateStatus(status) {
        const statuses = {
            'Active': 'Активен',
            'Standby': 'Ожидание',
            'Done': 'Завершен',
            'Paid': 'Оплачен'
        };
        return statuses[status] || 'Неизвестный статус';
    }

    $('#addProjectBtn').on('click', function() {
        $('#modalTitle').text('Добавить новый проект');
        $('#projectForm')[0].reset();
        $('#projectId').val('');
        $('#projectStartDate').val(new Date().toISOString().split('T')[0]);
        $('#projectDeadline').val(new Date().toISOString().split('T')[0]);
        openProjectModal();
    });

// EmployeeShifts //

    // Модальное окно для работы со сменами
    function openShiftModal(projectId, date) {
        currentProjectId = projectId;
        currentDate = new Date(date);
        $('#addShiftForm').hide();
        $('#shiftForm')[0].reset();
        $('#shiftForm').removeData('shiftId');
        currentDate.setUTCHours(0, 0, 0, 0);

        $('#shiftForm').attr('data-date', date);
        $('#shiftDate').val(date);

        $('#shiftModal').show();
        loadExistingShifts(projectId, currentDate);
        loadAvailableEmployees();
        
        toggleFutureShiftFields(currentDate);
    }

    // Получаем список смен по Id проекта и дате
    function loadExistingShifts(projectId, date) {
        const shiftsTableBody = $('#shiftsTable tbody');
        shiftsTableBody.empty();

        $.ajax({
            url: `/api/employeeShifts/base/byProject/${projectId}`,
            method: 'GET',
            success: function(shifts) {
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
                                    <button class="btn delete-btn" data-shift-id="${shift.id}">⛌</button>
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
            error: function() {
                alert('Ошибка при загрузке смен');
            }
        });
    }

    // Удаляем смену
    function deleteEmployeeShift(employeeShiftId) {
        $.ajax({
            url: `/api/employeeShifts/base/${employeeShiftId}`,
            method: 'DELETE',
            success: function() {
                alert('Смена успешно удалена из проекта');
            },
            error: function() {
                alert('Ошибка при удалении смены из проекта');
            }
        });
    }

    // Получаем список всех сотрудников
    function loadAvailableEmployees() {
        $.ajax({
            url: '/api/employees/base',
            method: 'GET',
            success: function(employees) {
                const employeeSelect = $('#employeeSelect');
                employeeSelect.empty();
                employeeSelect.append($('<option>').val('').text('Выберите сотрудника'));

                employees.forEach(employee => {
                    employeeSelect.append($('<option>').val(employee.id).text(employee.name));
                });
            },
            error: function() {
                alert('Ошибка при загрузке списка сотрудников');
            }
        });
    }

    // Получаем данные смены для редактирования
    function editShift(shiftId) {
        $.ajax({
            url: `/api/employeeShifts/base/${shiftId}`,
            method: 'GET',
            success: function(shift) {
                const shiftDate = new Date(shift.date);

                $('#addShiftForm').show();
                $('#employeeSelect').val(shift.employee);
                $('#shiftDate').val(formatDateForInput(shiftDate));
                $('#hoursWorked').val(shift.hoursWorked);

                toggleFutureShiftFields(shiftDate);

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
            error: function() {
                alert('Ошибка при загрузке данных смены');
            }
        });
    }

    // Self-explanatory, нажимаем на плюс, появляется чистая форма создания смены на эту дату
    $('#addShiftBtn').click(function() {
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
        
        toggleFutureShiftFields(currentDate);
    });

    $('#shiftsTable').off('click', '.shift-row').on('click', '.shift-row', function() {
        const shiftId = $(this).data('shift-id');
        editShift(shiftId);
    });

    $('#shiftsTable').on('click', '.delete-btn', function(event) {
        event.stopPropagation();
        const shiftId = $(this).data('shift-id');
        deleteEmployeeShift(shiftId);
        updateAllShiftCounts();
        $(this).closest('tr').remove();
    });

    // Вдруг мы решим на будущее смену добавить, тогда поля "когда приехал" и "когда свалил" не потребуются
    $('#shiftDate').change(function() {
        const selectedDate = new Date($(this).val());
        toggleFutureShiftFields(selectedDate);
    });

    // Отминет создание, сворачиваемся
    $('#cancelAddShift').click(function() {
        $('#addShiftForm').hide();
        $('#shiftForm')[0].reset().removeData('shiftId');
    });

    // Сохранение данных смены из формы
    $('#shiftForm').submit(function(e) {
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
            success: function() {
                $('#addShiftForm').hide();
                $('#shiftForm')[0].reset();
                loadExistingShifts(currentProjectId, new Date($('#shiftDate').val()));

                updateShiftCount(currentProjectId, $('#shiftDate').val());

                alert(shiftId ? 'Смена успешно обновлена' : 'Смена успешно добавлена');
            },
            error: function() {
                alert('Ошибка при сохранении смены');
            }
        });
    });

    // Функция для сокрытия полей ввода для будущих смен
    function toggleFutureShiftFields(date) {
        const dateToCheck = toUTC(new Date(date));
        const today = toUTC(new Date());
        dateToCheck.setUTCHours(0, 0, 0, 0);
        today.setUTCHours(0, 0, 0, 0);
        const showFields = dateToCheck <= today;
        $('#futureShiftFields').toggle(showFields);
    }

    // Функция для объединения даты и времени в UTC DateTimeOffset
    function combineDateTime(date, time) {
        if (!date || !time) return null;
        const [year, month, day] = date.split('-');
        const [hours, minutes] = time.split(':');
        const localDateTime = new Date(year, month - 1, day, hours, minutes);
        return toUTC(localDateTime).toISOString();
    }

    // Получаем смещение часового пояса клиента в минутах
    const clientTimezoneOffset = new Date().getTimezoneOffset();

    // Функция для преобразования локальной даты в UTC
    function toUTC(localDate) {
        return new Date(localDate.getTime() - clientTimezoneOffset * 60000);
    }

    // Функция для преобразования UTC в локальную дату
    function toLocal(utcDate) {
        return new Date(utcDate.getTime() + clientTimezoneOffset * 60000);
    }

    // Функция для форматирования даты в строку для input[type="date"]
    function formatDateForInput(date) {
        const localDate = toLocal(new Date(date));
        return localDate.toISOString().split('T')[0];
    }

    // Функция для форматирования времени в строку для input[type="time"]
    function formatTimeForInput(dateTimeOffset) {
        if (!dateTimeOffset) return '';
        const localDate = toLocal(new Date(dateTimeOffset));
        return `${String(localDate.getHours()).padStart(2, '0')}:${String(localDate.getMinutes()).padStart(2, '0')}`;
    }
    
    loadProjects();
});