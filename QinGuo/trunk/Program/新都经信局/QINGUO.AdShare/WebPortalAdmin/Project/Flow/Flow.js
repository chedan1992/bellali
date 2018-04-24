//渲染类型1:单位管理员注册时的审核 2:普通用户注册审核  3:用户单位变更审核
function formartFlowType(val, rows) {
    switch (val) {
        case 1:
            return '单位审核';
            break;
        case 2:
            return '用户审核';
            break;
        case 3:
            return '变更审核';
            break;
        case 4:
            return '设备变更';
            break;
    }
}
//渲染状态 (-1:未通过 0:待审核 1:已通过)
function formartFlowStatus(val, rows) {
    switch (val) {
        case -1:
            return "<span style='color:orange'>未通过</span>";
            break;
        case 0:
            return "<span style='color:Red'>待审核</span>";
            break;
        case 1:
            return "<span style='color:green'>已通过</span>";
            break;
    }
}
//办理
function DoWork() {
    var grid =Ext.getCmp("gg");
     //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].get("Id"); 
        var FlowType = rows[0].get("FlowType"); //流程类型 1:单位管理员注册时的审核 2:普通用户注册审核  3:用户单位变更审核
        switch (parseInt(FlowType)) {
            case 1:
            case 2:
            case 3:
                DoWorkTypeOne(key);
                break;
            case 4: //设置责任制变更
                var AppointedId = rows[0].get("MasterId");
                DoWorkTypeFore(key,AppointedId);
                break;
        }
    }
    else {
        top.Ext.Msg.show({ title: "信息提示", msg: '请选中一条记录', buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
    }
}

//查看信息
function LookWork() {
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].get("Id");
        var FlowType = rows[0].get("FlowType"); //流程类型 1:单位管理员注册时的审核 2:普通用户注册审核  3:用户单位变更审核
        switch (parseInt(FlowType)) {
            case 1:
            case 2:
            case 3:
                LookWorkTypeOne(key);
                break;
            case 4:
                var AppointedId = rows[0].get("MasterId");
                LookWorkTypeFore(key, AppointedId);
                break;
        }
    }
    else {
        top.Ext.Msg.show({ title: "信息提示", msg: '请选中一条记录', buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
    }
}


//处理流程第一,二种类型
function DoWorkTypeOne(key) {
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
              xtype: 'fieldset',
              title: '类型',
              autoHeight: true,
              layout: 'column',
              items: [
                    {
                        columnWidth: .5,
                        layout: 'form',
                        items: [
                           {
                               fieldLabel: '用户类型',
                               xtype: 'textfield',
                               value:'单位管理员',
                               readOnly: true,
                               maxLength: 50,
                               anchor: '90%'
                           }
                        ]
                       },
                     {
                         columnWidth: .5,
                         layout: 'form',
                         items: [
                           {
                               fieldLabel: '单位类型',
                               xtype: 'textfield',
                               value: '',
                               id:'CompanyType',
                               readOnly: true,
                               maxLength: 50,
                               anchor: '90%'
                           }
                        ]
                     }
                ]
          },
          {
                xtype: "fieldset",
                autoHeight: true,
                title: "基本信息",
                layout: 'column',
                items: [
                    {
                        columnWidth: .5,
                        layout: 'form',
                        items: [
                            {
                                name: 'Name',
                                fieldLabel: '单位名称',
                                xtype: 'textfield',
                                id: 'Name',
                                readOnly: true,
                                maxLength: 50,
                                anchor: '90%'
                            },
                            {
                                name: 'LinkUser',
                                fieldLabel: '联系人',
                                xtype: 'textfield',
                                id: 'LinkUser',
                                flex: 2,
                                readOnly: true,
                                anchor: '90%'
                            },
                               {
                                   name: 'LoginName',
                                   id: 'LoginName',
                                   fieldLabel: '用户账号',
                                   readOnly: true,
                                   maxLength: 20,
                                   xtype: 'textfield',
                                   anchor: '90%'
                               }
                        ]
                    },
                     {
                         columnWidth: .5,
                         layout: 'form',
                         items: [
                          {
                              name: 'Address',
                              fieldLabel: '单位地址',
                              xtype: 'textfield',
                              id: 'Address',
                              flex: 2,
                              readOnly: true,
                              maxLength: 200,
                              anchor: '90%'
                          },
                            {
                                name: 'Phone',
                                fieldLabel: '联系电话',
                                id: 'Phone',
                                flex: 2,
                                readOnly: true,
                                xtype: 'textfield',
                                regex: /^\d+$/,
                                anchor: '90%'
                            }
                        ]
                     }
                ]
            },
          {
                xtype: 'fieldset',
                title: '审批意见',
                autoHeight: true,
                items: [
                    {
                        fieldLabel: '审批意见',
                        xtype: 'textarea',
                        id: 'Introduction',
                        name: 'Introduction',
                        height: 80,
                        allowBlank: true,
                        anchor: '95%',
                        emptyText: '可输入审批意见(70字符内)', ////textfield自己的属性
                        maxLength: 70,
                        maxLengthText: '描述长度不能超过70个字符'
                    }
                ]
            }
        ]
    });
            //窗体
    var win = new top.Ext.Window({
        id: "window",
        title: '流程处理',
        width: 600,
        height: 425,
        layout: 'fit',
        modal: true,
        shadow: false,
        stateful: false,
        items: form,
        border: false,
        closeAction: 'close',
        buttons: [
        {
            text: '通过',
            iconCls: 'GTP_submit',
            handler: function () {
                var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要通过该审核吗?', function (e) {
                    if (e == "yes") {
                        var uresopn = Ext.lib.Ajax.getConnectionObject().conn;
                        uresopn.open("post", "/Flow/CompanyWaitWork?id=" + key + "&Attribute=1&Introduction=" + top.Ext.getCmp("Introduction").getValue(), false);
                        uresopn.send(null);
                        var urs = Ext.util.JSON.decode(uresopn.responseText);
                        if (urs.success == true) {
                            top.Ext.getCmp("window").close(); //直接销毁
                            Ext.getCmp("gg").store.reload();
                            MessageInfo("操作成功！", "right");
                        }
                        else {
                            MessageInfo("操作失败！", "error");
                        }
                    }
                });
            }
        },
        {
            text: '不通过',
            iconCls: 'GTP_cancel',
            handler: function () {
                var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要拒绝通过该审核吗?', function (e) {
                    if (e == "yes") {
                        var uresopn = Ext.lib.Ajax.getConnectionObject().conn;
                        uresopn.open("post", "/Flow/CompanyWaitWork?id=" + key + "&Attribute=2&Introduction=" + top.Ext.getCmp("Introduction").getValue(), false);
                        uresopn.send(null);
                        var urs = Ext.util.JSON.decode(uresopn.responseText);
                        if (urs.success == true) {
                            top.Ext.getCmp("window").close(); //直接销毁
                            Ext.getCmp("gg").store.reload();
                            MessageInfo("操作成功！", "right");
                        }
                        else {
                            MessageInfo("操作失败！", "error");
                        }
                    }
                });
            }
        },
        {
            text: '取消',
            handler: function () {
                top.Ext.getCmp("window").close(); //直接销毁
            }
        }]
    });

    win.addListener('beforeshow', function (o) {
        win.center(); //始终居中显示
    });
    win.show();

    //查询信息
    //管理员信息表单
    var uresopn = Ext.lib.Ajax.getConnectionObject().conn;
    uresopn.open("post", "/Flow/GetCompanyByFlow?Id=" + key, false);
    uresopn.send(null);
    var urs = Ext.util.JSON.decode(uresopn.responseText);
    if (urs.data) {
        top.Ext.getCmp("formPanel").findById("LoginName").setValue(urs.data.LoginName);
        top.Ext.getCmp("formPanel").findById("Name").setValue(urs.data.Name);
        top.Ext.getCmp("formPanel").findById("LinkUser").setValue(urs.data.LinkUser);
        top.Ext.getCmp("formPanel").findById("Address").setValue(urs.data.Address);
        top.Ext.getCmp("formPanel").findById("Phone").setValue(urs.data.Phone);

        switch (parseInt(urs.data.Attribute)) {
            case 1:
                top.Ext.getCmp("formPanel").findById("CompanyType").setValue('社会单位');
                break;
            case 3:
                top.Ext.getCmp("formPanel").findById("CompanyType").setValue('消防单位');
                break;
            case 4:
                top.Ext.getCmp("formPanel").findById("CompanyType").setValue('维保公司');
                break;
            case 5:
                top.Ext.getCmp("formPanel").findById("CompanyType").setValue('供应商');
                break;
        }
    }
}

