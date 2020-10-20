import React, { Fragment } from 'react';
import { Segment, List, Item, Label, Image } from 'semantic-ui-react';
import { Link } from 'react-router-dom';
import { IBuyer } from '../../../app/models/ITraining';
import { observer } from 'mobx-react-lite';

interface IProps {
    buyers: IBuyer[];
}

const ActivityDetailedSidebar: React.FC<IProps> = ({ buyers }) => {
    return (
        <Fragment>
            <Segment
                textAlign='center'
                style={{ border: 'none' }}
                attached='top'
                secondary
                inverted
                color='teal'
            >
                {buyers.length} {buyers.length === 1 ? "Person" : "People"} participate
      </Segment>
            <Segment attached>
                <List relaxed divided>
                    {buyers.map((buyer) =>
                        <Item key={buyer.username} style={{ position: 'relative' }} >
                            {buyer.isOwner && <Label
                                style={{ position: 'absolute' }}
                                color='orange'
                                ribbon='right'
                            >
                               the trainer
                            </Label>}
                            <Image size='tiny' src={buyer.image || '/assets/user.jpg'} />
                            <Item.Content verticalAlign='middle'>
                                <Item.Header as='h3'>
                                    <Link to={`/profile/${buyer.username}`}>{buyer.displayName}</Link>
                                </Item.Header>
                                {buyer.following && <Item.Extra style={{ color: 'orange' }}>Following</Item.Extra> }
                                
                            </Item.Content>
                        </Item>
                        )}
                </List>
            </Segment>
        </Fragment>
    );
};

export default observer(ActivityDetailedSidebar);
