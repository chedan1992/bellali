Ext.onReady(function () {
    /*
    * ================页面布局=======================
    */
    //定义树的加载器 
    var treeloader = new Ext.tree.TreeLoader({
        url: "/Organizational/SearchPostData"
    });

    var tree = new Ext.tree.TreePanel({
        id: 'tr',
        layout: 'fit',
        layoutConfig: {
            animate: true
        },
        autoScroll: true,
        animate: true, //动画效果  
        rootVisible: true, //根节点是否可见  
        lines: true, //显示树形控件的前导线
        containerScroll: true,
        border: false,
        bodyStyle: 'padding-top:5px',
        loader: new Ext.tree.TreeLoader({
            dataUrl: '/Organizational/SearchPostData'
        }),
        tbar: [
               {
                   xtype: 'textfield',
                   emptyText: '输入单位/部门名称....',
                   id: 'cityText',
                   width: 140,
                   enableKeyEvents: true,
                   listeners: {
                       'keyup': function (val) {
                           if (val.getValue()) {
                               tree.loader.dataUrl = "/Organizational/SearchPostData?BrandName=" + encodeURIComponent(val.getValue());
                           }
                           else {
                               tree.loader.dataUrl = "/Organizational/SearchPostData";
                           }
                           tree.root.reload();
                       }
                   }
               },
                '->',
                 {
                     iconCls: 'GTP_refresh',
                     text: '刷新',
                     tooltip: '刷新单位/部门',
                     handler: function () {
                         var name = Ext.getCmp("cityText").getValue();
                         if (name) {
                             tree.loader.dataUrl = "/Organizational/SearchPostData?BrandName=" + encodeURIComponent(name);
                         }
                         else {
                             tree.loader.dataUrl = "/Organizational/SearchPostData";
                         }
                         treeNodeId = "-1";
                         Ext.getCmp("gg").store.reload();
                         tree.root.reload();
                     }
                 }
            ],
        root: {
            nodeType: 'async',
            text: '组织架构',
            iconCls: 'GTP_org',
            draggable: false,
            expanded: true, //此处展开所有。
            id: 'top'//区分是否根节点
        },
        listeners: {
            click: treeitemclick,
            afterrender: function () {
                var creator = getLoginUser(); //获取用户信息
                if (creator) {
                    if (creator.Attribute == 1)//系统管理员
                    {
                        var node = tree.getRootNode().childNodes;
                        for (var i = 0; i < node.length; i++) {
                            node[i].expand();
                        }
                    }
                    else {
                        tree.expandAll();
                    }
                }

            }
        }
    });



    //转义列
    var Master = Ext.data.Record.create([
              { name: "Id", type: "string", mapping: "Id" },
              { name: "LoginName", type: "string", mapping: "LoginName" },
              { name: "UserName", type: "string", mapping: "UserName" },
              { name: "Sex", type: "int", mapping: "Sex" },
              { name: "Cid", type: "string", mapping: "Cid" },
              { name: "Pwd", type: "string", mapping: "Pwd" },
              { name: "Email", type: "string", mapping: "Email" },
              { name: "Phone", type: "string", mapping: "Phone" },
              { name: "Status", type: "int", mapping: "Status" },
              { name: "IsMain", type: "bool", mapping: "IsMain" },
              { name: "HeadImg", type: "string", mapping: "HeadImg" },
              { name: "CardNum", type: "string", mapping: "CardNum" },
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
    var store = GridStore(Master, '/UserStaff/SearchData?OrganizaId=-1', className);

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
                 header: "操作",
                 dataIndex: 'Id',
                 width:70,
                 sortable: false,
                 menuDisabled: true,
                 renderer: function (value, meta, record, rowIdx, colIdx, store) {
                     var content = '<a href="#" title="设备查看" style="vertical-align: middle;cursor:pointer" onclick="LookAppointed()"><img style="vertical-align: middle; width:18px; height:18px;border:0" class="GTP_view"  src="../../Content/Extjs/resources/images/default/s.gif"/>设备</a>&nbsp;|&nbsp;<a href="#" title="一键交接" style="vertical-align: middle;cursor:pointer" onclick="ChangeUser()"><img style="vertical-align: middle; width:18px; height:18px;border:0" class="GTP_trustaccept"  src="../../Content/Extjs/resources/images/default/s.gif"/>交接</a>';
                     return content;
                 }
             },
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
                },
        //            {
        //                header: "联系电话",
        //                dataIndex: 'Phone',
        //                flex: 2,
        //                menuDisabled: true
        //            },
            {
            header: "身份证号码",
            dataIndex: 'CardNum',
            flex: 4,
            menuDisabled: true
        },
            {
                header: "状态",
                dataIndex: 'Status',
                align: 'center',
                width: 70,
                renderer: formartEnableOrDisable,
                menuDisabled: true
            },
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
        grid.getStore().proxy.conn.url = '/UserStaff/SearchData?OrganizaId=' + treeNodeId;
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

                layout: 'fit',
                items: [grid]
            },
            new Ext.Panel({
                region: 'west',
                layout: 'fit',
                collapsible: false,
                collapsed: false, //默认折叠
                collapseMode: 'mini', //出现小箭头
                split: true,
                width: 230,
                minSize: 175,
                bodyStyle: 'border-left:0px;border-top:0px;border-bottom:0px',
                deferredRender: false,
                items: [tree]
            })]
    });

});

