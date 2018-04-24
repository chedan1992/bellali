//广告展示方式变化
function ChangeType() {
    var val = Ext.getCmp("ShowType").getValue().inputValue;
    switch (val) {
        case 1: //无时间限制
            Ext.getCmp("timeLimit").hide(); //有效时间
            Ext.getCmp("StartDate").allowBlank = true;
            Ext.getCmp("StartTime").allowBlank = true;
            Ext.getCmp("EndDate").allowBlank = true;
            Ext.getCmp("EndTime").allowBlank = true;

            break;
        case 2: //自动下架
            Ext.getCmp("timeLimit").show(); //有效时间

            Ext.getCmp("StartDate").allowBlank = false;
            Ext.getCmp("StartTime").allowBlank = false;
            Ext.getCmp("EndDate").allowBlank = false;
            Ext.getCmp("EndTime").allowBlank = false;

            break;
    }
    Ext.getCmp("viewMain").doLayout();
}
//广告类型变化
function ChangeActionType() {
    var val = Ext.getCmp("ActionType").getValue().inputValue;
    switch (val) {
        case 1: //内部广告
            Ext.getCmp("Link").hide(); //链接地址
            Ext.getCmp("Info").show(); //内容
            var editor = new baidu.editor.ui.Editor({
                autoClearinitialContent: false,
                initialContent: '', //初始化编辑器的内容            
                minFrameHeight: 300, //设置高度            
                textarea: 'txteditor' //设置提交时编辑器内容的名字，之前我们用的名字是默认的editorValue       
            });
            editor.render('Info'); //渲染
            Ext.getCmp("Link").allowBlank = true;
            Ext.getCmp("Info").allowBlank = false;

            Ext.getCmp("NewList").hide(); //行业资讯
            Ext.getCmp("RelateName").allowBlank = true;
            break;
        case 2: //外部广告
            Ext.getCmp("Info").hide(); //内容
            Ext.getCmp("Link").show(); //链接地址
            Ext.getCmp("Link").allowBlank = false;
            Ext.getCmp("Info").allowBlank = true;
            if (Ext.getCmp("Link").getValue().trim() == "") {
                Ext.getCmp("Link").setValue("http://");
            }

            Ext.getCmp("NewList").hide(); //行业资讯
            Ext.getCmp("RelateName").allowBlank = true;

            break;
        case 3: //资讯广告
            Ext.getCmp("Link").hide(); //链接地址
            Ext.getCmp("Info").hide(); //内容
            Ext.getCmp("Link").allowBlank = true;
            Ext.getCmp("Info").allowBlank = true;

            Ext.getCmp("NewList").show(); //行业资讯
            Ext.getCmp("RelateName").allowBlank = false;
            break;
    }
    Ext.getCmp("viewMain").doLayout();
}
//选择资讯信息
function openMulit() {
    var shop = new top.Ext.Window({
        width: 840,
        id: 'NewsWindow',
        height: 500,
        closable: false,
        shadow: false,
        stateful: false,
        border: false,
        modal: true,
        layout: 'fit',
        plain: true,
        autoDestroy: true,
        closeAction: 'close',
        title: '资讯选择',
        items: {
            autoScroll: true,
            border: false,
            autoLoad: { url: '../../Project/Html/SelectNews.htm', scripts: true, nocache: true }
        },
        buttons: [
                    {
                        text: '确定',
                        iconCls: 'GTP_submit',
                        tooltip: '保存当前的选择',
                        handler: function () {
                            SaveSelectNews();
                        }
                    },
                    {
                        text: '取消',
                        iconCls: 'GTP_cancel',
                        tooltip: '取消当前的操作',
                        handler: function () {
                            top.Ext.getCmp("NewsWindow").close();
                        }
                    }]
    }).show();
            }

