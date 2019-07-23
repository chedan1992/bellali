//单位公司
Ext.onReady(function () {
    //快捷查询,如果不需要,可以不用定义
    var className = ''; //页面类名
    if (this.frameElement) {
        className = this.frameElement.name
    }
    moreSearch = true; //更多查询按钮
    TotalStr = '勾选统计：<span style="color:blue; font-weight:400;">' +
               '退货金额：<span id="chart1">0</span>&nbsp;&nbsp;，' +
               '盈亏金额：<span id="chart2">0</span>&nbsp;&nbsp;';
    //转义列
    var Member = Ext.data.Record.create([
              { name: "Id", type: "string", mapping: "Id" },
              { name: "CreateTime", type: "datetime", mapping: "CreateTime" },
              { name: "Status", type: "Int32", mapping: "Status" },
              { name: "OutNumber", type: "string", mapping: "OutNumber" },
              { name: "StoreId", type: "string", mapping: "StoreId" },
              { name: "CusterId", type: "string", mapping: "CusterId" },
              { name: "GetNumber", type: "string", mapping: "GetNumber" },
              { name: "BillDate", type: "datetime", mapping: "BillDate" },
              { name: "OutStatus", type: "Int32", mapping: "OutStatus" },
              { name: "FinancialState", type: "Int32", mapping: "FinancialState" },
              { name: "Remark", type: "string", mapping: "Remark" },
                { name: "CheckoutType", type: "Int32", mapping: "CheckoutType" },
              { name: "PaymentType", type: "Int32", mapping: "PaymentType" },
              { name: "Maker", type: "string", mapping: "Maker" },
              { name: "Judger", type: "string", mapping: "Judger" },
              { name: "JudgeDate", type: "datetime", mapping: "JudgeDate" },
              { name: "TotalPrice", type: "decimal", mapping: "TotalPrice" },
              { name: "LossPrice", type: "decimal", mapping: "LossPrice" },
               { name: "oldPrice", type: "decimal", mapping: "oldPrice" },
              { name: "StoreName", type: "string", mapping: "StoreName" },
              { name: "CusterName", type: "string", mapping: "CusterName" },
              { name: "MakerName", type: "string", mapping: "MakerName" },
              { name: "JudgerName", type: "string", mapping: "JudgerName" },
              { name: "HasFileAttach", type: "bool", mapping: "HasFileAttach" }
            ]);
    //数据源   FinancialState asc,Status asc ,CreateTime desc
    var store = GridStore(Member, '/CACAI/HPurchase/SearchAuditData', className, 'FinancialState', 'asc');

    //快捷查询,如果不需要,可以不用定义
    searchData = [['全查询', ''], ['供应商', 'CusterName'], ['退货单号', 'OutNumber'], ['采购单号', 'GetNumber'], ['备注', 'Remark']];
    var sm = new Ext.grid.CheckboxSelectionModel({
        onHdMouseDown: function (e, t) {
            //全选或全不选后的动作
            Ext.grid.CheckboxSelectionModel.prototype.onHdMouseDown.call(this, e, t);
            MainCellclick();
        }
    });
    var gridId = 'gg';
    var grid = new Ext.grid.GridPanel({
        region: 'center',
        id: gridId,
        store: store,
        stripeRows: true, //隔行颜色不同
        border: false,
        sm: sm,
        enableDragDrop: false, //禁用才能选择复选框
        loadMask: { msg: '数据请求中，请稍后...' },
        columns: [
            new Ext.grid.RowNumberer({ header: '', width: 26 }), //设置行号
            sm,
            { header: "凭证", dataIndex: 'HasFileAttach', menuDisabled: true, sortable: true, width: 40, align: 'center',
                renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                    if (value == true) {
                        return '<a title="凭证详情" href="#" onclick="ShowAttach()"><img alt="凭证详情" style="vertical-align: middle; width:15px; height:15px;border:false" src="../../Resource/css/icons/toolbar/GTP_accessory.png"/>&nbsp;</a>';
                    }
                }
            },
           {
               header: "退货单号",
               dataIndex: 'OutNumber',
               width: 80,
               sortable: true,
               menuDisabled: false
           },
           {
               header: "仓库",
               width: 50,
               dataIndex: 'StoreName',
               sortable: true,
               menuDisabled: false
           },
           {
               header: "供应商",
               width: 140,
               dataIndex: 'CusterName',
               sortable: true,
               menuDisabled: false
           },
             {
                 header: "采购单号",
                 width: 80,
                 dataIndex: 'GetNumber',
                 sortable: true,
                 menuDisabled: false
             },
              {
                  header: "单据日期",
                  width: 80,
                  dataIndex: 'BillDate',
                  sortable: true,
                  menuDisabled: false
              },
        {
            header: "退货状态",
            width: 50,
            dataIndex: 'OutStatus',
            align: 'center',
            sortable: true,
            menuDisabled: false,
            renderer: renderOutStatus
        },
          {
              header: "财务状态",
              dataIndex: 'FinancialState',
              align: 'center',
              width: 50,
              sortable: true,
              menuDisabled: false,
              renderer: renderFinancialState
          },
        {
            header: '退货金额',
            sortable: true,
            dataIndex: 'TotalPrice',
            menuDisabled: false,
            width: 80,
            renderer: function (data, metadata, record, rowIndex, columnIndex, store) {
                metadata.attr = 'ext:qtip="点击查看凭证"';
                var oldPrice = parseFloat(record.data.oldPrice);
                if (parseFloat(data) - oldPrice == 0) {
                    return "<span style='cursor:pointer;' onclick='ShowAttach()'>" + data + "</span>";
                }
                else {
                    return "<span style='color:red;cursor:pointer;' onclick='ShowAttach()'>" + data + "</span>";
                }
            }
        },
          {
              header: '盈亏金额',
              sortable: true,
              dataIndex: 'LossPrice',
              menuDisabled: false,
              width: 80,
              renderer: renderMoney
          },
        {
            header: '备注',
            sortable: true,
            dataIndex: 'Remark',
            menuDisabled: false,
            width: 70,
            renderer: function (data, metadata, record, rowIndex, columnIndex, store) {
                if (data) {
                    metadata.attr = 'ext:qtip="' + data + '"';
                    return data;
                }
            }
        },
         {
             header: "结账方式",
             dataIndex: 'CheckoutType',
             width: 70,
             sortable: true,
             menuDisabled: true,
             renderer: formartCheckoutType
         },
        {
            header: "付款方式",
            dataIndex: 'PaymentType',
            width: 70,
            sortable: true,
            menuDisabled: true,
            renderer: formartPaymentType
        },
        {
            header: '添加时间',
            sortable: false,
            dataIndex: 'CreateTime',
            width: 140,
            menuDisabled: false
        }
        ],
        tbar: tbar(className),
        bbar: bbar(store, gridId, className), //分页工具条
        viewConfig: {
            forceFit: true,
            enableRowBody: true,
            scrollOffset: 0, //不加这个的话，会在grid的最右边有个空白，留作滚动条的位置
            getRowClass: function (record, index, p, ds) {

            }
        },
        listeners: {
            'beforeedit': function (o) {
             
            }
        }

    });

    //默认选中第一行
    grid.store.on("load", function () {
        Ext.getCmp('gg').getSelectionModel().selectFirstRow(); //用这个就可以了，前提是你要有SelectionModel的实现
        //判断是否有数据
        gridrowclick();
        MainCellclick();
    });
    //单元格选中
    grid.on('cellclick', MainCellclick);
    //添加单机事件
    grid.addListener('rowclick', gridrowclick);
    //添加双击事件
     grid.addListener('rowdblclick', dbGridClick);
    //为右键菜单添加事件监听器
    //grid.addListener('rowcontextmenu', rightClickFn);
    //确定编辑后
    //grid.on('afteredit', afterEditMain, this);


    //下面明细
    var Human = Ext.data.Record.create([
                  { name: "Id", type: "string", mapping: "Id" },
                  { name: "Code", type: "string", mapping: "Code" },
                  { name: "GoodId", type: "string", mapping: "GoodId" },
                  { name: "GoodName", type: "string", mapping: "GoodName" },
                  { name: "GoodUnit", type: "string", mapping: "GoodUnit" },
                  { name: "Count", type: "string", mapping: "Count" },
                  { name: "Price", type: "string", mapping: "Price" },
                  { name: "Money", type: "string", mapping: "Money" },
                  { name: "Batch", type: "string", mapping: "Batch" },
                  { name: "Remark", type: "string", mapping: "Remark" },
                  { name: "StyleNum", type: "Int", mapping: "StyleNum" },
                  { name: "FreightNum", type: "Int", mapping: "FreightNum" },
                  { name: "ListOrder", type: "Int", mapping: "ListOrder" },
                  { name: "StylePrice", type: "decimal", mapping: "StylePrice" },
                  { name: "BillPrice", type: "decimal", mapping: "BillPrice" },
                  { name: "BillMoney", type: "decimal", mapping: "BillMoney" },
                  { name: "CheckPrice", type: "decimal", mapping: "CheckPrice" },
                  { name: "LosstPrice", type: "decimal", mapping: "LosstPrice" },
                  { name: "StyleCount", type: "Int", mapping: "StyleCount" },
                  { name: "BillNum", type: "Int", mapping: "BillNum" },
                  { name: "CheckNum", type: "Int", mapping: "CheckNum" }
     ]);
    var rightstore = GridStore(Human, '/CACAI/HPurchase/SearchDataDetail?MainId=' + treeNodeId, '', 'StyleNum', 'asc', false, true);


    var smm = new Ext.grid.CheckboxSelectionModel();
    var rightGrid = new Ext.grid.GridPanel({
        region: 'south',
        id: 'rightGrid',
        store: rightstore,
        stripeRows: true, //隔行颜色不同
        collapseMode: 'mini', //出现小箭头
        animCollapse: true,
        split: true,
        stripeRows: true, //隔行颜色不同
        border: false,
        height: 330,
        sm: smm,
        enableDragDrop: false, //禁用才能选择复选框
        loadMask: { msg: '数据请求中，请稍后...' },
        columns: [
                new Ext.grid.RowNumberer({ header: '', width: 26 }), //设置行号
                    {
                    header: "商品编码",
                    dataIndex: 'Code',
                    menuDisabled: true,
                    sortable: true,
                    width: 80
                },
                {
                    header: "款式编号",
                    dataIndex: 'StyleNum',
                    menuDisabled: true,
                    sortable: true,
                    width: 80
                },
            {
                header: "颜色及规格",
                dataIndex: 'GoodUnit',
                menuDisabled: true,
                sortable: true,
                width: 90
            },
            {
                header: "商品名称",
                dataIndex: 'GoodName',
                menuDisabled: true,
                sortable: true,
                width: 90
            },
            {
                header: "供应商货号",
                dataIndex: 'FreightNum',
                menuDisabled: true,
                sortable: true,
                width: 80
            },
            {
                header: "数量",
                dataIndex: 'Count',
                menuDisabled: true,
                sortable: true,
                width: 60
            },
                {
                    header: "单价",
                    dataIndex: 'Price',
                    menuDisabled: true,
                    sortable: true,
                    width: 60
                },
                {
                    header: "金额",
                    dataIndex: 'Money',
                    menuDisabled: true,
                    sortable: true,
                    width: 60
                },
        //            {
        //                header: "款式数量",
        //                dataIndex: 'StyleCount',
        //                width: 80,
        //                menuDisabled: true
        //                //                renderer: function (value, metadata, record, rowIdx, colIdx, store) {
        //                //                    var ProBrief = record.data.ProBrief;
        //                //                    return value + '/' + ProBrief.split(',')[1];
        //                //                }
        //            },
        //             {
        //                 header: "款式金额",
        //                 dataIndex: 'StylePrice',
        //                 menuDisabled: true,
        //                 width: 80
        //             },
            {
            header: "单据数量",
            dataIndex: 'BillNum',
            width: 70,
            align: 'center',
            sortable: true,
            menuDisabled: true,
            renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                var Count = parseFloat(record.data.Count);
                if (parseFloat(value) - Count == 0) {
                    return value;
                }
                else {
                    return "<span style='color:red'>" + value + "</span>"; //修改过
                }
            }
        },
            {
                header: "单据金额",
                dataIndex: 'BillPrice',
                width: 70,
                align: 'center',
                enableHdMenu: true,
                sortable: true,
                menuDisabled: true,
                renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                    var Price = parseFloat(record.data.Price);
                    if (parseFloat(value) - Price == 0) {
                        return value;
                    }
                    else {
                        return "<span style='color:red'>" + value + "</span>"; //修改过
                    }
                }
            },
                {
                    header: "单据款金额",
                    dataIndex: 'BillMoney',
                    width: 70,
                    align: 'center',
                    enableHdMenu: true,
                    menuDisabled: true,
                    sortable: true
                },
                {
                    header: "核查补充数量",
                    dataIndex: 'CheckNum',
                    width: 70,
                    align: 'center',
                    sortable: true,
                    menuDisabled: true,
                    renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                        if (parseFloat(value) != 0) {
                            return "<span style='color:red'>" + value + "</span>"; //修改过
                        }
                        else {
                            return value;
                        }
                    }
                },
                {
                    header: "核查补充金额",
                    dataIndex: 'CheckPrice',
                    width: 70,
                    align: 'center',
                    enableHdMenu: true,
                    sortable: true,
                    menuDisabled: true
                },
            {
                header: "盈亏金额",
                dataIndex: 'LosstPrice',
                menuDisabled: true,
                width: 90,
                align: 'center',
                renderer: renderMoney
            },
             {
                 header: '备注',
                 sortable: true,
                 dataIndex: 'Remark',
                 sortable: true,
                 menuDisabled: false,
                 width: 140,
                 renderer: function (data, metadata, record, rowIndex, columnIndex, store) {
                     if (data) {
                         metadata.attr = 'ext:qtip="' + data + '"';
                         return data;
                     }
                 }
             }
                ],
             tbar: tbarDIY(className),
             bbar: DIYbbar(rightstore), //分页工具条
        viewConfig: {
            forceFit: true,
            enableRowBody: true,
            scrollOffset: 0, //不加这个的话，会在grid的最右边有个空白，留作滚动条的位置
            getRowClass: function (record, index, p, ds) {

            }
        },
        listeners: {
            'beforeedit': function (o) {
            
            }
        }

    });
    //确定编辑后
    //rightGrid.on('afteredit', afterEdit, this);
    //单元格选中
    rightGrid.on('cellclick', cellclick);
    //默认选中第一行
    rightGrid.store.on("load", function () {
        //Ext.getCmp('rightGrid').getSelectionModel().selectFirstRow(); //用这个就可以了，前提是你要有SelectionModel的实现
        //判断是否有数据
        if (Ext.getCmp('rightGrid').getStore().totalLength > 0) {
            Ext.getCmp('rightGrid').getSelectionModel().selectAll();
        }
        cellclick();
    });
    rightGrid.store.on('beforeload', function (node) {
        //判断是显示当前供应商还是全部
        rightGrid.getStore().proxy.conn.url = '/CACAI/HPurchase/SearchDataDetail?MainId=' + treeNodeId;
    });
    //viewport
    var viewport = new Ext.Viewport({
        layout: 'border',
        items: [grid, rightGrid],
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












