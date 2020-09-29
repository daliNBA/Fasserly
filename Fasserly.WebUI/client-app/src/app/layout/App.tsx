import React, { Fragment } from 'react';
import { Container } from 'semantic-ui-react'
import NavBar from '../../features/Nav/navBar';
import { observer } from 'mobx-react-lite'
import { Route, RouteComponentProps, withRouter, Switch } from 'react-router-dom';
import TrainingDashboard from '../../features/Training/defaultDashboard';
import TrainingEdit from '../../features/Training/form';
import TrainingDetails from '../../features/Training/detail';
import NotFound from './NoFound';
import LoginFrom from '../../features/User/LoginForm'
import { ToastContainer } from 'react-toastify';

const App: React.FC<RouteComponentProps> = ({ location }) => {
    return (
        <div>
            <Fragment>
                <ToastContainer position='bottom-right' />
                <NavBar />
                <Container style={{ marginTop: '3em' }}>
                    <Switch>
                        <Route exact path='/' component={TrainingDashboard} />
                        <Route exact path='/trainings' component={TrainingDashboard} />
                        <Route key={location.key} exact path={['/createTraining', '/manage/:id']} component={TrainingEdit} />
                        <Route path='/detailTraining/:id' component={TrainingDetails} />
                        <Route path='/login' component={LoginFrom} />
                        <Route component={NotFound} />
                    </Switch>
                </Container>
            </Fragment>
        </div>
    );
}

export default withRouter(observer(App));