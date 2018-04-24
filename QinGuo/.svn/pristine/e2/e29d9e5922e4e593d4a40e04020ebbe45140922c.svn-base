//启用
function EnableUse() {
    var grid = Ext.getCmp("tg");
    var rows = grid.getSelectionModel().getSelectedNode();
    if (rows) {
        if (rows.attributes.IsSystem) {
            MessageInfo("该分类为系统初始化,不能操作！", "statusing");
            return;
        }
        if (rows.attributes.Status == 1) {
            MessageInfo("该分类已经启用！", "statusing");
            return;
        }
      
        var confirm = top.Ext.MessageBox.confirm('系统确认', '启用后,所属子类与父类也将被启用,您确认要启用该分类吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                ids.push(rows.attributes.Id);
                Ext.Ajax.request({
                    url: '/ShopCategory/EnableUse',
                    params: { id: ids.join(",") },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            Ext.getCmp("tg").root.reload();
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
    var grid = Ext.getCmp("tg");
    var rows = grid.getSelectionModel().getSelectedNode();
    if (rows) {
        if (rows.attributes.IsSystem) {
            MessageInfo("该分类为系统初始化,不能操作！", "statusing");
            return;
        }
        if (rows.attributes.Status == 0) {
            MessageInfo("该分类已经禁用！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '禁用后,所属子类也将被禁用,您确认要禁用该分类吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                ids.push(rows.attributes.Id);
                Ext.Ajax.request({
                    url: '/ShopCategory/DisableUse',
                    params: { id: ids.join(",") },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            // Ext.getCmp("tg").root.load({ id: Ext.getCmp("tg").getNodeById(rows.attributes.Id) });  
                            Ext.getCmp("tg").root.reload();
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

//删除
function DeleteDate() {
    var grid = Ext.getCmp("tg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelectedNode();
    if (rows) {
        if (rows.attributes.IsSystem) {
            MessageInfo("该分类为系统初始化,不能操作！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要删除该分类吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                ids.push(rows.attributes.Id);
                Ext.Ajax.request({
                    url: '/ShopCategory/DeleteData',
                    params: { id: ids.join(",") },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            //判断是否在grid最后一条的时候删除,如果删除,重新加载
                            Ext.getCmp("tg").root.reload();
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
function AddDate() {
    url = '/ShopCategory/SaveData?1=1';
    var win = CreateFromWindow("新增", '');
    var leftTree = '商品分类';
    top.Ext.getCmp("xcomboboxtree").tree.root.text = leftTree;
    top.Ext.getCmp("xcomboboxtree").setValue(leftTree);
    top.Ext.getCmp("HidComBox").setValue('0');
    win.show();
}

//表单保存
function SaveDate() {
    var IsChange = false; //记录用户是否更改节点
    var id = top.Ext.getCmp("HidComBox").getValue();
    if (id != '') {
        var node = top.Ext.getCmp("treecombo").getSelectionModel().getSelectedNode();
        IsChange = (node == null ? false : (id == node.id ? false : true));
    }

    var grid = Ext.getCmp("tg");
    var rows = grid.getSelectionModel().getSelectedNode();
   
    var parentId = (node == null ? top.Ext.getCmp("HidComBox").getValue() : (node.id == '-1' ? "0" : node.id)); //所属链接

    var formPanel = top.Ext.getCmp("formPanel");

    if (formPanel.getForm().isValid()) {//如果验证通过
        
        url = url + '&parentId=' + parentId + "&IsChange=" + IsChange;

        formPanel.getForm().submit({
            waitTitle: '系统提示', //标题
            waitMsg: '正在提交数据,请稍后...', //提示信息  
            submitEmptyText: false,
            url: url, //记录表单提交的路径
            method: "POST",
            success: function (form, action) {
                var flag = action.result; //成功后
                if (flag.success) {
                    Ext.getCmp("tg").root.reload();
                    top.Ext.getCmp('window').close();
                    MessageInfo("保存成功！", "right");
                } else {
                    MessageInfo(flag.msg, "error");
                }
            },
            failure: function (form, action) {
                var flag = action.result; //成功后
                MessageInfo(flag.msg, "error");
            }
        });
    }
}

//创建表单弹框
function CreateFromWindow(title,key) {

    var treeloader = new Ext.tree.TreeLoader({
        url: "/ShopCategory/Comboboxtree?key=" + key
    });

    var form = new top.Ext.FormPanel({
        id: "formPanel",
        labelWidth: 65,
        fileUpload: true, //有需文件上传的,需填写该属性
        labelAlign: 'right',
        frame: true,
        bodyStyle: 'padding:5px',
        width: 650,
        autoScroll: true,
        defaultType: 'textfield',
        items: [
              {
                  id: 'HidComBox',
                  xtype: 'textfield',
                  hidden: true
              },
              {
                  xtype: 'xcomboboxtree',
                  fieldLabel: '<span class="required">*</span>父节点',
                  id: 'xcomboboxtree',
                  enableClearValue: true, //显示清除值的trigger 
                  lines: true, //显示树形控件的前导线
                  autoScroll: true,
                  rootVisible: false,
                  useArrows: true,
                  animate: true,
                  allowBlank: false,
                  enableDD: true,
                  istWidth: 70, //下拉框的长度   
                  listHeight: 200, //下拉框的高度   
                  tree: new top.Ext.tree.TreePanel({
                      root: new Ext.tree.AsyncTreeNode({
                          id: '-1',
                          text: '商品分类'
                      }),
                      id: 'treecombo',
                      rootVisible: true, //设为false将隐藏根节点，
                      loader: treeloader
                  })
              },
           {
               name: 'Name',
               fieldLabel: '<span class="required">*</span>类别名称',
               id: 'Name',
               flex: 2,
               maxLength: 50,
               allowBlank: false,
               emptyText: '商品类别名称',
               maxLengthText: '类别名称长度不能超过20个字符',
               anchor: '95%'
           },
			{
			    xtype: 'numberfield',
			    name: 'OrderNum',
			    id: 'OrderNum',
			    fieldLabel: '<span class="required">*</span>编码',
			    allowBlank: false,
			    value: 0,
			    minValue: 0,
			    maxValue: 999999999,
			    anchor: '95%'
			},
            {
                fieldLabel: '备注',
                xtype: 'textarea',
                height: 100,
                id: 'Remark',
                name: 'Remark',
                anchor: '95%'
            }
        ]
    });

    //窗体
    var win = Window("window", title, form);
    win.width = 550;
    win.height =350;
    return win;
}

//编辑
function EditDate() {
    var grid = Ext.getCmp("tg");
    var rows = grid.getSelectionModel().getSelectedNode();
    if (rows) {
        var key = rows.attributes.Id;
        if (rows.attributes.IsSystem) {
            MessageInfo("该分类为系统初始化,不能操作！", "statusing");
            return;
        }
        var win = CreateFromWindow("编辑", key);
        var form = top.Ext.getCmp('formPanel');
      
        url = '/ShopCategory/SaveData?Id=' + key + '&modify=1';
        top.Ext.getCmp("Name").setValue(rows.attributes.Name);
        top.Ext.getCmp("OrderNum").setValue(rows.attributes.OrderNum);
        //top.Ext.getCmp("PicUrl").setValue(rows.attributes.PicUrl);
        top.Ext.getCmp("Remark").setValue(rows.attributes.Remark);

        var leftTree = '';
        if (rows.attributes.ParentCategoryId == '0') {//编辑根节点
            leftTree = '商品分类';
            top.Ext.getCmp("xcomboboxtree").tree.root.text = leftTree;
        }
        else {
            leftTree = rows.parentNode.attributes.Name;
        }
        top.Ext.getCmp("xcomboboxtree").setValue(leftTree);
        top.Ext.getCmp("HidComBox").setValue(rows.attributes.ParentCategoryId);

        win.show();
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}


//同步
function GTPSynchronization() {
    var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要同步T+最新商品分类吗?', function (e) {
        if (e == "yes") {
            var success = false;
            top.Ext.MessageBox.show({
                title: '请等待',
                msg: '系统正在同步...',
                progressText: 'Initializing...',
                width: 300,
                progress: true,
                closable: false
            });
            var f = function (v) {
                return function () {
                    if (v != 12) {
                        var i = v / 11;
                        top.Ext.MessageBox.updateProgress(i, Math.round(100 * i) + '% 已完成');
                    }
                };
            };
            for (var i = 1; i < 13; i++) {
                setTimeout(f(i), i * 200);
            }
            Ext.Ajax.request({
                url: '/ShopCategory/GTPSynchronization',
                success: function (response) {
                    var rs = eval('(' + response.responseText + ')');
                    if (rs.success) {
                        top.Ext.MessageBox.hide();
                        Ext.getCmp("tg").root.reload();
                        if (rs.msg == "0") {
                            top.Ext.Msg.show({ title: "信息提示", msg: "类别同步成功,没有要更新的记录.", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                        }
                        else {
                            top.Ext.Msg.show({ title: "信息提示", msg: "类别同步成功,更新记录条数:" + rs.msg + "条", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                        }
                    } else {
                        MessageInfo("同步失败！", "error");
                    }
                }
            });
        }
    });
}