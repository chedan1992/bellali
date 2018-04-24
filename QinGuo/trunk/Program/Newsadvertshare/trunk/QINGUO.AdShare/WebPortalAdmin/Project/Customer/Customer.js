//加载公司信息表单
function load(title, key) {
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
                                        respon.open("post", "/SysCompany/SearchNameExits?attr=4&key=" + key + "&name=" + encodeURIComponent(val.getValue().trim()), false);
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
                            xtype: 'textfield',
                            name: 'Email',
                            fieldLabel: '邮箱',
                            vtype: 'email',
                            anchor: '90%'
                        },
//                          {
//                                        xtype: 'compositefield',
//                                        fieldLabel: '省市区',
//                                        combineErrors: false,
//                                        anchor: '100%',
//                                        items: [
//                                                 {
//                                                     xtype: 'combo',
//                                                     triggerAction: 'all',
//                                                     id: 'comProvince',
//                                                     emptyText: '所在省',
//                                                     forceSelection: true,
//                                                     editable: false,
//                                                     allowBlank: false,
//                                                     displayField: 'Name',
//                                                     valueField: 'Code',
//                                                     hiddenName: 'Name',
//                                                     width: 80,
//                                                     store: comProvince,
//                                                     listeners: {
//                                                         select: {
//                                                             fn: function (combo, record, index) {
//                                                                 comCity.proxy = new Ext.data.HttpProxy({ url: '/Area/comCity?code=' + record.get('Code'), method: 'POST' });
//                                                                 comCity.load();
//                                                                 top.Ext.getCmp("comCity").setValue(""); //value
//                                                                 top.Ext.getCmp("comCity").setRawValue(""); //text 

//                                                                 top.Ext.getCmp("Bx").setValue("");
//                                                                 top.Ext.getCmp("By").setValue("");
//                                                                 top.Ext.getCmp("Address").setValue(record.get('Name'));
//                                                             }
//                                                         }
//                                                     }
//                                                 },
//                                                 {
//                                                     xtype: 'combo',
//                                                     triggerAction: 'all',
//                                                     id: 'comCity',
//                                                     emptyText: '所在市',
//                                                     forceSelection: true,
//                                                     editable: false,
//                                                     allowBlank: false,
//                                                     displayField: 'Name',
//                                                     valueField: 'Code',
//                                                     hiddenName: 'Name',
//                                                     width: 80,
//                                                     store: comCity,
//                                                     listeners: {
//                                                         select: {
//                                                             fn: function (combo, record, index) {
//                                                                 comArea.proxy = new Ext.data.HttpProxy({ url: '/Area/comArea?code=' + record.get('Code'), method: 'POST' });
//                                                                 comArea.load();
//                                                                 top.Ext.getCmp("comArea").setValue(""); //value
//                                                                 top.Ext.getCmp("comArea").setRawValue(""); //text 

