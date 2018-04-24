//渲染设备状态
function renderMaintenanceStatus(val, metadata, record, rowIndex, columnIndex, store) {
    if (val == -1) {
        return "<span style='color:red'>已过期</span>";
    }
    else if (val == 0)
        return "<span style='color:green'>设备正常</span>";
    else if (val == 1) {
        return "<span style='color:orange'>设备异常</span>";
    }
}

//查看详细
function LookDeitl() {
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].get("Id");
        var win = CreateFromWindow("详细", key);
        var Img = rows[0].get("Img");
        if (Img != "") {
            Img = Img.split(',');
            var img = "";
            for (var i = 0; i < Img.length; i++) {
                var ID = "Img" + i + "";
                img += '<div style="position:absoulute;float:left;"><a id=' + ID + ' title="" href="' + Img[i] + '" target="_blank"><img style="top:-5px;z-index:1; padding-left:3px;" src="' + Img[i] + '" width="80" height="80""/></a></div>';
            }
            top.Ext.getCmp("Plzz").html = img;
        }
        else {
            top.Ext.getCmp("Plzz").html = "<span style='color:silver;margin-top:10px;'>(未传照片)</span>";
        }
        var form = top.Ext.getCmp('formPanel');
        top.Ext.getCmp("formPanel").form.loadRecord(rows[0]); //再加载数据   
        form.doLayout();
        win.show();

        var Img = rows[0].get("Img");
        if (Img != "") {
            Img = Img.split(',');
            var img = "";
            for (var i = 0; i < Img.length; i++) {
                var ID = "Img" + i + "";
                $("#" + ID + "").imgbox();
            }
        }

    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}

//创建表单弹框
function CreateFromWindow(title, key) {
    //表单
    var form = new top.Ext.FormPanel({
        id: "formPanel",
        labelAlign: 'right',
        frame: true,
        labelWidth: 80,
        bodyStyle: 'padding:5px 5px 0',
        width: 650,
        autoScroll: true,
        items: [
            {
                xtype: "fieldset",
                autoHeight: true,
                title: "巡检照片信息",
                layout: 'column',
                items: [
                    {
                        columnWidth: 1,
                        layout: 'form',
                        items: [
                            {
                                name: 'Plzz',
                                id: 'Plzz',
                                fieldLabel: '照片',
                                style: 'padding-bottom:5px;',
                                xtype: 'panel',
                                html: ''
                            }
                        ]
                    }
                ]
            }
        ]
    });

    //弹出窗体
    var win = new top.Ext.Window({
        id: "jiucuo",
        title: title,
        shadow: false,
        stateful: false,
        width: 650,
        height: 345,
        minHeight: 300,
        modal: true,
        layout: 'fit',
        border: false,
        closeAction: 'close',
        items: form,
        buttons: [
                    {
                        text: '取消',
                        iconCls: 'GTP_cancel',
                        handler: function () {
                            top.Ext.getCmp("jiucuo").close(); //直接销毁
                        }
                    }
       ]
    });
    return win;
}