//保存确认选择
function SaveSelectNews() {
    var grid = top.Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].get("Id");
        var name = rows[0].get("Name");
        if (Ext.getCmp("ActiveName").getValue() == "") {
            Ext.getCmp("ActiveName").setValue(name);
        }
        Ext.getCmp("RelateName").setValue(name);
        Ext.getCmp("ActionFormId").setValue(key); //隐藏域,存储主键
        top.Ext.getCmp('NewsWindow').close();
    }
    else {
        top.Ext.Msg.show({ title: "信息提示", msg: '请选中一条资讯记录', buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
    }
}
//创建工具条
function Toolbar() {
    var tb = new Ext.Toolbar();
    tb.add({
        text: '保存',
        iconCls: 'GTP_save',
        id: 'GTP_save',
        tooltip: '提交保存',
        handler: SaveDate
    });
    tb.add('-');
    tb.add({
        text: '关闭',
        iconCls: 'GTP_cancel',
        tooltip: '关闭取消',
        handler: function (sender) {
            CloseWindow();
        }
    });
    return tb;
};

//可以显示上传图片
function FileUploadAction(val, a) {
    if (checkFile(val)) {
        url += "&isUpLoad=1";
        Ext.getCmp("browseImage").show();
        Ext.getCmp("browseImage_img").hide();
        if (isFirefox = navigator.userAgent.indexOf("Firefox") > 0) {
            //Firefox浏览器 
            //Ext.get('browseImage').dom.src = Ext.get('browseImage').dom.files.item(0).getAsDataURL();
        }
        else {
            //IE浏览器
            document.getElementById("browseImage").filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = "file://" + a;
        }
    }
}

//初始化图片信息
function SetPicView(path) {
    Ext.getCmp("browseImage").hide();
    Ext.getCmp("browseImage_img").show();
    Ext.getCmp("browseImage_img").getEl().dom.src = path;
}

//保存后关闭
function CloseWindow() {
    var id = parent.GetActiveTabId();
    if (id) {
        parent.CloseTabPanel(id); //传递页面id
    }
}
//图片上传
function OneFileUploadAction(val, a) {
    if (checkFile(val)) {
        var id = val.controlId; //控件编号
        //取消控件验证
        Ext.getCmp("Title").allowBlank = true;
        if (Ext.getCmp("Position").getValue().inputValue==1) {//轮播取消控件验证 
            Ext.getCmp("Img").allowBlank = true;
        }
        //模拟提交图片
        var formPanel = Ext.getCmp("formPanel");
        formPanel.getForm().submit({
            waitTitle: '', //标题
            waitMsg: '', //提示信息  
            submitEmptyText: false,
            url: '/Active/FileUpload?id=' + id, //记录表单提交的路径
            method: "POST",
            success: function (form, action) {
                //恢复控件验证
                Ext.getCmp("Title").allowBlank = false;
                if (Ext.getCmp("Position").getValue().inputValue == 1) {//轮播取消控件验证 
                    Ext.getCmp("Img").allowBlank = false;
                }

                var flag = action.result; //成功后
                if (flag.success) {
                    Ext.getCmp("winImg" + id).setValue(flag.msg);
                    Ext.getDom("showImg" + id).setAttribute("src", flag.msg); //右边预览图
                    //设置图片宽度高度
                    Ext.getCmp("ImgWitdh" + id).setValue(flag.data[0].width);
                    Ext.getCmp("ImgHeight" + id).setValue(flag.data[0].height);
                } else {
                    top.Ext.MessageBox.show({
                        title: '信息提示',
                        msg: flag.msg,
                        buttons: Ext.MessageBox.OK,
                        icon: Ext.MessageBox.INFO
                    });
                    Ext.getCmp("winImg" + id).setValue("");
                    return false
                }
            },
            failure: function (form, action) {
                //恢复控件验证
                Ext.getCmp("Title").allowBlank = false;
                if (Ext.getCmp("Position").getValue().inputValue == 1) {//轮播取消控件验证 
                    Ext.getCmp("Img").allowBlank = false;
                }
                var rs = eval('(' + action.response.responseText + ')');
                top.Ext.MessageBox.show({
                    title: '信息提示',
                    msg: rs.msg,
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.MessageBox.INFO
                });
                //清空图片值
                Ext.getCmp("winImg" + id).setValue("");
                return false
            }
        });
    }
}
//选择人员
function SelectJoinType() {
    //获取选中的信息
    var selectList = Ext.getCmp("txtPersonalId").getValue();
    new top.Ext.Window({
        id: "WinMasterList",
        width: 320,
        height: 470,
        closable: false,
        border: false,
        shadow: false,
        modal: true,
        layout: 'fit',
        plain: true,
        autoDestroy: true,
        closeAction: 'close',
        title: '人员选择',
        items: {
            autoScroll: true,
            autoLoad: { url: '../../Project/Html/SelectUser.htm', scripts: true, nocache: true },
            params: { Attribute:6, select: selectList }
        },
        buttons: [
        {
            text: '确定',
            iconCls: 'GTP_submit',
            tooltip: '保存当前的选择',
            handler: function () {
               SaveSelect();
            }
        },
        {
            text: '取消',
            iconCls: 'GTP_cancel',
            tooltip: '取消当前的操作',
            handler: function () {
                top.Ext.getCmp('WinMasterList').close();
            }
        }]
    }).show();
}

//保存私信选择人员
function SaveSelect() {
    var checkid = '';
    var checkName = '';
    var rt = top.rmtree.getChecked(); //得到所有所选的子节点
    var count = 0;
    if (rt.length > 0) {
        for (var i = 0; i < rt.length; i++) {//除去最顶端的根节点(模块功能分配)
            if (rt[i].getUI().checkbox) {
                if (!rt[i].getUI().checkbox.indeterminate) {
                    checkid += rt[i].id + ',';
                    checkName += rt[i].text + ',';
                    count++;
                }
            }
        }
    }
    else {
        top.Ext.Msg.show({ title: "信息提示", msg: '请先进行选择', buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
        return false;
    }
    if (count > 10) {
        top.Ext.Msg.show({ title: "信息提示", msg: '最多只能选择10人,请重新选择.', buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
        return false;
    }
    checkid = checkid.substring(0, checkid.length - 1);
    checkName = checkName.substring(0, checkName.length - 1);
    Ext.getCmp("txtPersonalId").setValue(checkid);
    Ext.getCmp("txtPersonalName").setValue(checkName);
    top.Ext.getCmp('WinMasterList').close();
}

