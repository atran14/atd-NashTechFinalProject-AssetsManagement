import { ErrorMessage, Field, Form, Formik } from "formik";
import * as Yup from "yup";
import React from "react";
import { useEffect } from "react";
import { useState } from "react";
import { Modal, Button } from "react-bootstrap";
import { history } from "../helpers/history";
import { authenticationService } from "../services/authentication.service";
import { userService } from "../services/user.service";

export function HomePage() {
  const [show, setShow] = useState(false);

  const handleClose = () => {
    setShow(false);
    authenticationService.logout();
    history.push("/login");
  };
  const handleShow = () => setShow(true);

  const [error, setError] = useState(null);

  let id = authenticationService.currentUserValue.id;

  useEffect(() => {
    if (authenticationService.currentUserValue.onFirstLogin == 1) {
      setShow(true);
    }
  });

  return (
    <div>
      <h1>HomePage</h1>
      <Modal
        show={show}
        onHide={handleClose}
        backdrop="static"
        keyboard={false}
      >
        <Modal.Header>
          <Modal.Title>Change Password</Modal.Title>
        </Modal.Header>
        <Formik
          initialValues={{
            newpassword: "",
          }}
          validationSchema={Yup.object().shape({
            newpassword: Yup.string().required("Password is required"),
          })}
          onSubmit={({ newpassword }, { setStatus, setSubmitting }) => {
            setStatus();
            userService
              .changePassword(id, " ", newpassword)
              .then((response) => {
                setError(response);
                alert(response);
              });
          }}
          render={({ errors, status, touched, isSubmitting }) => (
            <Form>
              <Modal.Body>
                <p>This is the first time you logged in.</p>
                <p>You have to change your password to continue.</p>
                <div className="form-group row">
                  <div className="col-4">Password</div>
                  <div className="col-8">
                    <Field
                      name="newpassword"
                      type="password"
                      className={
                        "col-8" +
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
                {status && <div className={"alert alert-danger"}>{status}</div>}
                <Button variant="secondary" onClick={handleClose}>
                  Close and Logout
                </Button>
              </Modal.Footer>
            </Form>
          )}
        />
      </Modal>
    </div>
  );
}
