export interface IUser {
    username: string;
    displayname: string;
    token: string;
    image?: string;
}

export interface IUserFromValues {
    email: string;
    password: string;
    username?: string;
    displayname?: string;
}