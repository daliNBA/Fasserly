import React, { useContext, useEffect } from 'react';
import { Button, Card, Image, Label } from 'semantic-ui-react';
import TrainingRepository from '../../app/repositories/trainingRepository';
import { observer } from 'mobx-react-lite';
import { RouteComponentProps } from 'react-router';
import Loading from '../../app/layout/Loading';
import { Link } from 'react-router-dom';
import { format } from 'date-fns';


interface DetailsParams {
    id: string;
}
const TrainingDetails: React.FC<RouteComponentProps<DetailsParams>> = ({ match, history }) => {

    const trainingStore = useContext(TrainingRepository);
    const { training, loadTraining, loading } = trainingStore;

    useEffect(() => {
        loadTraining(match.params.id);
    }, [loadTraining, match.params.id, history])

    if (loading) return <Loading content={"Loading Training.."} />
    if (!training) {
        return <h1>srf</h1>
    }
    return (
        <Card fluid>
            <Image size="tiny" src={`/assets/${training.title}.jpg`} ui={false} />
            <Card.Content>
                <Card.Header>
                    {training?.title}
                    <Label.Group tag>
                        <Label as='a'>€{training?.price}</Label>
                    </Label.Group>
                </Card.Header>
                <Card.Meta>
                    <span className='date'>{format(training.dateOfCreation, 'do MMMM YYYY')}</span>
                </Card.Meta>
                <Card.Description>
                    {training?.description}
                </Card.Description>
            </Card.Content>
            <Card.Content extra>
                <Button as={Link} to={`/manage/${training.trainingId}`} /*onClick={() => trainingStore.editMode = true}*/ color='green'>Edit</Button>
                <Button onClick={() => trainingStore.deleteTraining(training?.trainingId)} color='red'>Delete</Button>
                <Button onClick={() => history.push('/trainings')} > Cancel</Button>
            </Card.Content>
        </Card>
    );
}

export default observer(TrainingDetails);