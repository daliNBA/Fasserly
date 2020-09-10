import React from 'react';
import { Menu, Input, Button, Dropdown, Icon } from 'semantic-ui-react';
import { observer } from 'mobx-react-lite';
import { NavLink } from 'react-router-dom';
import { ITList } from '../../app/common/options/ITOptions'

const NavBar: React.FC = () => {
    return (
        <div>
            <Menu pointing>
                <Menu.Item as={NavLink} exact to='/' name='home'>
                    <img src='/assets/Logo.png' alt='Logo' height='30px' width='30' />
                </Menu.Item>
                <Dropdown
                    item
                    simple
                    text='Categories'
                    direction='right'
                    options={ITList}
                />
                <Menu.Item >
                    <Input icon='search' placeholder='Search...' position='left' />
                </Menu.Item>
                <Menu.Item>
                    <Button as={NavLink} to='/createTraining' content='Create Training' />
                </Menu.Item>
                <Menu.Item position='right'>
                    <Button animated='vertical' margin='10'>
                        <Button.Content hidden>Basket</Button.Content>
                        <Button.Content visible>
                            <Icon name='shop' />
                        </Button.Content>
                    </Button>
                </Menu.Item>
                <Menu.Item>
                    <div>
                        <Button basic color='blue'>Sign in</Button>
                        <Button color='red'>Sign up</Button>
                    </div>
                </Menu.Item>
            </Menu>
        </div >
    );
}

export default observer(NavBar);
