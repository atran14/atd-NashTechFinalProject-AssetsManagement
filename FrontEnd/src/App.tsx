import './App.css'
import { Router, Switch } from 'react-router-dom'
import React from 'react'
import 'antd/dist/antd.css'
import history from './router/history'
import MainLayout from './components/main-layout/MainLayout'
import RoutingLogin from './router/RoutingLogin'

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
      {/* <RoutingLogin /> */}
    </div>
  )
}

export default App
