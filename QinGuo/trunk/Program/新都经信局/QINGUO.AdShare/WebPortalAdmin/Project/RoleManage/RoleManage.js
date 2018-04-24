//点击组织架构
function treeitemclick(node, e) {
    if (node.attributes.Attribute == 2) {//1:集团 2:公司
        treeNodeId = node.attributes.id;
    }
    else {
        treeNodeId = "-1";
    }
    Ext.getCmp("gg").getStore().reload();
}


/*权限管理--功能权限设置
key:当前grid选中的记录主键
*/
function setOpearaRoles(key) {

    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    var CreaterId = rows[0].get("CreaterId");
    var creator = getLoginUser(); //获取用户信息
    var type = 'owner';
    if (creator.Id != CreaterId) {
        type = 'other';//查看他人的角色
    }
    new top.Ext.Window({
        id: "winRoles",
        width: 320,
        height: 460,
        closable: false,
        border: false,
        shadow: false,
        stateful: false,
        modal: true,
        layout: 'fit',
        plain: true,
        autoDestroy: true,
        closeAction: 'close',
        title: '菜单权限设置',
        items: {
            autoScroll: true,
            autoLoad: { url: '../../Project/Html/FunAuth.htm?date=' + new Date(), scripts: true, nocache: true },
            params: { key: key, type: type }
        },
        buttons: [
        {
            text: '确定',
            iconCls: 'GTP_submit',
            tooltip: '保存当前的选择',
            handler: function () {
                operateRole(key);
            }
        },
        {
            text: '取消',
            iconCls: 'GTP_cancel',
            tooltip: '取消当前的操作',
            handler: function () {
                top.Ext.getCmp('winRoles').close();
            }
        }]
    }).show();

}

