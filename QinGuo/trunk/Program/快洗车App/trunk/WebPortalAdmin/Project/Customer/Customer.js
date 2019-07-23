//加载公司信息表单
function load(title, key) {

    //供应商分类
    var Nature = new Ext.data.Store({
        proxy: new Ext.data.HttpProxy({
            url: '/SysDirc/GetSysDicByType?Type=1',
            method: 'POST'
        }),
        reader: new Ext.data.JsonReader({},
            Ext.data.Record.create(["id", "text"]
        ))
    });

//    /*
//    加载省
//    */
//    var comProvince = new Ext.data.Store({
//        proxy: new Ext.data.HttpProxy({
//            url: '/Area/comProvince',
//            method: 'POST'
//        }),
//        reader: new Ext.data.JsonReader({},
//            Ext.data.Record.create(["Code", "Name"]
//        ))
//    });
//    /*
//    加载市区
//    */
//    var comCity = new Ext.data.Store({
//        proxy: new Ext.data.HttpProxy({
//            url: '/Area/comCity',
//            method: 'POST'
//        }),
//        reader: new Ext.data.JsonReader({},
//            Ext.data.Record.create(["Code", "Name"]
//        ))
//    });
//    /*
//    加载区
//    */
//    var comArea = new Ext.data.Store({
//        proxy: new Ext.data.HttpProxy({
//            url: '/Area/comArea',
//            method: 'POST'
//        }),
//        reader: new Ext.data.JsonReader({},
//            Ext.data.Record.create(["Code", "Name"]
//        ))
//    });
    var key = "";
    if (title == "编辑") {
        var grid = Ext.getCmp("gg");
        //得到选后的数据   
        var rows = grid.getSelectionModel().getSelections();
        key = rows[0].get("Id");

        //供应商分类
        if (rows[0].get("Nature") != "") {
            Nature.proxy = new Ext.data.HttpProxy({ url: '/SysDirc/GetSysDicByType?Type=1', method: 'POST' });
            Nature.load();
        }

        //comProvince.proxy = new Ext.data.HttpProxy({ url: '/Area/comProvince?code=' + rows[0].get("Province"), method: 'POST' });
//        //comProvince.load();
//        if (rows[0].get("CityId") != "") {
//            comCity.proxy = new Ext.data.HttpProxy({ url: '/Area/comCity?code=' + rows[0].get("Province"), method: 'POST' });
//            comCity.load();
//        }
//        if (rows[0].get("AreaId") != "") {
//            comArea.proxy = new Ext.data.HttpProxy({ url: '/Area/comArea?code=' + rows[0].get('CityId'), method: 'POST' });
//            comArea.load();
//        }
    }

    var form = new top.Ext.FormPanel({
        id: "formPanel",
        labelAlign: 'right',
        frame: true,
        bodyStyle: 'padding:5px 5px 0',
        width: 650,
        labelWidth: 80,
        fileUpload: true,
        autoScroll: true,
        border: false,
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
                            fieldLabel: '<span class="required">*</span>供应商名称',
                            xtype: 'textfield',
                            id: 'Name',
                            maxLength: 50,
                            allowBlank: false,
                            emptyText: '填写供应商名称',
                            maxLengthText: '供应商名称长度不能超过50个字符',
                            anchor: '90%',
                            enableKeyEvents: true,
                            listeners: {
                                'blur': function (val) {
                                    if (val.getValue()) {
                                        var respon = Ext.lib.Ajax.getConnectionObject().conn;
                                        respon.open("post", "/SysCompany/SearchNameExits?attr=5&key=" + key + "&name=" + encodeURIComponent(val.getValue().trim()), false);
                                        respon.send(null);
                                        var response = Ext.util.JSON.decode(respon.responseText);
                                        if (response.success) {
                                            top.Ext.Msg.show({
                                                title: "信息提示",
                                                msg: "供应商名称已经重复,请重新输入！",
                                                buttons: Ext.Msg.OK,
                                                icon: Ext.MessageBox.INFO,
                                                fn: function () {
                                                    top.Ext.getCmp('Name').setValue("");
                                                }
                                            });
                                        }
                                    }
                                }
                            }
                        },
                        {
                            name: 'Address',
                            fieldLabel: '联系地址',
                            xtype: 'textfield',
                            id: 'Address',
                            flex: 2,
                            maxLength: 50,
                            maxLengthText: '名称长度不能超过300个字符',
                            anchor: '90%'
                        },
                        {
                                 xtype: 'combo',
                                 triggerAction: 'all',
                                 id: 'TypeName',
                                 name: 'TypeName',
                                 fieldLabel: '<span class="required">*</span>供应商分类',
                                 emptyText: '==选择==',
                                 forceSelection: true,
                                 editable: true,
                                 typeAhead: true, //模糊查询
                                 allowBlank: false,
                                 displayField: 'text',
                                 valueField: 'id',
                                 hiddenName: 'text',
                                 anchor: '90%',
                                 store: Nature,
                                 listeners: {
                                     beforequery: function (e) {
                                         var combo = e.combo;
                                         if (!e.forceAll) {
                                             var input = e.query;
                                             // 检索的正则  
                                             var regExp = new RegExp(".*" + input + ".*");
                                             // 执行检索  
                                             combo.store.filterBy(function (record, id) {
                                                 // 得到每个record的项目名称值  
                                                 var text = record.get(combo.displayField);
                                                 return regExp.test(text);
                                             });
                                             combo.expand();
                                             return false;
                                         }
                                     }
                                 }
                             }
                       ,
                          new top.Ext.form.ComboBox({
                                        store: new Ext.data.SimpleStore({
                                            fields: ['text', 'value'],
                                            data: [['不设置', '0'], ['月结', '1'], ['日结', '2']]
                                        }),
                                        fieldLabel: '结账方式',
                                        displayField: 'text',
                                        valueField: 'value',
                                        mode: 'local',
                                        id: 'CheckoutType',
                                        name: 'CheckoutType',
                                        selectOnFocus: true,
                                        orceSelection: true,
                                        editable: false,
                                        triggerAction: 'all',
                                        value: '0',
                                        anchor: '90%',
                                        width: 60
                                    }),
                                   new top.Ext.form.ComboBox({
                                       store: new Ext.data.SimpleStore({
                                              fields: ['text', 'value'],
                                              data: [['不设置', '0'], ['支付宝', '1'], ['工行', '2'], ['农行', '3']]
                                          }),
                                          fieldLabel: '付款方式',
                                          displayField: 'text',
                                          valueField: 'value',
                                          mode: 'local',
                                          id: 'PaymentType',
                                          name: 'PaymentType',
                                          selectOnFocus: true,
                                          orceSelection: true,
                                          value:'0',
                                          editable: false,
                                          triggerAction: 'all',
                                          anchor: '90%',
                                          allowBlank: false,
                                          width: 60
                                      })
                        
                        ]
                    },
                    {
                        columnWidth: .5,
                        layout: 'form',
                        items: [
                                  {
                                      name: 'Code',
                                      fieldLabel: '<span class="required">*</span>供应商编码',
                                      xtype: 'textfield',
                                      id: 'Code',
                                      maxLength: 50,
                                      allowBlank: false,
                                      maxLengthText: '编码长度不能超过50个字符',
                                      anchor: '90%',
                                      enableKeyEvents: true,
                                      listeners: {
                                          'blur': function (val) {
                                              if (val.getValue()) {
                                                  var respon = Ext.lib.Ajax.getConnectionObject().conn;
                                                  respon.open("post", "/SysCompany/SearchCodeExits?attr=5&key=" + key + "&code=" + encodeURIComponent(val.getValue().trim()), false);
                                                  respon.send(null);
                                                  var response = Ext.util.JSON.decode(respon.responseText);
                                                  if (response.success) {
                                                      top.Ext.Msg.show({
                                                          title: "信息提示",
                                                          msg: "编码已经重复,请重新输入！",
                                                          buttons: Ext.Msg.OK,
                                                          icon: Ext.MessageBox.INFO,
                                                          fn: function () {
                                                              top.Ext.getCmp('Code').setValue("");
                                                          }
                                                      });
                                                  }
                                              }
                                          }
                                      }
                                  },
                                  {
                                      name: 'Phone',
                                      fieldLabel: '电话',
                                      id: 'Phone',
                                      flex: 2,
                                      xtype: 'textfield',
                                      regex: /^\d+$/,
                                      emptyText: '请输入有效的电话号码',
                                      anchor: '90%'
                                  },
                                  {
                                      name: 'Tel',
                                       fieldLabel: '手机',
                                       xtype: 'textfield',
                                       id: 'Tel',
                                       flex: 2,
                                       anchor: '90%'
                                   },
                                  {
                                      name: 'AccountName',
                                      fieldLabel: '名字',
                                      xtype: 'textfield',
                                      id: 'AccountName',
                                      flex: 2,
                                      maxLength: 200,
                                      maxLengthText: '名字长度不能超过20个字符',
                                      anchor: '90%'
                                  },
                                {
                                    name: 'AccountNum',
                                    fieldLabel: '账号',
                                    xtype: 'textfield',
                                    id: 'AccountNum',
                                    flex: 2,
                                    maxLength: 50,
                                    maxLengthText: '账号长度不能超过50个字符',
                                    anchor: '90%'
                                }
                                      
                                 
                           ]
                    },
                    {
                        columnWidth: 1,
                        layout: 'form',
                        items: [
                         {
                             fieldLabel: '供应商介绍',
                             xtype: 'htmleditor',
                             id: 'Introduction',
                             name: 'Introduction',
                             height:120,
                             emptyText: '可输入对供应商的介绍信息', ////textfield自己的属性
                             anchor: '96%'
                         }
                        ]
                    }
                ]
            }
