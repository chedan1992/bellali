//单位公司
Ext.onReady(function () {
    /*
    * ================页面布局=======================
    */
    //定义树的加载器 
    var treeloader = new Ext.tree.TreeLoader({
        url: "/Organizational/SearchData"
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
            dataUrl: '/Organizational/SearchData'
        }),
        root: {
            nodeType: 'async',
            text: '组织架构',
            iconCls: 'GTP_org',
            draggable: false,
            id: 'top'//区分是否根节点
        },
        listeners: {
            click: treeitemclick
        }
    });
    tree.expandAll();


    //快捷查询,如果不需要,可以不用定义
    var className = ''; //页面类名
    if (this.frameElement) {
        className = this.frameElement.name
    }
    //转义列
    var Member = Ext.data.Record.create([
              { name: "Id", type: "string", mapping: "Id" },
              { name: "Name", type: "string", mapping: "Name" },
              { name: "CompanyName", type: "string", mapping: "CompanyName" },
              { name: "CreateTime", type: "datetime", mapping: "CreateTime" }
    ]);
    //数据源
    var store = GridStore(Member, '/SysGroup/SearchDataQuestions', className);

    var grid = new Ext.grid.GridPanel({
        region: 'center',
        layout: 'fit',
        id: 'gg',
        store: store,
        stripeRows: true, //隔行颜色不同
        border: false,
        defult: {
            sortable: false
        },
        loadMask: { msg: '数据请求中，请稍后...' },
        columns: [
                new Ext.grid.RowNumberer({ header: '', width: 25 }), //设置行号

                {
                header: "类别名称",
                dataIndex: 'Name',
                flex:1,
                menuDisabled: true
            },
             {
                 header: "所属单位",
                 dataIndex: 'CompanyName',
                 flex:1,
                 menuDisabled: true
             },
                    {
                        header: '添加时间',
                        dataIndex: 'CreateTime',
                        flex:1,
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

    grid.addListener('rowdblclick', dbGridClick);
    //默认选中第一行
    grid.store.on("load", function () {
        grid.getSelectionModel().selectFirstRow(); //用这个就可以了，前提是你要有SelectionModel的实现
    });

    // 响应加载前事件，传递node参数 
    grid.store.on('beforeload', function (node) {
        grid.getStore().proxy.conn.url = '/SysGroup/SearchDataQuestions?Id=' + treeNodeId;
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
                title: '组织架构',
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


//新增
function AddDate() {
    var grid = Ext.getCmp("tr");
    var rows = grid.getSelectionModel().getSelectedNode();
    var CreateCompanyId = '0';
    if (rows) {
        if (rows.attributes.Attribute == 1) {
            CreateCompanyId = rows.attributes.id;
        }
        else {
            MessageInfo("请先选择单位！", "statusing");
            return false;
        }
    }
    else {
        MessageInfo("请先选择单位！", "statusing");
        return false;
    }
    url = '/SysAppointed/SaveData?CreateCompanyId=' + CreateCompanyId;
    var win = load("新增", "");
    win.show();
}











