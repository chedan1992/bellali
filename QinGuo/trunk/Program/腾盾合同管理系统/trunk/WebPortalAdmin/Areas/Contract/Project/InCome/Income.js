/** 
* param 将要转为URL参数字符串的对象 
* key URL参数字符串的前缀 
* encode true/false 是否进行URL编码,默认为true 
*  
* return URL参数字符串 
*/
var urlEncode = function (param, key, encode) {
    if (param == null) return '';
    var paramStr = '';
    var t = typeof (param);
    if (t == 'string' || t == 'number' || t == 'boolean') {
        paramStr += '&' + key + '=' + ((encode == null || encode) ? encodeURIComponent(param) : param);
    } else {
        for (var i in param) {
            var k = key == null ? i : key + (param instanceof Array ? '[' + i + ']' :  i);
            paramStr += urlEncode(param[i], k, encode);
        }
    }
    return paramStr;
}; 
//附件管理
function FileAttach() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].get("Id");
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
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }

   
}
//导入
function ImportDate() {
    //下载地址
    var downMoBanUlr = "/Project/Template/收入合同模板.xlsx";
    var panel = new top.Ext.Panel({
        frame: true,
        layout: "card",
        activeItem: 0,
        defaults: {
            bodyStyle: "padding:3px; background-color: #FFFFFF"
        },
        items: [
            {
                id: "c1",
                xtype: 'panel',
                items: [
                new top.Ext.FormPanel({
                    xtype: 'panel',
                    layout: 'form',
                    labelAlign: 'right',
                    fileUpload: true, //有需文件上传的,需填写该属性
                    id: 'formPanel1',
                    labelWidth: 75,
                    bodyStyle: 'padding:15px;',
                    border: false,
                    defaultType: 'textfield',
                    items: [
                                {
                                    id: 'moban',
                                    name: 'moban',
                                    bodyStyle: 'border:0px;',
                                    fieldLabel: '导入模板',
                                    style: 'padding-top:3px;',
                                    xtype: 'panel',
                                    width: 300,
                                    anchor: '90%',
                                    html: '<a target=blank href="' + downMoBanUlr + '">模板下载</a>'
                                },
                                {
                                    xtype: 'tbtext',
                                    text: '<br/>'
                                },
                                {
                                    xtype: 'compositefield',
                                    fieldLabel: '附件文件',
                                    id: 'compositefieldPic',
                                    combineErrors: false,
                                    anchor: '85%',
                                    items: [

                                                    {
                                                        xtype: 'fileuploadfield',
                                                        id: 'excel',
                                                        emptyText: '请上传Excel格式文件',
                                                        name: 'excel',
                                                        buttonText: '',
                                                        allowBlank: false,
                                                        buttonCfg: {
                                                            iconCls: 'image_add',
                                                            tooltip: '附件选择'
                                                        },
                                                        width: 400,
                                                        listeners: {
                                                            'fileselected': {
                                                                fn: top.ExcelUploadAction,
                                                                scope: this
                                                            }
                                                        }
                                                    }
						                        ]
                                },
                                {
                                    xtype: 'tbtext',
                                    text: '<br/>'
                                },
                                {
                                    fieldLabel: '提示说明',
                                    xtype: 'panel',
                                    border: false,
                                    width: 300,
                                    style: 'padding-top:4px;color:#acacac',
                                    anchor: '90%',
                                    html: '1 导入的字典下拉数据选项需要和系统对应,没有则自动创建。<br/>'

                                }

                            ]
                })
                ]
            },
            { id: "c2",
                xtype: 'panel',
                items: [
               new top.Ext.FormPanel({
                   xtype: 'panel',
                   layout: 'form',
                   id: 'formPanel4',
                   labelAlign: 'right',
                   labelWidth: 75,
                   bodyStyle: 'padding:15px;',
                   border: false,
                   defaultType: 'textfield',
                   items: [
                         {
                             name: 'downerror',
                             style: 'color:Red',
                             bodyStyle: 'border:0px;',
                             id: 'downerror',
                             fieldLabel: '信息提示',
                             style: 'padding-left:5px;padding-top:3px;',
                             xtype: 'panel',
                             width: 300,
                             anchor: '90%',
                             html: ''
                         }
                     ]
               })
            ]
            }
        ],
        buttons: [
        {
            text: "上一步",
            hidden: true,
            id:'btnBack',
            handler: changePage
        },
        {
            text: "下一步",
            id:'BtnNext',
            handler: changePage
        },
         {
             text: "取消",
             handler: function () {
                 top.Ext.getCmp("Export").close();
             }
         }
    ]
    });

    var shop = new top.Ext.Window({
        width: 620,
        id: 'Export',
        height: 428,
        closable: false,
        shadow: false,
        stateful: false,
        border: false,
        modal: true,
        layout: 'fit',
        plain: true,
        autoDestroy: true,
        closeAction: 'close',
        title: '批量导入 1:文件上传',
        items: [panel]
    }).show();

    function changePage(btn) {
        var index = Number(panel.layout.activeItem.id.substring(1));
        if (btn.text == "上一步") {
            index -= 1;
            if (index == 1) {
                top.Ext.getCmp("Export").setTitle("批量导入 1:文件上传");
                top.Ext.getCmp("BtnNext").show();
            }
            if (index == 2) {
                top.Ext.getCmp("Export").setTitle("批量导入 2:数据导入");
            }
            if (index < 1) index = 1;
        }
        else {
            index += 1;
            if (index == 1) {
                var formPanel = top.Ext.getCmp("formPanel1");
                if (formPanel.getForm().isValid()) {//如果验证通过
                    top.Ext.getCmp("btnBack").hide();
                    top.Ext.getCmp("BtnNext").show();
                    top.Ext.getCmp("Export").setTitle("批量导入 1:文件上传");
                }
                else {
                    return false;
                }
            }
            if (index == 2) {
                var formPanel = top.Ext.getCmp("formPanel1");
                if (formPanel.getForm().isValid()) {//如果验证通过
                    top.Ext.getCmp("Export").setTitle("批量导入 2:数据导入");
                    top.Ext.getCmp("btnBack").show();
                    top.Ext.getCmp("BtnNext").hide();
                    // var para = { CType: 1 };
                    formPanel.getForm().submit({
                        waitTitle: '系统提示', //标题
                        waitMsg: '正在导入,请稍后...', //提示信息
                        submitEmptyText: false,
                        method: "POST",
                        url: '/Contract/InCome/ImportDate?CType=1', 
                        //params: para,
                        success: function (form, action) {
                            var flag = action.result; //成功后
                            var a = '';
                            if (flag.success) {
                                MessageInfo("导入成功！", "right");
                                a += '(' + flag.msg + ')<br/>';
                                if (flag.data.length > 0) {
                                    a += '<a style="color:Red" target=blank href="' + flag.data + '">下载错误数据</a><br/>';
                                }
                                Ext.getCmp("gg").getStore().reload();
                            } else {
                                var a = '<span style="color:Red">导入失败,文件有误.</span>';
                                MessageInfo("导入失败,文件有误", "error");
                            }
                            top.Ext.getCmp('downerror').show();
                            top.Ext.getCmp("downerror").update(a);
                            //导入数据
                            top.Ext.getCmp("Export").setTitle("批量导入 2:开始导入");
                        },
                        failure: function (form, action) {
                            MessageInfo("保存失败！", "error");
                        }
                    });
                }
                else {
                    return false;
                }
            }
            if (index > 2) index = 2;
        }
        panel.layout.setActiveItem("c" + index);
    }
}
//批量导出
function BatchExport() {
    var grid = Ext.getCmp("gg");
    if (grid.store.data.length > 0) {
        var confirm = top.Ext.MessageBox.confirm('系统确认', '您确定要导出当前列表信息吗?', function (e) {
            if (e == "yes") {
                top.Ext.MessageBox.wait('', '正在导出数据,请稍候...');

                var param = GetParam();
                //高级查询
                var comName = Ext.getCmp("comName").getValue(); //字段名称
                var comSign = Ext.getCmp("comSign").getValue(); //查询方式
                var comContent = Ext.getCmp("comContent").getValue(); //查询值
                var comTimeContent = Ext.getCmp("comTimeContent").value; //查询时间值
                if (!Ext.getCmp("comTimeContent").hidden) {//时间控件
                    comContent = comTimeContent;
                    //时间列需要转换
                    comName = "Convert(varchar(20)," + comName + ",120)";
                }
                else if (!Ext.getCmp("comEnum").hidden)//枚举控件
                {
                    comContent = Ext.getCmp("comEnum").getValue();
                }

                param.conditionField = comName;
                param.condition = comSign;
                param.conditionValue = comContent;

                var aa = urlEncode(param,"",true);
                var url = '/Contract/InCome/ImportOut?CType=1' + aa + '&date=' + new Date();
                $(".hideform").attr("action", url);
                $(".hideform").submit();
                top.Ext.MessageBox.hide(); //完成后隐藏消息框
            }
        });
    } else {
        MessageInfo("没有数据可以导出！", "statusing");
    }
}
//删除
function DeleteDate() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var confirm = top.Ext.MessageBox.confirm('系统确认', '删除合同,相关信息一并删除.确认要删除该合同信息吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push("'" + rows[i].data["Id"] + "'");
                }
                Ext.Ajax.request({
                    url: '/Contract/InCome/DeleteData',
                    params: { id:ids.join(",")},
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            //判断是否在grid最后一条的时候删除,如果删除,重新加载
                            Ext.getCmp("gg").store.reload();
                            MessageInfo("删除成功！", "right");
                        } else {
                            MessageInfo("删除失败！", "error");
                        }
                    }
                });
            }
        });
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}
//新增
function AddDate() {
//    if (parent) {
//        var tabId = parent.GetActiveTabId(); //公共变量
//        parent.AddTabPanel('新增收入合同', 'AddInCome', tabId, '/Contract/InCome/IncomeEdit?CType=1');
    //    }
    var window = CreateFromWindow("新增收入合同", "");
    var form = top.Ext.getCmp('formPanel');
    form.form.reset();
    url = '/Contract/InCome/SaveData?CType=1';
    window.show();
}
//编辑
function EditDate() {

    //        if (parent) {
    //            var tabId = parent.GetActiveTabId(); //公共变量
    //            parent.AddTabPanel(title, 'AddInCome', tabId, '/Contract/InCome/IncomeEdit?Id=' + key + '&modify=1');
    //        }

    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].get("Id");
        var window = CreateFromWindow("编辑", key);
        var form = top.Ext.getCmp('formPanel');
        form.form.loadRecord(rows[0]); //再加载数据
        window.show();
       
       //合同性质
        top.Ext.getCmp("NatureName").setValue(rows[0].data.Nature); //value
        top.Ext.getCmp("NatureName").setRawValue(rows[0].data.NatureName); //text

        //项目
        top.Ext.getCmp("ProjectName").setValue(rows[0].data.Project); //value
        top.Ext.getCmp("ProjectName").setRawValue(rows[0].data.ProjectName); //text

        //合同阶段
        top.Ext.getCmp("ProjectPhaseName").setValue(rows[0].data.ProjectPhase); //value
        top.Ext.getCmp("ProjectPhaseName").setRawValue(rows[0].data.ProjectPhaseName); //text

        //合同状态
        top.Ext.getCmp("ContractStateName").setValue(rows[0].data.ContractState); //value
        top.Ext.getCmp("ContractStateName").setRawValue(rows[0].data.ContractStateName); //text

        //发票类型
        top.Ext.getCmp("InvoiceTypeName").setValue(rows[0].data.InvoiceType); //value
        top.Ext.getCmp("InvoiceTypeName").setRawValue(rows[0].data.InvoiceTypeName); //text

        //交付日期
        if (rows[0].data.DeliverDate != "") {
            var DeliverDate = new Date(formartTime(rows[0].data.DeliverDate).format('Y-m-d'));
            if (DeliverDate == "NaN") {
                DeliverDate = formartTime(rows[0].data.DeliverDate).format('Y-m-d');
            }
            top.Ext.getCmp("DeliverDate").setValue(DeliverDate);
        }
        //签订日期
        if (rows[0].data.SigningDate != "") {
            var SigningDate = new Date(formartTime(rows[0].data.SigningDate).format('Y-m-d'));
            if (SigningDate == "NaN") {
                SigningDate = formartTime(rows[0].data.SigningDate).format('Y-m-d');
            }
            top.Ext.getCmp("SigningDate").setValue(SigningDate);
        }
        //约定收款
        if (rows[0].data.AgreedTime1 != "") {
            var AgreedTime1 = new Date(formartTime(rows[0].data.AgreedTime1).format('Y-m-d'));
            if (AgreedTime1 == "NaN") {
                AgreedTime1 = formartTime(rows[0].data.AgreedTime1).format('Y-m-d');
            }
            top.Ext.getCmp("AgreedTime1").setValue(AgreedTime1);
        }
        if (rows[0].data.AgreedTime2 != "") {
            var AgreedTime2 = new Date(formartTime(rows[0].data.AgreedTime2).format('Y-m-d'));
            if (AgreedTime2 == "NaN") {
                AgreedTime2 = formartTime(rows[0].data.AgreedTime2).format('Y-m-d');
            }
            top.Ext.getCmp("AgreedTime2").setValue(AgreedTime2);
        }
        if (rows[0].data.AgreedTime3 != "") {
            var AgreedTime3 = new Date(formartTime(rows[0].data.AgreedTime3).format('Y-m-d'));
            if (AgreedTime3 == "NaN") {
                AgreedTime3 = formartTime(rows[0].data.AgreedTime3).format('Y-m-d');
            }
            top.Ext.getCmp("AgreedTime3").setValue(AgreedTime3);
        }
        if (rows[0].data.AgreedTime4 != "") {
            var AgreedTime4 = new Date(formartTime(rows[0].data.AgreedTime4).format('Y-m-d'));
            if (AgreedTime4 == "NaN") {
                AgreedTime4 = formartTime(rows[0].data.AgreedTime4).format('Y-m-d');
            }
            top.Ext.getCmp("AgreedTime4").setValue(AgreedTime4);
        }
        if (rows[0].data.AgreedTime5 != "") {
            var AgreedTime5 = new Date(formartTime(rows[0].data.AgreedTime5).format('Y-m-d'));
            if (AgreedTime5 == "NaN") {
                AgreedTime5 = formartTime(rows[0].data.AgreedTime5).format('Y-m-d');
            }
            top.Ext.getCmp("AgreedTime5").setValue(AgreedTime5);
        }
        //财务实收

        if (rows[0].data.FinancialTime1 != "") {
            var FinancialTime1 = new Date(formartTime(rows[0].data.FinancialTime1).format('Y-m-d'));
            if (FinancialTime1 == "NaN") {
                FinancialTime1 = formartTime(rows[0].data.FinancialTime1).format('Y-m-d');
            }
            top.Ext.getCmp("FinancialTime1").setValue(FinancialTime1);
        }
        if (rows[0].data.FinancialTime2 != "") {
            var FinancialTime2 = new Date(formartTime(rows[0].data.FinancialTime2).format('Y-m-d'));
            if (FinancialTime2 == "NaN") {
                FinancialTime2 = formartTime(rows[0].data.FinancialTime2).format('Y-m-d');
            }
            top.Ext.getCmp("FinancialTime2").setValue(FinancialTime2);
        }
        if (rows[0].data.FinancialTime3 != "") {
            var FinancialTime3 = new Date(formartTime(rows[0].data.FinancialTime3).format('Y-m-d'));
            if (FinancialTime3 == "NaN") {
                FinancialTime3 = formartTime(rows[0].data.FinancialTime3).format('Y-m-d');
            }
            top.Ext.getCmp("FinancialTime3").setValue(FinancialTime3);
        }
        if (rows[0].data.FinancialTime4 != "") {
            var FinancialTime4 = new Date(formartTime(rows[0].data.FinancialTime4).format('Y-m-d'));
            if (FinancialTime4 == "NaN") {
                FinancialTime4 = formartTime(rows[0].data.FinancialTime4).format('Y-m-d');
            }
            top.Ext.getCmp("FinancialTime4").setValue(FinancialTime4);
        }
        if (rows[0].data.FinancialTime5 != "") {
            var FinancialTime5 = new Date(formartTime(rows[0].data.FinancialTime5).format('Y-m-d'));
            if (FinancialTime5 == "NaN") {
                FinancialTime5 = formartTime(rows[0].data.FinancialTime5).format('Y-m-d');
            }
            top.Ext.getCmp("FinancialTime5").setValue(FinancialTime5);
        }
        //支付节点
        if (rows[0].data.TicketTime1 != "") {
            var TicketTime1 = new Date(formartTime(rows[0].data.TicketTime1).format('Y-m-d'));
            if (TicketTime1 == "NaN") {
                TicketTime1 = formartTime(rows[0].data.TicketTime1).format('Y-m-d');
            }
            top.Ext.getCmp("TicketTime1").setValue(TicketTime1);
        }
        if (rows[0].data.TicketTime2 != "") {
            var TicketTime2 = new Date(formartTime(rows[0].data.TicketTime2).format('Y-m-d'));
            if (TicketTime2 == "NaN") {
                TicketTime2 = formartTime(rows[0].data.TicketTime2).format('Y-m-d');
            }
            top.Ext.getCmp("TicketTime2").setValue(TicketTime2);
        }
        if (rows[0].data.TicketTime3 != "") {
            var TicketTime3 = new Date(formartTime(rows[0].data.TicketTime3).format('Y-m-d'));
            if (TicketTime3 == "NaN") {
                TicketTime3 = formartTime(rows[0].data.TicketTime3).format('Y-m-d');
            }
            top.Ext.getCmp("TicketTime3").setValue(TicketTime3);
        }
        if (rows[0].data.TicketTime4 != "") {
            var TicketTime4 = new Date(formartTime(rows[0].data.TicketTime4).format('Y-m-d'));
            if (TicketTime4 == "NaN") {
                TicketTime4 = formartTime(rows[0].data.TicketTime4).format('Y-m-d');
            }
            top.Ext.getCmp("TicketTime4").setValue(TicketTime4);
        }
        if (rows[0].data.TicketTime5 != "") {
            var TicketTime5 = new Date(formartTime(rows[0].data.TicketTime5).format('Y-m-d'));
            if (TicketTime5 == "NaN") {
                TicketTime5 = formartTime(rows[0].data.TicketTime5).format('Y-m-d');
            }
            top.Ext.getCmp("TicketTime5").setValue(TicketTime1);
        } 
        url = '/Contract/InCome/SaveData?Id=' + key + '&modify=1';
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}
 //合计财务付款总额
