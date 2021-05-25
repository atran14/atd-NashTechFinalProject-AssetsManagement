import React from "react";
import { Router, Route, Link } from "react-router-dom";
import { Nav, NavDropdown } from "react-bootstrap";
import { Modal, Button } from "react-bootstrap";
import { ErrorMessage, Field, Form, Formik } from "formik";
import * as Yup from "yup";

import { history } from "../helpers/history";
import { Role } from "../helpers/role";
import { authenticationService } from "../services/authentication.service";
import { PrivateRoute } from "../components/PrivateRoute";
import { HomePage } from "../pages/HomePage";
import { AdminPage } from "../pages/AdminPage";
import { UserPage } from "../pages/UserPage";
import { LoginPage } from "../pages/LoginPage";
import { userService } from "../services/user.service";

export default class RoutingLogin extends React.Component<
  {},
  {
    showModal: boolean;
    currentUser: any;
    isAdmin: boolean;
    isUser: boolean;
    isPasswordChanged: boolean;
  }
> {
  constructor(props: any) {
    super(props);

    this.state = {
      showModal: false,
      currentUser: null,
      isAdmin: false,
      isUser: false,
      isPasswordChanged: false,
    };
    this.handleClose = this.handleClose.bind(this);
    this.handleShow = this.handleShow.bind(this);
    this.handlePasswordChange = this.handlePasswordChange.bind(this);
  }

  componentDidMount() {
    authenticationService.currentUser.subscribe((x: { type: Role }) =>
      this.setState({
        currentUser: x,
        isAdmin: x && x.type === Role.Admin,
        isUser: x && x.type === Role.User,
      })
    );
  }
  handleClose() {
    this.setState(() => ({ showModal: false }));
    this.setState(() => ({ isPasswordChanged: false }));
    this.logout();
  }
  handleShow() {
    this.setState(() => ({ showModal: true }));
  }
  handlePasswordChange() {
    this.setState(() => ({ isPasswordChanged: true }));
  }

  logout() {
    authenticationService.logout();
    history.push("/login");
  }

  render() {
    const { currentUser, isAdmin, isUser, isPasswordChanged } = this.state;
    const handleSelect = (eventKey: any) => console.log(`selected ${eventKey}`);
    return (
      <Router history={history}>
        <div>
          {localStorage.getItem("currentUser") != null && currentUser && (
            <Nav
              variant="tabs"
              className="navbar-expand "
              activeKey="${eventKey}"
              onSelect={handleSelect}
            >
              <Nav.Item>
                <Nav.Link eventKey="1" href="/">
                  Home
                </Nav.Link>
              </Nav.Item>
              {isAdmin && (
                <Nav.Item>
                  <Nav.Link eventKey="2" href="/Admin">
                    Admin
                  </Nav.Link>
                </Nav.Item>
              )}
              {isUser && (
                <Nav.Item>
                  <Nav.Link eventKey="2" href="/User">
                    User
                  </Nav.Link>
                </Nav.Item>
              )}
              <Nav.Item>
                <Nav.Link onClick={this.logout} href="#">
                  Logout
                </Nav.Link>
              </Nav.Item>
              <NavDropdown title="UserName" id="nav-dropdown">
                <NavDropdown.Item eventKey="4.1">User Detail</NavDropdown.Item>
                <NavDropdown.Item onClick={this.handleShow}>
                  Change Password
                </NavDropdown.Item>
                <NavDropdown.Divider />
                <NavDropdown.Item onClick={this.logout}>
                  Logout
                </NavDropdown.Item>
              </NavDropdown>
            </Nav>
          )}

          <Modal
            show={this.state.showModal}
            onHide={this.handleClose}
            backdrop="static"
            keyboard={false}
          >
            <Modal.Header>
              <Modal.Title>Change Password</Modal.Title>
            </Modal.Header>
            {!isPasswordChanged && (
              <Formik
                initialValues={{
                  oldpassword: "",
                  newpassword: "",
                }}
                validationSchema={Yup.object().shape({
                  oldpassword: Yup.string().required("Password is required"),
                  newpassword: Yup.string().required("Password is required"),
                })}
                onSubmit={(
                  { oldpassword, newpassword },
                  { setStatus, setSubmitting }
                ) => {
                  setStatus();
                  let id = authenticationService.currentUserValue.id;
                  userService.changePassword(id, oldpassword, newpassword).then(
                    () => {
                      setSubmitting(false);
                      this.handlePasswordChange();
                    },
                    (error) => {
                      setSubmitting(false);
                      setStatus(error);
                    }
                  );
                }}
                render={({ errors, status, touched, isSubmitting }) => (
                  <Form>
                    <Modal.Body>
                      <div className="form-group row">
                        <div className="col-4">Old Password</div>
                        <div className="col-8">
                          <Field
                            name="oldpassword"
                            type="password"
                            className={
                              "form-control" +
                              (errors.oldpassword && touched.oldpassword
                                ? " is-invalid"
                                : "")
                            }
                          />
                          {status && <div className={""}>{status}</div>}
                        </div>
                        <ErrorMessage
                          name="newpassword"
                          component="div"
                          className="invalid-feedback"
                        />
                      </div>
                      <div className="form-group row">
                        <div className="col-4">New Password</div>
                        <div className="col-8">
                          <Field
                            name="newpassword"
                            type="password"
                            className={
                              "form-control" +
                              (errors.newpassword && touched.newpassword
                                ? " is-invalid"
                                : "")
                            }
                          />
                        </div>
                        <ErrorMessage
                          name="newpassword"
                          component="div"
                          className="invalid-feedback"
                        />
                      </div>
                    </Modal.Body>
                    <Modal.Footer>
                      <div className="form-group">
                        <button
                          type="submit"
                          className="btn btn-primary"
                          disabled={isSubmitting}
                        >
                          Save
                        </button>
                        {isSubmitting && (
                          <img src="data:image/gif;base64,R0lGODlhEAAQAPIAAP///wAAAMLCwkJCQgAAAGJiYoKCgpKSkiH/C05FVFNDQVBFMi4wAwEAAAAh/hpDcmVhdGVkIHdpdGggYWpheGxvYWQuaW5mbwAh+QQJCgAAACwAAAAAEAAQAAADMwi63P4wyklrE2MIOggZnAdOmGYJRbExwroUmcG2LmDEwnHQLVsYOd2mBzkYDAdKa+dIAAAh+QQJCgAAACwAAAAAEAAQAAADNAi63P5OjCEgG4QMu7DmikRxQlFUYDEZIGBMRVsaqHwctXXf7WEYB4Ag1xjihkMZsiUkKhIAIfkECQoAAAAsAAAAABAAEAAAAzYIujIjK8pByJDMlFYvBoVjHA70GU7xSUJhmKtwHPAKzLO9HMaoKwJZ7Rf8AYPDDzKpZBqfvwQAIfkECQoAAAAsAAAAABAAEAAAAzMIumIlK8oyhpHsnFZfhYumCYUhDAQxRIdhHBGqRoKw0R8DYlJd8z0fMDgsGo/IpHI5TAAAIfkECQoAAAAsAAAAABAAEAAAAzIIunInK0rnZBTwGPNMgQwmdsNgXGJUlIWEuR5oWUIpz8pAEAMe6TwfwyYsGo/IpFKSAAAh+QQJCgAAACwAAAAAEAAQAAADMwi6IMKQORfjdOe82p4wGccc4CEuQradylesojEMBgsUc2G7sDX3lQGBMLAJibufbSlKAAAh+QQJCgAAACwAAAAAEAAQAAADMgi63P7wCRHZnFVdmgHu2nFwlWCI3WGc3TSWhUFGxTAUkGCbtgENBMJAEJsxgMLWzpEAACH5BAkKAAAALAAAAAAQABAAAAMyCLrc/jDKSatlQtScKdceCAjDII7HcQ4EMTCpyrCuUBjCYRgHVtqlAiB1YhiCnlsRkAAAOwAAAAAAAAAAAA==" />
                        )}
                      </div>
                      <Button variant="secondary" onClick={this.handleClose}>
                        Cancel
                      </Button>
                    </Modal.Footer>
                  </Form>
                )}
              />
            )}
            {isPasswordChanged && (
              <div>
                <Modal.Body>
                  <p>Your Password has been changed successfully!</p>
                </Modal.Body>
                <Modal.Footer>
                  <Button variant="secondary" onClick={this.handleClose}>
                    Close
                  </Button>
                </Modal.Footer>
              </div>
            )}
          </Modal>

          <div className="jumbotron">
            <div className="container">
              <div className="row">
                <div className="col-md-6 offset-md-3">
                  <PrivateRoute
                    exact
                    path="/"
                    roles={[Role.Admin]}
                    component={HomePage}
                  />
                  <PrivateRoute
                    exact
                    path="/"
                    roles={[Role.User]}
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
