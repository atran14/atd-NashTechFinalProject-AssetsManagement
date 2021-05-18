import React from 'react';
import { Layout, Menu } from 'antd';
import {
  ContainerOutlined,
  UnorderedListOutlined,
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
        <SubMenu key="sub1" icon={<UnorderedListOutlined />} title="Category">
          <Menu.Item key="1">
            <Link to="/category">List</Link>
          </Menu.Item>
        </SubMenu>
        <SubMenu key="sub2" icon={<ContainerOutlined />} title="Book">
          <Menu.Item key="3">
            <Link to="/book">List</Link>
          </Menu.Item>
          <Menu.Item key="4">
            <Link to="/book/create">Create</Link>
          </Menu.Item>
        </SubMenu>
        <SubMenu key="sub3" icon={<ContainerOutlined />} title="Request">
          <Menu.Item key="5">
            <Link to="/request">List</Link>
          </Menu.Item>
          <Menu.Item key="6">
            <Link to="/request/create">Create</Link>
          </Menu.Item>
        </SubMenu>
        <SubMenu key="sub4" icon={<UserOutlined />} title="Login">
          <Menu.Item key="9"><Link to="/login">Login</Link></Menu.Item>
          <Menu.Item key="10"><Link to="/register">Register</Link></Menu.Item>
        </SubMenu>
      </Menu>
    </Sider>
  );
}

export default SiderMenu;
