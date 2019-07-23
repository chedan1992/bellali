//汽配商资质管理表单
function btnMangClick() {
    //弹出窗体
    var win = new top.Ext.Window({
        id: "btnMangWin",
        title: "商家资质管理",
        shadow: false,
        stateful: false,
        width: 500,
        height: 500,
        minHeight: 400,
        //layout: 'fit',
        border: false,
        closeAction: 'close',
        items: {
            //autoScroll: true,
            //frame: true,
            autoLoad: {
                url: "/BIZ/AptitudeImg",
                scripts: true,
                nocache: true
            },
            params: { result: result }
        }
    });
    win.show();
}
//设置上传图片状态
function HProPicFileUploadAction(o) {
    if (checkFile(o)) {
        //标示图隐藏赋值
        top.Ext.getCmp('HProPic').setValue("1");
        //    填充图片
        if (navigator.userAgent.indexOf("Firefox") >= 1) {
            top.Ext.getCmp("ProPicImg").getEl().dom.src = top.window.URL.createObjectURL("file://" + top.Ext.getCmp("ProPic").value);
        } else {
            //      IE浏览器
            top.document.getElementById("ProPicImg").filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = "file://" + top.Ext.getCmp("ProPic").value;
            top.document.getElementById("ProPicImg").innerHTML = "";
        }
    }
}
//设置上传图片状态
function HBackPicFileUploadAction(o) {
    if (checkFile(o)) {
        //标示图隐藏赋值
        top.Ext.getCmp('HBackPic').setValue("1");
        //    填充图片
        if (navigator.userAgent.indexOf("Firefox") >= 1) {
            top.Ext.getCmp("BackPicImg").getEl().dom.src = top.window.URL.createObjectURL("file://" + top.Ext.getCmp("BackPic").value);
        } else {
            //      IE浏览器
            top.document.getElementById("BackPicImg").filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = "file://" + top.Ext.getCmp("BackPic").value;
            top.document.getElementById("BackPicImg").innerHTML = "";
        }
    }
}
//设置上传合同文件状态
function HPactFileUploadAction(o) {
    //验证同文件的正则  
    var img_reg = /\.([zZ][iI][pP]){1}$|\.([rR][aA][rR]){1}$|\.([dD][oO][cC]){1}$|\.([dD][oO][cC][xX]){1}$|\.([jJ][pP][gG]){1}$|\.([jJ][pP][eE][gG]){1}$|\.([gG][iI][fF]){1}$|\.([pP][nN][gG]){1}$|\.([bB][mM][pP]){1}$/;
    if (!img_reg.test(o.value)) {
        top.Ext.Msg.show({ title: "信息提示", msg: "文件类型错误,请选择文件(biz/rar/doc/docx/jpg/jpeg/gif/png/bmp)", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
        o.setRawValue('');
        return;
    }
    //标示图隐藏赋值
    top.Ext.getCmp('HPact').setValue("1");
}
parent.result = []; //汽配商资质返回结果
var result = [];

//文件上传成功后的回调函数
var onUploadSuccess = function (dialog, filename, resp_data, record) {
    var resultNew = [];
    if (parent.idArray.length == 0) { resultNew = result; } else {
        $.each(result, function (i, n) {
            if (n != null) {
                var isbool = true;
                for (var j = 0; j < parent.idArray.length; j++) {
                    if (n.Id == parent.idArray[j]) {
                        isbool = false;
                        return;
                    }
                }
                if (isbool) {
                    var mod = { "PicUrl": n.PicUrl, "Name": n.Name, "Id": n.Id };
                    resultNew.push(mod);
                }
            }
        });
    }


    var mod = { "PicUrl": resp_data.PicUrl, "Name": resp_data.Name, "Id": resp_data.Id };

    resultNew.push(mod);

    //{ "PicUrl": resp_data.PicUrl, "Name": resp_data.Name, "Id": resp_data.Id};
    //处理逻辑：
    result = resultNew;
    parent.result = result;
};


//文件上传失败后的回调函数  
var onUploadFailed = function (dialog, filename, resp_data, record) {
    top.Ext.MessageBox.show({
        title: "提示",
        msg: "上传失败！",
        buttons: { "ok": "确定" },
        width: 250,
        icon: Ext.MessageBox.INFO
    });
};

//文件上传完成后的回调函数
var onUploadComplete = function (dialog) {
    var img = "";
    $.each(result, function (i, n) {
        if (n != null) {
            var Id = "'" + n.Id + "'";
            img += '<div id="' + n.Id + '" style="position:absoulute;width:30px;float:left;margin-right:3px;" id="zzdiv"><img onclick="javascript:imgfun(' + Id + ')" width="13" height="13" src="/Common/icons/toolbar/GTP_decline.png" title="删除" style="cursor:pointer;position:relative;top:3px;z-index:2;"/><img style="top:-5px;z-index:1;" src="' + n.PicUrl + '" id="' + n.Id + '" width="30" height="30" title="' + n.Name + '""/></div>';
        }
    });
    top.Ext.getCmp("Plzz").update(img); //value
};

//上传资质
function AptitudeImg() {
    var dialog = new top.Ext.ux.UploadDialog.Dialog({
        title: '上传资质',
        url: '/BIZ/upload', //这里我用struts2做后台处理  
        post_var_name: 'fileName', //这里是自己定义的，默认的名字叫file  
        width: 450,
        height: 300,
        minWidth: 450,
        minHeight: 300,
        draggable: true,
        resizable: true,
        //autoCreate: true,      
        constraintoviewport: true,
        permitted_extensions: ['JPG', 'jpg', 'jpeg', 'JPEG', 'GIF', 'gif', 'png', 'PNG'],
        modal: true,
        reset_on_hide: false,
        allow_close_on_upload: false, //关闭上传窗口是否仍然上传文件   
        //upload_autostart: true     //是否自动上传文件   
        upload_autostart: false
    });

    dialog.show();

    dialog.on('uploadsuccess', onUploadSuccess); //定义上传成功回调函数  
    dialog.on('uploadfailed', onUploadFailed); //定义上传失败回调函数  

    dialog.on('uploaderror', onUploadFailed); //定义上传出错回调函数  

    dialog.on('uploadcomplete', onUploadComplete); //定义上传完成回调函数
}


//加载汽配商信息表单
function load(title, key) {

    var CompanyNature = [['个体工商', '个体工商'], ['私营独资', '私营独资'], ['国营商家', '国营商家']];
    var CompanyType = [['其它', '其它'], ['合资', '合资'], ['独资', '独资'], ['国有', '国有'], ['私有', '私有'], ['全民所有', '全民所有'], ['集体所有', '集体所有'], ['私有制', '私有制'], ['有限责任制', '有限责任制']];

    var Nature = new top.Ext.form.ComboBox({
        store: new Ext.data.SimpleStore({
            fields: ['text', 'value'],
            data: CompanyNature
        }),
        displayField: 'text',
        fieldLabel: '汽配商分类',
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
        fieldLabel: '汽配商性质',
        anchor: '90%',
        editable: false,
        triggerAction: 'all',
        allowBlank: false,
        value: '其它'
    });


  

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
                           {
                               name: 'Name',
                               fieldLabel: '<span class="required">*</span>汽配商名称',
                               xtype: 'textfield',
                               id: 'Name',
                               maxLength: 50,
                               allowBlank: false,
                               emptyText: '填写汽配商名称',
                               maxLengthText: '用户名称长度不能超过50个字符',
                               anchor: '90%',
                               enableKeyEvents: true,
                               listeners: {
                                   'blur': function (val) {
                                       if (val.getValue()) {
                                           var respon = Ext.lib.Ajax.getConnectionObject().conn;
                                           respon.open("post", "/SysCompany/SearchNameExits?attr=2&key=" + key + "&name=" + encodeURIComponent(val.getValue().trim()), false);
                                           respon.send(null);
                                           var response = Ext.util.JSON.decode(respon.responseText);
                                           if (response.success) {
                                               top.Ext.Msg.show({
                                                   title: "信息提示",
                                                   msg: "汽配商名称已经重复,请重新输入！",
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
                             {
                                 name: 'Address',
                                 fieldLabel: '汽配商地址',
                                 xtype: 'textfield',
                                 id: 'Address',
                                 flex: 2,
                                 maxLength: 50,
                                 maxLengthText: '用户名称长度不能超过300个字符',
                                 anchor: '90%'
                             }

                        ]
                    },
                    {
                        columnWidth: .5,
                        layout: 'form',
                        items: [
                                  {
                                      name: 'Phone',
                                      fieldLabel: '汽配商电话',
                                      id: 'Phone',
                                      flex: 2,
                                      //allowBlank: false,
                                      xtype: 'textfield',
                                      regex: /^\d+$/,
                                      emptyText: '请输入有效的电话号码',
                                      anchor: '90%'
                                  },
                                    {
                                        name: 'LinkUser',
                                        fieldLabel: '联系人',
                                        xtype: 'textfield',
                                        //allowBlank: false,
                                        id: 'LinkUser',
                                        flex: 2,
                                        anchor: '90%'
                                    },
                            {
                                xtype: 'compositefield',
                                fieldLabel: '位置坐标',
                                combineErrors: false,
                                anchor: '100%',
                                items: [
							            {
							                xtype: 'textfield',
							                id: "Bx",
							                name: "ComPLon",
							                width: 70,
							                height: 22, //固定高度
							                emptyText: 'X',
							                readOnly: true
							            },
                                         {
                                             xtype: 'textfield',
                                             height: 22, //固定高度
                                             id: "By",
                                             width: 70,
                                             name: "CompLat",
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
                             fieldLabel: '汽配商介绍',
                             xtype: 'htmleditor',
                             id: 'Introduction',
                             name: 'Introduction',
                             height: 120,
                             emptyText: '可输入对汽配商的介绍信息', ////textfield自己的属性
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
                                 allowBlank: false,
                                 maxLength: 12,
                                 //regex: /^\d+$/,
                                 emptyText: '请输入登录帐号',
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
    win.width =770;
    win.height =470;
    return win;
}

///验证管理员账号是否重复
function VilivaName(key, value) {
//    if (!value.trim().match(/^(((13[0-9]{1})|(15[0-9]{1})|(18[0-9]{1}))+\d{8})$/)) {
//        top.Ext.Msg.show({
//            title: "信息提示",
//            msg: "手机号码格式不正确",
//            buttons: Ext.Msg.OK,
//            icon: Ext.MessageBox.INFO,
//            fn: function () {
//                top.Ext.getCmp("LoginName").setValue("");
//            }
//        });
//        return false;
//    }
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
            MessageInfo("该汽配商已经启用！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要启用该汽配商吗?', function (e) {
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
            MessageInfo("该汽配商已经禁用！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要禁用该汽配商吗?', function (e) {
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
    result = [];
    parent.idArray = [];
    url = '/BIZ/SaveData?1=1';
    var win = load("新增","");
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
        win.show();

        var respon = Ext.lib.Ajax.getConnectionObject().conn;
        respon.open("post", "/BIZ/GetData?id=" + key, false);
        respon.send(null);
        var response = Ext.util.JSON.decode(respon.responseText);

        form.form.loadRecord(response);
        //坐标赋值
        top.$("#Bx").val(response.data.ComPLon);
        top.$("#By").val(response.data.CompLat);

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
        win.show();
        url = '/BIZ/SaveData?Id=' + key + '&modify=1';
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
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要删除汽配商吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push("'" + rows[i].data["Id"] + "'");
                }
                Ext.Ajax.request({
                    url: '/BIZ/DeleteData',
                    params: { id: ids.join(",") },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            MessageInfo("删除成功！", "right");
                            Ext.getCmp("gg").store.reload();
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
var SaveDate = function () {
    var grid = Ext.getCmp("gg");
    var formPanel = top.Ext.getCmp("formPanel");
    var win = top.Ext.getCmp("window");
    if (formPanel.getForm().isValid()) {//如果验证通过
//        //获取
//        //获取区和商圈
//        var comProvince = top.Ext.getCmp("comProvince").getValue();
//        var comCity = top.Ext.getCmp("comCity").getValue();
//        var comArea = top.Ext.getCmp("comArea").getValue();

        //获取
        var para = { x: top.$("#Bx").val(), y: top.$("#By").val()};
        if (win.title == "编辑") {
            //得到选后的数据   
            var rows = grid.getSelectionModel().getSelections();
            para = { Id: rows[0].get("Id"), x: top.$("#Bx").val(), y: top.$("#By").val()};
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


//导入
function ImportDate() {
    //弹出窗体
    var win = new top.Ext.Window({
        id: "ImportWin",
        title: "数据导入",
        width: 450,
        height: 480,
        layout: 'fit',
        modal: true,
        shadow: false,
        stateful: false,
        border: false,
        closeAction: 'close',
        items: {
            autoScroll: true,
            autoLoad: { url: '../../Project/Html/CompanyImport.htm', scripts: true, nocache: true },
            params: { attr: 4 }
        },
        buttons: [{
            text: '开始导入',
            iconCls: 'GTP_submit',
            tooltip: '确定导入汽配商信息',
            handler: function () {
                var form = top.Ext.getCmp('ImportWinform');
                var center = top.Ext.getCmp('TabPanel');
                var rt = top.top.rmCategoryTree.getChecked(); //得到所有所选的子节点
                if (!form.getForm().isValid()) {
                    center.setActiveTab(0);
                    return;
                }
                //                else if (rt.length == 0) {//分类
                //                    //获取选中子节点
                //                    center.setActiveTab(0);
                //                    top.Ext.Msg.show({ title: "信息提示", msg: '请选择你要导入的分类', buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                //                    return;
                //                }
                else {
                    rt = top.top.rmCategoryTree;
                    var CheckTreeNode = rt.getChecked(); //获取选中状态

                    //获取选中子节点
                    var checkid = '';
                    for (var i = 0; i < CheckTreeNode.length; i++) {//除去最顶端的根节点(模块功能分配)
                        if (CheckTreeNode[i].getUI().checkbox) {
                            if (!CheckTreeNode[i].getUI().checkbox.indeterminate) {
                                checkid += CheckTreeNode[i].id + ',';
                            }
                        }
                    }

                    GetIndeterminateNode(rt.getRootNode());
                    //遍历树,查找半选状态下的Node
                    function GetIndeterminateNode(node) {
                        var childCount = node.childNodes.length;
                        if (childCount > 0) {
                            for (var i = 0; i < childCount; i++) {
                                var child = node.childNodes[i];
                                var checkBox = child.getUI().checkbox;
                                if (checkBox.indeterminate) { //如果是半选状态
                                    checkid += child.id + ',';
                                }
                                GetIndeterminateNode(child);
                            }
                        }
                    }
                    checkid = checkid.substring(0, checkid.length - 1);

                    form.getForm().submit({
                        waitTitle: '系统提示', //标题
                        waitMsg: '正在导入数据,请稍后...', //提示信息
                        submitEmptyText: false,
                        method: "POST",
                        url: "/BIZ/ImportDate",
                        params: { checkid: checkid },
                        success: function (form, action) {
                            //成功后
                            var flag = action.result;
                            if (flag.success) {
                                if (flag.data.length > 10) {
                                    var a = '<a href="' + flag.data + '">下载错误数据</a><br/>(' + flag.msg + ')';
                                    top.Ext.getCmp('downerror').show();
                                    top.Ext.getCmp("downerror").update(a);
                                    top.Ext.getCmp("excel").setValue("");
                                } else {
                                    top.Ext.getCmp("ImportWin").close();
                                    MessageInfo(flag.msg, "right");
                                }
                                Ext.getCmp("gg").store.reload();
                            }
                        },
                        failure: function (form, action) {
                        }
                    });
                }
            }
        }, {
            text: '取消',
            iconCls: 'GTP_cancel',
            tooltip: '取消',
            handler: function () {
                top.Ext.getCmp("ImportWin").close(); //直接销毁
            }
        }]
    });
    win.show();
}

//grid双击
function dbGridClick(grid, rowindex, e) {
    EditDate();
}

































