//搜索面板
function ChangeSearch() {
    var Address = Ext.getCmp("txtAddress").getValue();
    var Name = Ext.getCmp("txtName").getValue();
    var _store = Ext.getCmp("gg").store;
    Ext.apply(_store.baseParams, { "txtAddress": Address, "txtName": Name });
    _store.reload({ params: { start: 0, limit: parseInt(Ext.getCmp("pagesize").getValue())} });
}

//批量生成二维码
function BatchImport() {
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
    var window = CreateImportFromWindow("批量生成二维码");
    var form = top.Ext.getCmp('formPanel');
    form.form.reset();
    url = '/FireBox/BatchImport?CompanyId=' + CreateCompanyId;
    window.show();
}

//批量生成弹框
function CreateImportFromWindow(title) {
    var form = new top.Ext.FormPanel({
        id: "formPanel",
        labelWidth: 75,
        fileUpload: true, //有需文件上传的,需填写该属性
        labelAlign: 'right',
        frame: true,
        bodyStyle: 'padding:5px',
        autoScroll: true,
        defaultType: 'textfield',
        items: [
           {
               xtype: 'numberfield',
               name: 'CountNum',
               id: 'CountNum',
               fieldLabel: '生成数量',
               allowBlank: false,
               value: 0,
               anchor: '90%',
               minValue: 0,
               maxValue: 999
           }
        ]
    });

    //窗体
    var win = Window("window", title, form);
    win.width = 450;
    win.height = 200;
    return win;
}

//批量导出
function BatchExport() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length > 0) {
        var confirm = top.Ext.MessageBox.confirm('系统确认', '您确定要导出当前选中的信息吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push("'" + rows[i].data["Id"] + "'");
                }
                var Linkurl = '/FireBox/ImportOut?IdList=' + ids.join(",") + '&date=' + new Date();
                $(".hideform").attr("action", Linkurl);
                $(".hideform").submit();
            }
        });
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}

//展示图片
function showImg(val) {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].data["Id"];
        var Address = rows[0].data["Address"];
        var QRCode = rows[0].data["QrCode"]; //二维码编码
        var shop = new top.Ext.Window({
            width: 450,
            id: 'shoper',
            height: 350,
            closable: false,
            shadow: false,
            stateful: false,
            border: false,
            modal: true,
            layout: 'fit',
            plain: true,
            autoDestroy: true,
            closeAction: 'close',
            title: Address + '二维码展示',
            items: {
                xtype: 'panel', //或者xtype: 'component',
                border: false,
                items: [{
                    xtype: 'box', //或者xtype: 'component',
                    id: 'PagLogo',
                    style: 'margin-left:100px;margin-top:20px',
                    width: 200, //图片宽度
                    height: 200, //图片高度
                    autoEl: {
                        tag: 'img',    //指定为img标签
                        src: '/Content/Extjs/resources/images/default/s.gif'    //指定url路径
                    }
                },
                 {
                     style: 'margin-left:100px;',
                     xtype: 'tbtext',
                     html: '<br/>二维码编码:' + QRCode
                 }
                ]

            },
            buttons: [
                    {
                        text: '取消',
                        iconCls: 'GTP_cancel',
                        tooltip: '取消当前的操作',
                        handler: function () {
                            top.Ext.getCmp("shoper").close();
                        }
                    }]
        }).show();

        top.Ext.getCmp("PagLogo").getEl().dom.src = rows[0].data["Img"];
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}

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
}

//渲染状态
function formartStatus(val, rows) {
    switch (val) {
        case -1:
            return "<span style='color:orange'>未通过</span>";
            break;
        case 0:
            return "<span style='color:#6a6a6a'>未使用</span>";
            break;
        case 1:
            return "<span style='color:green'>已使用</span>";
            break;
    }
}

//新增
function AddDate() {
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
    var window = CreateFromWindow("新增");
    var form = top.Ext.getCmp('formPanel');
    form.form.reset();
    url = '/FireBox/SaveData?CompanyId=' + CreateCompanyId;
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
        url = '/FireBox/SaveData?Id=' + key + '&modify=1';
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
    if (rows.length > 0) {
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要删除该记录吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push("'" + rows[i].data["Id"] + "'");
                }
                Ext.Ajax.request({
                    url: '/FireBox/DeleteData',
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
                    name: 'Address',
                    fieldLabel: '箱子位置',
                    allowBlank: false,
                    maxLength: 50,
                    maxLengthText: '位置长度不能超过100个字符'
                },
                {
                    name: 'Name',
                    fieldLabel: '查询简码',
                    allowBlank: false,
                    maxLength: 50,
                    maxLengthText: '简码不能超过100个字符'
                }
        ]
    });

    //窗体
    var win = Window("window", title, form);
    win.width = 360;
    win.height = 150;
    return win;
}

//展示设备
function showEqument() {
  var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var Address = rows[0].get("Address");
        var key = rows[0].get("Id");
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
            title: Address+'设备列表',
            items: {
                autoScroll: true,
                border: false,
                params: { CID: "", TypeShow:7, ResponsibleId: key },
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