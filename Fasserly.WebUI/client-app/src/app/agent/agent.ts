import { ITraining } from '../models/ITraining';
import { IProfile, IPhoto } from '../models/IPorfile';
import { IUser, IUserFromValues } from '../models/IUser';
import axios, { AxiosResponse } from 'axios'
import { toast } from 'react-toastify';
import { history } from '../..';

axios.defaults.baseURL = ("https://localhost:44310/api");
//we can found this modifcations in (inspector => Network => request => header)
axios.interceptors.request.use((config) => {
    const token = window.localStorage.getItem('jwt');
    if (token) config.headers.Authorization = `Bearer ${token}`;
    return config;
}, error => {
    return Promise.reject(error)
})

axios.interceptors.response.use(undefined, (error) => {
    if (error.messagge === 'Network Error' && !error.response) 
        toast.error('Network error - make sure API is running!')
    const { data, status, config } = error.response;
    if (!error.response)
        toast.error("Network error - Please check your connection ");
    if (status === 404)
        history.push('/notfound');
    if (status === 400 && config.method === 'get' && data.errors.hasOwnProperty('id'))
        history.push('/notfound');
    if (status === 500)
        toast.error("server error - check the terminal for more info!");
    throw error.response;
})

const responseBody = (response: AxiosResponse) => response.data;
const sleep = (ms: number) => (response: AxiosResponse) =>
    new Promise<AxiosResponse>(resolve => setTimeout(() => resolve(response), ms))

const requests = {
    get: (url: string) => axios.get(url).then(sleep(1000)).then(responseBody),
    post: (url: string, body: {}) => axios.post(url, body).then(sleep(1000)).then(responseBody),
    put: (url: string, body: {}) => axios.put(url, body).then(sleep(1000)).then(responseBody),
    delete: (url: string) => axios.delete(url).then(sleep(1000)).then(responseBody),
    postForm: (url: string, file: Blob) => {
        let formData = new FormData();
        formData.append('File', file);
        return axios.post(url, formData, {
            headers: { 'content-type': 'multipart/form-data' }
        }).then(responseBody)
    }
};
//(Communication bteween controller and Client-app)
//training Agent 
const trainingAgent = {
    list: (): Promise<ITraining[]> => requests.get('/training'),
    details: (id: string) => requests.get(`/training/${id}`),
    create: (training: ITraining) => requests.post('/training', training),
    update: (training: ITraining) => requests.put(`/training/${training.trainingId}`, training),
    delete: (id: string | undefined) => requests.delete(`/training/${id}`),
    buy: (id: string | undefined) => requests.post(`/training/${id}/buy`, {}),
    refund: (id: string | undefined) => requests.delete(`/training/${id}/buy`),
}

//Profile agent
const profileAgent = {
    get: (username: string): Promise<IProfile> => requests.get(`/profile/${username}`),
    uploadPhoto: (photo: Blob): Promise<IPhoto> => requests.postForm(`/photo`, photo),
    setMainPhoto: (id: string) => requests.post(`/photo/${id}/setmain`, {}),
    deletePhoto: (id: string) => requests.delete(`/photo/${id}`),
    editProfile: (profile: Partial<IProfile>) => requests.put(`/profile`, profile)
}
//User Agent
const userAgent = {
    current: (): Promise<IUser> => requests.get('/user'),
    login: (user: IUserFromValues): Promise<IUser> => requests.post(`/user/login`, user),
    register: (user: IUserFromValues): Promise<IUser> => requests.post(`/user/register`, user),
}
export default {
    trainingAgent,
    userAgent,
    profileAgent
}