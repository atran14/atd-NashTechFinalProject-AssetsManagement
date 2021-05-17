import React from 'react';
import { Route } from 'react-router-dom';
import {  UpdateUser } from '../pages/user-component/update/update';

const routes = [
    {
        path: '/user/update/:userId',
        component: UpdateUser,
        key: '/update/user/:userId',
      }

  ];

  export function RoutingList(): JSX.Element {
    return <>
      {
        routes.map(item => {
          if (item.path.split('/').length === 2) {
            return (
              <Route
                exact
                path={item.path}
                component={item.component}
                key={item.key}
              />
            );
          }
          return <Route path={item.path} component={item.component} key={item.key} />;
        })
      }
    </>
  }