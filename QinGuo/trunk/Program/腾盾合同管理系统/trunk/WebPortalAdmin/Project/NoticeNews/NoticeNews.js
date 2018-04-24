//渲染公告类型
function AdType(val, rows) {
    switch (val) {
        case 1:
            return '内部公告';
            break;
        case 2:
            return '外部公告';
            break;
        case 3:
            return '资讯公告';
            break;
    }
}

//重写工具条
function tbar(PageName) {
    //同步查询页面权限按钮
    var tb = new Ext.Toolbar();

    if (PageName != '') {
        var respon = Ext.lib.Ajax.getConnectionObject().conn;
        respon.open("post", "/SysMaster/GetRoleBtn?PageAction=" + PageName, false); //获取页面按钮
        respon.send(null);
        var result = Ext.util.JSON.decode(respon.responseText);
        if (result.success) {
            if (result.msg != "") {
                var result = Ext.util.JSON.decode(result.msg);
                for (var i = 0; i < result.length; i++) {
                    var Name = result[i].Name;
                    var NameTip = result[i].NameTip;
                    var IConName = result[i].IConName;
                    var ActionName = result[i].ActionName;
                    if (i != 0) {
                        tb.add('-');
                    }
                    tb.add({
                        text: Name,
                        tooltip: NameTip,
                        id: IConName,
                        iconCls: IConName,
                        handler: BtnFun(ActionName) //转义方法
                    });
                }
            }
        }
        else {
            if (result.errorCode == 9) {
                top.Ext.Msg.show({
                    title: "信息提示",
                    msg: "用户信息已过期,请重新登录",
                    buttons: Ext.Msg.OK,
                    icon: Ext.MessageBox.INFO,
                    fn: GoOut//退出
                });
                return false;
            }
        }
    }

    if (searchData.length > 0) {
        tb.add('->');
        //查询条件
        var comName = new Ext.form.ComboBox({
            store: new Ext.data.SimpleStore({
                fields: ['text', 'value'],
                data: this.searchData
            }),
            displayField: 'text',
            id: 'comName',
            valueField: 'value',
            mode: 'local',
            selectOnFocus: true,
            forceSelection: true,
            editable: false,
            triggerAction: 'all',
            allowBlank: false,
            value: this.seachkey,
            width: 90,
            listeners: {
                select: function (mysel) {
                    var itemstr = mysel.lastSelectionText;
                    if (itemstr.trim() == "全查询") {
                        Ext.getCmp("comContent").setValue("");
                        Ext.getCmp("comSign").setValue('like');
                    }
                    if (itemstr.indexOf('时间') > -1) {
                        Ext.getCmp("comTimeContent").show();
                        Ext.getCmp("comContent").hide();
                        Ext.getCmp("comEnum").hide();
                        Ext.getCmp("Attribute").hide();
                        Ext.getCmp("OrderStatus").hide();
                        Ext.getCmp("FlowEnum").hide();
                        Ext.getCmp("comSign").setValue('like');
                    }
                    else if (itemstr.indexOf('状态') > -1 && itemstr.length == 2) {
                        Ext.getCmp("comTimeContent").hide();
                        Ext.getCmp("comContent").hide();
                        Ext.getCmp("comEnum").show();
                        Ext.getCmp("FlowEnum").hide();
                        Ext.getCmp("Attribute").hide();
                        Ext.getCmp("OrderStatus").hide();
                        Ext.getCmp("comSign").setValue('=');
                    }
                    else if (itemstr.indexOf('公告位置') > -1) {
                        Ext.getCmp("comTimeContent").hide();
                        Ext.getCmp("comContent").hide();
                        Ext.getCmp("comEnum").hide();
                        Ext.getCmp("FlowEnum").show();
                        Ext.getCmp("Attribute").hide();
                        Ext.getCmp("OrderStatus").hide();
                        Ext.getCmp("comSign").setValue('=');
                    } else if (itemstr.indexOf('触发条件') > -1) {
                        Ext.getCmp("comTimeContent").hide();
                        Ext.getCmp("comContent").hide();
                        Ext.getCmp("comEnum").hide();
                        Ext.getCmp("Attribute").show();
                        Ext.getCmp("FlowEnum").hide();
                        Ext.getCmp("OrderStatus").hide();
                        Ext.getCmp("comSign").setValue('=');
                    }
                    else if (itemstr.indexOf('公告类型') > -1 && itemstr.length == 4) {
                        Ext.getCmp("comTimeContent").hide();
                        Ext.getCmp("comContent").hide();
                        Ext.getCmp("comEnum").hide();
                        Ext.getCmp("Attribute").hide();
                        Ext.getCmp("FlowEnum").hide();
                        Ext.getCmp("OrderStatus").show();
                        Ext.getCmp("comSign").setValue('=');
                    }
                    else {
                        Ext.getCmp("OrderStatus").hide();
                        Ext.getCmp("comTimeContent").hide();
                        Ext.getCmp("comContent").show();
                        Ext.getCmp("comEnum").hide();
                        Ext.getCmp("FlowEnum").hide();
                        Ext.getCmp("Attribute").hide();
                        Ext.getCmp("comSign").setValue('like');
                    }
                }
            }
        });
        //查询方式
        var comSign = new Ext.form.ComboBox({
            store: new Ext.data.SimpleStore({
                fields: ['text', 'value'],
                data: this.signdata
            }),
            displayField: 'text',
            valueField: 'value',
            mode: 'local',
            id: 'comSign',
            selectOnFocus: true,
            orceSelection: true,
            editable: false,
            triggerAction: 'all',
            allowBlank: false,
            value: 'like',
            width: 60
        });
        //查询内容
        var comContent = new Ext.form.TextField({
            width: 120,
            id: 'comContent',
            emptyText: '请输入搜索条件'
        });

        //查询时间内容
        var comTimeContent = new Ext.form.DateField({
            width: 120,
            id: 'comTimeContent',
            emptyText: '选择时间',
            format: 'Y-m-d',
            hidden: true
        });

        //查询枚举内容
        var comEnum = new Ext.form.ComboBox({
            store: new Ext.data.SimpleStore({
                fields: ['text', 'value'],
                data: [['启用', '1'], ['禁用', '0']]
            }),
            displayField: 'text',
            valueField: 'value',
            mode: 'local',
            id: 'comEnum',
            selectOnFocus: true,
            orceSelection: true,
            editable: false,
            triggerAction: 'all',
            allowBlank: false,
            value: '1',
            hidden: true,
            width: 120
        });

        //公告位置
        var FlowEnum = new Ext.form.ComboBox({
            store: new Ext.data.SimpleStore({
                fields: ['text', 'value'],
                data: [['App轮播', '1'], ['公司首页', '2'], ['App首页', '3']]
            }),
            displayField: 'text',
            valueField: 'value',
            mode: 'local',
            id: 'FlowEnum',
            selectOnFocus: true,
            orceSelection: true,
            editable: false,
            triggerAction: 'all',
            allowBlank: false,
            value: '1',
            hidden: true,
            width: 120
        });

        //触发条件
        var Type = new Ext.form.ComboBox({
            store: new Ext.data.SimpleStore({
                fields: ['text', 'value'],
                data: [['首下载', '1'], ['首登录', '2'], ['登录', '3'], ['App首页', '4'], ['首关注', '5'], ['首分享', '6'], ['首订单', '7']]
            }),
            displayField: 'text',
            valueField: 'value',
            mode: 'local',
            id: 'Attribute',
            selectOnFocus: true,
            orceSelection: true,
            editable: false,
            triggerAction: 'all',
            allowBlank: false,
            value: 1,
            hidden: true,
            width: 120
        });

        //公告类型
        var AdType = new Ext.form.ComboBox({
            store: new Ext.data.SimpleStore({
                fields: ['text', 'value'],
                data: [['内部公告', '1'], ['外部公告', '2'], ['资讯公告', '3']]
            }),
            displayField: 'text',
            valueField: 'value',
            mode: 'local',
            id: 'OrderStatus',
            selectOnFocus: true,
            orceSelection: true,
            editable: false,
            triggerAction: 'all',
            allowBlank: false,
            value: 1,
            hidden: true,
            width: 120
        });

        tb.add('快捷查询:');
        tb.add(comName);
        tb.add(comSign);
        tb.add(comContent); //文本输入
        tb.add(comTimeContent); //时间框输入
        tb.add(comEnum); //启用 禁用 枚举
        tb.add(FlowEnum);
        tb.add(Type);
        tb.add(AdType);

        tb.add([{
            text: '搜索',
            iconCls: 'GTP_search',
            id: 'GTP_search',
            tooltip: '搜索满足条件的数据',
            scope: this,
            handler: function () {
                if (comName.isValid() && comSign.isValid()) {
                    SearchDate();
                }
            }
        }]);
    }

    return tb;
};


