import React from "react";
import { Redirect } from "react-router";
import api from '../api';
import BoxEditor from "./BoxEditor";
import ConditionEditor from "./ConditionEditor";
import TestSummary from "./TestSummary";

export default class ConfigurationEditor extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            name: 'New Configuration',
            conditions: [],
            saved: false,
            metadata: null,
            conditionTypes: [],
            testBox: {
                Height: 100,
                Width: 100,
                Depth: 100,
                Weight: 997,
                Color: "Brown",
                ReceivedOn: "2021-01-01"
            },
            testResult: ['Test has not run yet.']
        };
    }

    loadConfiguration(){
        let $this = this;
        let configurationName = $this.props.match.params.configuration_name;
        
        if (!configurationName) return;

        api.get(`api/configuration/${configurationName}`).then(config => {
            if (!config) return;
            
            $this.setState({
                ...$this.state,
                name: config.Name,
                conditions: config.Conditions.map(({Type, Params}) => {
                    return {type: +Type, params: {
                        value: Params.Value,
                        min: Params.Min,
                        max: Params.Max,
                        error: Params.Error,
                        paramType: Params.ParamType
                    }};
                }),
                saved: false
            })
        })
    }

    loadConditionMetadata(){
        let $this = this;
        api.get(`api/configuration/metadata`).then(metadataList => {
            let conditionTypes = [];
            let metadata = {};
            for (let index in metadataList) {
                let metadataItem = metadataList[index];
                conditionTypes.push(+metadataItem.Item1);
                metadata[+metadataItem.Item1] = metadataItem.Item2;
            }
            $this.setState({
                ...$this.state,
                metadata,
                conditionTypes
            })
        })
    }

    componentDidMount(){
        this.loadConfiguration();
        this.loadConditionMetadata();
    }

    getConfigForServer(){
        let config = this.state;
        
        return {
            Name: config.name,
            Conditions: config.conditions.map(condition => {
                let { type, params } = condition;
                let value = params.value;
                if (typeof value === "string" &&
                    value.indexOf("," >= 0) &&
                    config.metadata[type].ParamType.indexOf("List") >= 0) value = value.split(",").map(_ => _.trim());

                return { 
                    Type: type, 
                    Params: {
                    __param_type__: this.state.metadata[type].ParamType,
                    Value: value,
                    Min: params.min,
                    Max: params.max,
                    Error: params.error
                }}
            })
        }
    }

    save(event){
        event.preventDefault();

        let $this = this;

        api.post('api/configuration', JSON.stringify(this.getConfigForServer()))
        .then(() => {
            $this.setState({...$this.state, saved: true})
        })
        return false;
    }

    testBox() {
        let $this = this;
        let $state = $this.state;

        if (!$state.conditions.length) {
            $this.setState({
                ...$state,
                testResult: ['You must specify at least one condition.']
            })
            return;
        }
        let testPayload = {
            box: $state.testBox,
            configuration: $this.getConfigForServer()   
        }

        api.post('api/configuration/test', JSON.stringify(testPayload))
        .then(result => {
            $this.setState({
                ...$this.state,
                testResult: result.map(_ => _.error)
            })
        })
    }

    handleTestBoxChange(newTestBox){
        let $this = this;
        $this.setState({
            ...$this.state,
            testBox: newTestBox
        }, $this.testBox);
    }
    
    handleConditionChange(index, newCondition) {
        let $this = this;
        let $state = this.state;
        let conditions = $state.conditions;

        conditions[index] = newCondition;

        $this.setState({
            ...$state,
            conditions
        }, $this.testBox)
    }

    addCondition(){
        let $this = this;
        let conditions = this.state.conditions;
        conditions.push({ type: this.state.conditionTypes[0], params: {}})

        $this.setState({
            ...this.state,
            conditions
        }, $this.testBox)
    }

    removeCondition(index){
        let $this = this;
        let conditions = this.state.conditions;
        conditions.splice(index, 1);

        $this.setState({
            ...this.state,
            conditions
        }, $this.testBox)
    }

    render() {
        let $state = this.state;
        if ($state.saved) return <Redirect to='/' />;
        if (!$state.metadata || !$state.conditionTypes.length) return <h4>Loading</h4>
        return (
            <div className="container">
                <div className="row">
                    <div className="col-7 card card-body">
                        <h2>Edit Configuration</h2>
                        <form onSubmit={_ => this.save(_)}>
                            <div className='form-group'>
                                <label>Name:</label>
                                <input name="name" 
                                    className='form-control'
                                    placeholder="Configuration Name" 
                                    value={$state.name} 
                                    onBlur={event => this.setState({
                                        ...this.state,
                                        name: event.target.value
                                    })} />
                            </div>
                            {$state.conditions.map((condition, index) => 
                                <ConditionEditor 
                                    key={index}
                                    condition={condition}
                                    conditionMetadata={$state.metadata}
                                    conditionTypes={$state.conditionTypes}
                                    onChange={newCondition => this.handleConditionChange(index, newCondition)}
                                    onRemove={() => this.removeCondition(index)}
                                />)}
                            <button className='btn btn-info' onClick={_ => _.preventDefault() || this.addCondition()}>Add Condition</button>
                            <input type="submit" className='btn btn-success' value="Save" />
                        </form>
                    </div>
                    <div className="col">
                        <div className='card card-body'>
                            <BoxEditor box={$state.testBox} onUpdate={newBox => this.handleTestBoxChange(newBox)} />
                        </div>
                        <div className='card card-body'>
                            <TestSummary result={$state.testResult} />
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}