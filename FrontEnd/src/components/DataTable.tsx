import { Table } from 'antd';
import useActionMenu from './ActionMenu';
import { IDataTable } from '../models/IDataTable';
import { useEffect, useState } from 'react';

const DEFAULT_PAGE_SIZE = 10;
const DEFAULT_PAGE_NUMBER = 0;

export function useDataTable<T extends object>({ columns, updateEntityPath, service }: IDataTable<T>) {
  const [items, setItems] = useState<T[]>([]);
  const [selectedRow, setSelectedRow] = useState({ id: 0 });
  const [currentPage, setCurrentPage] = useState(DEFAULT_PAGE_NUMBER);
  const actionColumn = useActionMenu({ selectedRow, updateEntityPath, service });
  const didChange = actionColumn.didChange;

  useEffect(() => {
    (async () => {
      let items = await service.getAll();
      setItems(items);
    })();
  }, [actionColumn.didChange]);

  const updatedColumns = [
    ...columns,
    {
      title: 'Action',
      key: 'action',
      align: 'center',
      render: () => actionColumn.actionColumnView,
    },
  ];

  const resetPagination = () => {
    setCurrentPage(DEFAULT_PAGE_NUMBER);
  };

  const handleTableChange = (pagination: any) => {
    console.log('pagination:', pagination);
    setCurrentPage(pagination.current - 1);
  };

  const DataTable = () => (
    <Table
      rowKey={(record: any) => record.id}
      columns={updatedColumns}
      dataSource={items}
      onRow={(record: any) => {
        return {
          onClick: () => {
            setSelectedRow(record);
          },
        };
      }}
      onChange={handleTableChange}
      pagination={{
        pageSize: DEFAULT_PAGE_SIZE,
        current: currentPage + 1,
        total: items.length,
        showTotal: (total, range) => {
          return `${range[0]}-${range[1]} of ${total} items`;
        },
      }}
    />
  );

  return {
    DataTable,
    didChange,
    currentPage,
    resetPagination,
  };
}

export default useDataTable;
