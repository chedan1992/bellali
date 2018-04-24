Ext.onReady(function () {
    //快捷查询,如果不需要,可以不用定义
    var className = ''; //页面类名
    if (this.frameElement) {
        className = this.frameElement.name
    }
    //转义列
    var Member = Ext.data.Record.create([
              { name: "Id", type: "string", mapping: "Id" },
              { name: "EmployerId", type: "string", mapping: "EmployerId" },
              { name: "CId", type: "string", mapping: "CId" },
              { name: "CompanyType", type: "Int", mapping: "CompanyType" },
              { name: "SelectType", type: "Int", mapping: "SelectType" },
              { name: "FlowId", type: "string", mapping: "FlowId" },
               { name: "Status", type: "Int", mapping: "Status" },
              { name: "ApprovalUser", type: "string", mapping: "ApprovalUser" },
              { name: "ApprovalTime", type: "datetime", mapping: "ApprovalTime" },
              { name: "AuditUser", type: "string", mapping: "AuditUser" },
              { name: "AuditTime", type: "datetime", mapping: "AuditTime" },
              { name: "CompanyId", type: "string", mapping: "CompanyId" },
              { name: "ApprovalUserName", type: "string", mapping: "ApprovalUserName" },
              { name: "EmployerName", type: "string", mapping: "EmployerName" },
              { name: "CName", type: "string", mapping: "CName" },
              { name: "EmployerPhone", type: "string", mapping: "EmployerPhone" },
              { name: "EmployerLinUser", type: "string", mapping: "EmployerLinUser" },
              { name: "EmployerAddress", type: "string", mapping: "EmployerAddress" },
              { name: "CPhone", type: "string", mapping: "CPhone" },
              { name: "CLinUser", type: "string", mapping: "CLinUser" },
              { name: "CAddress", type: "string", mapping: "CAddress" },
              { name: "AuditUserName", type: "string", mapping: "AuditUserName" },
              { name: "CreateTime", type: "datetime", mapping: "CreateTime" }
    ]);
    //数据源
    var store = GridStore(Member, '/SysCompanyCognate/SearchIndex', className);

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
                header: "单位名称",
                dataIndex: 'CName',
                width: 120,
                sortable: false,
                menuDisabled: true
            }, {
                header: "单位地址",
                dataIndex: 'CAddress',
                width: 160,
                sortable: false,
                menuDisabled: true
            },
            {
                header: "联系人",
                dataIndex: 'CLinUser',
                width: 80,
                sortable: false,
                menuDisabled: true,
                renderer: IsNull
            },
                {
                    header: "联系电话",
                    dataIndex: 'CPhone',
                    flex: 1,
                    sortable: false,
                    menuDisabled: true,
                    renderer: IsNull
                },
                {
                    header: "审核人",
                    dataIndex: 'AuditUserName',
                    flex: 1,
                    sortable: false,
                    menuDisabled: true,
                    renderer: IsNull
                },
                
                {
                    header: '状态',
                    sortable: false,
                    dataIndex: 'Status',
                    menuDisabled: true,
                    width: 70,
                    renderer: formartRelation
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










