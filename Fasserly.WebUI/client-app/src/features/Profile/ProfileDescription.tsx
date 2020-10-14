import React, { useContext, useState } from 'react';
import { Tab, Header, List, Button, Grid } from 'semantic-ui-react';
import { BaseRepositoryContext } from '../../app/repositories/baseRepository';
import { observer } from 'mobx-react-lite';
import ProfileEditForm from '../../app/common/EditProfile/ProfileEditForm';

const ProfileDescription = () => {
    const baseRepo = useContext(BaseRepositoryContext);
    const { isCurrentUser, profile, editProfile } = baseRepo.profileRepository;
    const [editProfileMode, setEditProfileMode] = useState(false);
    return (
        <Tab.Pane>
            <Grid>
                <Grid.Column width={16} style={{ paddingBottom: 0 }}>
                    <Header floated='left' icon='address card' content={`About ${profile!.displayName}`} />
                    {isCurrentUser && (
                        <Button
                            onClick={() => setEditProfileMode(!editProfileMode)}
                            floated='right'
                            basic
                            content={editProfileMode ? 'Cancel' : 'Edit Profile'}
                        />
                    )}
                </Grid.Column>
                <Grid.Column width={16}>
                    {editProfileMode ? (
                        <ProfileEditForm editProfile={editProfile} profile={profile!} />
                    ) : (
                            <List>
                                <List.Item>
                                    <List.Icon name='marker' />
                                    <List.Content>{profile!.bio}</List.Content>
                                </List.Item>
                                <List.Item>
                                    <List.Icon name='mail' />
                                    <List.Content>{profile!.email}</List.Content>
                                </List.Item>
                            </List>
                        )}
                </Grid.Column>
            </Grid>
        </Tab.Pane>
    );
}

export default observer(ProfileDescription);