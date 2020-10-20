﻿import React, { useContext } from 'react';
import { Tab, Grid, Header, Card } from 'semantic-ui-react';
import { BaseRepositoryContext } from '../../app/repositories/baseRepository';
import ProfileCard from './ProfileCard';

const ProfileFollowings = () => {
    const baseRepo = useContext(BaseRepositoryContext);
    const { profile, followings, loading, activeTab } = baseRepo.profileRepository;

    return (
        <Tab.Pane loading={loading}>
            <Grid>
                <Grid.Column width={16}>
                    <Header
                        floated='left'
                        icon='user'
                        content={
                            activeTab === 3
                                ? `People following ${profile!.displayName}`
                                : `People ${profile!.displayName} is following`
                        }
                    />
                </Grid.Column>
                <Grid.Column width={16}>
                    <Card.Group itemsPerRow={5}>
                        {followings.map((following) => (
                            <ProfileCard key={following.username} following={following} />
                        ))}

                    </Card.Group>
                </Grid.Column>
            </Grid>
        </Tab.Pane>
    );
};

export default ProfileFollowings;