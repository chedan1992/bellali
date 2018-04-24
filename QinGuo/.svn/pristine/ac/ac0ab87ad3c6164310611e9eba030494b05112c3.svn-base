Ext.onReady(function () {

    //数据源
    var className = ''; //页面类名
    if (this.frameElement) {
        className = this.frameElement.name
    }
    //转义列
    var EDynamic = Ext.data.Record.create([
              { name: "Id", type: "string", mapping: "Id" },
              { name: "UserName", type: "string", mapping: "UserName" },
              { name: "LinkUrl", type: "string", mapping: "LinkUrl" },
              { name: "IPAddress", type: "string", mapping: "IPAddress" },
              { name: "Remark", type: "string", mapping: "Remark" },
                { name: "ColumnName", type: "string", mapping: "ColumnName" },
                  { name: "LogType", type: "string", mapping: "LogType" },
              { name: "CreateTime", type: "datetime", mapping: "CreateTime" }
            ]);
    //数据源
    var store = GridStore(EDynamic, '/LogManage/SearchData', className);

    //快捷查询,如果不需要,可以不用定义

    var grid = new Ext.grid.GridPanel({
        region: 'center',
        layout: 'fit',
        id: 'gg',
        store: store,
        stripeRows: true, //隔行颜色不同
        border: false,
        enableDragDrop: true, //禁用才能选择复选框
        loadMask: { msg: '数据请求中，请稍后...' },
        columns: [
                new Ext.grid.RowNumberer({ header: '', width: 25 }), //设置行号
                {
                header: "用户名",
                dataIndex: 'UserName',
                flex: 3,
                menuDisabled: true
            }, {
                header: "栏目类型",
                dataIndex: 'ColumnName',
                flex: 3,
                menuDisabled: true
            }, {
                header: "访问IP",
                dataIndex: 'IPAddress',
                flex: 2,
                menuDisabled: true
            }, {
                header: "信息介绍",
                dataIndex: 'Remark',
                flex: 2,
                menuDisabled: true
            },
                    {
                        header: '访问时间',
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

    // 响应加载前事件，传递node参数 
    grid.store.on('beforeload', function (node) {
        grid.getStore().proxy.conn.url = '/LogManage/SearchData?GroupId=' + treeNodeId;
    });
    //默认选中第一行
    grid.store.on("load", function () {
        grid.getSelectionModel().selectFirstRow(); //用这个就可以了，前提是你要有SelectionModel的实现
    });



    var tree = new Ext.tree.TreePanel({
        width: 180,
        layout: 'fit',
        autoScroll: true,
        animate: true,
        enableDD: false,
        rootVisible: false, //根节点是否可见  
        lines: true, //显示树形控件的前导线
        bodyStyle: 'padding-top:5px; font-size:12px;',
        containerScroll: true,
        root: {
            text: '分级列表',
            expanded: true,
            children: [
                        {
                            text: '查看所有',
                            id: '-1',
                            leaf: true
                        },
                        {
                            text: '访问日志',
                            id: '0',
                            leaf: true
                        },
                        {
                            text: '操作日志',
                            id: '1',
                            leaf: true
                        },
                        {
                            text: '异常日志',
                            id: '2',
                            leaf: true
                        }
                    ]
        },
        minSize: 175,
        maxWidth: 200,
        margins: '0 2 0 0',
        border: false,
        listeners: {
            afterrender: function (tree) {
                tree.getRootNode().childNodes[0].select(); //默认选中第一个节点
            },
            click: treeitemclick
        }
    });

    var viewport = new Ext.Viewport({
        layout: 'border',
        id: 'viewport',
        items: [
            {
                region: 'center',
                id: 'eastOrganization',
                bodyStyle: 'border-top:0px;border-bottom:0px',
                layout: 'fit',
                items: [grid]
            },
            new Ext.Panel({
                region: 'west',
                layout: 'fit',
                collapsible: true,
                title: '日志类别',
                collapsed: false, //默认折叠
                collapseMode: 'mini', //出现小箭头
                split: true,
                width: 200,
                minSize: 175,
                bodyStyle: 'border-left:0px;border-top:0px;border-bottom:0px',
                deferredRender: false,
                items: [tree]
            })]
    });

});