function TotalReceiptsTotalAmount() {
    var FinancialAmount1 = top.Ext.getCmp("FinancialAmount1").getValue();
    if (FinancialAmount1 == "") {
        FinancialAmount1 = 0;
    }
    var FinancialAmount2 = top.Ext.getCmp("FinancialAmount2").getValue();
    if (FinancialAmount2 == "") {
        FinancialAmount2 = 0;
    }
    var FinancialAmount3 = top.Ext.getCmp("FinancialAmount3").getValue();
    if (FinancialAmount3 == "") {
        FinancialAmount3 = 0;
    }
    var FinancialAmount4 = top.Ext.getCmp("FinancialAmount4").getValue();
    if (FinancialAmount4 == "") {
        FinancialAmount4 = 0;
    }
    var FinancialAmount5 = top.Ext.getCmp("FinancialAmount5").getValue();
    if (FinancialAmount5 == "") {
        FinancialAmount5 = 0;
    }
    var temp=parseFloat(FinancialAmount1) + parseFloat(FinancialAmount2) + parseFloat(FinancialAmount3) + parseFloat(FinancialAmount4) + parseFloat(FinancialAmount5);
    top.Ext.getCmp("ReceiptsTotalAmount").setValue(temp);

    var TotalContractAmount = top.Ext.getCmp("TotalContractAmount").getValue();
    var ReceiptsTotalAmount = top.Ext.getCmp("ReceiptsTotalAmount").getValue();
    top.Ext.getCmp("ReceivablesAmount").setValue(TotalContractAmount - ReceiptsTotalAmount);

}
//合计已开发票金额
function TotalInvoiceValueHas() {
    var FinancialAmount1 = top.Ext.getCmp("TicketMoney1").getValue();
    if (FinancialAmount1 == "") {
        FinancialAmount1 = 0;
    }
    var FinancialAmount2 = top.Ext.getCmp("TicketMoney2").getValue();
    if (FinancialAmount2 == "") {
        FinancialAmount2 = 0;
    }
    var FinancialAmount3 = top.Ext.getCmp("TicketMoney3").getValue();
    if (FinancialAmount3 == "") {
        FinancialAmount3 = 0;
    }
    var FinancialAmount4 = top.Ext.getCmp("TicketMoney4").getValue();
    if (FinancialAmount4 == "") {
        FinancialAmount4 = 0;
    }
    var FinancialAmount5 = top.Ext.getCmp("TicketMoney5").getValue();
    if (FinancialAmount5 == "") {
        FinancialAmount5 = 0;
    }
    var temp = parseFloat(FinancialAmount1) + parseFloat(FinancialAmount2) + parseFloat(FinancialAmount3) + parseFloat(FinancialAmount4) + parseFloat(FinancialAmount5);
    top.Ext.getCmp("InvoiceValueHas").setValue(temp);
}
 //创建控件面板
