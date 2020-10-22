import React, { useEffect, useContext } from 'react';
import { observer } from 'mobx-react-lite';
import { Tab, Grid, Header, Card, Image, TabProps } from 'semantic-ui-react';
import { Link } from 'react-router-dom';
import { format } from 'date-fns';
import { BaseRepositoryContext } from '../../app/repositories/baseRepository';
import { IUserTraining } from '../../app/models/IPorfile';

const panes = [
    { menuItem: 'Owining', pane: { key: 'owned' } }
];
const loggedPanes = [
    { menuItem: 'Owining', pane: { key: 'owned' } },
    { menuItem: 'Past Courses', pane: { key: 'pastCourses' } },
    { menuItem: 'Future Courses', pane: { key: 'futureCourses' } }
];
const ProfileEvents = () => {
    const baseRepo = useContext(BaseRepositoryContext);
    const {
        loadUserTrainings,
        profile,
        loadingTraining,
        userTrainings
    } = baseRepo.profileRepository!;
    const { isLoggedIn } = baseRepo.userRepository;

    useEffect(() => {
        loadUserTrainings(profile!.username, 'owning');
    }, [loadUserTrainings, profile]);

    const handleTabChange = (
        e: React.MouseEvent<HTMLDivElement, MouseEvent>,
        data: TabProps
    ) => {
        let predicate;
        switch (data.activeIndex) {
            case 1:
                predicate = 'past';
                break;
            case 2:
                predicate = 'future';
                break;
            default:
                predicate = 'owning';
                break;
        }
        loadUserTrainings(profile!.username, predicate);
    };

    return (
        <Tab.Pane loading={loadingTraining}>
            <Grid>
                <Grid.Column width={16}>
                    <Header floated='left' icon='book' content={'Trainings'} />
                </Grid.Column>
                <Grid.Column width={16}>
                    <Tab
                        panes={isLoggedIn ? loggedPanes : panes}
                        menu={{ secondary: true, pointing: true }}
                        onTabChange={(e, data) => handleTabChange(e, data)}
                    />
                    <br />
                    <Card.Group itemsPerRow={4}>
                        {userTrainings.map((training: IUserTraining) => (
                            <Card
                                as={Link}
                                to={`/training/${training.id}`}
                                key={training.id}
                            >
                                <Image
                                    src={`/assets/${training.title}.jpg`}
                                    style={{ minHeight: 100, objectFit: 'cover' }}
                                />
                                <Card.Content>
                                    <Card.Header textAlign='center'>{training.title}</Card.Header>
                                    <Card.Meta textAlign='center'>
                                        <div>{format(new Date(training.date), 'do LLL')}</div>
                                        <div>{format(new Date(training.date), 'h:mm a')}</div>
                                    </Card.Meta>
                                </Card.Content>
                            </Card>
                        ))}
                    </Card.Group>
                </Grid.Column>
            </Grid>
        </Tab.Pane>
    );
};

export default observer(ProfileEvents);