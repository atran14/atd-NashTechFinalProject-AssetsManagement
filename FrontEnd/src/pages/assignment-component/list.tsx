import {
  Button,
  Col,
  DatePicker,
  Form,
  Input,
  message,
  Modal,
  Pagination,
  Row,
  Select,
  Table,
} from 'antd'
import {
  EditOutlined,
  UserAddOutlined,
  SearchOutlined,
  DeleteOutlined,
  FilterFilled,
  InfoCircleOutlined,
  RedoOutlined,
  ExclamationCircleOutlined,
} from '@ant-design/icons'
import React, { useEffect, useState } from 'react'
import { Assignment, AssignmentState } from '../../models/Assignment'
import {
  AssignmentPagedListResponse,
  PaginationParameters,
  ReturnRequestPagedListResponse,
} from '../../models/Pagination'
import { AssignmentsService } from '../../services/AssignmentService'
import { Asset } from '../../models/Asset'
import { User } from '../../models/User'
import { UserService } from '../../services/UserService'
import { AssetService } from '../../services/AssetService'
import { Link } from 'react-router-dom'
import moment from 'moment'
import { ReturnRequestService } from '../../services/ReturnRequestService'

interface SearchAction {
  action: 'filter' | 'search' | 'filterDate'
  query: number | string | Date
}
const { Option } = Select
const { confirm } = Modal

const dateFormat = 'YYYY-MM-DD'

