import React from 'react';
import { Layout, Menu } from 'antd';
import {  
  LoginOutlined,
  UserOutlined,
} from '@ant-design/icons';
import { Link, useLocation } from 'react-router-dom';

const { SubMenu } = Menu;

const { Sider } = Layout;

function SiderMenu() {
  let location = useLocation();

  return (
    <Sider width={200} className="site-layout-background"
      style={{
        marginTop: 60,
        overflow: 'auto',
        height: '100vh',
        position: 'fixed',
        left: 0,
      }}>
      <Menu theme="light"
        mode="inline"
        style={{ height: '100%', borderRight: 0 }}
        selectedKeys={[location.pathname]}
      >
        <SubMenu key="sub1" icon={<UserOutlined />} title="Users">
          <Menu.Item key="/users">
            <Link to="/users">List</Link>
          </Menu.Item>
          
          <Menu.Item key="/users/create">
            <Link to="/users/create">Create</Link>
          </Menu.Item>
        
        </SubMenu>
        <SubMenu key="sub4" icon={<LoginOutlined />} title="Login">
          <Menu.Item key="/login"><Link to="/login">Login</Link></Menu.Item>
        </SubMenu>
      </Menu>
    </Sider>
  );
}

export default SiderMenu;