//            {
//                xtype: 'fieldset',
//                title: '管理员信息',
//                autoHeight: true,
//                layout: 'column',
//                items: [
//                    {
//                        columnWidth: .5,
//                        layout: 'form',
//                        items: [
//                           {
//                               name: 'UserName',
//                               id: 'UserName',
//                               fieldLabel: '<span class="required">*</span>管理员名称',
//                               emptyText: '填写管理员名称', ////textfield自己的属性
//                               allowBlank: false,
//                               xtype: 'textfield',
//                               anchor: '90%',
//                               maxLength: 20,
//                               maxLengthText: '用管理员称长度不能超过20个字符'
//                           },
//                           {
//                               name: 'Pwd',
//                               id: 'Pwd',
//                               fieldLabel: '登录密码',
//                               emptyText: '默认登录密码:666666', ////textfield自己的属性
//                               maxLength: 20,
//                               xtype: 'textfield',
//                               anchor: '90%',
//                               maxLengthText: '登录密码长度不能超过20个字符'
//                           },
//                             {
//                                 name: 'UserPhone',
//                                 id: 'UserPhone',
//                                 fieldLabel: '联系电话',
//                                 xtype: 'textfield',
//                                 regex: /^\d+$/,
//                                 anchor: '90%',
//                                 emptyText: '请输入有效的电话号码'
//                             }
//                        ]
//                    },
//                    {
//                        columnWidth: .5,
//                        layout: 'form',
//                        items: [

