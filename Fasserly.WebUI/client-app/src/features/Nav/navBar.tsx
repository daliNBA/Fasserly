import React, { useContext } from 'react';
import { Menu, Input, Button, Dropdown, Icon, Header, Image } from 'semantic-ui-react';
import { observer } from 'mobx-react-lite';
import { NavLink, Link } from 'react-router-dom';
import { ITList } from '../../app/common/options/ITOptions'
import { BaseRepositoryContext } from '../../app/repositories/baseRepository';

const NavBar: React.FC = () => {
    const baseReposiitory = useContext(BaseRepositoryContext);
    const { isLoggedIn, user, logout } = baseReposiitory.userRepository;
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
                {isLoggedIn && user ?
                    (
                        <Menu.Item>
                            <Image avatar spaced='right' src={user.image || '/assets/Logo.png'} />
                            <Dropdown pointing='top right' text='User' >
                                <Dropdown.Menu>
                                    <Dropdown.Item as={Link} to={`/profile/username`} text='My profile' icon='user' />
                                    <Dropdown.Item onClick={logout} text='Logout' icon='power' />
                                </Dropdown.Menu>
                            </Dropdown>
                        </Menu.Item>
                    ) : (
                        <Menu.Item>
                            <div>
                                <Button as={Link} to='/login' basic color='blue'>Sign in</Button>
                                <Button color='red'>Sign up</Button>
                            </div>
                        </Menu.Item>)
                }
            </Menu>
        </div >
    );
}

export default observer(NavBar);
