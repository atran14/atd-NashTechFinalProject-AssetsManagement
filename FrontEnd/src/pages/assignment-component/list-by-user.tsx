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
  CheckOutlined,
  CloseOutlined,
  UndoOutlined,
  } from "@ant-design/icons";
import { ReturnRequestService } from "../../services/ReturnRequestService";

const { confirm } = Modal


export enum ResponeAction {
  NoAction,
  Accept,
  Decline,
  UndoRespone,
}

export function ListAssignmentsForEachUser() {
  let [assignmentList, setAssignmentList] = useState<Assignment[]>([]);
  const [user, setUser] = useState<User[]>([]);
  let [isDisabledStates, setIsDisabledStates] = useState<boolean[]>([])

  let userService = UserService.getInstance();
  let assignmentService = AssignmentsService.getInstance();
  let returnRequestService = ReturnRequestService.getInstance();


  //Respone to Assignment
  const [visible, setVisible] = React.useState(false);
  const [confirmLoading, setConfirmLoading] = React.useState(false);
  const [modalText, setModalText] = React.useState("");
  const [responeAction, setResponeAction] = React.useState(
    ResponeAction.NoAction
  );
  const [assignmentId, setAssignmentId] = React.useState(0);
  const [update, setUpdate] = useState(false);

  const showModal = () => {
    setVisible(true);
  };

  const handleOk = () => {
    if (responeAction == ResponeAction.NoAction) {
      setVisible(false);
      setConfirmLoading(false);
    }
    if (responeAction == ResponeAction.Accept) {
      setConfirmLoading(true);
      assignmentService.acceptAssignment(assignmentId);
    }
    if (responeAction == ResponeAction.Decline) {
      setConfirmLoading(true);
      assignmentService.declineAssignment(assignmentId);
    }
    if (responeAction == ResponeAction.UndoRespone) {
      setConfirmLoading(true);
      assignmentService.undoResponeAssignment(assignmentId);
    }
    setConfirmLoading(false);
    setVisible(false);
    setAssignmentId(0);
    setResponeAction(ResponeAction.NoAction);
    window.location.reload();
  };

  const handleCancel = () => {
    console.log("Clicked cancel button");
    setVisible(false);
    setAssignmentId(0);
    setResponeAction(ResponeAction.NoAction);
  };

  //


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
  }, [update]);

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

   //Respone to assignment
   const acceptAssignment = (id: number) => {
    setAssignmentId(id);
    setResponeAction(ResponeAction.Accept);
    setModalText("Do you want to accept to this assignment?");
    showModal();
    setUpdate((pre) => !pre);
  };

  const declineAssignment = (id: number) => {
    setAssignmentId(id);
    setResponeAction(ResponeAction.Decline);
    setModalText("Do you want to decline to this assignment?");
    showModal();
    setUpdate((pre) => !pre);
  };

  const undoResponeAssignment = (id: number) => {
    setAssignmentId(id);
    setResponeAction(ResponeAction.UndoRespone);
    setModalText("Do you want to respone to this assignment?");
    showModal();
    setUpdate((pre) => !pre);
  };

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
            <Col>
              <Button
                ghost
                type="link"
                icon={<CheckOutlined />}
                disabled={record.state != AssignmentState.WaitingForAcceptance}
                onClick={() => acceptAssignment(record.id)}
              />
            </Col>
            <Col>
              <Button
                ghost
                type="link"
                icon={<CloseOutlined />}
                disabled={record.state != AssignmentState.WaitingForAcceptance}
                onClick={() => declineAssignment(record.id)}
              />
            </Col>
            <Col>
              <Button
                ghost
                type="link"
                icon={<UndoOutlined rotate={180} />}
                disabled={record.state == AssignmentState.WaitingForAcceptance}
                onClick={() => undoResponeAssignment(record.id)}
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
        <Modal
        title="Are you sure?"
        visible={visible}
        onOk={handleOk}
        confirmLoading={confirmLoading}
        onCancel={handleCancel}
      >
        <p>{modalText}</p>
      </Modal>
    </>
  );
}
