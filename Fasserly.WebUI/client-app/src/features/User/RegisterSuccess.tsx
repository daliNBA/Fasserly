import React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { Button, Header, Segment, Icon } from 'semantic-ui-react';
import agent from '../../app/agent/agent';
import { toast } from 'react-toastify';
import queryString from 'query-string';

const RegisterSuccess: React.FC<RouteComponentProps> = ({ location }) => {
    const { email } = queryString.parse(location.search);
    const resensendEmailHandler = () => {
        console.log(email);
        agent.userAgent.resendEmailConfirm("dali").then(() => {
            toast.success('please check you email');
        }).catch((e) => console.log(e));
    }
    return (
        <Segment placeholder>
            <Header icon>
                <Icon name="check" />
                Successfully registred
            </Header>
            <Segment.Inline>
                <div className="centre">
                    <p>
                        Please check your email (including your junk folder) for the verification email
                    </p>
                    {email &&
                        <>
                            <p>Didn't receive the mail please click the button below to resend </p>
                            <Button onClick={resensendEmailHandler}
                                primary
                                content="Resend email"
                                size="huge"
                            />
                        </>
                    }
                </div>
            </Segment.Inline>
        </Segment>
    );
}

export default RegisterSuccess;