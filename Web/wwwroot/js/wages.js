let ProjectWages = (function () {
    const module = {};

    module.loadProjects = function () {
        $.ajax({
            url: '/api/projects/base',
            method: 'GET',
            success: function (projects) {
                $.ajax({
                    url: '/api/counteragents/base',
                    method: 'GET',
                    success: function (counteragents) {
                        const counteragentMap = {};

                        counteragents.forEach(function (counteragent) {
                            counteragentMap[counteragent.id] = counteragent.name;
                        });

                        const doneProjects = projects
                            .filter(project => project.projectStatus === 'Done')
                            .map(function (project) {
                                return {
                                    ...project,
                                    counteragent: counteragentMap[project.counteragent] || project.counteragent
                                };
                            });

                        displayProjects(doneProjects);
                    }
                });
            },
            error: function () {
                alert('Ошибка при загрузке проектов');
            }
        });
    };

    function displayProjects(projects) {
        const wagesContainer = $('#wagesContainer');
        wagesContainer.empty();

        projects.forEach((project, index) => {
            loadProjectWages(project, index + 1);
        });
    }

    function loadProjectWages(project, projectIndex) {
        $.ajax({
            url: `/api/projects/logic/wages/${project.id}`,
            method: 'GET',
            success: function (wagesData) {
                displayProjectWages(project, projectIndex, wagesData);
            },
            error: function () {
                alert(`Ошибка при загрузке данных о зарплатах для проекта ${project.name}`);
            }
        });
    }

    function displayProjectWages(project, projectIndex, wagesData) {
        const wagesContainer = $('#wagesContainer');
        const table = $('<table class="table table-striped wages-table"></table>');

        const projectHeader = $(`
            <tr class="project-header">
                <th>${projectIndex}</th>
                <th colspan="${wagesData.dailyShifts.length + 3}">${project.name}</th>
                <th>${translateStatus(project.projectStatus)}</th>
                <th>${project.counteragent ? project.counteragent : ''}</th>
            </tr>
        `);
        table.append(projectHeader);

        const columnHeaders = $('<tr></tr>');
        columnHeaders.append('<th>№</th><th>ФИО</th>');
        wagesData.dailyShifts.forEach(shift => {
            columnHeaders.append(`<th>${formatDate(shift.date)}</th>`);
        });
        columnHeaders.append('<th>Ставка</th><th>Премия</th><th>Компенсация</th><th>Сумма</th>');
        table.append(columnHeaders);

        wagesData.employeeWages.forEach((employee, index) => {
            const employeeRow = $('<tr></tr>');
            employeeRow.append(`<td>${index + 1}</td><td>${employee.employeeName}</td>`);

            wagesData.dailyShifts.forEach(shift => {
                const employeeShift = shift.shifts.find(s => s.employeeName === employee.employeeName);
                employeeRow.append(`<td>${employeeShift ? employeeShift.hours : ''}</td>`);
            });

            employeeRow.append(`<td>${formatCurrency(employee.baseWage)}</td>`);
            employeeRow.append(`<td>${formatCurrency(employee.bonus)}</td>`);
            employeeRow.append(`<td>${formatCurrency(employee.compensation)}</td>`);
            employeeRow.append(`<td>${formatCurrency(employee.totalWage)}</td>`);

            table.append(employeeRow);
        });

        wagesContainer.append(table);
        wagesContainer.append('<br>');
    }

    function formatDate(dateString) {
        const date = new Date(dateString);
        return `${date.getDate().toString().padStart(2, '0')}.${(date.getMonth() + 1).toString().padStart(2, '0')}`;
    }

    function formatCurrency(amount) {
        return new Intl.NumberFormat('ru-RU', { style: 'currency', currency: 'RUB' }).format(amount);
    }

    module.init = function () {
        $(document).ready(function () {
            if (!AuthManagement.checkAuth()) {
                return;
            }
            module.loadProjects();
        });
    };

    return module;
})();