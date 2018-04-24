//重写按钮事件
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
        text: '启用',
        tooltip: '启用',
        id: 'GTP_enabled',
        iconCls: 'GTP_enabled',
        handler: EnableUse
    });

    tb.add('-');

    tb.add({
        text: '禁用',
        tooltip: '禁用',
        id: 'GTP_disable',
        iconCls: 'GTP_disable',
        handler: DisableUse
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


//启用
function EnableUse() {
    var grid = Ext.getCmp("tg");
    var rows = grid.getSelectionModel().getSelectedNode();
    if (rows) {
        if (rows.attributes.IsSystem) {
            MessageInfo("该导航为系统初始化,不能操作！", "statusing");
            return;
        }
        if (rows.attributes.Status == 1) {
            MessageInfo("该导航已经启用！", "statusing");
            return;
        }

        var confirm = top.Ext.MessageBox.confirm('系统确认', '启用后,所属子类与父类也将被启用,您确认要启用该导航吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                ids.push(rows.attributes.id);
                Ext.Ajax.request({
                    url: '/SysFun/EnableUse',
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
            MessageInfo("该导航为系统初始化,不能操作！", "statusing");
            return;
        }
        if (rows.attributes.Status == 0) {
            MessageInfo("该导航已经禁用！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '禁用后,所属子类也将被禁用,您确认要禁用该导航吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                ids.push(rows.attributes.id);
                Ext.Ajax.request({
                    url: '/SysFun/DisableUse',
                    params: { id: ids.join(",") },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
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

//切换菜单事件
function treeitemclick(node, e) {
    if (node.isLeaf()) {
        treeNodeId = node.id;
        Ext.getCmp("tg").root.reload();
    }
}

//打开按钮选择窗体
function OpenBtnWin() {
    btnWindow();
}

//按钮选择
function BtnSave() {
    var formPanel = Ext.getCmp("_btnPanel");
    if (formPanel.getForm().isValid()) {//如果验证通过
        var val = formPanel.getForm().findField('itemselector').getValue().split(',');
        var BtnName = '';
        var Btnlue = '';
        Ext.getCmp("PageAction").setValue("");
        for (var i = 0; i < val.length; i++) {
            if (val[i] != "") {
                var str = val[i];
                Btnlue += str.split('|')[0] + ',';
                BtnName += str.split('|')[1] + ',';
            }
        }

        if (BtnName.length > 0) {
            BtnName = BtnName.substring(0, BtnName.length - 1);
            Btnlue = Btnlue.substring(0, Btnlue.length - 1);
        }
        Ext.getCmp("HidBtnValue").setValue(Btnlue); 
        Ext.getCmp("PageAction").setValue(BtnName); //控件赋值
        //关闭窗体
        Ext.getCmp("BTNWINDOW").close();
    }
}

// 表单
function windowForm(title) {
    //form表单
    var NodeId = Ext.getCmp("tree").getSelectionModel().getSelectedNode().id;
    var treeloader = new Ext.tree.TreeLoader({
        url: "/SysFun/Comboboxtree?LeftId=" + NodeId
    });
   
    var formPanel = new Ext.FormPanel({
        frame: true,
        bodyPadding: 5,
        id: '_formPanel',
        labelAlign: 'right',
        defaultType: 'textfield',
        defaults: { width:300 },
        items: [
                 {
                     id: 'HidComBox',
                     xtype: 'textfield',
                     hidden:true
                 },
                 {
                     id: 'HidBtnValue',
                     xtype: 'textfield',
                     hidden: true
                 },
                    {
                        xtype: 'xcomboboxtree',
                        fieldLabel: '所属父节点',
                        id: 'xcomboboxtree',
                        enableClearValue: true, //显示清除值的trigger 
                        lines: true, //显示树形控件的前导线
                        autoScroll: true,
                        rootVisible: false,
                        useArrows: true,
                        animate: true,
                        allowBlank: false,
                        enableDD: true,
                        istWidth:70, //下拉框的长度   
                        listHeight: 200, //下拉框的高度   
                        tree: new Ext.tree.TreePanel({
                            root: new Ext.tree.AsyncTreeNode({
                                id: '-1',
                                text: '菜单导航'
                            }),
                            id: 'treecombo',
                            rootVisible: true, //设为false将隐藏根节点，
                            loader: treeloader
                        })
                    },
                    {
                        name: 'FunName',
                        id: 'FunName',
                        fieldLabel: '导航名称',
                        allowBlank: false
                    },
                    {
                        name: 'PageUrl',
                        id: 'PageUrl',
                        fieldLabel: '导航链接'
                    },
                    {
                        name: 'ClassName',
                        id: 'ClassName',
                        fieldLabel: '类名'
                    },
                    {
                        name: 'iconCls',
                        id: 'iconCls',
                        fieldLabel: '栏目图标'
                    },
                    {
                        xtype: 'numberfield',
                        name: 'FunSort',
                        id: 'FunSort',
                        fieldLabel: '排序',
                        allowBlank: false,
                        value: 0,
                        minValue: 0,
                        maxValue: 9999
                    },
                    {
                        xtype: 'compositefield',
                        fieldLabel: '页面按钮',
                        combineErrors: false,
                        items: [
                    {
                        name: 'PageAction',
                        fieldLabel: '导航按钮',
                        xtype: 'textfield',
                        id: 'PageAction',
                        flex: 2,
                        emptyText: '右边选择页面按钮',
                        allowBlank: true,
                        readOnly: true
                    },
                     new Ext.Button({
                         text: '...',
                         width: 40,
                         handler: OpenBtnWin
                     })
                ]
                    }
        ]
    });

    //弹出窗体
    var win = new Ext.Window({
        id: 'SYSFUNWINDOW',
        title: title,
        width:500,
        height: 300,
        minHeight: 200,
        bodyPadding: 5,
        layout: 'fit',
        shadow: false,
        stateful: false,
        closable: false,
        border:false,
        resizable: true,
        modal: true,
        items: formPanel,
        closeAction: 'close',
        buttons: [{
            text: '保存',
            iconCls: 'GTP_save',
            tooltip: '保存',
            handler: SaveDate
        }, {
            text: '取消',
            iconCls: 'GTP_cancel',
            tooltip: '取消',
            handler: function () {
                Ext.getCmp('SYSFUNWINDOW').close();
            }
        }]
    });

    win.show();
}

//按钮表单
function btnWindow() {
    var grid = Ext.getCmp("tg");
    //得到选后的数据
    var rows = grid.getSelectionModel().getSelectedNode();
    var id = "";
    if (rows != null) {
        var title = Ext.getCmp("SYSFUNWINDOW").title;
        if (title != "新增") {
            id = rows.attributes.id;
        }
    }
    var rightStore = new Ext.data.Store({
        id: "_rightStore",
        proxy: new Ext.data.HttpProxy({ url: "/SysFun/itemselectorStore?Action=BtnRightSelect&Id=" + id }),
        reader: new Ext.data.JsonReader({
            totalProperty: "total",
            root: "rows"
        },
	            [
	                { name: "text" },
	                { name: "value" }
			            ]
	            )
    });


    var leftStore = new Ext.data.Store({
        id: "_leftStore",
        proxy: new Ext.data.HttpProxy({ url: "/SysFun/itemselectorStore?Action=BtnLeftSelect" }),
        reader: new Ext.data.JsonReader({
            totalProperty: "total",
            root: "rows"
        },
        [{ name: "text" },{ name: "value" }])
    });

    leftStore.load();
    rightStore.load();

    var btnPanel = new Ext.FormPanel({
        height:320,
        bodyStyle: 'padding:10px;',
        layout: 'fit',
        id: '_btnPanel',
        items: [{
            xtype: 'itemselector',
            name: 'itemselector',
            fieldLabel: 'ItemSelector',
            allowBlank: false,
            imagePath: '../../Content/Extjs/ux/css/images',
            multiselects: [{
                width: 250,
                height: 300,
                store: leftStore,
                legend: "可选按钮",
                displayField: 'text',
                style: 'overflow-y:scroll',
                valueField: 'value'
            }, {
                width: 250,
                height:300,
                legend: "已选按钮",
                store: rightStore,
                displayField: 'text',
                valueField: 'value'
            }]
        }]
    });

    //按钮弹框
    var btnwin = new Ext.Window({
        title: '按钮选择',
        id: 'BTNWINDOW',
        closable: false,
        shadow: false,
        stateful: false,
        modal: true,
        closeAction: 'hide',
        width: 580,
        border: false,
        minWidth: 250,
        modal: true,
        items: btnPanel,
        closeAction: 'close',
        buttons: [{
            text: '确定',
            iconCls: 'GTP_submit',
            tooltip: '确定',
            handler: BtnSave
        },
         {
             text: '取消',
             iconCls: 'GTP_cancel',
             tooltip: '取消',
             handler: function () {
                 Ext.getCmp('BTNWINDOW').close();

             }
         }
            ]
    });
    btnwin.show();
}

//新增
function AddDate() {
    windowForm('新增');
    url = '/SysFun/SaveData/?1=1';
    var leftTree = Ext.getCmp("tree").getSelectionModel().getSelectedNode().text;
    Ext.getCmp("xcomboboxtree").tree.root.text = leftTree;
}

//编辑
function EditDate() {
    var grid = Ext.getCmp("tg");
    //得到选后的数据
    var rows = grid.getSelectionModel().getSelectedNode();
    var selNodes = grid.getChecked();
    if (rows) {
        if (rows.attributes.IsSystem) {//系统定义的不能删除编辑
            Ext.MessageBox.show({ title: '系统提示', msg: '无法编辑系统定义记录!', buttons: Ext.MessageBox.OK, animEl: 'GTP_edit', icon: Ext.MessageBox.INFO });
            return;
        }
        windowForm('编辑');

        var respon = Ext.lib.Ajax.getConnectionObject().conn;
        respon.open("post", "/SysFun/itemselectorStore?Action=BtnRightSelect&Id=" + rows.attributes.id, false); //获取页面按钮
        respon.send(null);
        var result = Ext.util.JSON.decode(respon.responseText);
        var BtnName = [];
        var Btnlue =[];
        for (var i = 0; i < result.total; i++) {
            var res = result.rows[i].value.split('|');
            Btnlue.push(res[0]);
            BtnName.push(res[1]);
        }

        Ext.getCmp("PageAction").setValue(BtnName.join(',')); //控件赋值
        Ext.getCmp("HidBtnValue").setValue(Btnlue.join(',')); 

        var window = Ext.getCmp('SYSFUNWINDOW');
        url = '/SysFun/SaveData/?Id=' + rows.id + '&modify=1';

        Ext.getCmp("FunName").setValue(rows.attributes.text);
        Ext.getCmp("PageUrl").setValue(rows.attributes.pageUrl);
        Ext.getCmp("ClassName").setValue(rows.attributes.className);
        Ext.getCmp("iconCls").setValue(rows.attributes.iconCls);
        Ext.getCmp("FunSort").setValue(rows.attributes.funSort);

        //设置combox选中值
        var leftTree = '';
        if (rows.attributes.parentId == '0') {//编辑根节点
           leftTree = Ext.getCmp("tree").getSelectionModel().getSelectedNode().text;
           Ext.getCmp("xcomboboxtree").tree.root.text = leftTree;
        }
         else {
             var node = grid.getNodeById(rows.attributes.id).parentNode;
             leftTree = node.text;
        }
        Ext.getCmp("xcomboboxtree").setValue(leftTree);
        Ext.getCmp("HidComBox").setValue(rows.attributes.parentId);
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}

//保存
function SaveDate() {

    var formPanel = Ext.getCmp("_formPanel");
    var node = Ext.getCmp("treecombo").getSelectionModel().getSelectedNode();
    var parentId = (node == null ? Ext.getCmp("HidComBox").getValue() : (node.id == '-1' ? "0" : node.id)); //所属链接
    var NodeId = Ext.getCmp("tree").getSelectionModel().getSelectedNode().id;

    var btnSelect = Ext.getCmp("HidBtnValue").getValue();

    var grid = Ext.getCmp("tg");
    //得到选后的数据
    var rows = grid.getSelectionModel().getSelectedNode();
    if (rows) {
        if (rows.attributes.id == parentId && url.indexOf('modify')>=0) {//系统定义的不能删除编辑
            MessageInfo("所属父节点不能选择自己本身！", "statusing");
            return;
        }
    }

    url = url+'&PageAction=' + btnSelect + '&parentId=' + parentId + "&leftType=" + NodeId;

    if (node) {
        if (node.attributes['isChild'] && url.indexOf('modify')<=0) {//不允许选择子节点
            Ext.MessageBox.show({ title: '系统提示', msg: '该节点已经是子节点,不能继续添加!', buttons: Ext.MessageBox.OK, animEl: 'GTP_save', icon: Ext.MessageBox.INFO });
            return;
        }
    }
    if (formPanel.getForm().isValid()) {//如果验证通过
        formPanel.getForm().submit({
            waitTitle: '系统提示', //标题
            waitMsg: '正在提交数据,请稍后...', //提示信息   
            submitEmptyText: false, //If set to true, the emptyText value will be sent with the form when it is submitted. Defaults to true                                  
            url: url, //记录表单提交的路径
            method: "POST",
            //baseParams: [{ leftType: NodeId}], //传递左边选择的树
            success: function (form, action) {
                //成功后
                var flag = action.result;
                if (flag.success) {
                    Ext.getCmp("tg").root.reload();
                    MessageInfo("保存成功！", "right");
                }
                else {
                    MessageInfo("保存失败！", "error");
                }
                Ext.getCmp("SYSFUNWINDOW").close();
            },
            failure: function (form, action) {
                MessageInfo("保存失败！", "error");
                Ext.getCmp("SYSFUNWINDOW").hide();
            }
        });
    }
}

//刷新
function refrushDate() {
    Ext.getCmp("tg").root.reload();
}

//渲染状态
function EnableOrDisable(value, meta, record, rowIdx, colIdx, store) {
    return formartEnableOrDisable(value);
}

//删除
function DeleteDate() {
    var grid = Ext.getCmp("tg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelectedNode();
    if (rows) {
        if (rows.childNodes.length > 0) {
            MessageInfo("不能直接删除根节点！", "statusing");
        }
        else if (rows.attributes.IsSystem) {//系统定义的不能删除编辑
            top.Ext.MessageBox.show({ title: '系统提示', msg: '无法删除系统定义记录!', buttons: Ext.MessageBox.OK, animEl: 'GTP_delete', icon: Ext.MessageBox.INFO });
            return;
        }
        else {
            var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要删除该链接吗?', function (e) {
                if (e == "yes") {
                    var ids = [];
                    ids.push(rows.attributes.id);
                    Ext.Ajax.request({
                        url: '/SysFun/DeleteData',
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
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}


