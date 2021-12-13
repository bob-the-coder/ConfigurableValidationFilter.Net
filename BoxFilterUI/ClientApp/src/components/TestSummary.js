import React from 'react';

export default function(props) {
    return (
        <div>
            <h2>Compare</h2>
            <br />
            <b>Result: {props.result.length === 0 ? 'Passed' : 'Failed'}</b>
            <br />
            <b>Summary:</b>
            <br />
            <code style={{whiteSpace: "pre-wrap"}}>
                {props.result.map(_ => <div>{_}</div>)}
            </code>
        </div>
    )
}