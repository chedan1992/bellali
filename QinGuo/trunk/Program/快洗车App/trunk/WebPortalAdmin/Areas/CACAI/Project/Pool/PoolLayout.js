//单位公司
Ext.onReady(function () {
    //快捷查询,如果不需要,可以不用定义
    var className = ''; //页面类名
    if (this.frameElement) {
        className = this.frameElement.name
    }
    moreSearch = true; //更多查询按钮
    TotalStr = '勾选统计：<span style="color:blue; font-weight:400;">' +
               '供应商金额：<span id="chart1">0</span>&nbsp;&nbsp;，' +
               '盈亏金额：<span id="chart2">0</span>&nbsp;&nbsp;';
    //转义列
    var Member = Ext.data.Record.create([
              { name: "Id", type: "string", mapping: "Id" },
              { name: "CreateTime", type: "datetime", mapping: "CreateTime" },
              { name: "Status", type: "Int32", mapping: "Status" },
              { name: "InNumber", type: "string", mapping: "InNumber" },
              { name: "StoreId", type: "string", mapping: "StoreId" },
              { name: "CusterId", type: "string", mapping: "CusterId" },
              { name: "GetNumber", type: "string", mapping: "GetNumber" },
              { name: "BillDate", type: "datetime", mapping: "BillDate" },
              { name: "InStatus", type: "Int32", mapping: "InStatus" },
               { name: "OnLineCount", type: "Int32", mapping: "OnLineCount" },
              { name: "FinancialState", type: "Int32", mapping: "FinancialState" },
              { name: "Remark", type: "string", mapping: "Remark" },
              { name: "LogisticsNumber", type: "string", mapping: "LogisticsNumber" },
              { name: "Maker", type: "string", mapping: "Maker" },
              { name: "Judger", type: "string", mapping: "Judger" },
              { name: "JudgeDate", type: "datetime", mapping: "JudgeDate" },
              { name: "TotalPrice", type: "decimal", mapping: "TotalPrice" },
              { name: "LossPrice", type: "decimal", mapping: "LossPrice" },
              { name: "oldPrice", type: "decimal", mapping: "oldPrice" },
              { name: "StoreName", type: "string", mapping: "StoreName" },
              { name: "CusterName", type: "string", mapping: "CusterName" },
                { name: "CheckoutType", type: "Int32", mapping: "CheckoutType" },
              { name: "PaymentType", type: "Int32", mapping: "PaymentType" },
              { name: "MakerName", type: "string", mapping: "MakerName" },
              { name: "FinancialDT", type: "datetime", mapping: "FinancialDT" },
              { name: "PoolStatus", type: "Int32", mapping: "PoolStatus" },
              { name: "JudgerName", type: "string", mapping: "JudgerName" },
              { name: "RelationId", type: "string", mapping: "RelationId" }
            ]);
    //数据源   FinancialState asc,Status asc ,CreateTime desc
    var store = GridStore(Member, '/CACAI/Finance/SearchHOrderInRelation', className, 'BillDate', 'desc');

    //快捷查询,如果不需要,可以不用定义
    searchData = [['全查询', ''], ['供应商', 'CusterName'], ['入库单号', 'InNumber'], ['采购单号', 'GetNumber'], ['备注', 'Remark']];
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
            new Ext.grid.RowNumberer({ header: '', width: 25 }), //设置行号
             sm,
             {
                 header: "操作",
                 dataIndex: 'IConName',
                 width: 45,
                 align: 'center',
                 menuDisabled: true,
                 renderer: function (value, meta, record, rowIdx, colIdx, store) {
                     var content = '<div><span style="vertical-align: middle;cursor:pointer" title="驳回" onclick="BtnBackMain();"><img style="vertical-align: middle; width:18px; height:18px;border:0" class="GTP_cancelagent"  src="../../Content/Extjs/resources/images/default/s.gif"/></span>' +
                                '&nbsp;<font style="color:silver">|</font>&nbsp;<span style="vertical-align: middle;cursor:pointer" title="查看明细" onclick="BtnMainDetail();"><img style="vertical-align: middle; width:18px; height:18px;border:0" class="GTP_view"  src="../../Content/Extjs/resources/images/default/s.gif"/></span>' +
                                '&nbsp;<font style="color:silver">|</font>&nbsp;<span style="vertical-align: middle;cursor:pointer" title="查看凭证" onclick="FileAttach();"><img style="vertical-align: middle; width:18px; height:18px;border:0" class="GTP_accessory"  src="../../Content/Extjs/resources/images/default/s.gif"/></span>' +
                                '</div>';
                     return content;
                 }
             },
           {
               header: "入库单号",
               dataIndex: 'InNumber',
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
        //        {
        //            header: "状态",
        //            width: 50,
        //            dataIndex: 'InStatus',
        //            align: 'center',
        //            sortable: true,
        //            menuDisabled: false,
        //            renderer: renderInStatus
        //        },
        //        {
        //        header: "财务状态",
        //        dataIndex: 'FinancialState',
        //        align: 'center',
        //        width: 50,
        //        sortable: true,
        //        menuDisabled: false,
        //        renderer: renderFinancialState
        //    },
        {
        header: '供应商金额',
        sortable: true,
        dataIndex: 'TotalPrice',
        menuDisabled: false,
        width: 80,
        renderer: function (data, metadata, record, rowIndex, columnIndex, store) {
            var oldPrice = parseFloat(record.data.oldPrice);
            if (parseFloat(data) - oldPrice == 0) {
                return data;
            }
            else {
                return "<span style='color:red;'>" + data + "</span>"; //修改过
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
              header: '完成状态',
              sortable: true,
              dataIndex: 'PoolStatus',
              menuDisabled: false,
              width: 40,
              renderer: function (value, meta, record, rowIdx, colIdx, store) {
                  if (value == -1) {
                      return "<span style='color:red'>已驳回</span>";
                  }
                  else if (value == 1) {
                      return "未完成";
                  }
                  else if (value == 2) {
                      return "<span style='color:green'>已完成</span>";
                  }
              }
          },
           {
               header: '是否在途',
               sortable: true,
               hidden:true,
               dataIndex: 'OnLineCount',
               menuDisabled: false,
               width: 40,
               renderer: function (value, metadata, record, rowIdx, colIdx, store) {
                   if (value) {
                       if (value > 0) {
                           metadata.attr = 'ext:qtip="在途单据数量：' + value + '"';
                           return "是";
                       }
                       else {
                           return "否";
                       }
                   }
               }
           },


           {
               header: "结账方式",
               dataIndex: 'CheckoutType',
               width: 50,
               sortable: true,
               menuDisabled: true,
               renderer: formartCheckoutType
           },
        {
            header: "付款方式",
            dataIndex: 'PaymentType',
            width: 50,
            sortable: true,
            menuDisabled: true,
            renderer: formartPaymentType
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
            header: '提交时间',
            sortable: true,
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

    }

});

// 响应加载前事件，传递node参数 
grid.store.on('beforeload', function (node) {
    //获取统计金额
    var search = GetSearch();
    grid.getStore().proxy.conn.url = '/CACAI/Finance/SearchHOrderInRelation?' + search;
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


//转义列
var Member2 = Ext.data.Record.create([
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
              { name: "OnLineCount", type: "Int32", mapping: "OnLineCount" },
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
              { name: "IsDecompose", type: "bit", mapping: "IsDecompose" },
              { name: "DecomposePrice", type: "decimal", mapping: "DecomposePrice" },
              { name: "OutNumberS", type: "string", mapping: "OutNumberS" },
              { name: "PoolPrice", type: "decimal", mapping: "PoolPrice" },
              { name: "FinancialDT", type: "datetime", mapping: "FinancialDT" },
              { name: "PoolStatus", type: "Int32", mapping: "PoolStatus" },
              { name: "RelationId", type: "string", mapping: "RelationId" },
              { name: "OnLine", type: "Int32", mapping: "OnLine" },

            ]);
//数据源   FinancialState asc,Status asc ,CreateTime desc
var rightstore = GridStore(Member2, '/CACAI/Finance/SearchHPurchaseRelation', '', 'CreateTime', 'desc', false, true);

//快捷查询,如果不需要,可以不用定义
var sm2 = new Ext.grid.CheckboxSelectionModel();

//sm2.handleMouseDown = Ext.emptyFn; //不响应MouseDown事件
sm2.on('selectionchange', function (sm_, rowIndex, record) {
    cellclick();
})//行选中的时候
var RightGrid = new Ext.grid.GridPanel({
    region: 'south',
    id: 'rightGrid',
    store: rightstore,
    stripeRows: true, //隔行颜色不同
    collapseMode: 'mini', //出现小箭头
    split: true,
    border: false,
    loadMask: { msg: '数据请求中，请稍后...' },
    height: 330,
    sm: sm2,
    enableDragDrop: false, //禁用才能选择复选框
    columns: [
            new Ext.grid.RowNumberer({ header: '', width: 25 }), //设置行号
            sm2,
            {
                header: "操作",
                dataIndex: 'IConName',
                width: 45,
                align: 'center',
                menuDisabled: true,
                renderer: function (value, meta, record, rowIdx, colIdx, store) {
                    var content = '<div><span style="vertical-align: middle;cursor:pointer" title="驳回" onclick="BtnBackDetail();"><img style="vertical-align: middle; width:18px; height:18px;border:0" class="GTP_cancelagent"  src="../../Content/Extjs/resources/images/default/s.gif"/></span>' +
                                '&nbsp;<font style="color:silver">|</font>&nbsp;<span style="vertical-align: middle;cursor:pointer" title="查看明细" onclick="BtnDetails();"><img style="vertical-align: middle; width:18px; height:18px;border:0" class="GTP_view"  src="../../Content/Extjs/resources/images/default/s.gif"/></span>' +
                                '&nbsp;<font style="color:silver">|</font>&nbsp;<span style="vertical-align: middle;cursor:pointer" title="查看凭证" onclick="FileAttach();"><img style="vertical-align: middle; width:18px; height:18px;border:0" class="GTP_accessory"  src="../../Content/Extjs/resources/images/default/s.gif"/></span>' +
                                '</div>';
                    return content;
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
               width: 100,
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


    //          {
    //              header: "财务状态",
    //              dataIndex: 'FinancialState',
    //              align: 'center',
    //              width: 50,
    //              sortable: true,
    //              menuDisabled: false,
    //              renderer: renderFinancialState
    //          },


              {
              header: '原退货金额',
              sortable: true,
              dataIndex: 'TotalPrice',
              menuDisabled: false,
              width: 50
          },
        {
            header: '是否拆分',
            sortable: true,
            dataIndex: 'IsDecompose',
            menuDisabled: false,
            width: 40,
            renderer: function (value, meta, record, rowIdx, colIdx, store) {
                if (value == 1) {
                    return "<span style='color:red'>已拆分</span>";
                }
                else {
                    return "<span style='color:green'>未拆</span>";
                }
            }
        },
         {
             header: '扣减状态',
             sortable: true,
             dataIndex: 'PoolStatus',
             menuDisabled: false,
             width: 40,
             renderer: function (value, meta, record, rowIdx, colIdx, store) {
                 if (value == -1) {
                     return "<span style='color:red'>已驳回</span>";
                 } else if (value == 1) {
                     return "未完成";
                 }
                 else if (value == 2) {
                     return "<span style='color:orange'>部分完成</span>";
                 }
                 else if (value == 3) {
                     return "<span style='color:green'>已完成</span>";
                 }
             }
         },
          {
              header: '拆分金额',
              sortable: true,
              dataIndex: 'DecomposePrice',
              menuDisabled: false,
              width: 40,
              renderer: renderMoney
          },
         {
             header: '剩余退货金额',
             sortable: true,
             dataIndex: 'PoolPrice',
             menuDisabled: false,
             width: 50,
             renderer: function (data, metadata, record, rowIndex, columnIndex, store) {
                 var oldPrice = parseFloat(record.data.oldPrice);
                 if (parseFloat(data) - oldPrice == 0) {
                     return data;
                 }
                 else {
                     return "<span style='color:red;'>" + data + "</span>"; //修改过
                 }
             }
         },
          {
              header: '盈亏金额',
              sortable: true,
              dataIndex: 'LossPrice',
              menuDisabled: false,
              width: 50,
              renderer: renderMoney
          },
          {
              header: '是否在途',
              sortable: true,
              dataIndex: 'OnLineCount',
              menuDisabled: false,
              width: 40,
              renderer: function (value, metadata, record, rowIdx, colIdx, store) {
                  var OnLine=record.data.OnLine;
                  if (OnLine==-1) {
                      OnLine=value;
                      if (value > 0) {
                          metadata.attr = 'ext:qtip="在途单据数量：' + value + '"';
                          return "是";
                      }
                      else {
                          return "否";
                      }
                  }
                  else{
                   if (OnLine > 0) {
                          metadata.attr = 'ext:qtip="在途单据数量：' + OnLine + '"';
                          return "是";
                      }
                      else {
                          return "否";
                      }
                  }
              }
          },
        {
            header: '备注',
            sortable: true,
            dataIndex: 'Remark',
            menuDisabled: false,
            width: 130,
            renderer: function (data, metadata, record, rowIndex, columnIndex, store) {
                if (data) {
                    metadata.attr = 'ext:qtip="' + data + '"';
                    return data;
                }
            }
        },
        {
            header: '提交时间',
            sortable: true,
            dataIndex: 'CreateTime',
            width: 140,
            menuDisabled: false
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
    }

});

//添加双击事件
RightGrid.addListener('rowdblclick', FileAttach2);
//单元格选中
RightGrid.addListener('cellclick', cellclick);

// 响应加载前事件，传递node参数 
RightGrid.store.on('beforeload', function (node) {
    //判断是显示当前供应商还是全部
    var search = GetSearch2();
    if (Ext.getCmp("NowAll").pressed == true) {
        RightGrid.getStore().proxy.conn.url = '/CACAI/Finance/SearchHPurchaseRelation?'+search+'&CusterId=';
    }
    else {
        RightGrid.getStore().proxy.conn.url = '/CACAI/Finance/SearchHPurchaseRelation?' +search+"&CusterId=" +treeNodeId;
    }
});
//默认选中第一行
RightGrid.store.on("load", function () {
    // RightGrid.getView().getRow(0).style.backgroundColor = "red"
    //判断是显示当前供应商还是全部
    Ext.getCmp('rightGrid').getSelectionModel().selectFirstRow(); //用这个就可以了，前提是你要有SelectionModel的实现
    if (Ext.getCmp("NowAll").pressed == true) {

    }
    else {
        if (Ext.getCmp('rightGrid').getStore().totalLength > 0) {
            Ext.getCmp('rightGrid').getSelectionModel().selectAll();
        }
    }
    cellclick();
});

//viewport
var viewport = new Ext.Viewport({
    layout: 'border',
    items: [grid, RightGrid],
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















