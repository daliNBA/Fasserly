import React from 'react';
import { SegmentGroup, Segment, Image, ItemGroup, Item, Button, Header, Rating } from 'semantic-ui-react';
import { ITraining } from '../../../app/models/ITraining';
import { observer } from 'mobx-react-lite';
import { Link } from 'react-router-dom';
interface IProps {
    training: ITraining;
}

const trainingImageStyle = {
    filtre: 'Brightness(50%)'
}

const trainingImageTextStyle = {

    position: 'absolute',
    bottom: '5%',
    left: '5%',
    width: '100%',
    heigth: 'auto',
    color: 'white'
}

const TrainingDetailHeader: React.FC<IProps> = ({ training }) => {
    return (
        <SegmentGroup>
            <Segment basic attached='top' style={{ padding: '0' }} as={Link} to={`/detailTraining/${training.trainingId}`} key={training.trainingId}>
                <Image src={`/assets/${training.title}.jpg`} size="medium" style={trainingImageStyle} />
                <Segment basic style={trainingImageTextStyle}>
                    <ItemGroup>
                        <Item>
                            <Item.Content>
                                <Header size='large' content={'Title'} style={{ color: 'white' }} />
                                <p>{training.description}</p>
                            </Item.Content>
                        </Item>
                    </ItemGroup>
                </Segment>
                <Segment clearing attached="bottom">
                    <Rating maxRating={5} defaultRating={3} icon='star' size='small' />
                    <br />
                    <br />
                </Segment>
                <Segment clearing attached="bottom">
                    <Button color="teal" content="Join Us" />
                    <Button color="orange" content="Join Us" />
                    <Button color="teal" floated="right" content="Hello" />
                </Segment>
            </Segment>
        </SegmentGroup>
    );
}

export default observer(TrainingDetailHeader);