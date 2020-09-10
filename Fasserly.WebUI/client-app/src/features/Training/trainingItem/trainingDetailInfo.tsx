import React from 'react';
import { SegmentGroup, Grid, Icon, Segment } from 'semantic-ui-react';
import { ITraining } from '../../../app/models/ITraining';
import { format } from 'date-fns';

const TrainingDetailInfo: React.FC<{ training: ITraining }> = ({ training }) => {
    return (
        <SegmentGroup>
            <Segment attached='top'>
                <Grid>
                    <Grid.Column width={1}>
                        <Icon size="large" color="teal" name="info" />
                    </Grid.Column>
                    <Grid.Column width={15}>
                        <p>{training.description}</p>
                    </Grid.Column>
                </Grid>
            </Segment>
            <Segment>
                <Grid verticalAlign="middle">
                    <Grid.Column width={1}>
                        <Icon size="large" color="teal" name="calendar" />
                    </Grid.Column>
                    <Grid.Column width={15}>
                        <span>{format(training.dateOfCreation, 'do MMMM YYYY')} at {format(training.dateOfCreation, 'h:mm a')}</span>
                    </Grid.Column>
                </Grid>
            </Segment>
            <Segment>
                <Grid verticalAlign="middle">
                    <Grid.Column width={1}>
                        <Icon size="large" color="teal" name="euro sign" />
                    </Grid.Column>
                    <Grid.Column width={15}>
                        <span>{training.price}</span>
                    </Grid.Column>
                </Grid>
            </Segment>
        </SegmentGroup>
    );
}

export default TrainingDetailInfo;