export function ListAssignments() {
  let [isFetchingData, setIsFetchingData] = useState(false)
  let [assignmentPagedList, setAssignmentPagedList] = useState<
    AssignmentPagedListResponse
  >()
  let [assignmentList, setAssignmentList] = useState<Assignment[]>([])
  const [asset, setAsset] = useState<Asset[]>([])
  const [user, setUser] = useState<User[]>([])
  let [filterSelected, setFilterSelected] = useState(false)
  let [filterDateSelected, setFilterDateSelected] = useState(false)
  let [latestSearchAction, setLatestSearchAction] = useState<SearchAction>({
    action: 'search',
    query: '',
  })
  let [isDisabledStates, setIsDisabledStates] = useState<boolean[]>([])

  let userService = UserService.getInstance()
  let assignmentService = AssignmentsService.getInstance()
  let returnRequestService = ReturnRequestService.getInstance()

  useEffect(() => {
    ;(async () => {
      let listUser = await userService.getAllNoCondition()
      setUser(listUser)
    })()
  }, [])

  useEffect(() => {
    ;(async () => {
      setIsFetchingData(true)
      let assignmentServices = AssignmentsService.getInstance()
      let assignmentPagedResponse = await assignmentServices.getAssignments()
      let disabledButtonStates: boolean[] = []

      for (const element of assignmentPagedResponse.items) {
        let associatedRRCount = await returnRequestService.getAssociatedCount(
          element.asset.assetCode
        )
        let isWaitingForAdminDecision =
          associatedRRCount > 0
        let isAcceptedState = element.state === AssignmentState.Accepted
        if (!isAcceptedState) {
          disabledButtonStates.push(true)
        } else {
          disabledButtonStates.push(isWaitingForAdminDecision)
        }
      }

      setIsDisabledStates(disabledButtonStates)
      setAssignmentPagedList(assignmentPagedResponse)
      setAssignmentList(assignmentPagedResponse.items)
      setIsFetchingData(false)
    })()
  }, [])

  async function detailAssignment(id: number) {
    let assignment = await assignmentService.getAssignment(id)
    Modal.info({
      title: `Detail of Assignment No. ${assignment.id}`,
      content: (
        <div>
          <p>Asset Code : {assignment.asset.assetCode}</p>
          <p>Asset Name : {assignment.asset.assetName}</p>
          <p>
            Assigned to :
            {user.map((c: User) => {
              if (c.id === assignment.assignedToUserId) return c.userName
            })}
          </p>
          <p>Assigned by : {assignment.assignedByUser.userName}</p>
          <p>Assigned Date : {assignment.assignedDate}</p>
          <p>State : {AssignmentState[assignment.state]}</p>
          <p>Note : {assignment.note}</p>
        </div>
      ),
      onOk() {},
    })
  }

  const createReturnRequest = (index: number, record: Assignment) => {
    confirm({
      title: 'Do you want to return this asset?',
      icon: <ExclamationCircleOutlined />,
      onOk: async () => {
        await returnRequestService.create({
          assignmentId: record.id,
        })

        let newIsDisabledStates = [...isDisabledStates]
        newIsDisabledStates[index] = !newIsDisabledStates[index]
        setIsDisabledStates(newIsDisabledStates)
      },
    })
  }

  const columns: any = [
    {
      title: 'No.',
      dataIndex: 'id',
      key: 'id',
      sorter: (a: Assignment, b: Assignment) => a.id - b.id,
      sortDirections: ['ascend', 'descend'],
      width: 60,
    },
    {
      title: 'Asset Code',
      dataIndex: 'assetName',
      key: 'assetName',
      sorter: (a: Assignment, b: Assignment) => {
        a.asset.assetCode.localeCompare(b.asset.assetCode)
      },
      render: (text: any, record: Assignment, index: number) => {
        return <div>{record.asset.assetCode}</div>
      },
      sortDirections: ['ascend', 'descend'],
    },
    {
      title: 'Asset Name',
      dataIndex: 'assetName',
      key: 'assetName',
      sorter: (a: Assignment, b: Assignment) =>
        a.asset.assetName.localeCompare(b.asset.assetName),
      render: (text: any, record: Assignment, index: number) => {
        return <div>{record.asset.assetName}</div>
      },
      sortDirections: ['ascend', 'descend'],
    },
    {
      title: 'Assigned to',
      dataIndex: 'assignedToUserId',
      key: 'assignedToUserId',
      sorter: (a: Assignment, b: Assignment) => {
        let userA = user.map((x: User) => {
          if (x.id === a.assignedToUserId) return x.userName
        })
        let userB = user.map((x: User) => {
          if (x.id === b.assignedToUserId) return x.userName
        })
        return userA.toString().localeCompare(userB.toString())
      },
      render: (text: any, record: Assignment, index: number) => {
        return (
          <div>
            {user.map((c: User) => {
              if (c.id === record.assignedToUserId) return c.userName
            })}
          </div>
        )
      },
      sortDirections: ['ascend', 'descend'],
    },
    {
      title: 'Assigned by',
      dataIndex: 'assignedByUserId',
      key: 'assignedByUserId',
      sorter: (a: Assignment, b: Assignment) => {
        a.assignedByUser.userName.localeCompare(b.assignedByUser.userName)
      },
      render: (text: any, record: Assignment, index: number) => {
        return <div>{record.assignedByUser.userName}</div>
      },
      sortDirections: ['ascend', 'descend'],
    },
    {
      title: 'Assigned Date',
      dataIndex: 'assignedDate',
      key: 'assignedDate',
      render: (text: any, record: Assignment, index: number) => {
        return <div>{new Date(record.assignedDate).toLocaleDateString()}</div>
      },
      sorter: (a: Assignment, b: Assignment) => {
        return (
          new Date(a.assignedDate).getTime() -
          new Date(b.assignedDate).getTime()
        )
      },
      sortDirections: ['ascend', 'descend'],
    },
    {
      title: 'State',
      dataIndex: 'state',
      key: 'state',
      render: (text: any, record: Assignment, index: number) => {
        return <div>{AssignmentState[record.state]}</div>
      },
      sorter: (a: Assignment, b: Assignment) => a.state - b.state,
      sortDirections: ['ascend', 'descend'],
    },
    {
      title: '',
      dataIndex: 'action',
      key: 'action',
      render: (text: any, record: Assignment, index: number) => {
        return (
          <Row onClick={(e) => e.stopPropagation()}>
            <Col>
              <Link to={`/assignments/update/${record.id}`}>
                <Button type="primary" icon={<EditOutlined />} />
              </Link>
            </Col>
            <Col>
              <Button
                ghost
                type="primary"
                icon={<InfoCircleOutlined />}
                onClick={() => detailAssignment(record.id)}
              />
            </Col>
            <Col>
              <Button
                danger
                type="primary"
                icon={<DeleteOutlined />}
                //onClick={() => disabledUser(record.id)}
              />
            </Col>
            <Button
              ghost
              type="primary"
              icon={<RedoOutlined />}
              disabled={isDisabledStates[index]}
              onClick={() => createReturnRequest(index, record)}
            />
            <Col></Col>
          </Row>
        )
      },
    },
  ]

  const onPaginationConfigChanged = (page: number, pageSize?: number) => {
    ;(async () => {
      setIsFetchingData(true)
      let parameters: PaginationParameters = {
        PageNumber: page,
        PageSize: pageSize ?? 10,
      }

      let assignmentPagedResponse: AssignmentPagedListResponse
      switch (latestSearchAction.action) {
        case 'search':
          let searchQuery = latestSearchAction.query as string
          if (searchQuery.length === 0) {
            assignmentPagedResponse = await assignmentService.getAssignments(
              parameters,
            )
          } else {
            assignmentPagedResponse = await assignmentService.searchAssignment(
              searchQuery,
              parameters,
            )
          }
          break
        case 'filter':
          let filterQuery = latestSearchAction.query as number
          assignmentPagedResponse = await assignmentService.filterByState(
            filterQuery,
            parameters,
          )
          break

        case 'filterDate':
          let filterDateQuery = latestSearchAction.query as Date
          assignmentPagedResponse = await assignmentService.filterByDate(
            filterDateQuery,
            parameters,
          )
          break
      }

      setAssignmentPagedList(assignmentPagedResponse)
      setAssignmentList(assignmentPagedResponse.items)
      setIsFetchingData(false)
    })()
  }

  const onSearchButtonClicked = (values: any) => {
    ;(async () => {
      setIsFetchingData(true)
      let { searchText } = values
      let assignmentService = AssignmentsService.getInstance()
      let assignmentPagedResponse: AssignmentPagedListResponse

      if (searchText.length === 0) {
        assignmentPagedResponse = await assignmentService.getAssignments()
      } else {
        assignmentPagedResponse = await assignmentService.searchAssignment(
          searchText,
        )
      }

      setLatestSearchAction({
        action: 'search',
        query: searchText as string,
      })
      setAssignmentPagedList(assignmentPagedResponse)
      setAssignmentList(assignmentPagedResponse.items)
      setIsFetchingData(false)
    })()
  }

  const onFilterStateButtonClicked = (values: any) => {
    ;(async () => {
      setIsFetchingData(true)

      let { filteredAssignmentByState } = values
      let assignmentService = AssignmentsService.getInstance()
      let assignmentPagedResponse: AssignmentPagedListResponse = await assignmentService.filterByState(
        filteredAssignmentByState as number,
      )

      setLatestSearchAction({
        action: 'filter',
        query: filteredAssignmentByState as number,
      })
      setAssignmentPagedList(assignmentPagedResponse)
      setAssignmentList(assignmentPagedResponse.items)
      setIsFetchingData(false)
    })()
  }

  const onFilterDateButtonClicked = (values: any) => {
    ;(async () => {
      setIsFetchingData(true)

      let { filteredAssignmentByDate } = values
      if (filteredAssignmentByDate === null) {
        message.error('Date is empty')
        setIsFetchingData(false)
      } else {
        let assignmentService = AssignmentsService.getInstance()
        let assignmentPagedResponse: AssignmentPagedListResponse = await assignmentService.filterByDate(
          filteredAssignmentByDate as Date,
        )

        setLatestSearchAction({
          action: 'filterDate',
          query: filteredAssignmentByDate as Date,
        })
        setAssignmentPagedList(assignmentPagedResponse)
        setAssignmentList(assignmentPagedResponse.items)
        setIsFetchingData(false)
      }
    })()
  }

  return (
    <>
      {assignmentPagedList !== undefined && (
        <>
          <Row>
            <Col span={5}>
              <Form onFinish={onFilterStateButtonClicked}>
                <Row justify="start">
                  <Col span={15}>
                    <Form.Item
                      name="filteredAssignmentByState"
                      className="no-margin-no-padding"
                    >
                      <Select
                        placeholder="Select assignment state"
                        style={{ width: '100%' }}
                        onSelect={() => setFilterSelected(true)}
                        disabled={isFetchingData}
                      >
                        <Option key="waitingForAcceptance" value={0}>
                          WaitingForAcceptance,
                        </Option>
                        <Option key="Accepted" value={1}>
                          Accepted
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

            <Col span={5}>
              <Form onFinish={onFilterDateButtonClicked}>
                <Row justify="start">
                  <Col span={15}>
                    <Form.Item
                      name="filteredAssignmentByDate"
                      className="no-margin-no-padding"
                    >
                      <DatePicker
                        onChange={() => setFilterDateSelected(true)}
                        format={dateFormat}
                      />
                    </Form.Item>
                  </Col>

                  <Col offset={1}>
                    <Form.Item className="no-margin-no-padding">
                      <Button
                        size="middle"
                        icon={<FilterFilled />}
                        htmlType="submit"
                        disabled={!filterDateSelected || isFetchingData}
                      />
                    </Form.Item>
                  </Col>
                </Row>
              </Form>
            </Col>

            <Col span={4} offset={5}>
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
                        placeholder="e.g. Laptop/LA0001"
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
              <Link to="/assignments/create">
                <Button
                  style={{
                    width: '100%',
                    backgroundColor: '#e9424d',
                    border: '#e9424d',
                  }}
                  type="primary"
                  icon={<UserAddOutlined />}
                >
                  Create new assignment
                </Button>
              </Link>
            </Col>
          </Row>

          <Table
            style={{
              margin: '1.25em 0 1.25em 0',
            }}
            dataSource={assignmentList}
            columns={columns}
            scroll={{ y: 400 }}
            pagination={false}
            loading={isFetchingData}
          />
          <Row justify="center">
            <Col>
              <Pagination
                disabled={isFetchingData}
                total={assignmentPagedList.totalCount}
                onChange={onPaginationConfigChanged}
                showTotal={(total: number) => `Total: ${total} result(s)`}
                pageSizeOptions={['10', '20', '50']}
                showSizeChanger
              />
            </Col>
          </Row>
        </>
      )}
    </>
  )
}
