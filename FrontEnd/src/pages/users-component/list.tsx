import {
  Button,
  Form,
  Input,
  message,
  Modal,
  Pagination,
  Popconfirm,
  Popover,
  Select,
  Table,
} from "antd";
import { useState } from "react";
import { useEffect } from "react";
import {
  PaginationParameters,
  UsersPagedListResponse,
} from "../../models/Pagination";
import { Location, User, UserGender, UserType } from "../../models/User";
import { UserService } from "../../services/UserService";
import {
  EditOutlined,
  InfoCircleTwoTone,
  SearchOutlined,
  UserDeleteOutlined,
  ExclamationCircleOutlined,
} from "@ant-design/icons";
import { Link, useHistory } from "react-router-dom";

const { Option } = Select;
const { confirm } = Modal;

let listAssignments = [
  {
    id: 1,
    assetId: 1,
    assignedByUserId: 1,
    assignedToUserId: 2,
    assignedDate: new Date(),
    state: 1,
    note: "abc",
  },
  {
    id: 2,
    assetId: 2,
    assignedByUserId: 1,
    assignedToUserId: 4,
    assignedDate: new Date(),
    state: 1,
    note: "abc",
  },
  {
    id: 3,
    assetId: 3,
    assignedByUserId: 2,
    assignedToUserId: 1,
    assignedDate: new Date(),
    state: 2,
    note: "abc",
  },
];