//处理设备信息
function DoWorkTypeFore(key, AppointedId) {
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
                title: "基本信息",
                layout: 'column',
                items: [
                    {
                        columnWidth: .5,
                        layout: 'form',
                        items: [
                        {
                            name: 'Name',
                            fieldLabel: '设备名称',
                            xtype: 'textfield',
                            id: 'Name',
                            maxLength: 50,
                            emptyText: '填写设备名称',
                            readOnly:true,
                            maxLengthText: '设备名称长度不能超过50个字符',
                            anchor: '90%'
                        },
                         {
                             name: 'PlacesName',
                             fieldLabel: '设备位置',
                             xtype: 'textfield',
                             id: 'PlacesName',
                             maxLength: 50,
                             readOnly: true,
                             maxLengthText: '位置长度不能超过50个字符',
                             anchor: '90%'
                         },
                        {
                            name: 'Model',
                            fieldLabel: '设备型号',
                            xtype: 'textfield',
                            id: 'Model',
                            flex: 2,
                            readOnly: true,
                            maxLength: 50,
                            maxLengthText: '型号长度不能超过300个字符',
                            anchor: '90%'
                        },
                        {
                            fieldLabel: '生产日期',
                            xtype: 'datefield',
                            value: new Date((new Date() / 1000 + 86400 * 1) * 1000),
                            width: 130,
                            readOnly: true,
                            id: 'ProductionDate',
                            name: 'ProductionDate',
                            emptyText: '选择开始时间',
                            format: 'Y-m-d(周l)',
                            anchor: '90%'
                        },
                        {
                            fieldLabel: '过期日期',
                            xtype: 'datefield',
                            id: 'LostTime',
                            value: new Date((new Date() / 1000 + 86400 * 1) * 1000),
                            width: 130,
                            readOnly: true,
                            emptyText: '选择开始时间',
                            format: 'Y-m-d(周l)',
                            anchor: '90%'
                        }
                        ]
                    },
                    {
                        columnWidth: .5,
                        layout: 'form',
                        items: [
                                  {
                                      name: 'Model',
                                      fieldLabel: '设备类型',
                                      xtype: 'textfield',
                                      id: 'Gid',
                                      readOnly: true,
                                      flex: 2,
                                      maxLength: 50,
                                      anchor: '90%'
                                  },
                                    {
                                      name: 'Model',
                                      fieldLabel: '责任人',
                                      xtype: 'textfield',
                                      id: 'ResponsibleId',
                                      readOnly: true,
                                      flex: 2,
                                      maxLength: 50,
                                      anchor: '90%'
                                  },
                            {
                                    name: 'Specifications',
                                    xtype: 'textfield',
                                    id: 'Specifications',
                                    fieldLabel: '设备规格',
                                    readOnly: true,
                                    anchor: '90%',
                                    maxLength: 50,
                                    maxLengthText: '规格长度不能超过100个字符'
                                }, {
                                    xtype: 'numberfield',
                                    name: 'StoreNum',
                                    id: 'StoreNum',
                                    fieldLabel: '数量',
                                    value: 0,
                                    readOnly: true,
                                    minValue: 0,
                                    maxValue: 9999,
                                    anchor: '90%'
                                }
                                
                               
                           ]
                    },
                    {
                        columnWidth: 1,
                        layout: 'form',
                        items: [
                         {
                             fieldLabel: '设备介绍',
                             xtype: 'htmleditor',
                             readOnly: true,
                             id: 'Mark',
                             name: 'Mark',
                             height: 80,
                             emptyText: '可输入对设备的介绍信息', ////textfield自己的属性
                             anchor: '96%'
                         }
                        ]
                    }
                ]
                 },
          {
              xtype: "fieldset",
              autoHeight: true,
              title: "设备便跟人",
              layout: 'column',
              items: [
                    {
                        columnWidth: .5,
                        layout: 'form',
                        items: [
                        {
                            xtype: 'tbtext',
                             id:'NameTip',
                             html: '申请变更人：'
                        }
                        ]
                    }
                ]
          },
          {
              xtype: 'fieldset',
              title: '审批意见',
              autoHeight: true,
              items: [
                    {
                        fieldLabel: '审批意见',
                        xtype: 'textarea',
                        id: 'Introduction',
                        name: 'Introduction',
                        height: 80,
                        allowBlank: true,
                        anchor: '95%',
                        emptyText: '可输入审批意见(70字符内)', ////textfield自己的属性
                        maxLength: 70,
                        maxLengthText: '描述长度不能超过70个字符'
                    }
                ]
          }
        ]
    });
    //窗体
    var win = new top.Ext.Window({
        id: "window",
        title: '流程处理',
        width: 600,
        height:585,
        layout: 'fit',
        modal: true,
        shadow: false,
        stateful: false,
        items: form,
        border: false,
        closeAction: 'close',
        buttons: [
        {
            text: '通过',
            iconCls: 'GTP_submit',
            handler: function () {
                var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要通过该审核吗?', function (e) {
                    if (e == "yes") {
                        var uresopn = Ext.lib.Ajax.getConnectionObject().conn;
                        uresopn.open("post", "/Flow/CompanyWaitWork?id=" + key + "&Attribute=1&Introduction=" + top.Ext.getCmp("Introduction").getValue(), false);
                        uresopn.send(null);
                        var urs = Ext.util.JSON.decode(uresopn.responseText);
                        if (urs.success == true) {
                            top.Ext.getCmp("window").close(); //直接销毁
                            Ext.getCmp("gg").store.reload();
                            MessageInfo("操作成功！", "right");
                        }
                        else {
                            MessageInfo("操作失败！", "error");
                        }
                    }
                });
            }
        },
        {
            text: '不通过',
            iconCls: 'GTP_cancel',
            handler: function () {
                var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要拒绝通过该审核吗?', function (e) {
                    if (e == "yes") {
                        var uresopn = Ext.lib.Ajax.getConnectionObject().conn;
                        uresopn.open("post", "/Flow/CompanyWaitWork?id=" + key + "&Attribute=2&Introduction=" + top.Ext.getCmp("Introduction").getValue(), false);
                        uresopn.send(null);
                        var urs = Ext.util.JSON.decode(uresopn.responseText);
                        if (urs.success == true) {
                            top.Ext.getCmp("window").close(); //直接销毁
                            Ext.getCmp("gg").store.reload();
                            MessageInfo("操作成功！", "right");
                        }
                        else {
                            MessageInfo("操作失败！", "error");
                        }
                    }
                });
            }
        },
        {
            text: '取消',
            handler: function () {
                top.Ext.getCmp("window").close(); //直接销毁
            }
        }]
    });

    win.addListener('beforeshow', function (o) {
        win.center(); //始终居中显示
    });
    win.show();

    //查询设备信息
    var uresopn = Ext.lib.Ajax.getConnectionObject().conn;
    uresopn.open("post", "/SysAppointed/GetData?Id=" + AppointedId, false);
    uresopn.send(null);
    var urs = Ext.util.JSON.decode(uresopn.responseText);
    if (urs.data) {
    debugger
        var form = top.Ext.getCmp('formPanel');
        form.form.loadRecord(urs); //再加载数据  
     
        //设备类型
        top.Ext.getCmp("Gid").setValue(urs.data.GroupName); //text
        //责任人
        top.Ext.getCmp("ResponsibleId").setValue(urs.data.Responsible); //text 
        //格式化时间

        var ProductionDate = formartValTime(urs.data.ProductionDate).format('Y-m-d');
        top.Ext.getCmp("ProductionDate").setValue(ProductionDate);

        var ProductionDate = formartValTime(urs.data.LostTime).format('Y-m-d');
        top.Ext.getCmp("LostTime").setValue(ProductionDate);


        
        var grid = Ext.getCmp("gg");
        //得到选后的数据   
        var rows = grid.getSelectionModel().getSelections();
        top.Ext.getDom("NameTip").innerText = "申请变更人：" + rows[0].get("ApprovalUserName");
    }
}

