﻿//grid单击默认进行编辑操作
function gridrowclick(grid, rowindex, e) {
    var grid = Ext.getCmp("gg");
    var SourceGrid = Ext.getCmp("rightGrid");
    if (grid.store.data.length > 0) {
        var rows = grid.getSelectionModel().getSelections();
        if (rows.length == 1) {
            treeNodeId = rows[0].data["Id"];
            SourceGrid.getStore().proxy.conn.url = '/CACAI/HPurchase/SearchDataDetail?MainId=' + treeNodeId;
            SourceGrid.getStore().reload();
        }
        else {
            SourceGrid.getStore().reload();
        }
    }
    else {
        treeNodeId = -1;
        SourceGrid.getStore().reload();
    }
}
function dbGridClick(row, rowindex) {
    if (row.editing == false && IsEdit == true) {
        FileAttach();
    }
}
//渲染退货状态
function renderOutStatus(val, metadata, record, rowIndex, columnIndex, store) {
    switch (val) {
        case 0:
            return "<span style='color:silver'>未确认</span>";
            break;
        case 1:
            return "<span style='color:green'>已确认</span>";
            break;
    }
}
function renderMoney(val, metadata, record, rowIndex, columnIndex, store) {
    if (val > 0) {
        return "<span style='color:red'>" + val + "</span>";
    }
    else if (val == 0) {
        return val;
    } else {
        return "<span style='color:green'>" + val + "</span>";
    }
}

//渲染财务状态
function renderFinancialState(val, metadata, record, rowIndex, columnIndex, store) {
    switch (val) {
        case -2: //单据管理驳回
            return "<span style='color:red'>已驳回</span>";
            break;
        case -1: //凭证审核驳回
            return "<span style='color:Orange'>待审核</span>";
            break;
        case 0:
            return "未提交";
            break;
        case 1:
            return "<span style='color:Orange'>待审核</span>";
            break;
        case 2:
            return "<span style='color:green'>已审核</span>";
            break;
        case 3:
            return "<span style='color:green'>已支付</span>";
            break;
        default:
            return "未提交";
            break;
    }
}
//批量导出
function BatchExport() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length > 0) {
        var confirm = top.Ext.MessageBox.confirm('系统确认', '您确定要导出当前选中的信息吗?', function (e) {
            if (e == "yes") {
                top.Ext.MessageBox.wait('', '系统正在执行中,请稍后...'); //显示等待信息框   
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push("'" + rows[i].data["Id"] + "'");
                }
                Ext.Ajax.request({
                    url: '/CACAI/HPurchase/ImportOut',
                    params: {
                        IdList: ids.join(",")
                    },
                    methond: 'post',
                    timeout: '60000000',
                    async: true,
                    success: function (repsones, options) {
                        var rs = eval('(' + repsones.responseText + ')');
                        if (rs.success == true) {
                            showExport(rs.msg);
                        }
                        top.Ext.MessageBox.hide(); //完成后隐藏消息框   
                    },
                    failure: function (response, options) {
                        top.Ext.MessageBox.hide(); //完成后隐藏消息框   
                    }
                });
//                var url = '/CACAI/HPurchase/ImportOut?IdList=' + ids.join(",") + '&date=' + new Date();
//                $(".hideform").attr("action", url);
//                $(".hideform").submit();
//                top.Ext.MessageBox.hide(); //完成后隐藏消息框   
            }
        });
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}

//列表文件上传
function UoLoad(OutNumber) {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        top.Ext.MessageBox.wait('', '系统正在执行中,请稍后...'); //显示等待信息框   
        Ext.Ajax.request({
            url: "/SysFileAttach/FileUpload?KeyId=" + rows[0].get("Id") + "&ModelCode=OutCome",
            isUpload: true,
            form: "upForm" + OutNumber,
            success: function (response) {
                var rs = eval('(' + response.responseText + ')');
                if (rs.success) {
                    MessageInfo("上传成功！", "right");
                    rows[0].data.HasFileAttach = true;
                    Ext.getCmp("gg").view.refresh();
                } else {
                    MessageInfo("上传失败！", "error");
                }
                top.Ext.MessageBox.hide(); //完成后隐藏消息框   
            },
            failure: function (response) {
                top.Ext.MessageBox.hide(); //完成后隐藏消息框   
                top.Ext.Msg.show({ title: "系统错误提示", msg: '操作失败，请联系开发人员!', buttons: Ext.Msg.OK, icon: Ext.MessageBox.ERROR });
            }
        });
    }
}

//上传凭证
function uploadFile() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        //多文件上传   
        var dialog = new top.Ext.ux.UploadDialog.Dialog({
            autoCreate: true,
            closable: true,
            collapsible: false,
            draggable: true,
            modal: true,
            shadow: false,
            stateful: false,
            minWidth: 400,
            minHeight: 200,
            width: 500,
            height: 350,
            // permitted_extensions:['JPG','jpg','jpeg','JPEG','GIF','gif','xls','XLS'],      
            proxyDrag: true,
            resizable: true,
            constraintoviewport: true,
            title: '文件上传',
            url: '/SysFileAttach/FileUpload', //上传地址
            post_var_name: 'mms',
            base_params: { hehe: "wayfoon", ModelCode: 'OutCome', KeyId: rows[0].get("Id") },
            reset_on_hide: false,
            allow_close_on_upload: false,  //关闭上传窗口是否仍然上传文件
            upload_autostart: false //是否自动上传文件   
        });
        dialog.show();
        dialog.on('uploadsuccess', onUploadSuccess); //定义上传成功回调函数      
        dialog.on('uploadfailed', onUploadFailed); //定义上传失败回调函数      
        dialog.on('uploaderror', onUploadFailed); //定义上传出错回调函数     
        dialog.on('uploadcomplete', onUploadComplete); //定义上传完成回调函数
    }
}

//文件上传成功后的回调函数     
onUploadSuccess = function (dialog, filename, resp_data, record) {
    if (!resp_data.success) {
        alert(resp_data.message); //resp_data是json格式的数据     
    }
    else {
        Ext.getCmp("gg").store.reload();
    }
}

//文件上传失败后的回调函数     
onUploadFailed = function (dialog, filename, resp_data, record) {
    alert(resp_data.message);
}

//文件上传完成后的回调函数
onUploadComplete = function (dialog) {
    // dialog.hide();
}

//凭证管理
function FileAttach() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var ShowBtn = true; //是否显示附件工具栏
//        if (!Ext.getCmp("EditDate")) {
//            ShowBtn = false;
        //        }
        var FinancialState = rows[0].get("FinancialState");
        if (FinancialState == 1 || FinancialState == 2) {
            ShowBtn = false;
        }
        var key = rows[0].get("Id");
        var CusterName = rows[0].get("CusterName");
        var TotalPrice = rows[0].get("TotalPrice");
        var shop = new top.Ext.Window({
            width: 880,
            id: 'FileAttachWin',
            height: 510,
            closable: true,
            constrainHeader: true, //视图区域中显示
            //shadow: false,
            //stateful: false,
            border: false,
            //modal: true,
            layout: 'fit',
            plain: true,
            autoDestroy: true,
            closeAction: 'close',
            title: '凭证管理' + '  ( <span style="color:red"> 供应商：' + CusterName + ',退货金额：' + TotalPrice + '</span>)',
            items: {
                autoScroll: true,
                border: false,
                params: { KeyId: key, ModelCode: "OutCome", ShowBtn: ShowBtn },
                autoLoad: { url: '../../Project/Html/FileAttach.htm', scripts: true, nocache: true }
            },
            buttons: [
                        {
                            text: '取消',
                            iconCls: 'GTP_cancel',
                            tooltip: '取消当前的操作',
                            handler: function () {
                                try {
                                    top.Ext.getCmp("FileAttachWin").close();
                                }
                                catch (e) {
                                    Ext.MessageBox.alert("提示", "操作错误，请手动关闭窗体，右上角点击退出重新打开！");
                                }
                            }
                        }]
        }).show();
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }


}

