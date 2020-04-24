import axios from "axios";

let  Api = axios.create({ baseURL: "https://localhost:44330/api/" })

if (process.env.NODE_ENV == 'production') {
    Api = axios.create({ baseURL: "https://alderamstocksapi.azurewebsites.net/api/" })
}

export default Api;