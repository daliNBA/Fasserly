export interface IProfile {
    displayName: string;
    username: string;
    bio: string;
    image: string;
    email: string;
    following: boolean;
    followersCount: number;
    followingsCount: number; 
    photos: IPhoto[];
}

export interface IUserTraining {
    id: string;
    title: string;
    category: string;
    date: Date;
}


export interface IPhoto {
    id: string;
    url: string;
    isMain: boolean;
}