//加载明细表单
function loadDetail(title, key) {
    var form = new top.Ext.FormPanel({
        id: "formPanel",
        labelAlign: 'right',
        frame: true,
        bodyStyle: 'padding:5px 5px 0',
        width: 650,
        labelWidth: 80,
        fileUpload: true,
        autoScroll: true,
        border: false,
        items: [
            {
                xtype: "fieldset",
                autoHeight: true,
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
                                    {
                                        name: 'GoodName',
                                        fieldLabel: '商品名称',
                                        xtype: 'textfield',
                                        anchor: '90%',
                                        id: 'GoodName',
                                        style: 'background-color:transparent;background-image: none;', readOnly: true
                                    }
                                ]
                                },{
                                    columnWidth: .5, // 该列有整行中所占百分比
                                    border: false,
                                    layout: "form", // 从上往下的布局
                                    items: [
                                        {
                                            name: 'Code',
                                            fieldLabel: '商品编码',
                                            xtype: 'textfield',
                                            anchor: '90%',
                                            id: 'Code'
                                        }
                                    ]
                                },
                                
                                {
                                    columnWidth: .5, // 该列有整行中所占百分比
                                    border: false,
                                    layout: "form", // 从上往下的布局
                                    items: [
                                        {
                                            name: 'StyleNum',
                                            id: 'StyleNum',
                                            fieldLabel: '款式编号',
                                            anchor: '90%',
                                            xtype: 'textfield'
                                        }
                                    ]
                                },
                                {
                                    columnWidth: .5, // 该列有整行中所占百分比
                                    border: false,
                                    layout: "form", // 从上往下的布局
                                    items: [
                                      {
                                          name: 'GoodUnit',
                                          fieldLabel: '颜色及规格',
                                          xtype: 'textfield',
                                          anchor: '90%',
                                          id: 'GoodUnit'
                                      }
                                    ]
                                },
                                {
                                    columnWidth: 1, // 该列有整行中所占百分比
                                    border: false,
                                    layout: "form", // 从上往下的布局
                                    items: [
                                       {
                                           name: 'FreightNum',
                                           fieldLabel: '供应商货号',
                                           xtype: 'textfield',
                                           anchor: '95%',
                                           id: 'FreightNum'
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
                                           fieldLabel: '数量/单价',
                                           combineErrors: false,
                                           anchor: '90%',
                                           items: [
                                           { fieldLabel: '数量', xtype: 'numberfield', id: 'Count', name: 'Count', maxLength: 50,
                                               emptyText: '数量', width: 95, minValue: 0, maxValue: 999999999, enableKeyEvents: true, listeners: {
                                                   'change': function (e) {
                                                       //设置金额
                                                       var Count = top.Ext.getCmp("Count").getValue();
                                                       var Price = top.Ext.getCmp("Price").getValue();
                                                       top.Ext.getCmp("Money").setValue(Count * Price);

                                                       if (top.Ext.getCmp("BillNum").getValue() == "") {
                                                           top.Ext.getCmp("BillNum").setValue(Count);

                                                           ControlBillMoney(); //联动单据款金额
                                                       }
                                                       ControlLosstPrice(); //联动盈亏金额
                                                   }
                                               }
                                           },
                                            {
                                                xtype: 'tbtext',
                                                text: '~'
                                            },
                                             { fieldLabel: '单价', xtype: 'numberfield', id: 'Price', name: 'Price', maxLength: 50,
                                                 emptyText: '单价', width: 95, minValue: 0, maxValue: 999999999, enableKeyEvents: true, listeners: {
                                                     'change': function (e) {
                                                         //设置金额
                                                         var Count = top.Ext.getCmp("Count").getValue();
                                                         var Price = top.Ext.getCmp("Price").getValue();
                                                         top.Ext.getCmp("Money").setValue(Count * Price);

                                                         if (top.Ext.getCmp("BillPrice").getValue() == "") {
                                                             top.Ext.getCmp("BillPrice").setValue(Price);

                                                             ControlBillMoney(); //联动单据款金额
                                                         }
                                                         ControlLosstPrice(); //联动盈亏金额
                                                     }
                                                 }
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
                                       { fieldLabel: '金额', xtype: 'numberfield', id: 'Money', name: 'Money', maxLength: 50,
                                           emptyText: '', anchor: '90%', minValue: 0, maxValue: 999999999, style: 'background-color:transparent;background-image: none;', readOnly: true
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
                                           fieldLabel: '单据数量/金额',
                                           combineErrors: false,
                                           anchor: '90%',
                                           items: [
                                                { fieldLabel: '单据数量', xtype: 'numberfield', id: 'BillNum', name: 'BillNum', maxLength: 50,
                                                    emptyText: '单据数量', width: 95, maxValue: 999999999, enableKeyEvents: true, listeners: {
                                                        'keyup': function (e) {
                                                            //设置单据金额值
                                                            var BillNum = top.Ext.getCmp("BillNum").getValue();
                                                            var Money = top.Ext.getCmp("Money").getValue();
                                                            top.Ext.getCmp("BillPrice").setValue(BillNum * Money);

                                                            //设置单据款金额
                                                            top.Ext.getCmp("BillMoney").setValue(BillNum * top.Ext.getCmp("BillPrice").getValue());

                                                            ControlLosstPrice();
                                                        }
                                                    }
                                                },
                                            {
                                                xtype: 'tbtext',
                                                text: '~'
                                            },
                                            { fieldLabel: '单据金额', xtype: 'numberfield', id: 'BillPrice', name: 'BillPrice', maxLength: 50,
                                                emptyText: '单据金额', width: 95, maxValue: 999999999, enableKeyEvents: true, listeners: {
                                                    'keyup': function (e) {
                                                        var BillNum = top.Ext.getCmp("BillNum").getValue();
                                                        var BillPrice = top.Ext.getCmp("BillPrice").getValue();
                                                        //设置单据款金额
                                                        top.Ext.getCmp("BillMoney").setValue(BillNum * BillPrice);

                                                        ControlLosstPrice();
                                                    }
                                                }
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
                                   { fieldLabel: '单据款金额', xtype: 'numberfield', id: 'BillMoney', name: 'BillMoney', maxLength: 50,
                                       emptyText: '', anchor: '90%', maxValue: 999999999, style: 'background-color:transparent;background-image: none;', readOnly: true, listeners: {
                                           'keyup': function (e) {
                                               //设置单据金额值
                                               var BillNum = top.Ext.getCmp("BillNum").getValue();
                                               var Money = top.Ext.getCmp("Money").getValue();
                                               top.Ext.getCmp("BillMoney").setValue(BillNum * Money);
                                           },
                                           change: function (e) {
                                               ControlLosstPrice();
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
                                    { fieldLabel: '核查补充数量', xtype: 'numberfield', id: 'CheckNum', name: 'CheckNum', maxLength: 50,
                                        emptyText: '', anchor: '90%', maxValue: 999999999, enableKeyEvents: true, listeners: {
                                            'keyup': function (e) {
                                                //设置核查补充金额
                                                var CheckNum = top.Ext.getCmp("CheckNum").getValue();
                                                var BillPrice = top.Ext.getCmp("BillPrice").getValue();
                                                top.Ext.getCmp("CheckPrice").setValue(CheckNum * BillPrice);
                                                ControlLosstPrice(); //联动盈亏金额
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
                                    { fieldLabel: '核查补充金额', xtype: 'numberfield', id: 'CheckPrice', name: 'CheckPrice', maxLength: 50,
                                        emptyText: '', anchor: '90%', maxValue: 999999999, style: 'background-color:transparent;background-image: none;', readOnly: true, enableKeyEvents: true, listeners: {
                                            'change': function (e) {
                                                ControlLosstPrice();
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
                                      { fieldLabel: '盈亏金额', xtype: 'numberfield', id: 'LosstPrice', name: 'LosstPrice', maxLength: 50,
                                          emptyText: '', anchor: '90%', maxValue: 999999999, style: 'background-color:transparent;background-image: none;', readOnly: true
                                      }
                                ]
                                },
                                 {
                                     columnWidth: 1, // 该列有整行中所占百分比
                                     border: false,
                                     layout: "form", // 从上往下的布局
                                     items: [
                                     {
                                         fieldLabel: '备注',
                                         xtype: 'htmleditor',
                                         id: 'Remark',
                                         name: 'Remark',
                                         height: 150,
                                         emptyText: '可输入对备注的介绍信息', ////textfield自己的属性
                                         anchor: '95%'
                                     }
                                ]
                                 }
                              ]
                   }
                ]
            }
        ]
    });

    //窗体
    var win = Window("window", title, form);
    win.width = 700;
    win.height = 520;
    return win;
}
//新增
function BtnNewAdd() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var Id = rows[0].data["Id"];
        url = '/CACAI/HPurchase/SaveDetail?MainId=' + Id;
        var win = loadDetail("新增", "");
        win.show();
        top.Ext.getCmp("GoodName").setValue("新增");
    }
    else {
        MessageInfo("请选中一条主表记录！", "statusing");
    }
}
//编辑
function BtnNewEdit() {
    var grid = Ext.getCmp("rightGrid");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].get("Id");
        var win = loadDetail("编辑", key);
        var form = top.Ext.getCmp('formPanel');
        form.form.loadRecord(rows[0]);
        win.show();
        url = '/CACAI/HPurchase/SaveDetail?Id=' + key + '&modify=1';
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}
//删除
function BtnNewDelete() {
    var grid = Ext.getCmp("rightGrid");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length > 0) {
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要删除该信息吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push("'" + rows[i].data["Id"] + "'");
                }
                //获取主表ID
                var grid = Ext.getCmp("gg");
                var mains = grid.getSelectionModel().getSelections();
                var key = mains[0].data.Id;
                top.Ext.MessageBox.wait('', '系统正在执行中,请稍后...'); //显示等待信息框   

                Ext.Ajax.request({
                    url: '/CACAI/HPurchase/DeleteDetail',
                    params: { id: ids.join(","), key: key },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            var LossPrice = rs.msg.split(',')[0]; //返回盈亏金额
                            var TotalPrice = rs.msg.split(',')[1]; //供应商金额
                            var grid = Ext.getCmp("gg");
                            var rows = grid.getSelectionModel().getSelections();
                            rows[0].data.LossPrice = parseFloat(LossPrice).toFixed(2);
                            rows[0].data.TotalPrice = parseFloat(TotalPrice).toFixed(2);
                            Ext.getCmp("gg").view.refresh();

                            MessageInfo("删除成功！", "right");
                            Ext.getCmp("rightGrid").store.reload();
                        } else {
                            MessageInfo("删除失败！", "error");
                        }
                    },
                    failure: function (response) {
                        top.Ext.MessageBox.hide(); //完成后隐藏消息框   
                        top.Ext.Msg.show({ title: "系统错误提示", msg: '操作失败，请联系开发人员!', buttons: Ext.Msg.OK, icon: Ext.MessageBox.ERROR });
                    }
                });
            }
        });
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}
//刷新明细
function BtnRefeshDetail() {
    Ext.getCmp("rightGrid").store.reload();
}


//合并操作
function BtnCombine() {
    if (Ext.getCmp('rightGrid').getStore().totalLength > 0) {
        //获取主表信息
        var grid = Ext.getCmp("gg");
        var rows = grid.getSelectionModel().getSelections();
        if (rows.length == 1) {
            var Id = rows[0].data.Id;
            top.Ext.MessageBox.wait('', '系统正在执行中,请稍后...'); //显示等待信息框 
            Ext.Ajax.request({
                url: '/CACAI/HPurchase/CombineAll',
                params: { id: Id },
                success: function (response) {
                    var rs = eval('(' + response.responseText + ')');
                    if (rs.success) {
                        var LossPrice = rs.msg.split(',')[0]; //返回盈亏金额
                        var TotalPrice = rs.msg.split(',')[1]; //供应商金额
                        var count = rs.msg.split(',')[2]; // 

                        rows[0].data.LossPrice = parseFloat(LossPrice).toFixed(2);
                        rows[0].data.TotalPrice = parseFloat(TotalPrice).toFixed(2);

                        Ext.getCmp("gg").view.refresh();
                        if (count > 0) {
                            Ext.getCmp("rightGrid").store.reload();
                            top.Ext.MessageBox.alert("提示", "合并成功，合并条数：<span style='color:red'>" + count + "<span/>");
                        }
                        else {
                            top.Ext.MessageBox.alert("提示", "暂时没有相同信息的商品！");
                        }
                    } else {
                        MessageInfo("合并失败！", "error");
                    }
                    top.Ext.MessageBox.hide(); //完成后隐藏消息框   
                },
                failure: function (response) {
                    top.Ext.MessageBox.hide(); //完成后隐藏消息框   
                    top.Ext.Msg.show({ title: "系统错误提示", msg: '操作失败，请联系开发人员!', buttons: Ext.Msg.OK, icon: Ext.MessageBox.ERROR });
                }
            });
        }
        else {
            MessageInfo("请选中一条主表记录！", "statusing");
        }
    }
    else {
        MessageInfo("暂无明细，无法进行合并操作！", "statusing");
    }
}
function tbarDIY(PageName) {
    //同步查询页面权限按钮
    var tb = new Ext.Toolbar();
    tb.add({
        text: '刷新',
        tooltip: '刷新',
        id: 'BtnRefeshDetail',
        iconCls: 'GTP_refresh',
        handler: BtnRefeshDetail //因为是字符串类型,找不到方法,所以需转义一下
    });
    tb.add('-');
    tb.add({
        text: '新增',
        tooltip: '新增',
        id: 'BtnNewAdd',
        iconCls: 'GTP_add',
        handler: BtnNewAdd //因为是字符串类型,找不到方法,所以需转义一下
    });
//    tb.add('-');
//    tb.add({
//        text: '编辑',
//        tooltip: '编辑',
//        id: 'BtnNewEdit',
//        iconCls: 'GTP_edit',
//        handler: BtnNewEdit //因为是字符串类型,找不到方法,所以需转义一下
//    });
//    tb.add('-');
//    tb.add({
//        text: '删除',
//        tooltip: '删除',
//        id: 'PriceCut',
//        iconCls: 'GTP_delete',
//        handler: BtnNewDelete //因为是字符串类型,找不到方法,所以需转义一下
//    });

    tb.add('->');
    tb.add({
        text: '明细合并',
        tooltip: '明细合并',
        id: 'Combine',
        iconCls: 'GTP_combine',
        handler: BtnCombine //因为是字符串类型,找不到方法,所以需转义一下
    });
    tb.add('-');
    tb.add({
        text: '多选统计：',
        id: 'waitBtn',
        iconCls: 'GTP_count',
        handler: function () {
        }
    });

    tb.add({
        xtype: 'tbtext',
        text: '<span id="txtChart" style="color:blue; font-weight:400;">' +
        //'货号：<span id="totalMoney1">0</span>&nbsp;&nbsp;，' +
              '数量：<span id="totalMoney2">0</span>&nbsp;&nbsp;，' +
               '金额：<span id="totalMoney3">0</span>&nbsp;&nbsp;，' +
               '单据数量：<span id="totalMoney4">0</span>&nbsp;&nbsp;，' +
               '单据金额：<span id="totalMoney5">0</span>&nbsp;&nbsp;，' +
               '单据款金额：<span id="totalMoney15">0</span>&nbsp;&nbsp;，' +
               '核查金额：<span id="totalMoney6">0</span>&nbsp;&nbsp;，' +
               '盈亏金额：<span id="totalMoney7">0</span>&nbsp;'
    })

    //    if (moreSearch == true) {
    //        tb.add('-');
    //        tb.add({
    //            text: '更多>>',
    //            id: 'BtnMore',
    //            pressed: false,
    //            handler: BtnMore
    //        });
    //    }
    return tb;
};
//主表选中事件
function MainCellclick() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length >= 1) {
        //进项统计
        var TotalPrice = 0;
        var LossPrice = 0;

        for (var i = 0; i < rows.length; i++) {
            TotalPrice += parseFloat(rows[i].get("TotalPrice")); //供应商金额
            LossPrice += parseFloat(rows[i].get("LossPrice")); //盈亏金额
        }
        document.getElementById("chart1").innerHTML = TotalPrice;
        document.getElementById("chart2").innerHTML = LossPrice;
    }
    else {
        document.getElementById("chart1").innerHTML = 0;
        document.getElementById("chart2").innerHTML = 0;
    }
}
//行选中事件
function cellclick() {
    var grid = Ext.getCmp("rightGrid");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length >= 1) {
        //进项统计
        var Count = 0;
        var Money = 0;
        var BillNum = 0;
        var BillPrice = 0;
        var CheckPrice = 0;
        var LosstPrice = 0;
        var BillMoney = 0;
        for (var i = 0; i < rows.length; i++) {
            Count += parseFloat(rows[i].get("Count")); //数量
            Money += parseFloat(rows[i].get("Money")); //金额
            BillNum += parseFloat(rows[i].get("BillNum")); //单据数量
            BillPrice += parseFloat(rows[i].get("BillPrice")); //单据金额
            BillMoney += parseFloat(rows[i].get("BillMoney")); //单据款金额
            CheckPrice += parseFloat(rows[i].get("CheckPrice")); //核查金额
            LosstPrice += parseFloat(rows[i].get("LosstPrice")); //盈亏金额
        }

        document.getElementById("totalMoney2").innerHTML = Count;
        document.getElementById("totalMoney3").innerHTML = Money;
        document.getElementById("totalMoney4").innerHTML = BillNum;
        document.getElementById("totalMoney5").innerHTML = BillPrice;
        document.getElementById("totalMoney15").innerHTML = BillMoney;
        document.getElementById("totalMoney6").innerHTML = CheckPrice;
        document.getElementById("totalMoney7").innerHTML = LosstPrice;
    }
    else {
        //货号：<span id="totalMoney1">0</span>&nbsp;&nbsp;，
        document.getElementById("totalMoney2").innerHTML = 0;
        document.getElementById("totalMoney3").innerHTML = 0;
        document.getElementById("totalMoney4").innerHTML = 0;
        document.getElementById("totalMoney5").innerHTML = 0;
        document.getElementById("totalMoney15").innerHTML = 0;
        document.getElementById("totalMoney6").innerHTML = 0;
        document.getElementById("totalMoney7").innerHTML = 0;
    }
}

