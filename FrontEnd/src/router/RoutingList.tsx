import React from 'react';
import { Route } from 'react-router-dom';
import { CreateAssignment } from '../pages/assignment-component/create';
import { ListAssignments } from '../pages/assignment-component/list';
import { ListAssignmentsForEachUser } from '../pages/assignment-component/list-by-user';
import { UpdateAssignment } from '../pages/assignment-component/update';
import { CreateAsset } from '../pages/assets-component/create';
import { ListAssets } from '../pages/assets-component/list';
import { UpdateAsset } from '../pages/assets-component/update';
import { AccessDenied } from '../pages/error-component/AccessDenied';
import { NotFound } from '../pages/error-component/NotFound';
import { Home } from '../pages/home-component/home';
import { Login } from '../pages/login-component/login';
import { ListReturnRequests } from '../pages/return-request-component/list';
import { ReportView } from '../pages/reports-component/report.view';
import { CreateUser } from '../pages/users-component/create';
import { ListUsers } from '../pages/users-component/list';
import { UpdateUser } from '../pages/users-component/update';

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
    path: '/home',
    component: Home,
    key: '/home',
  },
  {
    path: '/users',
    component: ListUsers,
    key: '/users'
  },
  {
    path: '/users/update/:userId',
    component: UpdateUser,
    key: '/users/update/:userId',
  },
  {
    path: '/users/create',
    component: CreateUser,
    key: '/users/create',
  },
  {
    path: '/assignments',
    component: ListAssignments,
    key: '/assignments',
  },
  {
    path: '/home',
    component: ListAssignmentsForEachUser,
    key: '/home',
  },
  {
    path: '/assignments/create',
    component: CreateAssignment,
    key: '/assignments/create',
  },
  {
    path: '/assignments/update/:assignmentId',
    component: UpdateAssignment,
    key: '/assignments/update/:assignmentId',
  },
  {
    path: '/return-requests',
    component: ListReturnRequests,
    key: '/return-requests',
  },
  {
    path: '/assets/create',
    component: CreateAsset,
    key: '/assets/create',
  },
  {
    path: '/assets',
    component: ListAssets,
    key: '/assets',
  },
  {
    path: '/assets/update/:assetId',
    component: UpdateAsset,
    key: '/assets/update/:assetId',
  },
  {
    path: "/reports",
    component: ReportView,
    key: "/reports",
  },
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
