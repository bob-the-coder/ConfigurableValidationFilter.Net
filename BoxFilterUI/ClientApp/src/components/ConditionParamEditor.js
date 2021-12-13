import React from 'react';

const UI_PARAM_SINGLE = 0, UI_PARAM_MIN_MAX = 1, UI_PARAM_CHECKBOX = 2;

const ConditionParamEditor = function(props) {
    switch (props.uiParamType) {
        case UI_PARAM_SINGLE: return <SingleEditor {...props} />;
        case UI_PARAM_MIN_MAX: return <MinMaxEditor {...props} />;
        case UI_PARAM_CHECKBOX: return <BoolEditor {...props} />;
        default: return null;
    }
}

const SingleEditor = function(props) {
    return (
        <div className='row'>
            <input placeholder="Value" className='form-control col'
                value={props.conditionParams.value} 
                onBlur={event => props.onUpdate({...props.conditionParams, value: event.target.value })} />
        </div>
    );
}

const MinMaxEditor = function(props) {
    return (
        <div className='row'>
            <input placeholder="Min" className='form-control col-6'
                value={props.conditionParams.min} 
                onBlur={event => props.onUpdate({...props.conditionParams, min: event.target.value })} />
            <input placeholder="Max" className='form-control col-6'
                value={props.conditionParams.max} 
                onBlur={event => props.onUpdate({...props.conditionParams, max: event.target.value })} />
        </div>
    );
}

const BoolEditor = function(props) {
    return (
        <div className='row'>
            <div className='checkbox'>
                <label>
                    <input type="checkbox"
                        checked={props.conditionParams.value} 
                        onClick={event => props.onUpdate({...props.conditionParams, value: !props.conditionParams.value })} />
                    {(props.conditionParams.value ? "Yes" : "No")}
                </label>
            </div>
        </div>
    );
}

export default ConditionParamEditor;