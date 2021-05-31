import React, { useEffect, useState } from "react";
import { Button, Col, Modal, Row, Table } from "antd";
import { Assignment, AssignmentState } from "../../models/Assignment";
import { User } from "../../models/User";
import { AssignmentsService } from "../../services/AssignmentService";
import { UserService } from "../../services/UserService";
import {
    InfoCircleOutlined,
    RedoOutlined,
    ExclamationCircleOutlined,
  } from "@ant-design/icons";
import { ReturnRequestService } from "../../services/ReturnRequestService";

const { confirm } = Modal

export function ListAssignmentsForEachUser() {
  let [assignmentList, setAssignmentList] = useState<Assignment[]>([]);
  const [user, setUser] = useState<User[]>([]);
  let [isDisabledStates, setIsDisabledStates] = useState<boolean[]>([])

  let userService = UserService.getInstance();
  let assignmentService = AssignmentsService.getInstance();
  let returnRequestService = ReturnRequestService.getInstance();

  useEffect(() => {
    (async () => {
      let listUser = await userService.getAllNoCondition();
      setUser(listUser);
    })();
  }, []);

  useEffect(() => {
    (async () => {
      let listAssignments = await assignmentService.getAllForEachUser(
        JSON.parse(sessionStorage.getItem("id")!)
      );
      let disabledButtonStates: boolean[] = []

      for (const element of listAssignments) {
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
      setAssignmentList(listAssignments);
    })();
  }, []);

  async function detailAssignment(id: number) {
    let assignment = await assignmentService.getAssignment(id);
    Modal.info({
      title: `Detail of Assignment No. ${assignment.id}`,
      content: (
        <div>
          <p>Asset Code : {assignment.asset.assetCode}</p>
          <p>Asset Name : {assignment.asset.assetName}</p>
          <p>
            Assigned to :
            {user.map((c: User) => {
              if (c.id === assignment.assignedToUserId) return c.userName;
            })}
          </p>
          <p>Assigned by : {assignment.assignedByUser.userName}</p>
          <p>Assigned Date : {assignment.assignedDate}</p>
          <p>State : {AssignmentState[assignment.state]}</p>
          <p>Note : {assignment.note}</p>
        </div>
      ),
      onOk() {},
    });
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
      title: "Asset Code",
      dataIndex: "assetCode",
      key: "assetCode",
      sorter: (a: Assignment, b: Assignment) =>
        a.asset.assetCode.localeCompare(b.asset.assetCode),
      render: (text: any, record: Assignment, index: number) => {
        return <div>{record.asset.assetCode}</div>;
      },
      sortDirections: ["ascend", "descend"],
    },
    {
      title: "Asset Name",
      dataIndex: "assetName",
      key: "assetName",
      sorter: (a: Assignment, b: Assignment) =>
        a.asset.assetName.localeCompare(b.asset.assetName),
      render: (text: any, record: Assignment, index: number) => {
        return <div>{record.asset.assetName}</div>;
      },
      sortDirections: ["ascend", "descend"],
    },
    {
      title: "Assigned to",
      dataIndex: "assignedToUserId",
      key: "assignedToUserId",
      sorter: (a: Assignment, b: Assignment) => {
        let userA = user.map((x: User) => {
          if (x.id === a.assignedToUserId) return x.userName;
        });
        let userB = user.map((x: User) => {
          if (x.id === b.assignedToUserId) return x.userName;
        });
        return userA.toString().localeCompare(userB.toString());
      },
      render: (text: any, record: Assignment, index: number) => {
        return (
          <div>
            {user.map((c: User) => {
              if (c.id === record.assignedToUserId) return c.userName;
            })}
          </div>
        );
      },
      sortDirections: ["ascend", "descend"],
    },
    {
      title: "Assigned by",
      dataIndex: "assignedByUserId",
      key: "assignedByUserId",
      sorter: (a: Assignment, b: Assignment) => {
        a.assignedByUser.userName.localeCompare(b.assignedByUser.userName);
      },
      render: (text: any, record: Assignment, index: number) => {
        return <div>{record.assignedByUser.userName}</div>;
      },
      sortDirections: ["ascend", "descend"],
    },
    {
      title: "Assigned Date",
      dataIndex: "assignedDate",
      key: "assignedDate",
      render: (text: any, record: Assignment, index: number) => {
        return <div>{new Date(record.assignedDate).toLocaleDateString()}</div>;
      },
      sorter: (a: Assignment, b: Assignment) => {
        return (
          new Date(a.assignedDate).getTime() -
          new Date(b.assignedDate).getTime()
        );
      },
      sortDirections: ["ascend", "descend"],
    },
    {
      title: "State",
      dataIndex: "state",
      key: "state",
      render: (text: any, record: Assignment, index: number) => {
        return <div>{AssignmentState[record.state]}</div>;
      },
      sorter: (a: Assignment, b: Assignment) => a.state - b.state,
      sortDirections: ["ascend", "descend"],
    },
    {
        title: "",
        dataIndex: "action",
        key: "action",
        render: (text: any, record: Assignment, index: number) => {
          return (
            <Row >
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
                ghost
                type="primary"
                icon={<RedoOutlined />}
                disabled={isDisabledStates[index]}
                onClick={() => createReturnRequest(index, record)}
              />
            </Col>
            </Row>
          );
        },
    }
  ];
  return (
    <>
      <Table
        style={{
          margin: "1.25em 0 1.25em 0",
        }}
        dataSource={assignmentList}
        columns={columns}
        scroll={{ y: 400 }}
        pagination={false}
      />
    </>
  );
}