var rightClick = new Ext.menu.Menu({
    id: 'rightClickCont',
    items: [
    //        {
    //            id: 'rMenu1',
    //            handler: EditDate,
    //            iconCls: 'GTP_edit',
    //            text: '编辑'
    //        },'-',
        {
        id: 'rMenu2',
        handler: DeleteDate,
        iconCls: 'GTP_delete',
        text: '删除'
    },
        {
            id: 'rMenu3',
            handler: DeleteAll,
            text: '清空全部',
            iconCls: 'GTP_clear'
        }
    ]
});
//右键菜单代码关键部分 
function rightClickFn(grid, rowindex, e) {
    e.preventDefault();
    rightClick.showAt(e.getXY());
}

//创建表单弹框
function CreateFromWindow(title) {
    var form = new top.Ext.FormPanel({
        labelWidth: 65,
        frame: true,
        border: false,
        labelAlign: 'right',
        bodyStyle: 'padding:5px 5px 0',
        id: 'formPanel',
        defaultType: 'textfield',
        items: [
                {
                    fieldLabel: '备注信息',
                    xtype: 'textarea',
                    id: 'Remark',
                    name: 'Remark',
                    height: 150,
                    allowBlank: false,
                    anchor: '95%',
                    emptyText: '可输入备注的描述信息', ////textfield自己的属性
                    maxLength: 200,
                    maxLengthText: '备注长度不能超过200个字符'
                }
        ]
    });

    //窗体
    var win = Window("window", title, form);
    win.width = 500;
    win.height = 300;
    return win;
}