function CreateFromWindow(title, key) {

    //合同性质
    var Nature = new Ext.data.Store({
        proxy: new Ext.data.HttpProxy({
            url: '/SysDirc/GetSysDicByType?Type=1',
            method: 'POST'
        }),
        reader: new Ext.data.JsonReader({},
            Ext.data.Record.create(["id", "text"]
        ))
    });
    //项目
    var Project = new Ext.data.Store({
        proxy: new Ext.data.HttpProxy({
            url: '/SysDirc/GetSysDicByType?Type=0',
            method: 'POST'
        }),
        reader: new Ext.data.JsonReader({},
            Ext.data.Record.create(["id", "text"]
        ))
    });
    //合同阶段
    var ProjectPhase = new Ext.data.Store({
        proxy: new Ext.data.HttpProxy({
            url: '/SysDirc/GetSysDicByType?Type=2',
            method: 'POST'
        }),
        reader: new Ext.data.JsonReader({},
            Ext.data.Record.create(["id", "text"]
        ))
    });
    //合同状态
    var ContractState = new Ext.data.Store({
        proxy: new Ext.data.HttpProxy({
            url: '/SysDirc/GetSysDicByType?Type=3',
            method: 'POST'
        }),
        reader: new Ext.data.JsonReader({},
            Ext.data.Record.create(["id", "text"]
        ))
    });
    //发票类型
    var InvoiceType = new Ext.data.Store({
        proxy: new Ext.data.HttpProxy({
            url: '/SysDirc/GetSysDicByType?Type=4',
            method: 'POST'
        }),
        reader: new Ext.data.JsonReader({},
            Ext.data.Record.create(["id", "text"]
        ))
    });


    if (title == "编辑") {
        var grid = Ext.getCmp("gg");
        //得到选后的数据   
        var rows = grid.getSelectionModel().getSelections();
        key = rows[0].get("Id");

        //合同性质
        if (rows[0].get("Nature") != "") {
            Nature.proxy = new Ext.data.HttpProxy({ url: '/SysDirc/GetSysDicByType?Type=1', method: 'POST' });
            Nature.load();
        }
        //项目
        if (rows[0].get("Project") != "") {
            Project.proxy = new Ext.data.HttpProxy({ url: '/SysDirc/GetSysDicByType?Type=0', method: 'POST' });
            Project.load();
        }
        //合同阶段
        if (rows[0].get("ProjectPhase") != "") {
            ProjectPhase.proxy = new Ext.data.HttpProxy({ url: '/SysDirc/GetSysDicByType?Type=2', method: 'POST' });
            ProjectPhase.load();
        }
        //合同状态
        if (rows[0].get("ContractState") != "") {
            ContractState.proxy = new Ext.data.HttpProxy({ url: '/SysDirc/GetSysDicByType?Type=3', method: 'POST' });
            ContractState.load();
        }
        //发票类型
        if (rows[0].get("InvoiceType") != "") {
            InvoiceType.proxy = new Ext.data.HttpProxy({ url: '/SysDirc/GetSysDicByType?Type=4', method: 'POST' });
            InvoiceType.load();
        }
    }

    var form = new top.Ext.form.FormPanel({
        layout: "form", // 整个大的表单是form布局
        id: 'formPanel',
        border: false,
        labelWidth: 75,
        autoScroll: true,
        bodyStyle: 'padding:15px',
        labelAlign: "right",
        items: [
        {
            xtype: "fieldset",
            title: "基本信息",
            items: [
               {
                   layout: "column", // 从上往下的布局
                   border: false,
                   items: [
                     {
                         columnWidth: .25, // 该列有整行中所占百分比
                         border: false,
                         layout: "form", // 从上往下的布局
                         items: [
                             { fieldLabel: '合同编号', xtype: 'textfield', id: 'NumCode', name: 'NumCode', maxLength: 50,
                                 emptyText: '请输入合同编号', maxLengthText: '合同编号长度不能超过50个字符', anchor: '90%'
                             }]
                     },
                       {
                           columnWidth: .25, // 该列有整行中所占百分比
                           border: false,
                           layout: "form", // 从上往下的布局
                           items: [
                                { fieldLabel: '原合同编号', xtype: 'textfield', id: 'OldNumCode', name: 'OldNumCode', maxLength: 50,
                                    maxLengthText: '原合同编号长度不能超过50个字符', anchor: '90%'
                                }]
                       },
                         {
                             columnWidth: .25, // 该列有整行中所占百分比
                             border: false,
                             layout: "form", // 从上往下的布局
                             items: [
                                 { fieldLabel: '<span class="required">*</span>合同名称', xtype: 'textfield', id: 'ContraceName', name: 'ContraceName', maxLength: 50,
                                     allowBlank: false, maxLengthText: '合同名称长度不能超过50个字符', anchor: '90%'
                                 }
                         ]
                         },

                           {
                               columnWidth: .25, // 该列有整行中所占百分比
                               border: false,
                               layout: "form", // 从上往下的布局
                               items: [
                                     {
                                         xtype: 'combo',
                                         triggerAction: 'all',
                                         id: 'NatureName',
                                         name: 'NatureName',
                                         fieldLabel: '<span class="required">*</span>合同性质',
                                         emptyText: '==选择==',
                                         forceSelection: true,
                                         editable: true,
                                         typeAhead: true, //模糊查询
                                         allowBlank: false,
                                         displayField: 'text',
                                         valueField: 'id',
                                         hiddenName: 'text',
                                         anchor: '90%',
                                         store: Nature,
                                         listeners: {
                                             beforequery: function (e) {
                                                 var combo = e.combo;
                                                 if (!e.forceAll) {
                                                     var input = e.query;
                                                     // 检索的正则  
                                                     var regExp = new RegExp(".*" + input + ".*");
                                                     // 执行检索  
                                                     combo.store.filterBy(function (record, id) {
                                                         // 得到每个record的项目名称值  
                                                         var text = record.get(combo.displayField);
                                                         return regExp.test(text);
                                                     });
                                                     combo.expand();
                                                     return false;
                                                 }
                                             }
                                         }
                                     }
                             ]
                           }
                     ]
               },
               {
                   layout: "column", // 从上往下的布局
                   border: false,
                   items: [
                     {
                         columnWidth: .25, // 该列有整行中所占百分比
                         border: false,
                         layout: "form", // 从上往下的布局
                         items: [
                            { fieldLabel: '发起人', xtype: 'textfield', id: 'InitiatorUser', name: 'InitiatorUser', maxLength: 50,
                                emptyText: '', maxLengthText: '发起人长度不能超过50个字符', anchor: '90%'
                            }]
                     },
                       {
                           columnWidth: .25, // 该列有整行中所占百分比
                           border: false,
                           layout: "form", // 从上往下的布局
                           items: [
                                { fieldLabel: '发起部门', xtype: 'textfield', id: 'InitiatorDept', name: 'InitiatorDept', maxLength: 50,
                                    emptyText: '', maxLengthText: '发起部门长度不能超过50个字符', anchor: '90%'
                                }]
                       },
                         {
                             columnWidth: .25, // 该列有整行中所占百分比
                             border: false,
                             layout: "form", // 从上往下的布局
                             items: [
                                   { fieldLabel: '经办人', xtype: 'textfield', id: 'Agent', name: 'Agent', maxLength: 50,
                                       emptyText: '', maxLengthText: '经办人长度不能超过50个字符', anchor: '90%'
                                   }
                         ]
                         },
                           {
                               columnWidth: .25, // 该列有整行中所占百分比
                               border: false,
                               layout: "form", // 从上往下的布局
                               items: [
                                   { fieldLabel: '审批人', xtype: 'textfield', id: 'Approver', name: 'Approver', maxLength: 50,
                                       emptyText: '', maxLengthText: '经办人长度不能超过50个字符', anchor: '90%'
                                   }
                             ]
                           }
                     ]
               },
               {
                   layout: "column", // 从上往下的布局
                   border: false,
                   items: [
                     {
                         columnWidth: .25, // 该列有整行中所占百分比
                         border: false,
                         layout: "form", // 从上往下的布局
                         items: [

                            {
                                xtype: 'combo',
                                triggerAction: 'all',
                                id: 'ProjectName',
                                name: 'ProjectName',
                                fieldLabel: '<span class="required">*</span>项目',
                                emptyText: '==选择==',
                                forceSelection: true,
                                editable: true,
                                typeAhead: true, //模糊查询
                                allowBlank: false,
                                displayField: 'text',
                                valueField: 'id',
                                hiddenName: 'text',
                                anchor: '90%',
                                store: Project,
                                listeners: {
                                    beforequery: function (e) {
                                        var combo = e.combo;
                                        if (!e.forceAll) {
                                            var input = e.query;
                                            // 检索的正则  
                                            var regExp = new RegExp(".*" + input + ".*");
                                            // 执行检索  
                                            combo.store.filterBy(function (record, id) {
                                                // 得到每个record的项目名称值  
                                                var text = record.get(combo.displayField);
                                                return regExp.test(text);
                                            });
                                            combo.expand();
                                            return false;
                                        }
                                    }
                                }
                            }]
                     },
                       {
                           columnWidth: .25, // 该列有整行中所占百分比
                           border: false,
                           layout: "form", // 从上往下的布局
                           items: [
                                { fieldLabel: '子系统名称', xtype: 'textfield', id: 'Subsystem', name: 'Subsystem', maxLength: 50,
                                    emptyText: '', maxLengthText: '子系统名称长度不能超过50个字符', anchor: '90%'
                                }]
                       },
                         {
                             columnWidth: .5, // 该列有整行中所占百分比
                             border: false,
                             layout: "form", // 从上往下的布局
                             items: [
                                 { fieldLabel: '产品说明', xtype: 'textfield', id: 'ProductDescription', name: 'ProductDescription', maxLength: 50,
                                     emptyText: '', maxLengthText: '产品说明名称长度不能超过50个字符', anchor: '95%'
                                 }
                         ]
                         }

                     ]
               },
               {
                   layout: "column", // 从上往下的布局
                   border: false,
                   items: [
                     {
                         columnWidth: .25, // 该列有整行中所占百分比
                         border: false,
                         layout: "form", // 从上往下的布局
                         items: [
                            { fieldLabel: '交付物及数量', xtype: 'textfield', id: 'DeliveriesQuantities', name: 'DeliveriesQuantities', maxLength: 50,
                                emptyText: '', maxLengthText: '交付物及数量长度不能超过50个字符', anchor: '90%'
                            }]
                     },
                       {
                           columnWidth: .25, // 该列有整行中所占百分比
                           border: false,
                           layout: "form", // 从上往下的布局
                           items: [
                                 { fieldLabel: '交付地点', xtype: 'textfield', id: 'DeliveriesAddress', name: 'DeliveriesAddress', maxLength: 50,
                                     emptyText: '', maxLengthText: '交付地点长度不能超过50个字符', anchor: '90%'
                                 }]
                       },
                         {
                             columnWidth: .25, // 该列有整行中所占百分比
                             border: false,
                             layout: "form", // 从上往下的布局
                             items: [
                                   { fieldLabel: '交付日期', xtype: 'datefield', id: 'DeliverDate', name: 'DeliverDate',
                                       emptyText: '选择交付日期', format: 'Y-m-d', anchor: '90%'
                                   }
                         ]
                         },
                           {
                               columnWidth: .25, // 该列有整行中所占百分比
                               border: false,
                               layout: "form", // 从上往下的布局
                               items: [

                                   {
                                       xtype: 'combo',
                                       triggerAction: 'all',
                                       id: 'ProjectPhaseName',
                                       name: 'ProjectPhaseName',
                                       fieldLabel: '<span class="required">*</span>项目阶段',
                                       emptyText: '==选择==',
                                       forceSelection: true,
                                       editable: true,
                                       typeAhead: true, //模糊查询
                                       allowBlank: false,
                                       displayField: 'text',
                                       valueField: 'id',
                                       hiddenName: 'text',
                                       anchor: '90%',
                                       store: ProjectPhase,
                                       listeners: {
                                           beforequery: function (e) {
                                               var combo = e.combo;
                                               if (!e.forceAll) {
                                                   var input = e.query;
                                                   // 检索的正则  
                                                   var regExp = new RegExp(".*" + input + ".*");
                                                   // 执行检索  
                                                   combo.store.filterBy(function (record, id) {
                                                       // 得到每个record的项目名称值  
                                                       var text = record.get(combo.displayField);
                                                       return regExp.test(text);
                                                   });
                                                   combo.expand();
                                                   return false;
                                               }
                                           }
                                       }
                                   }
                             ]
                           }
                     ]
               },
               {
                   layout: "column", // 从上往下的布局
                   border: false,
                   items: [
                     {
                         columnWidth: .25, // 该列有整行中所占百分比
                         border: false,
                         layout: "form", // 从上往下的布局
                         items: [

                            {
                                xtype: 'combo',
                                triggerAction: 'all',
                                id: 'ContractStateName',
                                name: 'ContractStateName',
                                fieldLabel: '<span class="required">*</span>合同状态',
                                emptyText: '==选择==',
                                forceSelection: true,
                                editable: true,
                                typeAhead: true, //模糊查询
                                allowBlank: false,
                                displayField: 'text',
                                valueField: 'id',
                                hiddenName: 'text',
                                anchor: '90%',
                                store: ContractState,
                                listeners: {
                                    beforequery: function (e) {
                                        var combo = e.combo;
                                        if (!e.forceAll) {
                                            var input = e.query;
                                            // 检索的正则  
                                            var regExp = new RegExp(".*" + input + ".*");
                                            // 执行检索  
                                            combo.store.filterBy(function (record, id) {
                                                // 得到每个record的项目名称值  
                                                var text = record.get(combo.displayField);
                                                return regExp.test(text);
                                            });
                                            combo.expand();
                                            return false;
                                        }
                                    }
                                }
                            }]
                     },
                       {
                           columnWidth: .25, // 该列有整行中所占百分比
                           border: false,
                           layout: "form", // 从上往下的布局
                           items: [
                                { fieldLabel: '签订日期', xtype: 'datefield', id: 'SigningDate', name: 'SigningDate',
                                    emptyText: '', emptyText: '选择签订日期', format: 'Y-m-d', anchor: '90%'
                                }]
                       },
                         {
                             columnWidth: .25, // 该列有整行中所占百分比
                             border: false,
                             layout: "form", // 从上往下的布局
                             items: [
                                  { fieldLabel: '有效期', xtype: 'textfield', id: 'ValidityDate', name: 'ValidityDate', maxLength: 50,
                                      emptyText: '', maxLengthText: '有效期长度不能超过50个字符', anchor: '90%'
                                  }
                         ]
                         },
                           {
                               columnWidth: .25, // 该列有整行中所占百分比
                               border: false,
                               layout: "form", // 从上往下的布局
                               items: [
                                   { fieldLabel: '合同执行情况', xtype: 'textfield', id: 'ContractIimplementation', name: 'ContractIimplementation', maxLength: 50,
                                       emptyText: '', maxLengthText: '合同执行情况长度不能超过50个字符', anchor: '90%'
                                   }
                             ]
                           }
                     ]
               },
               {
                   layout: "column", // 从上往下的布局
                   border: false,
                   items: [
                     {
                         columnWidth: .25, // 该列有整行中所占百分比
                         border: false,
                         layout: "form", // 从上往下的布局
                         items: [
                             { fieldLabel: '合同金额币种', xtype: 'textfield', id: 'Currency', name: 'Currency', maxLength: 50,
                                 emptyText: '', maxLengthText: '合同金额币种长度不能超过50个字符', anchor: '90%'
                             }]
                     },
                       {
                           columnWidth: .25, // 该列有整行中所占百分比
                           border: false,
                           layout: "form", // 从上往下的布局
                           items: [
                               { fieldLabel: '金额单位', xtype: 'textfield', id: 'CurrencyUnit', name: 'CurrencyUnit', maxLength: 50,
                                   emptyText: '', maxLengthText: '金额单位长度不能超过50个字符', anchor: '90%'
                               }]
                       },
                         {
                             columnWidth: .25, // 该列有整行中所占百分比
                             border: false,
                             layout: "form", // 从上往下的布局
                             items: [
                                 { fieldLabel: '发票税率', xtype: 'textfield', id: 'TaxRate', name: 'TaxRate', maxLength: 50,
                                     emptyText: '', maxLengthText: '发票税率长度不能超过50个字符', anchor: '90%'
                                 }
                         ]
                         },
                           {
                               columnWidth: .25, // 该列有整行中所占百分比
                               border: false,
                               layout: "form", // 从上往下的布局
                               items: [

                               {
                                   xtype: 'combo',
                                   triggerAction: 'all',
                                   id: 'InvoiceTypeName',
                                   name: 'InvoiceTypeName',
                                   fieldLabel: '<span class="required">*</span>发票类型',
                                   emptyText: '==选择==',
                                   forceSelection: true,
                                   editable: true,
                                   typeAhead: true, //模糊查询
                                   allowBlank: false,
                                   displayField: 'text',
                                   valueField: 'id',
                                   hiddenName: 'text',
                                   anchor: '90%',
                                   store: InvoiceType,
                                   listeners: {
                                       beforequery: function (e) {
                                           var combo = e.combo;
                                           if (!e.forceAll) {
                                               var input = e.query;
                                               // 检索的正则  
                                               var regExp = new RegExp(".*" + input + ".*");
                                               // 执行检索  
                                               combo.store.filterBy(function (record, id) {
                                                   // 得到每个record的项目名称值  
                                                   var text = record.get(combo.displayField);
                                                   return regExp.test(text);
                                               });
                                               combo.expand();
                                               return false;
                                           }
                                       }
                                   }
                               }

                             ]
                           }
                     ]
               },
               {
                   layout: "column", // 从上往下的布局
                   border: false,
                   items: [
                     {
                         columnWidth: .25, // 该列有整行中所占百分比
                         border: false,
                         layout: "form", // 从上往下的布局
                         items: [
                            { fieldLabel: '已开票总金额', xtype: 'numberfield', id: 'InvoiceValueHas', name: 'InvoiceValueHas', maxLength: 50,
                                emptyText: '', anchor: '90%', minValue: 0, maxValue: 999999999, readOnly: true
                            }]
                     },
                       {
                           columnWidth: .25, // 该列有整行中所占百分比
                           border: false,
                           layout: "form", // 从上往下的布局
                           items: [
                                { fieldLabel: '待开发票金额', xtype: 'textfield', id: 'InvoiceValueBefore', name: 'InvoiceValueBefore', maxLength: 50,
                                    emptyText: '', anchor: '90%'
                                }]
                       },
                         {
                             columnWidth: .25, // 该列有整行中所占百分比
                             border: false,
                             layout: "form", // 从上往下的布局
                             items: [
                              { fieldLabel: '应开发票金额', xtype: 'numberfield', id: 'InvoiceValueBe', name: 'InvoiceValueBe', maxLength: 50,
                                  emptyText: '', anchor: '90%', minValue: 0, maxValue: 999999999
                              }
                         ]
                         },
                           {
                               columnWidth: .25, // 该列有整行中所占百分比
                               border: false,
                               layout: "form", // 从上往下的布局
                               items: [
                                 { fieldLabel: '预算情况', xtype: 'textfield', id: 'BudgetSituation', name: 'BudgetSituation', maxLength: 50,
                                     emptyText: '', maxLengthText: '联系方式长度不能超过50个字符', anchor: '90%'
                                 }
                             ]
                           }
                     ]
               },
               {
                   layout: "column", // 从上往下的布局
                   border: false,
                   items: [
                     {
                         columnWidth: .25, // 该列有整行中所占百分比
                         border: false,
                         layout: "form", // 从上往下的布局
                         items: [
//                             { fieldLabel: '计划收款总额', xtype: 'numberfield', id: 'PlanTotalAmount', name: 'PlanTotalAmount', maxLength: 50,
//                                 emptyText: '', anchor: '90%', minValue: 0, maxValue: 999999999
//                             }
                             ]
                     },
                       {
                           columnWidth: .25, // 该列有整行中所占百分比
                           border: false,
                           layout: "form", // 从上往下的布局
                           items: [
                               { fieldLabel: '合同总金额', xtype: 'numberfield', id: 'TotalContractAmount', name: 'TotalContractAmount', maxLength: 50,
                                   emptyText: '', anchor: '90%', minValue: 0, maxValue: 999999999, enableKeyEvents: true, listeners: {
                                       'keyup': function (e) {
                                           var TotalContractAmount = top.Ext.getCmp("TotalContractAmount").getValue();
                                           var ReceiptsTotalAmount = top.Ext.getCmp("ReceiptsTotalAmount").getValue();
                                           top.Ext.getCmp("ReceivablesAmount").setValue(TotalContractAmount - ReceiptsTotalAmount);
                                       }
                                   }
                               }]
                       },
                         {
                             columnWidth: .25, // 该列有整行中所占百分比
                             border: false,
                             layout: "form", // 从上往下的布局
                             items: [
                                  { fieldLabel: '财务收款总额', xtype: 'textfield', id: 'ReceiptsTotalAmount', name: 'ReceiptsTotalAmount', maxLength: 50,
                                      emptyText: '', anchor: '90%', readOnly: true
                                  }
                            ]
                         },
                           {
                               columnWidth: .25, // 该列有整行中所占百分比
                               border: false,
                               layout: "form", // 从上往下的布局
                               items: [
                                    { fieldLabel: '待收款金额', xtype: 'numberfield', id: 'ReceivablesAmount', name: 'ReceivablesAmount', maxLength: 50,
                                        emptyText: '', anchor: '90%', minValue: 0, maxValue: 999999999, readOnly: true
                                    }
                             ]
                           }
                     ]
               },
               {
                   layout: "column", // 从上往下的布局
                   border: false,
                   items: [
                     {
                         columnWidth: .25, // 该列有整行中所占百分比
                         border: false,
                         layout: "form", // 从上往下的布局
                         items: [
                            { fieldLabel: '协作单位名称', xtype: 'textfield', id: 'UnitName', name: 'UnitName', maxLength: 50,
                                emptyText: '', maxLengthText: '协作单位名称长度不能超过50个字符', anchor: '90%'
                            }]
                     },
                       {
                           columnWidth: .25, // 该列有整行中所占百分比
                           border: false,
                           layout: "form", // 从上往下的布局
                           items: [
                                { fieldLabel: '协作单位地址', xtype: 'textfield', id: 'UnitAddress', name: 'UnitAddress', maxLength: 50,
                                    emptyText: '', maxLengthText: '协作单位地址长度不能超过50个字符', anchor: '90%'
                                }]
                       },
                         {
                             columnWidth: .25, // 该列有整行中所占百分比
                             border: false,
                             layout: "form", // 从上往下的布局
                             items: [
                               { fieldLabel: '联系人', xtype: 'textfield', id: 'LinkUser', name: 'LinkUser', maxLength: 50,
                                   emptyText: '', maxLengthText: '联系人长度不能超过50个字符', anchor: '90%'
                               }
                         ]
                         },
                           {
                               columnWidth: .25, // 该列有整行中所占百分比
                               border: false,
                               layout: "form", // 从上往下的布局
                               items: [
                                     { fieldLabel: '联系方式', xtype: 'textfield', id: 'LinType', name: 'LinType', maxLength: 50,
                                         emptyText: '', maxLengthText: '联系方式长度不能超过50个字符', anchor: '90%'
                                     }
                             ]
                           }
                     ]
               },
               {
                   layout: "column", // 从上往下的布局
                   border: false,
                   items: [
                     {
                         columnWidth: .5, // 该列有整行中所占百分比
                         border: false,
                         layout: "form", // 从上往下的布局
                         items: [
                            { fieldLabel: '单位开户行', xtype: 'textfield', id: 'OpeningBank', name: 'OpeningBank', maxLength: 50,
                                emptyText: '', emptyText: '协作单位开户行', maxLengthText: '协作单位开户行长度不能超过50个字符', anchor: '95%'
                            }]
                     },
                       {
                           columnWidth: .5, // 该列有整行中所占百分比
                           border: false,
                           layout: "form", // 从上往下的布局
                           items: [
                                { fieldLabel: '开户行账号', xtype: 'textfield', id: 'OpeningAccount', name: 'OpeningAccount', maxLength: 50,
                                    emptyText: '', emptyText: '协作单位开户行账号', maxLengthText: '协作单位开户行账号长度不能超过50个字符', anchor: '95%'
                                }]
                       }
                     ]
               },
               {
                   layout: "column", // 从上往下的布局
                   border: false,
                   items: [
                     {
                         columnWidth: .5, // 该列有整行中所占百分比
                         border: false,
                         layout: "form", // 从上往下的布局
                         items: [
                            { fieldLabel: '依据文件', xtype: 'textfield', id: 'AccordingDocument', name: 'AccordingDocument', maxLength: 50,
                                emptyText: '', maxLengthText: '依据文件长度不能超过50个字符', anchor: '90%'
                            }]
                     },
                       {
                           columnWidth: .5, // 该列有整行中所占百分比
                           border: false,
                           layout: "form", // 从上往下的布局
                           items: [
                                { fieldLabel: '归档情况', xtype: 'textfield', id: 'FilingSituation', name: 'FilingSituation', maxLength: 50,
                                    emptyText: '', maxLengthText: '归档情况长度不能超过50个字符', anchor: '90%'
                                }]
                       }
//                         {
//                             columnWidth: .5, // 该列有整行中所占百分比
//                             border: false,
//                             layout: "form", // 从上往下的布局
//                             items: [
//                             { fieldLabel: '备注', xtype: 'textfield', id: 'Remark', name: 'Remark', maxLength: 50,
//                                 emptyText: '', maxLengthText: '备注长度不能超过50个字符', anchor: '95%'
//                             }
//                         ]
//                         }
                     ]
               },
               {
                   layout: "column", // 从上往下的布局
                   border: false,
                   items: [
                     {
                         columnWidth: .25, // 该列有整行中所占百分比
                         border: false,
                         layout: "form", // 从上往下的布局
                         items: [
                             { fieldLabel: '扩展字段1', xtype: 'textfield', id: 'cdefine1', name: 'cdefine1', maxLength: 50,
                                 emptyText: '', maxLengthText: '扩展字段1长度不能超过50个字符', anchor: '90%'
                             }]
                     },
                       {
                           columnWidth: .25, // 该列有整行中所占百分比
                           border: false,
                           layout: "form", // 从上往下的布局
                           items: [
                               { fieldLabel: '扩展字段2', xtype: 'textfield', id: 'cdefine2', name: 'cdefine2', maxLength: 50,
                                   emptyText: '', maxLengthText: '扩展字段2长度不能超过50个字符', anchor: '90%'
                               }]
                       },
                         {
                             columnWidth: .25, // 该列有整行中所占百分比
                             border: false,
                             layout: "form", // 从上往下的布局
                             items: [
                               { fieldLabel: '扩展字段3', xtype: 'textfield', id: 'cdefine3', name: 'cdefine3', maxLength: 50,
                                   emptyText: '', maxLengthText: '扩展字段3长度不能超过50个字符', anchor: '90%'
                               }
                         ]
                         },
                           {
                               columnWidth: .25, // 该列有整行中所占百分比
                               border: false,
                               layout: "form", // 从上往下的布局
                               items: [
                                     { fieldLabel: '扩展字段4', xtype: 'textfield', id: 'cdefine4', name: 'cdefine4', maxLength: 50,
                                         emptyText: '', maxLengthText: '扩展字段4长度不能超过50个字符', anchor: '90%'
                                     }
                             ]
                           }
                     ]
               },
               {
                   layout: "column", // 从上往下的布局
                   border: false,
                   items: [
                     {
                         columnWidth: .25, // 该列有整行中所占百分比
                         border: false,
                         layout: "form", // 从上往下的布局
                         items: [
                             { fieldLabel: '扩展字段5', xtype: 'textfield', id: 'cdefine5', name: 'cdefine5', maxLength: 50,
                                 emptyText: '', maxLengthText: '扩展字段1长度不能超过50个字符', anchor: '90%'
                             }]
                     },
                       {
                           columnWidth: .25, // 该列有整行中所占百分比
                           border: false,
                           layout: "form", // 从上往下的布局
                           items: [
                               { fieldLabel: '扩展字段6', xtype: 'textfield', id: 'cdefine6', name: 'cdefine6', maxLength: 50,
                                   emptyText: '', maxLengthText: '扩展字段6长度不能超过50个字符', anchor: '90%'
                               }]
                       },
                         {
                             columnWidth: .25, // 该列有整行中所占百分比
                             border: false,
                             layout: "form", // 从上往下的布局
                             items: [
                               { fieldLabel: '扩展字段7', xtype: 'textfield', id: 'cdefine7', name: 'cdefine7', maxLength: 50,
                                   emptyText: '', maxLengthText: '扩展字段7长度不能超过50个字符', anchor: '90%'
                               }
                         ]
                         },
                           {
                               columnWidth: .25, // 该列有整行中所占百分比
                               border: false,
                               layout: "form", // 从上往下的布局
                               items: [
                                     { fieldLabel: '扩展字段8', xtype: 'textfield', id: 'cdefine8', name: 'cdefine8', maxLength: 50,
                                         emptyText: '', maxLengthText: '扩展字段8长度不能超过50个字符', anchor: '90%'
                                     }
                             ]
                           }
                     ]
               },
               {
                   layout: "column", // 从上往下的布局
                   border: false,
                   items: [
                     {
                         columnWidth: .25, // 该列有整行中所占百分比
                         border: false,
                         layout: "form", // 从上往下的布局
                         items: [
                             { fieldLabel: '扩展字段9', xtype: 'textfield', id: 'cdefine9', name: 'cdefine9', maxLength: 50,
                                 emptyText: '', maxLengthText: '扩展字段1长度不能超过50个字符', anchor: '90%'
                             }]
                     },
                       {
                           columnWidth: .25, // 该列有整行中所占百分比
                           border: false,
                           layout: "form", // 从上往下的布局
                           items: [
                                { fieldLabel: '扩展字段10', xtype: 'textfield', id: 'cdefine10', name: 'cdefine10', maxLength: 50,
                                    emptyText: '', maxLengthText: '扩展字段10长度不能超过50个字符', anchor: '90%'
                                }]
                       }
                     ]
               },
               {
                   layout: "column", // 从上往下的布局
                   xtype: "fieldset",
                   title: "收款节点",
                   collapsible: true,
                   items: [
                     {
                         columnWidth: .3, // 该列有整行中所占百分比
                         border: false,
                         layout: "form", // 从上往下的布局
                         items: [
                             {
                                 xtype: 'fieldset',
                                 title: '约定收款',
                                 anchor: '100%',
                                 collapsible: false,
                                 border: false,
                                 items: [
                                        {
                                            xtype: 'compositefield',
                                            fieldLabel: '节点1',
                                            id: 'receivablesDate1',
                                            msgTarget: 'under',
                                            items: [
                                                   { fieldLabel: '收款日期', xtype: 'datefield', id: 'AgreedTime1', name: 'AgreedTime1',
                                                       emptyText: '', format: 'Y-m-d', anchor: '100%', emptyText: '收款时间'
                                                   },
                                                    { fieldLabel: '收款金额', xtype: 'numberfield', id: 'AgreedMoney1', name: 'AgreedMoney1', maxLength: 50,
                                                        emptyText: '收款金额', width: 90, minValue: 0, maxValue: 999999999
                                                    }
                                            ]
                                        },
                                        {
                                            xtype: 'compositefield',
                                            fieldLabel: '节点2',
                                            id: 'receivablesDate2',
                                            msgTarget: 'under',
                                            items: [
                                                   { fieldLabel: '收款日期', xtype: 'datefield', id: 'AgreedTime2', name: 'AgreedTime2',
                                                       emptyText: '', format: 'Y-m-d', anchor: '100%', emptyText: '收款时间'
                                                   },
                                                    { fieldLabel: '收款金额', xtype: 'numberfield', id: 'AgreedMoney2', name: 'AgreedMoney2', maxLength: 50,
                                                        emptyText: '', width: 90, minValue: 0, maxValue: 999999999
                                                    }
                                            ]
                                        },
                                        {
                                            xtype: 'compositefield',
                                            fieldLabel: '节点3',
                                            id: 'receivablesDate3',
                                            msgTarget: 'under',
                                            items: [
                                                   { fieldLabel: '收款日期', xtype: 'datefield', id: 'AgreedTime3', name: 'AgreedTime3',
                                                       emptyText: '', format: 'Y-m-d', anchor: '100%', emptyText: '收款时间'
                                                   },
                                                    { fieldLabel: '收款金额', xtype: 'numberfield', id: 'AgreedMoney3', name: 'AgreedMoney3', maxLength: 50,
                                                        emptyText: '', width: 90, minValue: 0, maxValue: 999999999
                                                    }
                                            ]
                                        },
                                        {
                                            xtype: 'compositefield',
                                            fieldLabel: '节点4',
                                            id: 'receivablesDate4',
                                            msgTarget: 'under',
                                            items: [
                                                   { fieldLabel: '收款日期', xtype: 'datefield', id: 'AgreedTime4', name: 'AgreedTime4',
                                                       emptyText: '', format: 'Y-m-d', anchor: '100%', emptyText: '收款时间'
                                                   },
                                                    { fieldLabel: '收款金额', xtype: 'numberfield', id: 'AgreedMoney4', name: 'AgreedMoney4', maxLength: 50,
                                                        emptyText: '', width: 90, minValue: 0, maxValue: 999999999
                                                    }
                                            ]
                                        },
                                        {
                                            xtype: 'compositefield',
                                            fieldLabel: '节点5',
                                            id: 'receivablesDate5',
                                            msgTarget: 'under',
                                            items: [
                                                   { fieldLabel: '收款日期', xtype: 'datefield', id: 'AgreedTime5', name: 'AgreedTime5',
                                                       emptyText: '', format: 'Y-m-d', anchor: '100%', emptyText: '收款时间'
                                                   },
                                                    { fieldLabel: '收款金额', xtype: 'numberfield', id: 'AgreedMoney5', name: 'AgreedMoney5', maxLength: 50,
                                                        emptyText: '', width: 90, minValue: 0, maxValue: 999999999
                                                    }
                                            ]
                                        }
                                     ]
                             }]
                     },
                       {
                           columnWidth: .3, // 该列有整行中所占百分比
                           border: false,
                           layout: "form", // 从上往下的布局
                           items: [
                               {
                                   xtype: 'fieldset',
                                   title: '财务实收',
                                   collapsible: false,
                                   border: false,
                                   items: [
                                        {
                                            xtype: 'compositefield',
                                            fieldLabel: '节点1',
                                            id: 'receivablesPlan1',
                                            msgTarget: 'under',
                                            items: [
                                                   { fieldLabel: '收款日期', xtype: 'datefield', id: 'FinancialTime1', name: 'FinancialTime1',
                                                       emptyText: '', format: 'Y-m-d', anchor: '100%', emptyText: '收款时间'
                                                   },
                                                    { fieldLabel: '收款金额', xtype: 'numberfield', id: 'FinancialAmount1', name: 'FinancialAmount1', maxLength: 50,
                                                        emptyText: '', width: 90, minValue: 0, maxValue: 999999999, enableKeyEvents: true, listeners: {
                                                            'keyup': function (e) {
                                                            TotalReceiptsTotalAmount();
                                                            }
                                                        }
                                                    }
                                            ]
                                        },
                                        {
                                            xtype: 'compositefield',
                                            fieldLabel: '节点2',
                                            id: 'receivablesPlan2',
                                            msgTarget: 'under',
                                            items: [
                                                   { fieldLabel: '收款日期', xtype: 'datefield', id: 'FinancialTime2', name: 'FinancialTime2',
                                                       emptyText: '', format: 'Y-m-d', anchor: '100%', emptyText: '收款时间'
                                                   },
                                                    { fieldLabel: '收款金额', xtype: 'numberfield', id: 'FinancialAmount2', name: 'FinancialAmount2', maxLength: 50,
                                                        emptyText: '', width: 90, minValue: 0, maxValue: 999999999, enableKeyEvents: true, listeners: {
                                                            'keyup': function (e) {
                                                          TotalReceiptsTotalAmount();
                                                            }
                                                        }
                                                    }
                                            ]
                                        },
                                        {
                                            xtype: 'compositefield',
                                            fieldLabel: '节点3',
                                            id: 'receivablesPlan3',
                                            msgTarget: 'under',
                                            items: [
                                                   { fieldLabel: '收款日期', xtype: 'datefield', id: 'FinancialTime3', name: 'FinancialTime3',
                                                       emptyText: '', format: 'Y-m-d', anchor: '100%', emptyText: '收款时间'
                                                   },
                                                    { fieldLabel: '收款金额', xtype: 'numberfield', id: 'FinancialAmount3', name: 'FinancialAmount3', maxLength: 50,
                                                        emptyText: '', width: 90, minValue: 0, maxValue: 999999999, enableKeyEvents: true, listeners: {
                                                            'keyup': function (e) {
                                                            TotalReceiptsTotalAmount();
                                                            }
                                                        }
                                                    }
                                            ]
                                        },
                                        {
                                            xtype: 'compositefield',
                                            fieldLabel: '节点4',
                                            id: 'receivablesPlan4',
                                            msgTarget: 'under',
                                            items: [
                                                   { fieldLabel: '收款日期', xtype: 'datefield', id: 'FinancialTime4', name: 'FinancialTime4',
                                                       emptyText: '', format: 'Y-m-d', anchor: '100%', emptyText: '收款时间'
                                                   },
                                                    { fieldLabel: '收款金额', xtype: 'numberfield', id: 'FinancialAmount4', name: 'FinancialAmount4', maxLength: 50,
                                                        emptyText: '', width: 90, minValue: 0, maxValue: 999999999, enableKeyEvents: true, listeners: {
                                                            'keyup': function (e) {
                                                            TotalReceiptsTotalAmount();
                                                            }
                                                        }
                                                    }
                                            ]
                                        },
                                        {
                                            xtype: 'compositefield',
                                            fieldLabel: '节点5',
                                            id: 'receivablesPlan5',
                                            msgTarget: 'under',
                                            items: [
                                                   { fieldLabel: '收款日期', xtype: 'datefield', id: 'FinancialTime5', name: 'FinancialTime5',
                                                       emptyText: '', format: 'Y-m-d', anchor: '100%', emptyText: '收款时间'
                                                   },
                                                    { fieldLabel: '收款金额', xtype: 'numberfield', id: 'FinancialAmount5', name: 'FinancialAmount5', maxLength: 50,
                                                        emptyText: '', width: 90, minValue: 0, maxValue: 999999999, enableKeyEvents: true, listeners: {
                                                            'keyup': function (e) {
                                                            TotalReceiptsTotalAmount();
                                                            }
                                                        }
                                                    }
                                            ]
                                        }
                                     ]
                               }]
                       },
                       {
                           columnWidth: .3, // 该列有整行中所占百分比
                           border: false,
                           layout: "form", // 从上往下的布局
                           items: [
                               {
                                   xtype: 'fieldset',
                                   title: '开票节点',
                                   collapsible: false,
                                   border: false,
                                   items: [
                                        {
                                            xtype: 'compositefield',
                                            fieldLabel: '节点1',
                                            id: 'receivablesPlan21',
                                            msgTarget: 'under',
                                            items: [
                                                   { fieldLabel: '收款日期', xtype: 'datefield', id: 'TicketTime1', name: 'TicketTime1',
                                                       emptyText: '', format: 'Y-m-d', anchor: '100%', emptyText: '开票时间'
                                                   },
                                                    { fieldLabel: '收款金额', xtype: 'numberfield', id: 'TicketMoney1', name: 'TicketMoney1', maxLength: 50,
                                                        emptyText: '', width: 90, minValue: 0, maxValue: 999999999, enableKeyEvents: true, listeners: {
                                                            'keyup': function (e) {
                                                                TotalInvoiceValueHas();
                                                            }
                                                        }
                                                    }
                                            ]
                                        },
                                        {
                                            xtype: 'compositefield',
                                            fieldLabel: '节点2',
                                            id: 'receivablesPlan22',
                                            msgTarget: 'under',
                                            items: [
                                                   { fieldLabel: '收款日期', xtype: 'datefield', id: 'TicketTime2', name: 'TicketTime2',
                                                       emptyText: '', format: 'Y-m-d', anchor: '100%', emptyText: '开票时间'
                                                   },
                                                    { fieldLabel: '收款金额', xtype: 'numberfield', id: 'TicketMoney2', name: 'TicketMoney2', maxLength: 50,
                                                        emptyText: '', width: 90, minValue: 0, maxValue: 999999999, enableKeyEvents: true, listeners: {
                                                            'keyup': function (e) {
                                                                TotalInvoiceValueHas();
                                                            }
                                                        }
                                                    }
                                            ]
                                        },
                                        {
                                            xtype: 'compositefield',
                                            fieldLabel: '节点3',
                                            id: 'receivablesPlan23',
                                            msgTarget: 'under',
                                            items: [
                                                   { fieldLabel: '收款日期', xtype: 'datefield', id: 'TicketTime3', name: 'TicketTime3',
                                                       emptyText: '', format: 'Y-m-d', anchor: '100%', emptyText: '开票时间'
                                                   },
                                                    { fieldLabel: '收款金额', xtype: 'numberfield', id: 'TicketMoney3', name: 'TicketMoney3', maxLength: 50,
                                                        emptyText: '', width: 90, minValue: 0, maxValue: 999999999, enableKeyEvents: true, listeners: {
                                                            'keyup': function (e) {
                                                                TotalInvoiceValueHas();
                                                            }
                                                        }
                                                    }
                                            ]
                                        },
                                        {
                                            xtype: 'compositefield',
                                            fieldLabel: '节点4',
                                            id: 'receivablesPlan24',
                                            msgTarget: 'under',
                                            items: [
                                                   { fieldLabel: '收款日期', xtype: 'datefield', id: 'TicketTime4', name: 'TicketTime4',
                                                       emptyText: '', format: 'Y-m-d', anchor: '100%', emptyText: '开票时间'
                                                   },
                                                    { fieldLabel: '收款金额', xtype: 'numberfield', id: 'TicketMoney4', name: 'TicketMoney4', maxLength: 50,
                                                        emptyText: '', width: 90, minValue: 0, maxValue: 999999999, enableKeyEvents: true, listeners: {
                                                            'keyup': function (e) {
                                                                TotalInvoiceValueHas();
                                                            }
                                                        }
                                                    }
                                            ]
                                        },
                                        {
                                            xtype: 'compositefield',
                                            fieldLabel: '节点5',
                                            id: 'receivablesPla2n5',
                                            msgTarget: 'under',
                                            items: [
                                                   { fieldLabel: '收款日期', xtype: 'datefield', id: 'TicketTime5', name: 'TicketTime5',
                                                       emptyText: '', format: 'Y-m-d', anchor: '100%', emptyText: '开票时间'
                                                   },
                                                    { fieldLabel: '收款金额', xtype: 'numberfield', id: 'TicketMoney5', name: 'TicketMoney5', maxLength: 50,
                                                        emptyText: '', width: 90, minValue: 0, maxValue: 999999999, decimalPrecision: 2,
                                                        allowDecimals: true, emptyText: '收款金额', enableKeyEvents: true, listeners: {
                                                            'keyup': function (e) {
                                                                TotalInvoiceValueHas();
                                                            }
                                                        }
                                                    }
                                            ]
                                        }
                                     ]
                               }]
                       }

                     ]
               }
               ]
        }
          ]
    });

    var win = Window("window", title, form);
    win.width = 1080;
    win.height =750;
    return win;
}

