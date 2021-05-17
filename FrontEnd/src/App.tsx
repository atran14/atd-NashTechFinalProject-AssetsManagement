import React from 'react';
import logo from './logo.svg';
import './App.css';
import 'antd/dist/antd.css';
import { Router, Switch } from 'react-router-dom';
import history from './router/history';
import MainLayout from './component/main-layout/MainLayout';

function App() {
  return (
    <div className="App">
    <Router history={history}>
      <div>
        <Switch>
          <MainLayout />
        </Switch>
      </div>
    </Router>
</div>
  );
}

export default App;
