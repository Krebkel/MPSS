$(document).ready(function() {

    $('#loginPhone').mask('+7(999)999-99-99', {
        clearIfNotMatch: true,
        onComplete: function(value) {
            $(this).val(value);
        }
    });

    $('#regPhone').mask('+7(999)999-99-99', {
        clearIfNotMatch: true,
        onComplete: function(value) {
            $(this).val(value);
        }
    });
    
    function showAuthModal() {
        $('#authModal').fadeIn();
        $('#loginForm').show();
        $('#registerForm').hide();
    }

    if (!localStorage.getItem('jwt_token')) {
        showAuthModal();
    }

    $('.close, #authModal').on('click', function (event) {
        if (event.target === this) {
            $('#authModal').fadeOut();
        }
    });

    $('#showRegisterForm').on('click', function (e) {
        e.preventDefault();
        $('#loginForm').hide();
        $('#registerForm').show();
    });

    $('#showLoginForm').on('click', function (e) {
        e.preventDefault();
        $('#registerForm').hide();
        $('#loginForm').show();
    });

    $('#loginBtn').on('submit', function(e) {
        e.preventDefault();
        const phone = $('#loginPhone').val();
        const password = $('#loginPassword').val();

        $.ajax({
            url: '/api/token',
            method: 'POST',
            data: JSON.stringify({ phone, password }),
            contentType: 'application/json',
            success: function(response) {
                localStorage.setItem('jwt_token', response);

                const decodedToken = JSON.parse(atob(response.split('.')[1]));

                localStorage.setItem('user_name', decodedToken.Name);
                localStorage.setItem('user_phone', decodedToken.Phone);
                localStorage.setItem('user_role', decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']);

                $('#authModal').fadeOut();
                location.reload(); 
            },
            error: function(xhr, status, error) {
                if (xhr.status === 400) {
                    alert(xhr.responseText); 
                } else {
                    alert('Ошибка при авторизации. Пожалуйста, попробуйте снова.');
                }
            }
        });
    });

    $('#registerBtn').on('submit', function (e) {
        e.preventDefault();
        const name = $('#regName').val();
        const phone = $('#regPhone').val();
        const password = $('#regPassword').val();

        //AJAX
    });
});