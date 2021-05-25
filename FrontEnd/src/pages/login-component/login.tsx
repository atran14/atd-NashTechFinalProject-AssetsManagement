import { Button, Form, Input } from "antd";
import Title from "antd/lib/typography/Title";
import React from "react";
import { useState } from "react";
import { useHistory } from "react-router-dom";
import { UserLogin, UserType } from "../../models/User";
import { authenticationService } from "../../services/authentication.service";
import { AuthenticationService } from "../../services/AuthenticationService";

export function Login() {
  const layout = {
    labelCol: {
      span: 16,
      offset: 3,
      pull: 9,
    },
    wrapperCol: {
      span: 16,
      pull: 9,
    },
  };
  const tailLayout = {
    wrapperCol: {},
  };

  let history = useHistory();
  const service = AuthenticationService.getInstance();

  const onFinish = (data: UserLogin) => {
    authenticationService.login(data.username, data.password).then(
      (user) => {
        (async () => {
          let userLogin = await service.login(data);
          sessionStorage.setItem("id", userLogin.id.toString());
          sessionStorage.setItem("type", UserType[userLogin.type]);
          sessionStorage.setItem("username", userLogin.userName);
          sessionStorage.setItem("token", userLogin.token);
          sessionStorage.setItem("location", userLogin.location.toString());
          history.replace("/home");
        })();
      },
      (error) => {
        // setSubmitting(false);
        setStatus(error);
      }
    );
  };

  const onFinishFailed = (errorInfo: any) => {
    console.log("Failed:", errorInfo);
  };

  //

  // const [error, setError] = useState(null);
  const [status, setStatus] = useState(null);
  return (
    <>
      <Title>Login</Title>
      <Form
        {...layout}
        name="basic"
        onFinish={onFinish}
        onFinishFailed={onFinishFailed}
      >
        <Form.Item
          label="Username"
          name="username"
          rules={[
            {
              required: true,
              message: "Please input your username!",
            },
          ]}
        >
          <Input />
        </Form.Item>

        <Form.Item
          label="Password"
          name="password"
          rules={[
            {
              required: true,
              message: "Please input your password!",
            },
          ]}
        >
          <Input.Password />
        </Form.Item>

        <Form.Item {...tailLayout}>
          <Button
            style={{ backgroundColor: "#e9424d", color: "white" }}
            htmlType="submit"
          >
            Login
          </Button>
        </Form.Item>

        {status && <div className={"alert alert-danger"}>{status}</div>}
      </Form>
    </>
  );
}
