Ext.onReady(function () {

    //快捷查询,如果不需要,可以不用定义
    var className = ''; //页面类名
    if (this.frameElement) {
        className = this.frameElement.name
    }

    searchData = [['全查询', ''], ['二维码编号', 'Name']];

    //转义列
    var Member = Ext.data.Record.create([
              { name: "Id", type: "string", mapping: "Id" },
              { name: "Name", type: "string", mapping: "Name" },
              { name: "GroupName", type: "string", mapping: "GroupName" },
              { name: "QrCode", type: "string", mapping: "QrCode" },
              { name: "Status", type: "Int", mapping: "Status" },
              { name: "Img", type: "string", mapping: "Img" },
              { name: "SysId", type: "string", mapping: "SysId" },
              { name: "CreateTime", type: "datetime", mapping: "CreateTime" }
    ]);
    //数据源
    var store = GridStore(Member, '/SysQRCode/SearchData');

    var sm = new Ext.grid.CheckboxSelectionModel();
    var grid = new Ext.grid.GridPanel({
        layout: 'fit',
        id: 'gg',
        region: 'center',
        style: 'border-left:0px;',
        height: 300,
        sm: sm,
        store: store,
        stripeRows: true, //隔行颜色不同
        defult: {
            sortable: false
        },
        loadMask: { msg: '数据请求中，请稍后...' },
        columns: [
                new Ext.grid.RowNumberer({ header: '', width: 25 }), //设置行号
                 sm, //设置复选框
            {
            header: "二维码编号",
            dataIndex: 'Name',
            width: 100,
            menuDisabled: true
        },
            {
                header: "状态",
                dataIndex: 'Status',
                width: 30,
                menuDisabled: true,
                renderer: formartStatus
            },
            {
                header: '添加时间',
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

    grid.store.on("load", function () {
        grid.getSelectionModel().selectFirstRow(); //用这个就可以了，前提是你要有SelectionModel的实现
        Gridrowclick();
    });

    //单机事件
    grid.addListener('rowclick', Gridrowclick);
    //    grid.addListener('rowdblclick', dbGridClick);



    //viewport
    var viewport = new Ext.Viewport({
        layout: 'border',
        items: [
       grid, {
           region: 'east',
           id: 'eastOrganization',
           collapsible: false,
           collapsed: false, //默认折叠
           collapseMode: 'mini', //出现小箭头
           split: true,
           bodyStyle: 'border-top:0px;border-bottom:0px',
           width: 300,
           title: '二维码展示',
           minSize: 225,
           maxSize: 400,
           margins: '0 5 0 0',
           layout: 'fit',
           contentEl: 'MicrositeRt'
       }],
        listeners: {
            afterrender: function () {
            }
        }
    });
});
