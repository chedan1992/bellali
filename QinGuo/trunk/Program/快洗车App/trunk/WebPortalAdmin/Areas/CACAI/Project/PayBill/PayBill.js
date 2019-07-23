//grid单击默认进行编辑操作
function gridrowclick(grid, rowindex, e) {
    var grid = Ext.getCmp("gg");
    var SourceGrid = Ext.getCmp("rightGrid");
    if (grid.store.data.length > 0) {
        var rows = grid.getSelectionModel().getSelections();
        if (rows.length == 1) {
            treeNodeId = rows[0].data["Id"];
            SourceGrid.getStore().proxy.conn.url = '/CACAI/Finance/SearchDataDetail?MainId=' + treeNodeId;
            SourceGrid.getStore().reload();
        }
        else {
            SourceGrid.getStore().reload();
        }
    }
    else {
        SourceGrid.getStore().proxy.conn.url = '/CACAI/Finance/SearchDataDetail?MainId=' + -1;
        SourceGrid.getStore().reload();
    }
}
function dbGridClick(row, rowindex) {
    ShowAttach();
}
function ShowAttach2() {
    ShowAttach();
}


//渲染支付状态
function renderPayStatus(val, metadata, record, rowIndex, columnIndex, store) {
    switch (val) {
        case 0:
            return "<span style='color:silver'>未支付</span>";
            break;
        case 1:
            return "<span style='color:green'>已支付</span>";
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
        case -1:
            return "<span style='color:red'>已驳回</span>";
            break;
        case 0:
            return "未提交";
            break;
        case 1:
            return "<span style='color:green'>已提交</span>";
            break;
        default:
            return "未提交";
            break;
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
            width:500,
            height: 350,
            // permitted_extensions:['JPG','jpg','jpeg','JPEG','GIF','gif','xls','XLS'],      
            proxyDrag: true,
            resizable: true,
            constraintoviewport: true,
            title: '文件上传',
            url: '/SysFileAttach/FileUpload', //上传地址
            post_var_name: 'mms',
            base_params: { hehe: "wayfoon", ModelCode: 'InCome', KeyId: rows[0].get("Id") },
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
}

//文件上传失败后的回调函数     
onUploadFailed = function (dialog, filename, resp_data, record) {
    alert(resp_data.message);
}

//文件上传完成后的回调函数
onUploadComplete = function (dialog) {
    // dialog.hide();
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
        var PaymentAmmount = rows[0].get("PaymentAmmount");
        var title = '凭证预览' + '  ( <span style="color:red"> 供应商：' + CusterName + ',付款金额：' + PaymentAmmount + '</span>)';
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
            width:850,
            id: 'ImgShowWin',
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
                                        top.Ext.getCmp("ImgShowWin").close();
                                    }
                                }]
        }).show();

    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
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
        var key = rows[0].get("Id");
        var CusterName=rows[0].get("CusterName");
        var TotalPrice = rows[0].get("TotalPrice");
        var shop = new top.Ext.Window({
            width: 880,
            id: 'FileAttachWin',
            height: 500,
            closable: false,
            buttonAlign: 'right', //左边
            shadow: false,
            stateful: false,
            border: false,
            modal: true,
            //draggable : false,//禁止拖动
            resizable : false,//禁止缩放
            layout: 'fit',
            plain: true,
            autoDestroy: true,
            closeAction: 'close',
            title: '凭证管理' + '  ( <span style="color:red"> 供应商：' + CusterName + ',供应商金额：' + TotalPrice + '</span>)',
            items: {
                autoScroll: true,
                border: false,
                params: { KeyId: key, ModelCode: "InCome", ShowBtn: ShowBtn },
                autoLoad: { url: '../../Project/Html/FileAttach.htm', scripts: true, nocache: true }
            },
            buttons: [
//                        {
//                
//                                xtype: 'tbtext',
//                                id: 'online',
//                                text: '浏览模式'
//                            },
//                        {
//                               text: '',
//                               iconCls: 'GTP_solcard',
//                               tooltip: '卡片视图',
//                               pressed: true,
//                               handler: function () {
//                                   top.Ext.getCmp("FileAttachWin").close();
//                               }
//                           },
//                        {
//                            text: '',
//                            iconCls: 'GTP_sollist',
//                            tooltip: '列表视图',
//                            handler: function () {
//                                top.Ext.getCmp("FileAttachWin").close();
//                            }
//                        },
//                        new Ext.Toolbar.Fill(),
                        {
                             text: '取消',
                             iconCls: 'GTP_cancel',
                             tooltip: '取消当前的操作',
                             handler: function () {
                                 top.Ext.getCmp("FileAttachWin").close();
                             }
                         }

                      ]
        }).show();
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
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
//展示图片
function showImg(val) {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].data["Id"];
        var Title = rows[0].data["Name"];
        var QRCode = rows[0].data["QRCode"];//二维码编码
        var shop = new top.Ext.Window({
            width: 450,
            id: 'shoper',
            height: 350,
            closable: false,
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




/**************************方法************************************/
//主表选中事件
function MainCellclick() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length >= 1) {
        //进项统计
        var ReturnAmount = 0;
        var ProfitTotal = 0;
        var PaymentAmmount = 0;
        var LossPrice = 0;
        for (var i = 0; i < rows.length; i++) {
            ReturnAmount += parseFloat(rows[i].get("ReturnAmount")); //供应商金额
            ProfitTotal += parseFloat(rows[i].get("ProfitTotal")); //退款金额
            PaymentAmmount += parseFloat(rows[i].get("PaymentAmmount")); //付款金额
            LossPrice += parseFloat(rows[i].get("LossPrice"));
        }
        document.getElementById("chart1").innerHTML = ReturnAmount;
        document.getElementById("chart2").innerHTML = ProfitTotal;
        document.getElementById("chart33").innerHTML = PaymentAmmount;
        document.getElementById("chart4").innerHTML = LossPrice;
    }
    else {
        document.getElementById("chart1").innerHTML = 0;
        document.getElementById("chart2").innerHTML = 0;
        document.getElementById("chart33").innerHTML = 0;
        document.getElementById("chart4").innerHTML = 0;
    }
}


