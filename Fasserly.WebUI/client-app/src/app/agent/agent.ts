﻿import { ITraining, ITrainingEnvelope } from '../models/ITraining';
import { IProfile, IPhoto } from '../models/IPorfile';
import { IUser, IUserFromValues } from '../models/IUser';
import axios, { AxiosResponse } from 'axios';
import { toast } from 'react-toastify';
import { history } from '../..';

axios.defaults.baseURL = process.env.REACT_APP_API_URL;
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
    const { data, status, config, headers } = error.response;
    if (!error.response)
        toast.error("Network error - Please check your connection ");
    if (status === 404)
        history.push('/notfound');
    if (status === 401 && headers['www-authenticate'].startsWith('Bearer error="invalid_token"')) {
        window.localStorage.removeItem('jwt');
        history.push('/');
        toast.info("Your session is expired, please login again");
    }
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
    get: (url: string) => axios.get(url).then(responseBody),
    post: (url: string, body: {}) => axios.post(url, body).then(responseBody),
    put: (url: string, body: {}) => axios.put(url, body).then(responseBody),
    delete: (url: string) => axios.delete(url).then(responseBody),
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
    list: (params: URLSearchParams): Promise<ITrainingEnvelope> =>
        axios.get('/training', { params: params }).then(responseBody),
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
    editProfile: (profile: Partial<IProfile>) => requests.put(`/profile`, profile),
    follow: (username: string) => requests.post(`/profile/${username}/follow`, {}),
    unfollow: (username: string) => requests.delete(`/profile/${username}/follow`),
    listFollowings: (username: string, predicate: string) => requests.get(`/profile/${username}/follow?predicate=${predicate}`),
    listTrainings: (username: string, predicate: string) => requests.get(`/profile/${username}/trainings?predicate=${predicate}`),
}
//User Agent
const userAgent = {
    current: (): Promise<IUser> => requests.get('/user'),
    login: (user: IUserFromValues): Promise<IUser> => requests.post(`/user/login`, user),
    register: (user: IUserFromValues): Promise<IUser> => requests.post(`/user/register`, user),
    fbLogin: (accessToken: string) => requests.post(`/user/facebook`, { accessToken }),
    refreshToken: (): Promise<IUser> => requests.post(`/user/refreshToken`, {}),
    verifyEmail: (token: string, email: string): Promise<IUser> => requests.post(`/user/verfiyEmail`, { token, email }),
    resendEmailConfirm: (email: string) => requests.get(`/user/resendEmailVerification?email=${email}`)
}
export default {
    trainingAgent,
    userAgent,
    profileAgent
}