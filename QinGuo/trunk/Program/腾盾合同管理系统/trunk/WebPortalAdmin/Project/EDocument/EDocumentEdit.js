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
