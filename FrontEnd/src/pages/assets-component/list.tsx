import { Button, Col, Form, Input, message, Modal, Pagination, Popover, Row, Select, Table, } from 'antd'
import { useState, useEffect } from 'react'
import {
  PaginationParameters,
} from '../../models/Pagination'
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
import { Asset, AssetState } from '../../models/Asset'
import { AssetService } from '../../services/AssetService'
import { AssetsPagedListResponse } from '../../models/PagedListResponse'
import { AssetCategory } from '../../models/AssetCategory'
import { AssetCategoryService } from '../../services/AssetCategoryService'
import { Location, User } from '../../models/User'
import { Assignment, AssignmentState } from '../../models/Assignment'
import { UserService } from '../../services/UserService'

const { Option } = Select
const { confirm } = Modal
const { Column } = Table;

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

interface PassedInEditedAssetProps {
  editedAsset: Asset
}

interface SearchAction {
  action: 'filterByCategory' | 'filterByState' | 'search'
  query: number | string
}

const ADMIN = "Admin";

export function ListAssets({ editedAsset }: PassedInEditedAssetProps) {
  let [isAdminAuthorized] = useState(sessionStorage.getItem('type') === ADMIN);
  let [isFetchingData, setIsFetchingData] = useState(false);
  let [assetsPagedList, setAssetsPagedList] = useState<AssetsPagedListResponse>();
  let [assetsList, setAssetsList] = useState<Asset[]>([]);
  let [categoriesList, setCategoriesList] = useState<AssetCategory[]>([]);
  let [filterSelected, setFilterSelected] = useState(false);
  let [update, setUpdate] = useState(false);
  const [user, setUser] = useState<User[]>([])
  let [latestSearchAction, setLatestSearchAction] = useState<SearchAction>({
    action: 'search',
    query: '',
  });

  let userService = UserService.getInstance();
  useEffect(() => {
    ; (async () => {
      let listUser = await userService.getAllNoCondition();
      setUser(listUser);
    })()
  }, []);

  useEffect(() => {
    if (isAdminAuthorized) {
      ; (async () => {
        setIsFetchingData(true)
        let assetServices = AssetService.getInstance()
        let categoryServices = AssetCategoryService.getInstance()
        let assetsPagedResponse = await assetServices.getAssets()
        let categories = await categoryServices.getAll()

        setAssetsPagedList(assetsPagedResponse)
        setAssetsList(assetsPagedResponse.items)
        setCategoriesList(categories)
        setLatestSearchAction({
          action: 'search',
          query: '',
        })
        setIsFetchingData(false)
      })()
    }
  }, [editedAsset, update]);

  let disableDelete = (asset: Asset) => {
    if (asset.state === AssetState.ASSIGNED) return true;
    return false;
  };

  function deleteAsset(asset: Asset) {

    confirm({
      title: 'Do you want to delete this asset?',
      icon: <ExclamationCircleOutlined />,
      onOk() {
        if (asset.assignments != null) {
          Modal.error({
            title:
              'Cannot delete the asset because it belongs to one or more historical assignments. If the asset is not able to be used anymore, please update its state in Edit Asset page'
          });
        }
        else {
          let assetService = AssetService.getInstance();
          try {
            assetService.delete(asset.id);
            message.success('Delete Successfully');
            setUpdate(pre => !pre);
          } catch {
            message.success('Something went wrong');
          }
        };
      },
      onCancel() {
        console.log('Cancel');
      },
    })
  };

  const onSearchButtonClicked = (values: any) => {
    ; (async () => {
      setIsFetchingData(true)
      let { searchText } = values
      let assetService = AssetService.getInstance()
      let assetsPagedResponse: AssetsPagedListResponse

      if (searchText.length === 0) {
        assetsPagedResponse = await assetService.getAssets()
      } else {
        assetsPagedResponse = await assetService.searchAssets(searchText)
      }

      setLatestSearchAction({
        action: 'search',
        query: searchText as string,
      })
      setAssetsPagedList(assetsPagedResponse)
      setAssetsList(assetsPagedResponse.items)
      setIsFetchingData(false)
    })()
  }

  const onFilterByCategoryButtonClicked = (values: any) => {
    ; (async () => {
      setIsFetchingData(true)

      let { filteredAssetCategory } = values
      let assetService = AssetService.getInstance()
      let assetsPagedResponse: AssetsPagedListResponse = await assetService.filterByCategory(
        filteredAssetCategory as number,
      )

      setLatestSearchAction({
        action: 'filterByCategory',
        query: filteredAssetCategory as number,
      })
      setAssetsPagedList(assetsPagedResponse)
      setAssetsList(assetsPagedResponse.items)

      setIsFetchingData(false)
    })()
  }

  const onFilterByStateButtonClicked = (values: any) => {
    ; (async () => {
      setIsFetchingData(true)

      let { filteredAssetState } = values
      let assetService = AssetService.getInstance()
      let assetsPagedResponse: AssetsPagedListResponse = await assetService.filterByState(
        filteredAssetState as number,
      )

      setLatestSearchAction({
        action: 'filterByState',
        query: filteredAssetState as number,
      })
      setAssetsPagedList(assetsPagedResponse)
      setAssetsList(assetsPagedResponse.items)

      setIsFetchingData(false)
    })()
  }

  async function generateDetailedAssetContent(record: Asset) {
    Modal.info({
      title: `Detail of Assets No. ${record.id}`,
      width:650,
      content: (
        <div>
          <p>Asset Code : {record.assetCode}</p>
          <p>Asset Name : {record.assetName}</p>
          <p>Category : {record.category.categoryName}</p>
          <p>Installed Date : {record.installedDate}</p>
          <p>Specification : {record.specification}</p>
          <p>Location : {Location[record.location]}</p>
          <p>State : {AssetState[record.state]}</p>
          <h5>Historical Assignments</h5>
          <Table key="table" dataSource={record.assignments} pagination={{ defaultPageSize: 7 }}>
            <Column
              align='center'
              title="Assigned To"
              dataIndex="assignedTo"
              key="assignedTo"
              render={(text, record: Assignment, index) => (
                <div>{user.map((c: User) => {
                  if (c.id === record.assignedToUserId) return c.userName
                })}</div>
              )}
            />
            <Column 
            align='center' 
            title="Assigned By" 
            dataIndex="assignedBy" 
            key="assignedBy" 
            render={(text, record: Assignment, index) => (
              <div>{`${record.assignedByUser.firstName} ${record.assignedByUser.lastName}`}</div>
            )}/>
            <Column
              align='center'
              title="Assigned Date"
              dataIndex="assignedDate"
              key="assignedDate"
              render={(text, record: Assignment, index) => (
                <div>{record.assignedDate}</div>
              )}
            />
            <Column
              align='center'
              title="State"
              dataIndex="state"
              key="state"
              render={(text, record: Assignment, index) => (
                <div>{AssignmentState[record.state]}</div>
              )}
            />
            <Column
              align='center'
              title="Note"
              dataIndex="note"
              key="note"
              render={(text, record: Assignment, index) => (
                <div>{record.note}</div>
              )}
            />
          </Table>
          {/* <table className="table">
            <thead>
              <tr>
                <th scope="col">Assigned By</th>
                <th scope="col">Assigned To</th>
                <th scope="col">Assigned Date</th>
                <th scope="col">State</th>
                <th scope="col">Note</th>
              </tr>
            </thead>
            {record.assignments.map((assignment: Assignment) => (
              <tbody key={i}>
                <tr>
                  <th>{i++}</th>
                  <th>{`${assignment.assignedByUser.firstName} ${assignment.assignedByUser.lastName}`}</th>
                  <th>{user.map((c: User) => {
                    if (c.id === assignment.assignedToUserId) return c.userName
                  })}</th>
                  <th>{assignment.assignedDate}</th>
                  <th>{AssetState[assignment.state]}</th>
                  <th>{assignment.note}</th>
                </tr>
              </tbody>
            ))}
          </table> */}
        </div>
      ),
      onOk() { },
    })
  }

  const onPaginationConfigChanged = (page: number, pageSize?: number) => {
    ; (async () => {
      setIsFetchingData(true)
      let assetService = AssetService.getInstance()
      let parameters: PaginationParameters = {
        PageNumber: page,
        PageSize: pageSize ?? 10,
      }

      let assetsPagedResponse: AssetsPagedListResponse
      switch (latestSearchAction.action) {
        case 'search':
          let searchQuery = latestSearchAction.query as string
          if (searchQuery.length === 0) {
            assetsPagedResponse = await assetService.getAssets(parameters)
          } else {
            assetsPagedResponse = await assetService.searchAssets(
              searchQuery,
              parameters,
            )
          }
          break
        case 'filterByCategory':
          let filterByCategoryQuery = latestSearchAction.query as number
          assetsPagedResponse = await assetService.filterByCategory(
            filterByCategoryQuery,
            parameters,
          )
          break
        case 'filterByState':
          let filterByStateQuery = latestSearchAction.query as number
          assetsPagedResponse = await assetService.filterByState(
            filterByStateQuery,
            parameters,
          )
          break
      }

      setAssetsPagedList(assetsPagedResponse)
      setAssetsList(assetsPagedResponse.items)
      setIsFetchingData(false)
    })()
  }

  const columns: any = [
    {
      title: 'Asset Code',
      dataIndex: 'assetCode',
      key: 'assetCode',
      sorter: (a: Asset, b: Asset) => a.assetCode.localeCompare(b.assetCode),
      sortDirections: ['ascend', 'descend'],
    },
    {
      title: 'Asset Name',
      dataIndex: 'assetName',
      key: 'assetName',
      sorter: (a: Asset, b: Asset) => a.assetName.localeCompare(b.assetName),
      sortDirections: ['ascend', 'descend'],
    },
    {
      title: 'Category',
      dataIndex: 'category',
      key: 'category',
      render: (text: any, record: Asset, index: number) => {
        return <div>{record.category.categoryName}</div>
      },
      sorter: (a: Asset, b: Asset) => a.category.categoryName.localeCompare(b.category.categoryName),
      sortDirections: ['ascend', 'descend'],
    },
    {
      title: 'State',
      dataIndex: 'state',
      key: 'state',
      render: (text: any, record: Asset, index: number) => {
        return <div>{AssetState[record.state]}</div>
      },
      sorter: (a: Asset, b: Asset) => a.state - b.state,
      sortDirections: ['ascend', 'descend'],
    },
    {
      title: '',
      dataIndex: 'action',
      key: 'action',
      render: (text: any, record: Asset, index: number) => {
        return (
          <Row>
            <Col offset={1}>
              <Button
                ghost
                type="primary"
                icon={<InfoCircleOutlined />}
                onClick={() => generateDetailedAssetContent(record)}
              />
            </Col>
            <Col offset={1}>
              <Link to={`/assets/update/${record.id}`}>
                <Button type="primary" icon={<EditOutlined />} />
              </Link>
            </Col>
            <Col offset={1}>
              <Button
                danger
                type="primary"
                icon={<DeleteOutlined />}
                disabled={disableDelete(record)}
                onClick={() => deleteAsset(record)}
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
      {isAdminAuthorized && assetsPagedList !== undefined && (
        <>
          <Row>
            <Col span={5}>
              <Form onFinish={onFilterByStateButtonClicked}>
                <Row justify="start">
                  <Col span={15}>
                    <Form.Item
                      name="filteredAssetState"
                      className="no-margin-no-padding"
                    >
                      <Select
                        placeholder="State"
                        style={{ width: '100%' }}
                        onSelect={() => setFilterSelected(true)}
                        disabled={isFetchingData}
                      >
                        <Option key="availabe" value={AssetState.AVAILABLE}>
                          Available
                        </Option>
                        <Option key="notAvailable" value={1}>
                          Not Available
                        </Option>
                        <Option key="assigned" value={2}>
                          Assigned
                        </Option>
                        <Option key="waitingForRecycling" value={3}>
                          Waiting For Recycling
                        </Option>
                        <Option key="recycled" value={4}>
                          Recycled
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
              <Form onFinish={onFilterByCategoryButtonClicked}>
                <Row justify="start">
                  <Col span={15}>
                    <Form.Item
                      name="filteredAssetCategory"
                      className="no-margin-no-padding"
                    >
                      <Select
                        placeholder="Category"
                        style={{ width: '100%' }}
                        onSelect={() => setFilterSelected(true)}
                        disabled={isFetchingData}
                      >
                        {categoriesList &&
                          categoriesList.length > 0 &&
                          categoriesList.map((category: AssetCategory) =>
                          (
                            <Option key={category.id} value={category.id}>{category.categoryName}</Option>
                          ))}
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
              <Link to="/assets/create">
                <Button
                  style={{
                    width: '100%',
                    backgroundColor: '#e9424d',
                    border: '#e9424d',
                  }}
                  type="primary"
                  icon={<UserAddOutlined />}
                >
                  Create new asset
                </Button>
              </Link>
            </Col>
          </Row>

          <Table
            style={{
              margin: '1.25em 0 1.25em 0',
            }}
            dataSource={assetsList}
            columns={columns}
            scroll={{ y: 400 }}
            pagination={false}
            loading={isFetchingData}
          />

          <Row justify="center">
            <Col>
              <Pagination
                disabled={isFetchingData}
                total={assetsPagedList.totalCount}
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
