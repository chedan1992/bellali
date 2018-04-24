//点击组织架构
function treeitemclick(node, e) {
    if (node.attributes.id !="top") {
        treeNodeId = node.attributes.id;
    }
    else {
        treeNodeId = "-1";
    }
    Ext.getCmp("gg").getStore().reload();
}


//查看
function LookInfo() {
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].get("Id");
        var title = rows[0].get("Name");
        if (title.length > 8) {
            title = title.substring(0, 8) + "....";
        }
        if (parent) {
            var tabId = parent.GetActiveTabId(); //公共变量
            parent.AddTabPanel(title, key, tabId, '/EDocument/DocumentView?Id=' + key);
        }  
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}

//渲染时间
function renderTime(val, metadata, record, rowIndex, colIndex, store) {
    if (val) {
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
}
//启用
function EnableUse() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        if (rows[0].data["Status"] == 1) {
            MessageInfo("该文章已经启用！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要启用该文章吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                Ext.Ajax.request({
                    url: '/EDocument/EnableUse',
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
            MessageInfo("该文章已经禁用！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要禁用该文章吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                Ext.Ajax.request({
                    url: '/EDocument/DisableUse',
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
    var grid = Ext.getCmp("tr");
    var rows = grid.getSelectionModel().getSelectedNode();
    var CreateCompanyId = '0';
    if (rows) {
        if (rows.attributes.id !="top") {
            CreateCompanyId = rows.attributes.id;
        }
        else {
            MessageInfo("请先选择品牌！", "statusing");
            return false;
        }
    }
    else {
        MessageInfo("请先选择品牌！", "statusing");
        return false;
    }

    if (parent) {
        var tabId = parent.GetActiveTabId(); //公共变量
        parent.AddTabPanel('新增文章', 'EDocumentWindow', tabId, '/EDocument/Edit?BrandId=' + CreateCompanyId);
    }
}

//编辑
function EditDate() {
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].get("Id");
        var title = rows[0].get("Name");
        if (title.length > 8) {
            title = title.substring(0, 8) + "....";
        }
        if (parent) {
            var tabId = parent.GetActiveTabId(); //公共变量
            parent.AddTabPanel(title, 'EDocumentWindow', tabId, '/EDocument/Edit?Id=' + key + '&modify=1');
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
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要删除该文章吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(""+rows[i].data["Id"]+"");
                }
                Ext.Ajax.request({
                    url: '/EDocument/DeleteData',
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
