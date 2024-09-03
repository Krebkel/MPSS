let ExpenseManagement = (function () {
    const module = {};

    module.loadProjects = function () {
        $.ajax({
            url: '/api/projects/base',
            method: 'GET',
            success: function (projects) {
                displayProjects(projects);
            },
            error: function () {
                alert('Ошибка при загрузке проектов');
            }
        });
    };

    function displayProjects(projects) {
        const expensesContainer = $('#expensesContainer');
        expensesContainer.empty();

        projects.forEach((project, index) => {
            const projectRow = $(`
                <table class="table table-striped expense-table" data-project-id="${project.id}">
                    <thead>
                        <tr class="project-header">
                            <th class="shortcol">${index + 1}</th>
                            <th colspan="5">${project.name}</th>
                            <th class="shortcol">${formatDateForOutput(new Date(project.deadlineDate))}</th>
                            <th class="btncol">
                                <button class="btn btn-add addExpenseBtn" data-project-id="${project.id}">+</button>
                            </th>
                        </tr>
                    </thead>
                    <tbody style="display: none;">
                        <tr>
                            <th class="expense-header shortcol">№</th>
                            <th class="expense-header">Наименование</th>
                            <th class="expense-header midcol">Сумма</th>
                            <th class="expense-header shortcol">Тип</th>
                            <th class="expense-header midcol">Оплатил</th>
                            <th class="expense-header">Комментарий</th>
                            <th class="expense-header shortcol">Компенсация</th>
                            <th class="btncol"></th>
                        </tr>
                    </tbody>
                </table>
            `);

            expensesContainer.append(projectRow);

            projectRow.find('.project-header').on('click', function() {
                const tbody = $(this).closest('table').find('tbody');
                if (tbody.is(':visible')) {
                    tbody.hide();
                } else {
                    loadExpenses(project.id, tbody);
                    tbody.show();
                }
            });

            projectRow.find('.addExpenseBtn').on('click', function(e) {
                e.stopPropagation();
                module.openExpenseModal(project.id);
            });
        });
    }

    function loadExpenses(projectId, tbody) {
        $.ajax({
            url: `/api/expenses/base/byProject/${projectId}`,
            method: 'GET',
            success: function (expenses) {
                displayExpenses(expenses, tbody);
            },
            error: function () {
                alert('Ошибка при загрузке расходов');
            }
        });
    }

    async function displayExpenses(expenses, tbody) {
        tbody.find('tr:gt(0)').remove();
        for (const [index, expense] of expenses.entries()) {
            let employeeName = '';
            if (expense.employee) {
                try {
                    const employee = await $.getJSON(`/api/employees/base/${expense.employee}`);
                    employeeName = employee ? employee.name : '';
                } catch (error) {
                    console.error('Ошибка при получении сотрудника:', error);
                }
            }

            const expenseRow = $(`
            <tr data-expense-id="${expense.id}">
                <td class="expense-data shortcol">${index + 1}</td>
                <td class="expense-data">${expense.name}</td>
                <td class="expense-data midcol">${expense.amount}</td>
                <td class="expense-data shortcol">${translateExpenseType(expense.type)}</td>
                <td class="expense-data midcol">${employeeName}</td>
                <td class="expense-data">${expense.description || ''}</td>
                <td class="expense-data">${expense.isPaidByCompany ? 'Да' : 'Нет'}</td>
                <td class="btncol expensebtns">
                    <button class="btn delete-btn" data-expense-id="${expense.id}">⛌</button>
                </td>
            </tr>
        `);

            tbody.append(expenseRow);

            expenseRow.on('click', function() {
                module.openExpenseModal(expense.project, expense.id);
            });

            expenseRow.find('.delete-btn').on('click', function(e) {
                e.stopPropagation();
                module.deleteExpense(expense.id);
            });
        }
    }

    module.openExpenseModal = function (projectId, expenseId = null) {
        const modal = $('#expenseModal');
        const form = $('#expenseForm')[0];
        form.reset();

        $.ajax({
            url: '/api/employees/base',
            method: 'GET',
            success: function (employees) {
                const employeeOptions = `<option value="">Не указан</option>` +
                    employees.map(employee =>
                        `<option value="${employee.id}">${employee.name}</option>`
                    ).join('');

                $('#expenseEmployee').html(employeeOptions);
                $('#expenseProjectId').val(projectId);

                if (expenseId) {
                    $('#expenseHeader').text('Редактировать расход');
                    $.ajax({
                        url: `/api/expenses/base/${expenseId}`,
                        method: 'GET',
                        success: function (expense) {
                            $('#expenseId').val(expense.id);
                            $('#expenseName').val(expense.name);
                            $('#expenseAmount').val(expense.amount);
                            $('#expenseIsPaidByCompany').prop('checked', expense.isPaidByCompany);
                            $('#expenseType').val(expense.type);
                            $('#expenseEmployee').val(expense.employee || null);
                            $('#expenseDescription').val(expense.description);
                        },
                        error: function () {
                            alert('Ошибка при загрузке данных расхода');
                        }
                    });
                } else {
                    $('#expenseHeader').text('Добавить новый расход');
                    $('#expenseId').val('');
                }

                modal.fadeIn();
            },
            error: function () {
                alert('Ошибка загрузки сотрудников');
            }
        });
    };

    module.deleteExpense = function (expenseId) {
        if (confirm('Вы уверены, что хотите удалить этот расход?')) {
            $.ajax({
                url: `/api/expenses/base/${expenseId}`,
                method: 'DELETE',
                success: function () {
                    module.loadProjects();
                },
                error: function () {
                    alert('Ошибка при удалении расхода');
                }
            });
        }
    };

    module.submitExpenseForm = function () {
        const employeeValue = $('#expenseEmployee').val();
        const expenseData = {
            id: $('#expenseId').val() || null,
            project: $('#expenseProjectId').val(),
            name: $('#expenseName').val(),
            amount: parseFloat($('#expenseAmount').val()),
            isPaidByCompany: $('#expenseIsPaidByCompany').is(':checked'),
            type: $('#expenseType').val(),
            employee: employeeValue ? employeeValue : null,
            description: $('#expenseDescription').val()
        };

        const url = expenseData.id ? `/api/expenses/base/${expenseData.id}` : '/api/expenses/base';
        const method = expenseData.id ? 'PUT' : 'POST';

        $.ajax({
            url: url,
            method: method,
            contentType: 'application/json',
            data: JSON.stringify(expenseData),
            success: function () {
                $('#expenseModal').hide();
                module.loadProjects();
            },
            error: function () {
                alert('Ошибка при сохранении расхода');
            }
        });
    };

    module.init = function () {
        $(document).ready(function () {
            module.loadProjects();

            $('#expenseForm').on('submit', function (event) {
                event.preventDefault();
                module.submitExpenseForm();
            });

            $('.modal .close').on('click', function() {
                $(this).closest('.modal').hide();
            });

            $(window).on('click', function(event) {
                if ($(event.target).hasClass('modal')) {
                    $('.modal').hide();
                }
            });
        });
    };

    return module;
})();

$(document).ready(function() {
    ExpenseManagement.init();
});