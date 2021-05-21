import React from 'react';
import { Layout, Menu } from 'antd';
import { getUsernameAvatar } from '../UserAvatar';
import { LogoutOutlined, UserOutlined } from '@ant-design/icons';
import { useHistory } from 'react-router-dom';

const { Header } = Layout;
const { SubMenu } = Menu;

function LayoutBanner() {
  let history = useHistory();
  
  let OnLogout = () => {
    sessionStorage.clear();
    window.location.href = '/';
  };

  return (
    <Header  className="header" style={{ position: 'fixed', width: '100%', float: 'right', zIndex: 1 }}>
      <div className="logo" />
      <Menu
        key="1" style={{ float: 'right' }} theme="dark" mode="horizontal" defaultSelectedKeys={['2']}>
        <SubMenu title={getUsernameAvatar('Cemal')}>
          <Menu.Item key="setting:1">
            <span>
              <UserOutlined />
              Profile
            </span>
          </Menu.Item>
          <Menu.Item key="setting:2">
            <span onClick={OnLogout}>
              <LogoutOutlined />
              Logout
            </span>
          </Menu.Item>
        </SubMenu>
      </Menu>
    </Header>
  );
}

export default LayoutBanner;
