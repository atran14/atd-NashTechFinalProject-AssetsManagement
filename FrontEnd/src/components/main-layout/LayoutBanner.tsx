import { Menu } from 'antd';
import { LogoutOutlined } from '@ant-design/icons';

const { SubMenu } = Menu;

function LayoutBanner() {

  let OnLogout = () => {
    sessionStorage.clear();
    window.location.href = '/login';
  };

  return (
    <>
      {sessionStorage.getItem("username") && <Menu
        key="1"
        style={{ backgroundColor: '#cf2338', width: '100%', zIndex: 1, position: 'fixed' }}
        theme="light"
        mode="horizontal"
        defaultSelectedKeys={['2']}
      >
        <SubMenu title='Asset Management Portal' style={{ fontFamily: 'Roboto', color: 'white', fontSize: 20 }}>
        </SubMenu>
        <SubMenu
          title={sessionStorage.getItem("username")}
          style={{ float: 'right', fontFamily: 'Roboto', color: 'white', fontSize: 20 }}>
          <Menu.Item key="setting:2">
            <span onClick={OnLogout}>
              <LogoutOutlined />
              Logout
            </span>
          </Menu.Item>
        </SubMenu>
      </Menu>
      }
      {!sessionStorage.getItem("username") && 
      <Menu
        key="1"
        style={{ backgroundColor: '#cf2338', width: '100%', zIndex: 1, position: 'fixed' }}
        theme="light"
        mode="horizontal"
        defaultSelectedKeys={['2']}
      >
        <SubMenu title='Asset Management Portal' style={{ fontFamily: 'Roboto', color: 'white', fontSize: 20 }}>
        </SubMenu>
      </Menu>
      }
    </>
  );
}

export default LayoutBanner;
