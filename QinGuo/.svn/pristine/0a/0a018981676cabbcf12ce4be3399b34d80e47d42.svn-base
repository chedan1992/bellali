function formartFlowStatus(val, row) {
    if (val == -1) {
        return "<span style='color:Red'>删除</span>";
    }
    else if (val == 0)
        return "<span style='color:#7A7A7A'>未启动</span>";
    else if (val == 1) {
        return "<span style='color:Red'>待审核</span>";
    } else if (val == 2) {
        return "<span style='color:green'>已受理</span>";
    }
    else if (val == 3) {
        return "<span style='color:#7A7A7A'>未通过</span>";
    }
}

//grid双击默认进行编辑操作
function dbGridClick(grid, rowindex, e) {
    LookInfo();
}

//审核
function GTPAudit() {
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var Status = rows[0].get("Status");
        if (Status != 1) {
            MessageInfo("该申请已经处理过了！", "statusing");
        }
        else {
            var key = rows[0].get("Id");
            var win = CreateFromWindow("审核", key);

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
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}


//查看
function LookInfo() {
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].get("Id");
        var win = CreateFromWindow("审核", key);

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

        var key = rows[0].get("Id");
        var title = rows[0].get("Name");
       
        var form = top.Ext.getCmp('formPanel');
        top.Ext.getCmp("formPanel").form.loadRecord(rows[0]); //再加载数据   
        form.doLayout();
        win.show();

        var Status = rows[0].get("Status");
        if (parseInt(Status) == 2 || parseInt(Status) == 3) {
            top.Ext.getCmp("feedback1").setValue(rows[0].get("feedback"));
            top.Ext.getCmp("GTP_save").hide();
            top.Ext.getCmp("cancelPass").hide();
        }

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
                title: "基本信息",
                layout: 'column',
                items: [
                    {
                        columnWidth: .5,
                        layout: 'form',
                        items: [
                            {
                                name: 'Name',
                                fieldLabel: '配件名称',
                                xtype: 'textfield',
                                id: 'Name',
                                maxLength: 50,
                                maxLengthText: '名称长度不能超过50个字符',
                                anchor: '95%',
                                readOnly: true,
                                enableKeyEvents: true
                            },
                            {
                                name: 'Spec',
                                fieldLabel: '配件规格',
                                xtype: 'textfield',
                                id: 'Spec',
                                flex: 2,
                                readOnly: true,
                                maxLength: 200,
                                maxLengthText: '地址长度不能超过200个字符',
                                anchor: '95%'
                            },
                            {
                                name: 'EName',
                                fieldLabel: '申请人',
                                xtype: 'textfield',
                                id: 'EName',
                                readOnly: true,
                                flex: 2,
                                maxLength: 200,
                                maxLengthText: '地址长度不能超过200个字符',
                                anchor: '95%'
                            }

                        ]
                    },
                    {
                        columnWidth: .5,
                        layout: 'form',
                        items: [
                         {
                             name: 'Model',
                             fieldLabel: '配件型号',
                             xtype: 'textfield',
                             id: 'Model',
                             readOnly: true,
                             flex: 2,
                             anchor: '95%'
                         },
                            {
                                name: 'Num',
                                fieldLabel: '申请数量',
                                xtype: 'textfield',
                                id: 'Num',
                                readOnly: true,
                                flex: 2,
                                maxLength: 50,
                                maxLengthText: '编码长度不能超过50个字符',
                                anchor: '95%'
                            },
                            {
                                name: 'BrandName',
                                fieldLabel: '所属品牌',
                                id: 'BrandName',
                                readOnly: true,
                                xtype: 'textfield',
                                anchor: '95%'
                            }
                        ]
                    },
                    {
                         columnWidth: 1,
                         layout: 'form',
                         autoHeight: true,
                         items: [
                         {
                             fieldLabel: '申请介绍',
                             xtype: 'textarea',
                             id: 'Mark',
                             name: 'Mark',
                             readOnly: true,
                             height: 80,
                             anchor: '98%'
                         }]
                     },
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
            },
            {
                xtype: 'fieldset',
                title: '审核意见',
                autoHeight: true,
                layout: 'column',
                items: [
                {
                    columnWidth: 1,
                    layout: 'form',
                    autoHeight: true,
                    items: [
                        {
                            fieldLabel: '审核意见',
                            xtype: 'textarea',
                            height: 80,
                            id: 'feedback1',
                            name: 'feedback1',
                            allowBlank: false,
                            emptyText: '可输入对申请反馈的审核意见信息', ////textfield自己的属性
                            anchor: '97%',
                            maxLength: 200,
                            maxLengthText: '描述长度不能超过200个字符'
                        }]
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
        height: 545,
        minHeight: 300,
        modal: true,
        layout: 'fit',
        border: false,
        closeAction: 'close',
        items: form,
        buttons: [
          {
              text: '通过',
              iconCls: 'GTP_submit',
              id: 'GTP_save',
              handler: function () {
                  var feedback = top.Ext.getCmp("feedback1").getValue().trim();
                  if (feedback == "") {
                      top.Ext.Msg.show({ title: "信息提示", msg: "审批意见不能为空", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                      return false;
                  }
                  Ext.Ajax.request({
                      url: '/EPartsApply/UpdateIsAudit?feedback=' + encodeURIComponent(feedback),
                      params: { id: key, modfile: 2 },
                      success: function (response) {
                          var rs = eval('(' + response.responseText + ')');
                          if (rs.success) {
                              //判断是否在grid最后一条的时候删除,如果删除,重新加载
                              Ext.getCmp("gg").store.reload();
                              MessageInfo("审核通过！", "right");
                              win.close();
                          } else {
                              MessageInfo("审核失败！", "error");
                          }
                      }
                  });
              }
          },
        {
            text: '未通过',
            id: 'cancelPass',
            handler: function () {
                var feedback = top.Ext.getCmp("feedback1").getValue().trim();
                if (feedback == "") {
                    top.Ext.Msg.show({ title: "信息提示", msg: "审批意见不能为空", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                    return false;
                }
                Ext.Ajax.request({
                    url: '/EPartsApply/UpdateIsAudit?feedback=' + encodeURIComponent(feedback),
                    params: { id: key, modfile: 3 },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            //判断是否在grid最后一条的时候删除,如果删除,重新加载
                            Ext.getCmp("gg").store.reload();
                            MessageInfo("审核未通过！", "right");
                            win.close();
                        } else {
                            MessageInfo("审核失败！", "error");
                        }
                    }
                });
            }
        },
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



