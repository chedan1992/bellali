//新增
function AddDate() {
    var shop = new top.Ext.Window({
        width: 680,
        id: 'shoper',
        height: 510,
        closable: false,
        shadow: false,
        stateful: false,
        border: false,
        modal: true,
        layout: 'fit',
        plain: true,
        autoDestroy: true,
        closeAction: 'close',
        title: '社会单位选择',
        items: {
            autoScroll: true,
            border: false,
            autoLoad: { url: '../../Project/Html/SelectCompanyGrid.htm', scripts: true, nocache: true }
        },
        buttons: [{
            text: '确定',
            iconCls: 'GTP_submit',
            handler: function () {
                SaveSelect();
            }
        },
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

//确定选择
function SaveSelect() {
    //存储题库
    var rows = top.CheckList; //获取选中的
    //得到选后的数据   
    if (rows.length == 0) {
        var test = top.TestListId;
        if (test.length == 0) {
            top.Ext.Msg.show({ title: "信息提示", msg: '请先进行选择', buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
            return;
        }
        else {
            top.Ext.Msg.show({ title: "信息提示", msg: '没有新的数据变化.请重新选择', buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
            return;
        }
    }
    Ext.Ajax.request({
        url: '/SysCompanyCognate/SaveParentIndex',
        params: { IdList: rows.join(",") },
        success: function (response) {
            var rs = eval('(' + response.responseText + ')');
            if (rs.success) {
                top.Ext.getCmp("shoper").close();
                MessageInfo("关联成功！", "right");
                Ext.getCmp("gg").store.reload();
            } else {
                MessageInfo("关联失败！", "error");
            }
        }
    });
}
//删除
function DeleteDate() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要删除该关联单位吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                Ext.Ajax.request({
                    url: '/SysCompanyCognate/DeleteData',
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


//撤销
function GTPIntransit() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var FlowStatus = rows[0].data["Status"];
        if (FlowStatus == 0) {
            var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要撤销该关联单位吗?', function (e) {
                if (e == "yes") {
                    var ids = [];
                    for (var i = 0; i < rows.length; i++) {
                        ids.push(rows[i].data["Id"]);
                    }
                    Ext.Ajax.request({
                        url: '/SysCompanyCognate/DeleteIndex',
                        params: { id: ids.join(",") },
                        success: function (response) {
                            var rs = eval('(' + response.responseText + ')');
                            if (rs.success) {
                                MessageInfo("撤销成功！", "right");
                                Ext.getCmp("gg").store.reload();
                            } else {
                                MessageInfo("撤销失败！", "error");
                            }
                        }
                    });
                }
            });
        } else {
            MessageInfo("流程已经审核,不能撤销！", "statusing");
        }

    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}