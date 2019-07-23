//普通用户登录查看自己增删改的设备
Ext.onReady(function () {
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
              { name: "Places", type: "string", mapping: "Places" },
              { name: "StoreNum", type: "string", mapping: "StoreNum" },
              { name: "GroupName", type: "string", mapping: "GroupName" },
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
    var store = GridStore(Member, '/SysAppointed/SearchMyData', className);

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
        selModel: sm, //这个 selModel 可简写为sm 
        columns: [
            new Ext.grid.RowNumberer({ header: '', width: 25 }), //设置行号
            sm,
            {
            header: "",
            dataIndex: 'Img',
            width: 40,
            align: 'center',
            menuDisabled: true,
            renderer: function (data, metadata, record, rowIndex, columnIndex, store) {
                var content = '<div><span style="vertical-align: middle;"><img onclick="showImg(this)" style="vertical-align: middle;width:15px; height:15px;border:0"  src="../..' + data + '"/></span></div>';
                return content;
            }
        }, {
            header: "设备名称",
            dataIndex: 'Name',
            width: 130,
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
                width:80,
                dataIndex: 'Specifications',
                sortable: true,
                menuDisabled: true
            }, {
                header: "设备型号",
                width:80,
                dataIndex: 'Model',
                sortable: true,
                menuDisabled: true
            },
        {
            header: "设备位置",
            width: 140,
            dataIndex: 'Address',
            sortable: false,
            menuDisabled: true,
            renderer: formartPlacesName
        },
//        {
//            header: "数量",
//            dataIndex: 'StoreNum',
//            width: 50,
//            sortable: true,
//            menuDisabled: true
//        },
        {
            header: '生产日期',
            sortable: true,
            dataIndex: 'ProductionDate',
            menuDisabled: true,
            width: 100,
            renderer: renderDate
        },
          {
              header: '过期日期',
              sortable: true,
              dataIndex: 'LostTime',
              menuDisabled: true,
              width: 100,
              renderer: renderDate
          },
        {
            header: '责任人',
            sortable: true,
            dataIndex: 'Responsible',
            menuDisabled: true,
            width:80,
            renderer: renderDeptName
        },
        {
            header: '责任部门',
            sortable: true,
            dataIndex: 'DeptName',
            menuDisabled: true,
            width:80
        },
            {
                header: '电话',
                sortable: true,
                dataIndex: 'LoginName',
                menuDisabled: true,
                width:120
            },
             {
                 header: "所属分类",
                 width:100,
                 dataIndex: 'GroupName',
                 sortable: false,
                 menuDisabled: true
             },
       
         {
             header: '设备状态',
             sortable: true,
             dataIndex: 'MaintenanceStatus',
             menuDisabled: true,
             width: 60,
             renderer: renderMaintenanceStatus
         },
        {
            header: '添加时间',
            sortable: false,
            dataIndex: 'CreateTime',
            width:140,
            menuDisabled: true
        }

        ],
        tbar: tbar(className),
        bbar: bbar(store), //分页工具条
        viewConfig: {
            //forceFit: true,
            enableRowBody: true,
            scrollOffset: 0, //不加这个的话，会在grid的最右边有个空白，留作滚动条的位置
            getRowClass: function (record, index, p, ds) {
                var LostTime = record.data.LostTime;
                var MaintenanceStatus = record.data.MaintenanceStatus;
                var tipTime = getthedate(LostTime, -4);
                var cls = '';
                if (MaintenanceStatus != -1) {//已过期的设备不添加颜色区别
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
        //grid.getSelectionModel().selectFirstRow(); //用这个就可以了，前提是你要有SelectionModel的实现
    });

    //为右键菜单添加事件监听器
    grid.addListener('rowcontextmenu', rightClickFn);

    //viewport
    var viewport = new Ext.Viewport({
        layout: 'border',
        items: [grid],
        listeners: {
            afterrender: function () {

            }
        }
    });
});












