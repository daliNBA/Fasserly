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
            <GridColumn width={12}>
                <TrainingsList />
            </GridColumn>
        </Grid>
    );
}

export default observer(TrainingDashboard);