export interface ITraining {
    trainingId: string;
    title: string;
    description: string;
    language: string;
    rating?: number;
    dateOfCreation: Date;
    updateDate?: Date;
    price: number;
    isActive: boolean;
    category: string;
}
export interface ITrainingFormValue extends Partial<ITraining> {
    time?: Date
}

export class TrainingFormValues implements ITraining {
    trainingId: string = '';
    title: string = '';
    language: string = '';
    price: number = 0;
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
