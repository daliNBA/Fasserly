import TrainingRepository from './trainingRepository';
import UserRepository from './userRepository';
import CommonRepository from './commonRepository';
import ModalRepository from './modalRepository';
import { createContext } from 'react';
import { configure } from 'mobx';

configure({ enforceActions: "always" });

export class BaseRepository {

    trainingsRepository: TrainingRepository;
    userRepository: UserRepository;
    commonRepository: CommonRepository;
    modalRepository: ModalRepository;

    constructor() {
        this.trainingsRepository = new TrainingRepository(this);
        this.userRepository = new UserRepository(this);
        this.commonRepository = new CommonRepository(this);
        this.modalRepository = new ModalRepository(this);
    }
}
export const BaseRepositoryContext = createContext(new BaseRepository());