//创建查询面板
function CreateSearchWindow(title, key) {

    //合同性质
    var Nature = new Ext.data.Store({
        proxy: new Ext.data.HttpProxy({
            url: '/SysDirc/GetSysDicByType?Type=1',
            method: 'POST'
        }),
        reader: new Ext.data.JsonReader({},
            Ext.data.Record.create(["id", "text"]
        ))
    });
    //项目
    var Project = new Ext.data.Store({
        proxy: new Ext.data.HttpProxy({
            url: '/SysDirc/GetSysDicByType?Type=0',
            method: 'POST'
        }),
        reader: new Ext.data.JsonReader({},
            Ext.data.Record.create(["id", "text"]
        ))
    });
    //合同阶段
    var ProjectPhase = new Ext.data.Store({
        proxy: new Ext.data.HttpProxy({
            url: '/SysDirc/GetSysDicByType?Type=2',
            method: 'POST'
        }),
        reader: new Ext.data.JsonReader({},
            Ext.data.Record.create(["id", "text"]
        ))
    });
    //合同状态
    var ContractState = new Ext.data.Store({
        proxy: new Ext.data.HttpProxy({
            url: '/SysDirc/GetSysDicByType?Type=3',
            method: 'POST'
        }),
        reader: new Ext.data.JsonReader({},
            Ext.data.Record.create(["id", "text"]
        ))
    });
    //发票类型
    var InvoiceType = new Ext.data.Store({
        proxy: new Ext.data.HttpProxy({
            url: '/SysDirc/GetSysDicByType?Type=4',
            method: 'POST'
        }),
        reader: new Ext.data.JsonReader({},
            Ext.data.Record.create(["id", "text"]
        ))
    });

    var form = new top.Ext.form.FormPanel({
        layout: "form", // 整个大的表单是form布局
        id: 'SearchPanel',
        border: false,
        labelWidth: 75,
        autoScroll: true,
        bodyStyle: 'padding:15px',
        labelAlign: "right",
        items: [
        {
            xtype: "fieldset",
            title: "基本信息",
            items: [
               {
                   layout: "column", // 从上往下的布局
                   border: false,
                   items: [
                     {
                         columnWidth: .5, // 该列有整行中所占百分比
                         border: false,
                         layout: "form", // 从上往下的布局
                         items: [
                         { fieldLabel: '合同名称', xtype: 'textfield', id: 'ContraceName', name: 'ContraceName', maxLength: 50,
                             maxLengthText: '合同名称长度不能超过50个字符', anchor: '90%'
                         }
                             ]
                     },
                       {
                           columnWidth: .5, // 该列有整行中所占百分比
                           border: false,
                           layout: "form", // 从上往下的布局
                           items: [{
                               xtype: 'combo',
                               triggerAction: 'all',
                               id: 'NatureName',
                               name: 'NatureName',
                               fieldLabel: '合同性质',
                               emptyText: '==选择==',
                               forceSelection: true,
                               editable: true,
                               typeAhead: true, //模糊查询
                               displayField: 'text',
                               valueField: 'id',
                               hiddenName: 'text',
                               anchor: '90%',
                               store: Nature,
                               listeners: {
                                   beforequery: function (e) {
                                       var combo = e.combo;
                                       if (!e.forceAll) {
                                           var input = e.query;
                                           // 检索的正则  
                                           var regExp = new RegExp(".*" + input + ".*");
                                           // 执行检索  
                                           combo.store.filterBy(function (record, id) {
                                               // 得到每个record的项目名称值  
                                               var text = record.get(combo.displayField);
                                               return regExp.test(text);
                                           });
                                           combo.expand();
                                           return false;
                                       }
                                   }
                               }
                           }
                                ]
                       }
                     ]
               },
               {
                   layout: "column", // 从上往下的布局
                   border: false,
                   items: [
                         {
                             columnWidth: .5, // 该列有整行中所占百分比
                             border: false,
                             layout: "form", // 从上往下的布局
                             items: [
                               { fieldLabel: '发起人', xtype: 'textfield', id: 'InitiatorUser', name: 'InitiatorUser', maxLength: 50,
                                   emptyText: '', maxLengthText: '发起人长度不能超过50个字符', anchor: '90%'
                               }
                         ]
                         },

                           {
                               columnWidth: .5, // 该列有整行中所占百分比
                               border: false,
                               layout: "form", // 从上往下的布局
                               items: [
                                   { fieldLabel: '发起部门', xtype: 'textfield', id: 'InitiatorDept', name: 'InitiatorDept', maxLength: 50,
                                       emptyText: '', maxLengthText: '发起部门长度不能超过50个字符', anchor: '90%'
                                   }
                             ]
                           }
                     ]
               },
               {
                   layout: "column", // 从上往下的布局
                   border: false,
                   items: [
                     {
                         columnWidth: .5, // 该列有整行中所占百分比
                         border: false,
                         layout: "form", // 从上往下的布局
                         items: [{ fieldLabel: '经办人', xtype: 'textfield', id: 'Agent', name: 'Agent', maxLength: 50,
                             emptyText: '', maxLengthText: '经办人长度不能超过50个字符', anchor: '90%'
                         }
                            ]
                     },
                       {
                           columnWidth: .5, // 该列有整行中所占百分比
                           border: false,
                           layout: "form", // 从上往下的布局
                           items: [{ fieldLabel: '审批人', xtype: 'textfield', id: 'Approver', name: 'Approver', maxLength: 50,
                               emptyText: '', maxLengthText: '经办人长度不能超过50个字符', anchor: '90%'
                           }
                               ]
                       }
                     ]
               },
               {
                   layout: "column", // 从上往下的布局
                   border: false,
                   items: [
                         {
                             columnWidth: .5, // 该列有整行中所占百分比
                             border: false,
                             layout: "form", // 从上往下的布局
                             items: [
                                  {
                                      xtype: 'combo',
                                      triggerAction: 'all',
                                      id: 'ProjectName',
                                      name: 'ProjectName',
                                      fieldLabel: '项目',
                                      emptyText: '==选择==',
                                      forceSelection: true,
                                      editable: true,
                                      typeAhead: true, //模糊查询
                                      displayField: 'text',
                                      valueField: 'id',
                                      hiddenName: 'text',
                                      anchor: '90%',
                                      store: Project,
                                      listeners: {
                                          beforequery: function (e) {
                                              var combo = e.combo;
                                              if (!e.forceAll) {
                                                  var input = e.query;
                                                  // 检索的正则  
                                                  var regExp = new RegExp(".*" + input + ".*");
                                                  // 执行检索  
                                                  combo.store.filterBy(function (record, id) {
                                                      // 得到每个record的项目名称值  
                                                      var text = record.get(combo.displayField);
                                                      return regExp.test(text);
                                                  });
                                                  combo.expand();
                                                  return false;
                                              }
                                          }
                                      }
                                  }
                         ]
                         },
                           {
                               columnWidth: .5, // 该列有整行中所占百分比
                               border: false,
                               layout: "form", // 从上往下的布局
                               items: [
                                      { fieldLabel: '子系统名称', xtype: 'textfield', id: 'Subsystem', name: 'Subsystem', maxLength: 50,
                                          emptyText: '', maxLengthText: '子系统名称长度不能超过50个字符', anchor: '90%'
                                      }
                             ]
                           }
                     ]
               },
               {
                   layout: "column", // 从上往下的布局
                   border: false,
                   items: [
                     {
                         columnWidth: .5, // 该列有整行中所占百分比
                         border: false,
                         layout: "form", // 从上往下的布局
                         items: [{ fieldLabel: '交付地点', xtype: 'textfield', id: 'DeliveriesAddress', name: 'DeliveriesAddress', maxLength: 50,
                             emptyText: '', maxLengthText: '交付地点长度不能超过50个字符', anchor: '90%'
                         }
                           ]
                     },
                       {
                           columnWidth: .5, // 该列有整行中所占百分比
                           border: false,
                           layout: "form", // 从上往下的布局
                           items: [{
                               xtype: 'combo',
                               triggerAction: 'all',
                               id: 'ProjectPhaseName',
                               name: 'ProjectPhaseName',
                               fieldLabel: '项目阶段',
                               emptyText: '==选择==',
                               forceSelection: true,
                               editable: true,
                               typeAhead: true, //模糊查询
                               displayField: 'text',
                               valueField: 'id',
                               hiddenName: 'text',
                               anchor: '90%',
                               store: ProjectPhase,
                               listeners: {
                                   beforequery: function (e) {
                                       var combo = e.combo;
                                       if (!e.forceAll) {
                                           var input = e.query;
                                           // 检索的正则  
                                           var regExp = new RegExp(".*" + input + ".*");
                                           // 执行检索  
                                           combo.store.filterBy(function (record, id) {
                                               // 得到每个record的项目名称值  
                                               var text = record.get(combo.displayField);
                                               return regExp.test(text);
                                           });
                                           combo.expand();
                                           return false;
                                       }
                                   }
                               }
                           }
                                ]
                       }
                     ]
               },
               {
                   layout: "column", // 从上往下的布局
                   border: false,
                   items: [
                         {
                             columnWidth: .5, // 该列有整行中所占百分比
                             border: false,
                             layout: "form", // 从上往下的布局
                             items: [
                                  {
                                      xtype: 'combo',
                                      triggerAction: 'all',
                                      id: 'ContractStateName',
                                      name: 'ContractStateName',
                                      fieldLabel: '合同状态',
                                      emptyText: '==选择==',
                                      forceSelection: true,
                                      editable: true,
                                      typeAhead: true, //模糊查询
                                      displayField: 'text',
                                      valueField: 'id',
                                      hiddenName: 'text',
                                      anchor: '90%',
                                      store: ContractState,
                                      listeners: {
                                          beforequery: function (e) {
                                              var combo = e.combo;
                                              if (!e.forceAll) {
                                                  var input = e.query;
                                                  // 检索的正则  
                                                  var regExp = new RegExp(".*" + input + ".*");
                                                  // 执行检索  
                                                  combo.store.filterBy(function (record, id) {
                                                      // 得到每个record的项目名称值  
                                                      var text = record.get(combo.displayField);
                                                      return regExp.test(text);
                                                  });
                                                  combo.expand();
                                                  return false;
                                              }
                                          }
                                      }
                                  }
                         ]
                         },
                           {
                               columnWidth: .5, // 该列有整行中所占百分比
                               border: false,
                               layout: "form", // 从上往下的布局
                               items: [
                                     { fieldLabel: '合同执行情况', xtype: 'textfield', id: 'ContractIimplementation', name: 'ContractIimplementation', maxLength: 50,
                                         emptyText: '', maxLengthText: '合同执行情况长度不能超过50个字符', anchor: '90%'
                                     }

                             ]
                           }
                     ]
               },
               {
                   layout: "column", // 从上往下的布局
                   border: false,
                   items: [
                     {
                         columnWidth: .5, // 该列有整行中所占百分比
                         border: false,
                         layout: "form", // 从上往下的布局
                         items: [
                         {
                             xtype: 'compositefield',
                             fieldLabel: '签订日期',
                             id: 'timeLimit',
                             combineErrors: false,
                             items: [
							    {
							        xtype: 'textfield',
							        hidden: true
							    },
                                {
                                    name: 'BegSigningDate',
                                    id: 'BegSigningDate',
                                    xtype: 'datefield',
                                    width: 130,
                                    emptyText: '开始时间',
                                    format: 'Y-m-d',
                                    vtype: 'daterange',
                                    endDateField: 'EndSigningDate'
                                },
                                {
                                    xtype: 'tbtext',
                                    text: '~'
                                },
                                { xtype: 'datefield', id: 'EndSigningDate', name: 'EndSigningDate',
                                    emptyText: '', emptyText: '结束日期', format: 'Y-m-d', anchor: '90%'
                                }
					          ]
                         }
                            ]
                     },

                         {
                             columnWidth: .5, // 该列有整行中所占百分比
                             border: false,
                             layout: "form", // 从上往下的布局
                             items: [
                                 {
                                     xtype: 'compositefield',
                                     fieldLabel: '交付日期',
                                     combineErrors: false,
                                     items: [
							            {
							                xtype: 'textfield',
							                hidden: true
							            },
                                        {
                                            name: 'BenDeliverDate',
                                            id: 'BenDeliverDate',
                                            xtype: 'datefield',
                                            width: 130,
                                            emptyText: '开始时间',
                                            format: 'Y-m-d',
                                            vtype: 'daterange',
                                            endDateField: 'EndDeliverDate'
                                        },
                                        {
                                            xtype: 'tbtext',
                                            text: '~'
                                        },
                                        { xtype: 'datefield', id: 'EndDeliverDate', name: 'EndDeliverDate',
                                            emptyText: '', emptyText: '结束日期', format: 'Y-m-d', anchor: '90%'
                                        }
					                    ]
                                 } 
                         ]
                         }
                     ]
               },
               {
                   layout: "column", // 从上往下的布局
                   border: false,
                   items: [
                     {
                         columnWidth: .5, // 该列有整行中所占百分比
                         border: false,
                         layout: "form", // 从上往下的布局
                         items: [
                              { fieldLabel: '有效期', xtype: 'textfield', id: 'ValidityDate', name: 'ValidityDate', maxLength: 50,
                                  emptyText: '', maxLengthText: '有效期长度不能超过50个字符', anchor: '90%'
                              }]
                     },
                       {
                           columnWidth: .5, // 该列有整行中所占百分比
                           border: false,
                           layout: "form", // 从上往下的布局
                           items: [
                              { fieldLabel: '合同金额币种', xtype: 'textfield', id: 'Currency', name: 'Currency', maxLength: 50,
                                  emptyText: '', maxLengthText: '合同金额币种长度不能超过50个字符', anchor: '90%'
                              }]
                       }
                     ]
                  },
               {
                   layout: "column", // 从上往下的布局
                   border: false,
                   items: [
                         {
                             columnWidth: .5, // 该列有整行中所占百分比
                             border: false,
                             layout: "form", // 从上往下的布局
                             items: [
                                 { fieldLabel: '发票税率', xtype: 'textfield', id: 'TaxRate', name: 'TaxRate', maxLength: 50,
                                     emptyText: '', maxLengthText: '发票税率长度不能超过50个字符', anchor: '90%'
                                 }
                         ]
                         },
                           {
                               columnWidth: .5, // 该列有整行中所占百分比
                               border: false,
                               layout: "form", // 从上往下的布局
                               items: [
                               {
                                   xtype: 'combo',
                                   triggerAction: 'all',
                                   id: 'InvoiceTypeName',
                                   name: 'InvoiceTypeName',
                                   fieldLabel: '发票类型',
                                   emptyText: '==选择==',
                                   forceSelection: true,
                                   editable: true,
                                   typeAhead: true, //模糊查询
                                   displayField: 'text',
                                   valueField: 'id',
                                   hiddenName: 'text',
                                   anchor: '90%',
                                   store: InvoiceType,
                                   listeners: {
                                       beforequery: function (e) {
                                           var combo = e.combo;
                                           if (!e.forceAll) {
                                               var input = e.query;
                                               // 检索的正则  
                                               var regExp = new RegExp(".*" + input + ".*");
                                               // 执行检索  
                                               combo.store.filterBy(function (record, id) {
                                                   // 得到每个record的项目名称值  
                                                   var text = record.get(combo.displayField);
                                                   return regExp.test(text);
                                               });
                                               combo.expand();
                                               return false;
                                           }
                                       }
                                   }
                               }

                             ]
                           }
                     ]
               },
               {
                   layout: "column", // 从上往下的布局
                   border: false,
                   items: [
                     {
                         columnWidth: .5, // 该列有整行中所占百分比
                         border: false,
                         layout: "form", // 从上往下的布局
                         items: [
                         {
                                     xtype: 'compositefield',
                                     fieldLabel: '合同总金额',
                                     combineErrors: false,
                                     items: [
							            {
							                xtype: 'textfield',
							                hidden: true
							            },
                                       { xtype: 'numberfield', id: 'BegTotalContractAmount', name: 'BegTotalContractAmount', maxLength: 50,
                                           emptyText: '', width: 110, minValue: 0, maxValue: 999999999
                                         },
                                        {
                                            xtype: 'tbtext',
                                            text: '~'
                                        },
                                        { xtype: 'numberfield', id: 'EndTotalContractAmount', name: 'EndTotalContractAmount', maxLength: 50,
                                            emptyText: '', width: 120, minValue: 0, maxValue: 999999999
                                        }
					                    ]
                                 }
                             ]
                     },
                       {
                           columnWidth: .5, // 该列有整行中所占百分比
                           border: false,
                           layout: "form", // 从上往下的布局
                           items: [
                           {
                               xtype: 'compositefield',
                               fieldLabel: '财务收款总额',
                               combineErrors: false,
                               items: [
							            {
							                xtype: 'textfield',
							                hidden: true
							            },
                                       { xtype: 'numberfield', id: 'BenReceiptsTotalAmount', name: 'BenReceiptsTotalAmount', maxLength: 50,
                                           emptyText: '', width: 110, minValue: 0, maxValue: 999999999
                                       },
                                        {
                                            xtype: 'tbtext',
                                            text: '~'
                                        },
                                        { xtype: 'numberfield', id: 'EndReceiptsTotalAmount', name: 'EndReceiptsTotalAmount', maxLength: 50,
                                            emptyText: '', width: 120, minValue: 0, maxValue: 999999999
                                        }
					                    ]
                           }
                              ]
                       }
                     ]
                            },
               {
                   layout: "column", // 从上往下的布局
                   border: false,
                   items: [
                     {
                         columnWidth: .5, // 该列有整行中所占百分比
                         border: false,
                         layout: "form", // 从上往下的布局
                         items: [
                         {
                             xtype: 'compositefield',
                             fieldLabel: '待收款金额',
                             combineErrors: false,
                             items: [
							            {
							                xtype: 'textfield',
							                hidden: true
							            },
                                      { fieldLabel: '待收款金额', xtype: 'numberfield', id: 'BegReceivablesAmount', name: 'BegReceivablesAmount', maxLength: 50,
                                        emptyText: '',  width: 110, minValue: 0, maxValue: 999999999, readOnly: true
                                    },
                                        {
                                            xtype: 'tbtext',
                                            text: '~'
                                        },
                                        { xtype: 'numberfield', id: 'EndReceivablesAmount', name: 'EndReceivablesAmount', maxLength: 50,
                                            emptyText: '', width: 120, minValue: 0, maxValue: 999999999
                                        }
					                    ]
                         }
                             ]
                     },
                       {
                           columnWidth: .5, // 该列有整行中所占百分比
                           border: false,
                           layout: "form", // 从上往下的布局
                           items: [
                                 { fieldLabel: '协作单位名称', xtype: 'textfield', id: 'UnitName', name: 'UnitName', maxLength: 50,
                                     emptyText: '', maxLengthText: '协作单位名称长度不能超过50个字符', anchor: '90%'
                                 }
                              ]
                       }
                     ]
               }
               ]
        }
          ]
    });

          return form;
}

