import React, { useState } from 'react';

const defaultResult = {
    success: false,
    error: 'Test has not been run yet.',
    metadata: {
        elapsed: 'N/A'
    }
}

const renderResult = function(result, seed, index) {
    let errors = [<code key={seed + index} style={{whiteSpace: "pre-wrap", display: 'block'}}>{result.error}</code>];
    if (!result.internal || !result.internal.length) return errors;

    return errors.concat(result.internal.map((_, _i) => renderResult(_, seed + index, _i)));
}

export default function(props) {
    const [showDetails, setShowDetails] = useState(true);
    let result = props.result || defaultResult;

    return (
        <div>
            <h2>Test the Box</h2>
            <br />
            <b>Test Summary: {result.success ? 'Passed' : 'Failed'}</b>
            <br/>
            {result.metaData && 
                <code style={{whiteSpace: "pre-wrap", display: 'block'}}>Elapsed: {result.metaData.elapsed}</code>
            }
            <i onClick={_ => setShowDetails(!showDetails)}>{showDetails ? 'Hide' : 'Show'} Details</i>
            {showDetails && (<div>
                {renderResult(result, 0, 0)}
            </div>)}
        </div>
    )
}