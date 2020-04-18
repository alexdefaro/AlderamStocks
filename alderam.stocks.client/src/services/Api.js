import axios from "axios";
 
const Api = axios.create({ baseURL: "https://localhost:44330/api/" })

export default Api;