//批量子表备注
function GTP_AllEdit() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length >= 1) {
        var ids = [];
        for (var i = 0; i < rows.length; i++) {
            ids.push("'" + rows[i].data["Id"] + "'");
        }

        var window = CreateFromWindow("批量备注");
        var form = top.Ext.getCmp('formPanel');
        form.form.reset();
        url = '/CACAI/HPurchase/EditDetailRemark?ids=' + ids.join(',');
        window.show();
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}
//情况全部
function DeleteAll() {
    if (Ext.getCmp('gg').store.data.length > 0) {
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要清空列表所有信息吗?', function (e) {
            if (e == "yes") {
                top.Ext.MessageBox.wait('', '系统正在执行中,请稍后...'); //显示等待信息框   
                Ext.Ajax.request({
                    url: '/CACAI/HPurchase/DeleteAll',
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            MessageInfo("清空成功！", "right");
                            reload();
                        } else {
                            MessageInfo("清空失败！", "error");
                        }
                        top.Ext.MessageBox.hide(); //完成后隐藏消息框   
                    },
                    failure: function (response) {
                        top.Ext.MessageBox.hide(); //完成后隐藏消息框   
                        top.Ext.Msg.show({ title: "系统错误提示", msg: '操作失败，请联系开发人员!', buttons: Ext.Msg.OK, icon: Ext.MessageBox.ERROR });
                    }
                });
            }
        });
    }
    else {
        top.Ext.Msg.show({ title: "信息提示", msg: '请选中一条记录', buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
    }
}
//JS日期函数 加几天 减几天 推后天数日期计算
function getthedate(dd, dadd) {
    //可以加上错误处理
    var a = new Date(dd)
    a = a.valueOf()
    a = a + dadd * 24 * 60 * 60 * 1000
    a = new Date(a);
    var m = a.getMonth() + 1;
    if (m.toString().length == 1) {
        m = '0' + m;
    }
    var d = a.getDate();
    if (d.toString().length == 1) {
        d = '0' + d;
    }
    return a.getFullYear() + "-" + m + "-" + d;
}
//渲染设备状态
function renderMaintenanceStatus(val, metadata, record, rowIndex, columnIndex, store) {
    if (val == -1) {
        return "<span style='color:red'>已过期</span>";
    }
    else if (val == 0)
        return "<span style='color:green'>设备正常</span>";
    else if (val == 1) {
        return "<span style='color:orange'>设备异常</span>";
    }
}
//展示图片
function showImg(val) {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].data["Id"];
        var Title = rows[0].data["Name"];
        var QRCode = rows[0].data["QRCode"]; //二维码编码
        var shop = new top.Ext.Window({
            width: 450,
            id: 'shoper',
            height: 350,
            closable: false,
            constrainHeader: true, //视图区域中显示
            shadow: false,
            stateful: false,
            border: false,
            modal: true,
            layout: 'fit',
            plain: true,
            autoDestroy: true,
            closeAction: 'close',
            title: Title + '二维码展示',
            items: {
                xtype: 'panel', //或者xtype: 'component',
                border: false,
                items: [{
                    xtype: 'box', //或者xtype: 'component',
                    id: 'PagLogo',
                    style: 'margin-left:100px;margin-top:20px',
                    width: 200, //图片宽度
                    height: 200, //图片高度
                    autoEl: {
                        tag: 'img',    //指定为img标签
                        src: '/Content/Extjs/resources/images/default/s.gif'    //指定url路径
                    }
                },
                 {
                     style: 'margin-left:100px;',
                     xtype: 'tbtext',
                     html: '<br/>二维码编码:' + QRCode
                 }
                ]

            },
            buttons: [
                    {
                        text: '取消',
                        iconCls: 'GTP_cancel',
                        tooltip: '取消当前的操作',
                        handler: function () {
                            top.Ext.getCmp("shoper").close();
                        }
                    }]
        }).show();

        top.Ext.getCmp("PagLogo").getEl().dom.src = rows[0].data["Img"];
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}

//点击组织架构
function treeitemclick(node, e) {
    if (node.attributes.Attribute == 1) {
        treeNodeId = node.attributes.id;
    }
    else {
        treeNodeId = "-1";
    }
    reload();
}

