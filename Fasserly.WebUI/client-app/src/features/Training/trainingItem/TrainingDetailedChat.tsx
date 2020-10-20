import React, { Fragment, useContext, useEffect } from 'react';
import { Segment, Header, Form, Button, Comment } from 'semantic-ui-react';
import { BaseRepositoryContext } from '../../../app/repositories/baseRepository';
import { Form as FinalFrom, Field } from 'react-final-form';
import { Link } from 'react-router-dom';
import InputTextArea from '../../../app/common/form/inputTextArea';
import { observer } from 'mobx-react-lite';
import { formatDistance } from 'date-fns';

const TrainingDetailedChat = () => {
    const baseRepo = useContext(BaseRepositoryContext);
    const { createHubConnection, stopHubConnection, addcomment, training } = baseRepo.trainingsRepository;
    useEffect(() => {
        createHubConnection(training!.trainingId);
        return () => {
            stopHubConnection();
        }
    }, [createHubConnection, stopHubConnection, training])
    return (
        <Fragment>
            <Segment
                textAlign='center'
                attached='top'
                inverted
                color='teal'
                style={{ border: 'none' }}
            >
                <Header>Chat about this course</Header>
            </Segment>
            <Segment attached>
                <Comment.Group>
                    {training && training.comments && training && training.comments.map((comment) => (
                        <Comment key={comment.trainingId}>
                            <Comment.Avatar src={comment.image || '/assets/user.png'} />
                            <Comment.Content>
                                <Comment.Author as={Link} to={`/profile/${comment.username}`}>{comment.diplayName}</Comment.Author>
                                <Comment.Metadata>
                                    <div>{formatDistance(comment.dateOfComment, new Date())}</div>
                                </Comment.Metadata>
                                <Comment.Text>{comment.body}</Comment.Text>
                            </Comment.Content>
                        </Comment>
                    ))}
                    <FinalFrom
                        onSubmit={addcomment}
                        render={({ handleSubmit, submitting, form }) => (
                            <Form onSubmit={() => handleSubmit()!.then(() => form.reset())}>
                                <Field
                                    name='body'
                                    component={InputTextArea}
                                    rows={2}
                                    placeholder='Add you comment'
                                />
                                <Button
                                    content='Add Reply'
                                    labelPosition='left'
                                    icon='edit'
                                    primary
                                    loading={submitting}
                                />
                            </Form>
                        )}
                    />
                </Comment.Group>
            </Segment>
        </Fragment>
    );
};

export default observer(TrainingDetailedChat);
