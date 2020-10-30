﻿import React from 'react';
import { FieldRenderProps } from 'react-final-form'
import { FormFieldProps, Form, Label } from 'semantic-ui-react';
import { DateTimePicker } from 'react-widgets';

interface IProps extends FieldRenderProps<Date, HTMLElement>, FormFieldProps { }

const InputDate: React.FC<IProps> = ({ id = null, input, width, placeholder, date = false, meta: touched, error, ...rest }) => {
    return (
        <Form.Field error={touched && !!error} width={width} >
            <DateTimePicker
                placeholder={placeholder}
                value={input.value || null}
                date={date}
                onChange={input.onChange}
                onBlur={input.onBlur}
                onKeyDown={(e) => e.preventDefault()}
            />
            {touched && error && (
                <Label basic color='red'>
                    {error}
                </Label>
            )}
        </Form.Field>
    );
}

export default InputDate;