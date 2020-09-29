import { observable, action, computed, runInAction } from 'mobx'
import { createContext } from 'react';
import agent from '../agent/agent';
import { ITraining } from '../models/ITraining';
import { history } from '../../index';
import { toast } from 'react-toastify';
import { BaseRepository } from './baseRepository'

export default class TrainingRepository {

    _baseRepository: BaseRepository;
    constructor(baseRepository: BaseRepository) {
        this._baseRepository = baseRepository;
    }

    @observable training: ITraining | null = null;
    @observable loading = false;
    @observable submitting = false;
    @observable trainingRegestry = new Map();

    @computed get trainingByDate() {
        return this.sortByDate(Array.from(this.trainingRegestry.values()));
    }

    sortByDate(trainings: ITraining[]) {
        const sortedTrainings = trainings.sort((a, b) => a.dateOfCreation!.getDate() - b.dateOfCreation!.getDate());
        return Object.entries(sortedTrainings.reduce((trainings, training) => {
            const date = training.dateOfCreation!.toISOString().split('T')[0];
            trainings[date] = trainings[date] ? [...trainings[date], training] : [training];
            return trainings;
        }, {} as { [key: string]: ITraining[] }));
    }

     @action trainingList = async () => {
        this.loading = true;
        try {
            const trainings = await agent.trainingAgent.list()
            runInAction('Loading', () => {
                trainings.forEach((training) => {
                    training.dateOfCreation = new Date(training.dateOfCreation);
                    this.trainingRegestry.set(training.trainingId, training);
                });
                this.loading = false;
                console.log(trainings);
            })

        } catch (error) {
            runInAction('loading Error', () => {
                console.log(error);
                this.loading = false;
            })
        }
    }

    @action createTraining = async (training: ITraining) => {
        this.submitting = true;
        try {
            await agent.trainingAgent.create(training);
            runInAction('Creating', () => {
                this.trainingRegestry.set(training.trainingId, training);
                this.training = training;
                this.submitting = false;
            })
            history.push(`/detailTraining/${training.trainingId}`);
        } catch (error) {
            runInAction('Creating Error', () => {
                console.log(error);
                this.submitting = false;
            })
            toast.error("Error submitting Data");
        }
    }

    @action editTraining = async (training: ITraining) => {
        this.submitting = true;
        try {
            await agent.trainingAgent.update(training);
            runInAction('editnig', () => {
                this.trainingRegestry.set(training.trainingId, training);
                this.training = training;
                this.submitting = false;
            })
            history.push(`/detailTraining/${training.trainingId}`);
        }
        catch (error) {
            runInAction('edit Error', () => {
                console.log(error);
                this.submitting = false;
            })
            toast.error("Error submitting Data");
        }
    }

    @action deleteTraining = async (id: string | undefined) => {
        this.submitting = true;
        try {
            await agent.trainingAgent.delete(id)
            runInAction(() => {
                this.trainingRegestry.delete(id);
                this.training = null;
                this.submitting = false;
            })
        } catch (e) {
            runInAction(() => {
                console.log(e);
                this.submitting = false;
            })
        }
    }

    @action loadTraining = async (id: string) => {
        let training = this.getTraining(id);
        if (training) {
            this.training = training;
            return training;
        } else {
            try {
                this.loading = true;
                training = await agent.trainingAgent.details(id);
                runInAction('Editing', () => {
                    //training.dateOfCreation = new Date(training.dateOfCreation);
                    this.training = training;
                    this.loading = false;
                    return training;
                });
            } catch (e) {
                runInAction('Edit error', () => {
                    console.log(e);
                    this.loading = false;
                });
            }
        }
    }

    @action getTraining = (id: string) => {
        return this.trainingRegestry.get(id);
    }

    @action selectTraining = (id: string) => {
        this.training = this.trainingRegestry.get(id);
    }
    @action clearTraining = () => {
        this.training = null;
    }
}
