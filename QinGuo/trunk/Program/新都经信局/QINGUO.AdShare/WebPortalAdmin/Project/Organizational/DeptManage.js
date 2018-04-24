//查看用户管理设备
function LookAppointed() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var ResponsibleId = rows[0].data["Id"];
        var shop = new top.Ext.Window({
            width: 1000,
            id: 'WinMasterList',
            height: 540,
            closable: false,
            shadow: false,
            stateful: false,
            border: false,
            modal: true,
            layout: 'fit',
            plain: true,
            autoDestroy: true,
            closeAction: 'close',
            title: '部门下的设备列表',
            items: {
                autoScroll: true,
                border: false,
                params: { CID: "", TypeShow:6, ResponsibleId: ResponsibleId },
                autoLoad: { url: '../../Project/Html/SysAppointedGrid.htm', scripts: true, nocache: true }
            },
            buttons: [
                    {
                        text: '取消',
                        iconCls: 'GTP_cancel',
                        tooltip: '取消当前的操作',
                        handler: function () {
                            top.Ext.getCmp("WinMasterList").close();
                        }
                    }]
        }).show();
    }
}

//点击组织架构
function treeitemclick(node, e) {
    if (node.attributes.Attribute ==1) {
        treeNodeId = node.attributes.id;
    }
    else {
        treeNodeId ="-1";
    }
    //刷新之前,重置分页
    reload();
}

//加载部门信息表单
function load(title, key) {
    //表单
    var form = new top.Ext.FormPanel({
        id: "formPanel",
        labelAlign: 'right',
        frame: true,
        labelWidth: 70,
        width: 450,
        autoScroll: true,
        layout: 'column',
        items: [
            {
                        columnWidth: .5,
                        layout: 'form',
                        items: [
                            {
                                name: 'Name',
                                fieldLabel: '<span class="required">*</span>部门名称',
                                xtype: 'textfield',
                                id: 'Name',
                                maxLength: 50,
                                allowBlank: false,
                                emptyText: '填写部门名称',
                                maxLengthText: '名称长度不能超过50个字符',
                                anchor: '90%',
                                enableKeyEvents: true
//                                listeners: {
//                                    'keyup': function (val) {
//                                        if (val.getValue().trim() != "") {
//                                            top.Ext.getCmp("Code").setValue(codeConvert(top.Ext.getCmp("Name").getValue()));
//                                        }
//                                    }
//                                }
                            },
//                             {
//                                 name: 'Code',
//                                 fieldLabel: '部门编码',
//                                 xtype: 'textfield',
//                                 id: 'Code',
//                                 flex: 2,
//                                 maxLength: 50,
//                                 maxLengthText: '编码长度不能超过50个字符',
//                                 anchor: '90%'
//                             },
                              {
                                  name: 'LinkUser',
                                  fieldLabel: '联系人',
                                  xtype: 'textfield',
                                  id: 'LinkUser',
                                  flex: 2,
                                  anchor: '90%'
                              },
                               {
                                   xtype: 'textfield',
                                   name: 'Email',
                                   id: 'Email',
                                   fieldLabel: '邮箱',
                                   vtype: 'email',
                                   anchor: '90%'
                               }
//                               {
//                                   name: 'Order',
//                                   fieldLabel: '排序',
//                                   xtype: 'numberfield',
//                                   id: 'Order',
//                                   flex: 2,
//                                   anchor: '90%',
//                                   value: 0,
//                                   minValue: 0,
//                                   maxValue: 9999
//                               }
                        ]
                    },
            {
                         columnWidth: .5,
                         layout: 'form',
                         items: [
                           {

                               fieldLabel: '部门简称',
                               xtype: 'textfield',
                               id: 'NameTitle',
                               name: 'NameTitle',
                               flex: 2,
                               maxLength: 50,
                               maxLengthText: '编码长度不能超过50个字符',
                               anchor: '90%'
                           },
                            {
                                name: 'Phone',
                                fieldLabel: '联系电话',
                                id: 'Phone',
                                flex: 2,
                                xtype: 'textfield',
                                regex: /^\d+$/,
                                emptyText: '请输入有效的电话号码',
                                anchor: '90%'
                            }
                              
                        ]
                     },
            {
                         columnWidth: 1,
                         layout: 'form',
                         autoHeight: true,
                         items: [
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
                         }]
                     }
        ]
    });
    //自定义窗体
    var button1 = new top.Ext.Button({
        text: '批量创建',
        iconCls: 'GTP_submit',
        tooltip: '批量创建',
        handler: function () {
             CreateFromWindow("批量创建");
        }
    });
    var win = SaveWindowdiy("window", title, form, button1);
    win.width =580;
    win.height =285;
    return win;
}

