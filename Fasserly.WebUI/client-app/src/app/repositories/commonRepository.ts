import { BaseRepository } from './baseRepository'
import { observable, action, reaction } from 'mobx';

export default class commonRepository {

    baseRepository: BaseRepository;
    constructor(baseRepository: BaseRepository) {
        this.baseRepository = baseRepository

        reaction(
            () => this.token,
            token =>
            {
                if (token) {
                    window.localStorage.setItem('jwt', token);
                }
                else {
                    window.localStorage.removeItem('jwt')
                }
            }
        )
    }

    @observable token: string | null = window.localStorage.getItem('jwt');;
    @observable appLoaded = false;

    @action setToken = (token: string | null) => {
        this.token = token;
    }
    @action setAppLoaded = () => {
        this.appLoaded = true;
    }
}