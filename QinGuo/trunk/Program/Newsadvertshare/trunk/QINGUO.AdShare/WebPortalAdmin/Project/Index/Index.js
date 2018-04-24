//弹框详细设备信息
function OpenInfo(month, Attribute) {
    var shop = new top.Ext.Window({
        width: 680,
        id: 'shoper',
        height: 548,
        closable: false,
        shadow: false,
        stateful: false,
        border: false,
        modal: true,
        layout: 'fit',
        plain: true,
        autoDestroy: true,
        closeAction: 'close',
        title: month + '月份巡检记录',
        items: {
            autoScroll: true,
            border: false,
            params: { month: month, Attribute: Attribute },
            autoLoad: { url: '../../Project/Html/ChartRepairRecordGrid.htm', scripts: true, nocache: true }
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
}

//验证文件上传
function ExcelUploadAction(o) {
    //验证同文件的正则  
    var img_reg = /\.([xX][lL][sS]){1}$|\.([xX][lL][sS][xX]){1}$/;
    if (!img_reg.test(o.value)) {
        top.Ext.Msg.show({ title: "信息提示", msg: "文件类型错误,请选择文件(xls,xlsx)", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
        o.setRawValue('');
        return;
    }
}
//获取在线用户
function GetServices() {
    var respon = Ext.lib.Ajax.getConnectionObject().conn;
    respon.open("post", "/SysMaster/online", false); //获取页面按钮
    respon.send(null);
    var result = Ext.util.JSON.decode(respon.responseText);
    if (result.success) {
        Ext.getCmp("online").setText("【在线:<span style='color:red'>" + result.msg + "</span>人】");
    } else {
        //固定索引
        Ext.getCmp("online").setText("【在线:<span style='color:red'>1</span>人】");
    }
}

//退出
function logout() {
    var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要退出系统吗?', function (e) {
        if (e == "yes") {
            GoOut();
        }
    });

}

//north底部工具条
function northBar() {
    //菜单
    var menu = new Ext.menu.Menu({
        id: 'mainMenu',
        items: [
                     {
                         text: '皮肤切换',
                         tooltip: '皮肤切换',
                         iconCls: 'GTP_color',
                         id: 'GTPcolor',
                         menu: {
                             items: [
                                    {
                                        text: '自定义蓝',
                                        checked: false,
                                        group: 'theme',
                                        id: 'gtpblue',
                                        value: 'gtp-blue.css',
                                        checkHandler: themeCheck
                                    },
                                    {
                                        text: '浅色蓝',
                                        checked: false,
                                        group: 'theme',
                                        id: 'extall',
                                        value: 'ext-all.css',
                                        checkHandler: themeCheck
                                    },
                                    {
                                        text: '默认灰',
                                        checked: false,
                                        group: 'theme',
                                        id: 'xthemegray',
                                        value: 'xtheme-gray.css',
                                        checkHandler: themeCheck
                                    }




                            ]
                         }
                     }
                     , '-', {
                         text: '密码修改',
                         tooltip: '密码修改',
                         iconCls: 'GTP_setpassword',
                         handler: UpPassword
                     }
        //                      '-', {
        //                          text: '保存首页',
        //                          tooltip: '保存修改主页',
        //                          iconCls: 'GTP_dept_root',
        //                          handler: SavePortal
        //                      }
        ]
    });

    var bbar = new Ext.Toolbar({
        id: 'northBar'
    });


    bbar.add({
        iconCls: 'iconuser',
        id: 'iconUser',
        text: '您好:'
    });
    bbar.add('-');
    //    bbar.add(
    //                        {
    //                            xtype: 'tbtext',
    //                            id: 'online',
    //                            text: '【在线:loading..】'
    //                        }
    //                        );
    //    bbar.add('-');
    bbar.add(
                {
                    xtype: 'tbtext',
                    id: 'noticeTxt',
                    html: '最新通知：<marquee scrollamount=2 border=1 width=400 height=100% scrolldelay=3><font color=#625052 id=noticeValue><font></marquee>',
                    style: 'margin-top:-1px'
                });
    bbar.add(
                {
                    xtype: 'tbtext',
                    id: 'nnowdate',
                    text: ''
                });

    bbar.add(
                {
                    xtype: 'tbtext',
                    id: 'timedate',
                    text: ''
                });

    bbar.add('->');
    bbar.add({
        text: '设置',
        iconCls: 'icon-user-set',
        menu: menu
    });

    bbar.add('-');
    bbar.add({
        text: '帮助',
        id: 'userhelp',
        iconCls: 'GTP_userhelp',
        handler: function () {
           
//            var respon = Ext.lib.Ajax.getConnectionObject().conn;
//            respon.open("post", "/Home/Help"); //获取页面按钮
//            respon.send(null);
        }
    });

    bbar.add('-');
    bbar.add({
        text: '注销',
        id: 'logout',
        iconCls: 'icon-close',
        handler: logout
    });

    //设置皮肤默认选中状态
    if (theme) {
        if (Ext.getCmp(theme.replace('-', '').substring(0, theme.indexOf('.') - 1))) {
            Ext.getCmp(theme.replace('-', '').substring(0, theme.indexOf('.') - 1)).checked = true;
        }
    }
    else {
        Ext.getCmp("extall").checked = true;
    }

    return bbar;
}
function Help() {
    top.Ext.Msg.show({ title: "信息提示", msg: "文档正在编写中,请耐心等待.", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
}
//皮肤切换
function themeCheck(item, checked) {
    if (checked) {
        Ext.MessageBox.show({
            title: '请等待',
            msg: '系统正在替换主题...',
            progressText: 'Initializing...',
            width: 300,
            progress: true,
            closable: false
        });
        var f = function (v) {
            return function () {
                if (v == 12) {
                    Ext.MessageBox.hide();
                    //设置cookies
                    var date = new Date();
                    date.setTime(date.getTime() + 30 * 24 * 3066 * 1000); //30天后的日期             
                    document.cookie = "css=" + item.value + ";expires=" + date.toGMTString();
                    ChangeTheme();
                } else {
                    var i = v / 11;
                    Ext.MessageBox.updateProgress(i, Math.round(100 * i) + '% 已完成');
                }
            };
        };
        for (var i = 1; i < 13; i++) {
            setTimeout(f(i), i * 300);
        }
    }
}

function UpPwd() {

    var formPanel = new Ext.FormPanel({
        labelWidth: 65,
        frame: true,
        border: false,
        labelAlign: 'right',
        bodyStyle: 'padding:5px 5px 0;',
        id: '_formPanel',
        defaults: {
            xtype: 'textfield',
            width: 230,
            inputType: 'password',
            allowBlank: false
        },
        items: [
                {
                    name: 'oldpwd',
                    fieldLabel: '<span class="required">*</span>原密码',
                    id: 'oldpwd',
                    allowBlank: false,
                    blankText: '原密码不能为空',
                    inputType: 'textfield',
                    regex: /^[\s\S]{0,20}$/,
                    regexText: '密码长度不能超过20个字符',
                    maxLength: 20,
                    minLength: 6
                },
                {
                    xtype: 'tbtext',
                    text: '<br/>'
                },
                {
                    name: 'newpwd',
                    fieldLabel: '<span class="required">*</span>新密码',
                    id: 'newpwd',
                    allowBlank: false,
                    blankText: '新密码不能为空',
                    regex: /^[\s\S]{0,20}$/,
                    regexText: '密码长度不能超过20个字符',
                    maxLength: 20,
                    minLength: 6
                },
               {
                   xtype: 'tbtext',
                   text: '<br/>'
               },
                {
                    name: 'newpwd2',
                    id: 'newpwd2',
                    fieldLabel: '<span class="required">*</span>确认密码',
                    initialPassField: 'newpwd',
                    allowBlank: false,
                    blankText: '确认密码不能为空',
                    regex: /^[\s\S]{0,20}$/,
                    regexText: '密码长度不能超过20个字符',
                    vtype: "password",
                    vtypeText: "密码不一致",
                    confirmTo: "newpwd",
                    maxLength: 20,
                    minLength: 6
                }
                ]
    });


    var win = new Ext.Window({
        title: '密码修改',
        id: '_pwdwin',
        items: formPanel,
        width: 360,
        height: 200,
        modal: true, //设置遮罩
        layout: 'fit',
        shadow: false,
        stateful: false,
        closeAction: 'close',
        border: false,
        buttons: [{
            text: '保存',
            iconCls: 'GTP_submit',
            tooltip: '保存',
            id: 'GTP_submit',
            handler: function () {
                if (formPanel.getForm().isValid()) {
                    //密码修改
                    var oldpwd = Ext.getCmp("oldpwd").getValue().trim();
                    var newpwd = Ext.getCmp("newpwd").getValue().trim();
                    var newpwd2 = Ext.getCmp("newpwd2").getValue().trim();
                    Ext.Ajax.request({
                        url: '/Home/UpdatePwd',
                        params: { oldpwd: oldpwd, newpwd: newpwd, newpwd2: newpwd },
                        success: function (response) {
                            var rs = eval('(' + response.responseText + ')');
                            if (rs.success) {
                                $('#GTP_submit').tip({ width: '240', status: 'right', content: '修改成功！', dis_time: 1000 });
                            } else {
                                top.Ext.Msg.show({ title: "信息提示", msg: rs.msg, buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                            }
                            win.close();
                        }
                    });
                }
            }
        }, {
            text: '取消',
            iconCls: 'GTP_cancel',
            tooltip: '取消',
            handler: function () {
                win.close();
            }
        }]
    });

    win.addListener('beforeshow', function (o) {
        win.center(); //始终居中显示
    });

    win.show();
}


//密码修改
function UpPassword() {
    UpPwd();
    var form = Ext.getCmp('_formPanel');
    form.form.reset();
}

//tab添加菜单事件
function treeitemclick(node, e) {
    if (node.isLeaf()) {
        var centerPanel = Ext.getCmp("ShareCenter");
        var tabs = centerPanel.getItem(node.id);
        if (!tabs) {
            var index = centerPanel.items.length;
            var tabs = centerPanel.add({
                tabTip: '双击关闭',
                'id': node.id,
                layout: 'fit',
                name: '', //记录它的父页面id,刷新使用
                'title': node.text,
                //iconCls: node.attributes['iconCls'],
                closable: true,
                html: '<iframe id="frame_content" width="100%" height="100%" name="' + node.attributes['className'] + '" frameborder="0" src=' + node.attributes['pageUrl'] + '></iframe>',
                destroy: function () {//销毁tabpanel 销毁tab在浏览器中的内存
                    if (this.fireEvent("destroy", this) != false) {
                        this.el.remove();
                        if (Ext.isIE) {
                            CollectGarbage(); //CollectGarbage()函数强制收回内存
                        }
                    }
                }
            });
            centerPanel.setActiveTab(tabs);
        } else {
            centerPanel.setActiveTab(tabs);
        }
    }
}

//保存修改过的我的主页
function SavePortal() {
    var portal = Ext.getCmp("myportal");
    var result = [];
    var items = portal.items;
    for (var i = 0; i < items.getCount(); i++) {
        var c = items.get(i);
        c.items.each(function (portlet, index) {
            var o = {
                id: portlet.getId(),
                col: i,
                row: index
            };
            result.push(o); ;
        });
    }
    setCookie(Ext.encode(result), "layoutInfo");
    MessageInfo("保存成功！", "right");
    //alert("以下json串保存到数据库即可记住各自位置:" + Ext.encode(result));
}

//主页保存cookie
function setCookie(info, cookieName) {
    var expiration = new Date((new Date()).getTime() + 15 * 60000);
    document.cookie = cookieName + "=" + info + "; expires =" + expiration.toGMTString();
}

//首页时间显示
function y2k(number) {
    return (number < 1000) ? number + 1900 : number;
}

function dispdate() {
    var now = new Date();
    var dd = now.getDate(), mt = now.getMonth() + 1, yy = y2k(now.getYear()), weekVal = now.getDay();
    if (weekVal == 0)
        msg1 = "星期日";
    else if (weekVal == 1)
        msg1 = "星期一";
    else if (weekVal == 2)
        msg1 = "星期二";
    else if (weekVal == 3)
        msg1 = "星期三";
    else if (weekVal == 4)
        msg1 = "星期四";
    else if (weekVal == 5)
        msg1 = "星期五";
    else if (weekVal == 6)
        msg1 = "星期六";
    Ext.getDom("nnowdate").innerHTML = ("今天是 " + yy + "年" + mt + "月" + dd + "日 " + msg1);
}

//显示小时分钟
function updateTime() {
    var now = new Date();
    var theHour = now.getHours();
    var theMin = now.getMinutes();
    var theSec = now.getSeconds();
    if (theHour < 10) {
        theHour = "0" + theHour
    }
    if (theMin < 10) {
        theMin = "0" + theMin
    }
    if (theSec < 10) {
        theSec = "0" + theSec
    }
    var theTime = theHour + ":" + theMin + ":" + theSec;
    Ext.getDom("timedate").innerHTML = (theTime);
    timerID = setTimeout("updateTime()", 1000)
}

//角色管理---删除范围权限
function deletePic(key) {
    if (Ext.getCmp('key').getValue().trim() == key) {//grid删除的是编辑数据
        Ext.getCmp('key').setValue('');
    }
    var respon = Ext.lib.Ajax.getConnectionObject().conn;
    respon.open("post", "/SysRoleRangeData/DeleteData?id=" + key, false); //获取页面按钮
    respon.send(null);
    var result = Ext.util.JSON.decode(respon.responseText);
    if (result.success) {
        Ext.getCmp("RoleGG").store.reload();
        Ext.getCmp('tab1').doLayout();
    }
}

//资讯预览
function LookDynamic(val) {
    var id = val.id;
    var title = val.title;
    if (title.length > 8) {
        title = title.substring(0, 8) + "....";
    }
    var html = '/EDynamic/DocumentView?Id=' + id
    AddTabPanel(title, id, "", html);
}
//异常弹框详细设备信息
function showYiChang() {
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
        title: '异常设备列表',
        items: {
            autoScroll: true,
            border: false,
            params: { CID: "", TypeShow: 2, ResponsibleId: "" },
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

//过期弹框详细设备信息
function showGuoQi() {
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
        title: '过期设备列表',
        items: {
            autoScroll: true,
            border: false,
            params: { CID: "", TypeShow: 1, ResponsibleId: "" },
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

//查看设备维护情况
function clickDetail() {
    var grid = top.Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].data["Id"];
        var Title = rows[0].data["Name"];
        var shop = new top.Ext.Window({
            width: 680,
            id: 'shoper',
            height: 548,
            closable: false,
            shadow: false,
            stateful: false,
            border: false,
            modal: true,
            layout: 'fit',
            plain: true,
            autoDestroy: true,
            closeAction: 'close',
            title: Title + '巡检记录',
            items: {
                autoScroll: true,
                border: false,
                params: { CEId: key },
                autoLoad: { url: '../../Project/Html/RepairRecordGrid.htm', scripts: true, nocache: true }
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
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}