import React from 'react';
import { SegmentGroup, Segment, Image, ItemGroup, Item, Button, Header, Rating, Label } from 'semantic-ui-react';
import { ITraining } from '../../../app/models/ITraining';
import { observer } from 'mobx-react-lite';
import { Link } from 'react-router-dom';

interface IProps {
    training: ITraining;
}

const trainingImageStyle = {
    filtre: 'Brightness(50%)'
}

const TrainingDetailHeader: React.FC<IProps> = ({ training }) => {
    return (
        <SegmentGroup>
            <Segment basic attached='top' style={{ padding: '0' }} as={Link} to={`/detailTraining/${training.trainingId}`} key={training.trainingId}>
                <Image src={`/assets/${training.title}.jpg`} size="medium" style={trainingImageStyle} />
                <Segment clearing attached="bottom">
                    <Rating maxRating={5} defaultRating={3} icon='star' size='small' />
                    <br />
                    {training.isOwner && <Label basic color='orange' content='You are the trainer of this course' />}
                </Segment>
                {training.isOwner || training.isOwner &&
                    <Segment clearing attached="bottom">
                        <Button color="teal" content="Put to basket" />
                        <Button color="orange" content="Buy now" />
                    </Segment>}  
            </Segment>
        </SegmentGroup>
    );
}

export default observer(TrainingDetailHeader);