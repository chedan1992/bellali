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
              { name: "Model", type: "string", mapping: "Model" },
              { name: "Specifications", type: "string", mapping: "Specifications" },
              { name: "Gid", type: "string", mapping: "Gid" },
              { name: "ProductionDate", type: "datetime", mapping: "ProductionDate" },
              { name: "MaintenanceDate", type: "datetime", mapping: "MaintenanceDate" },
              { name: "ResponsibleId", type: "string", mapping: "ResponsibleId" },
              { name: "Mark", type: "string", mapping: "Mark" },
              { name: "QRCode", type: "string", mapping: "QRCode" },
              { name: "Img", type: "string", mapping: "Img" },
              { name: "Status", type: "string", mapping: "Status" },
              { name: "Cid", type: "string", mapping: "Cid" },
               { name: "GroupName", type: "string", mapping: "GroupName" },
              { name: "Places", type: "string", mapping: "Places" },
              { name: "StoreNum", type: "string", mapping: "StoreNum" },
              { name: "LoginName", type: "string", mapping: "LoginName" },
              { name: "DeptName", type: "string", mapping: "DeptName" },
              { name: "Responsible", type: "string", mapping: "Responsible" },
              { name: "CreateTime", type: "datetime", mapping: "CreateTime" },
              { name: "LostTime", type: "datetime", mapping: "LostTime" },
               { name: "PlacesCode", type: "string", mapping: "PlacesCode" },
                { name: "PlacesName", type: "string", mapping: "PlacesName" },
                   { name: "Placesed", type: "string", mapping: "Placesed" },
                     { name: "Address", type: "string", mapping: "Address" },
              { name: "MaintenanceStatus", type: "Int", mapping: "MaintenanceStatus" }
            ]);
    //数据源
    var store = GridStore(Member, '/SysAppointed/SuperManIndexData', className);

    //快捷查询,如果不需要,可以不用定义
    searchData = [['全查询', ''], ['设备名称', 'Name'], ['设备规格', 'Specifications'], ['设备型号', 'Model'], ['设备位置', 'PlacesName'], ['责任人', 'Responsible'], ['责任部门', 'DeptName'], ['电话', 'LoginName'], ['所属分类', 'GroupName']];

    var sm = new Ext.grid.CheckboxSelectionModel(); 

    var grid = new Ext.grid.GridPanel({
        region: 'center',
        layout: 'fit',
        id: 'gg',
        store: store,
        stripeRows: true, //隔行颜色不同
        border: false,
        loadMask: { msg: '数据请求中，请稍后...' },
        selModel:sm, //这个 selModel 可简写为sm 
        columns: [
            new Ext.grid.RowNumberer({ header: '', width: 25 }), //设置行号
             sm,
             {
             header: "",
             dataIndex: 'Img',
             width: 20,
             align: 'center',
             menuDisabled: true,
             renderer: function (data, metadata, record, rowIndex, columnIndex, store) {
                 var content = '<div><span style="vertical-align: middle;"><img  onclick="showImg(this)" style="vertical-align: middle;width:15px; height:15px;border:0"  src="../..' + data + '"/></span></div>';
                 return content;
             }
         },
           {
               header: "设备名称",
               dataIndex: 'Name',
               width: 120,
               sortable: true,
               menuDisabled: true,
               renderer: function (data, metadata, record, rowIndex, columnIndex, store) {
                   if (data) {
                       metadata.attr = 'ext:qtip="点击查看详细"';
                       return "<span style='color:blue;cursor:pointer;' onclick='clickDetail()'>" + data + "</span>";
                   }
               }
           }, {
              header: "二维码编码",
              width: 140,
              dataIndex: 'QRCode',
              sortable: true,
              menuDisabled: true
          },
            {
                header: "设备规格",
                width: 70,
                dataIndex: 'Specifications',
                sortable: true,
                menuDisabled: true
            }, {
                header: "设备型号",
                width:70,
                dataIndex: 'Model',
                sortable: true,
                menuDisabled: true
            },
        {
            header: "设备位置",
            width:120,
            dataIndex: 'Address',
            sortable: true,
            menuDisabled: true,
            renderer: formartPlacesName
        },
//        {
//            header: "数量",
//            dataIndex: 'StoreNum',
//            width:50,
//            sortable: true,
//            menuDisabled: true
//        },
        {
            header: '生产日期',
            sortable: true,
            dataIndex: 'ProductionDate',
            menuDisabled: true,
            width:100,
            renderer: renderDate
        },
        {
            header: '过期日期',
            sortable: true,
            dataIndex: 'LostTime',
            menuDisabled: true,
            width:100,
            renderer: renderDate
        },
        {
            header: '责任人',
            sortable: true,
            dataIndex: 'Responsible',
            menuDisabled: true,
            width:70
        },
        {
            header: '责任部门',
            sortable: true,
            dataIndex: 'DeptName',
            menuDisabled: true,
            width:80,
            renderer: renderDeptName
        },
            {
                header: '电话',
                sortable: true,
                dataIndex: 'LoginName',
                menuDisabled: true,
                width: 120
            },
            {
                header: "所属分类",
                width:90,
                dataIndex: 'GroupName',
                sortable: true,
                menuDisabled: true
            }, 
            {
                header: '设备状态',
                sortable: true,
                dataIndex: 'MaintenanceStatus',
                menuDisabled: true,
                width: 60,
                renderer: renderMaintenanceStatus
            }
        ],
        tbar: tbar(className),
        bbar: bbar(store), //分页工具条
        viewConfig: {
            //forceFit: true,
            //enableRowBody: true,
            scrollOffset: 0, //不加这个的话，会在grid的最右边有个空白，留作滚动条的位置
            getRowClass: function (record, index, p, ds) {
                var LostTime = record.data.LostTime;
                var MaintenanceStatus = record.data.MaintenanceStatus;
                var tipTime = getthedate(LostTime, -4);
                var cls = '';
                if (MaintenanceStatus != -1) {
                    if (new Date(tipTime) < new Date()) {
                        cls = 'x-grid-record-red'
                    }
                }
                else {
                    cls = 'LostTime';
                }
                return cls;
            }
        }
    });
    grid.addListener('rowdblclick', dbGridClick);
    //默认选中第一行
    grid.store.on("load", function () {
        // grid.getSelectionModel().selectFirstRow(); //用这个就可以了，前提是你要有SelectionModel的实现
    });

    //为右键菜单添加事件监听器
   grid.addListener('rowcontextmenu', rightClickFn);

    // 响应加载前事件，传递node参数 
    grid.store.on('beforeload', function (node) {
        grid.getStore().proxy.conn.url = '/SysAppointed/SuperManIndexData?Id=' + treeNodeId;
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












