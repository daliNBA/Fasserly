import React, { useContext } from 'react';
import { Menu, Input, Button, Dropdown, Icon, Image } from 'semantic-ui-react';
import { observer } from 'mobx-react-lite';
import { NavLink, Link } from 'react-router-dom';
import { ITList } from '../../app/common/options/ITOptions'
import { BaseRepositoryContext } from '../../app/repositories/baseRepository';
import LoginFrom from '../User/LoginForm';
import RegisterForm from '../User/RegisterForm';

const NavBar: React.FC = () => {
    const baseReposiitory = useContext(BaseRepositoryContext);
    const { isLoggedIn, user, logout } = baseReposiitory.userRepository;
    const { openModal } = baseReposiitory.modalRepository;

    return (
        <div>
            <Menu pointing>
                <Menu.Item as={NavLink} exact to='/trainings' name='home'>
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
                {isLoggedIn && user && <Menu.Item>
                    <Button as={NavLink} to='/createTraining' content='Create Training' />
                </Menu.Item>
                }
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
                            <Dropdown pointing='top right' text={user.displayName}>
                                <Dropdown.Menu>
                                    <Dropdown.Item as={Link} to={`/profile/${user.username}`} text='My profile' icon='user' />
                                    <Dropdown.Item onClick={logout} text='Logout' icon='power'/>
                                </Dropdown.Menu>
                            </Dropdown>
                        </Menu.Item>
                    ) : (
                        <Menu.Item>
                            <div>
                                <Button onClick={() => openModal(<LoginFrom />)} basic color='blue'>Sign in</Button>
                                <Button onClick={() => openModal(<RegisterForm />)} color='red'>Sign up</Button>
                            </div>
                        </Menu.Item>)
                }
            </Menu>
        </div >
    );
}

export default observer(NavBar);
