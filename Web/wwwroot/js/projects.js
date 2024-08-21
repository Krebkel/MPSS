$(document).ready(function() {
    
    function createGanttChart(projects) {
        const chartContainer = $('#ganttChart');
        chartContainer.empty();
        const today = new Date();
        const startDate = new Date();
        startDate.setDate(today.getDate() - 15);
        const endDate = new Date();
        endDate.setDate(today.getDate() + 15);

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
        $.getJSON('/api/projects', function(projects) {
            createGanttChart(projects);
            const projectsTableBody = $('#projectsTable tbody');
            projectsTableBody.empty();

            projects.forEach((project, index) => {
                const projectRow = `
                <tr class="project-row" data-project-id="${project.id}">
                    <td class="shortcol">${index + 1}</td>
                    <td class="midcol">${project.name}</td>
                    <td class="longcol">${project.address}</td>
                    <td class="midcol">${formatDateForOutput(new Date(project.deadlineDate))}</td>
                    <td class="midcol">${translateStatus(project.projectStatus)}</td>
                    <td class="midcol">
                        <button class="btn btn-danger delete-project-btn" data-project-id="${project.id}">Удалить</button>
                    </td>
                </tr>
            `;
                projectsTableBody.append(projectRow);
            });

            $('.delete-project-btn').on('click', function(event) {
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

    function translateStatus(status) {
        const statuses = {
            'Active': 'Активен',
            'Standby': 'Ожидание',
            'Done': 'Завершен',
            'Paid': 'Оплачен'
        };
        return statuses[status] || 'Неизвестный статус';
    }

    function deleteProject(projectId) {
        if (confirm('Вы уверены, что хотите удалить этот проект?')) {
            $.ajax({
                url: `/api/projects/${projectId}`,
                method: 'DELETE',
                success: function() {
                    alert('Проект успешно удален');
                    loadProjects();
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

        $.getJSON(`/api/employeeShifts/project/${projectId}/date/${date}`, function(shifts) {
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
            url: '/api/employeeShifts',
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

    function openProjectModal(projectId) {
        $.ajax({
            url: `/api/projects/${projectId}`,
            type: 'GET',
            success: function(data) {
                $('#modalTitle').text('Редактировать проект');
                $('#projectId').val(data.id);
                $('#projectName').val(data.name);
                $('#projectAddress').val(data.address);
                $('#projectStartDate').val(formatDateForInput(new Date(data.startDate)));
                $('#projectDeadline').val(formatDateForInput(new Date(data.deadlineDate)));
                $('#projectCounteragent').val(data.counteragent);
                $('#projectResponsibleEmployee').val(data.responsibleEmployeeId);
                $('#projectManagerShare').val(data.managerShare);
                $('#projectStatus').val(data.projectStatus);
                $('#projectModal').fadeIn();
            },
            error: function(err) {
                console.error('Ошибка при получении данных проекта:', err);
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
    
    function submitProjectForm() {
        const projectData = {
            name: $('#projectName').val(),
            address: $('#projectAddress').val(),
            startDate: $('#projectStartDate').val(),
            deadlineDate: $('#projectDeadline').val(),
            counteragent: $('#projectCounteragent').val() || null,
            responsibleEmployeeId: $('#projectResponsibleEmployee').val(),
            managerShare: $('#projectManagerShare').val(),
            projectStatus: $('#projectStatus').val(),
        };

        const projectId = $('#projectId').val();
        const url = projectId ? `/api/projects/${projectId}` : '/api/projects';
        const method = projectId ? 'PUT' : 'POST';

        $.ajax({
            url: url,
            method: method,
            contentType: 'application/json',
            data: JSON.stringify(projectData),
            success: function() {
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

    $('#addProjectBtn').on('click', function() {
        $('#modalTitle').text('Добавить новый проект');
        $('#projectForm')[0].reset();
        $('#projectId').val('');
        $('#projectStartDate').val(new Date().toISOString().split('T')[0]);
        $('#projectDeadline').val(new Date().toISOString().split('T')[0]);
        $('#projectModal').fadeIn();
    });
    
    loadProjects();
});
