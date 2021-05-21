import React from "react";
import { Route, Redirect } from "react-router-dom";
import { authenticationService } from "../services/authentication.service";

export const PrivateRoute = ({ component: Component, roles, ...rest }) => (
  <Route
    {...rest}
    render={(props) => {
      const currentUser = authenticationService.currentUserValue;
      if (!currentUser || localStorage.getItem("currentUser") == null) {
        return <Redirect to={{ pathname: "/login" }} />;
      }

      // check if route is restricted by role
      if (roles && roles.indexOf(currentUser.type) === -1) {
        // role not authorized so redirect to home page
        return <Redirect to={{ pathname: "/" }} />;
      }

      return <Component {...props} />;
    }}
  />
);