//                             {
//                                 name: 'LoginName',
//                                 id: 'LoginName',
//                                 fieldLabel: '<span class="required">*</span>登录账号',
//                                 emptyText: '填写登录账号', ////textfield自己的属性
//                                 allowBlank: false,
//                                 maxLength: 12,
//                                 regex: /^\d+$/,
//                                 emptyText: '请输入有效的电话号码',
//                                 xtype: 'textfield',
//                                 maxLengthText: '登录账号长度不能超过20个字符',
//                                 anchor: '90%',
//                                 enableKeyEvents: true,
//                                 listeners: {
//                                     'blur': function (val) {
//                                         if (val.getValue()) {
//                                             return VilivaName(top.Ext.getCmp("UID").getValue(), val.getValue());
//                                         }
//                                     }
//                                 }
//                             },

//                    {
//                        xtype: 'textfield',
//                        name: 'UserEmail',
//                        id: 'UserEmail',
//                        fieldLabel: '邮箱',
//                        vtype: 'email',
//                        anchor: '90%'
//                    },
//                    {
//                        name: 'UID',
//                        id: 'UID',
//                        xtype: 'textfield',
//                        hidden: true
//                    }
//                          ]

//                    }

//                ]
//            }
        ]
    });

    //窗体
    var win = Window("window", title, form);
    win.width = 780;
    win.height =470;
    return win;
}

