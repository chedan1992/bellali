
Ext.onReady(function () {
    /*
    * ================页面布局=======================
    */
    /*树形菜单*/
    var tree = new Ext.tree.TreePanel({
        title: '省份列表',
        width: 200,
        id: 'tree',
        autoScroll: true,
        layout: 'fit',
        expanded: true,
        border:false,
        animate: true,
        enableDD: true,
        bodyStyle: 'padding-top:4px;overflow:auto',
        lines: true, //显示树形控件的前导线 
        containerScroll: true,
        loader: new Ext.tree.TreeLoader({
            dataUrl: '/SysBusinessCircle/SearchProvinceByTree'
        }),
        root: {
            nodeType: 'async',
            text: '省份列表导航',
            id: 'root',
            draggable: false,
            expanded: true
        },
        tbar: [
               {
                   xtype: 'textfield',
                   emptyText: '输入省份名称或字母....',
                   id: 'cityText',
                   width: 140,
                   enableKeyEvents: true,
                   listeners: {
                       'keyup': function (val) {
                           if (val.getValue()) {
                               tree.loader.dataUrl = "/SysBusinessCircle/SearchProvinceByTree?name=" + encodeURIComponent(val.getValue());
                           }
                           else {
                               tree.loader.dataUrl = "/SysBusinessCircle/SearchProvinceByTree";
                           }
                           tree.root.reload();
                       }
                   }
               },
                '->',
                 {
                     iconCls: 'GTP_refresh',
                     text: '刷新',
                     tooltip: '刷新城市',
                     handler: function () {
                         var name = Ext.getCmp("cityText").getValue();
                         if (name) {
                             tree.loader.dataUrl = "/SysBusinessCircle/SearchProvinceByTree?name=" + encodeURIComponent(name);
                         }
                         else {
                             tree.loader.dataUrl = "/SysBusinessCircle/SearchProvinceByTree";
                         }
                         tree.root.reload();
                     }
                 }
            ],
        listeners: {
            click: treeitemclick,
            afterrender: function () {

            }
        }
    });
    tree.expandPath();

    var west = new Ext.Panel({
        region: 'west',
        id: 'TreeWest',
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
              { name: "Id", type: "string", mapping: "Id" },
              { name: "Name", type: "string", mapping: "Name" },
              { name: "ProvinceCode", type: "string", mapping: "ProvinceCode" },
              { name: "Code", type: "string", mapping: "Code" },
              { name: "Status", type: "int", mapping: "Status" }
            ]);
    var store = GridStore(SysBusinessCircle, '/SysBusinessCircle/SearchData?ProvinceCode=' + treeNodeId);

    //快捷查询,如果不需要,可以不用定义
    searchData = [['全查询', ''], ['区域名称', 'Name'], ['状态', 'Status']];
    var className = ''; //页面类名
    if (this.frameElement) {
        className = this.frameElement.name
    }
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
                header: "市区名称",
                dataIndex: 'Name',
                width: 140,
                menuDisabled: true
            },
                {
                    header: "状态",
                    dataIndex: 'Status',
                    width: 140,
                    menuDisabled: true,
                    renderer: formartEnableOrDisable
                }
                ],
        tbar: tbar(className),
        bbar: bbar(store), //分页工具条
        viewConfig: {
            forceFit: true,
            enableRowBody: true,
            scrollOffset: 0 //不加这个的话，会在grid的最右边有个空白，留作滚动条的位置
        },
        listeners: {
            beforerender: function () {
            }
        }
    });

    // 响应加载前事件，传递node参数 
    center.store.on('beforeload', function (node) {
        center.getStore().proxy.conn.url = '/SysBusinessCircle/SearchData?ProvinceCode=' + treeNodeId;
    });


    //viewport
    var viewport = new Ext.Viewport({
        layout: 'border',
        items: [west, center],
        listeners: {
            afterrender: function () {
            }
        }
    });
});