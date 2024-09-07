let TotalsManagement = (function () {
    const module = {};
    let projectsData = [];

    module.loadProjectsForTotals = function () {
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

                        projectsData = projects.map(function (project) {
                            return {
                                ...project,
                                counteragent: counteragentMap[project.counteragent] || project.counteragent
                            };
                        });

                        loadProjectCosts();
                    }
                });
            },
            error: function () {
                alert('Ошибка при загрузке проектов');
            }
        });
    };

    function loadProjectCosts() {
        let loadedCount = 0;
        projectsData.forEach((project, index) => {
            $.ajax({
                url: `/api/projects/logic/cost/${project.id}`,
                method: 'GET',
                success: function (costData) {
                    projectsData[index].costData = costData;
                    loadedCount++;
                    if (loadedCount === projectsData.length) {
                        displayProjects();
                    }
                },
                error: function () {
                    alert(`Ошибка при загрузке данных о стоимости проекта ${project.name}`);
                }
            });
        });
    }

    function displayProjects() {
        const totalsContainer = $('#totalCostsContainer');
        totalsContainer.empty();

        const costTable = $('<table class="table table-striped cost-table"></table>');
        costTable.append('<thead><tr><th>№</th><th>Проект</th><th>Стоимость работ</th><th>Зарплатный фонд</th><th>Компенсация</th><th>Итоговая стоимость</th></tr></thead>');
        const tableBody = $('<tbody></tbody>');
        costTable.append(tableBody);
        totalsContainer.append(costTable);

        projectsData.forEach((project, index) => {
            displayProjectCost(tableBody, project, index + 1);
        });
    }

    function displayProjectCost(tableBody, project, projectIndex) {
        const costRow = $('<tr></tr>');
        costRow.append(`<td>${projectIndex}</td>`);
        costRow.append(`<td>${project.name}</td>`);
        costRow.append(`<td>${formatCurrency(project.costData.totalProductCost)}</td>`);
        costRow.append(`<td>${formatCurrency(project.costData.totalMarkup)}</td>`);
        costRow.append(`<td>${formatCurrency(project.costData.totalCompensatedExpenses)}</td>`);
        costRow.append(`<td>${formatCurrency(project.costData.totalCost)}</td>`);
        tableBody.append(costRow);
    }

    function formatCurrency(amount) {
        return new Intl.NumberFormat('ru-RU', { style: 'currency', currency: 'RUB' }).format(amount);
    }

    module.sortProjects = function(field) {
        projectsData.sort((a, b) => {
            if (a[field] < b[field]) return -1;
            if (a[field] > b[field]) return 1;
            return 0;
        });
        displayProjects();
    };

    module.init = function () {
        $(document).ready(function () {
            module.loadProjectsForTotals();
        });
    };

    return module;
})();