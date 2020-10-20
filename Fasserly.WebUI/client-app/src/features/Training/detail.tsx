import React, { useContext, useEffect } from 'react';
import { Button, Card, Image, Label, Grid, GridColumn } from 'semantic-ui-react';
import { observer } from 'mobx-react-lite';
import { RouteComponentProps } from 'react-router';
import Loading from '../../app/layout/Loading';
import { Link } from 'react-router-dom';
import { format } from 'date-fns';
import { BaseRepositoryContext } from '../../app/repositories/baseRepository';
import TrainingDetailInfo from './trainingItem/trainingDetailInfo';
import TrainingDetailSideBar from './trainingItem/trainingDetailSideBar';
import TrainingDetailedChat from './trainingItem/TrainingDetailedChat';


interface DetailsParams {
    id: string;
}
const TrainingDetails: React.FC<RouteComponentProps<DetailsParams>> = ({ match, history }) => {

    const baseRepository = useContext(BaseRepositoryContext);
    const { training, loadTraining, loading, deleteTraining, refundTraining, buyTraining, loadingBuy } = baseRepository.trainingsRepository;

    useEffect(() => {
        loadTraining(match.params.id);
    }, [loadTraining, match.params.id, history])

    if (loading) return <Loading content={"Loading Training.."} />
    if (!training) {
        return <h1>srf</h1>
    }
    return (
        <Grid>
            <GridColumn width={10}>
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
                    {training.isOwner ?
                        (
                            <Card.Content extra>
                                <Button as={Link} to={`/manage/${training.trainingId}`} /*onClick={() => trainingStore.editMode = true}*/ color='green'>Edit</Button>
                                <Button /*onClick={() => deleteTraining(training?.trainingId)}*/ color='red'>Delete</Button>
                                <Button onClick={() => history.push('/trainings')} > Cancel</Button>
                            </Card.Content>) :
                        training.isBuyer ? (
                            <Card.Content extra>
                                <Button as={Link} to={`/share/${training.trainingId}`} color='green'>Share</Button>
                                <Button as={Link} to={`/favorite/${training.trainingId}`} color='red'>favorites</Button>
                                <Button loading={loadingBuy} onClick={refundTraining} > Refund</Button>
                            </Card.Content>) :
                            (
                                <Card.Content extra>
                                    <Button loading={loadingBuy} onClick={buyTraining} color='green'>Purchase now</Button>
                                    <Button as={Link} to={`/basket/${training.trainingId}`} color='teal'>Put to basket</Button>
                                    <Button onClick={() => history.push('/trainings')} > Cancel</Button>
                                </Card.Content>
                            )
                    }
                </Card>
                <TrainingDetailInfo training={training} />
                <TrainingDetailedChat />
            </GridColumn>
            <GridColumn width={6}>
                <TrainingDetailSideBar buyers={training.buyers} />
            </GridColumn>
        </Grid>
    );
}

export default observer(TrainingDetails);