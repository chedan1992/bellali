//一件交接
function ChangeUser() {
    var CreateCompanyId = '0';
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].get("Id");
        CreateCompanyId = rows[0].get("Cid");
    }
    url = '/UserStaff/ChangeUser?UserId=' + key;
    var win = CreateUserWindow("工作一键交接", CreateCompanyId, key);
    win.show();

}
//创建表单
function CreateUserWindow(title,CreateCompanyId,UserId) {
    /*
    责任人
    */
    var comBrand = new Ext.data.Store({
        proxy: new Ext.data.HttpProxy({
            url: '/UserStaff/GetSysMaster?CompanyId=' + CreateCompanyId + "&UserId=" + UserId,
            method: 'POST'
        }),
        reader: new Ext.data.JsonReader({},
            Ext.data.Record.create(["Id", "UserName"]
        ))
    });

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
                    xtype: 'combo',
                    triggerAction: 'all',
                    fieldLabel: '<span class="required">*</span>交接人',
                    emptyText: '选择交接人',
                    forceSelection: true,
                    editable: true,
                    id: 'comResponsibleId',
                    name: 'comResponsibleId',
                    typeAhead: true, //模糊查询
                    allowBlank: false,
                    displayField: 'UserName',
                    valueField: 'Id',
                    hiddenName: 'UserName',
                    anchor: '90%',
                    store: comBrand,
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
        ]
    });

    //窗体
    var win = Window("window", title, form);
    win.width = 360;
    win.height = 150;
    return win;
}


function LookSign() {
    alert('功能暂未开通');
}

//点击组织架构
function treeitemclick(node, e) {
    treeNodeId = node.attributes.id;

    reload();
}

//渲染用户头像
function renderUserName(value, meta, record, rowIdx, colIdx, store) {
    if (record) {
        var sex = record.data.Sex == 0 ? "GTP_management.png" : "GTP_women.png";
        return '<span style="vertical-align: middle;"><img style="vertical-align: middle; width:15px; height:15px;border:false" src="../../Resource/css/icons/tree/' + sex + '"/>&nbsp;' + value + '</span>';
    }
    else {
        return value;
    }
}

//格式化 启用 or 禁用
function formartEnableOrDisable(val, row) {
    if (val == 1)
        return "<span style='color:green'>启用</span>";
    else if (val == 0) {
        return "<span style='color:Red'>禁用</span>";
    }
    else if (val == -1) {
        return "<span style='color:silver'>删除</span>";
    }
    else if (val == -2) {
        return "<span style='color:999999'>待审核</span>";
    } else {
        return "<span style='color:#999999'>已过期</span>";
    }
}

function init() {
    //初始化上传控件
    var ImageBtn = top.$('#showImg');
    var ImageValue = top.$("#headImg");
    upImage(ImageBtn, ImageValue, 'HeadImg', null);
}

//上传图片要求格式
var allowfile = 'bmp,jpg,jpeg,gif,png';

