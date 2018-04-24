//加载单位信息表单
function load(title, key) {

    var CompanyNature = [['个体工商', '个体工商'], ['私营独资', '私营独资'], ['国营商家', '国营商家']];
    var CompanyType = [['其它', '其它'], ['合资', '合资'], ['独资', '独资'], ['国有', '国有'], ['私有', '私有'], ['全民所有', '全民所有'], ['集体所有', '集体所有'], ['私有制', '私有制'], ['有限责任制', '有限责任制']];
    var CompanyAttribute = [['集团', '1'], ['单位', '2']]; //, ['分单位', '3'], ['子单位', '4']

    var Nature = new top.Ext.form.ComboBox({
        store: new Ext.data.SimpleStore({
            fields: ['text', 'value'],
            data: CompanyNature
        }),
        displayField: 'text',
        fieldLabel: '单位分类',
        anchor: '90%',
        valueField: 'value',
        mode: 'local',
        id: 'Nature',
        selectOnFocus: true,
        orceSelection: true,
        editable: false,
        triggerAction: 'all',
        allowBlank: false,
        value: '私营独资'
    });

    var Type = new top.Ext.form.ComboBox({
        store: new Ext.data.SimpleStore({
            fields: ['text', 'value'],
            data: CompanyType
        }),
        displayField: 'text',
        valueField: 'value',
        mode: 'local',
        id: 'Type',
        selectOnFocus: true,
        orceSelection: true,
        fieldLabel: '单位性质',
        anchor: '90%',
        editable: false,
        triggerAction: 'all',
        allowBlank: false,
        value: '其它'
    });

    var Attribute = new top.Ext.form.ComboBox({
        store: new Ext.data.SimpleStore({
            fields: ['text', 'value'],
            data: CompanyAttribute
        }),
        displayField: 'text',
        valueField: 'value',
        mode: 'local',
        id: 'Attribute',
        name: 'Attribute',
        selectOnFocus: true,
        orceSelection: true,
        fieldLabel: '单位类型',
        anchor: '90%',
        editable: false,
        triggerAction: 'all',
        allowBlank: false,
        value: '1'
    });

    //表单
    var form = new top.Ext.FormPanel({
        id: "formPanel",
        labelAlign: 'right',
        frame: true,
        labelWidth: 80,
        bodyStyle: 'padding:5px 5px 0',
        width: 650,
        autoScroll: true,
        items: [
            {
                xtype: "fieldset",
                autoHeight: true,
                title: "单位基本信息",
                layout: 'column',
                items: [
                    {
                        columnWidth: .5,
                        layout: 'form',
                        items: [
                            {
                                name: 'Name',
                                fieldLabel: '<span class="required">*</span>单位名称',
                                xtype: 'textfield',
                                id: 'Name',
                                maxLength: 50,
                                allowBlank: false,
                                emptyText: '填写单位名称',
                                maxLengthText: '名称长度不能超过50个字符',
                                anchor: '90%',
                                enableKeyEvents: true,
                                listeners: {
                                    'keyup': function (val) {
                                        if (val.getValue().trim() != "") {
                                            top.Ext.getCmp("Code").setValue(codeConvert(top.Ext.getCmp("Name").getValue()));
                                        }
                                    }
                                }
                            },
                             {
                                 name: 'Code',
                                 fieldLabel: '单位编码',
                                 xtype: 'textfield',
                                 id: 'Code',
                                 flex: 2,
                                 maxLength: 50,
                                 maxLengthText: '编码长度不能超过50个字符',
                                 anchor: '90%'
                             },
                            {
                                name: 'LegalPerson',
                                fieldLabel: '单位法人',
                                xtype: 'textfield',
                                id: 'LegalPerson',
                                flex: 2,
                                anchor: '90%'
                            },
                            
                             Type,
                              {
                                  name: 'LinkUser',
                                  fieldLabel: '联系人',
                                  xtype: 'textfield',
                                  id: 'LinkUser',
                                  flex: 2,
                                  anchor: '90%'
                              },
                            {
                                name: 'Address',
                                fieldLabel: '<span class="required">*</span>单位地址',
                                xtype: 'textfield',
                                id: 'Address',
                                allowBlank: false,
                                flex: 2,
                                maxLength: 200,
                                maxLengthText: '地址长度不能超过200个字符',
                                anchor: '90%'
                            },
                             {
                                 xtype: 'compositefield',
                                 fieldLabel: '单位位置',
                                 combineErrors: false,
                                 anchor: '90%',
                                 items: [
                                        {
                                            xtype: 'textfield',
                                            name: "ComPLon",
                                            id: "Bx",
                                            width: 70,
                                            height: 22, //固定高度
                                            emptyText: 'X',
                                            readOnly: true
                                        },
                                         {
                                             xtype: 'textfield',
                                             height: 22, //固定高度
                                             id: "By",
                                             name: "CompLat",
                                             width: 70,
                                             emptyText: 'Y',
                                             readOnly: true
                                         },
                                        new top.Ext.Button({
                                            text: '地图',
                                            width: 45,
                                            handler: function () {
                                                if (title == "新增") {
                                                    if (top.$("#Bx").val() != "X" && top.$("#By").val() != "Y") {
                                                        WindowMap(top.$("#Bx").val(), top.$("#By").val()).show();
                                                    } else {
                                                        WindowMap('', '').show();
                                                    }
                                                } else {
                                                    if (top.$("#Bx").val() != "X" && top.$("#By").val() != "Y") {
                                                        WindowMap(top.$("#Bx").val(), top.$("#By").val()).show();
                                                    } else {
                                                        WindowMap('', '').show();
                                                    }
                                                }
                                            }
                                        })
                                 ]
                             }

                        ]
                    },
                     {
                         columnWidth: .5,
                         layout: 'form',
                         items: [
                               Attribute,
                             {
                                 name: 'ReegistMoney',
                                 fieldLabel: '注册资金',
                                 xtype: 'numberfield',
                                 id: 'ReegistMoney',
                                 anchor: '90%',
                                 value: 0,
                                 minValue: 0,
                                 maxValue: 9999999999
                             },
                            Nature,
                             {
                                 xtype: 'datefield',
                                 name: 'RegisiTime',
                                 id: 'RegisiTime',
                                 fieldLabel: '成立时间',
                                 format: 'Y-m-d',
                                 //value: new Date(),
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
                            },
                               {
                               xtype: 'textfield',
                               name: 'Email',
                               id: 'Email',
                               fieldLabel: '邮箱',
                               vtype: 'email',
                               anchor: '90%'
                           },
                                {
                                    xtype: 'textfield',
                                    hidden: true,
                                    name: 'CityName',
                                    id: 'CityName',
                                    fieldLabel: '城市',
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
                             fieldLabel: '信息介绍',
                             xtype: 'textarea',
                             id: 'Introduction',
                             name: 'Introduction',
                             height: 120,
                             emptyText: '可输入对单位的介绍信息', ////textfield自己的属性
                             anchor: '98%',
                             maxLength: 200,
                             maxLengthText: '描述长度不能超过200个字符'
                         }]
                     }
                ]
            }
        ]
    });
    //窗体
    var win = Window("window", title, form);
    win.width = 680;
    win.height = 455;
    return win;
}

