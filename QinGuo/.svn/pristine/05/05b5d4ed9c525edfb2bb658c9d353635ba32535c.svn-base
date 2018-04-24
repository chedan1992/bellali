//我的巡检记录
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
                { name: "Img", type: "string", mapping: "Img" },
              { name: "Mark", type: "string", mapping: "Mark" },
              { name: "CreateTime", type: "decimal", mapping: "CreateTime" },
              { name: "Status", type: "Int", mapping: "Status" }

    ]);
    //数据源
    var store = GridStore(Member, '/SysAppointCheckNotes/SearchData', className);

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
                 header: "",
                 dataIndex: 'Id',
                 width: 35,
                 sortable: false,
                 menuDisabled: true,
                 renderer: function (value, meta, record, rowIdx, colIdx, store) {
                     var content = '<a href="#" style="vertical-align: middle;cursor:pointer" onclick="LookDeitl()"><img style="vertical-align: middle; width:18px; height:18px;border:0" class="GTP_view"  src="../../Content/Extjs/resources/images/default/s.gif"/>查看</a>';
                     return content;
                 }
             },
                {
                    header: "设备名称",
                    dataIndex: 'Name',
                    width:120,
                    sortable: false,
                    menuDisabled: true
                }, {
                    header: "巡检备注",
                    dataIndex: 'Mark',
                    width: 160,
                    sortable: false,
                    menuDisabled: true
                },
                 {
                     header: "巡检状态",
                     dataIndex: 'Status',
                     width: 160,
                     sortable: false,
                     menuDisabled: true,
                     renderer: renderMaintenanceStatus
                 },
                {
                    header: '巡检时间',
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
    grid.addListener('rowdblclick', dbGridClick);
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










