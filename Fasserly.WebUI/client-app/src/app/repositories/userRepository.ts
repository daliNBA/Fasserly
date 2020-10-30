import { observable, computed, action, runInAction } from 'mobx';
import { IUser, IUserFromValues } from '../models/IUser';
import agent from '../agent/agent';
import { history } from '../../index';
import { BaseRepository } from './baseRepository'

export default class UserRepository {

     refreshTokenTimeOut: any;
    _baseRepository: BaseRepository;
    constructor(baseRepository: BaseRepository) {
        this._baseRepository = baseRepository;
    }

    @observable user: IUser | null = null;
    @observable loading = false;

    @computed get isLoggedIn() { return !!this.user }

    @action login = async (values: IUserFromValues) => {
        try {
            const user = await agent.userAgent.login(values);
            runInAction(() => {
                this.user = user;
            })
            this._baseRepository.commonRepository.setToken(user.token);
            this.startRefreshTokenTimer(user);
            this._baseRepository.modalRepository.closeModal();
            history.push('/trainings');
            this._baseRepository.trainingsRepository.loadTrainings();
            console.log(this.user);
        } catch (error) {
            console.log(error);
            throw error;
        }
    }

    @action register = async (values: IUserFromValues) => {
        try {
            const user = await agent.userAgent.register(values);
            this._baseRepository.modalRepository.closeModal();
            this._baseRepository.commonRepository.setToken(user.token);
            history.push(`/user/registerSuccess?Email=${values.email}`);
            console.log(this.user);
        } catch (error) {
            console.log(error);
            throw error;
        }
    }

    @action getUser = async () => {
        try {
            const user = await agent.userAgent.current();
            runInAction(() => {
                this.user = user;
            })
            this._baseRepository.commonRepository.setToken(user.token);
            this.startRefreshTokenTimer(user);
        } catch (error) {
            console.log(error);
        }
    }

    @action logout = () => {
        this._baseRepository.commonRepository.setToken(null);
        this.user = null;
        this._baseRepository.trainingsRepository.loadTrainings();
    }

    @action refreshToken = async () => {
        this.stopRefreshTokenTimer();
        try {
            const user = await agent.userAgent.refreshToken();
            runInAction(() => {
                this.user = user;
            })
            this._baseRepository.commonRepository.setToken(user.token);
            this.startRefreshTokenTimer(user);
        } catch (e) {
            console.log(e);
        }
    }

    @action fbLogin = async (response: any) => {
        this.loading = true;
        try {
            const user = await agent.userAgent.fbLogin(response.accessToken);
            runInAction(() => {
                this.user = user;
                this._baseRepository.commonRepository.setToken(user.token);
                this.startRefreshTokenTimer(user);
                this._baseRepository.modalRepository.closeModal();
                history.push('/trainings');
                this._baseRepository.trainingsRepository.loadTrainings();
                this.loading = false;
            })

            console.log(this.user);
        } catch (error) {
            runInAction(() => {
                this.loading = false;
            })
            console.log(error);
            throw error;
        }
    }

    private startRefreshTokenTimer(user: IUser) {
        const jwtToken = JSON.parse(atob(user.token.split('.')[1]));
        const expired = new Date(jwtToken.exp * 1000);
        const timeOut = expired.getTime() - Date.now() - (60 * 1000);
        this.refreshTokenTimeOut = setTimeout(this.refreshToken, timeOut);
    }

    private stopRefreshTokenTimer() {
        clearTimeout(this.refreshTokenTimeOut);
    }
}
