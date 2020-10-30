import React from 'react';
import { Form as FinalForm, Field } from 'react-final-form';
import { Form, Button, } from 'semantic-ui-react';
import TextInput from '../form/inputText';
import InputTextArea from '../form/inputTextArea';
import { combineValidators, isRequired } from 'revalidate';
import { IProfile } from '../../models/IPorfile';
import { observer } from 'mobx-react-lite';

const validate = combineValidators({
    displayName: isRequired('displayName'),
})

interface IProps {
    editProfile: (profile: Partial<IProfile>) => void;
    profile: IProfile;
}

const ProfileEditForm: React.FC<IProps> = ({ editProfile, profile }) => {
    return (
        <FinalForm
            onSubmit={editProfile}
            validate={validate}
            initialValues={profile!}
            render={({ handleSubmit, invalid, pristine, submitting }) => (
                <Form onSubmit={handleSubmit} error>
                    <Field
                        name='displayName'
                        component={TextInput}
                        placeholder='DisplayName'
                        value={profile!.displayName}
                    />
                    <Field
                        name='bio'
                        component={InputTextArea}
                        rows={3}
                        placeholder='Bio'
                        value={profile!.bio}
                    />
                    <Button
                        loading={submitting}
                        floated='right'
                        disabled={invalid || pristine}
                        positive
                        content='Update profile'
                    />
                </Form>
            )}
        />
    );
};

export default observer(ProfileEditForm);
