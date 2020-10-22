import React, { useContext } from 'react'
import { RouteProps, RouteComponentProps, Route, Redirect } from 'react-router-dom';
import { BaseRepositoryContext } from '../../app/repositories/baseRepository';
import { observer } from 'mobx-react-lite';

interface IProps extends RouteProps {
    component: React.ComponentType<RouteComponentProps<any>>
}

const PrivateRoute: React.FC<IProps> = ({ component: Component, ...rest }) => {
    const baseRepo = useContext(BaseRepositoryContext);
    const { isLoggedIn } = baseRepo.userRepository;
    return (
        <Route
            {...rest}
            render={(props) => isLoggedIn ? <Component {...props} /> : <Redirect to='/' />}
        />
    )
}

export default observer(PrivateRoute)