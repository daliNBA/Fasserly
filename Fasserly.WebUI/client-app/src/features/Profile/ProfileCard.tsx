import React from 'react';
import { Card, Image, Icon } from 'semantic-ui-react';
import { Link } from 'react-router-dom';
import { IProfile } from '../../app/models/IPorfile';
interface IProps {
    following: IProfile;
}
const ProfileCard: React.FC<IProps> = ({ following }) => {
    return (
        <Card as={Link} to={`/profile/${following.username}`}>
            <Image src={following.image || '/assets/user.jpg'} />
            <Card.Content>
                <Card.Header>{following.displayName}</Card.Header>
            </Card.Content>
            <Card.Content extra>
                <div>
                    <Icon name='user' />
                    {following.followersCount} followers
                </div>
            </Card.Content>
        </Card>
    );
};

export default ProfileCard;