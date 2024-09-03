function decodeJwt(token) {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
}

function getUserRole() {
    const token = localStorage.getItem('jwt_token');
    if (!token) return null;

    const decodedToken = decodeJwt(token);
    return decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
}

function hasRole(requiredRole) {
    const userRole = getUserRole();
    return userRole === requiredRole;
}

function showElementIfHasRole(elementId, requiredRole) {
    const element = document.getElementById(elementId);
    if (element) {
        element.style.display = hasRole(requiredRole) ? 'block' : 'none';
    }
}