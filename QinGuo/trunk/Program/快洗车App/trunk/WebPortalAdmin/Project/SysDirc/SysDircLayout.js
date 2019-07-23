Ext.onReady(function () {

    //数据源
    var className = ''; //页面类名
    if (this.frameElement) {
        className = this.frameElement.name
    }
    //转义列
    var EDynamic = Ext.data.Record.create([
             { name: "Id", type: "string", mapping: "Id" },
             { name: "Name", type: "string", mapping: "Name" },
             { name: "OrderNum", type: "Int", mapping: "OrderNum" },
             { name: "CreateTime", type: "datetime", mapping: "CreateTime" }
            ]);
    //数据源
    var store = GridStore(EDynamic, '/SysDirc/SearchData', className, "OrderNum", "asc");

    //快捷查询,如果不需要,可以不用定义
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
                 header: "类别名称",
                 dataIndex: 'Name',
                 flex: 1,
                 sortable: true,
                 menuDisabled: true
             },
                {
                    header: "排序",
                    dataIndex: 'OrderNum',
                    flex: 1,
                    sortable: true,
                    menuDisabled: true
                },
                {
                    header: '添加时间',
                    dataIndex: 'CreateTime',
                    flex: 1,
                    menuDisabled: true,
                    sortable: true,
                    renderer: renderCreateTime
                }
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
        grid.getStore().proxy.conn.url = '/SysDirc/SearchData?Type=' + treeNodeId;
    });
    //默认选中第一行
    grid.store.on("load", function () {
        grid.getSelectionModel().selectFirstRow(); //用这个就可以了，前提是你要有SelectionModel的实现
    });
    grid.addListener('rowdblclick', dbGridClick);

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
            text: '字典类别',
            expanded: true,
            children: [
                        {
                            text: '仓库类别',
                            id: '0',
                            leaf: true
                        },
                         {
                             text: '供应商类别',
                             id: '1',
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
                treeNodeId = tree.getRootNode().childNodes[0].id;
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
                title: '字典类别',
                collapsed: false, //默认折叠
                collapseMode: 'mini', //出现小箭头
                split: true,
                width: 200,
                minSize: 175,
                bodyStyle: 'border-left:0px;border-top:0px;border-bottom:0px',
                deferredRender: false,
                items: [tree]
            })],
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
