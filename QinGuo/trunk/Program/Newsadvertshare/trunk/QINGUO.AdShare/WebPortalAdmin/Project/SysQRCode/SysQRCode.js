//渲染状态
function formartStatus(val, rows) {
    switch (val) {
        case -1:
            return "<span style='color:orange'>未通过</span>";
            break;
        case 0:
            return "<span style='color:#6a6a6a'>未使用</span>";
            break;
        case 1:
            return "<span style='color:green'>已使用</span>";
            break;
    }
}
//删除
function DeleteDate() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length > 0) {
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要删除该二维码吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push("'" + rows[i].data["Id"] + "'");
                }
                Ext.Ajax.request({
                    url: '/SysQRCode/DeleteData',
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

//新增
function BatchImport() {
    var window = CreateFromWindow("批量生成二维码");
    var form = top.Ext.getCmp('formPanel');
    form.form.reset();
    url = '/SysQRCode/SaveData?1=1';
    window.show();
}

//表单保存
function SaveDate() {
    var formPanel = top.Ext.getCmp("formPanel");
    if (formPanel.getForm().isValid()) {//如果验证通过
        formPanel.getForm().submit({
            waitTitle: '系统提示', //标题
            waitMsg: '正在生成二维码,请稍后...', //提示信息  
            submitEmptyText: false,
            url: url, //记录表单提交的路径
            method: "POST",
            success: function (form, action) {
                var flag = action.result; //成功后
                if (flag.success) {
                    top.Ext.getCmp('window').close();
                    MessageInfo("生成成功！", "right");
                } else {
                    MessageInfo(flag.msg, "error");
                }
                Ext.getCmp("gg").store.reload();
            },
            failure: function (form, action) {
                var flag = action.result; //成功后
                MessageInfo(flag.msg, "error");
            }
        });
    }
}

//创建表单弹框
function CreateFromWindow(title) {
    var form = new top.Ext.FormPanel({
        id: "formPanel",
        labelWidth:75,
        fileUpload: true, //有需文件上传的,需填写该属性
        labelAlign: 'right',
        frame: true,
        bodyStyle: 'padding:5px',
        autoScroll: true,
        defaultType: 'textfield',
        items: [
           {
               xtype: 'numberfield',
               name: 'CountNum',
               id: 'CountNum',
               fieldLabel: '生成数量',
               allowBlank: false,
               value: 0,
               anchor: '90%',
               minValue: 0,
               maxValue: 999
           }
        ]
    });

    //窗体
    var win = Window("window", title, form);
    win.width =450;
    win.height =200;
    return win;
}


//grid双击默认进行编辑操作
function Gridrowclick(grid, rowindex, e) {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var Img = rows[0].data["Img"];
        var Status = rows[0].data["Status"];
        var Name = rows[0].data["Name"];
        if (Img != "") {
            Ext.getDom("showImg").setAttribute("src", Img); //右边预览图
        }
        else {
            Ext.getDom("showImg").setAttribute("src", "../../Resource/img/null.jpg"); //无二维码状态
        }
        Ext.getCmp("eastOrganization").setTitle(Name+"二维码");
    }
}


//批量导出
function BatchExport() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length > 0) {
        var confirm = top.Ext.MessageBox.confirm('系统确认', '您确定要导出当前选中的信息吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push("'"+rows[i].data["Id"]+"'");
                }
                var url = '/SysQRCode/ImportOut?IdList=' + ids.join(",") + '&date=' + new Date();
                $(".hideform").attr("action", url);
                $(".hideform").submit();
            }
        });
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}