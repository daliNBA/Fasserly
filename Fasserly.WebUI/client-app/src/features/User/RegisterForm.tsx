import React, { useContext } from 'react';
import { Form as FinalForm, Field } from 'react-final-form';
import { Form, Button, Header } from 'semantic-ui-react';
import TextInput from '../../app/common/form/inputText';
import ErrorMessage from '../../app/common/form/ErrorMessage';
import { BaseRepositoryContext } from '../../app/repositories/baseRepository';
import { IUserFromValues } from '../../app/models/IUser';
import { FORM_ERROR } from 'final-form';
import { combineValidators, isRequired } from 'revalidate';

const validate = combineValidators({
    email: isRequired('Email'),
    username: isRequired('Username'),
    displayName: isRequired('DiplayName'),
    password: isRequired('Password'),
})

const RegisterFrom = () => {
    const baseReposiitory = useContext(BaseRepositoryContext);
    const { register } = baseReposiitory.userRepository;
    return (
        <FinalForm
            onSubmit={(values: IUserFromValues) => register(values).catch(error => ({
                [FORM_ERROR]: error
            }))}
            validate={validate}
            render={({ handleSubmit, submitting, submitError, invalid, pristine, dirtySinceLastSubmit }) => (
                <Form onSubmit={handleSubmit} error>
                    <Header as='h2' content='Sign Up to Fasserly' color='blue' textAlign='center' />
                    <Field name='username' component={TextInput} placeholder='Username' />
                    <Field name='displayName' component={TextInput} placeholder='DisplayName' />
                    <Field name='email' component={TextInput} placeholder='Email' />
                    <Field name='password' component={TextInput} placeholder='Password' type='Password' />
                    {submitError && !dirtySinceLastSubmit && <ErrorMessage error={submitError} />}
                    <Button
                        disabled={(invalid && !dirtySinceLastSubmit) || pristine}
                        loading={submitting}
                        color='blue'
                        content='Register'
                        fluid
                    />
                </Form>
            )}
        />
    );
}

export default RegisterFrom;
