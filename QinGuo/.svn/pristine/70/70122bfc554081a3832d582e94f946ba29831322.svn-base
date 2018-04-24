Ext.onReady(function () {
    //    //定义树的加载器 
    //    var treeloader = new Ext.tree.TreeLoader({
    //        url: "/Organizational/SearchData"
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
    //            dataUrl: '/Organizational/SearchData'
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
    //    var Role = Ext.data.Record.create([
    //              { name: "Id", type: "string", mapping: "Id" },
    //              { name: "Name", type: "string", mapping: "Name" },
    //              { name: "RoleType", type: "int", mapping: "RoleType" },
    //              { name: "RoleSort", type: "int", mapping: "RoleSort" },
    //              { name: "Introduction", type: "string", mapping: "Introduction" },
    //			  { name: "CreateTime", type: "datetime", mapping: "CreateTime", format: "yy-mm-dd HH:MM:ss" },
    //              { name: "CompanyName", type: "string", mapping: "CompanyName" },
    //              { name: "CreaterId", type: "string", mapping: "CreaterId" }
    //            ]);
    //    //数据源
    //    var className = '';//页面类名
    //    if (this.frameElement) {
    //        className=this.frameElement.name
    //    }
    //    var store = GridStore(Role, '/RoleManage/SearchData?Id=-1', 'RoleManage');

    //    //快捷查询,如果不需要,可以不用定义
    //    searchData = [['全查询', ''], ['角色名称', 'Name'], ['描述', 'Introduction'], ['添加时间', 'CreateTime']];

    //    var grid = new Ext.grid.GridPanel({
    //        region: 'center',
    //        layout: 'fit',
    //        id: 'gg',
    //        store: store,
    //        stripeRows: true, //隔行颜色不同
    //        border: false,
    //        enableDragDrop: true, //禁用才能选择复选框
    //        loadMask: { msg: '数据请求中，请稍后...' },
    //        columns: [
    //                new Ext.grid.RowNumberer({ header: '', width: 25}), //设置行号
    //                {
    //                header: "角色名称",
    //                dataIndex: 'Name',
    //                width: 140,
    //                menuDisabled: true
    //            },
    //            {
    //                header: '所属公司',
    //                dataIndex: 'CompanyName',
    //                width: 160,
    //                menuDisabled: true
    //            },
    //			 {
    //			     header: '添加时间',
    //			     dataIndex: 'CreateTime',
    //			     flex: 3,
    //			     menuDisabled: true,
    //			     renderer: renderCreateTime
    //			 },
    //           
    //			 {
    //			     header: '功能权限',
    //			     dataIndex: 'Id',
    //			     align: 'center',
    //			     width: 100,
    //			     menuDisabled: true,
    //			     renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
    //			         return "<a herf='#' onclick='setOpearaRoles(\"" + value + "\")'><span style='color:blue;cursor:pointer'>菜单权限设置</span></a>"
    //			     }
    //			 },
    //			{
    //			    header: '范围权限',
    //			    dataIndex: 'Id',
    //			    align: 'center',
    //			    width: 100,
    //			    menuDisabled: true,
    //			    renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
    //			       return "<a herf='#' onclick='setRangeRoles(\"" + value + "\")'><span style='color:blue;cursor:pointer'>数据权限设置</span></a>"
    //			    }
    //			}
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
    //        grid.getStore().proxy.conn.url = '/RoleManage/SearchData?Id=' + treeNodeId;
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
    //        });




    //转义列
    var Role = Ext.data.Record.create([
              { name: "Id", type: "string", mapping: "Id" },
              { name: "Name", type: "string", mapping: "Name" },
              { name: "RoleType", type: "int", mapping: "RoleType" },
              { name: "RoleSort", type: "int", mapping: "RoleSort" },
              { name: "Introduction", type: "string", mapping: "Introduction" },
			  { name: "CreateTime", type: "datetime", mapping: "CreateTime", format: "yy-mm-dd HH:MM:ss" },
              { name: "CompanyName", type: "string", mapping: "CompanyName" },
              { name: "CreaterId", type: "string", mapping: "CreaterId" }
            ]);
    //数据源
    var className = ''; //页面类名
    if (this.frameElement) {
        className = this.frameElement.name
    }
    var store = GridStore(Role, '/RoleManage/SearchData?Id=-1', className, "", "");

    //快捷查询,如果不需要,可以不用定义
    searchData = [['全查询', ''], ['角色名称', 'Sys_Role.Name'],['添加时间', 'Sys_Role.CreateTime']];
    var gridId = 'gg';
    var grid = new Ext.grid.GridPanel({
        region: 'center',
        layout: 'fit',
        id: gridId,
        store: store,
        stripeRows: true, //隔行颜色不同
        border: false,
        enableDragDrop: true, //禁用才能选择复选框
        loadMask: { msg: '数据请求中，请稍后...' },
        columns: [
                new Ext.grid.RowNumberer({ header: '', width: 25 }), //设置行号
                {
                header: "角色名称",
                dataIndex: 'Name',
                width: 140,
                sortable: true,
                menuDisabled: true
            },
			 {
			     header: '添加时间',
			     dataIndex: 'CreateTime',
			     flex: 3,
			     sortable: true,
			     menuDisabled: true,
			     renderer: renderCreateTime
			 },

			 {
			     header: '功能权限',
			     dataIndex: 'Id',
			     align: 'center',
			     width: 100,
			     menuDisabled: true,
			     renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
			         return "<a herf='#' onclick='setOpearaRoles(\"" + value + "\")'><span style='color:blue;cursor:pointer'>菜单权限设置</span></a>"
			     }
			 }
        //			{
        //			    header: '范围权限',
        //			    dataIndex: 'Id',
        //			    align: 'center',
        //			    width: 100,
        //			    menuDisabled: true,
        //			    renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
        //			        return "<a herf='#' onclick='setRangeRoles(\"" + value + "\")'><span style='color:blue;cursor:pointer'>数据权限设置</span></a>"
        //			    }
        //			}
                ],
        tbar: tbar(className),
        bbar: bbar(store, gridId, className), //分页工具条
        viewConfig: {
            forceFit: true,
            enableRowBody: true,
            scrollOffset: 0 //不加这个的话，会在grid的最右边有个空白，留作滚动条的位置
        }
    });
    // 响应加载前事件，传递node参数 
    grid.store.on('beforeload', function (node) {
        grid.getStore().proxy.conn.url = '/RoleManage/SearchData?Id=' + treeNodeId;
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
                border: false,
                items: [grid]
            }
            ],
        listeners: {
            afterrender: function () {
                //获取当前用户所能看到的列信息
                Ext.Ajax.request({
                    url: '/RoleManage/GetGridColumn',
                    params: { moudel: className },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            var grid = Ext.getCmp(gridId);
                            if (rs.data != "") {
                                var result = eval('(' + rs.data + ')');
                                columnHide = result.ColumnId;
                                var ColumnId = result.ColumnId.split(',');
                                for (var j = 0; j < ColumnId.length; j++) {
                                    if (ColumnId[j] != "") {
                                        //获取列的索引
                                        var index = getGridIndex(ColumnId[j]);
                                        grid.getColumnModel().setHidden(index, true);
                                    }
                                }
                                function getGridIndex(dataIndex) {
                                    var column = grid.getColumnModel();
                                    for (var i = 0; i < column.config.length; i++) {
                                        if (column.config[i].dataIndex != "") {
                                            if (column.config[i].dataIndex == dataIndex) {
                                                return i;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
            }
        }
    });

});






