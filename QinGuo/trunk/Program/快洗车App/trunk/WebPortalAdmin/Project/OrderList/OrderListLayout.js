//列表公司
Ext.onReady(function () {
    var className = ''; //页面类名
    if (this.frameElement) {
        className = this.frameElement.name
    }
    //转义列
    var SearchOrderData = Ext.data.Record.create([
              { name: "Id", type: "string", mapping: "Id" },
              { name: "CEId", type: "string", mapping: "CEId" },
              { name: "SysId", type: "string", mapping: "SysId" },
              { name: "Mark", type: "string", mapping: "Mark" },
              { name: "Contacts", type: "string", mapping: "Contacts" },
              { name: "ContactsPhone", type: "string", mapping: "ContactsPhone" },
              { name: "Price", type: "string", mapping: "Price" },
              { name: "OrderType", type: "string", mapping: "OrderType" },
              { name: "Ordercode", type: "string", mapping: "Ordercode" },
              { name: "CauseReason", type: "string", mapping: "CauseReason" },
              { name: "CauseImg", type: "string", mapping: "CauseImg" },
              { name: "DayType", type: "string", mapping: "DayType" },
              { name: "IsRepair", type: "bool", mapping: "IsRepair" },
              { name: "CEName", type: "string", mapping: "CEName" },
              { name: "SysName", type: "string", mapping: "SysName" },
              { name: "EngineerName", type: "string", mapping: "EngineerName" },
              { name: "OrderType", type: "string", mapping: "OrderType" },
              { name: "GrabSingleTime", type: "datetime", mapping: "GrabSingleTime" },
              { name: "NFC_Code", type: "string", mapping: "NFC_Code" },
              { name: "OrderType", type: "string", mapping: "OrderType" },
              { name: "Status", type: "string", mapping: "Status" }
            ]);
    //数据源
    var store = GridStore(SearchOrderData, '/ETask/SearchOrderData');
   
    //快捷查询,如果不需要,可以不用定义
    searchData = [['全查询', ''], ['订单编号', 'Ordercode'], ['电梯名称', 'Name'], ['故障描述', 'Mark']];

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
              header: "订单编号",
              dataIndex: 'Ordercode',
              width:120,
              sortable: false,
              menuDisabled: true
          },
            {
            header: "电梯名称",
            dataIndex: 'CEName',
            width: 120,
            sortable: false,
            menuDisabled: true
        },
//          {
//              header: 'NFC编码',
//              sortable: false,
//              dataIndex: 'NFC_Code',
//              menuDisabled: true,
//              width: 100,
//              renderer: IsNull
//          },
         {
             header: '类型',
             sortable: false,
             dataIndex: 'OrderType',
             menuDisabled: true,
             width: 60,
             renderer: formartOrderType
         },
//         {
//             header: '维修时间',
//             sortable: false,
//             dataIndex: 'DayType',
//             menuDisabled: true,
//             width: 60,
//             renderer: formartDayType
//         },
        {
            header: "价格",
            width: 70,
            dataIndex: 'Price',
            sortable: false,
            menuDisabled: true,
            renderer: formartPrice
        },
//        {
//            header: "故障描述",
//            width: 100,
//            dataIndex: 'Mark',
//            sortable: false,
//            menuDisabled: true,
//            renderer: IsNull
//        },
        {
            header: "抢单工程师",
            dataIndex: 'EngineerName',
            width: 80,
            sortable: false,
            menuDisabled: true,
            renderer: IsNull
        },
//         {
//             header: "物业联系人",
//             dataIndex: 'Contacts',
//             width: 80,
//             sortable: false,
//             menuDisabled: true,
//             renderer: IsNull
//         },
//            {
//                header: '物业电话',
//                sortable: false,
//                dataIndex: 'ContactsPhone',
//                menuDisabled: true,
//                width: 80,
//                renderer: IsNull
//            },
              {
                    header: "故障描述",
                    dataIndex: 'Mark',
                    width:100,
                    sortable: false,
                    menuDisabled: true,
                    renderer: IsNullTip
                },
            {
                header: '状态',
                sortable: false,
                dataIndex: 'Status',
                menuDisabled: true,
                width: 60,
                renderer: formartFlowStatus
            },
              {
                  header: '抢单时间',
                  sortable: false,
                  dataIndex: 'GrabSingleTime',
                  flex: 4,
                  menuDisabled: true,
                  renderer: renderCreateTime
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

    //单机事件
    grid.addListener('rowclick', GridClick);
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

//单机事件
function GridClick(grid, rowindex, e) {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelected();
    var Status = rows.get("Status"); //状态
    if (parseInt(Status) != 0) {
        if (Ext.getCmp("GTP_sharedwithme")) {
            Ext.getCmp("GTP_sharedwithme").setDisabled(true);
        }
    }
    else {
        if (Ext.getCmp("GTP_sharedwithme")) {
            Ext.getCmp("GTP_sharedwithme").setDisabled(false);
        }
    }
}











