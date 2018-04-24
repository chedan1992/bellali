//渲染类型1：顶部广告，2：底部广告
function formartType(val, rows) {
    switch (val) {
        case 1:
            return '顶部广告';
            break;
        case 2:
            return '底部广告';
            break;
    }
}

//渲染状态 0:待审核，1：审核通过，2：审核不通过)
function formartStatus(val, rows) {
    switch (val) {
        case 0:
            return "<span style='color:orange'>待审核</span>";
            break;
        case 1:
            return "<span style='color:green'>审核通过</span>";
            break;
        case 2:
            return "<span style='color:Red'>审核不通过</span>";
            break;
    }
}
function formartImg(val, rows) {
    return "<img src='" + val + "' style='max-width:150px'/>";
}

function formartPUserName(val, rows, record) {
    if (val == "") {
        return record.data.PLoginName;
    }
    return val;
}
function formartMUserName(val, rows, record) {
    if (val == "") {
        return record.data.MLoginName;
    }
    return val;
}
function formartIsShow(val, rows, record) {
    if (val) {
        return "是";
    }
    return "否";
}

//格式化时间
function formartValTime(val) {

    if (val == null) {
        return null;
    }
    var d = new Date();
    var str = val.toString();
    var str1 = str.replace("/Date(", "");
    var str2 = str1.replace(")/", "");
    var dd = parseInt(str2);
    d.setTime(dd); return d;
};
//审核
function GTPAudit() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length > 0) {
        var key = rows[0].data["Id"];
        if (rows[0].data["Status"] == 0) {
            var win = CreateFromAuditWindow('审核', key);
            win.show();
        } else {
            MessageInfo("已经审核！", "statusing");
        }
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}
//审核表单
function CreateFromAuditWindow(title, key) {
    var CompanyAttribute = [['通过', '1'], ['不通过', '2']];
    var Attribute = new top.Ext.form.ComboBox({
        store: new Ext.data.SimpleStore({
            fields: ['text', 'value'],
            data: CompanyAttribute
        }),
        displayField: 'text',
        valueField: 'value',
        mode: 'local',
        id: 'SelectType',
        selectOnFocus: true,
        orceSelection: true,
        fieldLabel: '审核状态',
        anchor: '90%',
        editable: false,
        triggerAction: 'all',
        allowBlank: false,
        value: '1'
        //        listeners: {
        //            'select': function (val) {
        //                var result = top.Ext.getCmp("SelectType").getValue();
        //                if (result == "1") {//通过
        //                    top.Ext.getCmp("Introduction").hide();
        //                    top.Ext.getCmp("Introduction").allowBlank = true;
        //                }
        //                else {//不通过
        //                    top.Ext.getCmp("Introduction").show();
        //                    top.Ext.getCmp("Introduction").allowBlank = false;
        //                }
        //            }
        //        }
    });
    var form = new top.Ext.FormPanel({
        labelWidth: 65,
        frame: true,
        border: false,
        labelAlign: 'right',
        bodyStyle: 'padding:5px 5px 0',
        id: 'formPanel',
        defaultType: 'textfield',
        items: [
            Attribute,
           {
               fieldLabel: '审批意见',
               xtype: 'textarea',
               id: 'Introduction',
               name: 'Introduction',
               height: 100,
               allowBlank: true,
               anchor: '90%',
               emptyText: '可输入审批意见(70字符内)', ////textfield自己的属性
               maxLength: 70,
               maxLengthText: '描述长度不能超过70个字符'
           }
        ]
    });

    //自定义button
    var button1 = new top.Ext.Button({
        text: '提交',
        iconCls: 'GTP_submit',
        tooltip: '提交',
        handler: function () {
            var formPanel = top.Ext.getCmp("formPanel");
            if (formPanel.getForm().isValid()) {//如果验证通过
                var para = { status: top.Ext.getCmp("SelectType").getValue(), Introduction: top.Ext.getCmp("Introduction").getValue() };
                formPanel.getForm().submit({
                    waitTitle: '系统提示', //标题
                    waitMsg: '正在提交数据,请稍后...', //提示信息
                    submitEmptyText: false,                          
                    url: '/AdAShare/Audit?id=' + key,
                    method: "POST",
                    params: para,
                    success: function (form, action) {
                        //成功后
                        var flag = action.result;
                        if (flag.success) {
                            Ext.getCmp("gg").store.reload();
                            MessageInfo("操作成功！", "right");
                        } else {
                            MessageInfo("操作失败！", "error");
                        }
                        top.Ext.getCmp('window').close();
                    },
                    failure: function (form, action) {
                        MessageInfo("操作失败！", "error");
                    }
                });
            }
        }
    })

    var win = WindowDiy("window", title, form, button1);
    win.width = 400;
    win.height = 250;
    return win;
}
































