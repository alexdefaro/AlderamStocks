import axios from "axios";

let Api = axios.create({ baseURL: "https://localhost:44330/api/" })

if (process.env.NODE_ENV == 'production') {
    Api = axios.create({ baseURL: "https://alderamstocksapi.azurewebsites.net/api/" })
}

export const ApiService = {
    get,
    post,
    put,
    remove
};
export default ApiService; 

async function get(enpoint) {
    let headers = {
        headers: {
            Authorization: 'Bearer ' + sessionStorage.getItem('AUTH_TOKEN')
        }
    }

    return await Api.get(enpoint, headers);
}

async function post(enpoint, data) {
    let headers = {
        headers: {
            Authorization: 'Bearer ' + sessionStorage.getItem('AUTH_TOKEN')
        }
    }

    return await Api.post(enpoint, data, headers);
} 

async function put(enpoint, data) {
    let headers = {
        headers: {
            Authorization: 'Bearer ' + sessionStorage.getItem('AUTH_TOKEN')
        }
    }

    return await Api.put(enpoint, data, headers);
} 

async function remove(enpoint) {
    let headers = {
        headers: {
            Authorization: 'Bearer ' + sessionStorage.getItem('AUTH_TOKEN')
        }
    }

    return await Api.delete(enpoint, headers);
}
