//点击组织架构
function treeitemclick(node, e) {
    if (node.attributes.Attribute ==5) {//1:集团 2:公司 5:岗位
        treeNodeId = node.attributes.id;
    }
    else  if (node.attributes.Attribute ==2) {
        treeNodeId = "-1";
    }
    Ext.getCmp("gg").getStore().reload();
}
//加载部门信息表单
function load(title, key) {
    //表单
    var form = new top.Ext.FormPanel({
        id: "formPanel",
        labelAlign: 'right',
        frame: true,
        labelWidth: 70,
        width: 400,
        autoScroll: true,
        items: [

                     {
                         name: 'Name',
                         fieldLabel: '<span class="required">*</span>职位名称',
                         xtype: 'textfield',
                         id: 'Name',
                         maxLength: 50,
                         allowBlank: false,
                         emptyText: '填写职位名称',
                         maxLengthText: '名称长度不能超过50个字符',
                         anchor: '95%',
                         enableKeyEvents: true,
                         listeners: {
                             'keyup': function (val) {
                                 if (val.getValue().trim() != "") {
                                     top.Ext.getCmp("Code").setValue(codeConvert(top.Ext.getCmp("Name").getValue()));
                                 }
                             }
                         }
                     },
                             {
                                 name: 'Code',
                                 fieldLabel: '职位编码',
                                 xtype: 'textfield',
                                 id: 'Code',
                                 flex: 2,
                                 maxLength: 50,
                                 maxLengthText: '编码长度不能超过50个字符',
                                 anchor: '95%'
                             },
                               {
                                   name: 'Order',
                                   fieldLabel: '排序',
                                   xtype: 'numberfield',
                                   id: 'Order',
                                   flex: 2,
                                   anchor: '95%',
                                   value: 0,
                                   minValue: 0,
                                   maxValue: 9999
                               },
                               {
                                   fieldLabel: '说明',
                                   xtype: 'textarea',
                                   id: 'Introduction',
                                   name: 'Introduction',
                                   height: 80,
                                   emptyText: '可输入对部门的介绍信息', ////textfield自己的属性
                                   anchor: '95%',
                                   maxLength: 200,
                                   maxLengthText: '描述长度不能超过200个字符'
                               }

        ]
    });
    //窗体
    var win = Window("window", title, form);
    win.width = 580;
    win.height = 285;
    return win;
}

//启用
function EnableUse() {
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        if (rows[0].get("Status") == "1") {
            MessageInfo("该职位已经启用！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要启用该职位吗?', function (e) {
            if (e == "yes") {
                var key = rows[0].get("Id");
                Ext.Ajax.request({
                    url: '/Organizational/EnableUse',
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
            MessageInfo("该部门已经职位！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '禁用后,职位下面所有的信息都会被禁用.<br/><br/>确认要禁用该职位吗?', function (e) {
            if (e == "yes") {
                var key = rows[0].get("Id");
                Ext.Ajax.request({
                    url: '/Organizational/DisableUse',
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
    var grid = Ext.getCmp("tr");
    var rows = grid.getSelectionModel().getSelectedNode();
    var CreateCompanyId = '0';
    if (rows) {
        if (rows.attributes.Attribute ==5) {
            CreateCompanyId = rows.attributes.id;
        }
        else {
            MessageInfo("请先选择部门！", "statusing");
            return false;
        }
    }
    else {
        MessageInfo("请先选择部门！", "statusing");
        return false;
    }
    var win = load("新增职位");
    url = '/Organizational/SavePostData?CreateCompanyId=' + CreateCompanyId;
    win.show();
}

//编辑
function EditDate() {
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].get("Id");
        var window = load("编辑", key);
        var form = top.Ext.getCmp('formPanel');
        form.form.loadRecord(rows[0]); //再加载数据   
        url = '/Organizational/SavePostData?Id=' + key + '&modify=1';
        window.show();
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
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要删除该职位吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                Ext.Ajax.request({
                    url: '/Organizational/DeleteData',
                    params: { id: ids.join(",") },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            MessageInfo("删除成功！", "right");
                            Ext.getCmp("gg").store.reload();
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
            submitEmptyText: false,
            method: "POST",
            url: url,
            // params: para,
            success: function (form, action) {
                //成功后
                var flag = action.result;
                if (flag.success) {
                    MessageInfo("保存成功！", "right");
                    Ext.getCmp("gg").store.reload();
                } else {
                    MessageInfo("保存失败！", "error");
                    return false;
                }
                top.Ext.getCmp('window').close();
            },
            failure: function (form, action) {
                MessageInfo("保存失败！", "error");
                return false;
            }
        });
    }
}

