import TrainingRepository from './trainingRepository';
import UserRepository from './userRepository';
import CommonRepository from './commonRepository';
import { createContext } from 'react';
import { configure } from 'mobx';

configure({ enforceActions: "always" });

export class BaseRepository {

    trainingsRepository: TrainingRepository;
    userRepository: UserRepository;
    commonRepository : CommonRepository

    constructor() {
        this.trainingsRepository = new TrainingRepository(this);
        this.userRepository = new UserRepository(this);
        this.commonRepository = new CommonRepository(this);
    }
}
export const BaseRepositoryContext = createContext(new BaseRepository());