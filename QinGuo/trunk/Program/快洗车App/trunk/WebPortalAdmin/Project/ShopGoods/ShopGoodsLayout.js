//列表公司
Ext.onReady(function () {
    //定义树的加载器 
    var treeloader = new Ext.tree.TreeLoader({
        url: "/Organizational/SearchPostData"
    });

    var tree = new Ext.tree.TreePanel({
        region: 'center',
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
            dataUrl: '/ShopCategory/Comboboxtree?key='
        }),
        root: {
            nodeType: 'async',
            text: '商品分类',
            expanded:true,
            //iconCls: 'GTP_home',
            draggable: false,
            id: '-1'//区分是否根节点
        },
        title:'&nbsp;',
        listeners: {
            click: treeitemclick
        }
    });
   // tree.expandAll();

    //快捷查询,如果不需要,可以不用定义
    var className = ''; //页面类名
    if (this.frameElement) {
        className = this.frameElement.name
    }
    //转义列
    var Member = Ext.data.Record.create([
              { name: "Id", type: "string", mapping: "Id" },
              { name: "ProName", type: "string", mapping: "ProName" },
              { name: "Code", type: "string", mapping: "Code" },
              { name: "CategoryId", type: "string", mapping: "CategoryId" },
              { name: "ProdectTypeId", type: "string", mapping: "ProdectTypeId" },
              { name: "shorthand", type: "string", mapping: "shorthand" },
              { name: "specification", type: "string", mapping: "specification" },
              { name: "ProOldPrice", type: "string", mapping: "ProOldPrice" },
              { name: "CreateTime", type: "datetime", mapping: "CreateTime" },
              { name: "Status", type: "string", mapping: "Status" }
            ]);
    //数据源
    var store = GridStore(Member, '/ShopGoods/SearchData?CategoryId=-1', className);

    //快捷查询,如果不需要,可以不用定义
    searchData = [['全查询', ''], ['电梯名称', 'Name'], ['电梯编号', 'Code'], ['电梯型号', 'Model']];
  
    var grid = new Ext.grid.GridPanel({
        region: 'center',
        layout: 'fit',
        id: 'gg',
        store: store,
        stripeRows: true, //隔行颜色不同
        border: false,
        loadMask: { msg: '数据请求中，请稍后...' },
        columns: [
            new Ext.grid.RowNumberer({ header: '', width: 25 }), //设置行号
            {
            header: "商品名称",
            dataIndex: 'ProName',
            width: 120,
            sortable: false,
            menuDisabled: true,
            renderer: function (data, metadata, record, rowIndex, columnIndex, store) {
                if (data) {
                    metadata.attr = 'ext:qtip="点击查看详细"';
                    return "<span style='color:blue;cursor:pointer;' onclick='clickDetail()'>" + data + "</span>";
                }
            }
        }, {
            header: "商品编码",
            width: 100,
            dataIndex: 'Code',
            sortable: false,
            menuDisabled: true
        }, {
            header: "助记码",
            width: 100,
            dataIndex: 'shorthand',
            sortable: false,
            menuDisabled: true
        },
        {
            header: "规格型号",
            width: 100,
            dataIndex: 'specification',
            sortable: false,
            menuDisabled: true,
            renderer: ElevatorType
        },
        {
            header: '状态',
            sortable: false,
            dataIndex: 'Status',
            menuDisabled: true,
            width: 60,
            renderer: formartEnableOrDisable
        },
        {
                header: '添加时间',
                sortable: false,
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
        grid.getStore().proxy.conn.url = '/ShopGoods/SearchData?CategoryId=' + treeNodeId;
    });

    grid.addListener('rowdblclick', dbGridClick);
    //默认选中第一行
    grid.store.on("load", function () {
        grid.getSelectionModel().selectFirstRow(); //用这个就可以了，前提是你要有SelectionModel的实现
    }); 
    //viewport
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
                width:200,
                minSize: 175,
                bodyStyle: 'border-left:0px;border-top:0px;border-bottom:0px',
                deferredRender: false,
                items: [tree]
            })]
    });
});












