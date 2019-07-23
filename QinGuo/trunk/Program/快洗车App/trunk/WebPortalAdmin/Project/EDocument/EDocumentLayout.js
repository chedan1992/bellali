Ext.onReady(function () {

    /*
    * ================页面布局=======================
    */
    //定义树的加载器 
    var treeloader = new Ext.tree.TreeLoader({
        url: "/EDocument/GetTreeList"
    });

    var tree = new Ext.tree.TreePanel({
        region: 'center',
        id: 'tr',
        layout: 'fit',
        width:200,
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
            dataUrl: '/EDocument/GetTreeList'
        }),
        tbar: [
               {
                   xtype: 'textfield',
                   emptyText: '输入品牌名称....',
                   id: 'cityText',
                   width: 140,
                   enableKeyEvents: true,
                   listeners: {
                       'keyup': function (val) {
                           if (val.getValue()) {
                               tree.loader.dataUrl = "/EDocument/GetTreeList?BrandName=" + encodeURIComponent(val.getValue());
                           }
                           else {
                               tree.loader.dataUrl = "/EDocument/GetTreeList";
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
                             tree.loader.dataUrl = "/EDocument/GetTreeList?BrandName=" + encodeURIComponent(name);
                         }
                         else {
                             tree.loader.dataUrl = "/EDocument/GetTreeList";
                         }
                         tree.root.reload();
                     }
                 }
            ],
        root: {
            nodeType: 'async',
            text: '品牌列表',
            draggable: false,
            id: 'top'//区分是否根节点
        },
        listeners: {
            click: treeitemclick
        }
    });
    tree.expandAll();


    //数据源
    var className = ''; //页面类名
    if (this.frameElement) {
        className = this.frameElement.name
    }
    //转义列
    var EDocument = Ext.data.Record.create([
              { name: "Id", type: "string", mapping: "Id" },
              { name: "Name", type: "string", mapping: "Name" },
              { name: "Content", type: "string", mapping: "Content" },
              { name: "Mark", type: "string", mapping: "Mark" },
              { name: "BrandId", type: "string", mapping: "BrandId" },
              { name: "BrandName", type: "string", mapping: "BrandName" },
              { name: "ModelName", type: "string", mapping: "ModelName" },
              { name: "ReadNum", type: "int", mapping: "ReadNum" },
              { name: "Status", type: "int", mapping: "Status" },
              { name: "CreateTime", type: "datetime", mapping: "CreateTime" }
            ]);
    //数据源
    var store = GridStore(EDocument, '/EDocument/SearchData?Id=-1', className);
    //快捷查询,如果不需要,可以不用定义
    searchData = [['全查询', ''], ['文档标题', 'Name'], ['文档简介', 'Mark']];

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
                    header: '操作',
                    dataIndex: 'Id',
                    width: 60,
                    align: 'center',
                    menuDisabled: true,
                    renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                        return "<a herf='#' onclick='LookInfo(\"" + value + "\")'><span style='color:blue;cursor:pointer'>查看</span></a>"
                    }
                },
                {
                header: "文档标题",
                dataIndex: 'Name',
                width: 180,
                menuDisabled: true,
                renderer: function (data, metadata, record, rowIndex, columnIndex, store) {
                    if (data) {
                        metadata.attr = 'ext:qtip="' + data + '"';
                        return data;
                    }
                }
            },
                {
                    header: '简介',
                    dataIndex: 'Mark',
                    width:130,
                    menuDisabled: true
                },
                 {
                    header: '所属品牌',
                    dataIndex: 'BrandName',
                    width:80,
                    menuDisabled: true
                },
                {
                    header: '所属型号',
                    dataIndex: 'ModelName',
                    width:80,
                    menuDisabled: true
                },
                  {
                      header: '状态',
                      dataIndex: 'Status',
                      align: 'center',
                      width:60,
                      menuDisabled: true,
                      renderer: formartEnableOrDisable
                  },
               {
                   header: '阅读量',//开始时间进行排序
                   dataIndex: 'ReadNum',
                   width:60,
                   menuDisabled: true
               },
                {
                    header: '添加时间',
                    dataIndex: 'CreateTime',
                    width: 120,
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
        grid.getStore().proxy.conn.url = '/EDocument/SearchData?Id=' + treeNodeId;
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
                collapsible: true,
                //title: '品牌列表',
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