//展示凭证
function ShowAttach() {
    if (top.Ext.getCmp("ImgShowWin")) {
        top.Ext.getCmp("ImgShowWin").close();
    }
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].get("Id");
        var CusterName = rows[0].get("CusterName");
        var TotalPrice = rows[0].get("TotalPrice");
        var HasFileAttach = rows[0].get("HasFileAttach");
        var title = '凭证预览' + '  ( <span style="color:red"> 供应商：' + CusterName + ',退货金额：' + TotalPrice + '</span>)';
        if (HasFileAttach == "true" || HasFileAttach == true) {
            var index = 0; //获取选中的索引
            var data = [];
            var respon = Ext.lib.Ajax.getConnectionObject().conn;
            respon.open("post", "/SysFileAttach/GetFileList?KeyId=" + key, false);
            respon.send(null);
            var result = Ext.util.JSON.decode(respon.responseText);
            if (result.length > 0) {
                for (var i = 0; i < result.length; i++) {
                    data.push(result[i].FilePath);
                }
            }
            var img1 = new top.ImgView({
                imgindex: index,
                src: data
            });
            var shop = new top.Ext.Window({
                width: 850,
                id: 'ImgShowWin',
                constrainHeader: true, //视图区域中显示
                height: 750,
                closable: true,
                border: false,
                buttonAlign: 'center', //左边
                layout: 'fit',
                plain: true,
                autoDestroy: true,
                closeAction: 'close',
                title: title,
                items: [img1],
                buttons: [
                                {
                                    text: '取消',
                                    iconCls: 'GTP_cancel',
                                    tooltip: '取消当前的操作',
                                    handler: function () {
                                        try {
                                            top.Ext.getCmp("ImgShowWin").close();
                                        }
                                        catch (e) {
                                            Ext.MessageBox.alert("提示", "操作错误，请手动关闭窗体，右上角点击退出重新打开！");
                                        }
                                    }
                                }]
            }).show();
        }
        else {
            top.Ext.Msg.show({ title: "信息提示", msg: '当前单据暂无凭证', buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
        }
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}
/**************************方法************************************/

//明细编辑
function BtnEditDetail() {
    var grid = Ext.getCmp("rightGrid");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].get("Id");
        var win = load("明细编辑", key);
        var form = top.Ext.getCmp('formPanel');
        form.form.loadRecord(rows[0]);
        win.show();
        url = '/CACAI/HPurchase/SaveDataDetail?Id=' + key + '&modify=1';

        //获取款式数量和金额
        GetStypeMoney();
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}

//控制单据款
function ControlBillMoney() {
    var BillNum = top.Ext.getCmp("BillNum").getValue(); //单据数量
    var BillPrice = top.Ext.getCmp("BillPrice").getValue(); //单据金额

    top.Ext.getCmp("BillMoney").setValue(BillNum * BillPrice);
}
//控制盈亏金额
function ControlLosstPrice() {
    var Money = top.Ext.getCmp("Money").getValue(); //金额
    var BillMoney = top.Ext.getCmp("BillMoney").getValue(); //单据款金额
    var CheckPrice = top.Ext.getCmp("CheckPrice").getValue(); //核查补充金额

    top.Ext.getCmp("LosstPrice").setValue(BillMoney - Money - CheckPrice);
}
//获取款式金额
function GetStypeMoney() {
    var totalNum = 0;
    var totalMoney = 0;
    var grid = Ext.getCmp("rightGrid");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var Id = rows[0].get("Id");
        var Code = rows[0].get("Code"); //商品编码
        var StyleNum = rows[0].get("StyleNum"); //款式编号
        var StyleCount = rows[0].get("StyleCount"); //款式数量
        var GoodName = rows[0].get("GoodName"); //商品名称
        var FreightNum = rows[0].get("FreightNum"); //供应商货号
        var FreightNum = rows[0].get("FreightNum"); //供应商货号
        totalNum = parseInt(StyleCount);
        totalMoney = parseFloat(rows[0].get("StylePrice"));
        for (var i = 0; i < grid.store.data.items.length; i++) {
            var record = grid.store.data.items;
            if (record[i].data.StyleNum == StyleNum && record[i].data.Id != Id) {
                totalNum += parseFloat(record[i].data.StyleCount);
                totalMoney += parseFloat(record[i].data.StylePrice);
            }
        }
    }
    top.Ext.getCmp("StyleCount").setValue(totalNum);
    top.Ext.getCmp("TotalStylePrice").setValue(totalMoney);
}
//启用
function EnableUse() {
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        if (rows[0].get("MaintenanceStatus") == "0") {
            MessageInfo("该设备为正常状态！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要恢复该设备正常状态吗?', function (e) {
            if (e == "yes") {
                var key = rows[0].get("Id");
                Ext.Ajax.request({
                    url: '/SysAppointed/RightStatus',
                    params: { id: key },
                    success: function (response) {
                        Ext.getCmp("gg").store.reload();
                    }
                });
            }
        });
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}
//禁用
function DisableUse() {
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        if (rows[0].get("Status") == "0") {
            MessageInfo("该设备已经禁用！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要禁用该设备吗?', function (e) {
            if (e == "yes") {
                var key = rows[0].get("Id");
                Ext.Ajax.request({
                    url: '/SysAppointed/DisableUse',
                    params: { id: key },
                    success: function (response) {
                        Ext.getCmp("gg").store.reload();
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
    var CreateCompanyId = '0';
    var grid = Ext.getCmp("tr");
    if (grid) {
        var rows = grid.getSelectionModel().getSelectedNode();
        if (rows) {
            if (rows.attributes.Attribute == 1) {
                CreateCompanyId = rows.attributes.id;
            }
            else {
                MessageInfo("请先选择单位！", "statusing");
                return false;
            }
        }
        else {
            MessageInfo("请先选择单位！", "statusing");
            return false;
        }
    }
    result = [];
    parent.idArray = [];
    url = '/SysAppointed/SaveData?CreateCompanyId=' + CreateCompanyId;
    var win = load("新增", "", CreateCompanyId);
    win.show();
}
//编辑
function EditDate() {
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();

    if (rows.length == 1) {
        var key = rows[0].get("Id");
        var win = loadTop("编辑", key);
        var form = top.Ext.getCmp('formPanel');
        form.form.loadRecord(rows[0]);
        win.show();
        url = '/CACAI/HPurchase/SaveData?Id=' + key + '&modify=1';
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}
//删除
function DeleteDate() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length > 0) {
        var ids = [];
        for (var i = 0; i < rows.length; i++) {
            var FinancialState = rows[i].data["FinancialState"];
            var OutNumber = rows[i].get("OutNumber"); //采购单号
            if (FinancialState == 1 || FinancialState == -1) {
                top.Ext.Msg.show({ title: "信息提示", msg: "退货单号【" + OutNumber + "】已提交，不能进行删除。", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                return false;
            }
            else if (FinancialState == 2) {
                top.Ext.Msg.show({ title: "信息提示", msg: "退货单号【" + OutNumber + "】已审核通过，不能进行删除。", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                return false;
            }
            else if (FinancialState == 3) {
                top.Ext.Msg.show({ title: "信息提示", msg: "退货单号【" + OutNumber + "】已支付，不能进行删除。", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                return false;
            }
            ids.push("'" + rows[i].data["Id"] + "'");
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要删除该信息吗?', function (e) {
            if (e == "yes") {
                top.Ext.MessageBox.wait('', '系统正在执行中,请稍后...'); //显示等待信息框
                Ext.Ajax.request({
                    url: '/CACAI/HPurchase/DeleteData',
                    params: { id: ids.join(",") },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            MessageInfo("删除成功！", "right");
                            Ext.getCmp("gg").store.reload();
                        } else {
                            MessageInfo("删除失败！", "error");
                        }
                        top.Ext.MessageBox.hide(); //完成后隐藏消息框   
                    },
                    failure: function (response) {
                        top.Ext.MessageBox.hide(); //完成后隐藏消息框   
                        top.Ext.Msg.show({ title: "系统错误提示", msg: '操作失败，请联系开发人员!', buttons: Ext.Msg.OK, icon: Ext.MessageBox.ERROR });
                    }
                });
            }
        });
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}
//提交审核
function GTPsubmit() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length >= 1) {
        var flag = true;
        var ids = [];
        for (var i = 0; i < rows.length; i++) {
            ids.push("'" + rows[i].data["Id"] + "'");
            var HasFileAttach = rows[i].get("HasFileAttach");
            var FinancialState = rows[i].get("FinancialState");
            var OutNumber = rows[i].get("OutNumber"); //采购单号
            if (HasFileAttach != 1) {
                flag = false;
                top.Ext.Msg.show({ title: "信息提示", msg: "退货单号【" + OutNumber + "】凭证不能为空，请先上传凭证!", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                break;
            }
            if (FinancialState == 1) {
                flag = false;
                top.Ext.Msg.show({ title: "信息提示", msg: "退货单号【" + OutNumber + "】单据已提交，不能重复提交!", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                break;
            }
            else if (FinancialState == 2) {
                flag = false;
                top.Ext.Msg.show({ title: "信息提示", msg: "退货单号【" + OutNumber + "】单据已审核，不能重复提交!", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                break;
            }
            else if (FinancialState ==3) {
                flag = false;
                top.Ext.Msg.show({ title: "信息提示", msg: "退货单号【" + OutNumber + "】单据已支付，不能重复提交!", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                break;
            }
        }
        if (flag == true) {
            var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要提交选中的记录信息吗?', function (e) {
                if (e == "yes") {
                    top.Ext.MessageBox.wait('', '系统正在执行中,请稍后...'); //显示等待信息框
                    Ext.Ajax.request({
                        url: '/CACAI/HPurchase/SubmitAudit',
                        params: { id: ids.join(",") },
                        success: function (response) {
                            var rs = eval('(' + response.responseText + ')');
                            if (rs.success) {
                                MessageInfo("提交成功！", "right");
                                Ext.getCmp("gg").store.reload();
                            } else {
                                MessageInfo("提交失败！", "error");
                            }
                            top.Ext.MessageBox.hide(); //完成后隐藏消息框   
                        },
                        failure: function (response) {
                            top.Ext.MessageBox.hide(); //完成后隐藏消息框   
                            top.Ext.Msg.show({ title: "系统错误提示", msg: '操作失败，请联系开发人员!', buttons: Ext.Msg.OK, icon: Ext.MessageBox.ERROR });
                        }
                    });
                }
            });
        }
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}

//审核通过
function GTPAudit() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length >= 1) {
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要审核通过选中的记录信息吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push("'" + rows[i].data["Id"] + "'");
                }
                top.Ext.MessageBox.wait('', '系统正在执行中,请稍后...'); //显示等待信息框  
                Ext.Ajax.request({
                    url: '/CACAI/HPurchase/SubmitData',
                    params: { id: ids.join(",") },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            MessageInfo("审核成功！", "right");
                            Ext.getCmp("gg").store.reload();
                        } else {
                            MessageInfo("审核失败！", "error");
                        }
                        top.Ext.MessageBox.hide(); //完成后隐藏消息框   
                    },
                    failure: function (response) {
                        top.Ext.MessageBox.hide(); //完成后隐藏消息框   
                        top.Ext.Msg.show({ title: "系统错误提示", msg: '操作失败，请联系开发人员!', buttons: Ext.Msg.OK, icon: Ext.MessageBox.ERROR });
                    }
                });
            }
        });
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}

//取消驳回
function GTP_cancelagent() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length >= 1) {
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要驳回选中的记录信息吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push("'" + rows[i].data["Id"] + "'");
                }
                top.Ext.MessageBox.wait('', '系统正在执行中,请稍后...'); //显示等待信息框  
                Ext.Ajax.request({
                    url: '/CACAI/HPurchase/CancelAudit',
                    params: { id: ids.join(",") },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            MessageInfo("驳回成功！", "right");
                            Ext.getCmp("gg").store.reload();
                        } else {
                            MessageInfo("驳回失败！", "error");
                        }
                        top.Ext.MessageBox.hide(); //完成后隐藏消息框   
                    },
                    failure: function (response) {
                        top.Ext.MessageBox.hide(); //完成后隐藏消息框   
                        top.Ext.Msg.show({ title: "系统错误提示", msg: '操作失败，请联系开发人员!', buttons: Ext.Msg.OK, icon: Ext.MessageBox.ERROR });
                    }
                });
            }
        });
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}


