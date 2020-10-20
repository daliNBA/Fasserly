import React, { useContext, useEffect } from 'react';
import ProfileHeader from './ProfileHeader';
import ProfileContent from './ProfileContent';
import { Grid } from 'semantic-ui-react';
import { BaseRepositoryContext } from '../../app/repositories/baseRepository';
import { RouteComponentProps } from 'react-router-dom';
import Loading from '../../app/layout/Loading';
import { observer } from 'mobx-react-lite';

interface RouteParams {
    username: string
}

interface IProps extends RouteComponentProps<RouteParams> { }

const ProfilePage: React.FC<IProps> = ({ match }) => {
    const baseRepo = useContext(BaseRepositoryContext);
    const { loadProfile, profile, loadingProfile, follow, unfollow, isCurrentUser, loading, setActiveTab } = baseRepo.profileRepository;

    useEffect(() => {
        loadProfile(match.params.username);
    }, [loadProfile, match])
    if (loadingProfile) return <Loading content="Loading Profile ..." />

    return (
        <Grid>
            <Grid.Column width={16}>
                <ProfileHeader
                    profile={profile!}
                    follow={follow}
                    unfollow={unfollow}
                    isCurrentUser={isCurrentUser}
                    loading={loading}
                />
                <ProfileContent setActiveTab={setActiveTab}/>
            </Grid.Column>
        </Grid>

    );
}

export default observer(ProfilePage);