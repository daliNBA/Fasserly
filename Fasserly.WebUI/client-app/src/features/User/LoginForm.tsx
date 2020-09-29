import React, { useContext } from 'react';
import { Form as FinalForm, Field } from 'react-final-form';
import { Form, Button, Label } from 'semantic-ui-react';
import TextInput from '../../app/common/form/inputText';
import { BaseRepositoryContext } from '../../app/repositories/baseRepository';
import { IUserFromValues } from '../../app/models/IUser';
import { FORM_ERROR } from 'final-form';
import { combineValidators, isRequired } from 'revalidate';

const validate = combineValidators({
    email: isRequired('email'),
    password: isRequired('password'),
})

const LoginFrom = () => {
    const baseReposiitory = useContext(BaseRepositoryContext);
    const { login } = baseReposiitory.userRepository;
    return (
        <FinalForm
            onSubmit={(values: IUserFromValues) => login(values).catch(error => ({
                [FORM_ERROR]: error
            }))}
            validate={validate}
            render={({ handleSubmit, submitting, form, submitError, invalid, pristine, dirtySinceLastSubmit }) => (
                <Form onSubmit={ handleSubmit }>
                    <Field name='email' component={TextInput} placeholder='Email' />
                    <Field name='password' component={TextInput} placeholder='Password' type='Password' />
                    {submitError && !dirtySinceLastSubmit && <Label color='red' basic content={submitError.status == 401 ? 'Unauthorized' : 'unknown'} />}
                    <br />
                    <Button
                        disabled={pristine && !dirtySinceLastSubmit || invalid}
                        loading={submitting}
                        positive
                        content='Login'
                    />
                    <pre>{JSON.stringify(form.getState(), null, 2)}</pre>
                </Form>                
                )}
        />
    );
}

export default LoginFrom;
