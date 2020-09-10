import React, { useContext, Fragment } from 'react';
import { Label, Item } from 'semantic-ui-react';
import { observer } from 'mobx-react-lite';
import TrainingRepository from '../../app/repositories/trainingRepository';
import TrainingItem from './trainingItem';

const TrainingsList = () => {
    const trainingStore = useContext(TrainingRepository);
    const { trainingByDate } = trainingStore;
    return (
        <Item.Group divided>
            <Fragment>
                {trainingByDate.map(([group, trainings]) => (
                    <Fragment key={group} >
                        <Label size="large" color="blue">
                            {group}
                        </Label>
                        <Item.Group>
                            {trainings.map((training) => (
                                <TrainingItem key={training.trainingId} training={training} />
                            ))}
                        </Item.Group>
                    </Fragment>
                )
                )}
            </Fragment>
        </Item.Group>
    );
}

export default observer(TrainingsList);