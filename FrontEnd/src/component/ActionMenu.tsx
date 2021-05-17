import React, { useState } from 'react';
import { Button, Dropdown, Menu, Popconfirm } from 'antd';
import {
  DeleteOutlined,
  DownOutlined,
  EditOutlined,
  QuestionCircleOutlined,
} from '@ant-design/icons';
import { useHistory } from 'react-router-dom';
import { IActionMenu } from '../models/IActionMenu';

function useActionMenu({ selectedRow, updateEntityPath, service }: IActionMenu) {
  const [didChange, setChange] = useState(false);
  const history = useHistory();

  const handleMenuClick = (action: any) => {
    if (action.key === 'edit') {
      const updatePath = '/' + updateEntityPath + '/' + selectedRow.id;
      history.push(updatePath);
    }
  };

  const handleSingleDelete = () => {
    (async () => {
      await service.delete(selectedRow.id);
      setChange(pre=>!pre);
    })();
  };

  const actionMenu = (
    <Menu onClick={handleMenuClick}>
      <Menu.Item key="edit">
        <EditOutlined style={{color:'blue'}} />
        Update
      </Menu.Item>
      <Menu.Item key="delete">
        <Popconfirm
          title="Sure to delete?"
          placement="left"
          icon={<QuestionCircleOutlined style={{ color: 'red' }} />}
          onConfirm={handleSingleDelete}
        >
          <DeleteOutlined style={{color:'red'}} />
          Delete
        </Popconfirm>
      </Menu.Item>
    </Menu>
  );

  const actionColumnView = (
    <span>
      <Dropdown overlay={actionMenu} trigger={['click']}>
        <Button className="ant-dropdown-link">
          Actions <DownOutlined />
        </Button>
      </Dropdown>
    </span>
  );

  return {actionColumnView, didChange};
}

export default useActionMenu;