//确认支付
function GTP_audit() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length >= 1) {
        var ids = [];
        for (var i = 0; i < rows.length; i++) {
            ids.push("'" + rows[i].data["Id"] + "'");
            var FinancialState = rows[i].get("PayStatus");
            var InNumber = rows[i].get("InNumber");
            if (FinancialState != 0) {
                top.Ext.Msg.show({ title: "信息提示", msg: "入库单号" + InNumber + "已经支付过了,不能再操作!", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                return false;
            }
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认支付选中的记录信息吗?', function (e) {
            if (e == "yes") {
                top.Ext.MessageBox.wait('', '系统正在执行中,请稍后...'); //显示等待信息框   
                Ext.Ajax.request({
                    url: '/CACAI/Finance/SureGive',
                    params: { id: ids.join(","), Status: 1 },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            MessageInfo("支付成功！", "right");
                            Ext.getCmp("gg").store.reload();
                        } else {
                            MessageInfo("支付失败！", "error");
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

//取消支付
function CancelPay() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length >= 1) {
        var ids = [];
        for (var i = 0; i < rows.length; i++) {
            ids.push("'" + rows[i].data["Id"] + "'");
            var FinancialState = rows[i].get("PayStatus");
            var InNumber = rows[i].get("InNumber");
            if (FinancialState == 0) {
                top.Ext.Msg.show({ title: "信息提示", msg: "入库单号" + InNumber + "还未支付，请重新再操作!", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                return false;
            }
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认取消支付选中的记录信息吗?', function (e) {
            if (e == "yes") {
                top.Ext.MessageBox.wait('', '系统正在执行中,请稍后...'); //显示等待信息框   
                Ext.Ajax.request({
                    url: '/CACAI/Finance/SureGive',
                    params: { id: ids.join(","),Status:0 },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            MessageInfo("取消成功！", "right");
                            Ext.getCmp("gg").store.reload();
                        } else {
                            MessageInfo("取消失败！", "error");
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
                    url: '/CACAI/Finance/ImportOut',
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

//                var url = '/CACAI/Finance/ImportOut?IdList=' + ids.join(",") + '&date=' + new Date();
//                $(".hideform").attr("action", url);
//                $(".hideform").submit();
            }
        });
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}



//驳回
function GTP_cancelagent() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length >= 1) {
        var flag = true;
        var ids = [];
        for (var i = 0; i < rows.length; i++) {
            ids.push("'" + rows[i].data["Id"] + "'");
            var FinancialState = rows[i].get("PayStatus");
            var InNumber = rows[i].get("InNumber");
            if (FinancialState != 0) {
                flag = false;
                top.Ext.Msg.show({ title: "信息提示", msg: "入库单号" + InNumber + "已经支付过了,不能再操作!", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                return false;
            }
        }
        if (flag == true) {
            var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要驳回选中的记录信息吗?', function (e) {
                if (e == "yes") {
                    top.Ext.MessageBox.wait('', '系统正在执行中,请稍后...'); //显示等待信息框   
                    Ext.Ajax.request({
                        url: '/CACAI/Finance/AuditBack',
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
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}


//保存主表
function afterEditMain(e) {
    var record = e.record.data;
    var Remark = record.FinanceRemark; //备注
    if (Remark != "") {
        Ext.Ajax.request({
            url: '/CACAI/Finance/SaveData',
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
            }
        });
    }
};

//创建查询面板
function CreateSearchWindow(title, key) {
    var form = new top.Ext.form.FormPanel({
        layout: "form", // 整个大的表单是form布局
        id: 'SearchPanel',
        border: false,
        labelWidth: 70,
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
                         { fieldLabel: '入库单号', xtype: 'textfield', id: 'InNumber', name: 'InNumber', maxLength: 50,
                             maxLengthText: '入库仓号长度不能超过50个字符', anchor: '90%'
                         }
                             ]
                     },
                     {
                         columnWidth: .5, // 该列有整行中所占百分比
                         border: false,
                         layout: "form", // 从上往下的布局
                         items: [
                        { fieldLabel: '供应商', xtype: 'textfield', id: 'CusterName', name: 'CusterName', maxLength: 50,
                            maxLengthText: '供应商长度不能超过50个字符', anchor: '90%'
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
                                 new top.Ext.form.ComboBox({
                                     store: new Ext.data.SimpleStore({
                                         fields: ['text', 'value'],
                                         data: [['全部', '-1'], ['未支付', '0'], ['已支付', '1']]
                                     }),
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
                                     anchor: '90%'
                                 })
                             ]
                     },
                     {
                         columnWidth: .5, // 该列有整行中所占百分比
                         border: false,
                         layout: "form", // 从上往下的布局
                         items: [
                                  { fieldLabel: '财务备注', xtype: 'textfield', id: 'FinanceRemark', name: 'FinanceRemark', maxLength: 50,
                                      maxLengthText: '备注长度不能超过50个字符', anchor: '90%'
                                  }  
                             ]
                     }
                     ]
               }
               ]
             },
             {
                 xtype: "fieldset",
                 title: "组合信息",
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
                             xtype: 'compositefield',
                             fieldLabel: '付款金额',
                             combineErrors: false,
                             items: [
							     {
							         xtype: 'textfield',
							         hidden: true
							     },
                                { xtype: 'numberfield', id: 'BegPaymentAmmount', name: 'BegPaymentAmmount', maxLength: 50,
                                    emptyText: '起始金额', width: 100, minValue: 0, maxValue: 999999999
                                },
                                {
                                    xtype: 'tbtext',
                                    text: '~'
                                },
                                { xtype: 'numberfield', id: 'EndPaymentAmmount', name: 'EndPaymentAmmount', maxLength: 50,
                                    emptyText: '结束金额', width: 100, minValue: 0, maxValue: 999999999
                                }
					          ]
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
        width: 710,
        id: 'SearchMore',
        height: 450,
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
                        text: '重置',
                        iconCls: 'GTP_eraser',
                        handler: function () {
                            top.Ext.getCmp("SearchPanel").getForm().reset();
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

//获取组合查询面板数据
function GetParam() {
    if (Ext.getCmp("formPanel")) {
        var FinancialState =Ext.getCmp("FinancialState").getValue(); //支付状态
        param = {
            FinancialState: FinancialState,
            PaymentType: Ext.getCmp("PaymentType").getValue(), //付款方式
            CheckoutType: Ext.getCmp("CheckoutType").getValue(), //结账方式
            //InNumber: top.Ext.getCmp("InNumber").getValue().trim(), //入库单号
            CusterName:Ext.getCmp("CusterName").getValue().trim(), //供应商
            //FinanceRemark: top.Ext.getCmp("FinanceRemark").getValue().trim(), //备注
            BegBillDate: Ext.getCmp("BegBillDate").value, //提交时间
            EndBillDate: Ext.getCmp("EndBillDate").value

        };
    }
    else {
        param = {
            FinancialState: -1,
            PaymentType: -1,
            CheckoutType:-1,
            //InNumber: '', //入库单号
            CusterName: '', //供应商
            //FinanceRemark: '', //备注
            BegBillDate: '', //提交时间
            EndBillDate: ''
        }

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


















