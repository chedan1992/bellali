//广告类型变化
function ChangeType() {
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
            break;
        case 2: //外部广告
            Ext.getCmp("Info").hide(); //内容
            Ext.getCmp("Link").show(); //链接地址
            Ext.getCmp("Link").allowBlank = false;
            Ext.getCmp("Info").allowBlank = true;
            if (Ext.getCmp("Link").getValue().trim() == "") {
                Ext.getCmp("Link").setValue("http://");
            }
            break;
    }
    Ext.getCmp("viewMain").doLayout();
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
            Ext.get('browseImage').dom.src = Ext.get('browseImage').dom.files.item(0).getAsDataURL();
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
