export const authenticationService = { isAuthenticated, logout };

function isAuthenticated() {
  const AUTH_TOKEN = sessionStorage.getItem('AUTH_TOKEN') ?? '';
  return AUTH_TOKEN !== '';
}

function logout() {
  sessionStorage.clear();
}
