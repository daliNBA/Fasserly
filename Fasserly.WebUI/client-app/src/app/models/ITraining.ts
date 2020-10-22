export interface ITrainingEnvelope {
    trainings: ITraining[];
    trainingCount: number;
}
export interface ITraining {
    trainingId: string;
    title: string;
    description: string;
    language: string;
    rating: number;
    dateOfCreation: Date;
    updateDate?: Date;
    price: string;
    isActive: boolean;
    category: string;
    isBuyer: boolean;
    isOwner: boolean;
    buyers?: IBuyer[];
    comments: IComment[]
}
export interface ITrainingFormValue extends Partial<ITraining> {
    time?: Date
}
export interface IComment {
    trainingId: string;
    dateOfComment: string;
    body: string;
    username: string;
    diplayName: string;
    image: string;
}
export class TrainingFormValues implements ITrainingFormValue {
    trainingId: string = '';
    title: string = '';
    language: string = '';
    price: string = '0';
    rating: number = 0;
    dateOfCreation: Date = new Date(Date.now());
    updateDate: Date = new Date(Date.now());
    description: string = '';
    isActive: boolean = true;
    category: string = '';

    constructor(init?: TrainingFormValues) {
        Object.assign(this, init)
    }
}

export interface IBuyer {
    username: string;
    displayName: string;
    image: string;
    isOwner: boolean;
    following?: boolean;
}
