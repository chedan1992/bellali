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
                  { name: "ParentId", type: "string", mapping: "ParentId" },
              { name: "CreateTime", type: "datetime", mapping: "CreateTime" }
    ]);
    //数据源
    var store = GridStore(EDynamic, '/SysDirc/SearchData?id=0', className, "OrderNum", "asc");

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
        bbar: bbar(store), //分页工具条
        viewConfig: {
            forceFit: true,
            enableRowBody: true,
            scrollOffset: 0 //不加这个的话，会在grid的最右边有个空白，留作滚动条的位置
        }
    });
    // 响应加载前事件，传递node参数 
    grid.store.on('beforeload', function (node) {
        treeNodeId = treeNodeId == -1 ? 0 : treeNodeId;
        grid.getStore().proxy.conn.url = '/SysDirc/SearchData?id=' + treeNodeId;
    });
    //默认选中第一行
    grid.store.on("load", function () {
        grid.getSelectionModel().selectFirstRow(); //用这个就可以了，前提是你要有SelectionModel的实现
    });


    /*
    * ================页面布局=======================
    */
    //定义树的加载器 
    var treeloader = new Ext.tree.TreeLoader({
        url: "/SysDirc/GetTree"
    });

    var tree = new Ext.tree.TreePanel({
        region: 'center',
        id: 'tree',
        layout: 'fit',
        width: 200,
        layoutConfig: {
            animate: true
        },
        autoScroll: true,
        animate: true, //动画效果  
        //rootVisible: false, //根节点是否可见  
        lines: true, //显示树形控件的前导线
        containerScroll: true,
        border: false,
        bodyStyle: 'padding-top:5px',
        loader: new Ext.tree.TreeLoader({
            dataUrl: '/SysDirc/GetTree'
        }),
        tbar: [
               {
                   xtype: 'textfield',
                   emptyText: '输入栏目名称....',
                   id: 'cityText',
                   width: 140,
                   enableKeyEvents: true,
                   listeners: {
                       'keyup': function (val) {
                           if (val.getValue()) {
                               tree.loader.dataUrl = "/SysDirc/GetTree?Name=" + encodeURIComponent(val.getValue());
                           }
                           else {
                               tree.loader.dataUrl = "/SysDirc/GetTree";
                           }
                           tree.root.reload();
                       }
                   }
               },
                '->',
                 {
                     iconCls: 'GTP_refresh',
                     text: '刷新',
                     tooltip: '刷新区域',
                     handler: function () {
                         var name = Ext.getCmp("cityText").getValue();
                         if (name) {
                             tree.loader.dataUrl = "/SysDirc/GetTree?Name=" + encodeURIComponent(name);
                         }
                         else {
                             tree.loader.dataUrl = "/SysDirc/GetTree";
                         }
                         tree.root.reload();
                     }
                 }
        ],
        root: {
            nodeType: 'async',
            text: '设置栏目',
            draggable: false,
            id: '0'//区分是否根节点
        },
        listeners: {
            click: treeitemclick
        },
        afterRender: function () {//默认选中
            Ext.tree.TreePanel.prototype.afterRender.apply(this, arguments);
            this.getSelectionModel().select(this.getRootNode());
        }
    });
    tree.expandAll();

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
                title: '栏目管理',
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
