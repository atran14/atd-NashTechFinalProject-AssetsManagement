import React from "react";
import { Layout, Menu } from "antd";
import { LoginOutlined, UserOutlined,PushpinOutlined } from "@ant-design/icons";
import { Link, useLocation } from "react-router-dom";

const { Sider } = Layout;

function SiderMenu() {
  let location = useLocation();

  return (
    <Sider
      width={200}
      className="site-layout-background"
      style={{
        marginTop: 50,
        overflow: "auto",
        height: "100vh",
        position: "fixed",
        left: 0,
      }}
    >
      <Menu
        theme="light"
        mode="inline"
        style={{ height: "100%", borderRight: 0 }}
        selectedKeys={[location.pathname]}
      >
        {sessionStorage.getItem("username") && (
          <Menu.Item icon={<UserOutlined />} key="/users" className="menu-item">
            <Link to="/users">Users</Link>
          </Menu.Item>
        )}
        {sessionStorage.getItem("username") && (
          <Menu.Item icon={<PushpinOutlined />} key="/users" className="menu-item">
            <Link to="/assignments">Assignments</Link>
          </Menu.Item>
        )}
        {!sessionStorage.getItem("username") && (
          <Menu.Item
            icon={<LoginOutlined />}
            key="/login"
            className="menu-item"
          >
            <Link to="/login">Login</Link>
          </Menu.Item>
        )}
      </Menu>
    </Sider>
  );
}

export default SiderMenu;
