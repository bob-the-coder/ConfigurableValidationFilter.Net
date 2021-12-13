import React, { useState } from "react";
import ConditionEditor from "./ConditionEditor";

const modifiers = [ 'All', 'Any', 'Count' ];

export default function(props) {
    let { 
        group, 
        conditionMetadata, 
        conditionTypes 
    } = props;

    const [ visible, setVisible ] = useState(true);

    const addCondition = function (){
        group.conditions.push({ type: conditionTypes[0], params: {}});
        props.onChange(group);
    }

    const handleConditionChange = function(index, newCondition) {
        group.conditions[index] = newCondition;
        props.onChange(group);
    }

    const removeCondition = function(index) {
        group.conditions.splice(index, 1);
        props.onChange(group);
    }

    if (!visible) return (
        <div class='card container'>
            <div className='card-body'>
                <div className='row'>
                    <h4 onClick={_ => setVisible(true)}>Group {group.number}</h4>
                </div>
            </div>
        </div>
    )

    return (
        <div class='card container'>
            <div className='card-body'>
                <div className='row'>
                    <h4 onClick={_ => setVisible(false)}>Group {group.number}</h4>
                </div>
                <div className='row'>
                    <div className='col-3'>
                        <select defaultValue={group.modifier} className='form-control' onChange={_ => props.onChange({...group, modifier: +_.target.value})}>
                            {modifiers.map((mod, index) => <option key={index} value={index}>{mod}</option>)}
                        </select>
                    </div>
                    {group.modifier === 2 && [
                        <div className='col-3'>
                            <input placeholder="Min" className='form-control col'
                                defaultValue={group.countMin} 
                                onBlur={event => props.onChange({...group, countMin: +event.target.value })} />
                        </div>,
                        <div className='col-3'>
                            <input placeholder="Max" className='form-control col'
                                defaultValue={group.countMax} 
                                onBlur={event => props.onChange({...group, countMax: +event.target.value })} />
                        </div>
                    ]}
                </div>
                <div className='row'>
                    {group.conditions.map((condition, index) => <ConditionEditor 
                        key={index}
                        condition={condition}
                        conditionMetadata={conditionMetadata}
                        conditionTypes={conditionTypes}
                        onChange={newCondition => handleConditionChange(index, newCondition)}
                        onRemove={() => removeCondition(index)}
                    />)}
                </div>
                <div className='row'>
                    <button className='btn btn-info' onClick={_ => _.preventDefault() || addCondition()}>Add Condition</button>
                    <button className='btn btn-danger' onClick={_ => _.preventDefault() || props.onRemove()}>Remove Group</button>
                </div>
            </div>
        </div>
    )
}