//启用
function EnableUse() {
    var grid = Ext.getCmp("tg");
    var rows = grid.getSelectionModel().getSelectedNode();
    if (rows) {
        if (rows.attributes.Status== "1") {
            MessageInfo("该单位已经启用！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要启用该单位吗?', function (e) {
            if (e == "yes") {
                var key =rows.attributes.Id;
                Ext.Ajax.request({
                    url: '/BIZ/EnableUse',
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
            MessageInfo("该单位已经禁用！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '禁用后,单位下面所有的信息都会被禁用.<br/><br/>确认要禁用该单位吗?', function (e) {
            if (e == "yes") {
                var key = rows[0].get("Id");
                Ext.Ajax.request({
                    url: '/BIZ/DisableUse',
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
    var win = load("新增");
    url = '/Organizational/SaveData?1=1';
    win.show();
}
//编辑
function EditDate() {
    var grid = Ext.getCmp("tg");
    //得到选后的数据
    var rows = grid.getSelectionModel().getSelectedNode();
    var selNodes = grid.getChecked();
    if (rows) {
        var key =rows.attributes.id;
        var win = load("编辑", key);
        var form = top.Ext.getCmp('formPanel');

        top.Ext.getCmp("Name").setValue(rows.attributes.text);
        top.Ext.getCmp("Phone").setValue(rows.attributes.Phone);
        top.Ext.getCmp("Email").setValue(rows.attributes.Email);
        top.Ext.getCmp("Nature").setValue(rows.attributes.Nature);
        top.Ext.getCmp("Type").setValue(rows.attributes.Type);
        top.Ext.getCmp("Attribute").setValue(rows.attributes.Attribute);

        top.Ext.getCmp("Code").setValue(rows.attributes.Code);
        top.Ext.getCmp("RegisiTime").setValue(rows.attributes.RegisiTime);


        url = '/Organizational/SaveData?Id=' + key + '&modify=1';
        win.show();
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
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要删除该代理商吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                Ext.Ajax.request({
                    url: '/SysCompany/DeleteData',
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

//启用
function EnableUse() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        if (rows[0].data["Status"] == 1) {
            MessageInfo("该供应商已经启用！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要启用该供应商吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                Ext.Ajax.request({
                    url: '/SysCompany/EnableUse',
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
            MessageInfo("该供应商已经禁用！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要禁用该供应商吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                Ext.Ajax.request({
                    url: '/SysCompany/DisableUse',
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





