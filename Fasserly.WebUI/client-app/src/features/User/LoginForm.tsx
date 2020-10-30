import React, { useContext } from 'react';
import { Form as FinalForm, Field } from 'react-final-form';
import { Form, Button, Header, Divider } from 'semantic-ui-react';
import TextInput from '../../app/common/form/inputText';
import ErrorMessage from '../../app/common/form/ErrorMessage';
import { BaseRepositoryContext } from '../../app/repositories/baseRepository';
import { IUserFromValues } from '../../app/models/IUser';
import { FORM_ERROR } from 'final-form';
import { combineValidators, isRequired } from 'revalidate';
import SocialLogin from './SocialLogin';
import { observer } from 'mobx-react-lite';

const validate = combineValidators({
    email: isRequired('email'),
    password: isRequired('password'),
})

const LoginFrom = () => {
    const baseReposiitory = useContext(BaseRepositoryContext);
    const { login, fbLogin, loading } = baseReposiitory.userRepository;
    return (
        <FinalForm
            onSubmit={(values: IUserFromValues) => login(values).catch(error => ({
                [FORM_ERROR]: error
            }))}
            validate={validate}
            render={({ handleSubmit, submitting, submitError, invalid, pristine, dirtySinceLastSubmit }) => (
                <Form onSubmit={handleSubmit} error>
                    <Header as='h2' content='Login to Fasserly' color='blue' textAlign='center' />
                    <Field name='email' component={TextInput} placeholder='Email' />
                    <Field name='password' component={TextInput} placeholder='Password' type='Password' />
                    {submitError && !dirtySinceLastSubmit && <ErrorMessage error={submitError} text={submitError.status === 401 ? ' Unauthorized Invalid email or password' : 'unknown'}/>}
                    <Button
                        disabled={(invalid && !dirtySinceLastSubmit) || pristine  }
                        loading={submitting}
                        color='blue'
                        content='Login'
                        fluid
                    />
                    <Divider horizontal>OR</Divider>
                    <SocialLogin loading={loading} fbCallBack={fbLogin} />
                </Form>                
                )}
        />
    );
}

export default observer(LoginFrom);