//查看
function LookInfo() {
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].get("Id");
        var ActionType = rows[0].get("ActionType");
        var title = rows[0].get("ActiveName");
        if (title.length > 8) {
            title = title.substring(0, 8) + "....";
        }
        if (parent) {
            var tabId = parent.GetActiveTabId(); //公共变量
            parent.AddTabPanel(title, key, tabId, '/NoticeNews/LookView?Id=' + key);
        }
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}

//渲染时间
function renderTime(val, metadata, record, rowIndex, colIndex, store) {
    if (val) {
        if (record.data.ActionType == 2) {
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
//启用
function EnableUse() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        if (rows[0].data["Status"] == 1) {
            MessageInfo("该公告已经启用！", "statusing");
            return;
        }
        if (rows[0].data["Status"] == -2) {
            MessageInfo("该公告已过期,不能启用！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要启用该公告吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                Ext.Ajax.request({
                    url: '/NoticeNews/EnableUse',
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
            MessageInfo("该公告已经禁用！", "statusing");
            return;
        }
        if (rows[0].data["Status"] == -2) {
            MessageInfo("该公告已过期,不能禁用！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要禁用该公告吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                Ext.Ajax.request({
                    url: '/NoticeNews/DisableUse',
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

//新增
function AddDate() {
    if (parent) {
        var tabId = parent.GetActiveTabId(); //公共变量
        parent.AddTabPanel('新增公告', 'AdvertiseWindow', tabId, '/NoticeNews/Edit');
    }
}

//编辑
function EditDate() {
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].get("Id");

        var Status = rows[0].get("Status");
        if (Status == 1) {
            MessageInfo("请先禁用后再编辑！", "statusing");
            return false;
        }
        var title = rows[0].get("ActiveName");
        if (title.length > 8) {
            title = title.substring(0, 8) + "....";
        }
        if (parent) {
            var tabId = parent.GetActiveTabId(); //公共变量
            parent.AddTabPanel(title, 'ShopAdvertiseWindow', tabId, '/NoticeNews/Edit?Id=' + key + '&modify=1');
        }
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
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要删除该公告吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push("" + rows[i].data["Id"] + "");
                }
                Ext.Ajax.request({
                    url: '/NoticeNews/DeleteData',
                    params: { id: ids.join(",") },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            //判断是否在grid最后一条的时候删除,如果删除,重新加载
                            Ext.getCmp("gg").store.reload();
                            MessageInfo("删除成功！", "right");
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

