function tbar(PageName) {
    //同步查询页面权限按钮
    var tb = new Ext.Toolbar();

    if (PageName != '') {
        //根据类名查询类名对应的栏目按钮列表
        var respon = Ext.lib.Ajax.getConnectionObject().conn;
        respon.open("post", "/SysMaster/GetRoleBtn?PageAction=" + PageName, false); //获取页面按钮
        respon.send(null);
        var result = Ext.util.JSON.decode(respon.responseText);
        if (result.success) {
            if (result.msg != "") {
                var result = Ext.util.JSON.decode(result.msg);
                for (var i = 0; i < result.length; i++) {
                    var Name = result[i].Name; //按钮名称
                    var NameTip = result[i].NameTip; //按钮tip
                    var IConName = result[i].IConName; //按钮图标
                    var ActionName = result[i].ActionName; //按钮方法,字符串类型
                    if (i != 0) {//按钮之间的分隔符
                        tb.add('-');
                    }
                    tb.add({
                        text: Name,
                        tooltip: NameTip,
                        id: IConName,
                        iconCls: IConName,
                        handler: BtnFun(ActionName) //因为是字符串类型,找不到方法,所以需转义一下
                    });
                }
            }
        }
        else {
            if (result.errorCode == 9) {
                top.Ext.Msg.show({
                    title: "信息提示",
                    msg: "用户信息已过期,请重新登录",
                    buttons: Ext.Msg.OK,
                    icon: Ext.MessageBox.INFO,
                    fn: GoOut//退出
                });
                return false;
            }
        }
    }
    //定义的 searchData,可以构建出快捷查询,如果不需要,可以不定义searchData

    tb.add('->');
    //查询内容
    var comContent = new Ext.form.TextField({
        width: 140,
        id: 'comContent',
        emptyText: '品牌名称/简拼',
        enableKeyEvents: true,
        listeners: {
            'keyup': function (val) {
                if (val.getValue()) {
                    SearchDate();
                }
            }
        }
    });

    tb.add('快捷查询:');
    tb.add(comContent); //文本输入
    tb.add([{
        text: '搜索',
        iconCls: 'GTP_search',
        id: 'GTP_search',
        tooltip: '搜索满足条件的数据',
        scope: this,
        handler: function () {
            if (comContent.isValid()) {
                SearchDate();
            }
        }
    }]);
    return tb;
};

