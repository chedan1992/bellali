//数据库备份
function Adjustment() {
    var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要备份数据库吗?', function (e) {
        if (e == "yes") {
            Ext.Ajax.request({
                url: '/SysDataBaseBack/Back',
                //params: { id: ids.join(",") },
                success: function (response) {
                    var rs = eval('(' + response.responseText + ')');
                    if (rs.success) {
                        //判断是否在grid最后一条的时候删除,如果删除,重新加载
                        Ext.getCmp("gg").store.reload();
                        MessageInfo("备份成功！", "right");
                    } else {
                        MessageInfo("备份失败！", "error");
                    }
                }
            });
        }
    });
}

//数据库还原
function Revadjustment() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var confirm = top.Ext.MessageBox.confirm('系统确认', '您确认要将数据库还原成当前备份库吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                Ext.Ajax.request({
                    url: '/SysDataBaseBack/Rollback',
                    params: { id: ids.join(",") },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            //判断是否在grid最后一条的时候删除,如果删除,重新加载
                            Ext.getCmp("gg").store.reload();
                            MessageInfo("还原成功！", "right");
                        } else {
                            MessageInfo("还原失败！", "error");
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

//删除
function DeleteDate() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要删除该备份数据库吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                Ext.Ajax.request({
                    url: '/SysDataBaseBack/DeleteData',
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

//文件下载
function GTPdownload() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要下载数据库吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                var url = '/SysDataBaseBack/ImportOut?Id=' + ids.join(",") + '&date=' + new Date();
                $(".hideform").attr("action", url);
                $(".hideform").submit();
            }
        });
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}