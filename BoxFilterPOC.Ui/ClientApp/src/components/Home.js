import React, { Component } from 'react';
import { NavLink } from 'react-router-dom';
import api from '../api';

export class Home extends Component {
  constructor() {
    super();
    this.state = {
      list: []
    }
  }

  componentDidMount() {
    this.loadConfigurations();
  }

  loadConfigurations() {
    let $this = this;
    api.get('api/configuration/list')
    .then(list => {
      $this.setState({
        list
      })
    });
    
  }

  renderConfiguration(configuration) {
    return (
      <tr>
        <td>
          <NavLink to={`/app/configuration-editor/${configuration}`}>{configuration}</NavLink>
        </td>
      </tr>
    );
  }

  renderConfigurations() {
    let $list = this.state.list;

    if (!$list.length) {
      return <h2>No  configurations found.</h2>
    }

    return (
      <table className="table">
        {$list.map(this.renderConfiguration)}
      </table>
    )
  }

  render() {
    return (
      <div>
        <h1>Available Configurations</h1>
        {this.renderConfigurations()}
        <NavLink to='/app/configuration-editor'>Create New</NavLink>
      </div>
    )
  }
}
