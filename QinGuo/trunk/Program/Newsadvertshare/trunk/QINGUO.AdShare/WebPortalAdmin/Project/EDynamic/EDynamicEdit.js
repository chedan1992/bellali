//渲染时间
function renderTime(val, metadata, record, rowIndex, colIndex, store) {
    if (val) {
        if (record.data.ShowType == 2) {
            //起止时间
            var EndTime = record.data.ActiveEndTime;
            val = val.replace(/-/g, "/");
            EndTime = EndTime.replace(/-/g, "/");
            var date = new Date(val); //开始时间
            var EndTime = new Date(EndTime); //结束时间

            if (date.format('Y-m-d') == EndTime.format('Y-m-d')) {
                var str = date.format('Y-m-d(周l) H:i') + "~" + EndTime.format('H:i');
                metadata.attr = 'ext:qtip="' + str + '"';
                return str;
            }
            else {
                var str = date.format('Y-m-d(周l) H:i') + "~" + EndTime.format('Y-m-d(周l) H:i');
                metadata.attr = 'ext:qtip="' + str + '"';
                return str;
            }
        }
        else {
            return "(无限制)";
        }
    }
    else {
        return "(无限制)";
    }
}

function itemclick(node, e) {
    if (node.id != '-1') {
        Ext.getCmp("GroupId").setValue(node.id);
    }
    else {
        top.Ext.Msg.show({ title: "信息提示", msg: "请选择具体的新闻分类.", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
        return;
    }
}

//切换模版
function radioChk(val) {
    var limit =parseInt(val.getAttribute("value"));
    var form = Ext.getCmp('ContentForm').items;
    var count = GetImageCount();
    if (limit < count) {

        var deleteNum = count - limit;
        var totalNum = form.getCount();

        for (var i = totalNum;; i--) {
            if (deleteNum == 0) {
                break;
            }
            if (form.items[i-1].hidden == false) {//隐藏的控件 
                var num = form.items[i-1].num;
                var win = Ext.getCmp('win' + num);
                win.hide();
                controlNum--;
                deleteNum--;
                Ext.getCmp('ContentForm').doLayout();
            }
        }
    }
}
//获取新增控件数量
function GetImageCount() {
    var index = 0;
    var form = Ext.getCmp('ContentForm').items;
    var count = form.getCount();
    for (var i = 0; i<count; i++) {
        if (form.items[i].hidden == false) {//隐藏的控件
            index++;
        }
    }
    return index+1;
}

//展示方式变化
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

//新闻类型
function ChangeActionType() {
    var val = Ext.getCmp("ActionType").getValue().inputValue;
    switch (val) {
        case 1: //内部新闻
            Ext.getCmp("Link").hide(); //链接地址
            Ext.getCmp("Content").show(); //内容
            var editor = new baidu.editor.ui.Editor({
                autoClearinitialContent: false,
                initialContent: '', //初始化编辑器的内容            
                minFrameHeight: 300, //设置高度            
                textarea: 'txteditor' //设置提交时编辑器内容的名字，之前我们用的名字是默认的editorValue       
            });
            editor.render('Content'); //渲染
            Ext.getCmp("Link").allowBlank = true;
            Ext.getCmp("Content").allowBlank = false;
            break;
        case 2: //外部新闻
            Ext.getCmp("Content").hide(); //内容
            Ext.getCmp("Link").show(); //链接地址
            Ext.getCmp("Link").allowBlank = false;
            Ext.getCmp("Content").allowBlank = true;
            if (Ext.getCmp("Link").getValue().trim() == "") {
                Ext.getCmp("Link").setValue("http://");
            }
            break;
    }
    Ext.getCmp("viewMain").doLayout();
}

//图片上传
function OneFileUploadAction(val, a) {
    if (checkFile(val)) {
        var id = val.controlId; //控件编号
        Ext.getCmp("Ismodify" + id).setValue("0"); //是否图片为修改记录
//        Ext.getCmp("browseImage" + id).show();
//        Ext.getCmp("browseImage_img" + id).hide();
//        if (isFirefox = navigator.userAgent.indexOf("Firefox") > 0) {
//            //Firefox浏览器 
//            Ext.get('browseImage').dom.src = Ext.get('browseImage').dom.files.item(0).getAsDataURL();
//        }
//        else {
//            //IE浏览器
//            document.getElementById("browseImage" + id).filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = "file://" + a;
//        }
    }
}


//添加组合广告
var controlid =1; //控件id
var controlNum = 1;
//添加控件
function AddCombin() {

    var limit =1;
    var id = document.getElementsByName('Vote');
    var value = new Array();
    for (var i = 0; i < id.length; i++) {
        if (id[i].checked)
            limit = id[i].value;
    }
    if (controlNum < parseInt(limit)) {
        controlid++;
        controlNum++;
        var oForm = Ext.getCmp('ContentForm');
        var panel = new Ext.Panel({
            layout: 'column',
            border: false,
            id: 'win' + controlid,
            num: controlid, //当前控件的主键序号
            bodyStyle: 'padding:2 2 2 2',
            items: [
                {
                    xtype: 'compositefield',
                    id: 'compositefield' + controlid,
                    combineErrors: false, //取消白色背景
                    items: [
                            {
                                layout: "form", // 从上往下的布局
                                labelAlign: "left",
                                border: false,
                                items: [
                                    {
                                        xtype: 'textfield',
                                        value: '1',
                                        id: 'Ismodify' + controlid,
                                        hidden: true
                                    },
                                     {
                                         fieldLabel: '主键',
                                         xtype: 'textfield',
                                         id: 'hidKey' + controlid,
                                         hidden: true
                                     },
                                     {
                                         xtype: 'fileuploadfield',
                                         emptyText: '可上传标识图',
                                         hideLabel: true,
                                         id: 'winImg' + controlid,
                                         controlId: controlid,
                                         allowBlank: false,
                                         buttonText: '',
                                         buttonCfg: {
                                             iconCls: 'image_add',
                                             tooltip: '图片选择'
                                         },
                                         width: 250,
                                         listeners: {
                                             'fileselected': {
                                                 fn: OneFileUploadAction,
                                                 scope: this
                                             }
                                         }
                                     }
                                   ]
                            },
                            {
                                xtype: "button",
                                labelAlign: "right",
                                text: "-",
                                width: 40,
                                colspan: 2,
                                id: 'del' + controlid,
                                listeners: {
                                    click: function (t, f, s) {
                                        controlNum--;
                                        var id = t.id
                                        id = id.substring(3, id.length);
                                        if (id) {
                                            var win = Ext.getCmp('win' + id);
                                            Ext.getCmp('Ismodify' + id).setValue(-1);
                                            win.hide();
                                            oForm.doLayout();
                                        }
                                    }
                                }
                            },
                            {
                                xtype: 'box', //或者xtype: 'component',
                                id: 'browseImage' + controlid,
                                width: 60, //图片宽度
                                height: 25, //图片高度
                                autoEl: {
                                    tag: 'div',    //指定为img标签
                                    style: "filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=scale);"
                                }
                            },
                            {
                                xtype: 'box', //或者xtype: 'component',
                                id: 'browseImage_img' + controlid,
                                width: 60, //图片宽度
                                hidden: true,
                                height: 25, //图片高度
                                autoEl: {
                                    tag: 'img',    //指定为img标签
                                    src: '/Extjs/resources/images/default/s.gif'
                                }
                            }
                         ]
                }
                ]
        });
        oForm.items.add(oForm.items.getCount(), panel);
        oForm.doLayout();
    }
    else {
        MessageInfo("最多上传"+limit+"张！", "right");
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

//保存后关闭
function CloseWindow() {
    var id = parent.GetActiveTabId();
    if (id) {
        parent.CloseTabPanel(id); //传递页面id
    }
}
