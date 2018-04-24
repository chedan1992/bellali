//点击组织架构
function treeitemclick(node, e) {
    if (node.attributes.id != "top") {
        treeNodeId = node.attributes.id;
    }
    else {
        treeNodeId = "0";
    }
    Ext.getCmp("gg").getStore().reload();
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
    url = '/SysDirc/SaveData';
    window.show();

    var node = Ext.getCmp("tree").getSelectionModel().getSelectedNode();
    top.Ext.getCmp("Parentname").setValue(node.text);
    top.Ext.getCmp("ParentId").setValue(node.id);
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
        url = '/SysDirc/SaveData?Id=' + key + '&modify=1';
        top.Ext.getCmp("Parentname").setValue("1").hide();
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
                    url: '/SysDirc/DeleteData',
                    params: { id: ids.join(",") },
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
    var node = Ext.getCmp("tree").getSelectionModel().getSelectedNode();
    if (node == null) {
        MessageInfo("请选中左侧分一条记录！", "statusing");
        return false;
    }
    treeNodeId = node.id;

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
        items: [{
            xtype: 'hidden',
            id: 'ParentId',
            name: 'ParentId',
            fieldLabel: 'ParentId',
            allowBlank: false,
            text: node.id
        }, {
            id: 'Parentname',
            name: 'Parentname',
            fieldLabel: "上级栏目",
            allowBlank: false,
            readOnly: true
        },
                {
                    name: 'Name',
                    fieldLabel: "分类名称",
                    allowBlank: false
                },
                {
                    xtype: 'numberfield',
                    name: 'OrderNum',
                    id: 'OrderNum',
                    fieldLabel: '排序',
                    allowBlank: false,
                    value: 0,
                    minValue: 0,
                    maxValue: 9999
                }
        ]
    });


    //窗体
    var win = Window("window", title, form);
    win.width = 360;
    win.height = 250;
    return win;
}