//查询
function SearchDate() {
    Ext.getCmp('phones').getStore().loadData([]);
    Ext.getCmp('phones').store.clearData();

    var respon = Ext.lib.Ajax.getConnectionObject().conn;
    respon.open("post", "/ChannelManager/SearchData?BrandName=" + encodeURIComponent(Ext.getCmp("comContent").getValue()), false);
    respon.send(null);
    var result = Ext.util.JSON.decode(respon.responseText);
    if (result.rows.length > 0) {
        var Data = [];
        for (var i = 0; i < result.rows.length; i++) {
            var o = [result.rows[i].Id, result.rows[i].Name, result.rows[i].Img];
            Data.push(o);
        }
        Ext.getCmp('phones').getStore().loadData(Data);
    }
}
//删除
function DeleteDate() {
    //得到选后的数据   
    var count = Ext.getCmp("phones").getSelectionCount();
    if (count == 1) {
        var gridlength = Ext.getCmp("gg").getStore().data.length;
        //判断型号是否有数据
        if (gridlength == 0) {
            var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要删除该品牌吗?', function (e) {
                if (e == "yes") {
                    var ids = [];
                    var node = Ext.getCmp("phones").getSelectedNodes()[0];
                    var key = node.id.replace('phones-', ''); //固有格式 id+-
                    ids.push(key);
                    Ext.Ajax.request({
                        url: '/ChannelManager/DeleteData',
                        params: { id: ids.join(",") },
                        success: function (response) {
                            var rs = eval('(' + response.responseText + ')');
                            if (rs.success) {
                                refrushDate();
                                //判断是否在grid最后一条的时候删除,如果删除,重新加载
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
            MessageInfo("品牌下还有型号列表，不能删除！", "statusing");
        }
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}

//新增
function AddDate() {
    var window = CreateFromWindow("新增");
    var form = top.Ext.getCmp('formPanel');
    form.form.reset();
    url = '/ChannelManager/SaveData?1=1';
    window.show();
}

//编辑
function EditDate() {
    var count = Ext.getCmp("phones").getSelectionCount();
    if (count==1) {
        var win = CreateFromWindow("编辑");
        var node = Ext.getCmp("phones").getSelectedNodes()[0];
        var form = top.Ext.getCmp('formPanel');
        var key = node.id.replace('phones-', '');//固有格式 id+-
        url = '/ChannelManager/SaveData?Id=' + key + '&modify=1';

        var respon = Ext.lib.Ajax.getConnectionObject().conn;
        respon.open("post", "/ChannelManager/LoadData?id=" + key, false);
        respon.send(null);
        var result = Ext.util.JSON.decode(respon.responseText);
        if (result) {
            //查询数据
            top.Ext.getCmp("Name").setValue(result.Name);
            top.Ext.getCmp("Img").setValue(result.Img);
        }    
        win.show();
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}

//表单保存
function SaveDate() {
    var formPanel = top.Ext.getCmp("formPanel");
    if (formPanel.getForm().isValid()) {//如果验证通过
//        if (top.Ext.getCmp("LinkUrl").getValue() == "http://") {
//            top.Ext.getCmp("LinkUrl").setValue("");
//        }
        formPanel.getForm().submit({
            waitTitle: '系统提示', //标题
            waitMsg: '正在提交数据,请稍后...', //提示信息  
            submitEmptyText: false,
            url: url, //记录表单提交的路径
            method: "POST",
            success: function (form, action) {
                var flag = action.result; //成功后
                if (flag.success) {
                    top.Ext.getCmp('window').close();
                    MessageInfo("保存成功！", "right");
                    this.location.href = "/ChannelManager/ChannelManager";
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
function CreateFromWindow(title) {
    var form = new top.Ext.FormPanel({
        id: "formPanel",
        labelWidth:75,
        fileUpload: true, //有需文件上传的,需填写该属性
        labelAlign: 'right',
        frame: true,
        bodyStyle: 'padding:5px',
        autoScroll: true,
        defaultType: 'textfield',
        items: [
           {
               name: 'Name',
               fieldLabel: '<span class="required">*</span>品牌名称',
               id: 'Name',
               flex: 2,
               maxLength: 50,
               allowBlank: false,
               emptyText: '品牌名称',
               maxLengthText: '名称长度不能超过20个字符',
               anchor: '95%'
           },
                {
                    xtype: 'tbtext',
                    text: '<br/>'
                },
                 {
                     xtype: 'compositefield',
                     fieldLabel: '<span class="required">*</span>标识图',
                     id: 'compositefieldPic',
                     combineErrors: false,
                     anchor: '95%',
                     items: [
							{
							    fieldLabel: '',
							    hidden: true,
							    xtype: 'textfield'
							},
                            {
                                xtype: 'fileuploadfield',
                                id: 'Img',
                                emptyText: '可上传类别标识图',
                                name: 'ImageUrl',
                                allowBlank: false,
                                buttonText: '',
                                buttonCfg: {
                                    iconCls: 'image_add',
                                    tooltip: '图片选择'
                                },
                                width:300,
                                listeners: {
                                    'fileselected': {
                                        fn: this.FileUploadAction,
                                        scope: this
                                    }
                                }
                            }
						]
                 },
                 {
                     xtype: 'tbtext',
                     style: 'margin-left:70px;color:Green',
                     text: '(图片建议尺寸:640px*320px,大小:500kb)<br/>'
                 },
                                 {
                                     xtype: 'tbtext',
                                     text: '<br/>'
                                 }
//           {
//               name: 'LinkUrl',
//               fieldLabel: '链接地址',
//               id: 'LinkUrl',
//               flex: 2,
//               maxLength: 50,
//               value:'http://',
//               emptyText: '图片名称',
//               maxLengthText: '图片名称长度不能超过20个字符',
//               anchor: '95%'
//           }
        ]
    });

    //窗体
    var win = Window("window", title, form);
    win.width =450;
    win.height =250;
    return win;
}

//可以显示上传图片
function FileUploadAction(val, a) {
    if (checkFile(val)) {
        url += "&isUpLoad=1";
    }
}

//刷新
function refrushDate() {
    SearchDate();
}

//加载grid
function loadGrid(val) {
    var key = val.id.replace("phones-", "");
    treeNodeId = key;
    var filestore = Ext.getCmp('gg').getStore();
    filestore.load({ params: { start: 0, limit: PageSize} });

    Ext.getCmp("AddGrid").setDisabled(false);
    Ext.getCmp("EditGrid").setDisabled(false);
    Ext.getCmp("DeleteGrid").setDisabled(false);

    Ext.getCmp("eastOrganization").expand(true); //展开
    Ext.getCmp("eastOrganization").setTitle(val.innerText + "型号管理");
}

//grid双击默认进行编辑操作
function dbGridClick(grid, rowindex, e) {
    EditGrid();
}

//新增型号
function AddGrid() {
    var win = load("新增");
    url = '/ChannelManager/SaveModelData?LinkId=' + treeNodeId;
    win.show();
}
//编辑型号
function EditGrid() {
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].get("Id");
        var win = load("编辑");
        url = '/ChannelManager/SaveModelData?Id=' + key + '&modify=1&LinkId=' + treeNodeId;
        var form = top.Ext.getCmp('formPanel');
        form.form.loadRecord(rows[0]);
        win.show();

    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}
//删除型号
function DeleteGrid() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要删除该型号吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push("'" + rows[i].data["Id"] + "'");
                }
                Ext.Ajax.request({
                    url: '/ChannelManager/DeleteModelData',
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

//加载代理商信息表单
function load(title) {
    //得到选后的数据
    var key = "";
    if (title == "编辑") {
        var grid = Ext.getCmp("gg");
        var rows = grid.getSelectionModel().getSelections();
        key = rows[0].get("Id");
    }
    //表单
    var form = new top.Ext.FormPanel({
        id: "formPanel",
        labelAlign: 'right',
        frame: true,
        bodyStyle: 'padding:5px 5px 0',
        width: 200,
        autoScroll: true,
        items: [
                     {
                         xtype: 'textfield',
                         name: 'ModelName',
                         id: 'ModelName',
                         fieldLabel: '<span class="required">*</span>型号名称',
                         emptyText: '请输入型号名称',
                         allowBlank: false,
                         anchor: '95%',
                         listeners: {
                             'blur': function (val) {
                                 if (val.getValue()) {
                                     var respon = Ext.lib.Ajax.getConnectionObject().conn;
                                     respon.open("post", "/SysModel/SearchCodeExits?key=" + key + "&ModelName=" + encodeURIComponent(val.getValue().trim()) + "&LinkId=" + treeNodeId, false);
                                     respon.send(null);
                                     var response = Ext.util.JSON.decode(respon.responseText);
                                     if (response.success) {
                                         top.Ext.Msg.show({
                                             title: "信息提示",
                                             msg: "型号名称已经重复,请重新输入！",
                                             buttons: Ext.Msg.OK,
                                             icon: Ext.MessageBox.INFO,
                                             fn: function () {
                                                 top.Ext.getCmp('ModelName').setValue("");
                                             }
                                         });
                                     }
                                 }
                             }
                         }
                     }, {
                         xtype: 'numberfield',
                         name: 'Sort',
                         id: 'Sort',
                         fieldLabel: '<span class="required">*</span>排序',
                         allowBlank: false,
                         value: 0,
                         minValue: 0,
                         maxValue: 9999,
                         anchor: '95%'
                     }, {
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
    win.width = 500;
    win.height = 250;
    return win;
}
