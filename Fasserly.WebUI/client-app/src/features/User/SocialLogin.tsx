import React from 'react';
import FacebookLogin from 'react-facebook-login/dist/facebook-login-render-props'
import { Button, Icon } from 'semantic-ui-react';
import { observer } from 'mobx-react-lite';

interface IProps {
    fbCallBack: (response: any) => void;
    loading: boolean;
}

const SocialLogin: React.FC<IProps> = ({ fbCallBack, loading }) => {
    return (
        <div>
            <FacebookLogin
                appId='366656587886217'
                fields='name,email,picture'
                callback={fbCallBack}
                render={(renderProps: any) => (
                    <Button onClick={renderProps.onClick} type="button" fluid color="facebook" loading={loading}>
                        <Icon name="facebook" />
                        Login with Facebook
                    </Button>
                    )}
            />
        </div>
    );
}

export default observer(SocialLogin);