//加载公司信息表单
function loadAll(title, key) {
    var key = "";
    if (title == "编辑") {
        var grid = Ext.getCmp("gg");
        //得到选后的数据   
        var rows = grid.getSelectionModel().getSelections();
        key = rows[0].get("Id");
    }
    var form = new top.Ext.FormPanel({
        id: "formPanel",
        labelAlign: 'right',
        frame: true,
        bodyStyle: 'padding:5px 5px 0',
        width: 650,
        labelWidth: 80,
        fileUpload: true,
        autoScroll: true,
        border: false,
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
                          new top.Ext.form.ComboBox({
                              store: new Ext.data.SimpleStore({
                                  fields: ['text', 'value'],
                                  data: [['不设置', '0'], ['月结', '1'], ['日结', '2']]
                              }),
                              fieldLabel: '结账方式',
                              displayField: 'text',
                              valueField: 'value',
                              mode: 'local',
                              id: 'CheckoutType',
                              selectOnFocus: true,
                              orceSelection: true,
                              editable: false,
                              triggerAction: 'all',
                              value: '0',
                              anchor: '90%',
                              width: 60
                          }),
                          new top.Ext.form.ComboBox({
                                       store: new Ext.data.SimpleStore({
                                           fields: ['text', 'value'],
                                           data: [['不设置', '0'], ['支付宝', '1'], ['工行', '2'], ['农行', '3']]
                                       }),
                                       fieldLabel: '付款方式',
                                       displayField: 'text',
                                       valueField: 'value',
                                       mode: 'local',
                                       id: 'PaymentType',
                                       selectOnFocus: true,
                                       orceSelection: true,
                                       value: '0',
                                       editable: false,
                                       triggerAction: 'all',
                                       anchor: '90%',
                                       allowBlank: false,
                                       width: 60
                                   })
                        ]
                    },
                    {
                        columnWidth: .5,
                        layout: 'form',
                        items: [
                                  {
                                      fieldLabel: '名字',
                                      xtype: 'textfield',
                                      id: 'AccountName',
                                      flex: 2,
                                      maxLength: 200,
                                      maxLengthText: '名字长度不能超过20个字符',
                                      anchor: '90%'
                                  },
                                {
                                    fieldLabel: '账号',
                                    xtype: 'textfield',
                                    id: 'AccountNum',
                                    flex: 2,
                                    maxLength: 50,
                                    maxLengthText: '账号长度不能超过50个字符',
                                    anchor: '90%'
                                }
                           ]
                    }
                ]
            }
        ]
    });

    //窗体
    var win = Window("window", title, form);
    win.width = 780;
    win.height =370;
    return win;
}
///验证管理员账号是否重复
function VilivaName(key, value) {
    if (!value.trim().match(/^(((13[0-9]{1})|(15[0-9]{1})|(18[0-9]{1}))+\d{8})$/)) {
        top.Ext.Msg.show({
            title: "信息提示",
            msg: "手机号码格式不正确",
            buttons: Ext.Msg.OK,
            icon: Ext.MessageBox.INFO,
            fn: function () {
                top.Ext.getCmp("LoginName").setValue("");
            }
        });
        return false;
    }
    var respon = Ext.lib.Ajax.getConnectionObject().conn;
    respon.open("post", "/SysMaster/ExitsMaster?key=" + key + "&code=" + encodeURIComponent(value.trim()), false);
    respon.send(null);
    var response = Ext.util.JSON.decode(respon.responseText);
    if (response.success) {
        top.Ext.Msg.show({
            title: "信息提示",
            msg: "该登录账号已被注册使用,请重新输入",
            buttons: Ext.Msg.OK,
            icon: Ext.MessageBox.INFO,
            fn: function () {
                top.Ext.getCmp("LoginName").setValue("");
            }
        });
        return false;
    }
}

