import { Button, Col, DatePicker, Form, Input, Radio, Select, Space } from "antd";
import moment from "moment";
import { useEffect, useState } from "react";
import { Link, useHistory } from "react-router-dom";
import { CreateUserModel, UserGender, UserType } from "../../models/User";
import { UserService } from "../../services/UserService";

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

    const [dob, setDob] = useState<Date>();
    const [form] = Form.useForm();
    const [, forceUpdate] = useState({}); // To disable submit button at the beginning.

    useEffect(() => {
        forceUpdate({});
    }, []);

    let service = UserService.getInstance();

    let history = useHistory();

    const location: number = Number(sessionStorage.getItem("location"));
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
            <Col span={9}>
                <h4>Create New User</h4>
            </Col>
            <Col span={16}>
                <Form
                    {...layout}
                    name="basic"
                    form={form}
                    onFinish={onFinish}
                    onFinishFailed={onFinishFailed}
                >
                    <Form.Item
                        label="First Name"
                        name="firstName"
                        rules={[
                            { required: true, message: "First Name is required!" },
                            { pattern: /^[A-Za-z ]+$/i, message: "Alphabet characters only!" },
                            { max: 50, message: "Maximum 50 characters!" },
                            { whitespace: true, message: "First Name can not be empty!" },
                        ]}
                        hasFeedback
                    >
                        <Input />
                    </Form.Item>

                    <Form.Item
                        label="Last Name"
                        name="lastName"
                        rules={[
                            { required: true, message: "Last Name is required!" },
                            { pattern: /^[A-Za-z ]+$/i, message: "Alphabet characters only!" },
                            { max: 50, message: "Maximum 50 characters!" },
                            { whitespace: true, message: "Last Name can not be empty!" },
                        ]}
                        hasFeedback
                    >
                        <Input />
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


                    <Form.Item {...tailLayout} shouldUpdate hasFeedback>
                        {() => (
                            <Space>
                                <Button
                                    style={{ backgroundColor: '#e9424d', color: 'white' }}
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
                                    <Link to="/users">
                                        Cancel
                                </Link>
                                </Button>
                            </Space>
                        )}
                    </Form.Item>
                </Form>
            </Col>
        </>
    );
}