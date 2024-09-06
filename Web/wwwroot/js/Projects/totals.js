let TotalsManagement = (function () {
    const module = {};

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
        const totalsContainer = $('#totalCostsContainer');
        totalsContainer.empty();

        const costTable = $('<table class="table table-striped cost-table"></table>');
        costTable.append('<tr><th>№</th><th>Проект</th><th>Стоимость работ</th><th>Зарплатный фонд</th><th>Компенсация</th><th>Итоговая стоимость</th></tr>');
        totalsContainer.append(costTable);

        projects.forEach((project, index) => {
            loadProjectCost(project, index + 1);
        });
    }

    function loadProjectCost(project, projectIndex) {
        $.ajax({
            url: `/api/projects/logic/cost/${project.id}`,
            method: 'GET',
            success: function (costData) {
                displayProjectCost(project, projectIndex, costData);
            },
            error: function () {
                alert(`Ошибка при загрузке данных о стоимости проекта ${project.name}`);
            }
        });
    }

    function displayProjectCost(project, projectIndex, costData) {
        const costTable = $('.cost-table');
        const costRow = $('<tr></tr>');
        costRow.append(`<td>${projectIndex}</td>`);
        costRow.append(`<td>${project.name}</td>`);
        costRow.append(`<td>${formatCurrency(costData.totalProductCost)}</td>`);
        costRow.append(`<td>${formatCurrency(costData.totalMarkup)}</td>`);
        costRow.append(`<td>${formatCurrency(costData.totalCompensatedExpenses)}</td>`);
        costRow.append(`<td>${formatCurrency(costData.totalCost)}</td>`);
        costTable.append(costRow);
    }

    function formatCurrency(amount) {
        return new Intl.NumberFormat('ru-RU', { style: 'currency', currency: 'RUB' }).format(amount);
    }

    module.init = function () {
        $(document).ready(function () {
            module.loadProjectsForTotals();
        });
    };

    return module;
})();