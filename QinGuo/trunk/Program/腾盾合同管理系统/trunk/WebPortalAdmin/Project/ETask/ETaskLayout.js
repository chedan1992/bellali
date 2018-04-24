//列表公司
Ext.onReady(function () {

    var className = ''; //页面类名
    if (this.frameElement) {
        className = this.frameElement.name
    }
    //转义列
    var ETask = Ext.data.Record.create([
              { name: "Id", type: "string", mapping: "Id" },
              { name: "UserName", type: "string", mapping: "UserName" },
              { name: "Type", type: "Int", mapping: "Type" },
              { name: "StarTime", type: "datetime", mapping: "StarTime" },
              { name: "EndTime", type: "datetime", mapping: "EndTime" },
                { name: "Remark", type: "string", mapping: "Remark" },
              { name: "OrderType", type: "string", mapping: "OrderType" },
             
              { name: "CreateTime", type: "datetime", mapping: "CreateTime" },
              { name: "Status", type: "string", mapping: "Status" }
            ]);
    //数据源（第三个参数不传，此页面不需要受数据权限控制）
    var store = GridStore(ETask, '/ETask/SearchData');
   
    //快捷查询,如果不需要,可以不用定义
    searchData = [['全查询', ''], ['电梯名称', 'Name'], ['故障描述', 'Mark']];

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
            header: "员工名称",
            dataIndex: 'UserName',
            width:80,
            sortable: false,
            menuDisabled: true
        },
         {
             header: '类型',
             sortable: false,
             dataIndex: 'Type',
             menuDisabled: true,
             width:50,
             renderer: formartOrderType
         },
         {
             header: '开始时间',
             sortable: false,
             dataIndex: 'StarTime',
             menuDisabled: true,
             width:160,
             renderer: renderTime
         },
          {
          header: "描述备注",
          dataIndex: 'Remark',
          width:100,
          sortable: false,
          menuDisabled: true,
          renderer: IsNullTip
      },
        {
            header: "状态",
            width: 100,
            dataIndex: 'Status',
            sortable: false,
            menuDisabled: true,
            renderer: formarFlow
        },
              {
                  header: '申请日期',
                  sortable: false,
                  dataIndex: 'CreateTime',
                  flex: 4,
                  menuDisabled: true,
                  renderer: renderDate
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











