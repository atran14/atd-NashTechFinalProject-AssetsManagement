import {
  Button,
  Col,
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
import { AssignmentsService } from '../../services/AssignmentService'
import { Assignment } from '../../models/Assignment'
import { UserSearchFilterParameters } from '../../models/SearchFilterParameters'

const { Option } = Select
const { confirm } = Modal

interface PassedInEditedUserProps {
  editedUser: User
}

const ADMIN = 'ADMIN'

export function ListUsers({ editedUser }: PassedInEditedUserProps) {
  let [isAdminAuthorized] = useState(sessionStorage.getItem('type') === ADMIN)
  let [isFetchingData, setIsFetchingData] = useState(false)
  let [usersPagedList, setUsersPagedList] = useState<UsersPagedListResponse>()
  let [usersList, setUsersList] = useState<User[]>([])
  let [assignmentList, setAssignmentList] = useState<Assignment[]>([])  
  let [searchFilterParams, setSearchFilterParams] = useState<
    UserSearchFilterParameters
  >({
    searchQuery: '',
  })
  let [paginationParams, setPaginationParams] = useState<PaginationParameters>({
    PageNumber: 1,
    PageSize: 10,
  })
  let [userSearchFieldValue] = useState('')
  let assignmentService = AssignmentsService.getInstance()
  let userService = UserService.getInstance()

  useEffect(() => {
    if (isAdminAuthorized) {
      ;(async () => {
        setIsFetchingData(true)
        setSearchFilterParams({
          searchQuery: '',
        })
        let usersPagedResponse = await userService.getUsers()

        setUsersPagedList(usersPagedResponse)
        setUsersList(usersPagedResponse.items)
        setIsFetchingData(false)
      })()
    }
  }, [])

  useEffect(() => {
    ;(async () => {
      let list = await assignmentService.getAllNoCondition()
      setAssignmentList(list)
    })()
  }, [])

  function notDisabledYourself(id: number) {
    if (id === JSON.parse(sessionStorage.getItem('id')!)) {
      return false
    }

    return true
  }

  let notOnlyOneAdminRemain = async (id: number) => {
    var count = 0
    var user = await userService.getUser(id)
    usersList.forEach((u: any) => {
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
    assignmentList.forEach((a: any) => {
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
            message.error('Something went wrong')
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

  const onSearchButtonClicked = async () => {
    setIsFetchingData(true)

    let newSearchFilterParams: UserSearchFilterParameters = {
      ...searchFilterParams,
      searchQuery: userSearchFieldValue,
    }

    let newPaginationParams: PaginationParameters = {
      ...paginationParams,
      PageNumber: 1,
    }

    let usersPagedResponse: UsersPagedListResponse = await userService.searchAndFilter(
      newSearchFilterParams,
      newPaginationParams,
    )

    setSearchFilterParams(newSearchFilterParams)
    setPaginationParams(newPaginationParams)
    setUsersPagedList(usersPagedResponse)
    setUsersList(usersPagedResponse.items)

    setIsFetchingData(false)
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

  const onTypeFilterSelected = async (value: UserType) => {
    setIsFetchingData(true)

    let newSearchFilterParams: UserSearchFilterParameters = {
      ...searchFilterParams,
      type: value,
    }

    let newPaginationParams: PaginationParameters = {
      ...paginationParams,
      PageNumber: 1,
    }

    let usersPagedResponse = await userService.searchAndFilter(
      newSearchFilterParams,
      newPaginationParams,
    )

    setSearchFilterParams(newSearchFilterParams)
    setPaginationParams(paginationParams)
    setUsersPagedList(usersPagedResponse)
    setUsersList(usersPagedResponse.items)
    setIsFetchingData(false)
  }

  const onTypeFilerClear = async () => {
    setIsFetchingData(true)

    let newSearchFilterParams: UserSearchFilterParameters = {
      ...searchFilterParams,
      type: undefined,
    }

    let newPaginationParams: PaginationParameters = {
      ...paginationParams,
      PageNumber: 1,
    }

    let usersPagedResponse = await userService.searchAndFilter(
      newSearchFilterParams,
      newPaginationParams,
    )

    setSearchFilterParams(newSearchFilterParams)
    setPaginationParams(paginationParams)
    setUsersPagedList(usersPagedResponse)
    setUsersList(usersPagedResponse.items)
    setIsFetchingData(false)
  }

  const onPaginationConfigChanged = async (page: number, pageSize?: number) => {
    setIsFetchingData(true)

    let newPaginationParams: PaginationParameters = {
      PageNumber: page,
      PageSize: pageSize ?? 10,
    }

    let usersPagedResponse: UsersPagedListResponse = await userService.searchAndFilter(
      searchFilterParams,
      newPaginationParams
    )

    setPaginationParams(newPaginationParams)
    setUsersPagedList(usersPagedResponse)
    setUsersList(usersPagedResponse.items)
    setIsFetchingData(false)
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
              <Row justify="start">
                <Col span={15}>
                  <Select
                    placeholder="Select user type"
                    style={{ width: '100%' }}
                    onSelect={onTypeFilterSelected}
                    disabled={isFetchingData}
                    allowClear
                    onClear={onTypeFilerClear}
                  >
                    <Option key="admin" value={0}>
                      Admin
                    </Option>
                    <Option key="user" value={1}>
                      User
                    </Option>
                  </Select>
                </Col>
                <Col offset={1}>
                  <FilterFilled />
                </Col>
              </Row>
            </Col>

            <Col span={4} offset={10}>
              <Row justify="end">
                <Col span={18}>
                  <Input
                    allowClear
                    disabled={isFetchingData}
                    style={{ width: '100%' }}
                    placeholder="e.g. Bob/SD0001"
                    value={userSearchFieldValue}
                  />
                </Col>
                <Col offset={1}>
                  <Button
                    size="middle"
                    icon={<SearchOutlined />}
                    type="primary"
                    htmlType="submit"
                    onClick={onSearchButtonClicked}
                    disabled={isFetchingData}
                  />
                </Col>
              </Row>
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
