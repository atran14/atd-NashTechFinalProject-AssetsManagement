import {
  Button,
  Col,
  DatePicker,
  Form,
  Input,
  message,
  Radio,
  Select,
  Space,
} from "antd";
import { useForm } from "antd/lib/form/Form";
import React, { useEffect, useState } from "react";
import { useHistory, useParams } from "react-router-dom";
import { EditUserModel, UserGender, UserType } from "../../models/User";
import moment from "moment";
import { UserService } from "../../services/UserService";

export function UpdateUser() {
  const layout = {
    labelCol: {
      span: 14,
      offset: 3,
      pull: 9,
    },
    wrapperCol: {
      span: 16,
      pull: 9,
    },
  }
  const tailLayout = {
    wrapperCol: {},
  }

  const { Option } = Select

  const { userId } = useParams<any>();
  const [dob, setDob] = useState<Date>();

  const today = new Date();

  const validateDateOfBirth = async (rule: any, value: any, callback: any) => {
    if (value && value._d.getFullYear() > (today.getFullYear() - 18)) {
      throw new Error("User is under 18. Please select a different date!");
    }
    setDob(value._d);

  };

  const validateJoinedDate = async (rule: any, value: any, callback: any) => {
    if (value && (value._d.getDay() === 0 || value._d.getDay() === 6)) {

      throw new Error("Joined date is Saturday or Sunday. Please select a different date!");
    }
    else if (value._d < moment(dob)) {
      throw new Error("Joined date is not later than Date of Birth. Please select a different date!");
    }
  };

  let history = useHistory()

  const dateFormat = 'YYYY-MM-DD'

  const onFinish = (data: EditUserModel) => {
    console.log('Success:', data)
    ;(async () => {
      let service = UserService.getInstance();
      try {
        await service.updateUser(data, userId)
          .then(
            (res) => {
              if (res.status === 200) {
                console.log('Updated Successfully');
              }
            }
          );
        message.success('Updated Successfully');
        history.push('/users');
      } catch {

      }
    })();
  };

  const onFinishFailed = (errorInfo: any) => {
    message.error(
      'Update Fail ! Please check date of birth or joined date again',
    )
    console.log('Failed:', errorInfo)
  }

  const [form] = useForm()

  useEffect(() => {
    ;(async () => {
      let service = UserService.getInstance();
      let user = await service.getUser(userId)
      setDob(user.dateOfBirth)
      form.setFieldsValue({
        firstName: user.firstName,
        lastName: user.lastName,
        dateOfBirth: moment(user.dateOfBirth),
        gender: user.gender,
        joinedDate: moment(user.joinedDate),
        type: user.type,
      })
    })()
  }, [])

  return (
    <>
      <Col span={9}>
        <h4>Edit User</h4>
      </Col>
      <Col span={16}>
        <Form
          {...layout}
          form={form}
          onFinish={onFinish}
          onFinishFailed={onFinishFailed}
        >
          <Form.Item name="firstName" label="Last Name">
            <Input disabled />
          </Form.Item>
          <Form.Item name="lastName" label="First Name">
            <Input disabled />
          </Form.Item>

          <Form.Item
            hasFeedback
            name="dateOfBirth"
            label="Date Of Birth"
            rules={[{ required: true, message: "Please select date of birth!" }, { validator: validateDateOfBirth }]}
          >
            <DatePicker format={dateFormat} />
          </Form.Item>

          <Form.Item name="gender" label="Gender" hasFeedback>
            <Radio.Group>
              <Radio value={UserGender.MALE}>Male</Radio>
              <Radio value={UserGender.FEMALE}>Female</Radio>
            </Radio.Group>
          </Form.Item>

          <Form.Item
            hasFeedback
            name="joinedDate"
            label="Joined Date"
            rules={[{ required: true, message: "Please select join date !" }, { validator: validateJoinedDate }]}
          >
            <DatePicker format={dateFormat} />
          </Form.Item>

          <Form.Item
            name="type"
            label="Type"
            hasFeedback
            rules={[{ required: true, message: "Please select type of user!" }]}
          >
            <Select>
              <Option value={UserType.ADMIN}>Admin</Option>
              <Option value={UserType.USER}>User</Option>
            </Select>
          </Form.Item>

          <Form.Item
            {...tailLayout}
          >
            <Space>
              <Button type="primary" htmlType="submit"
                style={{ backgroundColor: '#e9424d', color: 'white' }}>
                Save
            </Button>
              <Button type="default" danger>
                <a href="/users"> Cancel </a>
              </Button>
            </Space>
          </Form.Item>
        </Form>
      </Col>
    </>
  )
}
