import {
  Button,
  Col,
  DatePicker,
  Form,
  Input,
  Pagination,
  Row,
  Select,
  Table,
} from 'antd'
import { useEffect, useState } from 'react'
import moment from 'moment'
import { Link, Redirect } from 'react-router-dom'
import {
  CheckOutlined,
  CloseOutlined,
  FilterFilled,
  SearchOutlined,
} from '@ant-design/icons'
import {
  PaginationParameters,
  ReturnRequestPagedListResponse,
} from '../../models/Pagination'
import { ReturnRequest, ReturnRequestFilterParameters, ReturnRequestState } from '../../models/ReturnRequest'
import { ReturnRequestService } from '../../services/ReturnRequestService'
import Search from 'antd/lib/transfer/search'

const ADMIN = 'ADMIN'

const { Option } = Select

interface SearchAction {
  type: 'search'
  query: string
}

interface FilterAction {
  type: 'filter'
  query: number | string
}

export function ListReturnRequests() {
  let [isAdminAuthorized] = useState(sessionStorage.getItem('type') === ADMIN)
  let [isFetchingData, setIsFetchingData] = useState(false)
  let [returnRequestsPagedList, setReturnRequestsPagedList] = useState<
    ReturnRequestPagedListResponse
  >()
  let [returnRequestsList, setReturnRequestsList] = useState<ReturnRequest[]>(
    [],
  )
  let [stateFilterSelected, setStateFilterSelected] = useState(false)
  let [dateFilterSelected, setDateFilterSelected] = useState(false)
  let [latestAction, setLatestAction] = useState<SearchAction | FilterAction>({
    type: 'search',
    query: '',
  })

  useEffect(() => {
    if (isAdminAuthorized) {
      ;(async () => {
        setIsFetchingData(true)
        let returnRequestService = ReturnRequestService.getInstance()
        let returnRequestsPagedResponse = await returnRequestService.getAll()

        console.log({returnRequestsPagedResponse})

        setReturnRequestsPagedList(returnRequestsPagedResponse)
        setReturnRequestsList(returnRequestsPagedResponse.items)
        setLatestAction({
          type: 'search',
          query: '',
        })
        setIsFetchingData(false)
      })()
    }
  }, [])

  const approveRequest = (rrId: number) => {}
  const denyRequest = (rrId: number) => {}

  const onRequestStateFilterButtonClicked = (values: any) => {
    ;(async () => {
      setIsFetchingData(true)

      let filteredState = values['filteredRequestState'] as number
      let requestService = ReturnRequestService.getInstance()
      let filteredRequestsPagedResponse = await requestService.filter({
        state: filteredState,
      })

      setReturnRequestsPagedList(filteredRequestsPagedResponse)
      setReturnRequestsList(filteredRequestsPagedResponse.items)
      setLatestAction({
        query: filteredState,
      } as FilterAction)

      setIsFetchingData(false)
    })()
  }

  const onReturnedDateFilterButtonClicked = (values: any) => {
    ;(async () => {
      setIsFetchingData(true)

      let filteredReturnedDate = moment(values['filteredReturnedDate']).format(
        'YYYY-MM-DD',
      )
      let requestService = ReturnRequestService.getInstance()
      let filteredRequestsPagedResponse = await requestService.filter({
        returnedDate: filteredReturnedDate,
      })

      setReturnRequestsPagedList(filteredRequestsPagedResponse)
      setReturnRequestsList(filteredRequestsPagedResponse.items)
      setLatestAction({
        query: filteredReturnedDate,
      } as FilterAction)

      setIsFetchingData(false)
    })()
  }

  const onSearchButtonClicked = (values: any) => {
    ;(async () => {
      setIsFetchingData(true)
      let { searchText } = values
      let requestService = ReturnRequestService.getInstance()
      let assignmentPagedResponse = await requestService.search(searchText)

      setLatestAction({
        query: searchText,
      } as SearchAction)
      setReturnRequestsPagedList(assignmentPagedResponse)
      setReturnRequestsList(assignmentPagedResponse.items)
      setIsFetchingData(false)
    })()
  }

  const onPaginationConfigChanged = (page: number, pageSize?: number) => {
    ;(async() => {
      setIsFetchingData(true)
      let requestService = ReturnRequestService.getInstance()
      let paginationParameters: PaginationParameters = {
        PageNumber: page,
        PageSize: pageSize ?? 10,
      }
  
      let returnRequestsPagedResponse: ReturnRequestPagedListResponse
      switch (latestAction.type) {
        case 'search':
          returnRequestsPagedResponse = await requestService.search(
            latestAction.query,
            paginationParameters
          )
          break
        case 'filter':
          let filterParameters : ReturnRequestFilterParameters = {
            returnedDate: typeof latestAction.query === "string" ? latestAction.query : undefined,
            state: typeof latestAction.query === "number" ? latestAction.query : undefined
          }

          returnRequestsPagedResponse = await requestService.filter(
            filterParameters,
            paginationParameters
          )
          break
      }
  
      setIsFetchingData(false)
    })()
  }

  const columns: any = [
    {
      title: 'No.',
      dataIndex: 'id',
      key: 'id',
      sorter: (a: number, b: number) => a - b,
      sortDirections: ['ascend', 'descend'],
    },
    {
      title: 'Asset Code',
      dataIndex: 'assetCode',
      key: 'assetCode',
      sorter: (a: string, b: string) => a.localeCompare(b),
      sortDirections: ['ascend', 'descend'],
    },
    {
      title: 'Asset Name',
      dataIndex: 'assetName',
      key: 'assetName',
      sorter: (a: string, b: string) => a.localeCompare(b),
      sortDirections: ['ascend', 'descend'],
    },
    {
      title: 'Requested By',
      dataIndex: 'requestedByUser',
      key: 'requestedByUser',
      sorter: (a: string, b: string) => a.localeCompare(b),
      sortDirections: ['ascend', 'descend'],
    },
    {
      title: 'Assigned Date',
      dataIndex: 'assignedDate',
      key: 'assignedDate',
      render: (text: any, record: ReturnRequest, index: number) => {
        return <div>{new Date(record.assignedDate).toLocaleDateString()}</div>
      },
      sorter: (a: Date, b: Date) => a.getTime() - b.getTime(),
      sortDirections: ['ascend', 'descend'],
    },
    {
      title: 'Accepted By',
      dataIndex: 'acceptedByUser',
      key: 'acceptedByUser',
      sorter: (a: string | null, b: string | null) => {
        if (a === null && b === null) return 0
        else if (a === null) return -1
        else if (b === null) return 1
        else return a.localeCompare(b)
      },
      sortDirections: ['ascend', 'descend'],
    },
    {
      title: 'Returned Date',
      dataIndex: 'returnedDate',
      key: 'returnedDate',
      render: (text: any, record: ReturnRequest, index: number) => {
        return (
          <div>
            {record.returnedDate &&
              new Date(record.returnedDate).toLocaleDateString()}
          </div>
        )
      },
      sorter: (a: string | null, b: string | null) => {
        if (a === null && b === null) return 0
        else if (a === null) return -1
        else if (b === null) return 1
        else return new Date(a).getTime() - new Date(b).getTime()
      },
      sortDirections: ['ascend', 'descend'],
    },
    {
      title: 'State',
      dataIndex: 'state',
      key: 'state',
      render: (text: any, record: ReturnRequest, index: number) => {
        return <div>{ReturnRequestState[record.state]}</div>
      },
      sorter: (a: ReturnRequestState, b: ReturnRequestState) => a - b,
      sortDirections: ['ascend', 'descend'],
    },
    {
      title: '',
      dataIndex: 'action',
      key: 'action',
      render: (text: any, record: ReturnRequest, index: number) => {
        return (
          <Row>
            <Col offset={1}>
              <Button
                style={{ backgroundColor: 'green' }}
                icon={<CheckOutlined style={{ color: 'white' }} />}
                onClick={() => approveRequest(record.id)}
              />
            </Col>
            <Col offset={1}>
              <Button
                danger
                type="primary"
                icon={<CloseOutlined />}
                onClick={() => denyRequest(record.id)}
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
      {isAdminAuthorized && returnRequestsPagedList !== undefined && (
        <>
          <Row>
            <Col span={6}>
              <Form onFinish={onRequestStateFilterButtonClicked}>
                <Row justify="start">
                  <Col span={15}>
                    <Form.Item
                      name="filteredRequestState"
                      className="no-margin-no-padding"
                    >
                      <Select
                        placeholder="Select request state"
                        style={{ width: '100%' }}
                        onSelect={() => setStateFilterSelected(true)}
                        disabled={isFetchingData}
                        allowClear
                      >
                        <Option key="waitingForReturning" value={0}>
                          Waiting for Returning
                        </Option>
                        <Option key="completed" value={1}>
                          Completed
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
                        disabled={!stateFilterSelected || isFetchingData}
                      />
                    </Form.Item>
                  </Col>
                </Row>
              </Form>
            </Col>

            <Col span={5}>
              <Form onFinish={onReturnedDateFilterButtonClicked}>
                <Row justify="start">
                  <Col span={15}>
                    <Form.Item
                      name="filteredReturnedDate"
                      className="no-margin-no-padding"
                    >
                      <DatePicker
                        picker="date"
                        style={{ width: '100%' }}
                        onChange={() => setDateFilterSelected(true)}
                        disabled={isFetchingData}
                        allowClear
                      />
                    </Form.Item>
                  </Col>

                  <Col offset={1}>
                    <Form.Item className="no-margin-no-padding">
                      <Button
                        size="middle"
                        icon={<FilterFilled />}
                        htmlType="submit"
                        disabled={!dateFilterSelected || isFetchingData}
                      />
                    </Form.Item>
                  </Col>
                </Row>
              </Form>
            </Col>

            <Col span={6} offset={7}>
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
                        placeholder="e.g. Laptop 01/LA000001"
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
          </Row>

          <Table
            style={{
              margin: '1.25em 0 1.25em 0',
            }}
            dataSource={returnRequestsList}
            columns={columns}
            scroll={{ y: 400 }}
            pagination={false}
            loading={isFetchingData}
          />

          <Row justify="center">
            <Col>
              <Pagination
                disabled={isFetchingData}
                total={returnRequestsPagedList.totalCount}
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
