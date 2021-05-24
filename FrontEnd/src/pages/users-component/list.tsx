import {
  Button,
  Col,
  Form,
  Input,
  message,
  Modal,
  Pagination,
  Popover,
  Row,
  Select,
  Table,
} from 'antd'
import { useState, useEffect } from 'react'
import {
  PaginationParameters,
  UsersPagedListResponse,
} from '../../models/Pagination'
import {
  Location,
  User,
  UserGender,
  UserStatus,
  UserType,
} from '../../models/User'
import { UserService } from '../../services/UserService'
import {
  EditOutlined,
  UserAddOutlined,
  SearchOutlined,
  DeleteOutlined,
  FilterFilled,
  ExclamationCircleOutlined,
  InfoCircleOutlined,
} from '@ant-design/icons'
import { Link, Redirect } from 'react-router-dom'
import './users.css'

const { Option } = Select
const { confirm } = Modal

const listAssignments = [
  {
    id: 1,
    assetId: 1,
    assignedByUserId: 1,
    assignedToUserId: 2,
    assignedDate: new Date(),
    state: 1,
    note: 'abc',
  },
  {
    id: 2,
    assetId: 2,
    assignedByUserId: 1,
    assignedToUserId: 4,
    assignedDate: new Date(),
    state: 2,
    note: 'abc',
  },
  {
    id: 3,
    assetId: 3,
    assignedByUserId: 2,
    assignedToUserId: 1,
    assignedDate: new Date(),
    state: 2,
    note: 'abc',
  },
]

interface PassedInEditedUserProps {
  editedUser: User
}

interface SearchAction {
  action: 'filter' | 'search'
  query: number | string
}

const ADMIN = "ADMIN";