//格式化时间
function formartValTime(val) {
    
    //var d = new Date();
    //var str = val.toString();
    //var str1 = str.replace("/Date(", "");
    //var str2 = str1.replace(")/", "");
    //var dd = parseInt(str2);
    //d.setTime(dd); return d;


        if (val == null) {
            return null;
        }
        var d = new Date();
        var str = val.toString();
        var str1 = str.replace("/Date(", "");
        var str2 = str1.replace(")/", "");
        var dd = parseInt(str2);
        d.setTime(dd); return d;
};

//处理流程第一,二种类型
function LookWorkTypeOne(key) {
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
              xtype: 'fieldset',
              title: '类型',
              autoHeight: true,
              layout: 'column',
              items: [
                    {
                        columnWidth: .5,
                        layout: 'form',
                        items: [
                           {
                               fieldLabel: '用户类型',
                               xtype: 'textfield',
                               value: '单位管理员',
                               readOnly: true,
                               maxLength: 50,
                               anchor: '90%'
                           }

                        ]
                    },
                     {
                         columnWidth: .5,
                         layout: 'form',
                         items: [
                           {
                               fieldLabel: '单位类型',
                               xtype: 'textfield',
                               value: '',
                               id: 'CompanyType',
                               readOnly: true,
                               maxLength: 50,
                               anchor: '90%'
                           }

                        ]
                     }
                ]
          },
          {
              xtype: "fieldset",
              autoHeight: true,
              title: "基本信息",
              layout: 'column',
              items: [
                    {
                        columnWidth: .5,
                        layout: 'form',
                        items: [
                            {
                                name: 'Name',
                                fieldLabel: '单位名称',
                                xtype: 'textfield',
                                id: 'Name',
                                readOnly: true,
                                maxLength: 50,
                                anchor: '90%'
                            },
                            {
                                name: 'LinkUser',
                                fieldLabel: '联系人',
                                xtype: 'textfield',
                                id: 'LinkUser',
                                flex: 2,
                                readOnly: true,
                                anchor: '90%'
                            },
                               {
                                   name: 'LoginName',
                                   id: 'LoginName',
                                   fieldLabel: '用户账号',
                                   readOnly: true,
                                   maxLength: 20,
                                   xtype: 'textfield',
                                   anchor: '90%'
                               }
                        ]
                    },
                     {
                         columnWidth: .5,
                         layout: 'form',
                         items: [
                          {
                              name: 'Address',
                              fieldLabel: '单位地址',
                              xtype: 'textfield',
                              id: 'Address',
                              flex: 2,
                              readOnly: true,
                              maxLength: 200,
                              anchor: '90%'
                          },
                            {
                                name: 'Phone',
                                fieldLabel: '联系电话',
                                id: 'Phone',
                                flex: 2,
                                readOnly: true,
                                xtype: 'textfield',
                                regex: /^\d+$/,
                                anchor: '90%'
                            }
                        ]
                     }
                ]
          },
          {
              xtype: 'fieldset',
              title: '审批意见',
              autoHeight: true,
              items: [
                    {
                        fieldLabel: '审批意见',
                        xtype: 'textarea',
                        id: 'Introduction',
                        readOnly: true,
                        name: 'Introduction',
                        height: 80,
                        anchor: '95%',
                        maxLength: 70
                    }
                ]
          }
        ]
    });
    //窗体
    var win = new top.Ext.Window({
        id: "window",
        title: '流程查看',
        width: 600,
        height: 425,
        layout: 'fit',
        modal: true,
        shadow: false,
        stateful: false,
        items: form,
        border: false,
        closeAction: 'close',
        buttons: [
        {
            text: '取消',
            iconCls: 'GTP_cancel',
            handler: function () {
                top.Ext.getCmp("window").close(); //直接销毁
            }
        }]
    });
    win.addListener('beforeshow', function (o) {
        win.center(); //始终居中显示
    });
    win.show();

    //查询信息
    //管理员信息表单
    var uresopn = Ext.lib.Ajax.getConnectionObject().conn;
    uresopn.open("post", "/Flow/GetCompanyByFlow?Id=" + key, false);
    uresopn.send(null);
    var urs = Ext.util.JSON.decode(uresopn.responseText);
    if (urs.data) {
        top.Ext.getCmp("formPanel").findById("LoginName").setValue(urs.data.LoginName);
        top.Ext.getCmp("formPanel").findById("Name").setValue(urs.data.Name);
        top.Ext.getCmp("formPanel").findById("LinkUser").setValue(urs.data.LinkUser);
        top.Ext.getCmp("formPanel").findById("Address").setValue(urs.data.Address);
        top.Ext.getCmp("formPanel").findById("Phone").setValue(urs.data.Phone);
        top.Ext.getCmp("formPanel").findById("Introduction").setValue(urs.data.Introduction);
        switch (parseInt(urs.data.Attribute)) {
            case 1:
                top.Ext.getCmp("formPanel").findById("CompanyType").setValue('社会单位');
                break;
            case 3:
                top.Ext.getCmp("formPanel").findById("CompanyType").setValue('消防单位');
                break;
            case 4:
                top.Ext.getCmp("formPanel").findById("CompanyType").setValue('维保公司');
                break;
            case 5:
                top.Ext.getCmp("formPanel").findById("CompanyType").setValue('供应商');
                break;
        }
    }
}

