import { observable, action, computed, runInAction } from 'mobx'
import agent from '../agent/agent';
import { ITraining } from '../models/ITraining';
import { history } from '../../index';
import { toast } from 'react-toastify';
import { BaseRepository } from './baseRepository'
import { setTrainingProps, soldTraining } from '../common/util/util';
import { SyntheticEvent } from 'react';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

export default class TrainingRepository {

    _baseRepository: BaseRepository;
    constructor(baseRepository: BaseRepository) {
        this._baseRepository = baseRepository;
    }

    @observable training: ITraining | null = null;
    @observable loading = false;
    @observable submitting = false;
    @observable trainingRegestry = new Map();
    @observable target = '';
    @observable loadingBuy = false;
    //the hole class will be observe it (Hub connexion SignalR)
    @observable.ref hubConnection: HubConnection | null = null;

    @computed get trainingByDate() {
        return this.sortByDate(Array.from(this.trainingRegestry.values()));
    }

    //Hub for Signal token connection
    @action createHubConnection = (trainingId: string) => {
        this.hubConnection = new HubConnectionBuilder()
            //this url must have the same url of server
            .withUrl('https://localhost:44310/chat', {
                accessTokenFactory: () => this._baseRepository.commonRepository.token!
            })
            .configureLogging(LogLevel.Information)
            .build();
        this.hubConnection.start()
            .then(() => console.log(this.hubConnection!.state))
            .then(() => {
                console.log("Attempting to join group");
                //have to much to same name of methode chatHub
                this.hubConnection!.invoke('AddToGroup', trainingId);
            })
            .catch(error => console.log('Error estabilshed connection', error));
        //this must match the same of ReceiveComment in chatHub
        this.hubConnection.on('ReceiveComment', comment => {
            runInAction(() => {
                this.training!.comments.push(comment);
            })
        })
        this.hubConnection.on('Send', message => {
            toast.info(message);
        })
    }

    @action stopHubConnection = () => {
        this.hubConnection!.invoke('RemoveFromGroup', this.training?.trainingId)
            .then(() => {
                this.hubConnection?.stop()
            }).then(() => console.log('Connection stopped'))
            .catch(err => console.log(err));
        this.hubConnection!.stop();
    }

    @action addcomment = async (values: any) => {
        values.trainingId = this.training?.trainingId;
        try {
            //SendComment must have the same name of methode in hubChat
            await this.hubConnection!.invoke('SendComment', values)
        } catch (e) {
            console.log(e);
        }
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
                    setTrainingProps(training, this._baseRepository.userRepository.user!);
                    this.trainingRegestry.set(training.trainingId, training);
                    console.log(training.buyers);
                });
                this.loading = false;
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
            const buyer = soldTraining(this._baseRepository.userRepository.user!);
            buyer.isOwner = true;
            let buyers = [];
            buyers.push(buyer);
            training.buyers = buyers;
            training.isOwner = true;
            training.comments = [];
            runInAction('Creating training', () => {
                this.trainingRegestry.set(training.trainingId, training);
                this.training = training;
                this.submitting = false;
            })
            history.push(`/detailTraining/${training.trainingId}`);
        } catch (error) {
            runInAction('Creating training Error', () => {
                this.submitting = false;
                console.log(error);
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

    @action deleteTraining = async (event: SyntheticEvent<HTMLButtonElement>, id: string) => {
        this.submitting = true;
        this.target = event.currentTarget.name;
        try {
            await agent.trainingAgent.delete(id)
            runInAction(() => {
                this.trainingRegestry.delete(id);
                this.training = null;
                this.submitting = false;
                this.target = '';
            })
        } catch (e) {
            runInAction(() => {
                console.log(e);
                this.submitting = false;
                this.target = '';
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
                runInAction('Loading', () => {
                    setTrainingProps(training, this._baseRepository.userRepository.user!);
                    this.training = training;
                    this.loading = false;
                    console.log(training);
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

    @action buyTraining = async () => {
        const buyer = soldTraining(this._baseRepository.userRepository.user!);
        this.loadingBuy = true;
        try {

            await agent.trainingAgent.buy(this.training!.trainingId);
            runInAction(() => {
                if (this.training) {
                    this.training.buyers.push(buyer);
                    this.training.isBuyer = true;
                    this.trainingRegestry.set(this.training.trainingId, this.training);
                    this.loadingBuy = false;
                }
            })

        } catch (error) {
            runInAction(() => {
                this.loadingBuy = false;
            })
            toast.error('Problem buying course')
        }
    }

    @action refundTraining = async () => {
        this.loadingBuy = true;
        try {
            await agent.trainingAgent.refund(this.training!.trainingId);
            runInAction(() => {
                if (this.training) {
                    this.training.buyers = this.training.buyers.filter(b => b.username !== this._baseRepository.userRepository.user!.username);
                    this.training.isBuyer = false;
                    this.trainingRegestry.set(this.training.trainingId, this.training);
                    this.loadingBuy = false;
                }
            })
        } catch (error) {
            runInAction(() => {
                this.loadingBuy = false;
            })
            toast.error('Problem buying course')
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
