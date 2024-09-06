let CounterAgentManagement = (function () {
    const module = {};

    function setupInputMasks() {
        $('#counteragentINN').mask('9999999999', {
            clearIfNotMatch: true,
            onComplete: function(value) {
                $(this).val(value.replace(/\s/g, ''));
            }
        });

        $('#counteragentPhone').mask('+7(999)999-99-99', {
            clearIfNotMatch: true,
            onComplete: function(value) {
                $(this).val(value);
            }
        });

        $('#counteragentOGRN').mask('999999999999999', {
            onComplete: function(value) {
                $(this).val(value);
            }
        });

        $('#counteragentAccountNumber').mask('99999999999999999999', {
            clearIfNotMatch: true,
            onComplete: function(value) {
                $(this).val(value);
            }
        });

        $('#counteragentBIK').mask('999999999', {
            clearIfNotMatch: true,
            onComplete: function(value) {
                $(this).val(value);
            }
        });
    }

    module.loadCounteragents = function(fullData = false, tableId = 'counteragentsTable') {
        $.getJSON('/api/counteragents/base', function(counteragents) {
            const counteragentsTableBody = $(`#${tableId} tbody`);
            counteragentsTableBody.empty();

            counteragents.forEach((counteragent, index) => {
                const counteragentRow = `
                <tr class="counteragent-row" data-counteragent-id="${counteragent.id}">
                  <td class="shortcol">${index + 1}</td>
                  <td>${counteragent.name}</td>
                  ${fullData ? `
                    <td class="midcol">${counteragent.inn}</td>
                    <td class="midcol">${counteragent.ogrn}</td>
                    <td class="midcol">${counteragent.accountNumber}</td>
                    <td class="midcol">${counteragent.bik}</td>
                  ` : `
                    <td className="midcol">${counteragent.contact}</td>
                    <td className="midcol">${counteragent.phone}</td>
                    `}
                  <td class="btncol">
                    <button class="btn delete-btn" data-counteragent-id="${counteragent.id}">⛌</button>
                  </td>
                </tr>
              `;
                counteragentsTableBody.append(counteragentRow);
            });

            setupEventListeners();
        });
    };

    function deleteCounteragent(counteragentId) {
        if (confirm('Вы уверены, что хотите удалить этого контрагента?')) {
            $.ajax({
                url: `/api/counteragents/base/${counteragentId}`,
                method: 'DELETE',
                success: function() {
                    alert('Контрагент успешно удалён');
                    module.loadCounteragents(false, 'counteragentsTable');
                    module.loadCounteragents(true, 'dataTable');
                },
                error: function (xhr) {
                    const errorMessage = xhr.responseText ? xhr.responseText : 'Ошибка при удалении контрагента';
                    alert(errorMessage);
                }
            });
        }
    }

    module.openCounteragentModal = function(counteragentId) {
        if (counteragentId) {
            $.ajax({
                url: `/api/counteragents/base/${counteragentId}`,
                type: 'GET',
                success: function(data) {
                    populateCounteragentModal(data, counteragentId);
                },
                error: function(err) {
                    console.error(err);
                }
            });
        } else {
            populateCounteragentModal();
        }
    };

    function populateCounteragentModal(data = {}, counteragentId = '') {
        $('#modalTitle').text(counteragentId ? 'Редактировать контрагента' : 'Добавить нового контрагента');
        $('#counteragentId').val(counteragentId);
        $('#counteragentName').val(data.name || '');
        $('#counteragentContact').val(data.contact || '');
        $('#counteragentPhone').val(data.phone || '');
        $('#counteragentINN').val(data.inn || '');
        $('#counteragentOGRN').val(data.ogrn || '');
        $('#counteragentAccountNumber').val(data.accountNumber || '');
        $('#counteragentBIK').val(data.bik || '');
        $('#counteragentModal').fadeIn();
    }

    function submitCounteragentForm() {
        const counteragentData = {
            Name: $('#counteragentName').val(),
            Contact: $('#counteragentContact').val(),
            Phone: $('#counteragentPhone').val(),
            INN: $('#counteragentINN').val() || null,
            OGRN: $('#counteragentOGRN').val() || null,
            AccountNumber: $('#counteragentAccountNumber').val() || null,
            BIK: $('#counteragentBIK').val() || null
        };
        const counteragentId = $('#counteragentId').val();
        const url = counteragentId ? `/api/counteragents/base/${counteragentId}` : '/api/counteragents/base';
        const method = counteragentId ? 'PUT' : 'POST';

        if (counteragentId) {
            counteragentData.Id = counteragentId;
        }

        $.ajax({
            url: url,
            method: method,
            contentType: 'application/json',
            data: JSON.stringify(counteragentData),
            success: function () {
                $('#counteragentModal').hide();
                module.loadCounteragents(false, 'counteragentsTable');
                module.loadCounteragents(true, 'dataTable');
                alert(counteragentId ? 'Контрагент успешно обновлен' : 'Новый контрагент успешно добавлен');
            },
            error: function (xhr) {
                const errorMessage = xhr.response ? xhr.response : 'Ошибка при сохранении данных контрагента';
                alert(errorMessage);
            }
        });
    }

    function setupEventListeners() {
        $('.delete-btn').off('click').on('click', function(event) {
            event.stopPropagation();
            const counteragentId = $(this).data('counteragent-id');
            deleteCounteragent(counteragentId);
        });

        $('.counteragent-row').off('click').on('click', function() {
            const counteragentId = $(this).data('counteragent-id');
            module.openCounteragentModal(counteragentId);
        });

        $('#counteragentForm').off('submit').on('submit', function(event) {
            event.preventDefault();
            submitCounteragentForm();
        });

        $('#addCounteragentBtn, #addCounteragentBtn2').off('click').on('click', function() {
            module.openCounteragentModal();
        });
    }

    module.init = function () {
        $(document).ready(function () {
            if (!AuthManagement.checkAuth()) {
                return;
            }
            setupInputMasks();
            setupEventListeners();
            module.loadCounteragents(false, 'counteragentsTable');
            module.loadCounteragents(true, 'dataTable');
        });
    };

    return module;
})();

$(document).ready(function() {
    AuthManagement.init();
    CounterAgentManagement.init();
});