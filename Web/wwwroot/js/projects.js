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

                projectRow += `<td class="gantt-cell" data-project-id="${project.id}" data-date="${currentDate.toISOString()}" style="background-color: ${cellColor};"></td>`;
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

    function openShiftModal(projectId, date) {
        const modal = $('#shiftModal');
        const shiftList = $('#shiftList');
        const addShiftForm = $('#addShiftForm');

        shiftList.empty();
        addShiftForm.empty();

        $.getJSON(`/api/employeeShifts/logic/project/${projectId}/date/${date}`, function(shifts) {
            if (shifts.length > 0) {
                shifts.forEach((shift) => {
                    shiftList.append(`<div>${shift.name}</div>`);
                });
            } else {
                shiftList.append('<div>Смен нет</div>');
            }

            addShiftForm.html(`
                <h4>Добавить смену</h4>
                <input type="text" id="shiftName" placeholder="Название смены">
                <button id="saveShiftBtn" class="btn btn-primary">Сохранить</button>
            `);

            $('#saveShiftBtn').on('click', function() {
                const shiftName = $('#shiftName').val();
                saveShift(projectId, date, shiftName);
            });
        });

        modal.show();
    }

    function saveShift(projectId, date, shiftName) {
        $.ajax({
            url: '/api/employeeShifts/base',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                projectId: projectId,
                date: date,
                name: shiftName
            }),
            success: function() {
                $('#shiftModal').hide();
                alert('Смена успешно добавлена');
            }
        });
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
            error: function(xhr) {
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
            error: function(xhr) {
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
            error: function(xhr) {
                alert('Ошибка при сохранении проекта');
            }
        });
    }

    $('.close').on('click', function() {
        $(this).closest('.modal').fadeOut();
    });

    $(window).on('click', function(event) {
        if ($(event.target).hasClass('modal')) {
            $(event.target).fadeOut();
        }
    });

    $('#projectForm').on('submit', function(event) {
        event.preventDefault();
        submitProjectForm();
    });

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
    
    loadProjects();
});