/*保存 功能权限
key:当前grid选中的记录主键(角色编号)
*/
function operateRole(key) {
    var checkid = '';
    var rt = top.rmtree.getChecked(); //得到所有所选的子节点

    if (rt.length > 0) {
        for (var i = 0; i < rt.length; i++) {//除去最顶端的根节点(模块功能分配)
            if (rt[i].getUI().checkbox) {
                if (!rt[i].getUI().checkbox.indeterminate) {
                    checkid += rt[i].id + ',';

                }
            }
        }
    }
    else {
        top.Ext.Msg.show({ title: "信息提示", msg: '请先进行选择', buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
        return false;
    }
    checkid = checkid.substring(0, checkid.length - 1);


    Ext.Ajax.request({
        url: '/RoleManage/SaveRoles',
        params: { json: checkid, key: key }, //json类数据，主键，修改或者添加，数据实体类名
        success: function (response) {
            var result = Ext.util.JSON.decode(response.responseText);
            if (result.success) {
                MessageInfo("保存成功！", "right");
            }
            else {
                MessageInfo(result.msg, "right");
            }
        },
        failure: function (response) {
            //用户信息丢失,重新登录
            parent.window.location.href = parent.loginError;
        }
    });
    top.Ext.getCmp('winRoles').close();
}


/*权限管理--数据权限设置*/
function setRangeRoles(roleType) {
    var colModel = new Ext.grid.ColumnModel([
            new Ext.grid.RowNumberer({
                header: "",
                width: 30
            }),
            { header: "模块名称Id", dataIndex: "FunId", hidden: true, menuDisabled: true },
            { header: "模块名称", dataIndex: "FunName", menuDisabled: true,
                renderer: function (data, metadata, record, rowIndex, columnIndex, store) {
                    metadata.attr = 'ext:qtip="' + data + '"';
                    return data;
                }
            },
            { header: "权限类型", dataIndex: "lookPower", menuDisabled: true,
                width: 70,
                sortable: true,
                renderer: function (value) {
                    switch (value) {
                        case 1:
                            return "查看自建";
                            break;
                        case 2:
                            return "查看全部";
                            break;
                        default:
                            return "查看自建";
                            break;
                    }

                }
            },
            { header: "部门选择Id", dataIndex: "DeptId", menuDisabled: true, hidden: true },
            { header: "操作", dataIndex: "Id", width: 30, menuDisabled: true,
                renderer: function (data, metadata, record, rowIndex, columnIndex, store) {
                    return "<img src='../../Resource/css/icons/toolbar/GTP_delete.png' title='删除' style='border:0px; cursor:pointer'  onclick='deletePic(\"" + data + "\");'/>";
                }
            }
        ]);

    var store = new Ext.data.Store//表示从哪里获得数据
        ({
            proxy: new Ext.data.HttpProxy
            ({ url: '/RoleManage/GetAllData' }),
            //解析Json
            reader: new Ext.data.JsonReader
            ({
                root: 'rows',
                id: 'Id',
                fields:
                    [
                       'Id', 'FunId', 'FunName', 'lookPower', 'DeptId', 'DeptName'
                    ]
            }
            ),
            remoteSort: true
        });

    //最重要的来了~~~GridPanel~主角登场了~gridPanel就是把前面我们的东西全部整合的容器  
    var gridPanle = new top.Ext.grid.GridPanel({
        id: 'RoleGG',
        viewConfig: {
            forceFit: true, // 注意不要用autoFill:true,那样设置的话当GridPanel的大小变化（比如你resize了它）时不会自动调整column的宽度
            scrollOffset: 0 //不加这个的话，会在grid的最右边有个空白，留作滚动条的位置
        },
        cm: colModel,
        border: false,
        store: store,
        autoWidth: true,
        height: 210,
        autoScroll: true
    });
    //加载数据
    store.load({ params: { ea: roleType} });

    gridPanle.on("rowdblclick", function () {
        var rec = gridPanle.getSelectionModel().getSelected();
        var center = top.Ext.getCmp("tab"); //获取id为center的元素,也就是上面的tabpanel  
        var moviesDescr = top.Ext.getCmp("tab2"); //同理获取id为moviesDescr的元素  
        center.setActiveTab(1); //将tabpanel的items里的第2个元素设置为激活状态,也就是将Movies Description

        top.Ext.getCmp('AddSave').hide();
        top.Ext.getCmp('editSave').show();

        top.Ext.getCmp('FunName').setValue(rec.get("FunName"));
        top.Ext.getCmp('FunId').setValue(rec.get("FunId"));

        top.Ext.getCmp('key').setValue(rec.get("Id")); //主键

        var lookPower = 1;
        var radionum = top.document.getElementsByName("Type");
        for (var i = 0; i < radionum.length; i++) {
            if (radionum[i].value == parseInt(rec.get("lookPower"))) {
                radionum[i].checked = true;
                break;
            }
        }
    })

    var form = new top.Ext.FormPanel({
        id: "formPanel",
        fileUpload: true, //有需文件上传的,需填写该属性
        border: false,
        labelWidth: 65,
        labelAlign: 'right',
        items: [
                {
                    xtype: 'panel',
                    layout: 'form',
                    border: false,
                    defaultType: 'textfield',
                    bodyStyle: 'padding:25px;overflow:hidden',
                    items: [
                      {
                          name: 'key',
                          fieldLabel: '主键',
                          xtype: 'textfield',
                          id: 'key',
                          hidden: true
                      },
                        {
                            name: 'FunId',
                            fieldLabel: '关联字段',
                            xtype: 'textfield',
                            id: 'FunId',
                            hidden: true
                        },
                      {
                          xtype: 'compositefield',
                          fieldLabel: '<span class="required">*</span>模块名称',
                          id: 'composShoper',
                          combineErrors: false,
                          items: [
							{
							    name: 'FunName',
							    xtype: 'textfield',
							    id: 'FunName',
							    height: 22, //固定高度
							    emptyText: '右边选择模块',
							    width: 290,
							    allowBlank: true,
							    readOnly: true
							},
							new top.Ext.Button({
								    text: '选择',
								    width: 40,
								    handler: openRoleTree
								}),
                            new top.Ext.Button({
                                    text: '清除',
                                    width: 40,
                                    handler: clearRoleTree
                                })
						    ]
                       },
                       {
                           xtype: 'tbtext',
                           text: '<br/>'
                       },
                  
                        {
                            xtype: 'radiogroup',
                            fieldLabel: '查看类型',
                            id: 'Type',
                            width: 200,
                            items: [
						            { boxLabel: '查看自建', name: 'Type', id: 'Type1', inputValue: 1, checked: true, width: 100, height: 25 },
						            { boxLabel: '查看全部', name: 'Type', id: 'Type2', inputValue: 2, width: 100, height: 25 }
                                    ]
                            }
                        ]
                }
                ]
       });

    var winRangeRoles = new top.Ext.Window({
        id: "winRangeRoles",
        width: 550,
        height: 350,
        closable: false,
        modal: true,
        shadow: false,
        stateful: false,
        layout: 'fit',
        plain: true,
        autoDestroy: true,
        closeAction: 'close',
        title: '数据权限设置',
        items: new top.Ext.TabPanel({
            activeTab: 1, //
            border: false,
            id: 'tab',
            items: [
                    {
                        title: '权限列表',
                        id: 'tab1',
                        items: [gridPanle]
                    },
                     {
                         title: '权限编辑',
                         id: 'tab2',
                         items: [form]
                     }
                ],
            listeners: {// 添加监听器，点击此页面的tab时候要重新加载（刷新功能）
                tabchange: function (a, b, c, d) {
                    if (a.activeTab.id == "tab2") {
                        if (top.Ext.getCmp('key').getValue() != "") {//修改
                            top.Ext.getCmp('editSave').show();
                        }
                        else {
                            top.Ext.getCmp('AddSave').show();
                        }
                    }
                    else {
                        top.Ext.getCmp('AddSave').hide();
                        top.Ext.getCmp('editSave').hide();
                    }
                    store.reload();
                    top.Ext.getCmp('tab1').doLayout();
                }
            }
        }),
        buttons: [
                {
                    text: '保存', id: 'AddSave', iconCls: 'GTP_save', tooltip: '保存', handler: function () {
                        RangeRoles(roleType, 'add');
                    }
                },
                {
                    text: '保存', id: 'editSave', iconCls: 'GTP_save', tooltip: '保存', hidden: true, handler: function () {
                        RangeRoles(roleType, 'edit');
                    }
                },
                {
                    text: '取消', iconCls: 'GTP_cancel', tooltip: '取消', handler: function () {
                        top.Ext.getCmp("winRangeRoles").close();
                    }
                }
                ]
    }).show();
}

//清除
function clearRoleTree() {
    top.Ext.getCmp("FunId").setValue("");
    top.Ext.getCmp("FunName").setValue("");
    top.Ext.getCmp("Type1").setValue(true);
}


/*数据范围模块树选择
key:当前grid选中的记录主键
*/
function openRoleTree() {
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    var key = rows[0].get("Id");

    //获取选中的模块树
    var funid = top.Ext.getCmp("FunId").getValue();
    if (!funid) {
        funid = "";
    }

    var CreaterId = rows[0].get("CreaterId");
    var creator = getLoginUser(); //获取用户信息
    var type = 'owner';
    if (creator.Id != CreaterId) {
        type = 'other'; //查看他人的角色
    }

    new top.Ext.Window({
        id: "winRoleTree",
        width: 320,
        height: 460,
        closable: false,
        border: false,
        shadow: false,
        stateful: false,
        modal: true,
        layout: 'fit',
        plain: true,
        autoDestroy: true,
        closeAction: 'close',
        title: '数据权限设置',
        items: {
            autoScroll: true,
            autoLoad: { url: '../../Project/Html/RoleTree.htm', scripts: true, nocache: true },
            params: { key: key, funid: funid, type: type}//key:选中的角色编号 funid:选择的模块树
        },
        buttons: [
        {
            text: '确定',
            iconCls: 'GTP_submit',
            tooltip: '保存当前的选择',
            handler: function () {
                SaveRoleTree();
            }
        },
        {
            text: '取消',
            iconCls: 'GTP_cancel',
            tooltip: '取消当前的操作',
            handler: function () {
                top.Ext.getCmp('winRoleTree').close();
            }
        }]
    }).show();
}

//确定选择的模块树
function SaveRoleTree() {
    var json;
    var checkid = '';
    var checkName = '';
    var rt = top.rmtree.getChecked(); //得到所有所选的子节点
    if (rt.length > 0) {
        for (var i =0; i < rt.length; i++) {//除去最顶端的根节点(模块功能分配)
            if (rt[i].getUI().checkbox) {
                if (!rt[i].getUI().checkbox.indeterminate && rt[i].getUI().checkbox.disabled != true) {
                    checkid += rt[i].id + ',';
                    checkName += rt[i].text + ',';
                }
            }
        }
    }
    else {
        top.Ext.Msg.show({ title: "信息提示", msg: '请先进行选择', buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO }); return false;
    }
    if (checkid == '' && checkName == '') {
        top.Ext.Msg.show({ title: "信息提示", msg: '请先进行选择', buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO }); return false;
    }
    //*---------------------------------------------------------------------------------------
    checkid = checkid.substring(0, checkid.length - 1); //模块树id
    checkName = checkName.substring(0, checkName.length - 1); //模块树名称

    top.Ext.getCmp("FunId").setValue(checkid);
    top.Ext.getCmp("FunName").setValue(checkName);
    top.Ext.getCmp('winRoleTree').close();
}

/*数据范围模块树选择
roleId:当前grid选中的记录主键
type:新增or编辑
*/
function RangeRoles(roleId, type) {

    var FunName = top.Ext.getDom('FunName').value; //模块树名称
    var FunId = top.Ext.getDom('FunId').value; //模块树id
    var key = top.Ext.getDom('key').value == "" ? '' : top.Ext.getDom('key').value; //主键

    if (!FunName || !FunId) {
        top.Ext.Msg.show({ title: "信息提示", msg: '请先进行模块名称选择！', buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
        return false;
    }

    if (type == "edit") {
        if (!roleId) {
            top.Ext.Msg.show({ title: "信息提示", msg: '您当前是新增操作,请点击[新增保存]！', buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
            return false;
        }
        if (top.Ext.getCmp('key').getValue() == "") {
            type = 'add'
        }
    }
    else {
        key = '';
    }
    //获取选中的查看权限范围
    var lookPower = 1;
    var radionum = top.document.getElementsByName("Type");
    for (var i = 0; i < radionum.length; i++) {
        if (radionum[i].checked) {
            lookPower = radionum[i].value;
            break;
        }
    }

    var obj = new Object();
    obj.Id = key;
    obj.FunName = FunName;
    obj.FunId = FunId;
    obj.txtDeptName = '';
    obj.txtDeptId = '';
    obj.lookPower = lookPower;
    obj.RoleId = roleId

    var json = Ext.util.JSON.encode(obj); //转换成字符串

    Ext.Ajax.request({
        url: '/RoleManage/DateRoleSave',
        params: { json: json, opeaterType: type }, //json类数据，主键，修改or添加
        success: function (response) {
            var result = Ext.util.JSON.decode(response.responseText);
            if (result.success) {
                top.Ext.Msg.show({ title: "系统提示", msg: '设置成功!', buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                top.Ext.getCmp("RoleGG").store.reload();
                top.Ext.getCmp('tab').setActiveTab(0);
                clearRoleTree(); //清楚
            }
            else {
                top.Ext.Msg.show({ title: "信息提示", msg: result.msg, buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
            }
        },
        failure: function (response) {
            top.Ext.Msg.show({ title: "系统错误提示", msg: '用户信息过期，请重新登陆!', buttons: Ext.Msg.OK, icon: Ext.MessageBox.ERROR });
        }
    });
}


//新增
function AddDate() {

    //var grid = Ext.getCmp("tr");
    //var rows = grid.getSelectionModel().getSelectedNode();
   var CompanyId ='';
//    if (rows) {
//        if (rows.attributes.Attribute == 2) {
//            CompanyId = rows.attributes.id;
//        }
//        else {
//            MessageInfo("请先选择公司！", "statusing");
//            return false;
//        }
//    }
//    else {
//        MessageInfo("请先选择公司！", "statusing");
//        return false;
//    }
    var window = CreateFromWindow("新增");
    var form = top.Ext.getCmp('formPanel');
    form.form.reset();
    url = '/RoleManage/SaveData?CompanyId=' + CompanyId;
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
        var key = rows[0].get("Id");
        url = '/RoleManage/SaveData?Id=' + key + '&modify=1';
        window.show();
        top.Ext.getCmp("formPanel").form.loadRecord(rows[0]); //再加载数据   
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

//渲染状态
function EnableOrDisable(value, meta, record, rowIdx, colIdx, store) {

}

//删除
function DeleteDate() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var confirm = top.Ext.MessageBox.confirm('系统确认', '删除后,拥有该角色的人员将失去该角色的权限.<br/><br/>您确认要删除该角色吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                Ext.Ajax.request({
                    url: '/RoleManage/DeleteData',
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


//创建表单弹框
function CreateFromWindow(title) {
    var form = new top.Ext.FormPanel({
        labelWidth: 65,
        frame: true,
        border: false,
        labelAlign: 'right',
        bodyStyle: 'padding:5px 5px 0',
        id: 'formPanel',
        defaultType: 'textfield',
        //defaults: { width:350 },
        items: [
                {
                    name: 'Name',
                    fieldLabel: '<span class="required">*</span>角色名称',
                    emptyText: '输入角色的名称', ////textfield自己的属性
                    allowBlank: false,
                    maxLength: 20,
                    anchor: '95%',
                    maxLengthText: '角色名称长度不能超过20个字符'
                },
                 //{
                 //    xtype: 'numberfield',
                 //    name: 'RoleSort',
                 //    id: 'RoleSort',
                 //    fieldLabel: '排序',
                 //    allowBlank: false,
                 //    value: 0,
                 //    minValue: 0,
                 //    maxValue: 9999
                 //},
                {
                    fieldLabel: '描述信息',
                    xtype: 'textarea',
                    id: 'Introduction',
                    name: 'Introduction',
                    height: 150,
                    anchor: '95%',
                    emptyText: '可输入对角色的描述信息', ////textfield自己的属性
                    maxLength: 200,
                    maxLengthText: '描述长度不能超过200个字符'
                }
        ]
    });

    //窗体
    var win = Window("window", title, form);
    win.width =500;
    win.height =300;
    return win;
}


