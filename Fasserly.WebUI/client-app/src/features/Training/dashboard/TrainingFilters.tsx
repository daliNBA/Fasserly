import React, { Fragment, useContext } from 'react';
import { Menu, Header } from 'semantic-ui-react';
import { Calendar } from 'react-widgets';
import { BaseRepositoryContext } from '../../../app/repositories/baseRepository';
import { observer } from 'mobx-react-lite';

const TrainingFilters = () => {
    const baseRepo = useContext(BaseRepositoryContext);
    const { setPredicate, predicate } = baseRepo.trainingsRepository;
    return (
        <Fragment>
            <Menu vertical size={'large'} style={{ width: '100%', marginTop: 50 }}>
                <Header icon={'filter'} attached color={'teal'} content={'Filters'} />
                <Menu.Item
                    active={predicate.size === 0}
                    onClick={() => setPredicate('all', 'true')}
                    color={'blue'}
                    name={'all'}
                    content={'All Courses'}
                />
                <Menu.Item
                    active={predicate.has('isBuyer')}
                    onClick={() => setPredicate('isBuyer', 'true')}
                    color={'blue'}
                    name={'username'}
                    content={"My courses"}
                />
                <Menu.Item
                    active={predicate.has('isOwner')}
                    onClick={() => setPredicate('isOwner', 'true')}
                    color={'blue'}
                    name={'host'}
                    content={"I'm owner"}
                />
            </Menu>
            <Header
                icon={'calendar'}
                attached
                color={'teal'}
                content={'Select Date'}
            />
            <Calendar
                onChange={date => setPredicate('startDate', date!)}
                value={predicate.get('startDate') || new Date()}
            />
        </Fragment>
    );
};

export default observer(TrainingFilters);