//保存主表
function afterEditMain(e) {
    var record = e.record.data;
    var Remark = record.Remark; //备注
    if (Remark != "") {
        top.Ext.MessageBox.wait('', '系统正在执行中,请稍后...'); //显示等待信息框
        Ext.Ajax.request({
            url: '/CACAI/HPurchase/SaveData',
            params: {
                id: record.Id,
                Remark: Remark
            },
            success: function (response) {
                var rs = eval('(' + response.responseText + ')');
                if (rs.success) {
                    e.record.commit();
                    MessageInfo("修改成功！", "right");
                    //Ext.getCmp("gg").store.reload();
                } else {
                    MessageInfo("修改失败！", "error");
                }
                top.Ext.MessageBox.hide(); //完成后隐藏消息框   
            },
            failure: function (response) {
                top.Ext.MessageBox.hide(); //完成后隐藏消息框   
                top.Ext.Msg.show({ title: "系统错误提示", msg: '操作失败，请联系开发人员!', buttons: Ext.Msg.OK, icon: Ext.MessageBox.ERROR });
            }
        });
    }
};
//保存明细
function afterEdit(e) {
    //判断主表是否已提交
    if (selectNode != null) {
        var FinancialState = selectNode.data['FinancialState'];
        if (FinancialState == -1 || FinancialState == 1 || FinancialState == 2 || FinancialState == 3) {
            top.Ext.Msg.show({ title: "信息提示", msg: "单据已提交，不能进项编辑。", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
        }
        else {
            var record = e.record.data;
            var Count = record.Count; //原始数量
            var Money = parseFloat(record.Money); //原始金额
            var BillNum = record.BillNum; //单据数量
            var BillPrice = parseFloat(record.BillPrice); //单据金额
            var BillMoney = parseFloat(record.BillMoney); //单据款金额
            var Remark = record.Remark; //备注

            //计算核查补充数量
            var CheckNum = record.CheckNum; //核查补充数量

            var BillMoney = ((BillNum * BillPrice).toFixed(2));
            var CheckPrice = ((CheckNum * BillPrice).toFixed(2));
            e.record.data.BillMoney = BillMoney; //单据款金额
            e.record.data.CheckPrice = CheckPrice; //核查补充金额

            var LosstPrice = (BillMoney - Money - CheckPrice).toFixed(2);
            e.record.data.LosstPrice = LosstPrice; //盈亏金额
            top.Ext.MessageBox.wait('', '系统正在执行中,请稍后...'); //显示等待信息框
            Ext.Ajax.request({
                url: '/CACAI/HPurchase/SaveDataDetail?modify=1',
                params: {
                    id: record.Id,
                    BillNum: BillNum,
                    BillPrice: BillPrice,
                    BillMoney: BillMoney,
                    CheckNum: CheckNum,
                    CheckPrice: CheckPrice,
                    LosstPrice: LosstPrice,
                    Remark: Remark
                },
                success: function (response) {
                    var rs = eval('(' + response.responseText + ')');
                    if (rs.success) {
                        e.record.commit();
                        var LossPrice = rs.msg.split(',')[0]; //返回盈亏金额
                        var TotalPrice = rs.msg.split(',')[1]; //供应商金额

                        selectNode.data.LossPrice = parseFloat(LossPrice).toFixed(2);
                        selectNode.data.TotalPrice = parseFloat(TotalPrice).toFixed(2);
                        Ext.getCmp("gg").view.refresh();
                        MessageInfo("修改成功！", "right");
                    } else {
                        MessageInfo("修改失败！", "error");
                    }
                    top.Ext.MessageBox.hide(); //完成后隐藏消息框   
                },
                failure: function (response) {
                    top.Ext.MessageBox.hide(); //完成后隐藏消息框   
                    top.Ext.Msg.show({ title: "系统错误提示", msg: '操作失败，请联系开发人员!', buttons: Ext.Msg.OK, icon: Ext.MessageBox.ERROR });
                }
            });
        }
    }
};

//获取页面所有查询条件
function GetSearch() {
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
    else if (Ext.getCmp("ConSysDir") != null)//字典类型
    {
        if (!Ext.getCmp("ConSysDir").hidden) {
            comContent = Ext.getCmp("ConSysDir").getValue();
        }
    }
    param.conditionField = comName;
    param.condition = comSign;
    param.conditionValue = comContent;

    return urlEncode(param, "", true);
} 

//保存
var SaveDate = function () {
    var grid = Ext.getCmp("gg");
    var formPanel = top.Ext.getCmp("formPanel");
    var win = top.Ext.getCmp("window");
    if (formPanel.getForm().isValid()) {//如果验证通过
        //获取
        var para = {};
        if (win.title == "编辑") {
            //得到选后的数据   
            var rows = grid.getSelectionModel().getSelections();
            para = { Id: rows[0].get("Id") };
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
                    if (url.indexOf('SaveDataDetail') > -1) {
                        Ext.getCmp("rightGrid").getStore().proxy.conn.url = '/CACAI/InComeBill/SearchDataDetail?MainId=' + treeNodeId;
                        Ext.getCmp("rightGrid").getStore().reload();
                    }
                    else if (url.indexOf('SaveDetail') > -1) {
                        var grid = Ext.getCmp("gg");
                        var rows = grid.getSelectionModel().getSelections();
                        var LossPrice = flag.msg.split(',')[0]; //返回盈亏金额
                        var TotalPrice = flag.msg.split(',')[1]; //供应商金额
                        rows[0].data.LossPrice = parseFloat(LossPrice).toFixed(2);
                        rows[0].data.TotalPrice = parseFloat(TotalPrice).toFixed(2);
                        Ext.getCmp("gg").view.refresh();

                        Ext.getCmp("rightGrid").getStore().proxy.conn.url = '/CACAI/InComeBill/SearchDataDetail?MainId=' + treeNodeId;
                        Ext.getCmp("rightGrid").getStore().reload();
                    }
                    else {
                        Ext.getCmp("gg").store.reload();
                    }
                } else {
                    MessageInfo(flag.msg, "error");
                }
            },
            failure: function (form, action) {
                MessageInfo("保存失败！", "error");
            }
        });
    }
};


