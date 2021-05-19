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
      console.log({ text, record, index })
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
        new Date(a.joinedDate).getSeconds() -
        new Date(b.joinedDate).getSeconds()
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

          <Button danger type="primary" icon={<UserDeleteOutlined />} />
        </>
      )
    },
  },
]

export function ListUsers() {
  let [usersPagedList, setUsersPagedList] = useState<UsersPagedListResponse>()
  let [usersList, setUsersList] = useState<User[]>([])
  useEffect(() => {
    ;(async () => {
      let userServices = UserService.getInstance()
      let usersPagedResponse = await userServices.getUsers()

      // console.log(usersPagedResponse)
      setUsersPagedList(usersPagedResponse)
      setUsersList(usersPagedResponse.items)
    })()
  }, [])

  const onFinish = (values: any) => {
    console.log(values)
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

  return (
    <>
      {usersPagedList !== undefined && (
        <>
          <Form onFinish={onFinish}>
            <Input.Group compact>
              <Form.Item name="searchMode" initialValue="fullName">
                <Select defaultValue="fullName">
                  <Option value="fullName">Full name</Option>
                  <Option value="staffCode">Staff code</Option>
                </Select>
              </Form.Item>

              <Form.Item name="searchText" initialValue="">
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
          <Table dataSource={usersList} columns={columns} scroll={{ y: 400 }}></Table>
        </>
      )}
    </>
  )
}
