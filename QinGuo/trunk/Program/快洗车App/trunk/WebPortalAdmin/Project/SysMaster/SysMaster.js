//点击组织架构
function treeitemclick(node, e) {
    if (node.attributes.Attribute == 5) {//1:集团 2:公司 5:岗位
        treeNodeId = node.attributes.id;
    }
    else if (node.attributes.Attribute == 2) {
        treeNodeId = "-1";
    }
    Ext.getCmp("gg").getStore().reload();
}

//渲染用户头像
function renderUserName(value, meta, record, rowIdx, colIdx, store) {
    if (record) {
        var sex = record.data.Sex == 0 ? "GTP_management.png" : "GTP_women.png";
        return '<span style="vertical-align: middle;"><img style="vertical-align: middle; width:15px; height:15px;border:false" src="../../Resource/css/icons/tree/' + sex + '"/>&nbsp;' + value + '</span>';
    }
    else{
    return value;
    }
}

//设置角色列表信息
function CreateRole(masterId) {
    var items = top.Ext.getCmp('onlineRole').items;
    if (items) {
        items = top.Ext.getCmp('onlineRole').items.items;
        var length = items.length;
        for (var i = 0; i < length; i++) {
            top.Ext.getCmp('onlineRole').remove(items[0]);
        }
    }

    //同步查询online
    var respon = Ext.lib.Ajax.getConnectionObject().conn;
    respon.open("post", "/SysMaster/GetRoleList?masterId=" + masterId, false);
    respon.send(null);
    var items = [];
    var labelCatObj = Ext.util.JSON.decode(respon.responseText);
    for (i = 0; i < labelCatObj.length; i++) {
        var labelCat = labelCatObj[i];
        var chk = new top.Ext.form.Checkbox({
            boxLabel: labelCat.Name + "&nbsp;",
            text: labelCat.Name,
            width:120,
            name: labelCat.Id,
            checked:labelCat.checked
        });
        top.Ext.getCmp("onlineRole").items.add(chk);
    }
}

//设置角色列表信息
function CreateHightRole() {
    var items = top.Ext.getCmp('onlineRole').items;
    if (items) {
        items = top.Ext.getCmp('onlineRole').items.items;
        var length = items.length;
        for (var i = 0; i < length; i++) {
            top.Ext.getCmp('onlineRole').remove(items[0]);
        }
    }

    var chk = new top.Ext.form.Checkbox({
        boxLabel: '所有权限',
        name: '-1',
        checked: true,
        disabled: true
    });
    top.Ext.getCmp("onlineRole").items.add(chk);
}

////组织架构新增
//function AddDate() {
//    var grid = Ext.getCmp("tr");
//    var rows = grid.getSelectionModel().getSelectedNode();
//    var Cid = '0';
//    if (rows) {
//        if (rows.attributes.Attribute == 5) {
//            Cid = rows.attributes.id;
//        }
//        else {
//            MessageInfo("请先选择部门！", "statusing");
//            return false;
//        }
//    }
//    else {
//        MessageInfo("请先选择部门！", "statusing");
//        return false;
//    }

//    var window = CreateFromWindow("新增","");
//    CreateRole("");
//    var form = top.Ext.getCmp('formPanel');
//    form.form.reset();
//    top.Ext.getCmp("Pwd").setVisible(true); //显示密码框
//    url = '/SysMaster/SaveData?Cid=' + Cid;
//    window.show();
//}
//新增
function AddDate() {
   
    var Cid = '';
    var window = CreateFromWindow("新增", "");
    CreateRole("");
    var form = top.Ext.getCmp('formPanel');
    form.form.reset();
    top.Ext.getCmp("Pwd").setVisible(true); //显示密码框
    url = '/SysMaster/SaveData?Cid=' + Cid;
    window.show();
}

//grid双击
function dbGridClick(grid, rowindex, e) {
    EditDate();
    //判断用户是否有编辑权限
    if (!Ext.getCmp("EditDate")) {
        top.Ext.getCmp("GTP_save").hide();
    }
}