//                                                                 top.Ext.getCmp("Bx").setValue("");
//                                                                 top.Ext.getCmp("By").setValue("");
//                                                                 top.Ext.getCmp("Address").setValue(top.Ext.getCmp("comProvince").getRawValue() + record.get('Name'));
//                                                             }
//                                                         }
//                                                     }
//                                                 },
//                                                 {
//                                                     xtype: 'combo',
//                                                     triggerAction: 'all',
//                                                     id: 'comArea',
//                                                     emptyText: '所在区',
//                                                     forceSelection: true,
//                                                     editable: false,
//                                                     allowBlank: false,
//                                                     displayField: 'Name',
//                                                     hiddenName: 'Name',
//                                                     valueField: 'Code',
//                                                     width: 70,
//                                                     store: comArea,
//                                                     listeners: {
//                                                         select: {
//                                                             fn: function (combo, record, index) {
//                                                                 top.Ext.getCmp("Bx").setValue("");
//                                                                 top.Ext.getCmp("By").setValue("");
//                                                                 top.Ext.getCmp("Address").setValue(top.Ext.getCmp("comProvince").getRawValue() + top.Ext.getCmp("comCity").getRawValue() + record.get('Name'));
//                                                             }
//                                                         }
//                                                     }
//                                                 }
//                                         ]
//                                    }
//                       ,
                        {
                            name: 'Address',
                            fieldLabel: '供应商地址',
                            xtype: 'textfield',
                            id: 'Address',
                            flex: 2,
                            maxLength: 50,
                            maxLengthText: '名称长度不能超过300个字符',
                            anchor: '90%'
                        },
                        {
                            name: 'LicenseNumber',
                            fieldLabel: '营业执照号',
                            xtype: 'textfield',
                            id: 'LicenseNumber',
                            flex: 2,
                            maxLength: 200,
                            maxLengthText: '营业执照号长度不能超过200个字符',
                            anchor: '90%'
                        }
                        ]
                    },
                    {
                        columnWidth: .5,
                        layout: 'form',
                        items: [
//                                  {
//                                      name: 'Code',
//                                      fieldLabel: '<span class="required">*</span>供应商编码',
//                                      xtype: 'textfield',
//                                      id: 'Code',
//                                      maxLength: 50,
//                                      hidden:true,
//                                      allowBlank: false,
//                                      maxLengthText: '编码长度不能超过50个字符',
//                                      anchor: '90%',
//                                      enableKeyEvents: true,
//                                      listeners: {
//                                          'blur': function (val) {
//                                              if (val.getValue()) {
//                                                  var respon = Ext.lib.Ajax.getConnectionObject().conn;
//                                                  respon.open("post", "/SysCompany/SearchCodeExits?attr=4&key=" + key + "&code=" + encodeURIComponent(val.getValue().trim()), false);
//                                                  respon.send(null);
//                                                  var response = Ext.util.JSON.decode(respon.responseText);
//                                                  if (response.success) {
//                                                      top.Ext.Msg.show({
//                                                          title: "信息提示",
//                                                          msg: "编码已经重复,请重新输入！",
//                                                          buttons: Ext.Msg.OK,
//                                                          icon: Ext.MessageBox.INFO,
//                                                          fn: function () {
//                                                              top.Ext.getCmp('Code').setValue("");
//                                                          }
//                                                      });
//                                                  }
//                                              }
//                                          }
//                                      }
//                                  },
                                  {
                                      name: 'Phone',
                                      fieldLabel: '<span class="required">*</span>单位电话',
                                      id: 'Phone',
                                      flex: 2,
                                      xtype: 'textfield',
                                      allowBlank: false,
                                      regex: /^\d+$/,
                                      emptyText: '请输入有效的电话号码',
                                      anchor: '90%'
                                  },
                                   {
                                       name: 'LinkUser',
                                       fieldLabel: '<span class="required">*</span>联系人',
                                       xtype: 'textfield',
                                       id: 'LinkUser',
                                       flex: 2,
                                       allowBlank: false,
                                       anchor: '90%'
                                   }
                                    ,
                                   {
                                       xtype: 'compositefield',
                                       fieldLabel: '位置坐标',
                                       anchor: '100%',
                                       items: [
							            {
							                xtype: 'textfield',
							                id: "Bx",
							                name: "ComPLon",
							                width: 80,
							                height: 22, //固定高度
							                emptyText: 'X',
							                readOnly: true
							            },
                                         {
                                             xtype: 'textfield',
                                             height: 22, //固定高度
                                             id: "By",
                                             name: "CompLat",
                                             width: 80,
                                             emptyText: 'Y',
                                             readOnly: true
                                         },
								        new top.Ext.Button({
								            text: '地图',
								            width: 40,
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
            },
            {
                xtype: 'fieldset',
                title: '管理员信息',
                autoHeight: true,
                layout: 'column',
                items: [
                    {
                        columnWidth: .5,
                        layout: 'form',
                        items: [
                           {
                               name: 'UserName',
                               id: 'UserName',
                               fieldLabel: '<span class="required">*</span>管理员名称',
                               emptyText: '填写管理员名称', ////textfield自己的属性
                               allowBlank: false,
                               xtype: 'textfield',
                               anchor: '90%',
                               maxLength: 20,
                               maxLengthText: '用管理员称长度不能超过20个字符'
                           },
                           {
                               name: 'Pwd',
                               id: 'Pwd',
                               fieldLabel: '登录密码',
                               emptyText: '默认登录密码:666666', ////textfield自己的属性
                               maxLength: 20,
                               xtype: 'textfield',
                               anchor: '90%',
                               maxLengthText: '登录密码长度不能超过20个字符'
                           },
                             {
                                 name: 'UserPhone',
                                 id: 'UserPhone',
                                 fieldLabel: '联系电话',
                                 xtype: 'textfield',
                                 regex: /^\d+$/,
                                 anchor: '90%',
                                 emptyText: '请输入有效的电话号码'
                             }
                        ]
                    },
                    {
                        columnWidth: .5,
                        layout: 'form',
                        items: [

                             {
                                 name: 'LoginName',
                                 id: 'LoginName',
                                 fieldLabel: '<span class="required">*</span>登录账号',
                                 emptyText: '填写登录账号', ////textfield自己的属性
                                 allowBlank: false,
                                 maxLength: 12,
                                 regex: /^\d+$/,
                                 emptyText: '请输入有效的电话号码',
                                 xtype: 'textfield',
                                 maxLengthText: '登录账号长度不能超过20个字符',
                                 anchor: '90%',
                                 enableKeyEvents: true,
                                 listeners: {
                                     'blur': function (val) {
                                         if (val.getValue()) {
                                             return VilivaName(top.Ext.getCmp("UID").getValue(), val.getValue());
                                         }
                                     }
                                 }
                             },

                    {
                        xtype: 'textfield',
                        name: 'UserEmail',
                        id: 'UserEmail',
                        fieldLabel: '邮箱',
                        vtype: 'email',
                        anchor: '90%'
                    },
                    {
                        name: 'UID',
                        id: 'UID',
                        xtype: 'textfield',
                        hidden: true
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
    win.height =510;
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

        var respon = Ext.lib.Ajax.getConnectionObject().conn;
        respon.open("post", "/Customer/GetData?id=" + key, false);
        respon.send(null);
        var response = Ext.util.JSON.decode(respon.responseText);

        form.form.loadRecord(response);

        //管理员信息表单
        var uresopn = Ext.lib.Ajax.getConnectionObject().conn;
        uresopn.open("post", "/SysMaster/MasterInfo?cid=" + response.data.Id, false);
        uresopn.send(null);
        var urs = Ext.util.JSON.decode(uresopn.responseText);
        if (urs.data) {
            top.Ext.getCmp("formPanel").findById("UID").setValue(urs.data.Id);
            top.Ext.getCmp("formPanel").findById("UserName").setValue(urs.data.UserName);
            top.Ext.getCmp("formPanel").findById("LoginName").setValue(urs.data.LoginName);
            top.Ext.getCmp("formPanel").findById("Pwd").setValue(urs.data.Pwd);
            top.Ext.getCmp("formPanel").findById("UserEmail").setValue(urs.data.Email);
            top.Ext.getCmp("formPanel").findById("UserPhone").setValue(urs.data.Phone);
        }
        url = '/Customer/SaveData?Id=' + key + '&modify=1';

        //坐标赋值
        top.$("#Bx").val(response.data.ComPLon);
        top.$("#By").val(response.data.CompLat);

//        //加载省市区
//        if (response.data.Province != null) {
//            var result = Ext.lib.Ajax.getConnectionObject().conn;
//            result.open("post", "/Customer/GetArea?companyId=" + key, false);
//            result.send(null);
//            var resultse = Ext.util.JSON.decode(result.responseText);
//            if (resultse) {
//                //省
//                top.Ext.getCmp("comProvince").setValue(resultse.data.Province); //value
//                top.Ext.getCmp("comProvince").setRawValue(resultse.data.ProvinceName); //text
//                //市
//                if (resultse.data.CityId != null) {
//                    top.Ext.getCmp("comCity").setValue(resultse.data.CityId); //value
//                    top.Ext.getCmp("comCity").setRawValue(resultse.data.CityName); //text
//                }
//                //市
//                if (resultse.data.AreaId != null) {
//                    top.Ext.getCmp("comArea").setValue(resultse.data.AreaId); //value
//                    top.Ext.getCmp("comArea").setRawValue(resultse.data.AreaName); //text
//                }
//            }
//        }

//       
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

//        //获取区和商圈
//        var comProvince = top.Ext.getCmp("comProvince").getValue();
//        var comCity = top.Ext.getCmp("comCity").getValue();
//        var comArea = top.Ext.getCmp("comArea").getValue();

        //获取
        var para = { x: top.$("#Bx").val(), y: top.$("#By").val()};
        if (win.title == "编辑") {
            //得到选后的数据   
            var rows = grid.getSelectionModel().getSelections();
            para = { Id: rows[0].get("Id"), x: top.$("#Bx").val(), y: top.$("#By").val() };
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































