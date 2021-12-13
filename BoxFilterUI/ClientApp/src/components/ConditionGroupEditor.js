import React, { useState } from "react";
import ConditionEditor from "./ConditionEditor";

const modifiers = [ 'All', 'Any', 'Count' ];

const groupShortDescription = function(group) {
    let conditionCount = group.conditions.length;
    let conditions = `${conditionCount} ${conditionCount === 1 ? 'condition' : 'conditions'}`;

    let modifier = modifiers[group.modifier];
    if (group.modifier === 2) {
        if (group.countMin === group.countMax) modifier += ` ${group.countMin}`;
        else modifier += ` ${group.countMin}-${group.countMax}`;
    }

    return `${conditions}, ${modifier}`;
}

const renderTitle = function(props) {
    let group = props.group;

    const handleStyle = {
        fontSize: '0.6em',
        fontStyle: 'italic',
        display: 'inline-block',
        position: 'absolute',
        top: 0,
        left: 0,
        whiteSpace: 'nowrap',
        color: 'mediumpurple',
        width: '100%',
        textAlign: 'center',
        lineHeight: '76px',
        cursor: 'pointer'
    }

    let handleText = `Click to ${group.visible ? 'collapse' : 'expand'}`
    if (!group.visible) handleText = `${groupShortDescription(group)} - ${handleText}`;

    return (
        <div className='row'>
            <h4>Group {group.number} 
                <span onClick={_ => props.onChange({...group, visible: !group.visible})} style={handleStyle}>({handleText})</span>
            </h4>
        </div>
    );
}

export default function(props) {
    let { 
        group, 
        conditionMetadata, 
        conditionTypes 
    } = props;
    let [ hover, setHover ] = useState(false);

    const hoverStyle = {
        style: !hover ? {} : { 
            borderColor: 'mediumpurple', 
            boxShadow: 'inset 0 0 0 1px mediumpurple',
            zIndex: 1
        },
        onMouseEnter: _ => setHover(true),
        onMouseLeave: _ => setHover(false)
    }

    const addCondition = function() {
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

    if (!group.visible) return (
        <div className='card container' {...hoverStyle}>
            <div className='card-body'>
                {renderTitle(props, group.visible)}
            </div>
        </div>
    )

    return (
        <div className='card container' {...hoverStyle}>
            <div className='card-body' style={{position: 'relative'}}>
                {renderTitle(props, group.visible)}
                {!group.conditions.length && (
                    <div className='row card-body'>
                        <span style={{textAlign: 'center'}}>No conditions specified</span>
                    </div>
                )}
                {group.conditions.length > 0 && (
                    <div className='row card-body'>
                        <select defaultValue={group.modifier} className='form-control col-4' onChange={_ => props.onChange({...group, modifier: +_.target.value})}>
                            {modifiers.map((mod, index) => <option key={index} value={index}>{mod}</option>)}
                        </select>
                        {group.modifier === 2 && [
                                <input placeholder="Min" className='form-control col-4'
                                    defaultValue={group.countMin} 
                                    onBlur={event => props.onChange({...group, countMin: +event.target.value })} />,
                                <input placeholder="Max" className='form-control col-4'
                                    defaultValue={group.countMax} 
                                    onBlur={event => props.onChange({...group, countMax: +event.target.value })} />
                        ]}
                    </div>
                )}
                {group.conditions.length > 0 && (
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
                )}
                <div className='row card-body'>
                    <button className='btn btn-info col' onClick={_ => _.preventDefault() || addCondition()}>Add Condition</button>
                    <button className='btn btn-danger col' onClick={_ => _.preventDefault() || props.onRemove()}>Remove Condition Group</button>
                </div>
            </div>
        </div>
    )
}