import axios from "axios";

const AUTH_TOKEN = sessionStorage.getItem('AUTH_TOKEN') ?? 'null'; 

axios.defaults.headers.common['Authorization'] = 'Bearer '+ AUTH_TOKEN;

let Api = axios.create({ baseURL: "https://localhost:44330/api/" })

if (process.env.NODE_ENV == 'production') {
    Api = axios.create({ baseURL: "https://alderamstocksapi.azurewebsites.net/api/" })
}

export default Api; 