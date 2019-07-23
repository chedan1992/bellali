//切换菜单事件
function treeitemclick(node, e) {
    if (node.isLeaf()) {
        treeNodeId = node.id;
        var grid = Ext.getCmp("gg");
        grid.getStore().reload(); 
    }
}

//操作后刷新
function loadRefrush() {
    var node = Ext.getCmp("tree").getSelectionModel().getSelectedNode();
    Ext.getCmp("gg").getStore().proxy.conn.url = '/SysBusinessCircle/SearchShopData?TypeId=' + node.attributes.id;
    Ext.getCmp("gg").getStore().reload();
}

//启用
function EnableUse() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        if (rows[0].data["IsDredge"] == 1) {
            MessageInfo("该商圈已经启用！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要启用该商圈吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["ID"]);
                }
                Ext.Ajax.request({
                    url: '/SysBusinessCircle/EnableUse',
                    params: { id: ids.join(",") },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            loadRefrush();
                            MessageInfo("启用成功！", "right");
                        } else {
                            MessageInfo("启用失败！", "error");
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

//禁用
function DisableUse() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        if (rows[0].data["IsDredge"] == 0) {
            MessageInfo("该商圈已经禁用！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要禁用该商圈吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["ID"]);
                }
                Ext.Ajax.request({
                    url: '/SysBusinessCircle/DisableUse',
                    params: { id: ids.join(",") },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            loadRefrush();
                            MessageInfo("禁用成功！", "right");
                        } else {
                            MessageInfo("禁用失败！", "error");
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
    var node = Ext.getCmp("tree").getSelectionModel().getSelectedNode();
    if (node == null||node.id=='root') {
        MessageInfo("请先选择所在地区！", "statusing");
        return;
    }
    var window = CreateFromWindow("新增");
    var form = top.Ext.getCmp('formPanel');
    form.form.reset();
    url = '/SysBusinessCircle/SaveData?1=1';
    window.show();
}

//编辑
function EditDate() {
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var window = CreateFromWindow("编辑");
        var form = top.Ext.getCmp('formPanel');
        var key = rows[0].get("ID");
        url = '/SysBusinessCircle/SaveData?ID=' + key + '&modify=1';
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
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要删除该地区吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["ID"]);
                }
                Ext.Ajax.request({
                    url: '/SysBusinessCircle/DeleteData',
                    params: { id: ids.join(",") },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            //判断是否在grid最后一条的时候删除,如果删除,重新加载
                            loadRefrush();
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
        var node = Ext.getCmp("tree").getSelectionModel().getSelectedNode();
        url += "&BParentID=" + node.id + "&CityID=" + node.attributes['CityID'];
        formPanel.getForm().submit({
            waitTitle: '系统提示', //标题
            waitMsg: '正在提交数据,请稍后...', //提示信息  
            submitEmptyText: false, //If set to true, the emptyText value will be sent with the form when it is submitted. Defaults to true                  
            url: url, //记录表单提交的路径
            method: "POST",
            success: function (form, action) {
                var flag = action.result; //成功后
                if (flag.success) {
                    loadRefrush();
                    MessageInfo("保存成功！", "right");
                } else {
                    MessageInfo("保存失败！", "right");
                }
                top.Ext.getCmp('window').close();
            },
            failure: function (form, action) {
                MessageInfo("保存失败！", "error");
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
        bodyStyle: 'padding:5px 5px 0',
        id: 'formPanel',
        defaultType: 'textfield',
        defaults: { width: 220 },
        items: [
                {
                    name: 'BName',
                    fieldLabel: '<span class="required">*</span>地区名称',
                    emptyText: '输入地区的名称', ////textfield自己的属性
                    allowBlank: false,
                    maxLength: 20,
                    maxLengthText: '角色名称长度不能超过20个字符'
                }
        ]
    });

    //窗体
    var win = Window("window", title, form);
    win.width = 360;
    win.height = 120;
    return win;
}