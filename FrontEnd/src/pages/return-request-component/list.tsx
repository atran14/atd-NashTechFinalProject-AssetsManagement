import { Button, Col, Row, Table } from 'antd'
import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import {CheckOutlined, CloseOutlined} from '@ant-design/icons'
import { ReturnRequestPagedListResponse } from '../../models/Pagination'
import { ReturnRequest, ReturnRequestState } from '../../models/ReturnRequest'
import { ReturnRequestService } from '../../services/ReturnRequestService'

const ADMIN = 'ADMIN'

interface SearchAction {
  action: 'filter' | 'search'
  query: Date | number | string
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
  let [filterSelected, setFilterSelected] = useState(false)
  let [latestSearchAction, setLatestSearchAction] = useState<SearchAction>()

  useEffect(() => {
    if (isAdminAuthorized) {
      ;(async () => {
        setIsFetchingData(true)
        let returnRequestService = ReturnRequestService.getInstance()
        let returnRequestsPagedResponse = await returnRequestService.getAll()

        setReturnRequestsPagedList(returnRequestsPagedResponse)
        setReturnRequestsList(returnRequestsPagedResponse.items)
        setLatestSearchAction({
          action: 'search',
          query: '',
        })

        console.log({ returnRequestsPagedResponse })

        setIsFetchingData(false)
      })()
    }
  }, [])

  const approveRequest = (id:number) => {}
  const denyRequest = (id:number) => {}

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
          if (a === null && b === null) return 0;
          else if (a === null) return -1;
          else if (b === null) return 1;
          else return a.localeCompare(b)

      },
      sortDirections: ['ascend', 'descend'],
    },
    {
      title: 'Returned Date',
      dataIndex: 'returnedDate',
      key: 'returnedDate',
      render: (text: any, record: ReturnRequest, index: number) => {
        return <div>{record.returnedDate && new Date(record.returnedDate).toLocaleDateString()}</div>
      },
      sorter: (a: string | null, b: string | null) => {
          if (a === null && b === null) return 0;
          else if (a === null) return -1;
          else if (b === null) return 1;
          else return new Date(a).getTime() - new Date(b).getTime();
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
                style={{backgroundColor: 'green'}}
                icon={<CheckOutlined style={{color: "white"}}/>}
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
    </>
  )
}
