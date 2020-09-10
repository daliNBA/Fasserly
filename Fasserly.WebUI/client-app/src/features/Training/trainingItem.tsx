import React from 'react';
import { Grid, GridColumn } from 'semantic-ui-react';
import { ITraining } from '../../app/models/ITraining';
import TrainingDetailHeader from './trainingItem/trainingDetailHeader';
import TrainingDetailInfo from './trainingItem/trainingDetailInfo';
import TrainingDetailSideBar from './trainingItem/trainingDetailSideBar';

const TrainingItem: React.FC<{ training: ITraining }> = ({ training }) => {

    return (
        <Grid>
            <GridColumn width={10}>
                <TrainingDetailHeader training={training} />
                <TrainingDetailInfo training={training} />
            </GridColumn>
            <GridColumn width={6}>
                <TrainingDetailSideBar />
            </GridColumn>
        </Grid>
    );
}

export default TrainingItem;