import React, { useContext, useEffect } from 'react';
import { Grid, GridColumn } from 'semantic-ui-react';
import TrainingsList from './trainingList';
import { observer } from 'mobx-react-lite';
import Loading from '../../app/layout/Loading';
import TrainingRepository from '../../app/repositories/trainingRepository'

const TrainingDashboard: React.FC = () => {

    const trainingStore = useContext(TrainingRepository);
    useEffect(() => {
        trainingStore.trainingList();
    }, [trainingStore]);

    if (trainingStore.loading) return <Loading content={"Loading.."} />

    return (
        <Grid>
            <GridColumn width={12}>
                <TrainingsList />
            </GridColumn>
        </Grid>
    );
}

export default observer(TrainingDashboard);