function CreateSearchWindow(title, key) {

    var form = new top.Ext.form.FormPanel({
        layout: "form", // 整个大的表单是form布局
        id: 'SearchPanel',
        border: false,
        labelWidth: 65,
        autoScroll: true,
        bodyStyle: 'padding:10px',
        labelAlign: "right",
        items: [
             {
                 xtype: "fieldset",
                 title: "主表信息",
                 items: [
                    { fieldLabel: '供应商', xtype: 'textfield', id: 'CusterName', name: 'CusterName', maxLength: 50,
                        maxLengthText: '供应商长度不能超过50个字符', anchor: '95%', enableKeyEvents: true,
                        listeners: {
                            //回车事件
                            specialkey: function (field, e) {
                                if (e.getKey() == Ext.EventObject.ENTER) {
                                    SaveSearch();
                                }
                            }
                        }
                    },
                    { fieldLabel: '采购单号', xtype: 'textfield', id: 'GetNumber', name: 'GetNumber', maxLength: 50,
                        maxLengthText: '采购单号长度不能超过50个字符', anchor: '95%', enableKeyEvents: true,
                        listeners: {
                            //回车事件
                            specialkey: function (field, e) {
                                if (e.getKey() == Ext.EventObject.ENTER) {
                                    SaveSearch();
                                }
                            }
                        }
                    },
                    { fieldLabel: '退货单号', xtype: 'textfield', id: 'OutNumber', name: 'OutNumber', maxLength: 50,
                        maxLengthText: '退货单号长度不能超过50个字符', anchor: '95%', enableKeyEvents: true,
                        listeners: {
                            //回车事件
                            specialkey: function (field, e) {
                                if (e.getKey() == Ext.EventObject.ENTER) {
                                    SaveSearch();
                                }
                            }
                        }
                    },
                     new top.Ext.form.ComboBox({
                         store: new Ext.data.SimpleStore({
                             fields: ['text', 'value'],
                             data: [['已驳回', '-2'], ['未提交', '0'], ['待审核', '1'], ['已审核', '2'], ['已付款', '3']]
                         }),
                         fieldLabel: '财务状态',
                         displayField: 'text',
                         valueField: 'value',
                         mode: 'local',
                         tpl: '<tpl for="."><div class="x-combo-list-item"><span><input type="checkbox" {[values.check?"checked":""]}  value="{[values.value]}" /></span><span >{text}</span></div></tpl>',
                         id: 'FinancialState',
                         name: 'FinancialState',
                         selectOnFocus: true,
                         orceSelection: true,
                         editable: false,
                         triggerAction: 'all',
                         value: '0',
                         anchor: '95%', enableKeyEvents: true,
                         listeners: {
                             //回车事件
                             specialkey: function (field, e) {
                                 if (e.getKey() == Ext.EventObject.ENTER) {
                                     SaveSearch();
                                 }
                             }
                         },
                         onSelect: function (record, index) {
                             if (this.fireEvent('beforeselect', this, record, index) !== false) {
                                 record.set('check', !record.get('check'));
                                 var str = []; //页面显示的值            
                                 var strvalue = []; //传入后台的值  
                                 nextDayShowAjaxData = "";
                                 this.store.each(function (rc) {
                                     if (rc.get('check')) {
                                         str.push(rc.get('text'));
                                     }
                                 });
                                 this.setValue(str.join());
                                 this.value = strvalue.join();
                                 this.fireEvent('select', this, record, index);
                             }
                         }

                     }),
                    { fieldLabel: '主表备注', xtype: 'textfield', id: 'Remark', name: 'Remark', maxLength: 50,
                        maxLengthText: '备注长度不能超过50个字符', anchor: '95%', enableKeyEvents: true,
                        listeners: {
                            //回车事件
                            specialkey: function (field, e) {
                                if (e.getKey() == Ext.EventObject.ENTER) {
                                    SaveSearch();
                                }
                            }
                        }
                    },
                    new top.Ext.form.ComboBox({
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
                        anchor: '95%',
                        enableKeyEvents: true,
                        listeners: {
                            //回车事件
                            specialkey: function (field, e) {
                                if (e.getKey() == Ext.EventObject.ENTER) {
                                    SaveSearch();
                                }
                            }
                        }
                    }),
                             new top.Ext.form.ComboBox({
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
                                 anchor: '95%',
                                 enableKeyEvents: true,
                                 listeners: {
                                     //回车事件
                                     specialkey: function (field, e) {
                                         if (e.getKey() == Ext.EventObject.ENTER) {
                                             SaveSearch();
                                         }
                                     }
                                 }
                             }),
                          {
                              xtype: 'compositefield',
                              fieldLabel: '单据日期',
                              combineErrors: false,
                              items: [
							    {
							        xtype: 'textfield',
							        hidden: true
							    },
                                {
                                    id: 'BegBillDate',
                                    xtype: 'datefield',
                                    width: 95,
                                    emptyText: '开始时间',
                                    format: 'Y-m-d',
                                    vtype: 'daterange',
                                    endDateField: 'EndBillDate', enableKeyEvents: true,
                                    listeners: {
                                        //回车事件
                                        specialkey: function (field, e) {
                                            if (e.getKey() == Ext.EventObject.ENTER) {
                                                SaveSearch();
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
                                                SaveSearch();
                                            }
                                        }
                                    }
                                }
					          ]
                          },
                 {
                     xtype: 'compositefield',
                     fieldLabel: '退货金额',
                     combineErrors: false,
                     items: [
							     {
							         xtype: 'textfield',
							         hidden: true
							     },
                                { xtype: 'numberfield', id: 'BegTotalPrice', name: 'BegTotalPrice', maxLength: 50,
                                    emptyText: '起始金额', width: 95, maxValue: 999999999, enableKeyEvents: true,
                                    listeners: {
                                        //回车事件
                                        specialkey: function (field, e) {
                                            if (e.getKey() == Ext.EventObject.ENTER) {
                                                SaveSearch();
                                            }
                                        }
                                    }
                                },
                                {
                                    xtype: 'tbtext',
                                    text: '~'
                                },
                                { xtype: 'numberfield', id: 'EndTotalPrice', name: 'EndTotalPrice', maxLength: 50,
                                    emptyText: '结束金额', width: 100, maxValue: 999999999, enableKeyEvents: true,
                                    listeners: {
                                        //回车事件
                                        specialkey: function (field, e) {
                                            if (e.getKey() == Ext.EventObject.ENTER) {
                                                SaveSearch();
                                            }
                                        }
                                    }
                                }
					          ]
                 },
                 {
                     xtype: 'compositefield',
                     fieldLabel: '盈亏金额',
                     combineErrors: false,
                     items: [
							     {
							         xtype: 'textfield',
							         hidden: true
							     },
                                { xtype: 'numberfield', id: 'BegLossPrice', name: 'BegLossPrice', maxLength: 50,
                                    emptyText: '起始金额', width: 95, maxValue: 999999999, enableKeyEvents: true,
                                    listeners: {
                                        //回车事件
                                        specialkey: function (field, e) {
                                            if (e.getKey() == Ext.EventObject.ENTER) {
                                                SaveSearch();
                                            }
                                        }
                                    }
                                },
                                {
                                    xtype: 'tbtext',
                                    text: '~'
                                },
                                { xtype: 'numberfield', id: 'EndLossPrice', name: 'EndLossPrice', maxLength: 50,
                                    emptyText: '结束金额', width: 100, maxValue: 999999999, enableKeyEvents: true,
                                    listeners: {
                                        //回车事件
                                        specialkey: function (field, e) {
                                            if (e.getKey() == Ext.EventObject.ENTER) {
                                                SaveSearch();
                                            }
                                        }
                                    }
                                }
					          ]
                            },
                  {
                      xtype: "checkbox",
                      id: "PirceDel",
                      width: 150,
                      fieldLabel: '价格差'
                      //boxLabel: "(单据金额-单价)"
                  },
                     {
                         xtype: "checkbox",
                         id: "CounDel",
                         width: 150,
                         fieldLabel: '数量差'
                         // boxLabel: "(单据数量-数量)"
                     }


               ]
             },
             {
                 xtype: "fieldset",
                 title: "明细信息",
                 items: [
                   { fieldLabel: '商品名称', xtype: 'textfield', id: 'GoodName', name: 'GoodName', maxLength: 50,
                       maxLengthText: '商品名称长度不能超过50个字符', anchor: '95%', enableKeyEvents: true,
                       listeners: {
                           //回车事件
                           specialkey: function (field, e) {
                               if (e.getKey() == Ext.EventObject.ENTER) {
                                   SaveSearch();
                               }
                           }
                       }
                   },
                    { fieldLabel: '明细备注', xtype: 'textfield', id: 'DetailRemark', name: 'DetailRemark', maxLength: 50,
                        maxLengthText: '备注长度不能超过50个字符', anchor: '95%', enableKeyEvents: true,
                        listeners: {
                            //回车事件
                            specialkey: function (field, e) {
                                if (e.getKey() == Ext.EventObject.ENTER) {
                                    SaveSearch();
                                }
                            }
                        }
                    }
               ]
             }

          ]
    });

    return form;
}

