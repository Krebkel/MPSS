$(document).ready(function() {
    function loadProducts() {
        $.getJSON('/api/products/base', function(products) {
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
                        <button class="btn delete-btn" data-product-id="${product.id}">⛌</button>
                    </td>
                </tr>
            `;
                productsTableBody.append(productRow);
            });

            $('.delete-btn').on('click', function(event) {
                event.stopPropagation();
                const productId = $(this).data('product-id');
                deleteProduct(productId);
            });

            $('.product-row').on('click', function() {
                const productId = $(this).data('product-id');
                openProductModal(productId);
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
        if (confirm('Вы уверены, что хотите удалить это изделие?')) {
            $.ajax({
                url: `/api/products/base/${productId}`,
                method: 'DELETE',
                success: function() {
                    alert('Изделие успешно удалено');
                    loadProducts();
                },
                error: function(xhr) {
                    const errorMessage = xhr.responseText ? xhr.responseText : 'Ошибка при удалении изделия';
                    alert(errorMessage);
                }
            });
        }
    }

    function openProductModal(productId) {
        const modal = $('#productModal');
        const form = $('#productForm')[0];
        form.reset();
        $('#componentsTable tbody').empty();

        if (productId) {
            $.ajax({
                url: `/api/products/base/${productId}`,
                type: 'GET',
                success: function(data) {
                    $('#productModalTitle').text('Редактировать изделие');
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
                            alert('Ошибка при загрузке компонентов изделия');
                        }
                    });

                    modal.fadeIn();
                },
                error: function(xhr) {
                    alert('Ошибка при загрузке данных изделия');
                }
            });
        } else {
            $('#productModalTitle').text('Добавить новое изделие');
            $('#productId').val('');
            modal.fadeIn();
        }
    }

    function addComponentRow(component = {}) {
        const componentRow = `
        <tr class="component-row">
            <td><input type="text" name="componentName[]" value="${component.name || ''}" required></td>
            <td><input type="number" name="componentQuantity[]" value="${component.quantity || ''}"></td>
            <td><input type="number" name="componentWeight[]" value="${component.weight || ''}" step="0.01"></td>
            <td><button type="button" class="btn delete-btn">⛌</button></td>
        </tr>
    `;
        $('#componentsTable tbody').append(componentRow);
    }

    $('#addComponentBtn').on('click', function() {
        addComponentRow();
    });

    $(document).on('click', '.delete-btn', function() {
        $(this).closest('tr').remove();
    });

    $('#productForm').on('submit', function(event) {
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
                alert('Ошибка при сохранении изделия');
            }
        });
        
        loadProducts();
    });

    function saveProductComponents(productId) {
        const components = [];

        $('.component-row').each(function() {
            const component = {
                product: productId,
                name: $(this).find('input[name="componentName[]"]').val(),
                quantity: parseInt($(this).find('input[name="componentQuantity[]"]').val()) || null,
                weight: parseFloat($(this).find('input[name="componentWeight[]"]').val()) || null
            };

            if (component.name) {
                if ($(this).data('component-id')) {
                    component.id = $(this).data('component-id');
                }
                components.push(component);
            }
        });

        if (components.length === 0) {
            return;
        }

        $.ajax({
            url: '/api/productComponents/base',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(components),
            success: function() {
                $('#productModal').hide();
                loadProducts();
                alert('Изделие и компоненты успешно сохранены');
            },
            error: function(xhr) {
                alert('Ошибка при сохранении компонентов изделия');
            }
        });
    }

    $('#addProductBtn').on('click', function() {
        openProductModal();
    });

    loadProducts();
});