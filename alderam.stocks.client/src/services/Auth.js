export const isAuthenticated = () => {
    const AUTH_TOKEN = sessionStorage.getItem('AUTH_TOKEN') ?? '';
    return (AUTH_TOKEN != '');
} 