//更多查询面板
function BtnMore() {
    var form = CreateSearchWindow("组合查询", "");
    var shop = new top.Ext.Window({
        width: 780,
        id: 'SearchMore',
        height: 550,
        closable: false,
        shadow: false,
        stateful: false,
        border: false,
        modal: true,
        layout: 'fit',
        plain: true,
        autoDestroy: true,
        closeAction: 'close',
        title: '组合查询面板',
        items: form,
        buttons: [
                    {
                        text: '查询',
                        iconCls: 'GTP_query',
                        handler: function () {
                            SaveSearch();
                        }
                    },
                    {
                        text: '取消',
                        iconCls: 'GTP_cancel',
                        handler: function () {
                            top.Ext.getCmp("SearchMore").close();
                        }
                    }]
    }).show();
            }

var param = {};

//获取组合查询面板数据
function GetParam() {
    if (top.Ext.getCmp("SearchPanel")) {
        var Nature = top.Ext.getCmp("NatureName").getValue(); //合同性质
        var Project = top.Ext.getCmp("ProjectName").getValue(); //项目
        var ProjectPhase = top.Ext.getCmp("ProjectPhaseName").getValue(); //合同阶段
        var ContractState = top.Ext.getCmp("ContractStateName").getValue(); //合同状态
        var InvoiceType = top.Ext.getCmp("InvoiceTypeName").getValue(); //发票类型

        param = {
            Nature: Nature,
            Project: Project,
            ProjectPhase: ProjectPhase,
            ContractState: ContractState,
            InvoiceType: InvoiceType,
            ContraceName: top.Ext.getCmp("ContraceName").getValue().trim(), //合同名称
            InitiatorUser: top.Ext.getCmp("InitiatorUser").getValue().trim(), //发起人
            InitiatorDept: top.Ext.getCmp("InitiatorDept").getValue().trim(), //发起部门
            Agent: top.Ext.getCmp("Agent").getValue().trim(), //经办人
            Approver: top.Ext.getCmp("Approver").getValue().trim(), //审批人
            Subsystem: top.Ext.getCmp("Subsystem").getValue().trim(), //子系统名称
            DeliveriesAddress: top.Ext.getCmp("DeliveriesAddress").getValue().trim(), //交付地点
            ContractIimplementation: top.Ext.getCmp("ContractIimplementation").getValue().trim(), //合同执行情况
            BegSigningDate: top.Ext.getCmp("BegSigningDate").getValue(), //签订日期
            EndSigningDate: top.Ext.getCmp("EndSigningDate").getValue(), //
            BenDeliverDate: top.Ext.getCmp("BenDeliverDate").getValue(), //交付日期
            EndDeliverDate: top.Ext.getCmp("EndDeliverDate").getValue(), //
            ValidityDate: top.Ext.getCmp("ValidityDate").getValue().trim(), //有效期
            Currency: top.Ext.getCmp("Currency").getValue().trim(), //合同金额币种
            TaxRate: top.Ext.getCmp("TaxRate").getValue().trim(), //发票税率
            UnitName: top.Ext.getCmp("UnitName").getValue().trim(), //协作单位名
            BegTotalContractAmount: top.Ext.getCmp("BegTotalContractAmount").getValue(), //合同总金额
            EndTotalContractAmount: top.Ext.getCmp("EndTotalContractAmount").getValue(), //
            BenReceiptsTotalAmount: top.Ext.getCmp("BenReceiptsTotalAmount").getValue(), //财务收款总额
            EndReceiptsTotalAmount: top.Ext.getCmp("EndReceiptsTotalAmount").getValue(), //
            BegReceivablesAmount: top.Ext.getCmp("BegReceivablesAmount").getValue(), //待收款金额
            EndReceivablesAmount: top.Ext.getCmp("EndReceivablesAmount").getValue() //
        };
    }
    return param;
}
//查询
var SaveSearch = function () {
    var grid = Ext.getCmp("gg");
    var formPanel = top.Ext.getCmp("SearchPanel");
    if (formPanel.getForm().isValid()) {//如果验证通过
        var _store = Ext.getCmp("gg").store;
        Ext.apply(_store.baseParams, GetParam());
        _store.reload({ params: { start: 0, limit: parseInt(Ext.getCmp("pagesize").getValue())} });
    }
}
//保存
var SaveDate = function () {
    var grid = Ext.getCmp("gg");
    var formPanel = top.Ext.getCmp("formPanel");
    var win = top.Ext.getCmp("window");
    if (formPanel.getForm().isValid()) {//如果验证通过
        var Nature = top.Ext.getCmp("NatureName").getValue(); //合同性质
        var Project = top.Ext.getCmp("ProjectName").getValue(); //项目
        var ProjectPhase = top.Ext.getCmp("ProjectPhaseName").getValue(); //合同阶段
        var ContractState = top.Ext.getCmp("ContractStateName").getValue(); //合同状态
        var InvoiceType = top.Ext.getCmp("InvoiceTypeName").getValue(); //发票类型
//        var ProductionDate = top.Ext.getCmp("ProductionDate").getValue().format('Y-m-d'); //生产日期
        var para = { Nature: Nature, Project: Project, ProjectPhase: ProjectPhase, ContractState: ContractState, InvoiceType: InvoiceType };
        if (win.title == "编辑") {
            //得到选后的数据   
            var rows = grid.getSelectionModel().getSelections();
            para = { Id: rows[0].get("Id"), Nature: Nature, Project: Project, ProjectPhase: ProjectPhase, ContractState: ContractState, InvoiceType: InvoiceType };
        }
        formPanel.getForm().submit({
            waitTitle: '系统提示', //标题
            waitMsg: '正在提交数据,请稍后...', //提示信息
            submitEmptyText: false,
            method: "POST",
            url: url,
            params: para,
            success: function (form, action) {
                //成功后
                var flag = action.result;
                if (flag.success) {
                    MessageInfo("保存成功！", "right");
                    win.close();
                    Ext.getCmp("gg").store.reload();
                } else {
                    MessageInfo(flag.msg, "error");
                }
            },
            failure: function (form, action) {
                MessageInfo("保存失败！", "error");
            }
        });
    }
}