export function ListUsers({ editedUser }: PassedInEditedUserProps) {
  let [isAdminAuthorized] = useState(sessionStorage.getItem('type') === ADMIN)
  let [isFetchingData, setIsFetchingData] = useState(false)
  let [usersPagedList, setUsersPagedList] = useState<UsersPagedListResponse>()
  let [usersList, setUsersList] = useState<User[]>([])
  let [filterSelected, setFilterSelected] = useState(false)
  let [latestSearchAction, setLatestSearchAction] = useState<SearchAction>({
    action: 'search',
    query: '',
  })

  useEffect(() => {
    if (isAdminAuthorized) {
      ;(async () => {
        setIsFetchingData(true)
        let userServices = UserService.getInstance()
        let usersPagedResponse = await userServices.getUsers()

        setUsersPagedList(usersPagedResponse)
        setUsersList(usersPagedResponse.items)
        setLatestSearchAction({
          action: 'search',
          query: '',
        })
        setIsFetchingData(false)
      })()
    }
  }, [editedUser])

  function notDisabledYourself(id: number) {
    if (id === JSON.parse(sessionStorage.getItem('id')!)) {
      return false
    }

    return true
  }

  let notOnlyOneAdminRemain = async (id: number) => {
    let userService = UserService.getInstance()
    var count = 0
    var user = await userService.getUser(id)
    usersList.map((u: any) => {
      if (u.type === UserType.ADMIN && u.status === UserStatus.ACTIVE) {
        count++
      }
    })

    if (count < 2 && user.type === UserType.ADMIN) {
      return false
    }

    return true
  }

  function disabledUser(id: number) {
    var count = 0
    listAssignments.map((a: any) => {
      if (a.assignedToUserId === id && a.state !== 2) {
        count++
      }
    })

    if (!notDisabledYourself(id)) {
      Modal.error({
        title: 'You can not disable yourself',
      })
    }

    if (!notOnlyOneAdminRemain(id)) {
      Modal.error({
        title: 'System has only one admin remain',
      })
    }

    if (count === 0 && notDisabledYourself(id) && notOnlyOneAdminRemain(id)) {
      confirm({
        title: 'Do you want to disable this user?',
        icon: <ExclamationCircleOutlined />,
        onOk() {
          let userService = UserService.getInstance()
          try {
            userService.disableUser(id)
            message.success('Disabled Successfully')
            setUsersList((userId: any[]) =>
              userId.filter((item) => item.id !== id),
            )
          } catch {
            message.success('Something went wrong')
          }
        },
        onCancel() {
          console.log('Cancel')
        },
      })
    }
    if (count > 0) {
      Modal.error({
        title:
          'There are valid assignments belonging to this user. Please close all assignments before disabling user.',
      })
    }
  }

  const onSearchButtonClicked = (values: any) => {
    ;(async () => {
      setIsFetchingData(true)
      let { searchText } = values
      let userService = UserService.getInstance()
      let usersPagedResponse: UsersPagedListResponse

      if (searchText.length === 0) {
        usersPagedResponse = await userService.getUsers()
      } else {
        usersPagedResponse = await userService.searchUsers(searchText)
      }

      setLatestSearchAction({
        action: 'search',
        query: searchText as string,
      })
      setUsersPagedList(usersPagedResponse)
      setUsersList(usersPagedResponse.items)
      setIsFetchingData(false)
    })()
  }

  const onFilterButtonClicked = (values: any) => {
    ;(async () => {
      setIsFetchingData(true)

      let { filteredUserType } = values
      let userService = UserService.getInstance()
      let usersPagedResponse: UsersPagedListResponse = await userService.filterByType(
        filteredUserType as number,
      )

      setLatestSearchAction({
        action: 'filter',
        query: filteredUserType as number,
      })
      setUsersPagedList(usersPagedResponse)
      setUsersList(usersPagedResponse.items)

      setIsFetchingData(false)
    })()
  }

  const generateDetailedUserContent = (record: User) => {
    return (
      <table>
        <tr>
          <th>Staff code</th>
          <td>{record.staffCode}</td>
        </tr>
        <tr>
          <th>Username</th>
          <td>{record.userName}</td>
        </tr>
        <tr>
          <th>Full name</th>
          <td>{`${record.firstName} ${record.lastName}`}</td>
        </tr>
        <tr>
          <th>DOB</th>
          <td>{new Date(record.dateOfBirth).toLocaleDateString()}</td>
        </tr>
        <tr>
          <th>Gender</th>
          <td>{UserGender[record.gender]}</td>
        </tr>
        <tr>
          <th>Joined date</th>
          <td>{new Date(record.joinedDate).toLocaleDateString()}</td>
        </tr>
        <tr>
          <th>Type</th>
          <td>{UserType[record.type]}</td>
        </tr>
        <tr>
          <th>Location</th>
          <td>{Location[record.location]}</td>
        </tr>
      </table>
    )
  }

  const onPaginationConfigChanged = (page: number, pageSize?: number) => {
    ;(async () => {
      setIsFetchingData(true)
      let userService = UserService.getInstance()
      let parameters: PaginationParameters = {
        PageNumber: page,
        PageSize: pageSize ?? 10,
      }

      let usersPagedResponse: UsersPagedListResponse
      switch (latestSearchAction.action) {
        case 'search':
          let searchQuery = latestSearchAction.query as string
          if (searchQuery.length === 0) {
            usersPagedResponse = await userService.getUsers(parameters)
          } else {
            usersPagedResponse = await userService.searchUsers(
              searchQuery,
              parameters,
            )
          }
          break
        case 'filter':
          let filterQuery = latestSearchAction.query as number
          usersPagedResponse = await userService.filterByType(
            filterQuery,
            parameters,
          )
          break
      }

      setUsersPagedList(usersPagedResponse)
      setUsersList(usersPagedResponse.items)
      setIsFetchingData(false)
    })()
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
          new Date(a.joinedDate).getTime() - new Date(b.joinedDate).getTime()
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
      sorter: (a: User, b: User) => a.type - b.type,
      sortDirections: ['ascend', 'descend'],
    },
    {
      title: '',
      dataIndex: 'action',
      key: 'action',
      render: (text: any, record: User, index: number) => {
        return (
          <Row>
            <Col offset={1}>
              <Popover
                title="User details"
                content={generateDetailedUserContent(record)}
                placement="right"
                trigger="click"
              >
                <Button icon={<InfoCircleOutlined />} />
              </Popover>
            </Col>
            <Col offset={1}>
              <Link to={`/users/update/${record.id}`}>
                <Button type="primary" icon={<EditOutlined />} />
              </Link>
            </Col>
            <Col offset={1}>
              <Button
                danger
                type="primary"
                icon={<DeleteOutlined />}
                onClick={() => disabledUser(record.id)}
              />
            </Col>
          </Row>
        )
      },
    },
  ]

  return (
    <>
      {!isAdminAuthorized && <Redirect to="/401-access-denied" />}
      {isAdminAuthorized && usersPagedList !== undefined && (
        <>
          <Row>
            <Col span={5}>
              <Form onFinish={onFilterButtonClicked}>
                <Row justify="start">
                  <Col span={15}>
                    <Form.Item
                      name="filteredUserType"
                      className="no-margin-no-padding"
                    >
                      <Select
                        placeholder="Select user type"
                        style={{ width: '100%' }}
                        onSelect={() => setFilterSelected(true)}
                        disabled={isFetchingData}
                      >
                        <Option key="admin" value={0}>
                          Admin
                        </Option>
                        <Option key="user" value={1}>
                          User
                        </Option>
                      </Select>
                    </Form.Item>
                  </Col>

                  <Col offset={1}>
                    <Form.Item className="no-margin-no-padding">
                      <Button
                        size="middle"
                        icon={<FilterFilled />}
                        htmlType="submit"
                        disabled={!filterSelected || isFetchingData}
                      />
                    </Form.Item>
                  </Col>
                </Row>
              </Form>
            </Col>
            <Col span={4} offset={10}>
              <Form
                onFinish={onSearchButtonClicked}
                initialValues={{
                  searchText: '',
                }}
              >
                <Row justify="end">
                  <Col span={18}>
                    <Form.Item
                      name="searchText"
                      className="no-margin-no-padding"
                    >
                      <Input
                        allowClear
                        disabled={isFetchingData}
                        style={{ width: '100%' }}
                        placeholder="e.g. Bob/SD0001"
                      />
                    </Form.Item>
                  </Col>
                  <Col offset={1}>
                    <Form.Item className="no-margin-no-padding">
                      <Button
                        size="middle"
                        icon={<SearchOutlined />}
                        type="primary"
                        htmlType="submit"
                        disabled={isFetchingData}
                      />
                    </Form.Item>
                  </Col>
                </Row>
              </Form>
            </Col>
            <Col span={4} offset={1}>
              <Link to="/users/create">
                <Button
                  style={{
                    width: '100%',
                    backgroundColor: '#e9424d',
                    border: '#e9424d',
                  }}
                  type="primary"
                  icon={<UserAddOutlined />}
                >
                  Create new user
                </Button>
              </Link>
            </Col>
          </Row>

          <Table
            style={{
              margin: '1.25em 0 1.25em 0',
            }}
            dataSource={usersList}
            columns={columns}
            scroll={{ y: 400 }}
            pagination={false}
            loading={isFetchingData}
          />

          <Row justify="center">
            <Col>
              <Pagination
                disabled={isFetchingData}
                total={usersPagedList.totalCount}
                showTotal={(total: number) => `Total: ${total} result(s)`}
                pageSizeOptions={['10', '20', '50']}
                showSizeChanger
                onChange={onPaginationConfigChanged}
              />
            </Col>
          </Row>
        </>
      )}
    </>
  )
}
