//渲染时间
function renderTime(val, metadata, record, rowIndex, colIndex, store) {
    if (val) {
        //起止时间
        var EndTime = record.data.EndTime;
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
//选择员工
function selectMaster() {
    var shop = new top.Ext.Window({
        width: 680,
        id: 'shoper',
        height:500,
        closable: false,
        shadow: false,
        stateful: false,
        border: false,
        modal: true,
        layout: 'fit',
        plain: true,
        autoDestroy: true,
        closeAction: 'close',
        title: '工程师选择',
        items: {
            autoScroll: true,
            border: false,
            params: { IsActive: true, Active: 4 },
            autoLoad: { url: '../../Project/Html/SelectMasterGrid.htm', scripts: true, nocache: true }
        },
        buttons: [
                    {
                        text: '确定',
                        iconCls: 'GTP_submit',
                        handler: function () {
                            SaveMaster();
                        }
                    },
                    {
                        text: '取消',
                        iconCls: 'GTP_cancel',
                        handler: function () {
                            top.Ext.getCmp("shoper").close();
                        }
                    }]
    }).show();
}

//确定选择商家
function SaveMaster() {
    var grid = top.Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var EngineerId = rows[0].get("Id"); //工程师编号
        var Key = "";

         var Listgrid = Ext.getCmp("gg");
        //得到选后的数据   
         var Listrows = Listgrid.getSelectionModel().getSelections();
         if (Listrows.length == 1) {
             Key = Listrows[0].get("Id");
         }
         var HasOrder = rows[0].get("HasOrder");
         if (parseInt(HasOrder) == 1) {
            top.Ext.Msg.show({ title: "信息提示", msg: '该工程师当天任务已满，不能派单了', buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
        }
        else {
            Ext.Ajax.request({
                url: '/ETask/AssignSingle',
                params: { EngineerId: EngineerId, key: Key },
                success: function (response) {

                    top.Ext.getCmp('shoper').close();
                    var rs = eval('(' + response.responseText + ')');
                    if (rs.success) {
                        //判断是否在grid最后一条的时候删除,如果删除,重新加载
                        Ext.getCmp("gg").store.reload();
                        MessageInfo("派单成功！", "right");
                    } else {
                        MessageInfo("派单失败！", "error");
                    }
                }
            });
        }
    }
    else {
        top.Ext.Msg.show({ title: "信息提示", msg: '请选中一条记录', buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
    }

}

//审核
function GTPAudit() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length > 0) {
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要审核通过吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                Ext.Ajax.request({
                    url: '/ETask/Flow',
                    params: { id: ids.join(",") },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            Ext.getCmp("gg").store.reload();
                            MessageInfo("审核成功！", "right");
                        } else {
                            MessageInfo("审核失败！", "error");
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
