//更多查询面板
function BtnMore() {
    if (top.Ext.getCmp("SearchMore")) {
        top.Ext.getCmp("SearchMore").close();
    }
    var form = CreateSearchWindow("组合查询", "");
    var shop = new top.Ext.Window({
        width: 340,
        id: 'SearchMore',
        constrainHeader: true, //视图区域中显示
        height: 600,
        closable: true,
        //shadow: false,
        //stateful: false,
        border: false,
        // modal: true,
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
                            try {
                                SaveSearch();
                            }
                            catch (e) {
                                Ext.MessageBox.alert("提示", "操作错误，请手动关闭窗体，右上角点击退出重新打开！");
                            }
                        }
                    },
                    {
                        text: '重置',
                        iconCls: 'GTP_eraser',
                        handler: function () {
                            try {
                                top.Ext.getCmp("SearchPanel").getForm().reset();
                                //设置财务状态为驳回，未提交
                                top.Ext.getCmp("FinancialState").setValue("已驳回,未提交");
                                top.Ext.getCmp("FinancialState").store.each(function (rc) {
                                    if (rc.get('text') == "已驳回" || rc.get('text') == "未提交") {
                                        rc.set('check', true);
                                    } else {
                                        rc.set('check', false);
                                    }
                                });
                                SaveSearch();
                            } catch (e) {
                                Ext.MessageBox.alert("提示", "操作错误，请手动关闭窗体，右上角点击退出重新打开！");
                            }
                        }
                    },
                    {
                        text: '取消',
                        iconCls: 'GTP_cancel',
                        handler: function () {
                            try {
                                top.Ext.getCmp("SearchMore").close();
                            } catch (e) {
                                Ext.MessageBox.alert("提示", "操作错误，请手动关闭窗体，右上角点击退出重新打开！");
                            }
                        }
                    }]
    })
    shop.show();
    //光标定位
    top.Ext.getCmp('CusterName').focus(false, 200); //11为对应字段的id
    //设置财务状态为驳回，未提交
    top.Ext.getCmp("FinancialState").setValue("已驳回,未提交");
    top.Ext.getCmp("FinancialState").store.each(function (rc) {
        if (rc.get('text') == "已驳回" || rc.get('text') == "未提交") {
            rc.set('check', true);
        } else {
            rc.set('check', false);
        }
    });
}

var param = {};

//获取组合查询面板数据
function GetParam() {
    if (top.Ext.getCmp("SearchPanel")) {
        //var FinancialState = top.Ext.getCmp("FinancialState").getValue(); //财务状态
        var PirceDel = top.Ext.getCmp("PirceDel").getValue(); //价格差
        var CountDel = top.Ext.getCmp("CounDel").getValue(); //数量差

        var strvalue = []; //传入后台的值
        top.Ext.getCmp("FinancialState").store.each(function (rc) {
            if (rc.get('check')) {
                strvalue.push(rc.get('value'));
            }
        });

        param = {
            FinancialState: strvalue.join(','),
            OutNumber: top.Ext.getCmp("OutNumber").getValue().trim(), //退货单号
            GetNumber: top.Ext.getCmp("GetNumber").getValue().trim(), //采购单号
            CusterName: top.Ext.getCmp("CusterName").getValue().trim(), //供应商
            Remark: top.Ext.getCmp("Remark").getValue().trim(), //备注  
            DetailRemark: top.Ext.getCmp("DetailRemark").getValue().trim(),
            GoodName: top.Ext.getCmp("GoodName").getValue().trim(), //商品名称
            BegBillDate: top.Ext.getCmp("BegBillDate").value, //单据日期
            EndBillDate: top.Ext.getCmp("EndBillDate").value, //
            BegTotalPrice: top.Ext.getCmp("BegTotalPrice").getValue(), //供应商金额
            EndTotalPrice: top.Ext.getCmp("EndTotalPrice").getValue(), //
            BegLossPrice: top.Ext.getCmp("BegLossPrice").getValue(), //盈亏金额
            EndLossPrice: top.Ext.getCmp("EndLossPrice").getValue(), //
            PaymentType: top.Ext.getCmp("PaymentType").getValue(), //付款方式
            CheckoutType: top.Ext.getCmp("CheckoutType").getValue(), //结账方式
            PirceDel: PirceDel,
            CountDel: CountDel

        };
    } else {
        param = {
            FinancialState: "-2,0",
            OutNumber: '', //退货单号
            GetNumber: '', //采购单号
            CusterName: '', //供应商
            Remark: '', //备注
            DetailRemark: '',
            GoodName: '', //商品名称
            BegBillDate: '', //单据日期
            EndBillDate: '', //
            BegTotalPrice: '', //供应商金额
            EndTotalPrice: '', //
            BegLossPrice: '', //盈亏金额
            EndLossPrice: '', //
            PaymentType: -1,
            CheckoutType: -1,
            PirceDel: false,
            CountDel: false
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
    else {
        Ext.MessageBox.alert("提示", "请按提示填写数据！");
        return "";
    }
}




















