import { Button, DatePicker, Form, Input, Radio, Select, Space } from "antd";
import Title from "antd/lib/typography/Title";
import { useEffect, useState } from "react";
import { useHistory } from "react-router-dom";
import { CreateUserModel } from "../../../models/user";
import { UserSerivce } from "../../../services/UserService";

const { Option } = Select;

export function CreateUser() {
    const layout = {
        labelCol: {
            span: 16,
            offset: 3,
            pull: 9
        },
        wrapperCol: {
            span: 16,
            pull: 9
        },
    };
    const tailLayout = {
        wrapperCol: {
        },
    };

    const [form] = Form.useForm();
    const [, forceUpdate] = useState({}); // To disable submit button at the beginning.

    useEffect(() => {
        forceUpdate({});
    }, []);

    sessionStorage.setItem("Location", "0");
    let service = UserSerivce.getInstance();

    let history = useHistory();

    const location: number = Number(sessionStorage.getItem("Location"));

    const onFinish = (data: CreateUserModel) => {
        data.location = location;
        (async () => {
            await service.create(data);
            history.push("/users");
        })();
    };
    const dateFormat = "YYYY-MM-DD";

    const onFinishFailed = (errorInfo: any) => {
        console.log('Failed:', errorInfo);
    };

    return (
        <>
            <Title style={{ color: 'red' }}>Create New User</Title>
            <Form
                {...layout}
                name="basic"
                onFinish={onFinish}
                onFinishFailed={onFinishFailed}
                form={form}
            >
                <Form.Item
                    label="First Name"
                    name="firstName"
                    rules={[
                        { required: true, message: "First Name is required!" },
                        { pattern: /^[A-Za-z ]+$/i, message: "Alphebet characters only!" },
                        { max: 50, message: "Maximum 50 characters!" },
                        { whitespace: true, message: "First Name can not be emty!" },
                    ]}
                >
                    <Input />
                </Form.Item>

                <Form.Item
                    label="Last Name"
                    name="lastName"
                    rules={[
                        { required: true, message: "Last Name is required!" },
                        { pattern: /^[A-Za-z ]+$/i, message: "Alphebet characters only!" },
                        { max: 50, message: "Maximum 50 characters!" },
                        { whitespace: true, message: "Last Name can not be emty!" },
                    ]}
                >
                    <Input />
                </Form.Item>

                <Form.Item
                    label="Date Of Birth"
                    name="dateOfBirth"
                    rules={
                        [{ required: true, message: "Date Of Birth is required!" }]}
                >
                    <DatePicker format={dateFormat} />
                </Form.Item>

                <Form.Item
                    label="Gender"
                    name="gender"
                    rules={[{ required: true, message: "Gender is required!" }]}
                >
                    <Radio.Group>
                        <Radio value={0}>Male</Radio>
                        <Radio value={1}>Female</Radio>
                    </Radio.Group>
                </Form.Item>

                <Form.Item
                    label="Joined Date"
                    name="joinedDate"
                    rules={[{ required: true, message: "Joined Date is required!" }]}
                >
                    <DatePicker format={dateFormat}  />
                </Form.Item>

                <Form.Item
                    label="Type"
                    name="type"
                    rules={[{ required: true, message: "Type is required!" }]}
                >
                    <Select style={{ width: 200 }} placeholder="Select a type" >
                        <Option key={0} value={0}>Admin</Option>
                        <Option key={1} value={1}>User</Option>
                    </Select>
                </Form.Item>

                <Form.Item {...tailLayout} shouldUpdate>
                    {() => (
                        <Space>
                            <Button
                                type="primary"
                                htmlType="submit"
                                disabled={
                                    !form.isFieldsTouched(true) ||
                                    !!form.getFieldsError().filter(({ errors }) => errors.length).length
                                }
                            >
                                Save
                            </Button>
                            <Button>
                                Cancel
                            </Button>
                        </Space>
                    )}
                </Form.Item>
            </Form>
        </>
    );
}
