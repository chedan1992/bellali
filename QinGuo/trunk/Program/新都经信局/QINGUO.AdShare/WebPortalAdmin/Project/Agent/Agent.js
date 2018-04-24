//加载厂家信息表单
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

    if (title == "编辑") {
        var grid = Ext.getCmp("gg");
        //得到选后的数据   
        var rows = grid.getSelectionModel().getSelections();
        key = rows[0].get("Id");
//        if (rows[0].get("CityId") != "") {
//            comCity.proxy = new Ext.data.HttpProxy({ url: '/Area/comCity?code=' + rows[0].get("Province"), method: 'POST' });
//            comCity.load();
//        }
//        if (rows[0].get("AreaId") != "") {
//            comArea.proxy = new Ext.data.HttpProxy({ url: '/Area/comArea?code=' + rows[0].get('CityId'), method: 'POST' });
//            comArea.load();
//        }
    }

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
                title: "单位信息",
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
                                    'blur': function (val) {
                                        if (val.getValue()) {
                                            top.Ext.getCmp("Code").setValue(codeConvert(top.Ext.getCmp("Name").getValue()));
                                            var respon = Ext.lib.Ajax.getConnectionObject().conn;
                                            respon.open("post", "/SysCompany/SearchNameExits?attr=3&key=" + key + "&name=" + encodeURIComponent(val.getValue().trim()), false);
                                            respon.send(null);
                                            var response = Ext.util.JSON.decode(respon.responseText);
                                            if (response.success) {
                                                top.Ext.Msg.show({
                                                    title: "信息提示",
                                                    msg: "单位名称已经重复,请重新输入！",
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
                                  name: 'LinkUser',
                                  fieldLabel: '<span class="required">*</span>联系人',
                                  xtype: 'textfield',
                                  allowBlank: false,
                                  id: 'LinkUser',
                                  flex: 2,
                                  anchor: '90%'
                              },
//                            {
//                                xtype: 'compositefield',
//                                fieldLabel: '<span class="required">*</span>省市区',
//                                combineErrors: false,
//                                anchor: '100%',
//                                items: [
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

//                                                                 //判断是否选择全部
//                                                                 if (record.get('Code') == "-1") {
//                                                                     top.Ext.getCmp("comCity").setDisabled(true);
//                                                                     top.Ext.getCmp("comArea").setDisabled(true);
//                                                                     top.Ext.getCmp("comCity").setValue("");
//                                                                     top.Ext.getCmp("comArea").setValue("");
//                                                                     top.Ext.getCmp("comCity").allowBlank = true;
//                                                                     top.Ext.getCmp("comArea").allowBlank = true;

//                                                                     top.Ext.getCmp("Address").setValue("");
//                                                                 }
//                                                                 else {

//                                                                     top.Ext.getCmp("comCity").setDisabled(false);
//                                                                     top.Ext.getCmp("comArea").setDisabled(false);
//                                                                     top.Ext.getCmp("comCity").allowBlank = false;
//                                                                     top.Ext.getCmp("comArea").allowBlank = false;

//                                                                     comCity.proxy = new Ext.data.HttpProxy({ url: '/Area/comCity?code=' + record.get('Code'), method: 'POST' });
//                                                                     comCity.load();
//                                                                     top.Ext.getCmp("comCity").setValue(""); //value
//                                                                     top.Ext.getCmp("comCity").setRawValue(""); //text 

//                                                                     top.Ext.getCmp("Bx").setValue("");
//                                                                     top.Ext.getCmp("By").setValue("");
//                                                                     top.Ext.getCmp("Address").setValue(record.get('Name'));
//                                                                 }
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
//                                                                 if (record.get('Code') == "-1") {
//                                                                     top.Ext.getCmp("comArea").setDisabled(true);
//                                                                     top.Ext.getCmp("comArea").allowBlank = true;
//                                                                     top.Ext.getCmp("comArea").setValue("");

//                                                                     top.Ext.getCmp("Address").setValue(top.Ext.getCmp("comProvince").getRawValue() );
//                                                                 }
//                                                                 else {
//                                                                     top.Ext.getCmp("comArea").setDisabled(false);
//                                                                     top.Ext.getCmp("comArea").allowBlank = false;
//                                                                     comArea.proxy = new Ext.data.HttpProxy({ url: '/Area/comArea?code=' + record.get('Code'), method: 'POST' });
//                                                                     comArea.load();
//                                                                     top.Ext.getCmp("comArea").setValue(""); //value
//                                                                     top.Ext.getCmp("comArea").setRawValue(""); //text 

//                                                                     top.Ext.getCmp("Bx").setValue("");
//                                                                     top.Ext.getCmp("By").setValue("");
//                                                                     top.Ext.getCmp("Address").setValue(top.Ext.getCmp("comProvince").getRawValue() + record.get('Name'));
//                                                                 }
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
//                                                                 //判断是否选择全部
//                                                                 if (record.get('Code') == "-1") {
//                                                                     top.Ext.getCmp("Address").setValue(top.Ext.getCmp("comProvince").getRawValue() + top.Ext.getCmp("comCity").getRawValue());
//                                                                 }
//                                                                 else {
//                                                                     top.Ext.getCmp("Bx").setValue("");
//                                                                     top.Ext.getCmp("By").setValue("");
//                                                                     top.Ext.getCmp("Address").setValue(top.Ext.getCmp("comProvince").getRawValue() + top.Ext.getCmp("comCity").getRawValue() + record.get('Name'));
//                                                                 }
//                                                             }
//                                                         }
//                                                     }
//                                                 }
//                                         ]
//                            },
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
                                  name: 'LicenseNumber',
                                  fieldLabel: '<span class="required">*</span>营业执照号',
                                  xtype: 'textfield',
                                  id: 'LicenseNumber',
                                  allowBlank: false,
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
                            {
                                name: 'Code',
                                fieldLabel: '单位编码',
                                xtype: 'textfield',
                                id: 'Code',
                                flex: 2,
                                hidden:true,
                                maxLength: 50,
                                maxLengthText: '编码长度不能超过50个字符',
                                anchor: '90%',
                                enableKeyEvents: true,
                                listeners: {
                                    'blur': function (val) {
                                        if (val.getValue()) {
                                            var respon = Ext.lib.Ajax.getConnectionObject().conn;
                                            respon.open("post", "/SysCompany/SearchCodeExits?attr=3&key=" + key + "&code=" + encodeURIComponent(val.getValue().trim()), false);
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
                                fieldLabel: '<span class="required">*</span>单位电话',
                                id: 'Phone',
                                allowBlank: false,
                                flex: 2,
                                xtype: 'textfield',
                                regex: /^\d+$/,
                                emptyText: '请输入有效的电话号码',
                                anchor: '90%'
                            },
                               {
                                   xtype: 'textfield',
                                   name: 'Email',
                                   fieldLabel: '邮箱',
                                   vtype: 'email',
                                   anchor: '90%'
                               },
                           {
                               xtype: 'compositefield',
                               fieldLabel: '位置坐标',
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
                         columnWidth: 1,
                         layout: 'form',
                         autoHeight: true,
                         items: [
                         {
                             fieldLabel: '单位介绍',
                             xtype: 'htmleditor',
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
            },
              {
                  xtype: 'fieldset',
                  title: '登录账号',
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
                               fieldLabel: '<span class="required">*</span>账号名称',
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
                                             return VilivaName(top.Ext.getCmp("UID").getValue(), val.getValue()); //验证管理员登录账号是否重复
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
    win.height =515;
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
//启用
function EnableUse() {
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        if (rows[0].get("Status") == "1") {
            MessageInfo("该单位已经启用！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要启用该单位吗?', function (e) {
            if (e == "yes") {
                var key = rows[0].get("Id");
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
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要禁用该单位吗?', function (e) {
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
    var win = load("新增","");
    url = '/Agent/SaveData?1=1';
    win.show();
}
//编辑
function EditDate() {
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].get("Id");
        var win = load("编辑", key);
        win.show();

        var respon = Ext.lib.Ajax.getConnectionObject().conn;
        respon.open("post", "/BIZ/GetData?id=" + key, false);
        respon.send(null);
        var response = Ext.util.JSON.decode(respon.responseText);

        var form = top.Ext.getCmp('formPanel');
        form.form.loadRecord(response);

        url = '/Agent/SaveData?Id=' + key + '&modify=1';

        //管理员信息表单
        var uresopn = Ext.lib.Ajax.getConnectionObject().conn;
        uresopn.open("post", "/SysMaster/MasterInfo?cid=" + key, false);
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

//        //加载省市区
//        if (response.data.Province == "-1") {
//            //省
//            top.Ext.getCmp("comProvince").setValue("-1"); //value
//            top.Ext.getCmp("comProvince").setRawValue("全国"); //text
//            top.Ext.getCmp("comCity").setDisabled(true);
//            top.Ext.getCmp("comArea").setDisabled(true);
//            top.Ext.getCmp("comCity").setValue("");
//            top.Ext.getCmp("comArea").setValue("");
//            top.Ext.getCmp("comCity").allowBlank = true;
//            top.Ext.getCmp("comArea").allowBlank = true;

//        }
//        else {
//            if (response.data.Province != null) {
//                var result = Ext.lib.Ajax.getConnectionObject().conn;
//                result.open("post", "/Customer/GetArea?companyId=" + key, false);
//                result.send(null);
//                var resultse = Ext.util.JSON.decode(result.responseText);
//                if (resultse) {
//                    //省
//                    top.Ext.getCmp("comProvince").setValue(resultse.data.Province); //value
//                    top.Ext.getCmp("comProvince").setRawValue(resultse.data.ProvinceName); //text
//                    //市
//                    if (resultse.data.CityId == "-1") {
//                        //省
//                        top.Ext.getCmp("comCity").setValue("-1"); //value
//                        top.Ext.getCmp("comCity").setRawValue("全部"); //text

//                        top.Ext.getCmp("comArea").setDisabled(true);
//                        top.Ext.getCmp("comArea").allowBlank = true;
//                        top.Ext.getCmp("comArea").setValue("");
//                    }
//                    else {
//                        if (resultse.data.CityId != null) {
//                            top.Ext.getCmp("comCity").setValue(resultse.data.CityId); //value
//                            top.Ext.getCmp("comCity").setRawValue(resultse.data.CityName); //text
//                        }
//                        //市
//                        if (resultse.data.AreaId == "-1") {
//                            //省
//                            top.Ext.getCmp("comArea").setValue("-1"); //value
//                            top.Ext.getCmp("comArea").setRawValue("全部"); //text
//                        }
//                        else {
//                            if (resultse.data.AreaId != null) {
//                                top.Ext.getCmp("comArea").setValue(resultse.data.AreaId); //value
//                                top.Ext.getCmp("comArea").setRawValue(resultse.data.AreaName); //text
//                            }
//                        }
//                    }
//                }
//            }
//        }
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
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要删除该单位吗?', function (e) {
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
        //获取区和商圈
//        var comProvince = top.Ext.getCmp("comProvince").getValue();
//        var comCity = top.Ext.getCmp("comCity").getValue();
//        var comArea = top.Ext.getCmp("comArea").getValue();
        //获取
        var para = { x: top.$("#Bx").val(), y: top.$("#By").val()};

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


//grid双击
function dbGridClick(grid, rowindex, e) {
    EditDate();
}


