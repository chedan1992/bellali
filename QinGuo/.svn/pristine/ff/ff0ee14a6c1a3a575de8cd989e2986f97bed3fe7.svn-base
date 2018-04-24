//创建表单
function CreateForm() {


    /*
    合同类型
    */
    var AppoinedType = new Ext.data.Store({
        proxy: new Ext.data.HttpProxy({
            url: '/SysDirc/GetSysDicByType?Type=0',
            method: 'POST'
        }),
        reader: new Ext.data.JsonReader({},
            Ext.data.Record.create(["Id", "Name"]
        ))
    });

    var form = new Ext.form.FormPanel({
        layout: "form", // 整个大的表单是form布局
        id: 'formPanel',
        border: false,
        labelWidth: 75,
        autoScroll: true,
        bodyStyle: 'padding:15px',
        labelAlign: "right",
        items: [
              {
                  layout: 'column',
                  border: false,
                  items: [{
                      columnWidth: 0.25,    // 平分宽度
                      layout: 'form',
                      border: false,
                      defaultype: 'textfield',
                      items: [
                          { fieldLabel: '<span class="required">*</span>合同编号', xtype: 'textfield', id: 'NumCode', name: 'NumCode', maxLength: 50,
                              allowBlank: false, emptyText: '请输入合同编号', maxLengthText: '合同编号长度不能超过50个字符', anchor: '90%'
                          },
					      { fieldLabel: '发起人', xtype: 'textfield', id: 'InitiatorUser', name: 'InitiatorUser', maxLength: 50,
					          emptyText: '', maxLengthText: '发起人长度不能超过50个字符', anchor: '90%'
					      },
					      { fieldLabel: '项目', xtype: 'textfield', id: 'Project', name: 'Project', maxLength: 50,
					          emptyText: '', maxLengthText: '项目长度不能超过50个字符', anchor: '90%'
					      },
					       { fieldLabel: '交付地点', xtype: 'textfield', id: 'DeliveriesAddress', name: 'DeliveriesAddress', maxLength: 50,
					           emptyText: '', maxLengthText: '交付地点长度不能超过50个字符', anchor: '90%'
					       },

                              { fieldLabel: '签订日期', xtype: 'datefield', id: 'SigningDate', name: 'SigningDate',
                                  emptyText: '', emptyText: '选择签订日期', format: 'Y-m-d(周l)', anchor: '90%'
                              },
                                { fieldLabel: '金额单位', xtype: 'textfield', id: 'CurrencyUnit', name: 'CurrencyUnit', maxLength: 50,
                                    emptyText: '', maxLengthText: '金额单位长度不能超过50个字符', anchor: '90%'
                                },
                                 { fieldLabel: '计划收款总金额', xtype: 'numberfield', id: 'PlanTotalAmount', name: 'PlanTotalAmount', maxLength: 50,
                                     emptyText: '', anchor: '90%', minValue: 0, maxValue: 999999999
                                 },
                                   { fieldLabel: '联系方式', xtype: 'textfield', id: 'LinType', name: 'LinType', maxLength: 50,
                                       emptyText: '', maxLengthText: '联系方式长度不能超过50个字符', anchor: '90%'
                                   },
                        { fieldLabel: '发票税率', xtype: 'textfield', id: 'TaxRate', name: 'TaxRate', maxLength: 50,
                            emptyText: '', maxLengthText: '发票税率长度不能超过50个字符', anchor: '90%'
                        },
                         { fieldLabel: '预算情况', xtype: 'textfield', id: 'BudgetSituation', name: 'BudgetSituation', maxLength: 50,
                             emptyText: '', maxLengthText: '联系方式长度不能超过50个字符', anchor: '90%'
                         },
                          { fieldLabel: '扩展字段1', xtype: 'textfield', id: 'cdefine1', name: 'cdefine1', maxLength: 50,
                              emptyText: '', maxLengthText: '扩展字段1长度不能超过50个字符', anchor: '90%'
                          },
                          { fieldLabel: '扩展字段5', xtype: 'textfield', id: 'cdefine5', name: 'cdefine5', maxLength: 50,
                              emptyText: '', maxLengthText: '扩展字段1长度不能超过50个字符', anchor: '90%'
                          },
                          { fieldLabel: '扩展字段9', xtype: 'textfield', id: 'cdefine9', name: 'cdefine9', maxLength: 50,
                              emptyText: '', maxLengthText: '扩展字段1长度不能超过50个字符', anchor: '90%'
                          },
                          {
                              xtype: 'fieldset',
                              title: '财务收款节点',
                              collapsible: false,
                              items: [
                                        {
                                            xtype: 'compositefield',
                                            fieldLabel: '节点1',
                                            id:'receivablesDate1',
                                            msgTarget: 'under',
                                            items: [
                                                   { fieldLabel: '收款日期', xtype: 'datefield', id: 'Date1', name: 'Date1',
                                                          emptyText: '',  format: 'Y-m-d(周l)', anchor: '100%'
                                                  },
                                                    { fieldLabel: '收款金额', xtype: 'numberfield', id: 'Money1', name: 'Money1', maxLength: 50,
                                                        emptyText: '', anchor: '100%', minValue: 0, maxValue: 999999999
                                                    }
                                            ]
                                                },
                                        {
                                            xtype: 'compositefield',
                                            fieldLabel: '节点2',
                                            id: 'receivablesDate2',
                                            msgTarget: 'under',
                                            items: [
                                                   { fieldLabel: '收款日期', xtype: 'datefield', id: 'Date2', name: 'Date2',
                                                       emptyText: '', format: 'Y-m-d(周l)', anchor: '100%'
                                                   },
                                                    { fieldLabel: '收款金额', xtype: 'numberfield', id: 'Money2', name: 'Money2', maxLength: 50,
                                                        emptyText: '', anchor: '100%', minValue: 0, maxValue: 999999999
                                                    }
                                            ]
                                                },
                                        {
                                            xtype: 'compositefield',
                                            fieldLabel: '节点3',
                                            id: 'receivablesDate3',
                                            msgTarget: 'under',
                                            items: [
                                                   { fieldLabel: '收款日期', xtype: 'datefield', id: 'Date3', name: 'Date3',
                                                       emptyText: '', format: 'Y-m-d(周l)', anchor: '100%'
                                                   },
                                                    { fieldLabel: '收款金额', xtype: 'numberfield', id: 'Money3', name: 'Money3', maxLength: 50,
                                                        emptyText: '', anchor: '100%', minValue: 0, maxValue: 999999999
                                                    }
                                            ]
                                                },
                                        {
                                            xtype: 'compositefield',
                                            fieldLabel: '节点4',
                                            id: 'receivablesDate4',
                                            msgTarget: 'under',
                                            items: [
                                                   { fieldLabel: '收款日期', xtype: 'datefield', id: 'Date4', name: 'Date4',
                                                       emptyText: '', format: 'Y-m-d(周l)', anchor: '100%'
                                                   },
                                                    { fieldLabel: '收款金额', xtype: 'numberfield', id: 'Money4', name: 'Money4', maxLength: 50,
                                                        emptyText: '', anchor: '100%', minValue: 0, maxValue: 999999999
                                                    }
                                            ]
                                                },
                                        {
                                                    xtype: 'compositefield',
                                                    fieldLabel: '节点5',
                                                    id: 'receivablesDate5',
                                                    msgTarget: 'under',
                                                    items: [
                                                   { fieldLabel: '收款日期', xtype: 'datefield', id: 'Date5', name: 'Date5',
                                                       emptyText: '', format: 'Y-m-d(周l)', anchor: '100%'
                                                   },
                                                    { fieldLabel: '收款金额', xtype: 'numberfield', id: 'Money5', name: 'Money5', maxLength: 50,
                                                        emptyText: '', anchor: '100%', minValue: 0, maxValue: 999999999
                                                    }
                                            ]
                                                }
                                     ]
                          }

                            ]
                  },
                      {
                          columnWidth: 0.25,    // 平分宽度
                          layout: 'form',
                          border: false,
                          defaultype: 'textfield',
                          items: [
                           { fieldLabel: '<span class="required">*</span>原合同编号', xtype: 'textfield', id: 'OldNumCode', name: 'OldNumCode', maxLength: 50,
                               allowBlank: false, maxLengthText: '原合同编号长度不能超过50个字符', anchor: '90%'
                           },
							  { fieldLabel: '发起部门', xtype: 'textfield', id: 'InitiatorDept', name: 'InitiatorDept', maxLength: 50,
							      emptyText: '', maxLengthText: '发起部门长度不能超过50个字符', anchor: '90%'
							  },
							 { fieldLabel: '子系统名称', xtype: 'textfield', id: 'Subsystem', name: 'Subsystem', maxLength: 50,
							     emptyText: '', maxLengthText: '子系统名称长度不能超过50个字符', anchor: '90%'
							 },
							 { fieldLabel: '项目阶段', xtype: 'textfield', id: 'ProjectPhase', name: 'ProjectPhase', maxLength: 50,
							     emptyText: '', maxLengthText: '项目阶段长度不能超过50个字符', anchor: '90%'
							 },

                              { fieldLabel: '交付日期', xtype: 'datefield', id: 'DeliverDate', name: 'DeliverDate',
                                  emptyText: '选择交付日期', format: 'Y-m-d(周l)', anchor: '90%'
                              },
                                 { fieldLabel: '合同总金额', xtype: 'numberfield', id: 'TotalContractAmount', name: 'TotalContractAmount', maxLength: 50,
                                     emptyText: '', anchor: '90%', minValue: 0, maxValue: 999999999
                                 },
                                  { fieldLabel: '协作单位名称', xtype: 'textfield', id: 'UnitName', name: 'UnitName', maxLength: 50,
                                      emptyText: '', maxLengthText: '协作单位名称长度不能超过50个字符', anchor: '90%'
                                  },
                                   { fieldLabel: '协作单位开户行', xtype: 'textfield', id: 'OpeningBank', name: 'OpeningBank', maxLength: 50,
                                       emptyText: '', maxLengthText: '协作单位开户行长度不能超过50个字符', anchor: '90%'
                                   },
                             { fieldLabel: '应开发票金额', xtype: 'numberfield', id: 'InvoiceValueBe', name: 'InvoiceValueBe', maxLength: 50,
                                 emptyText: '', anchor: '90%', minValue: 0, maxValue: 999999999
                             },
                               { fieldLabel: '依据文件', xtype: 'textfield', id: 'AccordingDocument', name: 'AccordingDocument', maxLength: 50,
                                   emptyText: '', maxLengthText: '依据文件长度不能超过50个字符', anchor: '90%'
                               },
                               { fieldLabel: '扩展字段2', xtype: 'textfield', id: 'cdefine2', name: 'cdefine1', maxLength: 50,
                                   emptyText: '', maxLengthText: '扩展字段2长度不能超过50个字符', anchor: '90%'
                               },
                               { fieldLabel: '扩展字段6', xtype: 'textfield', id: 'cdefine6', name: 'cdefine1', maxLength: 50,
                                   emptyText: '', maxLengthText: '扩展字段6长度不能超过50个字符', anchor: '90%'
                               },
                               { fieldLabel: '扩展字段10', xtype: 'textfield', id: 'cdefine10', name: 'cdefine1', maxLength: 50,
                                   emptyText: '', maxLengthText: '扩展字段10长度不能超过50个字符', anchor: '90%'
                               }
                            ]
                      },
                      {
                          columnWidth: 0.25,    // 平分宽度
                          layout: 'form',
                          border: false,
                          defaultype: 'textfield',
                          items: [
                         { fieldLabel: '<span class="required">*</span>合同名称', xtype: 'textfield', id: 'ContraceName', name: 'ContraceName', maxLength: 50,
                             allowBlank: false, maxLengthText: '合同名称长度不能超过50个字符', anchor: '90%'
                         },
							{ fieldLabel: '经办人', xtype: 'textfield', id: 'Agent', name: 'Agent', maxLength: 50,
							    emptyText: '', maxLengthText: '经办人长度不能超过50个字符', anchor: '90%'
							},
						{ fieldLabel: '产品说明', xtype: 'textfield', id: 'ProductDescription', name: 'ProductDescription', maxLength: 50,
						    emptyText: '', maxLengthText: '产品说明名称长度不能超过50个字符', anchor: '90%'
						},
						{ fieldLabel: '合同状态', xtype: 'textfield', id: 'ContractState', name: 'ContractState', maxLength: 50,
						    emptyText: '', maxLengthText: '合同状态长度不能超过50个字符', anchor: '90%'
						},
                         	{ fieldLabel: '有效期', xtype: 'textfield', id: 'ValidityDate', name: 'ValidityDate', maxLength: 50,
                         	    emptyText: '', maxLengthText: '有效期长度不能超过50个字符', anchor: '90%'
                         	},
                            { fieldLabel: '待收款金额', xtype: 'numberfield', id: 'ReceivablesAmount', name: 'ReceivablesAmount', maxLength: 50,
                                emptyText: '', anchor: '90%', minValue: 0, maxValue: 999999999
                            },
                              { fieldLabel: '协作单位地址', xtype: 'textfield', id: 'UnitAddress', name: 'UnitAddress', maxLength: 50,
                                  emptyText: '', maxLengthText: '协作单位地址长度不能超过50个字符', anchor: '90%'
                              },
                               { fieldLabel: '协作单位开户行账号', xtype: 'textfield', id: 'OpeningAccount', name: 'OpeningAccount', maxLength: 50,
                                   emptyText: '', maxLengthText: '协作单位开户行账号长度不能超过50个字符', anchor: '90%'
                               },
                                 { fieldLabel: '已开发票金额', xtype: 'numberfield', id: 'InvoiceValueHas', name: 'InvoiceValueHas', maxLength: 50,
                                     emptyText: '', anchor: '90%', minValue: 0, maxValue: 999999999
                                 },
                                 { fieldLabel: '归档情况', xtype: 'textfield', id: 'FilingSituation', name: 'FilingSituation', maxLength: 50,
                                     emptyText: '', maxLengthText: '归档情况长度不能超过50个字符', anchor: '90%'
                                 },
                                  { fieldLabel: '扩展字段3', xtype: 'textfield', id: 'cdefine3', name: 'cdefine1', maxLength: 50,
                                      emptyText: '', maxLengthText: '扩展字段3长度不能超过50个字符', anchor: '90%'
                                  },
                                   { fieldLabel: '扩展字段7', xtype: 'textfield', id: 'cdefine7', name: 'cdefine1', maxLength: 50,
                                       emptyText: '', maxLengthText: '扩展字段7长度不能超过50个字符', anchor: '90%'
                                   },
                                    {
                                        xtype: 'fieldset',
                                        title: '计划收款节点',
                                        collapsible: false,
                                        items: [
                                        {
                                            xtype: 'compositefield',
                                            fieldLabel: '节点1',
                                            id: 'receivablesPlan1',
                                            msgTarget: 'under',
                                            items: [
                                                   { fieldLabel: '收款日期', xtype: 'datefield', id: 'PlanDate1', name: 'PlanDate1',
                                                       emptyText: '', format: 'Y-m-d(周l)', anchor: '100%'
                                                   },
                                                    { fieldLabel: '收款金额', xtype: 'numberfield', id: 'PlanMoney1', name: 'PlanMoney1', maxLength: 50,
                                                        emptyText: '', anchor: '100%', minValue: 0, maxValue: 999999999
                                                    }
                                            ]
                                        },
                                        {
                                            xtype: 'compositefield',
                                            fieldLabel: '节点2',
                                            id: 'receivablesPlan2',
                                            msgTarget: 'under',
                                            items: [
                                                   { fieldLabel: '收款日期', xtype: 'datefield', id: 'PlanDate2', name: 'PlanDate2',
                                                       emptyText: '', format: 'Y-m-d(周l)', anchor: '100%'
                                                   },
                                                    { fieldLabel: '收款金额', xtype: 'numberfield', id: 'PlanMoney2', name: 'PlanMoney2', maxLength: 50,
                                                        emptyText: '', anchor: '100%', minValue: 0, maxValue: 999999999
                                                    }
                                            ]
                                        },
                                        {
                                            xtype: 'compositefield',
                                            fieldLabel: '节点3',
                                            id: 'receivablesPlan3',
                                            msgTarget: 'under',
                                            items: [
                                                   { fieldLabel: '收款日期', xtype: 'datefield', id: 'PlanDate3', name: 'PlanDate3',
                                                       emptyText: '', format: 'Y-m-d(周l)', anchor: '100%'
                                                   },
                                                    { fieldLabel: '收款金额', xtype: 'numberfield', id: 'PlanMoney3', name: 'PlanMoney3', maxLength: 50,
                                                        emptyText: '', anchor: '100%', minValue: 0, maxValue: 999999999
                                                    }
                                            ]
                                        },
                                        {
                                            xtype: 'compositefield',
                                            fieldLabel: '节点4',
                                            id: 'receivablesPlan4',
                                            msgTarget: 'under',
                                            items: [
                                                   { fieldLabel: '收款日期', xtype: 'datefield', id: 'PlanDate4', name: 'PlanDate4',
                                                       emptyText: '', format: 'Y-m-d(周l)', anchor: '100%'
                                                   },
                                                    { fieldLabel: '收款金额', xtype: 'numberfield', id: 'PlanMoney4', name: 'PlanMoney4', maxLength: 50,
                                                        emptyText: '', anchor: '100%', minValue: 0, maxValue: 999999999
                                                    }
                                            ]
                                        },
                                        {
                                            xtype: 'compositefield',
                                            fieldLabel: '节点5',
                                            id: 'receivablesPlan5',
                                            msgTarget: 'under',
                                            items: [
                                                   { fieldLabel: '收款日期', xtype: 'datefield', id: 'PlanDate5', name: 'PlanDate5',
                                                       emptyText: '', format: 'Y-m-d(周l)', anchor: '100%'
                                                   },
                                                    { fieldLabel: '收款金额', xtype: 'numberfield', id: 'PlanMoney5', name: 'PlanMoney5', maxLength: 50,
                                                        emptyText: '', anchor: '100%', minValue: 0, maxValue: 999999999
                                                    }
                                            ]
                                        }
                                     ]
                                    }
                         ]
                      }, {
                          columnWidth: 0.25,    // 平分宽度
                          layout: 'form',
                          labelWidth: 85,
                          border: false,
                          defaultype: 'textfield',
                          items: [
                           { fieldLabel: '合同性质', xtype: 'textfield', id: 'Nature', name: 'Nature', maxLength: 50,
                               maxLengthText: '合同性质名称长度不能超过50个字符', anchor: '90%'
                           },
							 { fieldLabel: '审批人', xtype: 'textfield', id: 'Approver', name: 'Approver', maxLength: 50,
							     emptyText: '', maxLengthText: '经办人长度不能超过50个字符', anchor: '90%'
							 },
							{ fieldLabel: '交付物及数量', xtype: 'textfield', id: 'DeliveriesQuantities', name: 'DeliveriesQuantities', maxLength: 50,
							    emptyText: '', maxLengthText: '交付物及数量长度不能超过50个字符', anchor: '90%'
							},
							{ fieldLabel: '合同执行情况', xtype: 'textfield', id: 'ContractIimplementation', name: 'ContractIimplementation', maxLength: 50,
							    emptyText: '', maxLengthText: '合同执行情况长度不能超过50个字符', anchor: '90%'
							},
                            { fieldLabel: '合同金额币种', xtype: 'textfield', id: 'Currency', name: 'Currency', maxLength: 50,
                                emptyText: '', maxLengthText: '合同金额币种长度不能超过50个字符', anchor: '90%'
                            },
                              { fieldLabel: '财务收款总金额', xtype: 'textfield', id: 'ReceiptsTotalAmount', name: 'ReceiptsTotalAmount', maxLength: 50,
                                  emptyText: '', anchor: '90%'
                              },
                               { fieldLabel: '联系人', xtype: 'textfield', id: 'LinkUser', name: 'LinkUser', maxLength: 50,
                                   emptyText: '', maxLengthText: '联系人长度不能超过50个字符', anchor: '90%'
                               },
                               { fieldLabel: '发票类型', xtype: 'textfield', id: 'InvoiceType', name: 'InvoiceType', maxLength: 50,
                                   emptyText: '', maxLengthText: '发票类型长度不能超过50个字符', anchor: '90%'
                               },
                                 { fieldLabel: '待开发票金额', xtype: 'textfield', id: 'InvoiceValueBefore', name: 'InvoiceValueBefore', maxLength: 50,
                                     emptyText: '', anchor: '90%'
                                 },
                                  { fieldLabel: '备注', xtype: 'textfield', id: 'Remark', name: 'Remark', maxLength: 50,
                                      emptyText: '', maxLengthText: '备注长度不能超过50个字符', anchor: '90%'
                                  },
                                  { fieldLabel: '扩展字段4', xtype: 'textfield', id: 'cdefine4', name: 'cdefine1', maxLength: 50,
                                      emptyText: '', maxLengthText: '扩展字段4长度不能超过50个字符', anchor: '90%'
                                  },
                                  { fieldLabel: '扩展字段8', xtype: 'textfield', id: 'cdefine8', name: 'cdefine1', maxLength: 50,
                                      emptyText: '', maxLengthText: '扩展字段8长度不能超过50个字符', anchor: '90%'
                                  }

                             ]
                      }

                  ]
              }
            ]
    });
    return form;
}

