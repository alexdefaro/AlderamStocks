import axios from 'axios';

const Api = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL,
});

export const ApiService = { get, post, put, remove };
export default ApiService;

async function get(endpoint, params) {
  return Api.get(endpoint, {
    headers: {
      Authorization: 'Bearer ' + sessionStorage.getItem('AUTH_TOKEN'),
      'Content-Type': 'application/json;charset=UTF-8',
    },
    params,
  });
}

async function post(endpoint, data) {
  return Api.post(endpoint, data, {
    headers: { Authorization: 'Bearer ' + sessionStorage.getItem('AUTH_TOKEN') },
  });
}

async function put(endpoint, data) {
  return Api.put(endpoint, data, {
    headers: { Authorization: 'Bearer ' + sessionStorage.getItem('AUTH_TOKEN') },
  });
}

async function remove(endpoint) {
  return Api.delete(endpoint, {
    headers: { Authorization: 'Bearer ' + sessionStorage.getItem('AUTH_TOKEN') },
  });
}
