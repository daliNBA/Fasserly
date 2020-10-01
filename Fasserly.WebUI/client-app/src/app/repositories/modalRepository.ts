import { BaseRepository } from './baseRepository'
import { observable, action } from 'mobx';

export default class ModalRepository {
    baseRepository: BaseRepository;
    constructor(baseRepository: BaseRepository) {
        this.baseRepository = baseRepository;
    }

    @observable.shallow modal = {
        open: false,
        body: null
    }

    @action openModal = (content: any) => {
        this.modal.open = true;
        this.modal.body = content;
    }


    @action closeModal = () => {
        this.modal.open = false;
        this.modal.body = null;
    }
}
