import React from 'react';

export default function(props) {
    let box = props.box;

    let area = box.Width * box.Depth;
    let volume = (box.Height / 10) * (box.Width / 10) * (box.Depth / 10);
    let density = box.Weight / (box.Height / 100 * box.Width / 100  * box.Depth / 100);

    return (
        <div>
            <h2>Test Box</h2>
            <div>
                <input placeholder="Height (cm)" 
                    defaultValue={props.box.Height} 
                    onBlur={event => props.onUpdate({...props.box, Height: event.target.value })} />
                <br />
                <input placeholder="Width (cm)" 
                    defaultValue={props.box.Width} 
                    onBlur={event => props.onUpdate({...props.box, Width: event.target.value })} />
                <br />
                <input placeholder="Depth (cm)" 
                    defaultValue={props.box.Depth} 
                    onBlur={event => props.onUpdate({...props.box, Depth: event.target.value })} />
                <br />
                <input placeholder="Weight (kg)" 
                    defaultValue={props.box.Weight} 
                    onBlur={event => props.onUpdate({...props.box, Weight: event.target.value })} />
                <br />
                <input placeholder="Color" 
                    defaultValue={props.box.Color} 
                    onBlur={event => props.onUpdate({...props.box, Color: event.target.value })} />
                <br />
                <input placeholder="Received On" 
                    defaultValue={props.box.ReceivedOn} 
                    onBlur={event => props.onUpdate({...props.box, ReceivedOn: event.target.value })} />
                <h4>Computed Values</h4>
                <b>Area (cm^2): </b>{area}
                <br />
                <b>Volume (l): </b>{volume}
                <br />
                <b>Density (kg/m^3): </b>{density}
            </div>
        </div>
    )
}