function LookWorkTypeFore(key, AppointedId) {
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
             title: "基本信息",
             layout: 'column',
             items: [
                    {
                        columnWidth: .5,
                        layout: 'form',
                        items: [
                        {
                            name: 'Name',
                            fieldLabel: '设备名称',
                            xtype: 'textfield',
                            id: 'Name',
                            maxLength: 50,
                            emptyText: '填写设备名称',
                            readOnly: true,
                            maxLengthText: '设备名称长度不能超过50个字符',
                            anchor: '90%'
                        },
                         {
                             name: 'PlacesName',
                             fieldLabel: '设备位置',
                             xtype: 'textfield',
                             id: 'PlacesName',
                             maxLength: 50,
                             readOnly: true,
                             maxLengthText: '位置长度不能超过50个字符',
                             anchor: '90%'
                         },
                        {
                            name: 'Model',
                            fieldLabel: '设备型号',
                            xtype: 'textfield',
                            id: 'Model',
                            flex: 2,
                            readOnly: true,
                            maxLength: 50,
                            maxLengthText: '型号长度不能超过300个字符',
                            anchor: '90%'
                        },
                        {
                            fieldLabel: '生产日期',
                            xtype: 'datefield',
                            value: new Date((new Date() / 1000 + 86400 * 1) * 1000),
                            width: 130,
                            readOnly: true,
                            id: 'ProductionDate',
                            name: 'ProductionDate',
                            emptyText: '选择开始时间',
                            format: 'Y-m-d(周l)',
                            anchor: '90%'
                        },
                        {
                            fieldLabel: '过期日期',
                            xtype: 'datefield',
                            id: 'LostTime',
                            value: new Date((new Date() / 1000 + 86400 * 1) * 1000),
                            width: 130,
                            readOnly: true,
                            emptyText: '选择开始时间',
                            format: 'Y-m-d(周l)',
                            anchor: '90%'
                        }
                        ]
                    },
                    {
                        columnWidth: .5,
                        layout: 'form',
                        items: [
                                  {
                                      name: 'Model',
                                      fieldLabel: '设备类型',
                                      xtype: 'textfield',
                                      id: 'Gid',
                                      readOnly: true,
                                      flex: 2,
                                      maxLength: 50,
                                      anchor: '90%'
                                  },
                                    {
                                        name: 'Model',
                                        fieldLabel: '责任人',
                                        xtype: 'textfield',
                                        id: 'ResponsibleId',
                                        readOnly: true,
                                        flex: 2,
                                        maxLength: 50,
                                        anchor: '90%'
                                    },
                            {
                                name: 'Specifications',
                                xtype: 'textfield',
                                id: 'Specifications',
                                fieldLabel: '设备规格',
                                readOnly: true,
                                anchor: '90%',
                                maxLength: 50,
                                maxLengthText: '规格长度不能超过100个字符'
                            }, {
                                xtype: 'numberfield',
                                name: 'StoreNum',
                                id: 'StoreNum',
                                fieldLabel: '数量',
                                value: 0,
                                readOnly: true,
                                minValue: 0,
                                maxValue: 9999,
                                anchor: '90%'
                            }


                           ]
                    },
                    {
                        columnWidth: 1,
                        layout: 'form',
                        items: [
                         {
                             fieldLabel: '设备介绍',
                             xtype: 'htmleditor',
                             readOnly: true,
                             id: 'Mark',
                             name: 'Mark',
                             height: 80,
                             emptyText: '可输入对设备的介绍信息', ////textfield自己的属性
                             anchor: '96%'
                         }
                        ]
                    }
                ]
         },
          {
              xtype: "fieldset",
              autoHeight: true,
              title: "设备便跟人",
              layout: 'column',
              items: [
                    {
                        columnWidth: .5,
                        layout: 'form',
                        items: [
                        {
                            xtype: 'tbtext',
                            id: 'NameTip',
                            html: '申请变更人：'
                        }
                        ]
                    }
                ]
          },
          {
              xtype: 'fieldset',
              title: '审批意见',
              autoHeight: true,
              items: [
                    {
                        fieldLabel: '审批意见',
                        xtype: 'textarea',
                        id: 'Introduction',
                        name: 'Introduction',
                        height: 80,
                        allowBlank: true,
                        anchor: '95%',
                        emptyText: '可输入审批意见(70字符内)', ////textfield自己的属性
                        maxLength: 70,
                        maxLengthText: '描述长度不能超过70个字符'
                    }
                ]
          }
        ]
    });
    //窗体
    var win = new top.Ext.Window({
        id: "window",
        title: '流程处理',
        width: 600,
        height: 585,
        layout: 'fit',
        modal: true,
        shadow: false,
        stateful: false,
        items: form,
        border: false,
        closeAction: 'close',
        buttons: [
        {
            text: '取消',
            handler: function () {
                top.Ext.getCmp("window").close(); //直接销毁
            }
        }]
    });

    win.addListener('beforeshow', function (o) {
        win.center(); //始终居中显示
    });
    win.show();

    //查询设备信息
    var uresopn = Ext.lib.Ajax.getConnectionObject().conn;
    uresopn.open("post", "/SysAppointed/GetData?Id=" + AppointedId, false);
    uresopn.send(null);
    var urs = Ext.util.JSON.decode(uresopn.responseText);
    if (urs.data) {
        var form = top.Ext.getCmp('formPanel');
        form.form.loadRecord(urs); //再加载数据  

        //设备类型
        top.Ext.getCmp("Gid").setValue(urs.data.GroupName); //text
        //责任人
        top.Ext.getCmp("ResponsibleId").setValue(urs.data.Responsible); //text 
        //格式化时间

        var ProductionDate = formartValTime(urs.data.ProductionDate).format('Y-m-d');
        top.Ext.getCmp("ProductionDate").setValue(ProductionDate);

        var ProductionDate = formartValTime(urs.data.LostTime).format('Y-m-d');
        top.Ext.getCmp("LostTime").setValue(ProductionDate);



        var grid = Ext.getCmp("gg");
        //得到选后的数据   
        var rows = grid.getSelectionModel().getSelections();
        top.Ext.getCmp("formPanel").findById("Introduction").setValue(rows[0].get("Reamrk"));
        top.Ext.getDom("NameTip").innerText = "申请变更人：" + rows[0].get("ApprovalUserName");
    }
}
//审核
function GTPAudit() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length > 0) {
       var key = rows[0].data["Id"];
       var win= CreateFromAuditWindow('审核',key);
       win.show();
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}
//流程撤销
function GTPIntransit() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length > 0) {
        var FlowStatus = rows[0].data["FlowStatus"];
        if (FlowStatus == 0) {
            var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要撤销该申请吗?', function (e) {
                if (e == "yes") {
                    var ids = [];
                    for (var i = 0; i < rows.length; i++) {
                        ids.push(rows[i].data["Id"]);
                    }
                    Ext.Ajax.request({
                        url: '/Flow/DeleteData',
                        params: { id: ids.join(",") },
                        success: function (response) {
                            var rs = eval('(' + response.responseText + ')');
                            if (rs.success) {
                                Ext.getCmp("gg").store.reload();
                                MessageInfo("撤销成功！", "right");
                            } else {
                                MessageInfo("撤销失败！", "error");
                            }
                        }
                    });
                }
            });
        }
        else {
            MessageInfo("流程已经审核,不能撤销！", "statusing");
        }
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}
//审核表单
function CreateFromAuditWindow(title, key) {
    var CompanyAttribute = [['通过', '1'], ['不通过', '2']];
    var Attribute = new top.Ext.form.ComboBox({
        store: new Ext.data.SimpleStore({
            fields: ['text', 'value'],
            data: CompanyAttribute
        }),
        displayField: 'text',
        valueField: 'value',
        mode: 'local',
        id: 'SelectType',
        selectOnFocus: true,
        orceSelection: true,
        fieldLabel: '审核状态',
        anchor: '90%',
        editable: false,
        triggerAction: 'all',
        allowBlank: false,
        value: '1'
//        listeners: {
//            'select': function (val) {
//                var result = top.Ext.getCmp("SelectType").getValue();
//                if (result == "1") {//通过
//                    top.Ext.getCmp("Introduction").hide();
//                    top.Ext.getCmp("Introduction").allowBlank = true;
//                }
//                else {//不通过
//                    top.Ext.getCmp("Introduction").show();
//                    top.Ext.getCmp("Introduction").allowBlank = false;
//                }
//            }
//        }
    });
    var form = new top.Ext.FormPanel({
        labelWidth: 65,
        frame: true,
        border: false,
        labelAlign: 'right',
        bodyStyle: 'padding:5px 5px 0',
        id: 'formPanel',
        defaultType: 'textfield',
        items: [
            Attribute,
           {
                    fieldLabel: '审批意见',
                    xtype: 'textarea',
                    id: 'Introduction',
                    name: 'Introduction',
                    height: 100,
                    allowBlank: true,
                    anchor: '90%',
                    emptyText: '可输入审批意见(70字符内)', ////textfield自己的属性
                    maxLength: 70,
                    maxLengthText: '描述长度不能超过70个字符'
                }
        ]
    });

    //自定义button
    var button1 = new top.Ext.Button({
        text: '提交',
        iconCls: 'GTP_submit',
        tooltip: '提交',
        handler: function () {
            var formPanel = top.Ext.getCmp("formPanel");
            if (formPanel.getForm().isValid()) {//如果验证通过
                var para = { Attribute: top.Ext.getCmp("SelectType").getValue() };
                formPanel.getForm().submit({
                    waitTitle: '系统提示', //标题
                    waitMsg: '正在提交数据,请稍后...', //提示信息
                    submitEmptyText: false, //If set to true, the emptyText value will be sent with the form when it is submitted. Defaults to true                                   
                    url: '/Flow/CompanyWaitWork?id=' + key,
                    method: "POST",
                    params: para,
                    success: function (form, action) {
                        //成功后
                        var flag = action.result;
                        if (flag.success) {
                            Ext.getCmp("gg").store.reload();
                            MessageInfo("操作成功！", "right");
                        } else {
                            MessageInfo("操作失败！", "error");
                        }
                        top.Ext.getCmp('window').close();
                    },
                    failure: function (form, action) {
                        MessageInfo("操作失败！", "error");
                    }
                });
            }
        }
    })
   
    var win = WindowDiy("window", title, form, button1);
    win.width = 400;
    win.height = 250;
    return win;
}
































