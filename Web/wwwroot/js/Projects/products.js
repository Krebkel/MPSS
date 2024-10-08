let ProductManagement = (function () {
    const module = {};

    function loadProducts() {
        $.ajax({
            url: '/api/products/base',
            method: 'GET',
            dataType: 'json'
        }).done(function(products) {
            const productsTableBody = $('#productsTable tbody');
            productsTableBody.empty();

            products.forEach((product, index) => {
                const productRow = `
            <tr class="product-row" data-product-id="${product.id}">
                <td class="shortcol">${index + 1}</td>
                <td>${product.name}</td>
                <td class="shortcol">${product.cost}</td>
                <td class="shortcol">${translateProductType(product.type)}</td>
                <td class="btncol">
                    <button name="deleteProductBtn" class="btn delete-btn" data-product-id="${product.id}">⛌</button>
                </td>
            </tr>
        `;
                productsTableBody.append(productRow);
            });

            $('#productsTable').on('click', '.delete-btn', function(event) {
                event.stopPropagation();
                const productId = $(this).data('product-id');
                deleteProduct(productId);
            });

            $('.product-row').on('click', function() {
                const productId = $(this).data('product-id');
                module.openProductModal(productId);
            });
        });
    }

    function translateProductType(type) {
        const types = {
            'Main': 'Основное',
            'Extra': 'Доп. услуга',
            'Other': 'Прочее'
        };
        return types[type] || 'Неизвестный тип';
    }

    function deleteProduct(productId) {
        $.ajax({
            url: `/api/products/base/${productId}`,
            method: 'DELETE',
            success: function() {
                loadProducts();
            },
            error: function(xhr) {
                const errorMessage = xhr.responseText ? xhr.responseText : 'Ошибка при удалении работы';
                alert(errorMessage);
            }
        });
    }

    module.openProductModal = function(productId) {
        const modal = $('#productModal');
        const form = $('#productForm')[0];
        form.reset();
        $('#componentsTable tbody').empty();

        if (productId) {
            $.ajax({
                url: `/api/products/base/${productId}`,
                type: 'GET',
                success: function(data) {
                    $('#productModalTitle').text('Редактировать работу');
                    $('#productId').val(data.id);
                    $('#productName').val(data.name);
                    $('#productCost').val(data.cost);
                    $('#productType').val(data.type);

                    // Загрузка компонентов
                    $.ajax({
                        url: `/api/productComponents/base/byProduct/${productId}`,
                        type: 'GET',
                        success: function(components) {
                            components.forEach(component => addComponentRow(component));
                        },
                        error: function(xhr) {
                            alert('Ошибка при загрузке компонентов работы');
                        }
                    });

                    modal.fadeIn();
                },
                error: function(xhr) {
                    alert('Ошибка при загрузке данных работы');
                }
            });
        } else {
            $('#productModalTitle').text('Добавить новую работу');
            $('#productId').val('');
            modal.fadeIn();
        }
    };

    function addComponentRow(component = {}) {
        const componentRow = `
            <tr class="component-row" ${component.id ? `data-component-id="${component.id}"` : ''}>
                <td><input type="text" name="componentName[]" value="${component.name || ''}" required></td>
                <td><input type="number" name="componentQuantity[]" value="${component.quantity || ''}"></td>
                <td><input type="number" name="componentWeight[]" value="${component.weight || ''}" step="0.01"></td>
                <td><button type="button" class="btn delete-component-btn">⛌</button></td>
            </tr>
        `;
        $('#componentsTable tbody').append(componentRow);
    }

    function saveProductComponents(productId) {
        const components = $('.component-row').map(function() {
            const $this = $(this);
            return {
                id: $this.data('component-id'),
                product: productId,
                name: $this.find('input[name="componentName[]"]').val(),
                quantity: parseInt($this.find('input[name="componentQuantity[]"]').val()) || null,
                weight: parseFloat($this.find('input[name="componentWeight[]"]').val()) || null
            };
        }).get().filter(c => c.name);

        const deletePromises = $('[data-deleted-component-id]')
            .map((_, el) => $.ajax({
                url: `/api/productComponents/base/${$(el).data('deleted-component-id')}`,
                method: 'DELETE'
            })).get();

        Promise.all(deletePromises)
            .then(() => Promise.all(components.map(c =>
                $.ajax({
                    url: c.id ? `/api/productComponents/base/${c.id}` : '/api/productComponents/base',
                    method: c.id ? 'PUT' : 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(c)
                })
            )))
            .then(() => {
                $('#productModal').hide();
                loadProducts();
            })
            .catch(error => {
                console.error('Error:', error);
                alert('Ошибка при сохранении или удалении компонентов работы');
            });
    }

    function setupEventListeners() {
        $('#productsTable').off('click', '.delete-btn').on('click', '.delete-btn', function(event) {
            event.stopPropagation();
            const productId = $(this).data('product-id');
            deleteProduct(productId);
        });

        $('.product-row').off('click').on('click', function() {
            const productId = $(this).data('product-id');
            module.openProductModal(productId);
        });

        $('#addComponentBtn').off('click').on('click', function() {
            addComponentRow();
        });

        $(document).off('click', '.delete-component-btn').on('click', '.delete-component-btn', function() {
            const row = $(this).closest('tr');
            const componentId = row.data('component-id');

            if (componentId) {
                if (confirm('Вы уверены, что хотите удалить этот компонент?')) {
                    $.ajax({
                        url: `/api/productComponents/base/${componentId}`,
                        method: 'DELETE',
                        success: function() {
                            row.remove();
                        },
                        error: function(xhr) {
                            alert('Ошибка при удалении компонента');
                        }
                    });
                }
            } else {
                row.remove();
            }
        });

        $('#productForm').off('submit').on('submit', function(event) {
            event.preventDefault();
            const formData = new FormData(this);
            const productId = formData.get('productId');
            const productData = {
                name: formData.get('productName'),
                cost: parseFloat(formData.get('productCost')),
                type: formData.get('productType')
            };

            const url = productId ? `/api/products/base/${productId}` : '/api/products/base';
            const method = productId ? 'PUT' : 'POST';

            if (productId) {
                productData.id = parseInt(productId);
            }

            $.ajax({
                url: url,
                method: method,
                contentType: 'application/json',
                data: JSON.stringify(productData),
                success: function(response) {
                    const newProductId = productId || response.id;
                    saveProductComponents(newProductId);
                },
                error: function(xhr) {
                    alert('Ошибка при сохранении работы');
                }
            });
        });

        $('#addProductBtn').off('click').on('click', function() {
            module.openProductModal();
        });
    }

    module.init = function () {
        AuthManagement.setupAjaxInterceptor();
        setupEventListeners();
        loadProducts();
    };

    return module;
})();