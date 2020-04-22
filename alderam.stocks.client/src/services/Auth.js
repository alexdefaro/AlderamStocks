export const isAuthenticated = () => {

    var authenticated = localStorage.getItem('authenticated');
    return authenticated == 'true';
}