import { Button, Form, Input, message, Popconfirm, Select, Table } from 'antd'
import { useState } from 'react'
import { useEffect } from 'react'
import { UsersPagedListResponse } from '../../models/PagedListResponse'
import { User, UserType } from '../../models/User'
import { UserService } from '../../services/UserService'
import {
  EditOutlined,
  SearchOutlined,
  UserDeleteOutlined,
} from '@ant-design/icons'
import { Link } from 'react-router-dom'

const { Option } = Select

export function ListUsers() {
  let [usersPagedList, setUsersPagedList] = useState<UsersPagedListResponse>()
  let [usersList, setUsersList] = useState<User[]>([])
  useEffect(() => {
    (async () => {
      let userServices = UserService.getInstance()
      let usersPagedResponse = await userServices.getUsers()

      setUsersPagedList(usersPagedResponse)
      setUsersList(usersPagedResponse.items)
    })()
  }, [])

  function confirm(id: number) {
    let userServices = UserService.getInstance()
    try {
      userServices.disableUser(id)
      message.success('Disabled Successfully')
      setUsersList((userId: any[]) => userId.filter((item) => item.id !== id))
    } catch {
      message.success('Something went wrong')
    }
  }

  function cancel() {
    console.log('cancel')
  }

  const onFinish = (values: any) => {
    if (usersPagedList !== undefined) {
      let newList: User[]

      if (values['searchMode'] === 'fullName') {
        newList = usersPagedList.items.filter((u: User) => {
          let fullName = `${u.firstName} ${u.lastName}`
          return fullName.startsWith(values['searchText'])
        })
      } else {
        newList = usersPagedList.items.filter((u: User) => {
          return u.staffCode.startsWith(values['searchText'])
        })
      }

      setUsersList(newList)
    }
  }

  const columns: any = [
    {
      title: 'Staff code',
      dataIndex: 'staffCode',
      key: 'staffCode',
      sorter: (a: User, b: User) => a.staffCode.localeCompare(b.staffCode),
      sortDirections: ['ascend', 'descend'],
    },
    {
      title: 'Full name',
      dataIndex: 'fullName',
      key: 'fullName',
      sorter: (a: User, b: User) => {
        let fullNameA = `${a.firstName} ${a.lastName}`
        let fullNameB = `${b.firstName} ${b.lastName}`
        return fullNameA.localeCompare(fullNameB)
      },
      render: (text: any, record: User, index: number) => {
        return <div>{`${record.firstName} ${record.lastName}`}</div>
      },
      sortDirections: ['ascend', 'descend'],
    },
    {
      title: 'Username',
      dataIndex: 'userName',
      key: 'userName',
      sorter: (a: User, b: User) => a.userName.localeCompare(b.userName),
      sortDirections: ['ascend', 'descend'],
    },
    {
      title: 'Joined date',
      dataIndex: 'joinedDate',
      key: 'joinedDate',
      render: (text: any, record: User, index: number) => {
        return <div>{new Date(record.joinedDate).toLocaleDateString()}</div>
      },
      sorter: (a: User, b: User) => {
        return (
          new Date(a.joinedDate).getTime() -
          new Date(b.joinedDate).getTime()
        )
      },
      sortDirections: ['ascend', 'descend'],
    },
    {
      title: 'Type',
      dataIndex: 'type',
      key: 'type',
      render: (text: any, record: User, index: number) => {
        return <div>{UserType[record.type]}</div>
      },
      filters: [
        {
          text: 'ADMIN',
          value: UserType.ADMIN,
        },
        {
          text: 'USER',
          value: UserType.USER,
        },
      ],
      onFilter: (value: UserType, record: User) => {
        return record.type === value
      },
      sorter: (a: User, b: User) => a.type - b.type,
      sortDirections: ['ascend', 'descend'],
    },
    {
      title: '',
      dataIndex: 'action',
      key: 'action',
      render: (text: any, record: User) => {
        return (
          <>
            <Link to={`/users/update/${record.id}`}>
              <Button type="primary" icon={<EditOutlined />} />
            </Link>
            <Popconfirm
              title="Are you sure to disable this user?"
              onConfirm={() => {
                confirm(record.id)
              }}
              onCancel={cancel}
              okText="Yes"
              cancelText="No"
            >
              <Button danger type="primary" icon={<UserDeleteOutlined />} />
            </Popconfirm>
          </>
        )
      },
    },
  ]

  return (
    <>
      {usersPagedList !== undefined && (
        <>
          <Form onFinish={onFinish} initialValues={{
            searchMode: "fullName",
            searchText: ""
          }}>
            <Input.Group compact>
              <Form.Item name="searchMode">
                <Select>
                  <Option value="fullName">Full name</Option>
                  <Option value="staffCode">Staff code</Option>
                </Select>
              </Form.Item>

              <Form.Item name="searchText">
                <Input style={{ width: '75%' }} defaultValue="Nguyen Van A" />
              </Form.Item>

              <Form.Item>
                <Button
                  size="small"
                  icon={<SearchOutlined />}
                  type="primary"
                  htmlType="submit"
                />
              </Form.Item>
            </Input.Group>
          </Form>
          <Table dataSource={usersList} columns={columns}></Table>
        </>
      )}
    </>
  )
}
