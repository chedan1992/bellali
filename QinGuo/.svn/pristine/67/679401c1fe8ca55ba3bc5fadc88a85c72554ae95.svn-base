Ext.onReady(function () {
//    /*
//    * ================页面布局=======================
//    */
//    //定义树的加载器 
//    var treeloader = new Ext.tree.TreeLoader({
//        url: "/Organizational/SearchPostData"
//    });

//    var tree = new Ext.tree.TreePanel({
//        region: 'center',
//        id: 'tr',
//        layout: 'fit',
//        layoutConfig: {
//            animate: true
//        },
//        autoScroll: true,
//        animate: true, //动画效果  
//        rootVisible: false, //根节点是否可见  
//        lines: true, //显示树形控件的前导线
//        containerScroll: true,
//        border: false,
//        bodyStyle: 'padding-top:5px',
//        loader: new Ext.tree.TreeLoader({
//            dataUrl: '/Organizational/SearchPostData'
//        }),
//        root: {
//            nodeType: 'async',
//            text: '组织机构',
//            iconCls: 'GTP_home',
//            draggable: false,
//            id: 'top'//区分是否根节点
//        },
//        listeners: {
//            click: treeitemclick
//        }
//    });
//    tree.expandAll();


//    //转义列
//    var Master = Ext.data.Record.create([
//              { name: "Id", type: "string", mapping: "Id" },
//              { name: "LoginName", type: "string", mapping: "LoginName" },
//              { name: "UserName", type: "string", mapping: "UserName" },
//              { name: "Sex", type: "int", mapping: "Sex" },
//              { name: "Pwd", type: "string", mapping: "Pwd" },
//              { name: "Email", type: "string", mapping: "Email" },
//              { name: "Phone", type: "string", mapping: "Phone" },
//              { name: "Status", type: "int", mapping: "Status" },
//              { name: "IsMain", type: "bool", mapping: "IsMain" },
//              { name: "CreateTime", type: "datetime", mapping: "CreateTime" },
//              { name: "CreaterName", type: "string", mapping: "CreaterName" },
//              { name: "RoleName", type: "string", mapping: "RoleName" }

//            ]);

//    //快捷查询,如果不需要,可以不用定义
//    searchData = [['全查询', ''], ['用户名称', 'UserName'], ['登录账号', 'LoginName'], ['邮箱', 'Email'], ['联系电话', 'Phone'], ['状态', 'Status'], ['添加时间', 'CreateTime']];
//    var className = ''; //页面类名
//    if (this.frameElement) {
//        className = this.frameElement.name
//    }
//    //数据源
//    var store = GridStore(Master, '/SysMaster/SearchData?Id=-1', 'SysMaster');

//    var grid = new Ext.grid.GridPanel({
//        region: 'center',
//        layout: 'fit',
//        id: 'gg',
//        store: store,
//        stripeRows: true, //隔行颜色不同
//        border: false,
//        defult: {
//            sortable: false,
//            menuDisabled: true
//        },
//        loadMask: { msg: '数据请求中，请稍后...' },
//        columns: [
//                new Ext.grid.RowNumberer({ header: '', width: 25 }), //设置行号
//                {
//                header: "用户名称",
//                dataIndex: 'UserName',
//                menuDisabled: true,
//                flex: 2
//            }, {
//                header: "登录账号",
//                dataIndex: 'LoginName',
//                flex: 4,
//                menuDisabled: true
//            }, {
//                header: "性别",
//                dataIndex: 'Sex',
//                align: 'center',
//                width: 70,
//                renderer: formartSex,
//                menuDisabled: true
//            }, {
//                header: "邮箱",
//                dataIndex: 'Email',
//                flex: 2,
//                menuDisabled: true
//            }, {
//                header: "联系电话",
//                dataIndex: 'Phone',
//                flex: 2,
//                menuDisabled: true
//            }, {
//                header: "状态",
//                dataIndex: 'Status',
//                align: 'center',
//                width: 70,
//                renderer: formartEnableOrDisable,
//                menuDisabled: true
//            },
//            {
//                header: "所属角色",
//                dataIndex: 'RoleName',
//                width: 130,
//                menuDisabled: true,
//                renderer: function (data, metadata, record, rowIndex, columnIndex, store) {
//                    if (data) {
//                        metadata.attr = 'ext:qtip="' + data + '"';
//                        return data;
//                    }
//                }
//            },
//            {
//                header: '添加时间',
//                dataIndex: 'CreateTime',
//                flex: 4,
//                menuDisabled: true,
//                renderer: renderCreateTime
//            }
//                ],
//        tbar: tbar(className),
//        bbar: bbar(store), //分页工具条
//        viewConfig: {
//            forceFit: true,
//            enableRowBody: true,
//            scrollOffset: 0 //不加这个的话，会在grid的最右边有个空白，留作滚动条的位置
//        }
//    });

//    // 响应加载前事件，传递node参数 
//    grid.store.on('beforeload', function (node) {
//        grid.getStore().proxy.conn.url = '/SysMaster/SearchData?Id=' + treeNodeId;
//    });


//    var viewport = new Ext.Viewport({
//        layout: 'border',
//        id: 'viewport',
//        items: [
//            {
//                region: 'center',
//                id: 'eastOrganization',
//                bodyStyle: 'border-top:0px;border-bottom:0px',

//                layout: 'fit',
//                items: [grid]
//            },
//            new Ext.Panel({
//                region: 'west',
//                layout: 'fit',
//                collapsible: true,
//                title: '组织架构',
//                collapsed: false, //默认折叠
//                collapseMode: 'mini', //出现小箭头
//                split: true,
//                width: 230,
//                minSize: 175,
//                bodyStyle: 'border-left:0px;border-top:0px;border-bottom:0px',
//                deferredRender: false,
//                items: [tree]
//            })]
//    });


        //转义列
        var Master = Ext.data.Record.create([
                  { name: "Id", type: "string", mapping: "Id" },
                  { name: "LoginName", type: "string", mapping: "LoginName" },
                  { name: "UserName", type: "string", mapping: "UserName" },
                  { name: "Sex", type: "int", mapping: "Sex" },
                  { name: "Pwd", type: "string", mapping: "Pwd" },
                  { name: "Email", type: "string", mapping: "Email" },
                  { name: "Phone", type: "string", mapping: "Phone" },
                  { name: "Status", type: "int", mapping: "Status" },
                  { name: "IsMain", type: "bool", mapping: "IsMain" },
                  { name: "CreateTime", type: "datetime", mapping: "CreateTime" },
                  { name: "CreaterName", type: "string", mapping: "CreaterName" },
                  { name: "RoleName", type: "string", mapping: "RoleName" }

                ]);

        //快捷查询,如果不需要,可以不用定义
        searchData = [['全查询', ''], ['用户名称', 'UserName'], ['登录账号', 'LoginName'], ['邮箱', 'Email'], ['联系电话', 'Phone'], ['状态', 'Status'], ['添加时间', 'CreateTime']];
        var className = ''; //页面类名
        if (this.frameElement) {
            className = this.frameElement.name
        }
        //数据源
        var store = GridStore(Master, '/SysMaster/SearchData?Id=-1', className);

        var grid = new Ext.grid.GridPanel({
            region: 'center',
            layout: 'fit',
            id: 'gg',
            store: store,
            stripeRows: true, //隔行颜色不同
            border: false,
            defult: {
                sortable: false,
                menuDisabled: true
            },
            loadMask: { msg: '数据请求中，请稍后...' },
            columns: [
                    new Ext.grid.RowNumberer({ header: '', width: 25 }), //设置行号
                    {
                    header: "用户名称",
                    dataIndex: 'UserName',
                    menuDisabled: true,
                    flex: 2
                }, {
                    header: "登录账号",
                    dataIndex: 'LoginName',
                    flex: 4,
                    menuDisabled: true
                }, {
                    header: "性别",
                    dataIndex: 'Sex',
                    align: 'center',
                    width: 70,
                    renderer: formartSex,
                    menuDisabled: true
                }, {
                    header: "邮箱",
                    dataIndex: 'Email',
                    flex: 2,
                    menuDisabled: true
                }, {
                    header: "联系电话",
                    dataIndex: 'Phone',
                    flex: 2,
                    menuDisabled: true
                }, {
                    header: "状态",
                    dataIndex: 'Status',
                    align: 'center',
                    width: 70,
                    renderer: formartEnableOrDisable,
                    menuDisabled: true
                },
                {
                    header: "所属角色",
                    dataIndex: 'RoleName',
                    width: 130,
                    menuDisabled: true,
                    renderer: function (data, metadata, record, rowIndex, columnIndex, store) {
                        if (data) {
                            metadata.attr = 'ext:qtip="' + data + '"';
                            return data;
                        }
                    }
                },
                {
                    header: '添加时间',
                    dataIndex: 'CreateTime',
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

        // 响应加载前事件，传递node参数 
        grid.store.on('beforeload', function (node) {
            grid.getStore().proxy.conn.url = '/SysMaster/SearchData?Id=' + treeNodeId;
        });
        //默认选中第一行
        grid.store.on("load", function () {
            grid.getSelectionModel().selectFirstRow(); //用这个就可以了，前提是你要有SelectionModel的实现
        }); 

        var viewport = new Ext.Viewport({
            layout: 'border',
            id: 'viewport',
            items: [
                {
                    region: 'center',
                    id: 'eastOrganization',
                    bodyStyle: 'border-top:0px;border-bottom:0px',
                    border:false,
                    layout: 'fit',
                    items: [grid]
                }]
        });

});