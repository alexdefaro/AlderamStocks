export const isAuthenticated = () => {
    const AUTH_TOKEN = localStorage.getItem('AUTH_TOKEN') ?? '';

    return (AUTH_TOKEN != '');
}

//npm install jwt_decode