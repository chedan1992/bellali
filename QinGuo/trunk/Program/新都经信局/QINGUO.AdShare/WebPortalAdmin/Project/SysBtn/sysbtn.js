//重写按钮方法
function tbar(PageName) {
    var tb = new Ext.Toolbar();

    tb.add({
        text: '新增',
        tooltip: '新增',
        id: 'GTP_add',
        iconCls: 'GTP_add',
        handler: AddDate
    });

    tb.add('-');

    tb.add({
        text: '编辑',
        tooltip: '编辑',
        id: 'GTP_edit',
        iconCls: 'GTP_edit',
        handler: EditDate
    });

    tb.add('-');

    tb.add({
        text: '删除',
        tooltip: '删除',
        id: 'GTP_delete',
        iconCls: 'GTP_delete',
        handler: DeleteDate
    });

    tb.add('-');

    tb.add({
        text: '刷新',
        tooltip: '刷新',
        id: 'GTP_refresh',
        iconCls: 'GTP_refresh',
        handler: refrushDate
    });
    return tb;
};

//刷新
function refrushDate() {
    Ext.getCmp("gg").store.load({ params: { start: 0, limit: PageSize} });
}

//grid双击
function dbGridClick(grid, rowindex, e) {
    EditDate();
}

//新增
function AddDate() {
    var window = CreateFromWindow("新增");
    var form = top.Ext.getCmp('formPanel');
    form.form.reset();
    url = '/SysBtn/SaveData';
    window.show();
}

//编辑
function EditDate() {
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var window = CreateFromWindow("编辑");
        var form = top.Ext.getCmp('SYSBTNFORM');
        var key = rows[0].get("Id");
        url = '/SysBtn/SaveData?Id=' + key + '&modify=1';
        window.show();
        top.Ext.getCmp("formPanel").form.loadRecord(rows[0]); //再加载数据   
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}

//删除
function DeleteDate() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要删除该按钮吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                Ext.Ajax.request({
                    url: '/SysBtn/DeleteData',
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

//保存
function SaveDate() {
    var formPanel = top.Ext.getCmp("formPanel");
    if (formPanel.getForm().isValid()) {//如果验证通过
        formPanel.getForm().submit({
            waitTitle: '系统提示', //标题
            waitMsg: '正在提交数据,请稍后...', //提示信息
            submitEmptyText: false, //If set to true, the emptyText value will be sent with the form when it is submitted. Defaults to true                                      
            url: url,//记录表单提交的路径
            method: "POST",
            success: function (form, action) {
                //成功后
                var flag = action.result;
                if (flag.success) {
                    Ext.getCmp("gg").store.reload();
                    $('#GTP_edit').tip({ width: '240', status: 'right', content: '保存成功！', dis_time: 1000 }); 
                } else {
                    $('#GTP_edit').tip({ width: '240', status: 'error', content: '保存失败！', dis_time: 1000 });
                }
                top.Ext.getCmp('window').close();
            },
            failure: function (form, action) {
                $('#GTP_edit').tip({ width: '240', status: 'error', content: '保存失败！', dis_time: 1000 });
            }
        });
    }
}



//创建表单弹框
function CreateFromWindow(title) {
    var form = new top.Ext.FormPanel({
        labelWidth: 65,
        frame: true,
        width: 350,
        border: false,
        labelAlign: 'right',
        bodyStyle: 'padding:15px 15px 0',
        id: 'formPanel',
        defaultType: 'textfield',
        defaults: { width: 200 },
        items: [
                {
                    name: 'Name',
                    fieldLabel: '按钮名称',
                    allowBlank: false
                },
                 {
                     name: 'ActionName',
                     fieldLabel: '方法名称',
                     allowBlank: false
                 },
                {
                    name: 'IConName',
                    allowBlank: false,
                    fieldLabel: '图标名称'
                },
                {
                    fieldLabel: '按钮Tip',
                    name: 'NameTip',
                    emptyText: '可输入对按钮的描述信息', ////textfield自己的属性
                    maxLength: 12,
                    maxLengthText: '描述长度不能超过12个字符'
                }
        ]
    });

    //窗体
    var win = Window("window", title, form);
    win.width = 360;
    win.height = 220;
    return win;
}
