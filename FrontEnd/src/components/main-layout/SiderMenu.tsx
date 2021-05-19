import React from 'react';
import { Layout, Menu } from 'antd';
import {  
  LoginOutlined,
  UserOutlined,
} from '@ant-design/icons';
import { Link } from 'react-router-dom';

const { SubMenu } = Menu;

const { Sider } = Layout;

function SiderMenu() {
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
        defaultSelectedKeys={['1']}
        defaultOpenKeys={['sub1']}
        style={{ height: '100%', borderRight: 0 }}
      >
        <SubMenu key="sub1" icon={<UserOutlined />} title="Users">
          <Menu.Item key="1">
            <Link to="/users">List</Link>
          </Menu.Item>
          
          <Menu.Item key="2">
            <Link to="/users/create">Create</Link>
          </Menu.Item>
        
        </SubMenu>
        <SubMenu key="sub4" icon={<LoginOutlined />} title="Login">
          <Menu.Item key="9"><Link to="/login">Login</Link></Menu.Item>
        </SubMenu>
      </Menu>
    </Sider>
  );
}

export default SiderMenu;
