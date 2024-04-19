import axios from "axios";

const portApi = "4466"

const ip = "192.168.21.66"

const apiUrlLocal = `http://${ip}:${portApi}/api`

const api = axios.create({
    baseURL: apiUrlLocal
})

export default api;