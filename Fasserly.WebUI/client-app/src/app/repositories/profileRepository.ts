import { BaseRepository } from './baseRepository'
import { observable, runInAction, action, computed } from 'mobx';
import { IProfile, IPhoto } from '../models/IPorfile';
import agent from '../agent/agent';
import { toast } from 'react-toastify';



export default class ProfileRepository {
    baseRepository: BaseRepository
    constructor(baseRepository: BaseRepository) {
        this.baseRepository = baseRepository;
    }

    @observable profile: IProfile | null = null;
    @observable loadingProfile = true;
    @observable uploadingPhoto = false;
    @observable loading = false;

    @computed get isCurrentUser() {
        if (this.baseRepository.userRepository.user && this.profile)
            return this.baseRepository.userRepository.user.username === this.profile.username;
        else
            return false;
    }

    @action loadProfile = async (username: string) => {
        try {
            const profile = await agent.profileAgent.get(username);
            runInAction(() => {
                this.profile = profile;
                this.loadingProfile = false;
            })
        } catch (error) {
            runInAction(() => {
                this.loadingProfile = false;
            })
            console.log(error);
        }
    }

    @action uploadPhoto = async (file: Blob) => {
        this.uploadingPhoto = true;
        try {
            const photo = await agent.profileAgent.uploadPhoto(file);
            runInAction(() => {
                if (this.profile) {
                    this.profile.photos.push(photo);
                    if (photo.isMain && this.baseRepository.userRepository.user) {
                        this.baseRepository.userRepository.user.image = photo.url;
                        this.profile.image = photo.url;
                        this.profile.image = photo.url;
                    }
                }
                this.uploadingPhoto = false;
            })
        } catch (e) {
            console.log(e);
            toast.error("Problem Uploading photo!");
            runInAction(() => {
                this.uploadingPhoto = false;
            })
        }
    }

    @action setMainPhoto = async (photo: IPhoto) => {
        this.loading = true;
        try {
            await agent.profileAgent.setMainPhoto(photo.id);
            runInAction(() => {
                this.baseRepository.userRepository.user!.image = photo.url;
                this.profile!.photos.find(a => a.isMain)!.isMain = false;
                this.profile!.photos.find(a => a.id === photo.id)!.isMain = true;
                this.profile!.image = photo.url;
                this.loading = false;
            })
        } catch (e) {
            console.log(e);
            toast.error("Problem setting photo as main!");
            runInAction(() => {
                this.loading = false;
            })
        }
    };

    @action deletePhoto = async (photo: IPhoto) => {
        this.loading = true;
        try {
            await agent.profileAgent.deletePhoto(photo.id);
            runInAction(() => {
                this.profile!.photos = this.profile!.photos.filter(a => a.id !== photo.id);
                this.loading = false;
            })
        } catch (e) {
            console.log(e);
            toast.error("Problem deleting photo!");
            runInAction(() => {
                this.loading = false;
            })
        }
    }

    @action editProfile = async (profile: Partial<IProfile>) => {
        try {
            await agent.profileAgent.editProfile(profile);
            runInAction(() => {
                if (profile.displayName !== this.baseRepository.userRepository.user!.displayName) {
                    this.baseRepository.userRepository.user!.displayName = profile.displayName!;
                }
                this.profile = { ...this.profile!, ...profile }
            })
        } catch (e) {
            toast.error("Problem updating profile");
            runInAction(() => {
            })
        }
    }
}
