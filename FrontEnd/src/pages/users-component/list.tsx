import { Button, Table } from 'antd'
import { useState } from 'react'
import { useEffect } from 'react'
import { UsersPagedListResponse } from '../../models/PagedListResponse'
import { User, UserType } from '../../models/User'
import { UserService } from '../../services/UserService'

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
      let fullNameA = `${a.firstName} ${a.lastName}`;
      let fullNameB = `${b.firstName} ${b.lastName}`;
      return fullNameA.localeCompare(fullNameB);
    },
    render: (text: any, record: User, index: number) => {
      console.log({ text, record, index })
      return <div>{`${record.firstName} ${record.lastName}`}</div>
    },
    sortDirections: ['ascend', 'descend']
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
      return new Date(a.joinedDate).getSeconds() - new Date(b.joinedDate).getSeconds();
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
        value: UserType.ADMIN
      },
      {
        text: 'USER',
        value: UserType.USER
      }
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
    render: () => {
      return (
        <>
          <Button></Button>
        </>
      )
    },
  },
]

export function ListUsers() {
  let [usersPagedList, setUsersPagedList] = useState<UsersPagedListResponse>()
  useEffect(() => {
    ;(async () => {
      let userServices = UserService.getInstance()
      let usersPagedResponse = await userServices.getUsers()

      // console.log(usersPagedResponse)
      setUsersPagedList(usersPagedResponse)
    })()
  }, [])

  return (
    <>
      {usersPagedList !== undefined && (
        <Table dataSource={usersPagedList.items} columns={columns}></Table>
      )}
    </>
  )
}