//渲染结账方式
function formartCheckoutType(val, row) {
    if (val ==0)
        return "";
    else if (val == 1) {
        return "月结";
    }
    else if (val ==2) {
        return "日结";
    } 
}
//渲染付款方式
function formartPaymentType(val, row) {
    if (val == 0)
        return "";
    else if (val == 1) {
        return "支付宝";
    }
    else if (val == 2) {
        return "工行";
    } else if (val == 3) {
        return "农行";
    }
}

/**************************方法************************************/

//启用
function EnableUse() {
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        if (rows[0].get("Status") == "1") {
            MessageInfo("该供应商已经启用！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要启用该供应商吗?', function (e) {
            if (e == "yes") {
                var key = rows[0].get("Id");
                Ext.Ajax.request({
                    url: '/Customer/EnableUse',
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
            MessageInfo("该供应商已经禁用！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要禁用该供应商吗?', function (e) {
            if (e == "yes") {
                var key = rows[0].get("Id");
                Ext.Ajax.request({
                    url: '/Customer/DisableUse',
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
    result = [];
    parent.idArray = [];
    url = '/Customer/SaveData?1=1';
    var win = load("新增", "");
    win.show();
}
//编辑
function EditDate() {
    result = [];
    parent.idArray = [];
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].get("Id");
        var win = load("编辑", key);
        var form = top.Ext.getCmp('formPanel');
        form.form.loadRecord(rows[0]);
        win.show();
        //供应商分类
        top.Ext.getCmp("TypeName").setValue(rows[0].data.Type); //value
        top.Ext.getCmp("TypeName").setRawValue(rows[0].data.TypeName); //text
        url = '/Customer/SaveData?Id=' + key + '&modify=1';
       
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}

//批量编辑
function GTP_signnameadd() {
    result = [];
    parent.idArray = [];
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length >= 1) {
        var ids = [];
        for (var i = 0; i < rows.length; i++) {
            ids.push("'" + rows[i].data["Id"] + "'");
        }
        var win = loadAll("批量编辑", '');
        url = '/Customer/SaveDataAll?modify=1&IdList=' + ids.join(',');
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
    if (rows.length >= 1) {
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要删除供应商吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push("'" + rows[i].data["Id"] + "'");
                }
                Ext.Ajax.request({
                    url: '/Customer/DeleteData',
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
var SaveDate = function () {
    var grid = Ext.getCmp("gg");
    var formPanel = top.Ext.getCmp("formPanel");
    var win = top.Ext.getCmp("window");
    if (formPanel.getForm().isValid()) {//如果验证通过

        var Type =''; //供应商类别
        if (top.Ext.getCmp("TypeName")) {
            Type = top.Ext.getCmp("TypeName").getValue(); //供应商类别
        }
        var CheckoutType = top.Ext.getCmp("CheckoutType").getValue();
        var PaymentType = top.Ext.getCmp("PaymentType").getValue();

        var para = { LoginUserId: getLoginUser().Id, Type: Type, ComPaymentType: PaymentType, ComCheckoutType: CheckoutType };
        if (win.title == "编辑") {
            //得到选后的数据   
            var rows = grid.getSelectionModel().getSelections();
            para = { LoginUserId: getLoginUser().Id, Id: rows[0].get("Id"), Type: Type, ComPaymentType: PaymentType, ComCheckoutType: CheckoutType };
        }

        formPanel.getForm().submit({
            waitTitle: '系统提示', //标题
            waitMsg: '正在提交数据,请稍后...', //提示信息
            submitEmptyText: false,
            method: "POST",
            url: url,
            params: para,
            success: function (form, action) {
                //成功后
                var flag = action.result;
                if (flag.success) {
                    MessageInfo("保存成功！", "right");
                    win.close();
                    Ext.getCmp("gg").store.reload();
                } else {
                    MessageInfo(flag.msg, "error");
                }
            },
            failure: function (form, action) {
                MessageInfo("保存失败！", "error");
            }
        });
    }
};































