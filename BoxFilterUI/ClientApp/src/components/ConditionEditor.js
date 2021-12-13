import React from 'react';
import ConditionParamEditor from './ConditionParamEditor';

export default function(props) {
    let { 
        condition, 
        conditionMetadata, 
        conditionTypes 
    } = props;
        
    return (
        <div className="card container">
            <div className='card-body'>
                <div className='row'>
                    <div className='col-6'>
                        <div className='row'>
                            <select className='form-control'
                                defaultValue={condition.type} 
                                onChange={event => props.onChange({type: event.target.value, params: {}})}>
                                {conditionTypes.map(function(conditionType){
                                    return <option value={conditionType}>{conditionMetadata[conditionType].Name}</option>
                                })}
                            </select>
                        </div>
                    </div>
                    <div className='col-6'>
                        <ConditionParamEditor conditionParams={condition.params || {}} 
                            onUpdate={newConditionParams => props.onChange({...condition, params: newConditionParams})}
                            uiParamType={conditionMetadata[condition.type].UiParamType} />
                    </div>
                </div>
                <div className='row'>
                    <textarea placeholder="Error" className='form-control col'
                        style={{width: '100%'}}
                        defaultValue={condition.params.error} 
                        onBlur={event => props.onChange({...condition, params: {...condition.params, error: event.target.value }})}></textarea>
                </div>
                <div className='row'>
                    <b>ParamType: </b>{conditionMetadata[condition.type].ParamType}
                    <div>{conditionMetadata[condition.type].Description}</div>
                    <button className='btn btn-danger' onClick={props.onRemove}>Remove Condition</button>
                </div>
            </div>
        </div>
    )
}