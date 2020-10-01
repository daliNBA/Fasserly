import React, { useContext } from 'react';
import { Modal } from 'semantic-ui-react'
import { BaseRepositoryContext } from '../../repositories/baseRepository';
import { observer } from 'mobx-react-lite';

const ModalContainer = () => {
    const baseRepo = useContext(BaseRepositoryContext);
    const { closeModal, modal: { open, body } } = baseRepo.modalRepository;
    return (
        <Modal open={open} onClose={closeModal} size='mini'>
            <Modal.Content>{body}</Modal.Content>
        </Modal>
    );
}

export default observer(ModalContainer);