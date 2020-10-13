import { ITraining, IBuyer } from "../../models/ITraining";
import { IUser } from "../../models/IUser";

export const combineTimeAndDate = (date: Date) => {
    const year = date.getFullYear();
    const month = date.getMonth() + 1;
    const day = date.getDate();
    const stringDate = `${year}-${month}-${day}`;
    return new Date(stringDate);
}

export const setTrainingProps = (training: ITraining, user: IUser) =>
{
    training.dateOfCreation = new Date(training.dateOfCreation);
    training.isBuyer = training.buyers.some(
        a => a.username === user.username
    )
    training.isOwner = training.buyers.some(
        b => b.username === user.username && b.isOwner
    )
    return training;
}

export const soldTraining = (user: IUser): IBuyer => {
    return {
        displayName: user.displayName,
        isOwner: false,
        username: user.username,
        image: user.image!,
    }
}
