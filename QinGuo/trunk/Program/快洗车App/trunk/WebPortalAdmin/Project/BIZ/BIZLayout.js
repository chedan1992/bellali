//汽配商门店
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
              { name: "Address", type: "string", mapping: "Address" },
              { name: "CityId", type: "string", mapping: "CityId" },
              { name: "Code", type: "string", mapping: "Code" },
              { name: "Order", type: "string", mapping: "Order" },
              { name: "Status", type: "string", mapping: "Status" },
              { name: "LinkUser", type: "string", mapping: "LinkUser" },
              { name: "UserName", type: "string", mapping: "UserName" },
              { name: "LoginName", type: "string", mapping: "LoginName" },
              { name: "Phone", type: "string", mapping: "Phone" },
              { name: "CreateTime", type: "datetime", mapping: "CreateTime" }
            ]);
    //数据源
    var store = GridStore(Member, '/BIZ/SearchData', className);

    //快捷查询,如果不需要,可以不用定义
    searchData = [['全查询', ''], ['汽配商名称', 'Name'], ['汽配商地址', 'Address'], ['汽配商电话', 'Phone'], ['联系人', 'LinkUser']];

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
            header: "汽配商名称",
            dataIndex: 'Name',
            flex:4,
            sortable: false,
            menuDisabled: true
        }, {
            header: "汽配商地址",
            dataIndex: 'Address',
            flex:3,
            sortable: false,
            menuDisabled: true
        },
        {
            header: "联系人",
            dataIndex: 'LinkUser',
            width: 80,
            sortable: false,
            menuDisabled: true,
            renderer: IsNull
        },
            {
                header: '汽配商电话',
                sortable: false,
                dataIndex: 'Phone',
                menuDisabled: true,
                flex:1,
                renderer: IsNull
            },
            
             {
                 header: "管理员名",
                 dataIndex: 'UserName',
                 width: 60,
                 sortable: false,
                 menuDisabled: true
             },
                 {
                     header: "登录帐号",
                     dataIndex: 'LoginName',
                     width: 90,
                     sortable: false,
                     menuDisabled: true
                 },
            {
                header: '状态',
                sortable: false,
                dataIndex: 'Status',
                menuDisabled: true,
                width:60,
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















