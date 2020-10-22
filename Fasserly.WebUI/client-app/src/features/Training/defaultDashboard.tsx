import React, { useContext, useEffect, useState } from 'react';
import { Grid, GridColumn, Loader } from 'semantic-ui-react';
import TrainingsList from './trainingList';
import { observer } from 'mobx-react-lite';
import { BaseRepositoryContext } from '../../app/repositories/baseRepository';
import TrainingFilters from '../Training/dashboard/TrainingFilters'
import TrainingItemPlaceholder from '../Training/dashboard/TrainingItemPlaceholder'
import InfiniteScroll from 'react-infinite-scroller';

const TrainingDashboard: React.FC = () => {

    const baseRepository = useContext(BaseRepositoryContext);
    const { loadTrainings, loading, setPage, page, totalPages } = baseRepository.trainingsRepository;
    const token = window.localStorage.getItem('jwt');
    const { isLoggedIn } = baseRepository.userRepository;
    const [loadingNext, setLoadingNext] = useState(false);

    const handleGetNext = () => {
        setLoadingNext(true);
        setPage(page + 1);
        loadTrainings().then(() => setLoadingNext(false));
    }
    useEffect(() => {
            loadTrainings();
    }, [loadTrainings]);

    return (
        <Grid>
            <GridColumn width={10}>
                {loading && page === 0 ? <TrainingItemPlaceholder /> : (
                    <InfiniteScroll
                        pageStart={0}
                        loadMore={handleGetNext}
                        hasMore={!loadingNext && page + 1 < totalPages}
                        initialLoad={false}
                    >
                        <TrainingsList />
                    </InfiniteScroll>
                )}
            </GridColumn>
            {isLoggedIn && token &&
                <Grid.Column width={6}>
                    <TrainingFilters />
                </Grid.Column>
            }
            <Grid.Column width={10}>
                <Loader active={loadingNext} />
            </Grid.Column>

        </Grid>
    );
}

export default observer(TrainingDashboard);