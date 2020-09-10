import React from 'react';
import { observer } from 'mobx-react-lite';
import { Dropdown } from 'semantic-ui-react';
import { ITList } from '../../../app/common/options/ITOptions';

const IT_Menu: React.FC = () => {
    return (
        <Dropdown Item simple text='Categories' direction='right' options={ITList}></Dropdown>
    );
}

export default observer(IT_Menu);
