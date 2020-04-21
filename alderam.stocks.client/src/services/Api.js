import axios from "axios";
 
//const Api = axios.create({ baseURL: "https://localhost:44330/api/" })
const Api = axios.create({ baseURL: "https://azurestocksapi.azurewebsites.net/api" })

export default Api;