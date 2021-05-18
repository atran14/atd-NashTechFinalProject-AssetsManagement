import { Table } from 'antd'
import { useState } from 'react'
import { useEffect } from 'react'
import { UsersPagedListResponse } from '../../models/PagedListResponse'
import { User, UserType } from '../../models/user'
import { UserService } from '../../services/UserServices'

const columns = [
  {
    title: 'Staff code',
    dataIndex: 'staffCode',
    key: 'staffCode',
  },
  {
    title: 'Full name',
    dataIndex: 'fullName',
    key: 'fullName',
    render: (text: any, record: User, index: any) => {
      console.log({text, record, index});
      return <div>{`${record.firstName} ${record.lastName}`}</div>
    }
  },
  {
    title: 'Username',
    dataIndex: 'userName',
    key: 'userName',
  },
  {
    title: 'Joined date',
    dataIndex: 'joinedDate',
    key: 'joinedDate',
  },
  {
    title: 'Type',
    dataIndex: 'type',
    key: 'type',
    render: (text: any, record: User, index: any) => {
      return <div>{UserType[record.type]}</div>
    }
  },
  {
    title: '',
    dataIndex: 'action',
    key: 'action',
    render: () => {
      return <></>
    }
  }
]

export function ListUsers() {
  let [usersPagedList, setUsersPagedList] = useState<UsersPagedListResponse>()
  useEffect(() => {
    (async () => {
      let userServices = UserService.getInstance()
      let usersPagedResponse = await userServices.getUsers()

      console.log(usersPagedResponse)
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
