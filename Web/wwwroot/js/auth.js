let AuthManagement = (function () {
    const module = {};

    function decodeJwt(token) {
        const base64Url = token.split('.')[1];
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        const jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));
        return JSON.parse(jsonPayload);
    }

    module.setupEventListeners = function () {
        $('.close, #authModal').on('click', function(event) {
            if (event.target === this) {
                $('#authModal').fadeOut();
            }
        });

        $('#showRegisterForm, #showLoginForm').on('click', function(e) {
            e.preventDefault();
            $('#loginForm, #registerForm').toggle();
        });

        $('#loginBtn').on('click', module.handleLogin);
        $('#registerBtn').on('click', module.handleRegistration);
    };

    module.checkAuth = function() {
        const token = localStorage.getItem('jwt_token');
        
        if (!token) {
            //window.location.href = 'index.html';
            console.log("Не авторизован (или это не так работает?)");
            return false;
        }

        const userRole = module.getUserRole();
        if (userRole !== 'Service' && userRole !== 'Administrator') {
            console.log("Не админ");
            return false;
        }
        return true;
    }

    module.handleLogin = function (e) {
        e.preventDefault();
        const phone = $('#loginPhone').val();
        const password = $('#loginPassword').val();

        $.ajax({
            url: '/api/token',
            method: 'POST',
            data: JSON.stringify({ phone, password }),
            contentType: 'application/json',
            success: function(response) {
                module.handleSuccessfulAuth(response);
            },
            error: function(xhr) {
                module.handleAuthError(xhr, 'авторизации');
            }
        });
    };

    module.handleRegistration = function (e) {
        e.preventDefault();
        const name = $('#regName').val();
        const phoneNumber = $('#regPhone').val();
        const password = $('#regPassword').val();

        $.ajax({
            url: '/api/users/registration',
            method: 'POST',
            data: JSON.stringify({ name, phoneNumber, password }),
            contentType: 'application/json',
            success: function() {
                alert('Регистрация успешна. Пожалуйста, войдите в систему.');
                $('#loginForm').show();
                $('#registerForm').hide();
            },
            error: function(xhr) {
                module.handleAuthError(xhr, 'регистрации');
            }
        });
    };

    module.handleSuccessfulAuth = function (response) {
        localStorage.setItem('jwt_token', response);
        const decodedToken = decodeJwt(response);
        localStorage.setItem('user_name', decodedToken.DisplayName);
        localStorage.setItem('user_phone', decodedToken.Phone);
        localStorage.setItem('user_role', decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']);
        $('#authModal').fadeOut();
        location.reload();
    };

    module.handleAuthError = function (xhr, action) {
        alert(xhr.status === 400 ? xhr.responseText : `Ошибка при ${action}. Пожалуйста, попробуйте снова.`);
    };

    module.getUserRole = function () {
        const token = localStorage.getItem('jwt_token');
        if (!token) return null;
        const decodedToken = decodeJwt(token);
        return decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
    };

    module.hasRole = function (requiredRole) {
        const userRole = module.getUserRole().toLowerCase();
        return userRole === requiredRole.toLowerCase();
    };


    module.init = function () {
        $(document).ready(function () {
            module.setupEventListeners();
            module.checkAuth();
        });
    };

    return module;
})();

$(document).ready(function() {
    AuthManagement.init();
});