export function ListUsers() {
  let [isAdminAuthorized] = useState(
    sessionStorage.getItem("type") === "ADMIN"
  );
  let [isFetchingData, setIsFetchingData] = useState(false);
  let [assignments, setAssignments]: [any, any] = useState([]);
  let [usersPagedList, setUsersPagedList] = useState<UsersPagedListResponse>();
  let [usersList, setUsersList] = useState<User[]>([]);
  let history = useHistory();

  useEffect(() => {
    if (isAdminAuthorized) {
      (async () => {
        setIsFetchingData(true);
        let userServices = UserService.getInstance();
        let usersPagedResponse = await userServices.getUsers();

        setUsersPagedList(usersPagedResponse);
        setUsersList(usersPagedResponse.items);
        setIsFetchingData(false);
      })();
    } else {
      history.push("/401-access-denied");
    }
  }, []);

  useEffect(() => {
    setAssignments(listAssignments);
  }, []);

  function DisabledUser(id: number) {
    var count = 0;
    for (var i = 0; i < listAssignments.length; i++) {
      if (
        listAssignments[i].assignedToUserId === id &&
        listAssignments[i].state !== 2
      ) {
        count++;
      }
    }
    if (count === 0) {
      confirm({
        title: "Do you want to disable this user?",
        icon: <ExclamationCircleOutlined />,
        onOk() {
          let userServices = UserService.getInstance();
          try {
            userServices.disableUser(id);
            message.success("Disabled Successfully");
            setUsersList((userId: any[]) =>
              userId.filter((item) => item.id !== id)
            );
          } catch {
            message.success("Something went wrong");
          }
        },
        onCancel() {
          console.log("Cancel");
        },
      });
    }
    if (count > 0) {
      Modal.error({
        title:
          "There are valid assignments belonging to this user. Please close all assignments before disabling user.",
      });
    }
  }

  const onFinish = (values: any) => {
    if (usersPagedList !== undefined) {
      let newList: User[];

      if (values["searchMode"] === "fullName") {
        newList = usersPagedList.items.filter((u: User) => {
          let fullName = `${u.firstName} ${u.lastName}`;
          return fullName.startsWith(values["searchText"]);
        });
      } else {
        newList = usersPagedList.items.filter((u: User) => {
          return u.staffCode.startsWith(values["searchText"]);
        });
      }

      setUsersList(newList);
    }
  };

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
    );
  };

  const onPaginationConfigChanged = (page: number, pageSize?: number) => {
    (async () => {
      setIsFetchingData(true);
      let userService = UserService.getInstance();
      let parameters: PaginationParameters = {
        PageNumber: page,
        PageSize: pageSize ?? 10,
      };

      let usersPagedResponse = await userService.getUsers(parameters);
      setUsersPagedList(usersPagedResponse);
      setUsersList(usersPagedResponse.items);
      setIsFetchingData(false);
    })();
  };

  const columns: any = [
    {
      title: "Staff code",
      dataIndex: "staffCode",
      key: "staffCode",
      sorter: (a: User, b: User) => a.staffCode.localeCompare(b.staffCode),
      sortDirections: ["ascend", "descend"],
    },
    {
      title: "Full name",
      dataIndex: "fullName",
      key: "fullName",
      sorter: (a: User, b: User) => {
        let fullNameA = `${a.firstName} ${a.lastName}`;
        let fullNameB = `${b.firstName} ${b.lastName}`;
        return fullNameA.localeCompare(fullNameB);
      },
      render: (text: any, record: User, index: number) => {
        return <div>{`${record.firstName} ${record.lastName}`}</div>;
      },
      sortDirections: ["ascend", "descend"],
    },
    {
      title: "Username",
      dataIndex: "userName",
      key: "userName",
      sorter: (a: User, b: User) => a.userName.localeCompare(b.userName),
      sortDirections: ["ascend", "descend"],
    },
    {
      title: "Joined date",
      dataIndex: "joinedDate",
      key: "joinedDate",
      render: (text: any, record: User, index: number) => {
        return <div>{new Date(record.joinedDate).toLocaleDateString()}</div>;
      },
      sorter: (a: User, b: User) => {
        return (
          new Date(a.joinedDate).getTime() - new Date(b.joinedDate).getTime()
        );
      },
      sortDirections: ["ascend", "descend"],
    },
    {
      title: "Type",
      dataIndex: "type",
      key: "type",
      render: (text: any, record: User, index: number) => {
        return <div>{UserType[record.type]}</div>;
      },
      filters: [
        {
          text: "ADMIN",
          value: UserType.ADMIN,
        },
        {
          text: "USER",
          value: UserType.USER,
        },
      ],
      onFilter: (value: UserType, record: User) => {
        return record.type === value;
      },
      sorter: (a: User, b: User) => a.type - b.type,
      sortDirections: ["ascend", "descend"],
    },
    {
      title: "",
      dataIndex: "action",
      key: "action",
      render: (text: any, record: User) => {
        return (
          <>
            <Link to={`/users/update/${record.id}`}>
              <Button type="primary" icon={<EditOutlined />} />
            </Link>

            <Button
              danger
              type="primary"
              icon={<UserDeleteOutlined />}
              onClick={() => {
              DisabledUser(record.id);
              }}
            />

            <Popover
              trigger="click"
              title="User Detail"
              content={generateDetailedUserContent(record)}
            >
              <Button icon={<InfoCircleTwoTone />} />
            </Popover>
          </>
        );
      },
    },
  ];

  return (
    <>
      {isAdminAuthorized && usersPagedList !== undefined && (
        <>
          <Form
            onFinish={onFinish}
            initialValues={{
              searchMode: "fullName",
              searchText: "",
            }}
          >
            <Input.Group compact>
              <Form.Item name="searchMode">
                <Select disabled={isFetchingData}>
                  <Option value="fullName">Full name</Option>
                  <Option value="staffCode">Staff code</Option>
                </Select>
              </Form.Item>

              <Form.Item name="searchText">
                <Input
                  allowClear
                  disabled={isFetchingData}
                  style={{ width: "75%" }}
                  defaultValue="Nguyen Van A"
                />
              </Form.Item>

              <Form.Item>
                <Button
                  size="small"
                  icon={<SearchOutlined />}
                  type="primary"
                  htmlType="submit"
                  disabled={isFetchingData}
                />
              </Form.Item>
            </Input.Group>
          </Form>
          <Table
            dataSource={usersList}
            columns={columns}
            scroll={{ y: 400 }}
            pagination={false}
            loading={isFetchingData}
          />
          <Pagination
            disabled={isFetchingData}
            total={usersPagedList.totalCount}
            showTotal={(total: number) => `Total: ${total} result(s)`}
            pageSizeOptions={["10", "20", "50"]}
            showSizeChanger
            onChange={onPaginationConfigChanged}
          />
        </>
      )}
    </>
  );
}
