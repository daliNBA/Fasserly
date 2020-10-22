import { observable, computed, action, runInAction } from 'mobx';
import { IUser, IUserFromValues } from '../models/IUser';
import agent from '../agent/agent';
import { history } from '../../index';
import { BaseRepository } from './baseRepository'

export default class UserRepository {

    _baseRepository: BaseRepository;
    constructor(baseRepository: BaseRepository) {
        this._baseRepository = baseRepository;
    }

    @observable user: IUser | null = null;

    @computed get isLoggedIn() { return !!this.user }

    @action login = async (values: IUserFromValues) => {
        try {
            const user = await agent.userAgent.login(values);
            runInAction(() => {
                this.user = user;
            })
            this._baseRepository.commonRepository.setToken(user.token);
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
            this._baseRepository.commonRepository.setToken(user.token);
            this._baseRepository.modalRepository.closeModal();
            runInAction(() => {
                this.user = user;
            })
            this._baseRepository.commonRepository.setToken(user.token);
            history.push('/trainings');
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
        } catch (error) {
            console.log(error);
        }
    }

    @action logout = () => {
        this._baseRepository.commonRepository.setToken(null);
        this.user = null;
        this._baseRepository.trainingsRepository.loadTrainings();
    }
}
