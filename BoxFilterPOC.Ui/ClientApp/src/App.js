import React, { Component } from 'react';
import { Redirect, Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import ConfigurationEditor from './components/ConfigurationEditor';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={() => <Redirect to='app/configurations' />} />
        <Route exact path='/app/configurations' component={Home} />
        <Route path='/app/configuration-editor/:configuration_name?' component={ConfigurationEditor} />
      </Layout>
    );
  }
}
