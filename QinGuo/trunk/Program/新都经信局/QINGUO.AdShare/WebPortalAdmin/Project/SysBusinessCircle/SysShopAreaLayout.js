Ext.onReady(function () {

    /*
    * ================页面布局=======================
    */
    /*树形菜单*/
    var tree = new Ext.tree.TreePanel({
        title: '地区列表',
        width: 200,
        id:'tree',
        autoScroll: true,
        layout: 'fit',
        animate: true,
        enableDD: true,
        bodyStyle: 'padding-top:4px;overflow:auto',
        lines: true, //显示树形控件的前导线
        containerScroll: true,
        tbar: [
               {
                   xtype: 'textfield',
                   emptyText: '输入区域名称....',
                   id: 'cityText',
                   width: 140,
                   enableKeyEvents: true,
                   listeners: {
                       'keyup': function (val) {
                           if (val.getValue()) {
                               tree.loader.dataUrl = "/SysBusinessCircle/SearchTreeData?name=" + encodeURIComponent(val.getValue());
                           }
                           else {
                               tree.loader.dataUrl = "/SysBusinessCircle/SearchTreeData";
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
                             tree.loader.dataUrl = "/SysBusinessCircle/SearchTreeData?name=" + encodeURIComponent(name);
                         }
                         else {
                             tree.loader.dataUrl = "/SysBusinessCircle/SearchTreeData";
                         }
                         tree.root.reload();
                     }
                 }
            ],
        loader: new Ext.tree.TreeLoader({
            dataUrl: '/SysBusinessCircle/SearchTreeData'
        }),
        root: {
            nodeType: 'async',
            text: '区域列表导航',
            id: 'root',
            draggable: false,
            expanded: true
        },
        listeners: {
            afterrender: function (tree) {
            },
            click: treeitemclick
        }
    });


    var west = new Ext.Panel({
        region: 'west',
        layoutConfig: {
            animate: true
        },
        animCollapse: true,
        split: true,
        width: 200,
        minSize: 175,
        maxSize: 400,
        margins: '0 5 0 0',
        layout: 'fit',
        bodyStyle: 'border-left:0px;border-top:0px;border-bottom:0px;',
        items: new Ext.TabPanel({
            border: false,
            activeTab: 0,
            tabPosition: 'bottom',
            items: [tree]
        })
    });

    //转义列
    var SysBusinessCircle = Ext.data.Record.create([
              { name: "ID", type: "string", mapping: "ID" },
              { name: "BName", type: "string", mapping: "BName" },
              { name: "BParentID", type: "string", mapping: "BParentID" },
              { name: "CityID", type: "string", mapping: "CityID" }
            ]);

    //数据源
//    if (labelCatObj.length > 0) {
//        treeNodeId = labelCatObj[0].id;
//    }
    var store = GridStore(SysBusinessCircle, '/SysBusinessCircle/SearchShopData?TypeId=' + treeNodeId);

    //快捷查询,如果不需要,可以不用定义
    searchData = [['全查询', ''], ['商圈名称', 'BName']];

    var center = new Ext.grid.GridPanel({
        id: 'gg',
        region: 'center',
        layout: 'fit',
        store: store,
        stripeRows: true, //隔行颜色不同
        enableDragDrop: true, //禁用才能选择复选框
        loadMask: { msg: '数据请求中，请稍后...' },
        defult: {
            sortable: false
        },
        columns: [
                new Ext.grid.RowNumberer({ header: '', width: 25 }), //设置行号
                {
                header: "商圈名称",
                dataIndex: 'BName',
                width: 140,
                menuDisabled: true

            }
                ],
            tbar: tbar('SysShopArea'),
        bbar: bbar(store), //分页工具条
        viewConfig: {
            forceFit: true,
            enableRowBody: true,
            scrollOffset: 0 //不加这个的话，会在grid的最右边有个空白，留作滚动条的位置
        }
    });


    // 响应加载前事件，传递node参数 
    center.store.on('beforeload', function (node) {
        center.getStore().proxy.conn.url = '/SysBusinessCircle/SearchShopData?TypeId=' + treeNodeId;
    });

    //viewport
    var viewport = new Ext.Viewport({
        layout: 'border',
        items: [west, center],
        listeners: {
            afterrender: function () {
//                if (tree.getRootNode().firstChild) {
//                    tree.getRootNode().firstChild.select(); //默认选中tree第一个节点
//                }
            }
        }
    });

});