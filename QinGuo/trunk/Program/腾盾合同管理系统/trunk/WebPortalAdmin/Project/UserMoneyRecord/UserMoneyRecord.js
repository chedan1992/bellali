
//格式化 0:申请中
function formartEnableOrDisable(val, row) {
    if (val == 2)
     return "<span style='color:green'>已完成</span>";
    else if (val ==1) {
        return "<span style='color:Red'>等待处理</span>";
    }
}

//审核
function GTPAudit() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length>0) {
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要支付吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                AddTabPanel("取现支付", 'TakingCashSubmit', '', "/UserMoneyRecord/TakingCashSubmit?id=" + ids.join(","));
                //location.href = "/UserMoneyRecord/TakingCashSubmit?id=" + ids.join(",");
                return;
                //                Ext.Ajax.request({
                //                    url: '/UserMoneyRecord/TakingCashSubmit',
                //                    params: { id: ids.join(",") },
                //                    success: function (response) {
                //                        var rs = eval('(' + response.responseText + ')');
                //                        if (rs.success) {
                //                            Ext.getCmp("gg").store.reload();
                //                            MessageInfo("启用成功！", "right");
                //                        } else {
                //                            MessageInfo("启用失败！", "error");
                //                        }
                //                    }
                //                });
            }
        });
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}

////处理
//function GTPOpinions() {
//    var grid = Ext.getCmp("gg");
//    var rows = grid.getSelectionModel().getSelections();
//    if (rows.length == 1) {
//        if (rows[0].data["Status"] == 1) {
//            MessageInfo("该记录已经处理过的！", "statusing");
//            return;
//        }
////        url = '/UserMoneyRecord/SaveData?Id=' + rows[0].data["Id"] + '&modify=1';
////        var window = CreateFromWindow("处理意见","");
//        //        window.show();
//        var confirm = top.Ext.MessageBox.confirm('系统确认', '处理后代表金额已经打入客户银行卡.<br/><br/>您确定已经打过去了吗?', function (e) {
//            if (e == "yes") {
//                var ids = [];
//                for (var i = 0; i < rows.length; i++) {
//                    ids.push(rows[i].data["Id"]);
//                }
//                Ext.Ajax.request({
//                    url: '/UserMoneyRecord/Opinions',
//                    params: { id: ids.join(",") },
//                    success: function (response) {
//                        var rs = eval('(' + response.responseText + ')');
//                        if (rs.success) {
//                            Ext.getCmp("gg").store.reload();
//                            MessageInfo("处理成功！", "right");
//                        } else {
//                            MessageInfo("处理失败！", "error");
//                        }
//                    }
//                });
//            }
//        });

//    }
//    else {
//        MessageInfo("请选中一条记录！", "statusing");
//    }
//}

////保存
//function SaveDate() {
//    var formPanel = top.Ext.getCmp("formPanel");
//    if (formPanel.getForm().isValid()) {//如果验证通过
//        formPanel.getForm().submit({
//            waitTitle: '系统提示', //标题
//            waitMsg: '正在提交数据,请稍后...', //提示信息  
//            submitEmptyText: false, //If set to true, the emptyText value will be sent with the form when it is submitted. Defaults to true                  
//            url: url, //记录表单提交的路径
//            method: "POST",
//            success: function (form, action) {
//                var flag = action.result; //成功后
//                if (flag.success) {
//                    Ext.getCmp("gg").store.reload();
//                } else {
//                    MessageInfo("保存失败！", "right");
//                }
//                top.Ext.getCmp('window').close();
//            },
//            failure: function (form, action) {
//                MessageInfo("保存失败！", "error");
//            }
//        });
//    }
//}

////创建表单弹框
//function CreateFromWindow(title, key) {
//    var form = new top.Ext.FormPanel({
//        labelWidth: 65,
//        frame: true,
//        border: false,
//        layout: 'fit',
//        labelAlign: 'right',
//        id: 'formPanel',
//        items: [
//          {
//               fieldLabel: '备注信息',
//               xtype: 'textarea',
//               id: 'Remark',
//               name: 'Remark',
//               height: 150,
//               allowBlank: false,
//               anchor: '95%',
//               emptyText: '可输入对处理的备注信息', ////textfield自己的属性
//               maxLength: 200,
//               maxLengthText: '备注长度不能超过200个字符'
//           }
//        ]
//    });

//    //窗体
//    var win = Window("window", title, form);
//    win.width =400;
//    win.height = 200;
//    return win;
//}

//删除
function DeleteDate() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要删除该记录吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                Ext.Ajax.request({
                    url: '/UserMoneyRecord/DeleteData',
                    params: { id: ids.join(",") },
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

