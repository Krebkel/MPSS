let ProjectManagement = (function () {
    const module = {};

    module.createGanttChart = function (projects) {
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
                        case 'Active':
                            cellColor = '#FABCB2';
                            break;
                        case 'Standby':
                            cellColor = '#DAD4D2';
                            break;
                        case 'Done':
                            cellColor = '#C6E7D4';
                            break;
                        case 'Paid':
                            cellColor = '#B6D1F6';
                            break;
                        default:
                            cellColor = '';
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

        $('.gantt-cell').on('click', function () {
            const projectId = $(this).data('project-id');
            const date = $(this).data('date');
            ShiftManagement.openShiftModal(projectId, date);
        });

        module.loadAllShifts(projects, startDate, endDate);
    };

    module.loadAllShifts = function (projects, startDate, endDate) {
        const projectIds = projects.map(project => project.id);
        $.ajax({
            url: '/api/employeeShifts/base/byProjects',
            method: 'GET',
            data: {
                projectIds: projectIds.join(','),
                startDate: startDate.toISOString(),
                endDate: endDate.toISOString()
            },
            success: function (allShifts) {
                module.updateAllShiftCounts(allShifts);
            },
            error: function () {
                console.error('Error loading shifts');
            }
        });
    };

    module.updateAllShiftCounts = function (allShifts) {
        $('.gantt-cell').each(function () {
            const projectId = $(this).data('project-id');
            const date = $(this).data('date');
            const shiftCount = allShifts.filter(shift =>
                shift.projectId === projectId &&
                toLocal(new Date(shift.date)).toLocaleDateString('en-CA') === date
            ).length;

            $(this).find('.shift-count').text(shiftCount > 0 ? shiftCount : '');
        });
    };

    module.updateShiftCount = function (projectId, date) {
        $.ajax({
            url: `/api/employeeShifts/base/byProject/${projectId}`,
            method: 'GET',
            success: function (shifts) {
                const shiftCount = shifts.filter(shift => {
                    const shiftDate = toLocal(new Date(shift.date));
                    return shiftDate.toLocaleDateString('en-CA') === date;
                }).length;

                $(`.gantt-cell[data-project-id="${projectId}"][data-date="${date}"] .shift-count`).text(shiftCount > 0 ? shiftCount : '');
            },
            error: function () {
                console.error('Error loading shifts for count update');
            }
        });
    };

    module.loadProjects = function () {
        $.getJSON('/api/counteragents/base', function (counteragents) {
            const counteragentSelect = $('#projectCounteragent');
            counteragentSelect.empty();
            counteragents.forEach(counteragent => {
                counteragentSelect.append(new Option(counteragent.name, counteragent.id));
            });
        });

        $.getJSON('/api/employees/base', function (employees) {
            const employeeSelect = $('#projectResponsibleEmployee');
            employeeSelect.empty();
            employees.forEach(employee => {
                employeeSelect.append(new Option(employee.name, employee.id));
            });
        });

        $.getJSON('/api/projects/base', function (allProjects) {
            const today = new Date();
            const startDate = new Date(today);
            startDate.setDate(today.getDate() - 3);
            const endDate = new Date(today);
            endDate.setDate(today.getDate() + 30);

            const filteredProjects = allProjects.filter(project => {
                const deadlineDate = new Date(project.deadlineDate);
                return deadlineDate >= startDate && deadlineDate <= endDate;
            });

            module.createGanttChart(filteredProjects);
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

            $('.delete-btn').on('click', function (event) {
                event.stopPropagation();
                const projectId = $(this).data('project-id');
                module.deleteProject(projectId);
            });

            $('.project-row').on('click', function () {
                const projectId = $(this).data('project-id');
                module.openProjectModal(projectId);
            });
        });
    };

    module.deleteProject = function (projectId) {
        if (confirm('Вы уверены, что хотите удалить этот проект?')) {
            $.ajax({
                url: `/api/projects/base/${projectId}`,
                method: 'DELETE',
                success: function () {
                    alert('Проект успешно удален');
                    module.loadProjects();
                },
                error: function (xhr) {
                    const errorMessage = xhr.responseText ? xhr.responseText : 'Ошибка при удалении проекта';
                    alert(errorMessage);
                }
            });
        }
    };

    module.openProjectModal = async function (projectId) {
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
                $('#projectStartDate').val(toUTC(new Date(projectData.startDate)).toISOString().split('T')[0]);
                $('#projectDeadline').val(toUTC(new Date(projectData.deadlineDate)).toISOString().split('T')[0]);
                $('#projectCounteragent').val(projectData.counteragent);
                $('#projectResponsibleEmployee').val(projectData.responsibleEmployee);
                $('#projectManagerShare').val(projectData.managerShare);
                $('#projectStatus').val(projectData.projectStatus);

                const projectProducts = await $.ajax({
                    url: `/api/projectProducts/base/byProject/${projectId}`,
                    type: 'GET',
                });

                projectProducts.forEach(projectProduct => module.addProjectProductRow(projectProduct, availableProducts));
            } else {
                $('#modalTitle').text('Добавить новый проект');
                $('#projectId').val('');
            }

            $('#addProjectProductBtn').off('click').on('click', function () {
                module.addProjectProductRow({}, availableProducts);
            });

            modal.fadeIn();
        } catch (error) {
            alert('Ошибка при загрузке данных: ' + (error.responseText || error.statusText));
        }
    };

    module.addProjectProductRow = function (projectProduct = {}, availableProducts = []) {
        const productOptions = availableProducts.map(product =>
            `<option value="${product.id}" ${product.id === (projectProduct.product ? projectProduct.product : '') ? 'selected' : ''}>${product.name}</option>`
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

        $('.delete-btn').off('click').on('click', function () {
            const projectProductId = $(this).closest('.project-product-row').data('project-product-id');
            if (projectProductId) {
                module.deleteProjectProduct(projectProductId);
            }
            $(this).closest('.project-product-row').remove();
        });
    };

    module.deleteProjectProduct = function (projectProductId) {
        $.ajax({
            url: `/api/projectProducts/base/${projectProductId}`,
            method: 'DELETE',
            success: function () {
                alert('Изделие успешно удалено из проекта');
            },
            error: function () {
                alert('Ошибка при удалении изделия из проекта');
            }
        });
    };

    module.saveProjectProducts = function (projectId) {
        const projectProducts = $('.project-product-row').map(function () {
            const $this = $(this);
            return {
                id: $this.data('project-product-id') || null,
                project: projectId,
                product: $this.find('select[name="projectProduct[]"]').val(),
                quantity: parseInt($this.find('input[name="projectQuantity[]"]').val()) || null,
                markup: parseFloat($this.find('input[name="projectMarkup[]"]').val()) || null
            };
        }).get().filter(pp => pp.product);

        const deletePromises = $('[data-deleted-project-product-id]')
            .map((_, el) => $.ajax({
                url: `/api/projectProducts/base/${$(el).data('deleted-project-product-id')}`,
                method: 'DELETE'
            })).get();

        Promise.all(deletePromises)
            .then(() => Promise.all(projectProducts.map(pp => $.ajax({
                url: pp.id ? `/api/projectProducts/base/${pp.id}` : '/api/projectProducts/base',
                method: pp.id ? 'PUT' : 'POST',
                contentType: 'application/json',
                data: JSON.stringify(pp)
            }))))
            .then(() => {
                $('#projectModal').hide();
                module.loadProjects();
                alert('Проект и изделия успешно сохранены');
            })
            .catch(error => {
                alert('Ошибка при сохранении или удалении изделий проекта');
            });
    };

    module.submitProjectForm = function () {
        const startDate = new Date($('#projectStartDate').val());
        const deadlineDate = new Date($('#projectDeadline').val());

        if (deadlineDate < startDate) {
            alert('Дата окончания не может быть раньше даты начала проекта.');
            return;
        }
        const projectId = $('#projectId').val();

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

        if (projectId) {
            projectData.id = projectId;
        }

        const url = projectId ? `/api/projects/base/${projectId}` : '/api/projects/base';
        const method = projectId ? 'PUT' : 'POST';

        $.ajax({
            url: url,
            method: method,
            contentType: 'application/json',
            data: JSON.stringify(projectData),
            success: function (response) {
                const newProjectId = projectId || response.id;
                module.saveProjectProducts(newProjectId);
                $('#projectModal').hide();
                alert(projectId ? 'Проект успешно обновлен' : 'Проект успешно добавлен');
            },
            error: function () {
                alert('Ошибка при сохранении проекта');
            }
        });
    };

    module.init = function () {
        $(document).ready(function () {
            $('#projectForm').on('submit', function (event) {
                event.preventDefault();
                module.submitProjectForm();
            });

            $('#addProjectBtn').on('click', function () {
                $('#modalTitle').text('Добавить новый проект');
                $('#projectForm')[0].reset();
                $('#projectId').val('');
                $('#projectStartDate').val(new Date().toISOString().split('T')[0]);
                $('#projectDeadline').val(new Date().toISOString().split('T')[0]);
                module.openProjectModal();
            });

            module.loadProjects();
        });
    };

    return module;
})();

$(document).ready(function() {
    ProjectManagement.init();
    ShiftManagement.init();
});