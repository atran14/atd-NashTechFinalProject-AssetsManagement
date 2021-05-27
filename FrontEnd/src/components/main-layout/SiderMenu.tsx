import React from 'react'
import { Layout, Menu } from 'antd'
import {
  LoginOutlined,
  UserOutlined,
  RollbackOutlined,
} from '@ant-design/icons'
import { Link, useLocation } from 'react-router-dom'
import { UserType } from '../../models/User'

const { Sider } = Layout

function SiderMenu() {
  let location = useLocation()
  let userType = sessionStorage.getItem('type')

  return (
    <Sider
      width={200}
      className="site-layout-background"
      style={{
        marginTop: 50,
        overflow: 'auto',
        height: '100vh',
        position: 'fixed',
        left: 0,
      }}
    >
      <Menu
        theme="light"
        mode="inline"
        style={{ height: '100%', borderRight: 0 }}
        selectedKeys={[location.pathname]}
      >
        {userType && userType === UserType[UserType.ADMIN] && (
          <>
            <Menu.Item
              icon={<UserOutlined />}
              key="/users"
              className="menu-item"
            >
              <Link to="/users">Users</Link>
            </Menu.Item>
            <Menu.Item
              icon={<RollbackOutlined />}
              key="//return-requests"
              className="menu-item"
            >
              <Link to="/return-requests">Return Requests</Link>
            </Menu.Item>
          </>
        )}
        {userType && userType === UserType[UserType.USER] && (
          <>
            <Menu.Item
              icon={<UserOutlined />}
              key="/users"
              className="menu-item"
            >
              <Link to="/users">Users</Link>
            </Menu.Item>
          </>
        )}
        {!userType && (
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
  )
}

export default SiderMenu
