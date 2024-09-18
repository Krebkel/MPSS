let ProjectFileManagement = (function () {
    const module = {};

    // Функция для форматирования названия файла
    function formatFileName(fileName) {
        return fileName.length > 25 ? fileName.slice(0, 22) + '...' : fileName;
    }

    module.addProjectFileRow = function (projectFile = {}) {
        const fileName = projectFile.fileName || 'Click to upload';
        const formattedFileName = formatFileName(fileName);

        const row = $(`
            <tr class="project-file-row" data-project-file-id="${projectFile.id || ''}">
                <td class="midcol">${projectFile.uploadDate || new Date().toISOString().split('T')[0]}</td>
                <td>
                    ${formattedFileName} 
                    <input type="file" style="display: none;">
                </td>
                <td class="btncol"><button type="button" class="btn delete-btn">⛌</button></td>
            </tr>
        `);

        $('#projectFilesTable tbody').append(row);

        row.find('.delete-btn').on('click', function (event) {
            event.stopPropagation();
            const projectFileId = row.data('project-file-id');
            if (projectFileId) {
                module.deleteProjectFile(projectFileId);
            }
            row.remove();
        });

        row.find('input[type="file"]').on('change', function () {
            const file = this.files[0];
            if (file) {
                module.uploadFile(file, row);
            }
        });

        row.on('click', function (event) {
            const target = $(event.target);
            if (!target.hasClass('delete-btn') && !target.is('input[type="file"]') && !projectFile.id) {
                row.find('input[type="file"]').trigger('click');
            }
        });
    };

    module.uploadFile = function (file, row) {
        const formData = new FormData();
        formData.append('file', file);
        formData.append('projectId', $('#projectId').val());

        $.ajax({
            url: '/api/projectFiles',
            method: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                const fileName = response.name;
                const displayFileName = formatFileName(fileName);

                row.data('project-file-id', response.id);
                row.find('td:first').text(new Date(response.uploadDate).toISOString().split('T')[0]);
                row.find('td:nth-child(2)').attr('title', fileName).text(displayFileName);

                console.log('Файл загружен');
            },
            error: function () {
                alert('Ошибка загрузки файла');
                row.remove();
            }
        });
    };

    module.deleteProjectFile = function (projectFileId) {
        $.ajax({
            url: `/api/projectFiles/${projectFileId}`,
            method: 'DELETE',
            success: function () {
                console.log('Файл успешно удален');
            },
            error: function () {
                alert('Ошибка удаления файла');
            }
        });
    };

    module.loadProjectFiles = function (projectId) {
        $.ajax({
            url: `/api/projectFiles/byProject/${projectId}`,
            method: 'GET',
            success: function (files) {
                $('#projectFilesTable tbody').empty();
                files.forEach(file => module.addProjectFileRow(file));
            },
            error: function () {
                alert('Ошибка загрузки файлов проекта');
            }
        });
    };

    module.init = function () {
        $('#addProjectFileBtn').on('click', function () {
            module.addProjectFileRow();
        });
    };

    return module;
})();