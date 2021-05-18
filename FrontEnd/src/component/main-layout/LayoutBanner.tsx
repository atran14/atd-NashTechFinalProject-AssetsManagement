import React from 'react';
import { Layout, Menu } from 'antd';
import { getUsernameAvatar } from '../UserAvatar';
import { LogoutOutlined, UserOutlined } from '@ant-design/icons';
<<<<<<< HEAD

=======
import { AuthenticationService } from '../../services/AuthenticationService';
>>>>>>> develop

const { Header } = Layout;
const { SubMenu } = Menu;

function LayoutBanner() {
<<<<<<< HEAD

=======
  const service = new AuthenticationService();
  let OnLogout = () => {
    (async () => {
      await service.logout();
    })();
  };
>>>>>>> develop
  return (
    <Header className="header" style={{ position: 'fixed', width: '100%', float: 'right' }}>
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
<<<<<<< HEAD
            <span>
=======
            <span onClick={() => OnLogout}>
>>>>>>> develop
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
