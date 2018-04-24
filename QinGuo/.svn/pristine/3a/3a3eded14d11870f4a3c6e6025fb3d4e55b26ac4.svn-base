//列表公司
Ext.onReady(function () {
    //快捷查询,如果不需要,可以不用定义
    var className = ''; //页面类名
    if (this.frameElement) {
        className = this.frameElement.name
    }
    //转义列
    var Member = Ext.data.Record.create([
              { name: "Id", type: "string", mapping: "Id" },
              { name: "Name", type: "string", mapping: "Name" },
              { name: "Code", type: "string", mapping: "Code" },
              { name: "Led", type: "string", mapping: "Led" },
              { name: "CustomerId", type: "string", mapping: "CustomerId" },
              { name: "Model", type: "string", mapping: "Model" },
              { name: "Brand", type: "string", mapping: "Brand" },
              { name: "Addr", type: "string", mapping: "Addr" },
              { name: "Year_NFC", type: "string", mapping: "Year_NFC" },
              { name: "A_NFC", type: "string", mapping: "A_NFC" },
              { name: "B_NFC", type: "string", mapping: "B_NFC" },
              { name: "C_NFC", type: "string", mapping: "C_NFC" },
              { name: "D_NFC", type: "string", mapping: "D_NFC" },
              { name: "Year_NFCTime", type: "datetime", mapping: "Year_NFCTime" },
              { name: "A_NFCTime", type: "datetime", mapping: "A_NFCTime" },
                { name: "ElevatorType", type: "string", mapping: "ElevatorType" },
              { name: "B_NFCTime", type: "datetime", mapping: "B_NFCTime" },
              { name: "C_NFCTime", type: "datetime", mapping: "C_NFCTime" },
              { name: "D_NFCTime", type: "datetime", mapping: "D_NFCTime" },
              { name: "Content", type: "datetime", mapping: "Content" },
              { name: "CustomerName", type: "string", mapping: "CustomerName" },
              { name: "BrandName", type: "string", mapping: "BrandName" },
              { name: "ModelName", type: "string", mapping: "ModelName" },
              { name: "CreateTime", type: "datetime", mapping: "CreateTime" },
              { name: "InstallTime", type: "datetime", mapping: "InstallTime" },
              { name: "Status", type: "string", mapping: "Status" }
            ]);
    //数据源
    var store = GridStore(Member, '/ElevatorManage/SearchPropertyData');

    //快捷查询,如果不需要,可以不用定义
    searchData = [['全查询', ''], ['电梯名称', 'Name'], ['电梯编号', 'Code'], ['电梯型号', 'Model']];
  
    var grid = new Ext.grid.GridPanel({
        region: 'center',
        layout: 'fit',
        id: 'gg',
        store: store,
        stripeRows: true, //隔行颜色不同
        border: false,
        loadMask: { msg: '数据请求中，请稍后...' },
        columns: [
            new Ext.grid.RowNumberer({ header: '', width: 25 }), //设置行号
            {
            header: "电梯名称",
            dataIndex: 'Name',
            width: 120,
            sortable: false,
            menuDisabled: true,
            renderer: function (data, metadata, record, rowIndex, columnIndex, store) {
                if (data) {
                    metadata.attr = 'ext:qtip="点击查看详细"';
                    return "<span style='color:blue;cursor:pointer;' onclick='clickDetail()'>" + data + "</span>";
                }
            }
        }, {
            header: "电梯编号",
            width: 100,
            dataIndex: 'Code',
            sortable: false,
            menuDisabled: true
        }, {
            header: "电梯型号",
            width: 100,
            dataIndex: 'ModelName',
            sortable: false,
            menuDisabled: true
        },
        {
            header: "电梯类型",
            width: 100,
            dataIndex: 'ElevatorType',
            sortable: false,
            menuDisabled: true,
            renderer: ElevatorType
        },
//        {
//            header: "物业单位",
//            dataIndex: 'CustomerName',
//            width: 80,
//            sortable: false,
//            menuDisabled: true
//        },
            {
                header: '所属品牌',
                sortable: false,
                dataIndex: 'BrandName',
                menuDisabled: true,
                width: 80
            },
              {
                  header: '安装日期',
                  sortable: false,
                  dataIndex: 'InstallTime',
                  flex: 4,
                  menuDisabled: true,
                  renderer: renderDate
              },
            {
                header: '状态',
                sortable: false,
                dataIndex: 'Status',
                menuDisabled: true,
                width: 60,
                renderer: formartEnableOrDisable
            },
            {
                header: '添加时间',
                sortable: false,
                dataIndex: 'CreateTime',
                flex: 4,
                menuDisabled: true
            }
        ],
        tbar: tbar(className),
        bbar: bbar(store), //分页工具条
        viewConfig: {
            forceFit: true,
            enableRowBody: true,
            scrollOffset: 0 //不加这个的话，会在grid的最右边有个空白，留作滚动条的位置
        }
    });
    //默认选中第一行
    grid.store.on("load", function () {
        grid.getSelectionModel().selectFirstRow(); //用这个就可以了，前提是你要有SelectionModel的实现
    }); 
    //viewport
    var viewport = new Ext.Viewport({
        layout: 'border',
        items: [grid],
        listeners: {
            afterrender: function () {

            }
        }
    });
});












