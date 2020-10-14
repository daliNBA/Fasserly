﻿import React from 'react';
import { List, Image, Popup } from 'semantic-ui-react';
import { IBuyer } from '../../app/models/ITraining';

interface IProps {
    buyers: IBuyer[]
}

const TrainingListItemBuyers: React.FC<IProps> = ({ buyers }) => {
    return (
        <List horizontal>
            {buyers.map((buyer) => (
                <List.Item key={buyer.username} >
                    <Popup header={buyer.displayName}
                        trigger={<Image size='mini' circular src={buyer.image || '/assets/user.jpg'} />}
                    />
                </List.Item>
            ))}
        </List>
    );
}

export default TrainingListItemBuyers;