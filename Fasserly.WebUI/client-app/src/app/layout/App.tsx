import React, { Fragment, useContext, useEffect } from 'react';
import { Container } from 'semantic-ui-react'
import NavBar from '../../features/Nav/navBar';
import { observer } from 'mobx-react-lite'
import { Route, RouteComponentProps, withRouter, Switch } from 'react-router-dom';
import TrainingDashboard from '../../features/Training/defaultDashboard';
import TrainingEdit from '../../features/Training/form';
import TrainingDetails from '../../features/Training/detail';
import ProfilePage from '../../features/Profile/ProfilePage';
import RegisterSuccess from '../../features/User/RegisterSuccess';
import VerifyEmail from '../../features/User/VerifyEmail';
import NotFound from './NoFound';
import { ToastContainer } from 'react-toastify';
import { BaseRepositoryContext } from '../repositories/baseRepository';
import Loading from './Loading';
import ModalContainer from '../common/modal/ModalContainer';

const App: React.FC<RouteComponentProps> = ({ location }) => {
    const baseRepository = useContext(BaseRepositoryContext);
    const { token, setAppLoaded, appLoaded } = baseRepository.commonRepository;
    const { getUser } = baseRepository.userRepository;

    useEffect(() => {
        if (token && !appLoaded) {
            getUser().finally(() => setAppLoaded())
        }
        else {
            setAppLoaded();
        }
    }, [getUser, appLoaded, token, setAppLoaded])
    if (!appLoaded) return <Loading content='Loading app ...' />
    return (
        <div>
            <ModalContainer />
            <Fragment>
                <ToastContainer position='bottom-right' />
                <NavBar />
                <Container style={{ marginTop: '3em' }}>
                    <Switch>
                        <Route exact path='/' component={TrainingDashboard} />
                        <Route exact path='/trainings' component={TrainingDashboard} />
                        <Route key={location.key} exact path={['/createTraining', '/manage/:id']} component={TrainingEdit} />
                        <Route path='/detailTraining/:id' component={TrainingDetails} />
                        <Route path='/profile/:username' component={ProfilePage} />
                        <Route path='/user/RegisterSuccess' component={RegisterSuccess} />
                        <Route path='/user/VerifyEmail' component={VerifyEmail} />
                        <Route component={NotFound} />
                    </Switch>
                </Container>
            </Fragment>
        </div>
    );
}

export default withRouter(observer(App));