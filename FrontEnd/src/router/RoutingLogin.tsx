import React from "react";
import { Router, Route, Link } from "react-router-dom";

import { history } from "../helpers/history";
import { Role } from "../helpers/role";
import { authenticationService } from "../services/authentication.service";
import { PrivateRoute } from "../components/PrivateRoute";
import { HomePage } from "../pages/HomePage";
import { AdminPage } from "../pages/AdminPage";
import { UserPage } from "../pages/UserPage";
import { LoginPage } from "../pages/LoginPage";

export default class RoutingLogin extends React.Component<
  {},
  { currentUser: any; isAdmin: boolean; isUser: boolean }
> {
  constructor(props: any) {
    super(props);

    this.state = {
      currentUser: null,
      isAdmin: false,
      isUser: false,
    };
  }

  componentDidMount() {
    authenticationService.currentUser.subscribe((x: { type: Role; }) =>
      this.setState({
        currentUser: x,
        isAdmin: x && x.type === Role.Admin,
        isUser: x && x.type === Role.User,
      })
    );
  }

  logout() {
    authenticationService.logout();
    history.push("/login");
  }

  render() {
    const { currentUser, isAdmin, isUser } = this.state;
    return (
      <Router history={history}>
        <div>
          {currentUser && (
            <nav className="navbar navbar-expand navbar-dark bg-dark">
              <div className="navbar-nav">
                <Link to="/" className="nav-item nav-link">
                  Home
                </Link>
                {isAdmin && (
                  <Link to="/admin" className="nav-item nav-link">
                    Admin
                  </Link>
                )}
                {isUser && (
                  <Link to="/user" className="nav-item nav-link">
                    User
                  </Link>
                )}
                <Link
                  to="#"
                  onClick={this.logout}
                  className="nav-item nav-link"
                >
                  Logout
                </Link>
              </div>
            </nav>
          )}
          <div className="jumbotron">
            <div className="container">
              <div className="row">
                <div className="col-md-6 offset-md-3">
                  <PrivateRoute
                    exact
                    path="/"
                    roles={[Role.Admin] || [Role.User]}
                    component={HomePage}
                  />
                  <PrivateRoute
                    path="/admin"
                    roles={[Role.Admin]}
                    component={AdminPage}
                  />
                  <PrivateRoute
                    path="/user"
                    roles={[Role.User]}
                    component={UserPage}
                  />
                  <Route path="/login" component={LoginPage} />
                </div>
              </div>
            </div>
          </div>
        </div>
      </Router>
    );
  }
}
