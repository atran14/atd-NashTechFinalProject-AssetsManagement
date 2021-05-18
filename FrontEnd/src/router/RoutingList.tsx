import React from 'react';
import { Route } from 'react-router-dom';
import { AccessDenied } from '../pages/error-component/AccessDenied';
import { NotFound } from '../pages/error-component/NotFound';
import { Login } from '../pages/login-component/login';
import { Register } from '../pages/register-component/register';
import { UpdateUser } from '../pages/user-component/update/update';

const routes = [
  {
    path: '/400-not-found',
    component: NotFound,
    key: '/400-not-found',
  },
  {
    path: '/401-access-denied',
    component: AccessDenied,
    key: '/401-access-denied',
  },
  {
    path: '/login',
    component: Login,
    key: '/login',
  },
  {
    path: '/register',
    component: Register,
    key: '/register',
  },
  {
    path: '/home',
    component: Login,
    key: '/home',
  },
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
