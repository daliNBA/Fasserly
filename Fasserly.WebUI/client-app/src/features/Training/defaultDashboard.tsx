import React, { useContext, useEffect } from 'react';
import { Grid, GridColumn } from 'semantic-ui-react';
import TrainingsList from './trainingList';
import { observer } from 'mobx-react-lite';
import Loading from '../../app/layout/Loading';
import { BaseRepositoryContext } from '../../app/repositories/baseRepository';

const TrainingDashboard: React.FC = () => {

    const baseRepository = useContext(BaseRepositoryContext);
    const { trainingList, loading } = baseRepository.trainingsRepository;

    useEffect(() => {
        trainingList();
    }, [trainingList]);

    if (loading) return <Loading content={"Loading.."} />

    return (
        <Grid>
            <GridColumn width={10}>
                <TrainingsList />
            </GridColumn>
            <Grid.Column width={6}>
                <h2>Activity filters</h2>
            </Grid.Column>
        </Grid>
    );
}

export default observer(TrainingDashboard);