//单图-图片上传
function upImage(Obj, hobj, savefilepath, callbakfun) {
    if (Obj.length > 0) {
        var intervalTime = null;
        var urlstr = "/Common/UpImage";
        if (savefilepath != "") {
            urlstr = "/Common/UpImage";
        }
        var upload = new top.AjaxUpload(Obj, {
            action: urlstr,
            data: { filepath: savefilepath }, //保存文件路径
            name: 'userFile',
            onChange: function (file, extension) {
            },
            onSubmit: function (file, ext) {
                if (!(ext && allowfile.indexOf(ext.toLowerCase()) > -1)) {
                    top.Ext.Msg.show({ title: "信息提示", msg: "错误：无效的文件扩展名！请上传" + allowfile + "格式文件！", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                    return false;
                }
            },
            onComplete: function (file, response) {
                var json = $.parseJSON(response);
                if (json.success == true) {
                    var imgT = savefilepath == "" ? "/Uploadfile/Default/" : "";
                    readImg(imgT);
                    Obj.attr("src", imgT + json.msg);
                    //hobj.val(imgT + json.msg);
                    //预览
                    top.Ext.getCmp("headImg").getEl().dom.src = json.msg;
                    top.Ext.getCmp('HPact').setValue(json.msg);
                    if (callbakfun != null) {
                        callbakfun(json);
                    }
                }
                else {
                    top.Ext.Msg.show({ title: "信息提示", msg: json.Msg, buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                }
            }
        });
    }
}

//预加载某个图片
function readImg(imgSrc) {
    var img = new Image();
    img.src = imgSrc;
    img.title = "请选择文件";
}

//新增
function AddDate() {
    var grid = Ext.getCmp("tr");
    var rows = grid.getSelectionModel().getSelectedNode();
    var CreateCompanyId = '0';
    if (rows) {
        if (rows.attributes.Attribute ==2) {
            CreateCompanyId = rows.attributes.id;
        }
        else {
            MessageInfo("请先选择部门！", "statusing");
            return false;
        }
    }
    else {
        MessageInfo("请先选择部门！", "statusing");
        return false;
    }
    var window = CreateFromWindow("新增", "");
    var form = top.Ext.getCmp('formPanel');
    form.form.reset();
    top.Ext.getCmp("Pwd").setVisible(true); //显示密码框
    url = '/UserStaff/SaveData?OrganizaId=' + CreateCompanyId;

    window.show();
    init();
}
//编辑
function EditDate() {
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].get("Id");
       
        var window = CreateFromWindow("编辑", key);
        var form = top.Ext.getCmp('formPanel');
        form.form.loadRecord(rows[0]); //再加载数据   
        url = '/UserStaff/SaveData?Id=' + key + '&modify=1';
        top.Ext.getCmp("Pwd").setVisible(false); //隐藏密码框
        top.Ext.getCmp("Pwd").setValue("");
        window.show();

        //获取头像
        var HeadImg = rows[0].get("HeadImg");
        if (HeadImg != "") {
            top.Ext.getCmp("headImg").getEl().dom.src = HeadImg;
            top.Ext.getCmp('HPact').setValue(HeadImg);
        }
        else {
            var Sex = rows[0].get("Sex");
            if (Sex == 1) {
                top.Ext.getCmp("headImg").getEl().dom.src = '../../Resource/css/icons/head/GTP_hfemale_big.jpg';
            }
        }
        init();
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
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要删除该用户吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                Ext.Ajax.request({
                    url: '/SysMaster/DeleteData',
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
        var comResponsibleId = top.Ext.getCmp("comResponsibleId");
        if(comResponsibleId)
        {
            comResponsibleId = comResponsibleId.getValue();
        }
        var para = { comResponsibleId: comResponsibleId };

        formPanel.getForm().submit({
            waitTitle: '系统提示', //标题
            waitMsg: '正在提交数据,请稍后...', //提示信息
            submitEmptyText: false, //If set to true, the emptyText value will be sent with the form when it is submitted. Defaults to true                                   
            url: url, //记录表单提交的路径
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

//创建表单弹框
function CreateFromWindow(title, key) {

    var center = new top.Ext.TabPanel({
        deferredRender: false,
        animScroll: true,              //使用动画滚动效果   
        enableTabScroll: true,         //tab标签过宽时自动显示滚动条
        id: 'TabPanel',
        activeTab: 0,
        border: false,
        listeners: {
            tabchange: function (thisTab, Activetab) {
            }
        }
    });

    var form = new top.Ext.FormPanel({
        labelWidth: 75,
        frame: true,
        border: false,
        layout: 'fit',
        labelAlign: 'right',
        id: 'formPanel',
        layout: 'column',
        items: [
            {
                xtype: 'panel',
                defaultType: 'textfield',
                autoHeight: true,
                anchor: '100%',
                border: false,
                columnWidth: .7,
                style: 'margin-top:10px;',
                layout: 'form',
                items: [
                      {
                          name: 'UserName',
                          id: 'UserName',
                          fieldLabel: '<span class="required">*</span>用户名称',
                          emptyText: '填写用户名称', ////textfield自己的属性
                          allowBlank: false,
                          maxLength: 20,
                          anchor: '90%',
                          maxLengthText: '用户名称长度不能超过20个字符',
                          enableKeyEvents: true
//                          listeners: {
//                              'blur': function (val) {
//                                  if (val.getValue().trim() != "") {
//                                      var code = codeConvert(top.Ext.getCmp("UserName").getValue());
//                                      return VilivaName(key, code);
//                                  }
//                              }
//                          }

                      },
                      {
                          name: 'LoginName',
                          id: 'LoginName',
                          fieldLabel: '<span class="required">*</span>登录账号',
                          emptyText: '请输入手机号', ////textfield自己的属性
                          allowBlank: false,
                          maxLength: 12,
                          regex: /^\d+$/,
                          emptyText: '请输入有效的电话号码',
                          anchor: '90%',
                          enableKeyEvents: true,
                          listeners: {
                              'blur': function (val) {
                                  if (val.getValue()) {
                                      return VilivaName(key, val.getValue());
                                  }
                              }
                              //                              'keyup': function (val) {
                              //                                  if (val.getValue().trim() != "") {
                              //                                      top.Ext.getCmp("Phone").setValue(val.getValue());
                              //                                  }
                              //                              }
                          }
                      },
                      {
                          name: 'Pwd',
                          id: 'Pwd',
                          fieldLabel: '登录密码',
                          emptyText: '默认登录密码:666666', ////textfield自己的属性
                          maxLength: 20,
                          anchor: '90%',
                          maxLengthText: '登录密码长度不能超过12个字符'
                      },
                      {
                          name: 'Sex',
                          xtype: 'radiogroup',
                          fieldLabel: '性别',
                          id: 'Sex',
                          width: 100,
                          items: [
                                { boxLabel: '男', name: 'Sex', inputValue: 0, checked: true, width: 100, height: 25 },
                                { boxLabel: '女', name: 'Sex', inputValue: 1, width: 100, height: 25 }
                           ],
                          listeners: {
                              'change': function (val) {
                                  var result = top.Ext.getCmp("Sex").getValue();
                                  var hasHPact = top.Ext.getCmp('HPact').getValue();
                                  if (hasHPact == "" || hasHPact == null) {
                                      if (result.inputValue == 1) {//女
                                          top.Ext.getCmp("headImg").getEl().dom.src = '../../Resource/css/icons/head/GTP_hfemale_big.jpg';
                                      }
                                      else {
                                          top.Ext.getCmp("headImg").getEl().dom.src = '../../Resource/css/icons/head/GTP_hmale_big.jpg';
                                      }
                                  }
                              }
                          }
                      },
//                      {
//                          name: 'OperateNum',
//                          id: 'OperateNum',
//                          fieldLabel: '操作证号',
//                          anchor: '90%'
//                      },
                      {
                          name: 'Email',
                          id: 'Email',
                          fieldLabel: '<span class="required">*</span>邮箱',
                          allowBlank: false,
                          anchor: '90%',
                          xtype: 'textfield',
                          vtype: 'email'
                      },
                      {
                          name: 'CardNum',
                          id: 'CardNum',
                          fieldLabel: '身份证号码',
                          anchor: '90%',
                          enableKeyEvents: true,
                          listeners: {
                              'blur': function (val) {
                                  if (val.getValue()) {
                                      return validId(val);
                                  }
                              }
                          }
                      }
                    ]
            },
            {
                xtype: 'panel',
                columnWidth: .3,
                layout: 'form',
                border: false,
                isFormField: true,
                items: [
                     {
                         xtype: 'fieldset',
                         layout: 'form',
                         style: 'margin-top:10px;padding:10px',
                         defaultType: 'textfield',
                         height: 160, //图片高度
                         width: 120, //图片宽度
                         items: [
                                    {
                                        xtype: 'box', //或者xtype: 'component',
                                        width: 100, //图片宽度
                                        align: 'center',
                                        id: 'headImg',
                                        height: 110, //图片高度
                                        autoEl: {
                                            tag: 'img',    //指定为img标签
                                            src: '../../Resource/css/icons/head/GTP_hmale_big.jpg'    //指定url路径
                                        }
                                    },
                                     {
                                         xtype: 'tbtext',
                                         id: 'showImg',
                                         style: 'margin-left:30px;color:blue;cursor:pointer;margin-top:6px',
                                         text: '头像上传'
                                     },
                                    {
                                        name: 'HPact',
                                        id: 'HPact',
                                        xtype: 'textfield',
                                        hidden: true
                                    }
                            ]

                     }
                ]
            }
//            {
//                columnWidth: 1,
//                layout: 'form',
//                border: false,
//                autoHeight: true,
//                items: [
//                         
//                          {
//                              name: 'Phone',
//                              fieldLabel: '支付宝账号',
//                              xtype: 'textfield',
//                              anchor: '100%',
//                              id: 'Phone',
//                              emptyText: '请输入支付宝账号'
//                          }
//                    ]
//            }
        ]
    });

//    var tabs1 = center.add({
//        tabTip: '基本信息',
//        layout: 'fit',
//        title: '基本信息',
//        border: false,
//        items: [form]
//    });
//    center.setActiveTab(tabs1);
    //窗体
     var win = Window("window", title, form);
    win.width = 570;
    win.height =350;
    return win;
}

//设置上传用户头像文件状态
function HPactFileUploadAction(o) {
    //验证同文件的正则  
    var img_reg = /\.([jJ][pP][gG]){1}$|\.([jJ][pP][eE][gG]){1}$|\.([gG][iI][fF]){1}$|\.([pP][nN][gG]){1}$|\.([bB][mM][pP]){1}$/;
    if (!img_reg.test(o.value)) {
        top.Ext.Msg.show({ title: "信息提示", msg: "文件类型错误,请选择文件(jpg/jpeg/gif/png/bmp)", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
        o.setRawValue('');
        return;
    }
}

///验证用户名是否重复
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
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        if (rows[0].data["Status"] == 1) {
            MessageInfo("该用户已经启用！", "statusing");
            return;
        }
        if (rows[0].data["Status"] == -2) {
            MessageInfo("该用户还未审核通过,不能操作！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要启用该用户吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                Ext.Ajax.request({
                    url: '/SysMaster/EnableUse',
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
            MessageInfo("该用户已经禁用！", "statusing");
            return;
        }
        if (rows[0].data["Status"] == -2) {
            MessageInfo("该用户还未审核通过,不能操作！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '禁用后,该用户下的所有信息将被禁用.<br/><br/>确认要禁用该用户吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                Ext.Ajax.request({
                    url: '/SysMaster/DisableUse',
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

//密码重置
function ReSetPassword() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        if (rows[0].data["Status"] == -2) {
            MessageInfo("该用户还未审核通过,不能操作！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要重置该用户的登录密码吗? 重置后密码为:666666', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                Ext.Ajax.request({
                    url: '/Home/ReSetPwd',
                    params: { id: ids.join(",") },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            Ext.getCmp("gg").store.reload();
                            MessageInfo("重置成功！", "right");
                        } else {
                            MessageInfo("重置失败！", "error");
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

//批量导入
function ImportDate() {
    if (parent) {
        var grid = Ext.getCmp("tr");
        var rows = grid.getSelectionModel().getSelectedNode();
        var Cid = '0';
        if (rows) {
            if (rows.attributes.Attribute == 5) {
                Cid = rows.attributes.id;
            }
            else {
                MessageInfo("请先选择部门！", "statusing");
                return false;
            }
        }
        else {
            MessageInfo("请先选择部门！", "statusing");
            return false;
        }

        var tabId = parent.GetActiveTabId(); //公共变量
        parent.AddTabPanel('批量导入', 'TestExport', tabId, '/SysMaster/Export?Cid=' + Cid);
    }
}

//审核
function GTPAudit() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        if (rows[0].data["Status"] != -2) {
            MessageInfo("该用户无需审核！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要审核通过该用户吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                Ext.Ajax.request({
                    url: '/UserStaff/GTPAudit',
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

//金额详细
function MoneyDetails() {
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].data["Id"];
        LookDetails(key);
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}

//选择员工
function LookDetails(key) {
    var shop = new top.Ext.Window({
        width: 680,
        id: 'shoper',
        height: 394,
        closable: true,
        shadow: false,
        stateful: false,
        border: false,
        modal: true,
        layout: 'fit',
        plain: true,
        autoDestroy: true,
        closeAction: 'close',
        title: '个人流水信息',
        items: {
            autoScroll: true,
            border: false,
            params: { Eid: key },
            autoLoad: { url: '../../Project/Html/TotalMaster.htm', scripts: true, nocache: true }
        },
        buttons: [
                    {
                        text: '确定',
                        tooltip: '关闭窗体',
                        handler: function () {
                            top.Ext.getCmp("shoper").close();
                        }
                    }]
    }).show();
            }

//审核通过
function GTPsubmit() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        if (rows[0].data["Status"] != -2) {
            MessageInfo("该用户无需审核！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要审核通过该用户吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                Ext.Ajax.request({
                    url: '/UserStaff/GTPAudit',
                    params: { id: ids.join(",") },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            Ext.getCmp("gg").store.reload();
                            MessageInfo("审核成功！", "right");
                        } else {
                            MessageInfo("审核失败！", "error");
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

//取消审核
function GTPcancel() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        if (rows[0].data["Status"] != -2) {
            MessageInfo("该用户无需审核！", "statusing");
            return;
        }
        var key = rows[0].data["Id"];
        //弹出框
        var window = CreateFromAuditWindow('审核不通过',key);
        window.show();
//        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要取消该用户申请吗?', function (e) {
//            if (e == "yes") {
//                var ids = [];
//                for (var i = 0; i < rows.length; i++) {
//                    ids.push(rows[i].data["Id"]);
//                }
//                Ext.Ajax.request({
//                    url: '/UserStaff/GTPAuditcancel',
//                    params: { id: ids.join(",") },
//                    success: function (response) {
//                        var rs = eval('(' + response.responseText + ')');
//                        if (rs.success) {
//                            Ext.getCmp("gg").store.reload();
//                            MessageInfo("取消成功！", "right");
//                        } else {
//                            MessageInfo("取消失败！", "error");
//                        }
//                    }
//                });
//            }
//        });
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}

//创建表单弹框
function CreateFromAuditWindow(title, key) {
    var form = new top.Ext.FormPanel({
        labelWidth: 65,
        frame: true,
        border: false,
        labelAlign: 'right',
        bodyStyle: 'padding:5px 5px 0',
        id: 'formPanel',
        defaultType: 'textfield',
        items: [
                {
                    fieldLabel: '<span class="required">*</span>描述信息',
                xtype: 'textarea',
                id: 'Introduction',
                name: 'Introduction',
                height: 150,
                allowBlank: false,
                anchor: '95%',
                emptyText: '可输入审核失败缘由(70字符内)', ////textfield自己的属性
                maxLength:70,
                maxLengthText: '描述长度不能超过70个字符'
            }
        ]
        });

        //自定义button
    var button1= new top.Ext.Button({
        text: '提交',
        iconCls: 'GTP_submit',
        tooltip: '提交',
        handler: function () {
            var formPanel = top.Ext.getCmp("formPanel");
            if (formPanel.getForm().isValid()) {//如果验证通过
                formPanel.getForm().submit({
                    waitTitle: '系统提示', //标题
                    waitMsg: '正在提交数据,请稍后...', //提示信息
                    submitEmptyText: false, //If set to true, the emptyText value will be sent with the form when it is submitted. Defaults to true                                   
                    url:'/UserStaff/GTPAuditcancel?id=' + key,
                    method: "POST",
                    success: function (form, action) {
                        //成功后
                        var flag = action.result;
                        if (flag.success) {
                            Ext.getCmp("gg").store.reload();
                            MessageInfo("取消成功！", "right");
                        } else {
                            MessageInfo("取消失败！", "error");
                        }
                        top.Ext.getCmp('window').close();
                    },
                    failure: function (form, action) {
                        MessageInfo("取消失败！", "error");
                    }
                });
            }   
        }
    })
    //窗体
    var win = WindowDiy("window", title, form, button1);
    win.width =400;
    win.height =250;
    return win;
}

//查看用户管理设备
function LookAppointed() {
 var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var ResponsibleId = rows[0].data["Id"];
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
            title: '我管理的设备列表',
            items: {
                autoScroll: true,
                border: false,
                params: { CID: "", TypeShow: 5, ResponsibleId: ResponsibleId },
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