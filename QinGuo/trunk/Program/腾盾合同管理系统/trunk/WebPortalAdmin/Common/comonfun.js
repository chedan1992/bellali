/*
*注意要点:
*1:commonfun.js 和 common.js都是父类,为什么不把他们放在一起,因为commonfun.js存放的方法不用每次都调用,目前放在 Index.aspx页
*
*
*/
Ext.onReady(function () {
    //获取用户信息
    var respon = Ext.lib.Ajax.getConnectionObject().conn;
    respon.open("post", "/Home/GetLoginUser", false);
    respon.send(null);
    var result = Ext.util.JSON.decode(respon.responseText);
    if (result) {
        if (!result.success) {
            window.location.href = loginError; //重新登录
        }
        else {
            result = Ext.util.JSON.decode(result.msg);
            var smalldate = result.LoginTime == null ? new Date().format('Y-m-d H:m:s') : new Date(formartTime(result.LoginTime)).format('Y-m-d H:m:s');
            if (smalldate.indexOf('N') > 0) {
                smalldate = '您是第一次登录';
            }
            LoginUser = new Object();
            LoginUser.Id = result.Id; //用户编号
            LoginUser.UserName = result.UserName; //用户名称
            LoginUser.LoginIp = result.LoginIp == null ? "暂无IP信息" : result.LoginIp; //用户登录IP
            LoginUser.LoginTime = smalldate; //上次登录时间
            LoginUser.LoginNum = result.LoginNum; //总计登录次数
            LoginUser.Company = result.Company == null ? "超级系统管理员" : result.Company; //公司名称
            LoginUser.Attribute = result.Attribute; //角色类型(0:系统超级管理员 1:系统管理员 2:单位管理员 3:消防部门管理员 4:维保公司管理员)
            LoginUser.CompanyName = result.Company.Name; //公司名称
            LoginUser.IsLookMessageTip = result.IsLookMessageTip; //是否有右下角的消息提示权限
        }
    }
    else {
        window.location.href = loginError; //重新登录
    }


});

/*子页面添加tab
title:标题
id:tab的id
parentId:它的父业面
html:链接地址
*/
//点击左边树形菜单,右边添加tab页
function AddTabPanel(title, id,parentId, html) {
    var centerPanel = Ext.getCmp("ShareCenter");
    if (!centerPanel) {
        centerPanel = parent.Ext.getCmp("ShareCenter");
    }
    var index = centerPanel.items.length;

    var tabs = centerPanel.add({
        tabTip: '双击关闭',
        'id': id,
        layout: 'fit',
        name: parentId, //记录它的父页面id,刷新使用
        'title': title,
        closable: true,
        html: '<iframe id="frame_content" width="100%" height="100%" name="" frameborder="0" src=' + html + '></iframe>',
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
    centerPanel.doLayout();
}

//点击添加tab页
function AddFlowTabPanel(title, id, parentId, html) {
    var centerPanel = Ext.getCmp("ShareCenter");
    if (!centerPanel) {
        centerPanel = parent.Ext.getCmp("ShareCenter");
    }

    var tab = Ext.getCmp(id);
    if (!tab) {
        tab = parent.Ext.getCmp(id);
    }
    centerPanel.remove(tab);

    var index = centerPanel.items.length;

    var tabs = centerPanel.add({
        tabTip: '双击关闭',
        'id': id,
        layout: 'fit',
        name: parentId, //记录它的父页面id,刷新使用
        'title': title,
        closable: true,
        html: '<iframe id="frame_content" width="100%" height="100%" name="" frameborder="0" src=' + html + '></iframe>',
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
}

//关闭门户页面
function CloseTabPanel(id) {
    var tab = Ext.getCmp(id);
    if (!tab) {
        tab = parent.Ext.getCmp(id);
    }

    var centerPanel = Ext.getCmp("ShareCenter");
    if (!centerPanel) {
        centerPanel = parent.Ext.getCmp("ShareCenter");
    }

    centerPanel.remove(tab);

    if (tab.name) { //刷新父页面
        var parentTab = Ext.getCmp(tab.name);
        if (!parentTab) {
            parentTab = parent.Ext.getCmp(tab.name);
        }
        if (Ext.getDom("frame_content")) {
            Ext.getDom("frame_content").src = Ext.getDom("frame_content").src;
        }
    }
    centerPanel.getActiveTab().getUpdater().refresh();
 
}


//获取当前激活的tab id
function GetActiveTabId() {
    var centerPanel = Ext.getCmp("ShareCenter");
    if (!centerPanel) {
        centerPanel = parent.Ext.getCmp("ShareCenter");
    }
    return centerPanel.getActiveTab().id;
}