//创建工具条
function Toolbar() {
    var tb = new Ext.Toolbar();
    tb.add({
        text: '保存',
        iconCls: 'GTP_save',
        id: 'GTPsave',
        tooltip: '提交保存',
        handler: SaveDate
    });
    if ($("#HidKey").val() != "") {
        tb.add('-');
        tb.add({
            text: '附件管理',
            iconCls: 'GTP_accessory',
            tooltip: '附件管理',
            handler: function (sender) {
                FileAttach();
            }
        });
    }
    tb.add('-');
    tb.add({
        text: '关闭',
        iconCls: 'GTP_cancel',
        tooltip: '关闭取消',
        handler: function (sender) {
            CloseWindow();
        }
    });
    return tb;
};

//保存后关闭
function CloseWindow() {
    var id = parent.GetActiveTabId();
    if (id) {
        parent.CloseTabPanel(id); //传递页面id
    }
}

//附件管理
function FileAttach() {
    var key = $("#HidKey").val();
    var shop = new top.Ext.Window({
        width: 880,
        id: 'FileAttachWin',
        height: 510,
        closable: false,
        shadow: false,
        stateful: false,
        border: false,
        modal: true,
        layout: 'fit',
        plain: true,
        autoDestroy: true,
        closeAction: 'close',
        title: '附件管理',
        items: {
            autoScroll: true,
            border: false,
            params: { KeyId: key, ModelCode: "InCome" },
            autoLoad: { url: '../../Project/Html/FileAttach.htm', scripts: true, nocache: true }
        },
        buttons: [
                        {
                            text: '取消',
                            iconCls: 'GTP_cancel',
                            tooltip: '取消当前的操作',
                            handler: function () {
                                top.Ext.getCmp("FileAttachWin").close();
                            }
                        }]
    }).show();

}
