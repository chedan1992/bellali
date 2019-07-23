//点击组织架构
function treeitemclick(node, e) {
    if (node.attributes.Attribute == 1) {
        treeNodeId = node.attributes.id;
    }
    else {
        treeNodeId = "-1";
    }
    reload();
}

//grid双击
function dbGridClick(grid, rowindex, e) {
    EditDate();
    //判断用户是否有编辑权限
    if (!Ext.getCmp("EditDate")) {
        top.Ext.getCmp("GTP_save").hide();
    }
}

//新增
function AddDate() {
    //判断是否超级管理员添加
    var window = CreateFromWindow("新增");
    var form = top.Ext.getCmp('formPanel');
    form.form.reset();
    url = '/SysGroup/SaveData?Type=' +1; ;
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
        url = '/SysGroup/SaveData?Id=' + key + '&modify=1&Type=' +1;
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
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要删除该记录吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                Ext.Ajax.request({
                    url: '/SysGroup/DeleteData',
                    params: { id: ids.join(","), LoginUserId: getLoginUser().Id },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            //判断是否在grid最后一条的时候删除,如果删除,重新加载
                            Ext.getCmp("gg").store.reload();
                            MessageInfo("删除成功！", "right");
                        } else {
                            MessageInfo(rs.msg, "error");
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

        var para = { LoginUserId: getLoginUser().Id };
        formPanel.getForm().submit({
            waitTitle: '系统提示', //标题
            waitMsg: '正在提交数据,请稍后...', //提示信息
            submitEmptyText: false, //If set to true, the emptyText value will be sent with the form when it is submitted. Defaults to true                                      
            url: url, //记录表单提交的路径
            method: "POST",
            params: para,
            success: function (form, action) {
                //成功后
                var flag = action.result;
                if (flag.success) {
                    Ext.getCmp("gg").store.reload();
                    MessageInfo("保存成功！", "right");
                } else {
                    MessageInfo(flag.msg, "error");
                }
                top.Ext.getCmp('window').close();
            },
            failure: function (form, action) {
                top.Ext.Msg.show({ title: "信息提示", msg: action.result.msg, buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                return false;
            }
        });
    }
}

//创建表单弹框
function CreateFromWindow(title) {
    var lable = '类别名称';
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
                    fieldLabel: lable,
                    allowBlank: false
                }
        ]
    });

    //窗体
    var win = Window("window", title, form);
    win.width = 360;
    win.height =150;
    return win;
}



//引用字典库分类
function GTPBatchimport() {
    //判断是否超级管理员添加
    var creator = getLoginUser(); //获取用户信息
    var CreateCompanyId = '';
    if (creator.Attribute == 1)//系统管理员
    {
        var grid = Ext.getCmp("tr");
        var rows = grid.getSelectionModel().getSelectedNode();
        var CreateCompanyId = '0';
        if (rows) {
            if (rows.attributes.Attribute == 1) {
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

        var grid = Ext.getCmp("tr");
        var rows = grid.getSelectionModel().getSelectedNode();
        CreateCompanyId = rows.attributes.id;
    }
    new top.Ext.Window({
        id: "WinMasterList",
        width: 320,
        height: 470,
        closable: false,
        border: false,
        shadow: false,
        modal: true,
        layout: 'fit',
        plain: true,
        autoDestroy: true,
        closeAction: 'close',
        title: '设备分类选择',
        items: {
            autoScroll: true,
            params: { CId: CreateCompanyId },
            autoLoad: { url: '../../Project/Html/selectDircTree.htm', scripts: true, nocache: true }
        },
        buttons: [
        {
            text: '确定',
            iconCls: 'GTP_submit',
            tooltip: '保存当前的选择',
            handler: function () {
                SaveSelect(CreateCompanyId);
            }
        },
        {
            text: '取消',
            iconCls: 'GTP_cancel',
            tooltip: '取消当前的操作',
            handler: function () {
                top.Ext.getCmp('WinMasterList').close();
            }
        }]
    }).show();

 
}
//确定选择
function SaveSelect(CreateCompanyId) {
    var checkid = '';
    var checkName = '';
    var rt = top.rmtree.getChecked(); //得到所有所选的子节点
    var count = 0;
    if (rt.length > 0) {
        for (var i = 0; i < rt.length; i++) {//除去最顶端的根节点(模块功能分配)
            if (rt[i].getUI().checkbox) {
                if (!rt[i].getUI().checkbox.indeterminate) {
                    checkid +="'"+ rt[i].id +"'"+ ',';
                    checkName += rt[i].text + ',';
                    count++;
                }
            }
        }
    }
    else {
        top.Ext.Msg.show({ title: "信息提示", msg: '请先进行选择', buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
        return false;
    }
    if (count > 10) {
        top.Ext.Msg.show({ title: "信息提示", msg: '最多只能选择10个类别,请重新选择.', buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
        return false;
    }

    checkid = checkid.substring(0, checkid.length - 1);

    Ext.Ajax.request({
        url: '/SysGroup/SaveSelect',
        params: { checkid: checkid, CId: CreateCompanyId },
        success: function (response) {
            var rs = eval('(' + response.responseText + ')');
            if (rs.success) {
                //判断是否在grid最后一条的时候删除,如果删除,重新加载
                Ext.getCmp("gg").store.reload();
                MessageInfo("选择成功！", "right");
            } else {
                MessageInfo(rs.msg, "error");
            }
            top.Ext.getCmp('WinMasterList').close();
        }
    });
   
}