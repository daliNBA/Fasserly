import { BaseRepository } from './baseRepository'
import { observable, computed, action, runInAction } from 'mobx';

export default class commonRepository {

    baseRepository: BaseRepository;
    constructor(baseRepository: BaseRepository) {
        this.baseRepository = baseRepository
    }

    @observable token: string | null = null;
    @observable appLoaded = false;

    @action setToken = (token: string | null) => {
        window.localStorage.setItem('jwt', token!);
        this.token = token;
    }
    @action setAppUploaded = () => {
        this.appLoaded = true;
    }
}