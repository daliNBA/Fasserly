import React, { useContext } from 'react';
import { Item, Button, Segment, Icon, Label, Rating } from 'semantic-ui-react';
import { Link } from 'react-router-dom';
import { ITraining } from '../../app/models/ITraining';
import { format } from 'date-fns';
import TrainingListItemBuyers from './TrainingListItemBuyers';
import { BaseRepositoryContext } from '../../app/repositories/baseRepository';

const trainingImageStyle = {
    filtre: 'Brightness(50%)',
    marginBottom : 3
}

const TrainingItem: React.FC<{ training: ITraining }> = ({ training }) => {
    const owner = training.buyers.filter(x => x.isOwner)[0];
    const _baseRepository = useContext(BaseRepositoryContext);
    const { buyTraining } = _baseRepository.trainingsRepository;
    return (
        <Segment.Group>
            <Segment>
                <Item.Group>
                    <Item>
                        <Item.Image as={Link} to={`/detailTraining/${training.trainingId}`} src={`/assets/${training.title}.jpg`} size="medium" style={trainingImageStyle} />
                        <Item.Content>
                            <Item.Header as={Link} to={`/detailTraining/${training.trainingId}`}>
                                {training.title}
                            </Item.Header>
                            <Item.Description>Created By <Link to={`/profile/${owner.username}`}><strong>{owner.displayName}</strong></Link> </Item.Description>
                            <Item.Description><Rating maxRating={5} defaultRating={3} icon='star' size='small' /></Item.Description> 
                            {training.isOwner && (
                                <Item.Description>
                                    <Label
                                        basic
                                        color='orange'
                                        content='You are the trainer this activity'
                                    />
                                </Item.Description>
                            )}
                            {training.isBuyer && !training.isOwner && (
                                <Item.Description>
                                    <Label
                                        basic
                                        color='green'
                                        content='You are going to this activity'
                                    />
                                </Item.Description>
                            )}
                        </Item.Content>
                    </Item>
                </Item.Group>
            </Segment>
            <Segment>
                <Icon name='clock' /> {format(training.dateOfCreation, 'h:mm a')}
                <br />
                <span>{training.description}</span>
            </Segment>
            <Segment secondary>
                <TrainingListItemBuyers buyers={training.buyers} />
            </Segment>

            {!training.isOwner && !training.isBuyer &&
                <Segment clearing attached="bottom">
                <Button color="teal" content="Put to basket" />
                <Button color="orange" onClick={buyTraining} content="Buy now" />
                </Segment>
            }
        </Segment.Group>
    );
};

export default TrainingItem;
