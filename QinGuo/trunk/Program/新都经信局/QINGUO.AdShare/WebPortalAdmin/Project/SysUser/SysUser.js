
//渲染用户头像
function renderUserName(value, meta, record, rowIdx, colIdx, store) {
    if (record) {
        var sex = record.data.Sex == 0 ? "GTP_management.png" : "GTP_women.png";
        if (value == "") {
            value = "<span style='color:silver'>(" + record.data.Name + ")</span>";
        }
        return '<span style="vertical-align: middle;"><img style="vertical-align: middle; width:15px; height:15px;border:false" src="../../Resource/css/icons/tree/' + sex + '"/>&nbsp;' + value + '</span>';
    }
    else {
        return value;
    }
}

//启用
function EnableUse() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        if (rows[0].data["Status"] == 1) {
            MessageInfo("该用户已经启用！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要启用该用户吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                Ext.Ajax.request({
                    url: '/SysUser/EnableUse',
                    params: { id: ids.join(",") },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            Ext.getCmp("gg").store.reload();
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
        if (rows[0].data["Status"] == 0) {
            MessageInfo("该用户已经禁用！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '禁用后,该用户下的所有信息将被禁用.<br/><br/>确认要禁用该用户吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                Ext.Ajax.request({
                    url: '/SysUser/DisableUse',
                    params: { id: ids.join(",") },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            Ext.getCmp("gg").store.reload();
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

//密码重置
function ReSetPassword() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要重置该用户的登录密码吗? 重置后密码为:666666', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                Ext.Ajax.request({
                    url: '/SysUser/ReSetPwd',
                    params: { id: ids.join(",") },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            Ext.getCmp("gg").store.reload();
                            MessageInfo("重置成功！", "right");
                        } else {
                            MessageInfo("重置失败！", "error");
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

//删除
function DeleteDate() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要删除该用户吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                Ext.Ajax.request({
                    url: '/SysUser/DeleteData',
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

///验证用户名是否重复
function VilivaName(key, value) {
    var respon = Ext.lib.Ajax.getConnectionObject().conn;
    respon.open("post", "/SysUser/ExitsUser?key=" + key + "&code=" + encodeURIComponent(value.trim()), false);
    respon.send(null);
    var response = Ext.util.JSON.decode(respon.responseText);
    if (response.success) {
        top.Ext.Msg.show({
            title: "信息提示",
            msg: "该登录账号已被注册使用,请重新输入",
            buttons: Ext.Msg.OK,
            icon: Ext.MessageBox.INFO,
            fn: function () {
                top.Ext.getCmp("Name").setValue("");
            }
        });
        return false;
    }
}

//创建表单弹框
function CreateGroupWindow(title) {
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
                    name: 'Name',
                    fieldLabel: '<span class="required">*</span>班级名称',
                    emptyText: '输入分组名称', ////textfield自己的属性
                    allowBlank: false,
                    maxLength: 20,
                    maxLengthText: '分组名称长度不能超过20个字符'
                }
        ]
    });

    //窗体
    var win = Window("window", title, form);
    win.width = 360;
    win.height = 120;
    return win;
}

//新增
function AddDate1() {
    var window = CreateGroupWindow("新增");
    var form = top.Ext.getCmp('formPanel');
    form.form.reset();
    url = '/SysGroup/SaveData?Type=1';
    window.show();
}

//编辑
function EditDate1() {
    var grid = Ext.getCmp("ggwest");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].get("Id");
        var window = CreateGroupWindow("编辑");
        var form = top.Ext.getCmp('formPanel');
        url = '/SysGroup/SaveData?Id=' + key + '&Type=1&modify=1';
        window.show();
        top.Ext.getCmp("formPanel").form.loadRecord(rows[0]); //再加载数据   
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}

//删除
function DeleteDate1() {
    var grid = Ext.getCmp("ggwest");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length > 0) {
        var grid = Ext.getCmp("gg");
        //得到选后的数据
        if (grid.store.data.length > 0) {
            MessageInfo("该班级目前还在使用！", "statusing");
            return false;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要删除该班级吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push("'" + rows[i].data["Id"] + "'");
                }
                Ext.Ajax.request({
                    url: '/SysGroup/DeleteData',
                    params: { id: ids.join(",") },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            //判断是否在grid最后一条的时候删除,如果删除,重新加载
                            Ext.getCmp("ggwest").store.reload();
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
            url: url, //记录表单提交的路径
            method: "POST",
            success: function (form, action) {
                var flag = action.result; //成功后
                if (flag.success) {
                    if (url.indexOf('SysGroup') > 0) {
                        Ext.getCmp("ggwest").store.reload();
                    }
                    else {
                        var grid = Ext.getCmp("ggwest");
                        var rows = grid.getSelectionModel().getSelected();
                        Ext.getCmp("gg").getStore().proxy.conn.url = '/SysMasterUser/SearchData?TypeId=' + rows.data.Id;
                        Ext.getCmp("gg").store.reload();
                    }
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
function CreateFromWindow(title, key) {
    var form = new top.Ext.FormPanel({
        labelWidth: 65,
        frame: true,
        border: false,
        layout: 'fit',
        labelAlign: 'right',
        id: 'formPanel',
        items: [
            {
                xtype: 'fieldset',
                title: '人员信息',
                defaultType: 'textfield',
                autoHeight: true,
                anchor: '100%',
                defaults: { width: 220 },
                items: [
                      {
                          name: 'Nickname',
                          id: 'Nickname',
                          fieldLabel: '<span class="required">*</span>用户名称',
                          emptyText: '填写用户名称', ////textfield自己的属性
                          allowBlank: false,
                          maxLength: 20,
                          maxLengthText: '用户名称长度不能超过20个字符',
                          enableKeyEvents: true,
                          listeners: {
                              'keyup': function (val) {
                                  if (val.getValue().trim() != "") {
                                      var code = codeConvert(top.Ext.getCmp("Nickname").getValue());
                                      top.Ext.getCmp("Name").setValue(code);
                                      return VilivaName(key, code);
                                  }
                              }
                          }
                      },
                    {
                        name: 'Name',
                        id: 'Name',
                        fieldLabel: '<span class="required">*</span>登录账号',
                        emptyText: '填写登录账号', ////textfield自己的属性
                        allowBlank: false,
                        maxLength: 20,
                        maxLengthText: '登录账号长度不能超过20个字符',
                        enableKeyEvents: true,
                        listeners: {
                            'blur': function (val) {
                                if (val.getValue()) {
                                    return VilivaName(key, val.getValue());
                                }
                            }
                        }
                    },
                {
                    name: 'Pwd',
                    id: 'Pwd',
                    fieldLabel: '登录密码',
                    emptyText: '默认登录密码:666666', ////textfield自己的属性
                    maxLength: 20,
                    maxLengthText: '登录密码长度不能超过12个字符'
                },
                  {
                      name: 'Sex',
                      xtype: 'radiogroup',
                      fieldLabel: '性别',
                      id: 'Sex',
                      width: 100,
                      items: [
                     { boxLabel: '男', name: 'Sex', inputValue: 0, checked: true, width: 100, height: 25 },
                     { boxLabel: '女', name: 'Sex', inputValue: 1, width: 100, height: 25 }
                   ]
                  },
               {
                   name: 'Email',
                   id: 'Email',
                   fieldLabel: '邮箱',
                   vtype: 'email'
               },
               {
                   name: 'Tel',
                   id: 'Tel',
                   fieldLabel: '联系电话',
                   xtype: 'textfield',
                   regex: /^\d+$/,
                   emptyText: '请输入有效的电话号码'
               }
            ]
            }
        ]
    });

    //窗体
    var win = Window("window", title, form);
    win.width = 360;
    win.height = 290;
    return win;
}

//新增
function AddDate() {
    var grid = Ext.getCmp("ggwest");
    var rows = grid.getSelectionModel().getSelected();
    if (!rows) {
        MessageInfo("请选中所在班级！", "statusing");
        return false;
    }
    var window = CreateFromWindow("新增", "");
    var form = top.Ext.getCmp('formPanel');
    form.form.reset();
    url = '/SysUser/SaveData?TypeId=' + rows.data.Id;
    top.Ext.getCmp("Pwd").setVisible(true); //显示密码框
    window.show();
}

//编辑
function EditDate() {
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {

        var grid = Ext.getCmp("ggwest");
        var ggrows = grid.getSelectionModel().getSelected();
        if (!ggrows) {
            MessageInfo("请选中所在班级！", "statusing");
            return false;
        }
        var key = rows[0].get("Id");
        var window = CreateFromWindow("编辑", key);
        var form = top.Ext.getCmp('formPanel');

        url = '/SysUser/SaveData?Id=' + key + '&modify=1&TypeId=' + ggrows.data.Id;

        //设置用户性别
        if (rows[0].json.Sex) {
            top.Ext.getCmp("Sex").items[1].checked = true;
        }
        else {
            top.Ext.getCmp("Sex").items[0].checked = true;
        }
        top.Ext.getCmp("Nickname").setValue(rows[0].json.Nickname);
        top.Ext.getCmp("Name").setValue(rows[0].json.Name);
        top.Ext.getCmp("Email").setValue(rows[0].json.Email);
        top.Ext.getCmp("Tel").setValue(rows[0].json.Tel);

        top.Ext.getCmp("Pwd").setVisible(false); //隐藏密码框
        top.Ext.getCmp("Pwd").setValue("");
        window.show();
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}

