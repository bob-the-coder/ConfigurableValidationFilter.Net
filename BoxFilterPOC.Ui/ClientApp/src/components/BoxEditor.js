import React from 'react';

export default function(props) {
    let box = props.box;

    let area = box.Width * box.Depth;
    let volume = (box.Height / 10) * (box.Width / 10) * (box.Depth / 10);
    let density = box.Weight / (box.Height / 100 * box.Width / 100  * box.Depth / 100);

    const handleUpdate = function(propName, value) {
        let newBox = props.box;
        newBox[propName] = value;
        props.onUpdate(newBox);
    }

    const inputFor = function(propName) {
        return (
            <input className='form-control'
                defaultValue={props.box[propName]} 
                onBlur={event => handleUpdate(propName, event.target.value)} />
        );
    }

    return (
        <div>
            <h2>Design a Box</h2>
            <div>
                <table className='table'>
                    <thead>
                        <tr>
                            <th>Property</th>
                            <th>Value</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <th>Height (cm)</th>
                            <td>
                                {inputFor('Height')}
                            </td>
                        </tr>
                        <tr>
                            <th>Width (cm)</th>
                            <td>
                                {inputFor('Width')}
                            </td>
                        </tr>
                        <tr>
                            <th>Depth (cm)</th>
                            <td>
                                {inputFor('Depth')}
                            </td>
                        </tr>
                        <tr>
                            <th>Weight (kg)</th>
                            <td>
                                {inputFor('Weight')}
                            </td>
                        </tr>
                        <tr>
                            <th>Color</th>
                            <td>
                                {inputFor('Color')}
                            </td>
                        </tr>
                        <tr>
                            <th>Received On</th>
                            <td>
                                {inputFor('ReceivedOn')}
                            </td>
                        </tr>
                        <tr>
                            <th colspan='2' style={{textAlign: 'center'}}>Computed</th>
                        </tr>
                        <tr>
                            <th>Area</th>
                            <td>{area} cm^2</td>
                        </tr>
                        <tr>
                            <th>Volume</th>
                            <td>{volume} liters</td>
                        </tr>
                        <tr>
                            <th>Density</th>
                            <td>{density} kg/m^3</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    )
}