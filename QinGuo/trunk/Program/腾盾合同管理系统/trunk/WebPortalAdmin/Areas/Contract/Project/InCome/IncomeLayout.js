
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
              { name: "AgreedTime1", type: "datetime", mapping: "AgreedTime1" },
              { name: "AgreedTime2", type: "datetime", mapping: "AgreedTime2" },
              { name: "AgreedTime3", type: "datetime", mapping: "AgreedTime3" },
              { name: "AgreedTime4", type: "datetime", mapping: "AgreedTime4" },
              { name: "AgreedTime5", type: "datetime", mapping: "AgreedTime5" },
              { name: "FinancialTime1", type: "datetime", mapping: "FinancialTime1" },
              { name: "FinancialTime2", type: "datetime", mapping: "FinancialTime2" },
              { name: "FinancialTime3", type: "datetime", mapping: "FinancialTime3" },
              { name: "FinancialTime4", type: "datetime", mapping: "FinancialTime4" },
              { name: "FinancialTime5", type: "datetime", mapping: "FinancialTime5" },
              { name: "TicketTime1", type: "datetime", mapping: "TicketTime1" },
              { name: "TicketTime2", type: "datetime", mapping: "TicketTime2" },
              { name: "TicketTime3", type: "datetime", mapping: "TicketTime3" },
              { name: "TicketTime4", type: "datetime", mapping: "TicketTime4" },
              { name: "TicketTime5", type: "datetime", mapping: "TicketTime5" },
              { name: "AgreedMoney1", type: "decimal", mapping: "AgreedMoney1" },
              { name: "AgreedMoney2", type: "decimal", mapping: "AgreedMoney2" },
              { name: "AgreedMoney3", type: "decimal", mapping: "AgreedMoney3" },
              { name: "AgreedMoney4", type: "decimal", mapping: "AgreedMoney4" },
              { name: "AgreedMoney5", type: "decimal", mapping: "AgreedMoney5" },
              { name: "FinancialAmount1", type: "decimal", mapping: "FinancialAmount1" },
              { name: "FinancialAmount2", type: "decimal", mapping: "FinancialAmount2" },
              { name: "FinancialAmount3", type: "decimal", mapping: "FinancialAmount3" },
              { name: "FinancialAmount4", type: "decimal", mapping: "FinancialAmount4" },
              { name: "FinancialAmount5", type: "decimal", mapping: "FinancialAmount5" },
              { name: "TicketMoney1", type: "decimal", mapping: "TicketMoney1" },
              { name: "TicketMoney2", type: "decimal", mapping: "TicketMoney2" },
              { name: "TicketMoney3", type: "decimal", mapping: "TicketMoney3" },
              { name: "TicketMoney4", type: "decimal", mapping: "TicketMoney4" },
              { name: "TicketMoney5", type: "decimal", mapping: "TicketMoney5" }
            ]);
    //快捷查询,如果不需要,可以不用定义
    var className = ''; //页面类名
    if (this.frameElement) {
        className = this.frameElement.name
    }
    var store = GridStore(InCome, '/Contract/InCome/SearchData?CType=1', className);

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
              { header: "现行合同编号", dataIndex: 'NumCode', menuDisabled: true, width: 150 },
               { header: "原合同编号", dataIndex: 'OldNumCode', menuDisabled: true, width: 150 },
               { header: "合同名称", dataIndex: 'ContraceName', menuDisabled: true, width: 150 },
               { header: "合同性质", dataIndex: 'NatureName', menuDisabled: true, width: 100 },
               { header: "发起人", dataIndex: 'InitiatorUser', menuDisabled: true },
               { header: "发起部门", dataIndex: 'InitiatorDept', menuDisabled: true },
               { header: "经办人", dataIndex: 'Agent', menuDisabled: true },
               { header: "审批人", dataIndex: 'Approver', menuDisabled: true },
               { header: "项目", dataIndex: 'ProjectName', menuDisabled: true },
               { header: "子系统名称", dataIndex: 'Subsystem', menuDisabled: true },
               { header: "产品说明", dataIndex: 'ProductDescription', menuDisabled: true },
               { header: "交付物及数量", dataIndex: 'DeliveriesQuantities', menuDisabled: true },
               { header: "交付地点", dataIndex: 'DeliveriesAddress', menuDisabled: true },
               { header: "项目阶段", dataIndex: 'ProjectPhaseName', menuDisabled: true },
               { header: "合同状态", dataIndex: 'ContractStateName', menuDisabled: true },
               { header: "合同执行情况", dataIndex: 'ContractIimplementation', menuDisabled: true },
               { header: "签订日期", dataIndex: 'SigningDate', menuDisabled: true },
               { header: "交付日期", dataIndex: 'DeliverDate', menuDisabled: true },
               { header: "有效期", dataIndex: 'ValidityDate', menuDisabled: true },
               { header: "合同金额币种", dataIndex: 'Currency', menuDisabled: true },
               { header: "金额单位", dataIndex: 'CurrencyUnit', menuDisabled: true },
               { header: "合同总金额", dataIndex: 'TotalContractAmount', menuDisabled: true },
               { header: "待收款金额", dataIndex: 'ReceivablesAmount', menuDisabled: true },
               { header: "已开票总金额", dataIndex: 'TotalInvoice', menuDisabled: true },
               { header: "财务收款总金额", dataIndex: 'ReceiptsTotalAmount', menuDisabled: true },
               { header: "约定收款时间1", dataIndex: 'AgreedTime1', menuDisabled: true },
               { header: "约定收款金额1", dataIndex: 'AgreedMoney1', menuDisabled: true },
               { header: "约定收款时间2", dataIndex: 'AgreedTime2', menuDisabled: true },
               { header: "约定收款金额2", dataIndex: 'AgreedMoney2', menuDisabled: true },
               { header: "约定收款时间3", dataIndex: 'AgreedTime3', menuDisabled: true },
               { header: "约定收款金额3", dataIndex: 'AgreedMoney3', menuDisabled: true },
               { header: "约定收款时间4", dataIndex: 'AgreedTime4', menuDisabled: true },
               { header: "约定收款金额4", dataIndex: 'AgreedMoney4', menuDisabled: true },
               { header: "约定收款时间5", dataIndex: 'AgreedTime5', menuDisabled: true },
               { header: "约定收款金额5", dataIndex: 'AgreedMoney5', menuDisabled: true },
               { header: "财务实收时间1", dataIndex: 'FinancialTime1', menuDisabled: true },
               { header: "财务实收金额1", dataIndex: 'FinancialAmount1', menuDisabled: true },
               { header: "财务实收时间2", dataIndex: 'FinancialTime2', menuDisabled: true },
               { header: "财务实收金额2", dataIndex: 'FinancialAmount2', menuDisabled: true },
               { header: "财务实收时间3", dataIndex: 'FinancialTime3', menuDisabled: true },
               { header: "财务实收金额3", dataIndex: 'FinancialAmount3', menuDisabled: true },
               { header: "财务实收时间4", dataIndex: 'FinancialTime4', menuDisabled: true },
               { header: "财务实收金额4", dataIndex: 'FinancialAmount4', menuDisabled: true },
               { header: "财务实收时间5", dataIndex: 'FinancialTime5', menuDisabled: true },
               { header: "财务实收金额5", dataIndex: 'FinancialAmount5', menuDisabled: true },
               { header: "开票节点时间1", dataIndex: 'TicketTime1', menuDisabled: true },
               { header: "开票节点金额1", dataIndex: 'TicketMoney1', menuDisabled: true },
               { header: "开票节点时间2", dataIndex: 'TicketTime2', menuDisabled: true },
               { header: "开票节点金额2", dataIndex: 'TicketMoney2', menuDisabled: true },
               { header: "开票节点时间3", dataIndex: 'TicketTime3', menuDisabled: true },
               { header: "开票节点金额3", dataIndex: 'TicketMoney3', menuDisabled: true },
               { header: "开票节点时间4", dataIndex: 'TicketTime4', menuDisabled: true },
               { header: "开票节点金额4", dataIndex: 'TicketMoney4', menuDisabled: true },
               { header: "开票节点时间5", dataIndex: 'TicketTime5', menuDisabled: true },
               { header: "开票节点金额5", dataIndex: 'TicketMoney5', menuDisabled: true },
