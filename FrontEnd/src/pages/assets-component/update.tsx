import {
    Button,
    Col,
    DatePicker,
    Form,
    Input,
    message,
    Radio,
    Space,
} from "antd";
import { useForm } from "antd/lib/form/Form";
import { useEffect } from "react";
import { useHistory, useParams } from "react-router-dom";
import moment from "moment";
import { AssetState, EditAssetModel } from "../../models/Asset";
import { AssetService } from "../../services/AssetService";
import TextArea from "antd/lib/input/TextArea";

export function UpdateAsset() {
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

    const { assetId } = useParams<any>();

    let history = useHistory()

    const dateFormat = 'YYYY-MM-DD'

    const onFinish = (data: EditAssetModel) => {
        console.log('Success:', data)
            ; (async () => {
                let service = AssetService.getInstance();
                try {
                    await service.update(data, assetId)
                        .then(
                            (res) => {
                                if (res.status === 200) {
                                    console.log('Updated Successfully');
                                }
                            }
                        );
                    message.success('Updated Successfully');
                    history.push('/assets');
                } catch {

                }
            })();
    };

    const onFinishFailed = (errorInfo: any) => {
        message.error(
            'Update Fail !',
        )
        console.log('Failed:', errorInfo)
    }

    const [form] = useForm()

    useEffect(() => {
        ; (async () => {
            let service = AssetService.getInstance();
            let asset = await service.getAsset(assetId);
            form.setFieldsValue({
                assetName: asset.assetName,
                category: asset.category.categoryName,
                specification: asset.specification,
                installedDate: moment(asset.installedDate),
                state: asset.state,
            })
        })()
    }, [])

    return (
        <>
            <Col span={9}>
                <h4>Edit Asset</h4>
            </Col>
            <Col span={16}>
                <Form
                    {...layout}
                    form={form}
                    onFinish={onFinish}
                    onFinishFailed={onFinishFailed}
                >
                    <Form.Item
                        label="Name"
                        name="assetName"
                        rules={[
                            { required: true, message: "Name is required!" },
                            { max: 100, message: "Maximum 100 characters!" },
                            { whitespace: true, message: "Asset Name can not be empty!" },
                        ]}
                        hasFeedback
                    >
                        <Input />
                    </Form.Item>
                    <Form.Item name="category" label="Category">
                        <Input disabled />
                    </Form.Item>


                    <Form.Item
                        label="Specification"
                        name="specification"
                        rules={[
                            { required: true, message: "Specification is required!" },
                            { whitespace: true, message: "Specification can not be empty!" },
                        ]}
                        hasFeedback
                    >
                        <TextArea rows={8} />
                    </Form.Item>

                    <Form.Item
                        hasFeedback
                        name="installedDate"
                        label="Installed Date"
                        rules={[{ required: true, message: "Please select installed date!" }]}
                    >
                        <DatePicker format={dateFormat} />
                    </Form.Item>

                    <Form.Item 
                    name="state" 
                    label="State" 
                    hasFeedback
                    rules={[{ required: true, message: "Please select state!" }]}
                    >
                        <Radio.Group>
                            <Radio value={AssetState.AVAILABLE}>Available</Radio>
                            <Radio value={AssetState.NOT_AVAILABLE}>Not Available</Radio>
                            <Radio value={AssetState.ASSIGNED}>Assigned</Radio>
                            <Radio value={AssetState.WAITING_FOR_RECYCLING}>Waiting For Recycling</Radio>
                            <Radio value={AssetState.RECYCLED}>Recycled</Radio>
                        </Radio.Group>
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
                                <a href="/assets"> Cancel </a>
                            </Button>
                        </Space>
                    </Form.Item>
                </Form>
            </Col>
        </>
    )
}