//Ext.onReady(function () {
//    //员工管理
//    //转义列
//    var Master = Ext.data.Record.create([
//              { name: "Id", type: "string", mapping: "Id" },
//              { name: "LoginName", type: "string", mapping: "LoginName" },
//              { name: "UserName", type: "string", mapping: "UserName" },
//              { name: "OperateNum", type: "string", mapping: "OperateNum" },
//              { name: "CardNum", type: "string", mapping: "CardNum" },
//              { name: "Sex", type: "int", mapping: "Sex" },
//              { name: "Pwd", type: "string", mapping: "Pwd" },
//              { name: "Email", type: "string", mapping: "Email" },
//              { name: "Phone", type: "string", mapping: "Phone" },
//              { name: "HeadImg", type: "string", mapping: "HeadImg" },
//              { name: "Status", type: "int", mapping: "Status" },
//              { name: "IsMain", type: "bool", mapping: "IsMain" },
//              { name: "CreateTime", type: "datetime", mapping: "CreateTime" },
//              { name: "CreaterName", type: "string", mapping: "CreaterName" },
//              { name: "TotalBalance", type: "string", mapping: "TotalBalance" },
//              { name: "InCome", type: "string", mapping: "InCome" },
//               { name: "Alipay", type: "string", mapping: "Alipay" },
//              { name: "OrderCount", type: "string", mapping: "OrderCount" }
//            ]);

//    //快捷查询,如果不需要,可以不用定义
//    searchData = [['全查询', ''], ['用户名称', 'UserName'], ['登录账号', 'LoginName'],['身份证号码', 'CardNum'], ['邮箱', 'Email']];
//    var className = ''; //页面类名
//    if (this.frameElement) {
//        className = this.frameElement.name
//    }
//    //数据源
//    var store = GridStore(Master, '/UserStaff/SearchData?Id=-1', className);

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
//                width:60,
//                renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
//                    if (value) {
//                        return value;
//                    }
//                    else {
//                        return '<span style="color:silver">(' + record.data.LoginName + ')</span>';
//                    }
//                }
//            }, {
//                header: "登录账号",
//                dataIndex: 'LoginName',
//                width:80,
//                menuDisabled: true
//            },
//              {
//                  header: "身份证号码",
//                  dataIndex: 'CardNum',
//                  flex: 4,
//                  menuDisabled: true
//              },
//               {
//                header: "性别",
//                dataIndex: 'Sex',
//                width:50,
//                renderer: formartSex,
//                menuDisabled: true
//            }, {
//                header: "邮箱",
//                dataIndex: 'Email',
//                flex: 2,
//                menuDisabled: true
//            },
//            {
//                header: "状态",
//                dataIndex: 'Status',
//                align: 'center',
//                width: 70,
//                renderer: formartEnableOrDisable,
//                menuDisabled: true
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
//        grid.getStore().proxy.conn.url = '/UserStaff/SearchData?Id=' + treeNodeId;
//    });
//    //默认选中第一行
//    grid.store.on("load", function () {
//        grid.getSelectionModel().selectFirstRow(); //用这个就可以了，前提是你要有SelectionModel的实现
//    }); 
//    var viewport = new Ext.Viewport({
//        layout: 'border',
//        id: 'viewport',
//        items: [
//            {
//                region: 'center',
//                id: 'eastOrganization',
//                bodyStyle: 'border-top:0px;border-bottom:0px',
//                border:false,
//                layout: 'fit',
//                items: [grid]
//            }]
//        });

//});