//               { header: "计划收款付总金额", dataIndex: 'PlanTotalAmount', menuDisabled: true },
               { header: "协作单位名称", dataIndex: 'UnitName', menuDisabled: true },
                { header: "协作单位地址", dataIndex: 'UnitAddress', menuDisabled: true },
               { header: "联系人", dataIndex: 'LinkUser', menuDisabled: true },
               { header: "联系方式", dataIndex: 'LinType', menuDisabled: true },
               { header: "协作单位开户行", dataIndex: 'OpeningBank', menuDisabled: true },
                { header: "协作单位开户行账号", dataIndex: 'OpeningAccount', menuDisabled: true },
               { header: "发票类型", dataIndex: 'InvoiceTypeName', menuDisabled: true },
               { header: "发票税率", dataIndex: 'TaxRate', menuDisabled: true },
               { header: "应开发票金额", dataIndex: 'InvoiceValueBe', menuDisabled: true },
                { header: "已开发票金额", dataIndex: 'InvoiceValueHas', menuDisabled: true },
               { header: "待开发票金额", dataIndex: 'InvoiceValueBefore', menuDisabled: true },
               { header: "预算情况", dataIndex: 'BudgetSituation', menuDisabled: true },
               { header: "依据文件", dataIndex: 'AccordingDocument', menuDisabled: true },
               { header: "归档情况", dataIndex: 'FilingSituation', menuDisabled: true },
//               { header: "备注", dataIndex: 'Remark', menuDisabled: true },
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