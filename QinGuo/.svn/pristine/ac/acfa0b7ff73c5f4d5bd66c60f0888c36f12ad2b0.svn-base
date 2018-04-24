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

//    // 响应加载前事件，传递node参数 
//    tree.on('beforeload', function (node) {
//        var ID = (!node.attributes["id"] == true) ? "0" : node.attributes["id"];
//        if (ID == 'top') {
//            ID = '0';
//        }
//        tree.loader.dataUrl = "/Organizational/SearchData"; // 定义子节点的Loader 
//    });


    //转义列
    var Master = Ext.data.Record.create([
              { name: "Id", type: "string", mapping: "Id" },
              { name: "Name", type: "string", mapping: "Name" },
              { name: "Address", type: "string", mapping: "Address" },
              { name: "NameTitle", type: "string", mapping: "NameTitle" },
              { name: "CompanyName", type: "string", mapping: "CompanyName" },
              { name: "Order", type: "string", mapping: "Order" },
              { name: "Status", type: "string", mapping: "Status" },
              { name: "CreateTime", type: "datetime", mapping: "CreateTime" },
              { name: "Code", type: "string", mapping: "Code" },
              { name: "CityName", type: "string", mapping: "CityName" },
              { name: "Phone", type: "string", mapping: "Phone" },
              { name: "LinkUser", type: "string", mapping: "LinkUser" },
              { name: "Email", type: "string", mapping: "Email" },
              { name: "LegalPerson", type: "string", mapping: "LegalPerson" },
              { name: "Introduction", type: "string", mapping: "Introduction" },
              { name: "Type", type: "string", mapping: "Type" },
              { name: "Nature", type: "string", mapping: "Nature" },
              { name: "ComPLon", type: "string", mapping: "ComPLon" },
              { name: "CompLat", type: "string", mapping: "CompLat" }
            ]);
    //数据源
    var store = GridStore(Master, '/Organizational/SearchList?Id=-1');
    var className = ''; //页面类名
    if (this.frameElement) {
        className = this.frameElement.name
    }
    var grid = new Ext.grid.GridPanel({
        id: 'gg',
        store: store,
        border: false,
        stripeRows: true, //隔行颜色不同
        enableDragDrop: true, //禁用才能选择复选框
        loadMask: { msg: '数据请求中，请稍后...' },
        loadMask: false,
        defult: {
            sortable: false
        },
        columns: [
                new Ext.grid.RowNumberer({ header: '', width: 25 }), //设置行号
//                  {
//                  header: "部门编码",
//                  dataIndex: 'Code',
//                  width:70,
//                  sortable: false,
//                  menuDisabled: true
        //              },
                 {
                 header: "操作",
                 dataIndex: 'Id',
                 width: 60,
                 sortable: false,
                 menuDisabled: true,
                 renderer: function (value, meta, record, rowIdx, colIdx, store) {
                     var content = '<a href="#" style="vertical-align: middle;cursor:pointer" onclick="LookAppointed()"><img style="vertical-align: middle; width:18px; height:18px;border:0" class="GTP_view"  src="../../Content/Extjs/resources/images/default/s.gif"/>设备查看</a>';
                     return content;
                 }
                },
                {
                header: "部门名称",
                dataIndex: 'Name',
                width: 140,
                sortable: false,
                menuDisabled: true
            }, {
                header: "部门简称",
                dataIndex: 'NameTitle',
                width: 100,
                sortable: false,
                menuDisabled: true
            },
            {
                header: "联系人",
                dataIndex: 'LinkUser',
                width: 80,
                sortable: false,
                menuDisabled: true
            },
                 {
                     header: "联系电话",
                     dataIndex: 'Phone',
                     flex: 1,
                     sortable: false,
                     menuDisabled: true
                 },
                 {
                     header: '所属单位',
                     sortable: false,
                     dataIndex: 'CompanyName',
                     menuDisabled: true,
                     width:120
                 },
//                 {
//                      header: '排序',
//                      sortable: false,
//                      dataIndex: 'Order',
//                      menuDisabled: true,
//                      width:50
//                  },
                  {
                      header: '状态',
                      sortable: false,
                      dataIndex: 'Status',
                      menuDisabled: true,
                      width: 70,
                      renderer: formartEnableOrDisable
                  }

                  
//                {
//                    header: '添加时间',
//                    sortable: false,
//                    dataIndex: 'CreateTime',
//                    flex: 4,
//                    menuDisabled: true
//                }
                ],
                tbar: tbar(className),
        bbar: bbar(store), //分页工具条
        viewConfig: {
            forceFit: true,
            enableRowBody: true,
            scrollOffset: 0 //不加这个的话，会在grid的最右边有个空白，留作滚动条的位置
        },
        listeners: {

        }
    });

    // 响应加载前事件，传递node参数 
    grid.store.on('beforeload', function (node) {
        grid.getStore().proxy.conn.url = '/Organizational/SearchList?Id=' + treeNodeId;
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
                title:'组织架构',
                collapsed: false, //默认折叠
                collapseMode: 'mini', //出现小箭头
                split: true,
                width:230,
                minSize: 175,
                bodyStyle: 'border-left:0px;border-top:0px;border-bottom:0px',
                deferredRender: false,
                items: [tree]
            })]
    });
});