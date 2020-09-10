import { ITraining } from '../models/ITraining';
import axios, { AxiosResponse } from 'axios'
import { toast } from 'react-toastify';
import { history } from '../..';


axios.defaults.baseURL = ("https://localhost:44310/api");
axios.interceptors.response.use(undefined, error => {
    const { data, status, config } = error.response;
    if (!error.response)
        toast.error("Network error -  zda");
    if (status === 404)
        history.push('/notfound');
    if (status === 400 && config.method === 'get' && data.errors.hasOwnProperty('id'))
        history.push('/notfound');
    if (status === 500)
        toast.error("server error -  zda");
    console.log(error.response);
})

const responseBody = (response: AxiosResponse) => response.data;
const sleep = (ms: number) => (response: AxiosResponse) =>
    new Promise<AxiosResponse>(resolve => setTimeout(() => resolve(response), ms))

const requests = {
    get: (url: string) => axios.get(url).then(sleep(1000)).then(responseBody),
    post: (url: string, body: {}) => axios.post(url, body).then(sleep(1000)).then(responseBody),
    put: (url: string, body: {}) => axios.put(url, body).then(sleep(1000)).then(responseBody),
    delete: (url: string) => axios.delete(url).then(sleep(1000)).then(responseBody),
}

const trainingAgent = {
    list: (): Promise<ITraining[]> => requests.get('/home'),
    details: (id: string) => requests.get(`/training/${id}`),
    create: (training: ITraining) => requests.post('/training', training),
    update: (training: ITraining) => requests.put(`/training/${training.trainingId}`, training),
    delete: (id: string | undefined) => requests.delete(`/training/${id}`),
}

export default {
    trainingAgent
}