//编辑
function EditDate() {
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].get("Id");
        var window = CreateFromWindow("编辑", key);
        var form = top.Ext.getCmp('formPanel');
        form.form.loadRecord(rows[0]); //再加载数据   
        url = '/SysMaster/SaveData?Id=' + key + '&modify=1';

        if (rows[0].get("IsMain"))
        {
            CreateHightRole();
        }
        else {
            CreateRole(key);
        }
        top.Ext.getCmp("Pwd").setVisible(false); //隐藏密码框
        top.Ext.getCmp("Pwd").setValue("");
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
    if (rows.length >= 1) {
        var confirm =top.Ext.MessageBox.confirm('系统确认', '确认要删除该用户吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push("'"+rows[i].data["Id"]+"'");
                }
                Ext.Ajax.request({
                    url: '/SysMaster/DeleteData',
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
        var ids = [];
        var idsName = [];
        Ext.each(top.Ext.getCmp("onlineRole").items.items, function (item) {
            if (item.getXType() == 'checkbox') {
                if (item.checked) {
                    if (item.name != "-1")
                    {
                        ids.push(item.name);
                        idsName.push(item.text);
                    }
                }
            }
        });
        if (ids.length > 0)
        {
            url += "&roleId=" + ids.join(",") + "&roleName=" + encodeURIComponent(idsName.join(",")); //选中的角色编号
        }

        formPanel.getForm().submit({
            waitTitle: '系统提示', //标题
            waitMsg: '正在提交数据,请稍后...', //提示信息
            submitEmptyText: false, //If set to true, the emptyText value will be sent with the form when it is submitted. Defaults to true                                   
            url: url, //记录表单提交的路径
            method: "POST",
            success: function (form, action) {
                //成功后
                var flag = action.result;
                if (flag.success) {
                    Ext.getCmp("gg").store.reload();
                    MessageInfo("保存成功！", "right");
                } else {
                    MessageInfo("保存失败！", "error");
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
function CreateFromWindow(title ,key) {
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
                items: [
                      {
                          name: 'UserName',
                          id: 'UserName',
                          fieldLabel: '<span class="required">*</span>用户名称',
                          emptyText: '填写用户名称', ////textfield自己的属性
                          allowBlank: false,
                          maxLength: 20,
                          anchor: '90%',
                          maxLengthText: '用户名称长度不能超过20个字符',
                          enableKeyEvents: true,
                          listeners: {
                              'keyup': function (val) {
                                  if (val.getValue().trim() != "") {
                                      var code = codeConvert(top.Ext.getCmp("UserName").getValue());
                                      top.Ext.getCmp("LoginName").setValue(code);
                                      return VilivaName(key, code);
                                  }
                              }
                          }

                      },
                      {
                          name: 'LoginName',
                          id: 'LoginName',
                          fieldLabel: '<span class="required">*</span>登录账号',
                          emptyText: '填写登录账号', ////textfield自己的属性
                          allowBlank: false,
                          maxLength: 20,
                          anchor: '90%',
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
                    anchor: '90%',
                    maxLengthText: '登录密码长度不能超过12个字符'
                },
               {
                   name: 'Sex',
                   xtype: 'radiogroup',
                   fieldLabel: '性别',
                   width: 100,
                   items: [
                     { boxLabel: '男', name: 'Sex', inputValue: 0, checked: true, width: 100, height: 25 },
                     { boxLabel: '女', name: 'Sex', inputValue: 1, width: 100, height: 25 }
                   ]
               },
               {
                   name: 'Email',
                   fieldLabel: '邮箱',
                   anchor: '90%',
                   vtype: 'email'
               },
               {
                   name: 'Phone',
                   fieldLabel: '联系电话',
                   xtype: 'textfield',
                   anchor: '90%',
                   regex: /^\d+$/,
                   emptyText: '请输入有效的电话号码'
               }
            ]
            },
            {
                xtype: 'fieldset',
                title: '角色权限',
                anchor: '90%',
                items: [
                {
                    xtype: 'panel',
                    layout: "column",
                    fieldLabel: "所属角色",
                    isFormField: true,
                    id: 'onlineRole',
                    items: []
                }
                    ]

            }
        ]
    });

    //窗体
    var win = Window("window", title, form);
    win.width =400;
    win.height =450;
    return win;
}

///验证用户名是否重复
function VilivaName(key,value) {
    var respon = Ext.lib.Ajax.getConnectionObject().conn;
    respon.open("post", "/SysMaster/ExitsMaster?key=" + key + "&code=" + encodeURIComponent(value.trim()), false);
    respon.send(null);
    var response = Ext.util.JSON.decode(respon.responseText);
    if (response.success) {
        top.Ext.Msg.show({
            title: "信息提示",
            msg: "该登录账号已被注册使用,请重新输入",
            buttons: Ext.Msg.OK,
            icon: Ext.MessageBox.INFO,
            fn: function () {
                top.Ext.getCmp("LoginName").setValue("");
            }
        });
        return false;
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
                    url: '/SysMaster/EnableUse',
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
                    url: '/SysMaster/DisableUse',
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
                    url: '/Home/ReSetPwd',
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

//批量导入
function ImportDate() {
    if (parent) {
        var grid = Ext.getCmp("tr");
        var rows = grid.getSelectionModel().getSelectedNode();
        var Cid = '0';
        if (rows) {
            if (rows.attributes.Attribute == 5) {
                Cid = rows.attributes.id;
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

        var tabId = parent.GetActiveTabId(); //公共变量
        parent.AddTabPanel('批量导入', 'TestExport', tabId, '/SysMaster/Export?Cid=' + Cid);
    }
}