//批量创建表单弹框
function CreateFromWindow(title) {
    var form = new top.Ext.FormPanel({
        labelWidth: 65,
        frame: true,
        border: false,
        labelAlign: 'right',
        bodyStyle: 'padding:5px 5px 0',
        id: 'formDeptPanel',
        defaultType: 'textfield',
        items: [
                 {
                     xtype: 'panel',
                     height:30,
                     border: false,
                     style: 'margin-bottom:10px;',
                     html: '<div id="msg_info" class="msg_info">多个部门请用 , 号隔开</div>'
                 },
                {
                fieldLabel: '<span class="required">*</span>部门信息',
                xtype: 'textarea',
                id: 'NameList',
                name: 'NameList',
                allowBlank: false,
                height: 150,
                anchor: '95%',
                emptyText: '可输入对角色的描述信息', ////textfield自己的属性
                maxLength: 200,
                maxLengthText: '描述长度不能超过200个字符'
            }
        ]
    });

    //窗体
    var win = new top.Ext.Window({
        id: 'windowDept',
        title: title,
        width: 500,
        height: 300,
        layout: 'fit',
        modal: true,
        shadow: false,
        stateful: false,
        items: form,
        border: false,
        closeAction: 'close',
        buttons: [{
            text: '保存',
            iconCls: 'GTP_save',
            id: 'GTP_save',
            handler: function () {
                var formPanel = top.Ext.getCmp("formDeptPanel");
                if (formPanel.getForm().isValid()) {//如果验证通过
                    var CreateCompanyId = Ext.getCmp("tr").getSelectionModel().getSelectedNode().attributes.id;
                    var linkUrl = '/Organizational/SaveDeptListData?CreateCompanyId=' + CreateCompanyId;
                    formPanel.getForm().submit({
                        waitTitle: '系统提示', //标题
                        waitMsg: '正在提交数据,请稍后...', //提示信息
                        submitEmptyText: false,
                        method: "POST",
                        url: linkUrl,
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
                            top.Ext.getCmp('windowDept').close();
                        },
                        failure: function (form, action) {
                            MessageInfo("保存失败！", "error");
                            return false;
                        }
                    });
                }
            }
        }, {
            text: '取消',
            iconCls: 'GTP_cancel',
            id: 'GTP_cancel',
            handler: function () {
                top.Ext.getCmp("windowDept").close(); //直接销毁
            }
        }]
    });
    win.addListener('beforeshow', function (o) {
        win.center(); //始终居中显示
    });
    win.show();
}

//启用
function EnableUse() {
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        if (rows[0].get("Status") == "1") {
            MessageInfo("该部门已经启用！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要启用该部门吗?', function (e) {
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
            MessageInfo("该部门已经禁用！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '禁用后,部门下面所有的信息都会被禁用.<br/><br/>确认要禁用该部门吗?', function (e) {
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
        if (rows.attributes.Attribute ==1) {
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
    var win = load("新增部门");
    url = '/Organizational/SaveDeptData?CreateCompanyId=' + CreateCompanyId;
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
        url = '/Organizational/SaveDeptData?Id=' + key + '&modify=1';
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
        var confirm = top.Ext.MessageBox.confirm('系统确认', '删除部门,将会清除部门下的人员信息,得重新进行分配,确认要删除该部门吗?', function (e) {
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






