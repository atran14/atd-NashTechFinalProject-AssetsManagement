import React from 'react';
import { Layout, Menu } from 'antd';
import { getUsernameAvatar } from '../UserAvatar';
import { LogoutOutlined, UserOutlined } from '@ant-design/icons';
import { AuthenticationService } from '../../services/AuthenticationService';

const { Header } = Layout;
const { SubMenu } = Menu;

function LayoutBanner() {
  const service = AuthenticationService.getInstance();
  let OnLogout = () => {
    (async () => {
      // await service.logout();
    })();
  };
  return (
    <Header className="header" style={{ position: 'fixed', width: '100%', float: 'right', zIndex: 0 }}>
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
            <span onClick={() => OnLogout}>
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
