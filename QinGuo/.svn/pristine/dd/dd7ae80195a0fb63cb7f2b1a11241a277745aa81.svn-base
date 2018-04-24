
Ext.onReady(function () {
    //转义列
    var InCome = Ext.data.Record.create([
          { name: "Id", type: "string", mapping: "Id" },
              { name: "Status", type: "int", mapping: "Status" },
              { name: "CreateTime", type: "datetime", mapping: "CreateTime" },
              { name: "NatureName", type: "decimal", mapping: "NatureName" },
              { name: "ProjectName", type: "decimal", mapping: "ProjectName" },
              { name: "ProjectPhaseName", type: "decimal", mapping: "ProjectPhaseName" },
              { name: "ContractStateName", type: "decimal", mapping: "ContractStateName" },
              { name: "InvoiceTypeName", type: "decimal", mapping: "InvoiceTypeName" },
              { name: "NumCode", type: "string", mapping: "NumCode" },
              { name: "CreateUserName", type: "string", mapping: "CreateUserName" },
              { name: "OldNumCode", type: "string", mapping: "OldNumCode" },
              { name: "ContraceName", type: "string", mapping: "ContraceName" },
              { name: "Nature", type: "string", mapping: "Nature" },
              { name: "InitiatorUser", type: "string", mapping: "InitiatorUser" },
              { name: "InitiatorDept", type: "string", mapping: "InitiatorDept" },
              { name: "Agent", type: "string", mapping: "Agent" },
              { name: "Approver", type: "string", mapping: "Approver" },
              { name: "Project", type: "string", mapping: "Project" },
              { name: "Subsystem", type: "string", mapping: "Subsystem" },
              { name: "ProductDescription", type: "string", mapping: "ProductDescription" },
              { name: "DeliveriesQuantities", type: "string", mapping: "DeliveriesQuantities" },
              { name: "DeliveriesAddress", type: "string", mapping: "DeliveriesAddress" },
              { name: "ProjectPhase", type: "string", mapping: "ProjectPhase" },
              { name: "ContractState", type: "string", mapping: "ContractState" },
              { name: "ContractIimplementation", type: "string", mapping: "ContractIimplementation" },
              { name: "SigningDate", type: "datetime", mapping: "SigningDate" },
              { name: "DeliverDate", type: "datetime", mapping: "DeliverDate" },
              { name: "ValidityDate", type: "string", mapping: "ValidityDate" },
              { name: "Currency", type: "string", mapping: "Currency" },
              { name: "CurrencyUnit", type: "string", mapping: "CurrencyUnit" },
              { name: "TotalContractAmount", type: "decimal", mapping: "TotalContractAmount", summaryRenderer: renderSummary },
              { name: "ReceivablesAmount", type: "decimal", mapping: "ReceivablesAmount" },
              { name: "ReceiptsTotalAmount", type: "decimal", mapping: "ReceiptsTotalAmount" },
//              { name: "PlanTotalAmount", type: "decimal", mapping: "PlanTotalAmount" },
              { name: "HasFileAttach", type: "bool", mapping: "HasFileAttach" },
              { name: "UnitName", type: "string", mapping: "UnitName" },
              { name: "UnitAddress", type: "string", mapping: "UnitAddress" },
              { name: "LinkUser", type: "string", mapping: "LinkUser" },
              { name: "LinType", type: "string", mapping: "LinType" },
              { name: "OpeningBank", type: "string", mapping: "OpeningBank" },
              { name: "OpeningAccount", type: "string", mapping: "OpeningAccount" },
              { name: "InvoiceType", type: "string", mapping: "InvoiceType" },
              { name: "TaxRate", type: "string", mapping: "TaxRate" },
              { name: "InvoiceValueBe", type: "decimal", mapping: "InvoiceValueBe" },
              { name: "InvoiceValueHas", type: "decimal", mapping: "InvoiceValueHas" },
              { name: "InvoiceValueBefore", type: "decimal", mapping: "InvoiceValueBefore" },
              { name: "BudgetSituation", type: "string", mapping: "BudgetSituation" },
              { name: "AccordingDocument", type: "string", mapping: "AccordingDocument" },
              { name: "FilingSituation", type: "string", mapping: "FilingSituation" },
              { name: "Remark", type: "string", mapping: "Remark" },
              { name: "cdefine1", type: "string", mapping: "cdefine1" },
              { name: "cdefine1", type: "string", mapping: "cdefine1" },
              { name: "cdefine2", type: "string", mapping: "cdefine2" },
              { name: "cdefine3", type: "string", mapping: "cdefine3" },
              { name: "cdefine4", type: "string", mapping: "cdefine4" },
              { name: "cdefine5", type: "string", mapping: "cdefine5" },
              { name: "cdefine6", type: "string", mapping: "cdefine6" },
              { name: "cdefine7", type: "string", mapping: "cdefine7" },
              { name: "cdefine8", type: "string", mapping: "cdefine8" },
              { name: "cdefine9", type: "string", mapping: "cdefine9" },
              { name: "cdefine10", type: "string", mapping: "cdefine10" },
              { name: "TotalInvoice", type: "decimal", mapping: "TotalInvoice" },
              { name: "attribute1", type: "string", mapping: "attribute1" },
              { name: "attribute2", type: "string", mapping: "attribute2" },
              { name: "attribute3", type: "string", mapping: "attribute3" },
              { name: "attribute4", type: "string", mapping: "attribute4" },
              { name: "attribute5", type: "string", mapping: "attribute5" }
            ]);
    //快捷查询,如果不需要,可以不用定义
    var className = ''; //页面类名
    if (this.frameElement) {
        className = this.frameElement.name
    }
    var store = GridStore(InCome, '/Contract/InCome/SearchData?CType=4', className);

    //快捷查询,如果不需要,可以不用定义
    searchData = [['全查询', ''], ['合同名称', 'ContraceName'], ['合同性质', 'Nature'], ['项目', 'Project'], ['子系统名称', 'Subsystem'], ['项目阶段', 'ProjectPhase'], ['合同状态', 'ContractState'], ['合同总金额', 'TotalContractAmount'], ['协作单位', 'UnitName'], ['发票类型', 'InvoiceType'], ['税率', 'TaxRate'], ['预算情况', 'BudgetSituation'], ['归档情况', 'FilingSituation']];
    moreSearch = true; //更多查询按钮
    var sm = new top.Ext.grid.CheckboxSelectionModel();
    var gridId = 'gg';
    var center = new Ext.grid.GridPanel({
        id: gridId,
        region: 'center',
        layout: 'fit',
        store: store,
        stripeRows: true, //隔行颜色不同
        enableDragDrop: true, //禁用才能选择复选框
        loadMask: { msg: '数据请求中，请稍后...' },
        border: false,
        defult: {
            sortable: false
        },
        columns: [
                new Ext.grid.RowNumberer({ header: '', width: 25 }), //设置行号
                 sm, 
                 {header: "附件", dataIndex: 'HasFileAttach', menuDisabled: true, width: 40, align: 'center',
                 renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                     if (value == true) {
                         return '<a title="附件详情" href="#" onclick="FileAttach()"><img alt="附件详情" style="vertical-align: middle; width:15px; height:15px;border:false" src="../../Resource/css/icons/toolbar/GTP_accessory.png"/>&nbsp;</a>';
                     }
                 }
                },
               { header: "名称", dataIndex: 'ContraceName', menuDisabled: true, width: 150 },
               { header: "性质", dataIndex: 'NatureName', menuDisabled: true, width: 100 },
               { header: "规格/型号", dataIndex: 'NumCode', menuDisabled: true, width: 150 },
               { header: "类别", dataIndex: 'OldNumCode', menuDisabled: true, width: 150 },
               { header: "发起人", dataIndex: 'InitiatorUser', menuDisabled: true },
               { header: "发起部门", dataIndex: 'InitiatorDept', menuDisabled: true },
               { header: "项目", dataIndex: 'ProjectName', menuDisabled: true },
               { header: "子系统名称", dataIndex: 'Subsystem', menuDisabled: true },
               { header: "物品和数量", dataIndex: 'DeliveriesQuantities', menuDisabled: true },
               { header: "产品说明", dataIndex: 'ProductDescription', menuDisabled: true },
               { header: "起订量", dataIndex: 'attribute1', menuDisabled: true },
               { header: "开机费", dataIndex: 'attribute2', menuDisabled: true },
               { header: "属性3", dataIndex: 'attribute3', menuDisabled: true },
               { header: "属性4", dataIndex: 'attribute4', menuDisabled: true },
               { header: "属性5", dataIndex: 'attribute5', menuDisabled: true },
               { header: "报价地点", dataIndex: 'DeliveriesAddress', menuDisabled: true },
               { header: "报价日期", dataIndex: 'DeliverDate', menuDisabled: true },
               { header: "计量单位", dataIndex: 'OpeningBank', menuDisabled: true },
               { header: "金额", dataIndex: 'TotalContractAmount', menuDisabled: true },
               { header: "币种", dataIndex: 'Currency', menuDisabled: true },
               { header: "计价单位", dataIndex: 'OpeningAccount', menuDisabled: true },
               { header: "供应商名称", dataIndex: 'UnitName', menuDisabled: true },
               { header: "供应商地址", dataIndex: 'UnitAddress', menuDisabled: true },
               { header: "联系人", dataIndex: 'LinkUser', menuDisabled: true },
               { header: "联系方式", dataIndex: 'LinType', menuDisabled: true },
               { header: "发票类型", dataIndex: 'InvoiceTypeName', menuDisabled: true },
               { header: "发票税率", dataIndex: 'TaxRate', menuDisabled: true },
               { header: "预算情况", dataIndex: 'FilingSituation', menuDisabled: true },
               { header: "扩展1", dataIndex: 'cdefine1', menuDisabled: true },
               { header: "扩展2", dataIndex: 'cdefine2', menuDisabled: true },
               { header: "扩展3", dataIndex: 'cdefine3', menuDisabled: true },
               { header: "扩展4", dataIndex: 'cdefine4', menuDisabled: true },
               { header: "扩展5", dataIndex: 'cdefine5', menuDisabled: true },
               { header: "扩展6", dataIndex: 'cdefine6', menuDisabled: true },
               { header: "扩展7", dataIndex: 'cdefine7', menuDisabled: true },
               { header: "扩展8", dataIndex: 'cdefine8', menuDisabled: true },
               { header: "扩展9", dataIndex: 'cdefine9', menuDisabled: true },
               { header: "扩展10", dataIndex: 'cdefine10', menuDisabled: true },
               {
                           header: "添加人",
                           dataIndex: 'CreateUserName',
                           align: 'center',
                           flex: 1,
                           menuDisabled: true
             },
                    {
                        header: '添加时间',
                        dataIndex: 'CreateTime',
                        menuDisabled: true,
                        flex: 1,
                        renderer: renderCreateTime
                    }
                ],
        tbar: tbar(className),
        bbar: bbar(store, gridId, className), //分页工具条
        viewConfig: {
            //forceFit: true,//出现横向滚动条
            //enableRowBody: true,
            //scrollOffset: 0 //不加这个的话，会在grid的最右边有个空白，留作滚动条的位置
        },
        listeners: {
            beforerender: function () {
                //Ext.getCmp("tree").getRootNode();
            }
        }
    });

    // 响应加载前事件，传递node参数 
    center.store.on('beforeload', function (node) {
        //   center.getStore().proxy.conn.url = '/SysMasterUser/SearchData?TypeId=' + treeNodeId;
    });



    var south = new Ext.FormPanel({
        id: "formPanel",
        border: false,
        region: 'south',
        labelAlign: 'right',
        autoScroll: true,
        height: 30,
        bodyStyle: 'overflow-x:visible; overflow-y:visible',
        labelWidth: 80,
        items: [
                  {
                      layout: "column", // 从左往右的布局
                      bodyStyle: 'overflow-x:visible; overflow-y:visible',
                      border: false,
                      items: [
                         {
                             layout: "form",
                             border: false,
                             labelWidth: 50,
                             bodyStyle: 'overflow-x:visible; overflow-y:visible',
                             items: [
                                  {
                                      xtype: 'tbtext',
                                      html: '<h2>合同总金额:</h2>',
                                      style: 'margin-top:5px;margin-left:10px;color:red'
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
        items: [center],
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


var renderSummary = function (o, cs, cm) {
    return '合计：' + 123;
}

//双击分组
function GridClickwest(grid, rowindex, e) {
    var grid = Ext.getCmp("ggwest");
    var rows = grid.getSelectionModel().getSelected();
    treeNodeId = rows.data.Id;
    Ext.getCmp("gg").getStore().reload();
}