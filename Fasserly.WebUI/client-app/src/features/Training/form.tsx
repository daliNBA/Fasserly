import React, { useState, useContext, useEffect } from 'react';
import { TrainingFormValues } from '../../app/models/ITraining';
import { Form, Button, Segment, Grid, GridColumn } from 'semantic-ui-react';
import { v4 as uuid } from 'uuid';
import { observer } from 'mobx-react-lite';
import { RouteComponentProps } from 'react-router-dom';
import { Form as FinalFrom, Field } from 'react-final-form'
import TextInput from '../../app/common/form/inputText';
import InputTextArea from '../../app/common/form/inputTextArea';
import { ITList } from '../../app/common/options/ITOptions';
import InputSelect from '../../app/common/form/inputSelect';
import InputDate from '../../app/common/form/inputDate';
import { combineTimeAndDate } from '../../app/common/util/util';
import { combineValidators, isRequired, composeValidators, hasLengthGreaterThan } from 'revalidate'
import { BaseRepositoryContext } from '../../app/repositories/baseRepository';

const validate = combineValidators({
    title: isRequired({ message: 'title est obligatoir' }),
    category: isRequired({ message: 'category est obligatoir' }),
    description: composeValidators(
        isRequired('description '),
        hasLengthGreaterThan(10)({ message: 'au moin 10 charactéres ' })
    )(),
    price: isRequired({ message: 'price est obligatoir' }),

})

interface IDetailParams {
    id: string;
}
const TrainingEdit: React.FC<RouteComponentProps<IDetailParams>> = ({ match, history }) => {

    const baseRepository = useContext(BaseRepositoryContext);
    const {
            submitting,
            loadTraining,
            createTraining,
            editTraining 
          } = baseRepository.trainingsRepository;

    const [training, setTraining] = useState(new TrainingFormValues());
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        if (match.params.id) {
            setLoading(true);
            loadTraining(match.params.id).then(
                (training) => setTraining(new TrainingFormValues(training))
            ).finally(() => setLoading(false));
        }
    }, [loadTraining, match.params.id]);

    const handleFinalFormSubmit = (values: any) => {
        console.log(values);
        const dateAndTime = combineTimeAndDate(values.dateOfCreation);
        const { date, time, ...training } = values;
        training.dateOfCreation = dateAndTime;
        if (!training.trainingId) {
            let newTraining = {
                ...training,
                trainingId: uuid()
            };
            console.log(training)
            createTraining(newTraining);
        } else {
            editTraining(training);
        }
    }

    return (
        <Grid>
            <GridColumn width={10}>
                <Segment clearing  >
                    <FinalFrom
                        validate={validate} //validator
                        initialValues={training}
                        onSubmit={handleFinalFormSubmit}
                        render={({ handleSubmit, pristine }) =>
                            (
                                <Form onSubmit={handleSubmit} loading={loading}>
                                    <Field component={TextInput} placeholder='Title' name='title' value={training.title} />
                                    <Field component={InputTextArea} rows={2} placeholder='Description' name='description' value={training?.description} />
                                    <Field placeholder='Date' component={InputDate} date={true} name='dateOfCreation' value={training.dateOfCreation} />
                                    <Field component={TextInput} placeholder='Price' name='price' value={training.price} />
                                    <Field component={InputSelect} options={ITList} name='category' value={training?.category} />
                                    <Button.Group floated="right">
                                        <Button disabled={loading} onClick={training.trainingId ? () => history.push(`/detailTraining/${training.trainingId}`) : () => history.push('/trainings')} floated='right' type='button'>Cancel</Button>
                                        <Button.Or />
                                        <Button positive disabled={loading || pristine} loading={submitting} floated='right' type='submit'>Submit</Button>
                                    </Button.Group>
                                </Form>
                            )}
                    />
                </Segment>
            </GridColumn>
        </Grid>
    );
}

export default observer(TrainingEdit);