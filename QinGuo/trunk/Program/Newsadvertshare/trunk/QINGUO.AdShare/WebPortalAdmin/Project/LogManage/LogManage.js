
//点击左边树形
function treeitemclick(node, e) {
    if (node.attributes.id != 0) {
        treeNodeId = node.attributes.id;
    }
    else {
        treeNodeId = "-1";
    }
    Ext.getCmp("gg").getStore().reload();
}
//批量导出
function BatchExport() {
    var grid = Ext.getCmp("gg");
    if (grid.store.data.length > 0) {
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确定要导出列表信息吗?', function (e) {
            if (e == "yes") {
                var url = '/LogManage/ImportOut?date=' + new Date();
                $(".hideform").attr("action", url);
                $(".hideform").submit();
            }
        });
    } else {
        MessageInfo("没有数据可以导出！", "statusing");
    }
}

//清空
function GTPEmpty() {
    var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要全部清空访问记录吗?', function (e) {
        if (e == "yes") {
            Ext.Ajax.request({
                url: '/LogManage/DeleteAll?GroupId=' + treeNodeId,
                success: function (response) {
                    var rs = eval('(' + response.responseText + ')');
                    if (rs.success) {
                        //判断是否在grid最后一条的时候删除,如果删除,重新加载
                        reload();
                        MessageInfo("清空成功！", "right");
                    } else {
                        MessageInfo("清空失败！", "error");
                    }
                }
            });
        }
    });
}


//删除
function DeleteDate() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要删除该记录吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push("" + rows[i].data["Id"] + "");
                }
                Ext.Ajax.request({
                    url: '/LogManage/DeleteData',
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
