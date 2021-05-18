import './App.css';
import {
  Router,
  Switch} from 'react-router-dom';
import React from 'react';
import 'antd/dist/antd.css';
import MainLayout from './components/main-layout/MainLayout';
import history from './router/history';

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
