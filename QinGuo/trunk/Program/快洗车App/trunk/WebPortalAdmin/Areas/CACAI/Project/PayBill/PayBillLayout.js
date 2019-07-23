//单位公司
Ext.onReady(function () {
    //快捷查询,如果不需要,可以不用定义
    var className = ''; //页面类名
    if (this.frameElement) {
        className = this.frameElement.name
    }
    var IsEdit = false;
    TotalStr = '勾选统计：<span style="color:blue; font-weight:400;">供应商金额：<span id="chart1">0</span>&nbsp;&nbsp;,退款金额：<span id="chart2">0</span>&nbsp;&nbsp;,付款金额：<span id="chart33">0</span>&nbsp;&nbsp;,盈亏金额：<span id="chart4">0</span>&nbsp;&nbsp;';
    //转义列
    var Member = Ext.data.Record.create([
              { name: "Id", type: "string", mapping: "Id" },
              { name: "CreateTime", type: "datetime", mapping: "CreateTime" },
              { name: "Status", type: "Int32", mapping: "Status" },
              { name: "InNumber", type: "string", mapping: "InNumber" },
              { name: "OrderInId", type: "string", mapping: "OrderInId" },
              { name: "CusterId", type: "string", mapping: "CusterId" },
               { name: "BillDate", type: "datetime", mapping: "BillDate" },
              { name: "SubTime", type: "datetime", mapping: "SubTime" },
              { name: "InStatus", type: "Int32", mapping: "InStatus" },
              { name: "CheckoutType", type: "Int32", mapping: "CheckoutType" },
              { name: "PaymentType", type: "Int32", mapping: "PaymentType" },
              { name: "FinanceRemark", type: "string", mapping: "FinanceRemark" },
              { name: "Remark", type: "string", mapping: "Remark" },
              { name: "PayTime", type: "datetime", mapping: "PayTime" },
              { name: "ReturnAmount", type: "decimal", mapping: "ReturnAmount" },
              { name: "ProfitTotal", type: "decimal", mapping: "ProfitTotal" },
              { name: "PaymentAmmount", type: "decimal", mapping: "PaymentAmmount" },
              { name: "PayStatus", type: "Int32", mapping: "PayStatus" },
              { name: "RelationId", type: "string", mapping: "RelationId" },
              { name: "CusterName", type: "string", mapping: "CusterName" },
              { name: "AccountName", type: "string", mapping: "AccountName" },
              { name: "HasFileAttach", type: "bool", mapping: "HasFileAttach" },
              { name: "AccountNum", type: "string", mapping: "AccountNum" },
              { name: "LossPrice", type: "decimal", mapping: "LossPrice" }
              
            ]);
    //数据源   FinancialState asc,Status asc ,CreateTime desc
    var store = GridStore(Member, '/CACAI/Finance/SearchData', className, 'CreateTime', 'desc');

    //快捷查询,如果不需要,可以不用定义
    searchData = [['全查询', ''], ['供应商', 'CusterName'], ['入库单号', 'InNumber'], ['入库备注', 'Remark'], ['财务备注', 'FinanceRemark']];
    var sm = new Ext.grid.CheckboxSelectionModel({
        onHdMouseDown: function (e, t) {
            //全选或全不选后的动作
            Ext.grid.CheckboxSelectionModel.prototype.onHdMouseDown.call(this, e, t);
            MainCellclick();
        }
    });
    var gridId = 'gg';
    var grid = new Ext.grid.EditorGridPanel({
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
             { header: "凭证", dataIndex: 'HasFileAttach', menuDisabled: true, width: 40, align: 'center',
                 renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                     if (value == true) {
                         return '<a title="凭证详情" href="#" onclick="ShowAttach();"><img alt="凭证详情" style="vertical-align: middle; width:15px; height:15px;border:false" src="../../Resource/css/icons/toolbar/GTP_accessory.png"/>&nbsp;</a>';
                     }
                 }
             },
           {
               header: "入库单号",
               dataIndex: 'InNumber',
               width: 60,
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
                header: "单据日期",
                width: 80,
                dataIndex: 'BillDate',
                sortable: true,
                menuDisabled: false
            },
              {
                  header: "提交时间",
                  width: 100,
                  dataIndex: 'SubTime',
                  sortable: true,
                  menuDisabled: false
              },
              {
                  header: "入库备注",
                  width: 100,
                  dataIndex: 'Remark',
                  sortable: true,
                  menuDisabled: false
              },
               {
                   header: '供应商金额',
                   sortable: true,
                   dataIndex: 'ReturnAmount',
                   menuDisabled: false,
                   width: 80
               },
               {
                   header: '退款金额',
                   sortable: true,
                   dataIndex: 'ProfitTotal',
                   menuDisabled: false,
                   width: 80,
                   renderer: renderMoney
               },
              {
                  header: '付款金额',
                  sortable: true,
                  dataIndex: 'PaymentAmmount',
                  menuDisabled: false,
                  width: 80,
                  renderer: renderMoney
              },
                 {
                     header: "盈亏金额",
                     dataIndex: 'LossPrice',
                     menuDisabled: true,
                     width: 90,
                     align: 'center',
                     renderer: renderMoney
                 },
        {
            header: "支付状态",
            width: 50,
            dataIndex: 'PayStatus',
            align: 'center',
            sortable: true,
            menuDisabled: false,
            renderer: renderPayStatus
        },
         {
             header: "支付时间",
             width: 100,
             dataIndex: 'PayTime',
             sortable: true,
             menuDisabled: false
         },
        {
            header: "财务备注",
            dataIndex: 'FinanceRemark',
            align: 'center',
            width: 100,
            sortable: true,
            menuDisabled: false,
            css: 'background:#B4EEB4;',
            editor: {
                xtype: 'textarea',
                height: 150,
                id: 'txtFinanceRemark',
                anchor: '95%',
                emptyText: '可输入对备注信息', ////textfield自己的属性
                maxLength: 200,
                maxLengthText: '备注长度不能超过1000个字符',
                listeners: {
                    blur: function (a, b, c) {

                    }
                }
            },
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
           width:50,
           sortable: true,
           menuDisabled: true,
           renderer: formartCheckoutType
       },
        {
            header: "付款方式",
            dataIndex: 'PaymentType',
            width:50,
            sortable: true,
            menuDisabled: true,
            renderer: formartPaymentType
        },
        {
            header: "名字",
            dataIndex: 'AccountName',
            width: 50,
            sortable: true,
            menuDisabled: true
        },
        {
            header: "账号",
            dataIndex: 'AccountNum',
            width: 80,
            sortable: true,
            menuDisabled: true
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
            'beforeedit': function (o) {
                var PayStatus = o.record.get('PayStatus');
                if (o.field == "FinanceRemark") {
                    if (PayStatus == 1) {
                        top.Ext.Msg.show({ title: "信息提示", msg: "单据已支付，不能进项编辑。", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                        IsEdit = false;
                        return false;
                    }
                    IsEdit = true;
                    return true;
                }
            }
        }

    });

    // 响应加载前事件，传递node参数 
    grid.store.on('beforeload', function (node) {
        //获取统计金额
        var search = GetSearch();
        grid.getStore().proxy.conn.url = '/CACAI/Finance/SearchData?' + search;
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
    grid.on('afteredit', afterEditMain, this);


    //下面明细
    var Human = Ext.data.Record.create([
                  { name: "Id", type: "string", mapping: "Id" },
                  { name: "SubTime", type: "datetime", mapping: "SubTime" },
                  { name: "OutNumber", type: "string", mapping: "OutNumber" },
                  { name: "GetNumber", type: "string", mapping: "GetNumber" },
                  { name: "LossPrice", type: "decimal", mapping: "LossPrice" },
                  { name: "TotalPrice", type: "decimal", mapping: "TotalPrice" },
                  { name: "BillDate", type: "datetime", mapping: "BillDate" },
                  { name: "ReturnAmount", type: "decimal", mapping: "ReturnAmount" },
                  { name: "DecomposePrice", type: "decimal", mapping: "DecomposePrice" },
                  { name: "Remark", type: "string", mapping: "Remark" },
                  { name: "HasFileAttach", type: "bool", mapping: "HasFileAttach" },
                  { name: "IsDecompose", type: "bit", mapping: "IsDecompose" },
                  { name: "PoolPrice", type: "decimal", mapping: "PoolPrice" },
                  { name: "CreateTime", type: "datetime", mapping: "CreateTime" }
     ]);
    var rightstore = GridStore(Human, '/CACAI/Finance/SearchDataDetail?MainId=' + treeNodeId, '', 'CreateTime', 'asc', false, true);

    var smm = new Ext.grid.CheckboxSelectionModel();
    var rightGrid = new Ext.grid.EditorGridPanel({
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
                new Ext.grid.RowNumberer({ header: '', width: 25 }), //设置行号
                 {header: "凭证", dataIndex: 'HasFileAttach', menuDisabled: true, width: 40, align: 'center',
                 renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                     if (value == true) {
                         return '<a title="凭证详情" href="#" onclick="ShowAttach2();"><img alt="凭证详情" style="vertical-align: middle; width:15px; height:15px;border:false" src="../../Resource/css/icons/toolbar/GTP_accessory.png"/>&nbsp;</a>';
                     }
                 }
             },
                    {
                        header: "退货单号",
                        dataIndex: 'OutNumber',
                        menuDisabled: true,
                        sortable: true,
                        width: 80
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
                    header: "提交时间",
                    dataIndex: 'SubTime',
                    menuDisabled: true,
                    sortable: true,
                    width: 80
                },
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
                        header: '拆分金额',
                        sortable: true,
                        dataIndex: 'DecomposePrice',
                        menuDisabled: false,
                        width:60,
                        renderer: renderMoney
                    },
            {
                header: "退货金额",
                dataIndex: 'ReturnAmount',
                menuDisabled: true,
                sortable: true,
                width: 90
            },
              {
                header: "剩余退货金额",
                dataIndex: 'PoolPrice',
                menuDisabled: true,
                sortable: true,
                width: 90
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
                header: "退货备注",
                dataIndex: 'Remark',
                menuDisabled: true,
                sortable: true,
                width:140,
                renderer: function (data, metadata, record, rowIndex, columnIndex, store) {
                    if (data) {
                        metadata.attr = 'ext:qtip="' + data + '"';
                        return data;
                    }
                }
            }
                ],
        //tbar: tbarDIY(className),
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
    // rightGrid.on('afteredit', afterEdit, this);
    //单元格选中
    //rightGrid.on('cellclick', cellclick);
    //默认选中第一行
    rightGrid.store.on("load", function () {
        Ext.getCmp('rightGrid').getSelectionModel().selectFirstRow(); //用这个就可以了，前提是你要有SelectionModel的实现
        //判断是否有数据
    });
    rightGrid.store.on('beforeload', function (node) {
        //判断是显示当前供应商还是全部
        rightGrid.getStore().proxy.conn.url = '/CACAI/Finance/SearchDataDetail?MainId=' + treeNodeId;
    });


    var north = new Ext.FormPanel({
        id: "formPanel",
        border: false,
        region: 'north',
        labelAlign: 'right',
        autoScroll: true,
        height: 35,
        labelWidth: 80,
        items: [
                  {
                      layout: "column", // 从左往右的布局
                      bodyStyle: 'padding-top:5px;padding-left:10px;padding-right:10px;',
                      border: false,
                      items: [
                         {
                             layout: "form",
                             border: false,
                             labelWidth: 50,
                             items: [
                                  { fieldLabel: '供应商', xtype: 'textfield', id: 'CusterName', name: 'CusterName', maxLength: 50,
                                      maxLengthText: '供应商长度不能超过50个字符', anchor: '90%', emptyText: '输入供应商名称',
                                      enableKeyEvents: true,
                                      listeners: {
                                          //回车事件
                                          specialkey: function (field, e) {
                                              if (e.getKey() == Ext.EventObject.ENTER) {
                                                  Ext.getCmp("gg").store.reload();
                                              }
                                          }
                                      }
                                  }
                            ]
                         },
                          {
                              layout: "form",
                              border: false,
                              labelWidth: 70,
                              items: [
                                 new Ext.form.ComboBox({
                                     store: new Ext.data.SimpleStore({
                                         fields: ['text', 'value'],
                                         data: [['全部', '-1'], ['未支付', '0'], ['已支付', '1']]
                                     }),
                                     loadingText: '加载中...',
                                     selectOnFocus: true,
                                     triggerAction: 'all',
                                     forceSelection: true,
                                     typeAhead: true, //模糊查询
                                     fieldLabel: '支付状态',
                                     displayField: 'text',
                                     valueField: 'value',
                                     mode: 'local',
                                     id: 'FinancialState',
                                     name: 'FinancialState',
                                     selectOnFocus: true,
                                     orceSelection: true,
                                     editable: false,
                                     triggerAction: 'all',
                                     value: '-1',
                                     anchor: '60%',
                                     enableKeyEvents: true,
                                     listeners: {
                                         //回车事件
                                         specialkey: function (field, e) {
                                             if (e.getKey() == Ext.EventObject.ENTER) {
                                                 Ext.getCmp("gg").store.reload();
                                             }
                                         }
                                     }
                                 })
                            ]
                          },
                          {
                              layout: "form",
                              border: false,
                              labelWidth: 70,
                              items: [
                                 new Ext.form.ComboBox({
                                     store: new Ext.data.SimpleStore({
                                         fields: ['text', 'value'],
                                         data: [['全部', '-1'], ['支付宝', '1'], ['工行', '2'], ['农行', '3']]
                                     }),
                                     fieldLabel: '付款方式',
                                     displayField: 'text',
                                     valueField: 'value',
                                     mode: 'local',
                                     id: 'PaymentType',
                                     name: 'PaymentType',
                                     selectOnFocus: true,
                                     orceSelection: true,
                                     value: '-1',
                                     editable: false,
                                     triggerAction: 'all',
                                     anchor: '60%',
                                     enableKeyEvents: true,
                                     listeners: {
                                         //回车事件
                                         specialkey: function (field, e) {
                                             if (e.getKey() == Ext.EventObject.ENTER) {
                                                 Ext.getCmp("gg").store.reload();
                                             }
                                         }
                                     }
                                 })
                            ]
                          },
                          {
                              layout: "form",
                              border: false,
                              labelWidth: 70,
                              items: [
                                 new Ext.form.ComboBox({
                                     store: new Ext.data.SimpleStore({
                                         fields: ['text', 'value'],
                                         data: [['全部', '-1'], ['月结', '1'], ['日结', '2']]
                                     }),
                                     fieldLabel: '结账方式',
                                     displayField: 'text',
                                     valueField: 'value',
                                     mode: 'local',
                                     id: 'CheckoutType',
                                     name: 'CheckoutType',
                                     selectOnFocus: true,
                                     orceSelection: true,
                                     editable: false,
                                     triggerAction: 'all',
                                     value: '-1',
                                     anchor: '60%',
                                     enableKeyEvents: true,
                                     listeners: {
                                         //回车事件
                                         specialkey: function (field, e) {
                                             if (e.getKey() == Ext.EventObject.ENTER) {
                                                 Ext.getCmp("gg").store.reload();
                                             }
                                         }
                                     }
                                 })
                            ]
                          },
                          {
                              layout: "form",
                              border: false,
                              labelWidth: 70,
                              items: [
                                       {
                                         xtype: 'compositefield',
                                         fieldLabel: '单据日期',
                                         combineErrors: false,
                                         width:350,
                                         items: [
                                            {
                                                id: 'BegBillDate',
                                                xtype: 'datefield',
                                                width: 100,
                                                emptyText: '开始日期',
                                                format: 'Y-m-d',
                                                vtype: 'daterange',
                                                endDateField: 'EndBillDate',
                                                enableKeyEvents: true,
                                                listeners: {
                                                    //回车事件
                                                    specialkey: function (field, e) {
                                                        if (e.getKey() == Ext.EventObject.ENTER) {
                                                            Ext.getCmp("gg").store.reload();
                                                        }
                                                    }
                                                }
                                            },
                                            {
                                                xtype: 'tbtext',
                                                text: '~'
                                            },
                                            { xtype: 'datefield', id: 'EndBillDate', emptyText: '', emptyText: '结束日期', format: 'Y-m-d', anchor: '90%', enableKeyEvents: true,
                                                listeners: {
                                                    //回车事件
                                                    specialkey: function (field, e) {
                                                        if (e.getKey() == Ext.EventObject.ENTER) {
                                                            Ext.getCmp("gg").store.reload();
                                                        }
                                                    }
                                                }
                                            },
                                            {
                                                text: '搜索',
                                                iconCls: 'GTP_search',
                                                xtype: 'button',
                                                id: 'BtnSearchAll',
                                                tooltip: '搜索满足条件的数据',
                                                scope: this,
                                                handler: function () {
                                                    Ext.getCmp("gg").store.reload();
                                                },
                                                enableKeyEvents: true,
                                                listeners: {
                                                    //回车事件
                                                    specialkey: function (field, e) {
                                                        if (e.getKey() == Ext.EventObject.ENTER) {
                                                            Ext.getCmp("gg").store.reload();
                                                        }
                                                    }
                                                }
                                            },
                                            {
                                                text: '重置',
                                                iconCls: 'GTP_eraser',
                                                xtype: 'button',
                                                tooltip: '重置',
                                                scope: this,
                                                handler: function () {
                                                    Ext.getCmp("formPanel").getForm().reset();
                                                    Ext.getCmp("gg").store.reload();
                                                }
                                            }
					                      ]
                                        }
                                    ]
                                    }
                            ]
                  }
                ]

    });



    //viewport
    var viewport = new Ext.Viewport({
        layout: 'border',
        items: [north, grid, rightGrid],
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












