//列表公司
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
              { name: "Address", type: "string", mapping: "Address" },
              { name: "CityId", type: "string", mapping: "CityId" },
              { name: "Code", type: "string", mapping: "Code" },
              { name: "Type", type: "string", mapping: "Type" },
              { name: "TypeName", type: "string", mapping: "TypeName" },
              { name: "Order", type: "string", mapping: "Order" },
              { name: "Status", type: "string", mapping: "Status" },
              { name: "Province", type: "string", mapping: "Province" },
              { name: "CityId", type: "string", mapping: "CityId" },
              { name: "AreaId", type: "string", mapping: "AreaId" },
              { name: "Introduction", type: "string", mapping: "Introduction" },
              { name: "Phone", type: "string", mapping: "Phone" },
              { name: "Tel", type: "string", mapping: "Tel" },
              { name: "CreateTime", type: "datetime", mapping: "CreateTime" },
              { name: "UserName", type: "string", mapping: "UserName" },
              { name: "LoginName", type: "string", mapping: "LoginName" },
              { name: "CheckoutType", type: "Int32", mapping: "CheckoutType" },
              { name: "PaymentType", type: "Int32", mapping: "PaymentType" },
              { name: "AccountName", type: "string", mapping: "AccountName" },
              { name: "AccountNum", type: "string", mapping: "AccountNum" },
              { name: "Phone", type: "string", mapping: "Phone" }
            ]);
    //数据源
    var store = GridStore(Member, '/Customer/SearchData', className, "", "");
    var sm = new Ext.grid.CheckboxSelectionModel();
    //快捷查询,如果不需要,可以不用定义
    searchData = [['全查询', ''], ['供应商名称', 'Name'], ['供应商编码', 'Code'], ['联系地址', 'Address'], ['电话', 'Phone'], ['手机', 'Tel']];
    var gridId = 'gg';
    var grid = new Ext.grid.GridPanel({
        region: 'center',
        layout: 'fit',
        id: gridId,
        store: store,
        stripeRows: true, //隔行颜色不同
        sm: sm,
        border: false,
        loadMask: { msg: '数据请求中，请稍后...' },
        columns: [
            new Ext.grid.RowNumberer({ header: '', width: 25 }), //设置行号
            sm, {
            header: "供应商名称",
            dataIndex: 'Name',
            width:120,
            sortable: false,
            menuDisabled: true
        }, {
            header: "供应商编码",
            dataIndex: 'Code',
            width:80,
            sortable: false,
            menuDisabled: true
        }, {
            header: "联系地址",
            dataIndex: 'Address',
            width:120,
            sortable: false,
            menuDisabled: true
        },
        {
            header: "供应商分类",
            dataIndex: 'TypeName',
            width: 100,
            sortable: false,
            menuDisabled: true
        },
            {
                header: '电话',
                sortable: false,
                dataIndex: 'Phone',
                menuDisabled: true,
                 width:90,
                 renderer: IsNull
             },
               {
                   header: "手机",
                   dataIndex: 'Tel',
                   width: 60,
                   sortable: false,
                   menuDisabled: true
               },
                 {
                     header: "结账方式",
                     dataIndex: 'CheckoutType',
                     width:70,
                     sortable: false,
                     menuDisabled: true ,
                     renderer:formartCheckoutType
                 },
                 {
                     header: "付款方式",
                     dataIndex: 'PaymentType',
                     width:70,
                     sortable: false,
                     menuDisabled: true,
                     renderer: formartPaymentType
                 },
                {
                   header: "名字",
                   dataIndex: 'AccountName',
                   width:50,
                   sortable: false,
                   menuDisabled: true
               },
              {
                  header: "账号",
                  dataIndex: 'AccountNum',
                  width:80,
                  sortable: false,
                  menuDisabled: true
              },   
            {
                header: '状态',
                sortable: false,
                dataIndex: 'Status',
                menuDisabled: true,
                width:40,
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
        bbar: bbar(store, gridId, className), //分页工具条
        viewConfig: {
            forceFit: true,
            //enableTextSelection: true, //4.0后 单元格复制
            enableRowBody: true,
            scrollOffset: 0 //不加这个的话，会在grid的最右边有个空白，留作滚动条的位置
        }
    });

    grid.addListener('rowdblclick', dbGridClick);
    //默认选中第一行
    grid.store.on("load", function () {
        grid.getSelectionModel().selectFirstRow(); //用这个就可以了，前提是你要有SelectionModel的实现
    }); 
    //viewport
    var viewport = new Ext.Viewport({
        layout: 'border',
        items: [grid],
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












