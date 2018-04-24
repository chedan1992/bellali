//点击组织架构
function treeitemclick(node, e) {
    treeNodeId = node.attributes.id;
    Ext.getCmp("gg").getStore().reload();
}
//加载信息表单
function load(title, key) {

    /*
    物业信息
    */
    var comCustomer = new Ext.data.Store({
        proxy: new Ext.data.HttpProxy({
            url: '/ElevatorManage/GetSysCompany',
            method: 'POST'
        }),
        reader: new Ext.data.JsonReader({},
            Ext.data.Record.create(["Id", "Name"]
        ))
    });
    /*
    品牌信息
    */
    var comBrand = new Ext.data.Store({
        proxy: new Ext.data.HttpProxy({
            url: '/ElevatorManage/GetElevatorBrand',
            method: 'POST'
        }),
        reader: new Ext.data.JsonReader({},
            Ext.data.Record.create(["Id", "Name"]
        ))
    });

    /*
    型号信息
    */
    var Model = new Ext.data.Store({
        proxy: new Ext.data.HttpProxy({
            url: '/SysModel/LoadModel',
            method: 'POST'
        }),
        reader: new Ext.data.JsonReader({},
            Ext.data.Record.create(["Id", "ModelName"]
        ))
    });

    if (title == "编辑") {
        var grid = Ext.getCmp("gg");
        //得到选后的数据   
        var rows = grid.getSelectionModel().getSelections();
        key = rows[0].get("Id");

        if (rows[0].get("Brand") != "") {
            Model.proxy = new Ext.data.HttpProxy({ url: '/SysModel/LoadModel?code=' + rows[0].get("Brand"), method: 'POST' });
            Model.load();
        }
    }

    var group = [];
    for (var i = 1; i < 32; i++) {
        group.push([i, i]);
    }

    var form = new top.Ext.FormPanel({
        id: "formPanel",
        labelAlign: 'right',
        frame: true,
        bodyStyle: 'padding:5px 5px 0',
        width: 650,
        labelWidth: 80,
        fileUpload: true,
        autoScroll: true,
        border: false,
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
                            fieldLabel: '<span class="required">*</span>电梯名称',
                            xtype: 'textfield',
                            id: 'Name',
                            maxLength: 50,
                            allowBlank: false,
                            emptyText: '填写电梯名称',
                            maxLengthText: '电梯名称长度不能超过50个字符',
                            anchor: '90%',
                            enableKeyEvents: true,
                            listeners: {
                                'blur': function (val) {
                                    if (val.getValue()) {
                                        var respon = Ext.lib.Ajax.getConnectionObject().conn;
                                        respon.open("post", "/ElevatorManage/SearchNameExits?key=" + key + "&name=" + encodeURIComponent(val.getValue().trim()), false);
                                        respon.send(null);
                                        var response = Ext.util.JSON.decode(respon.responseText);
                                        if (response.success) {
                                            top.Ext.Msg.show({
                                                title: "信息提示",
                                                msg: "电梯名称已经重复,请重新输入！",
                                                buttons: Ext.Msg.OK,
                                                icon: Ext.MessageBox.INFO,
                                                fn: function () {
                                                    top.Ext.getCmp('Name').setValue("");
                                                }
                                            });
                                        }
                                    }
                                }
                            }
                        },
                        {
                            xtype: 'combo',
                            triggerAction: 'all',
                            id: 'comCustomer',
                            name: 'comCustomer',
                            fieldLabel: '<span class="required">*</span>物业单位',
                            emptyText: '物业单位',
                            forceSelection: true,
                            editable: true,
                            typeAhead: true, //模糊查询
                            allowBlank: false,
                            displayField: 'Name',
                            valueField: 'Id',
                            hiddenName: 'Name',
                            anchor: '90%',
                            store: comCustomer,
                            listeners: {
                                beforequery: function (e) {
                                    var combo = e.combo;
                                    if (!e.forceAll) {
                                        var input = e.query;
                                        // 检索的正则  
                                        var regExp = new RegExp(".*" + input + ".*");
                                        // 执行检索  
                                        combo.store.filterBy(function (record, id) {
                                            // 得到每个record的项目名称值  
                                            var text = record.get(combo.displayField);
                                            return regExp.test(text);
                                        });
                                        combo.expand();
                                        return false;
                                    }
                                }
                            }
                        },
                        //                        {
                        //                            name: 'Model',
                        //                            fieldLabel: '电梯型号',
                        //                            xtype: 'textfield',
                        //                            id: 'Model',
                        //                            flex: 2,
                        //                            maxLength: 50,
                        //                            maxLengthText: '型号长度不能超过300个字符',
                        //                            anchor: '90%'
                        //                        },
                        {
                        fieldLabel: '安装日期',
                        xtype: 'datefield',
                        allowBlank: false,
                        value: new Date((new Date() / 1000 + 86400 * 1) * 1000),
                        //minValue: new Date((new Date() / 1000 + 86400 * 1) * 1000),
                        width: 130,
                        id: 'Install',
                        emptyText: '选择开始时间',
                        format: 'Y-m-d(周l)',
                        anchor: '90%'
                    },
                          {
                              name: 'Year_NFC',
                              xtype: 'textfield',
                              id: 'Year_NFC',
                              maxLength: 50,
                              fieldLabel: '年检NFC',
                              maxLengthText: '年检NFC长度不能超过300个字符',
                              anchor: '90%'
                          },
                           {
                               name: 'A_NFC',
                               fieldLabel: 'A_NFC',
                               xtype: 'textfield',
                               fieldLabel: '<span class="required">*</span>A_NFC',
                               id: 'A_NFC',
                               allowBlank: false,
                               anchor: '90%',
                               maxLength: 50,
                               maxLengthText: 'NFC长度不能超过300个字符'
                           },
                    //                            {
                    //                                xtype: 'compositefield',
                    //                                fieldLabel: '<span class="required">*</span>A_NFC号',
                    //                                combineErrors: false,
                    //                                anchor: '100%',
                    //                                items: [
                    //                                    {
                    //                                        xtype: 'combo',
                    //                                        triggerAction: 'all',
                    //                                        emptyText: '选择号',
                    //                                        forceSelection: true,
                    //                                        allowBlank: false,
                    //                                        displayField: 'text',
                    //                                        valueField: 'value',
                    //                                        mode: 'local',
                    //                                        id: 'A_NFCTime',
                    //                                        name: 'A_NFCTime',
                    //                                        editable: false,
                    //                                        width:95,
                    //                                        store: new Ext.data.SimpleStore({
                    //                                            fields: ['text', 'value'],
                    //                                            data: group
                    //                                        })
                    //                                    },
                    //                                    {
                    //                                        xtype: 'combo',
                    //                                        triggerAction: 'all',
                    //                                        emptyText: '单双',
                    //                                        forceSelection: true,
                    //                                        displayField: 'text',
                    //                                        valueField: 'value',
                    //                                        mode: 'local',
                    //                                        id: 'ComA_Choise',
                    //                                        editable: false,
                    //                                        width: 95,
                    //                                        store: new Ext.data.SimpleStore({
                    //                                            fields: ['text', 'value'],
                    //                                            data: [['单月', '0'], ['双月', '1']]
                    //                                        }),
                    //                                        value: '0'
                    //                                    }
                    //                                ]
                    //                                },
                             {
                             name: 'C_NFC',
                             fieldLabel: 'C_NFC',
                             xtype: 'textfield',
                             id: 'C_NFC',
                             fieldLabel: '<span class="required">*</span>C_NFC',
                             anchor: '90%',
                             allowBlank: false,
                             maxLength: 50,
                             maxLengthText: 'NFC长度不能超过300个字符'
                         },
                          {
                              fieldLabel: '开始时间',
                              xtype: 'datefield',
                              allowBlank: false,
                              value: new Date((new Date() / 1000 + 86400 * 1) * 1000),
                              minValue: new Date((new Date() / 1000 + 86400 * 1) * 1000),
                              width: 130,
                              id: 'ComStartTime',
                              emptyText: '选择开始时间',
                              format: 'Y-m-d(周l)',
                              anchor: '90%'
                          },
                              {
                                  xtype: 'combo',
                                  triggerAction: 'all',
                                  emptyText: '维修类型',
                                  forceSelection: true,
                                  allowBlank: false,
                                  displayField: 'text',
                                  fieldLabel: '维修类型',
                                  valueField: 'value',
                                  id: 'ComStartKey',
                                  mode: 'local',
                                  editable: false,
                                  anchor: '90%',
                                  store: new Ext.data.SimpleStore({
                                      fields: ['text', 'value'],
                                      data: [['A项', 'A'], ['B项', 'B'], ['C项', 'C'], ['D项', 'D']]
                                  }),
                                  value: 'A'
                              }

                        ]
                },
                    {
                        columnWidth: .5,
                        layout: 'form',
                        items: [
                                  {
                                      name: 'Code',
                                      fieldLabel: '<span class="required">*</span>电梯编号',
                                      xtype: 'textfield',
                                      id: 'Code',
                                      maxLength: 50,
                                      emptyText: '唯一编号',
                                      allowBlank: false,
                                      maxLengthText: '编号长度不能超过50个字符',
                                      anchor: '90%',
                                      enableKeyEvents: true,
                                      listeners: {
                                          'blur': function (val) {
                                              if (val.getValue()) {
                                                  var respon = Ext.lib.Ajax.getConnectionObject().conn;
                                                  respon.open("post", "/ElevatorManage/SearchCodeExits?key=" + key + "&code=" + encodeURIComponent(val.getValue().trim()), false);
                                                  respon.send(null);
                                                  var response = Ext.util.JSON.decode(respon.responseText);
                                                  if (response.success) {
                                                      top.Ext.Msg.show({
                                                          title: "信息提示",
                                                          msg: "编码已经重复,请重新输入！",
                                                          buttons: Ext.Msg.OK,
                                                          icon: Ext.MessageBox.INFO,
                                                          fn: function () {
                                                              top.Ext.getCmp('Code').setValue("");
                                                          }
                                                      });
                                                  }
                                              }
                                          }
                                      }
                                  },
                                   {
                                       xtype: 'combo',
                                       id: 'comBrand',
                                       name: 'comBrand',
                                       fieldLabel: '<span class="required">*</span>所属品牌',
                                       emptyText: '所属品牌',
                                       loadingText: '加载中...',
                                       selectOnFocus: true,
                                       triggerAction: 'all',
                                       forceSelection: true,
                                       typeAhead: true, //模糊查询
                                       editable: true,
                                       allowBlank: false,
                                       displayField: 'Name',
                                       valueField: 'Id',
                                       hiddenName: 'Name',
                                       anchor: '90%',
                                       store: comBrand,
                                       listeners: {
                                           select: {
                                               fn: function (combo, record, index) {
                                                   Model.proxy = new Ext.data.HttpProxy({ url: '/SysModel/LoadModel?code=' + record.get('Id'), method: 'POST' });
                                                   Model.load();
                                                   top.Ext.getCmp("comModel").setValue(""); //value
                                                   top.Ext.getCmp("comModel").setRawValue(""); //text 
                                               }
                                           },
                                           beforequery: function (e) {
                                               var combo = e.combo;
                                               if (!e.forceAll) {
                                                   var input = e.query;
                                                   // 检索的正则  
                                                   var regExp = new RegExp(".*" + input + ".*");
                                                   // 执行检索  
                                                   combo.store.filterBy(function (record, id) {
                                                       // 得到每个record的项目名称值  
                                                       var text = record.get(combo.displayField);
                                                       return regExp.test(text);
                                                   });
                                                   combo.expand();
                                                   return false;
                                               }
                                           }
                                       }
                                   },
                                    {
                                        xtype: 'combo',
                                        triggerAction: 'all',
                                        id: 'comModel',
                                        name: 'comModel',
                                        fieldLabel: '<span class="required">*</span>所属型号',
                                        emptyText: '所属型号',
                                        forceSelection: true,
                                        typeAhead: true, //模糊查询
                                        allowBlank: false,
                                        displayField: 'ModelName',
                                        valueField: 'Id',
                                        hiddenName: 'ModelName',
                                        anchor: '90%',
                                        store: Model,
                                        listeners: {
                                            beforequery: function (e) {
                                                var combo = e.combo;
                                                if (!e.forceAll) {
                                                    var input = e.query;
                                                    // 检索的正则  
                                                    var regExp = new RegExp(".*" + input + ".*");
                                                    // 执行检索  
                                                    combo.store.filterBy(function (record, id) {
                                                        // 得到每个record的项目名称值  
                                                        var text = record.get(combo.displayField);
                                                        return regExp.test(text);
                                                    });
                                                    combo.expand();
                                                    return false;
                                                }
                                            }
                                        }
                                    },

                                   {
                                       xtype: 'combo',
                                       triggerAction: 'all',
                                       emptyText: '选择类型',
                                       forceSelection: true,
                                       displayField: 'text',
                                       fieldLabel: '电梯类型',
                                       valueField: 'value',
                                       id: 'comElevatorType',
                                       mode: 'local',
                                       editable: false,
                                       anchor: '90%',
                                       store: new Ext.data.SimpleStore({
                                           fields: ['text', 'value'],
                                           data: [['有机房乘客电梯', '1'], ['无机房乘客电梯', '2'], ['有机房货梯', '3'], ['无机房货梯', '4'], ['有机房观光电梯', '5'], ['无机房观光电梯', '6'], ['有机房医用电梯', '7'], ['无机房医用电梯', '8'], ['别墅电梯', '9'], ['汽车电梯', '10'], ['自动扶梯', '11'], ['自动人行道', '12'], ['立体停车库', '13']]
                                       }),
                                       value: '1'
                                   },
                                      {
                                          name: 'B_NFC',
                                          fieldLabel: 'B_NFC',
                                          xtype: 'textfield',
                                          id: 'B_NFC',
                                          fieldLabel: '<span class="required">*</span>B_NFC',
                                          allowBlank: false,
                                          anchor: '90%',
                                          maxLength: 50,
                                          maxLengthText: 'NFC长度不能超过300个字符'
                                      },
                                     {
                                     name: 'D_NFC',
                                     fieldLabel: 'D_NFC',
                                     xtype: 'textfield',
                                     id: 'D_NFC',
                                     fieldLabel: '<span class="required">*</span>D_NFC',
                                     allowBlank: false,
                                     anchor: '90%',
                                     maxLength: 50,
                                     maxLengthText: 'NFC长度不能超过300个字符'
                                 },
                                  {
                                      xtype: 'compositefield',
                                      fieldLabel: '<span class="required">*</span>间隔时间',
                                      combineErrors: false,
                                      anchor: '90%',
                                      items: [
                                               {
                                                   xtype: 'combo',
                                                   triggerAction: 'all',
                                                   emptyText: '巡检间隔时间',
                                                   fieldLabel: '',
                                                   forceSelection: true,
                                                   allowBlank: false,
                                                   displayField: 'text',
                                                   valueField: 'value',
                                                   mode: 'local',
                                                   id: 'IntervalDay',
                                                   editable: false,
                                                   width:170,
                                                   store: new Ext.data.SimpleStore({
                                                       fields: ['text', 'value'],
                                                       data: group
                                                   }),
                                                   value: '1'
                                               },
                                                 {
                                                      xtype: 'tbtext',
                                                      style: 'margin-top:5px;margin-left:-8px;',
                                                      text: '（天）'
                                                  }
                                       ]
                                  }
                                   
                        //                                    {
                        //                                        xtype: 'compositefield',
                        //                                        fieldLabel: '<span class="required">*</span>D_NFC号',
                        //                                        combineErrors: false,
                        //                                        anchor: '100%',
                        //                                        items: [
                        //                                  
                        //                                    {
                        //                                        xtype: 'combo',
                        //                                        triggerAction: 'all',
                        //                                        emptyText: '选择号',
                        //                                        forceSelection: true,
                        //                                        allowBlank: false,
                        //                                        displayField: 'text',
                        //                                        valueField: 'value',
                        //                                        mode: 'local',
                        //                                        id: 'D_NFCTime',
                        //                                        name: 'D_NFCTime',
                        //                                        editable: false,
                        //                                        width: 95,
                        //                                        store: new Ext.data.SimpleStore({
                        //                                            fields: ['text', 'value'],
                        //                                            data: group
                        //                                        })
                        //                                        
                        //                                    },
                        //                                    {
                        //                                        xtype: 'combo',
                        //                                        triggerAction: 'all',
                        //                                        emptyText: '单双',
                        //                                        forceSelection: true,
                        //                                        displayField: 'text',
                        //                                        valueField: 'value',
                        //                                        mode: 'local',
                        //                                        id: 'ComD_Choise',
                        //                                        editable: false,
                        //                                        width: 95,
                        //                                        store: new Ext.data.SimpleStore({
                        //                                            fields: ['text', 'value'],
                        //                                            data: [['单月', '0'], ['双月', '1']]
                        //                                        }),
                        //                                        value: '1'
                        //                                    }
                        //                                ]
                        //                                    }
                           ]
                    },
                    {
                        columnWidth: 1,
                        layout: 'form',
                        items: [
                         {
                             fieldLabel: '电梯介绍',
                             xtype: 'htmleditor',
                             id: 'Content',
                             name: 'Content',
                             height: 150,
                             emptyText: '可输入对电梯的介绍信息', ////textfield自己的属性
                             anchor: '96%'
                         }
                        ]
                    }
                ]
            }
        ]
    });

    //窗体
    var win = Window("window", title, form);
    win.width = 700;
    win.height = 520;
    return win;
}


/**************************方法************************************/

//启用
function EnableUse() {
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        if (rows[0].get("Status") == "1") {
            MessageInfo("该电梯已经启用！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要启用该电梯吗?', function (e) {
            if (e == "yes") {
                var key = rows[0].get("Id");
                Ext.Ajax.request({
                    url: '/ElevatorManage/EnableUse',
                    params: { id: key },
                    success: function (response) {
                        Ext.getCmp("gg").store.reload();
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
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        if (rows[0].get("Status") == "0") {
            MessageInfo("该电梯已经禁用！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要禁用该电梯吗?', function (e) {
            if (e == "yes") {
                var key = rows[0].get("Id");
                Ext.Ajax.request({
                    url: '/ElevatorManage/DisableUse',
                    params: { id: key },
                    success: function (response) {
                        Ext.getCmp("gg").store.reload();
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
    result = [];
    parent.idArray = [];
    url = '/ElevatorManage/SaveData?1=1';
    var win = load("新增", "");
    win.show();
}
//编辑
function EditDate() {
    result = [];
    parent.idArray = [];
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();

    if (rows.length == 1) {
        var key = rows[0].get("Id");
        var win = load("编辑", key);
        var form = top.Ext.getCmp('formPanel');
        win.show();

        var respon = Ext.lib.Ajax.getConnectionObject().conn;
        respon.open("post", "/ElevatorManage/GetData?id=" + key, false);
        respon.send(null);
        var response = Ext.util.JSON.decode(respon.responseText);

        form.form.loadRecord(response);
        //物业
        top.Ext.getCmp("comCustomer").setValue(response.data.CustomerId); //value
        top.Ext.getCmp("comCustomer").setRawValue(response.data.CustomerName); //text
        //品牌
        top.Ext.getCmp("comBrand").setValue(response.data.Brand); //value
        top.Ext.getCmp("comBrand").setRawValue(response.data.BrandName); //text
        //型号
        top.Ext.getCmp("comModel").setValue(response.data.Model); //value
        top.Ext.getCmp("comModel").setRawValue(response.data.ModelName); //text

        //格式化时间
        var InstallTime = new Date(formartTime(rows[0].data.InstallTime).format('Y-m-d'));
        if (InstallTime == "NaN") {
            InstallTime = formartTime(rows[0].data.InstallTime).format('Y-m-d');
        }
        top.Ext.getCmp("Install").setValue(InstallTime);

        var StartTime = new Date(formartTime(rows[0].data.StartTime).format('Y-m-d'));
        if (StartTime == "NaN") {
            StartTime = formartTime(rows[0].data.StartTime).format('Y-m-d');
        }
        top.Ext.getCmp("ComStartTime").setValue(StartTime);

        //设置电梯类型
        switch (parseInt(response.data.ElevatorType)) {
            case 1:
                top.Ext.getCmp("comElevatorType").setValue("1", "有机房乘客电梯");
                break;
            case 2:
                top.Ext.getCmp("comElevatorType").setValue("2", "无机房乘客电梯");
                break;
            case 3:
                top.Ext.getCmp("comElevatorType").setValue("3", "有机房货梯");
                break;
            case 4:
                top.Ext.getCmp("comElevatorType").setValue("4", "无机房货梯");
                break;
            case 5:
                top.Ext.getCmp("comElevatorType").setValue("5", "有机房观光电梯");
                break;
            case 6:
                top.Ext.getCmp("comElevatorType").setValue("6", "无机房观光电梯");
                break;
            case 7:
                top.Ext.getCmp("comElevatorType").setValue("7", "有机房医用电梯");
                break;
            case 8:
                top.Ext.getCmp("comElevatorType").setValue("8", "无机房医用电梯");
                break;
            case 9:
                top.Ext.getCmp("comElevatorType").setValue("9", "别墅电梯");
                break;
            case 10:
                top.Ext.getCmp("comElevatorType").setValue("10", "汽车电梯");
                break;
            case 11:
                top.Ext.getCmp("comElevatorType").setValue("11", "自动扶梯");
                break;
            case 12:
                top.Ext.getCmp("comElevatorType").setValue("12", "自动人行道");
                break;
            case 13:
                top.Ext.getCmp("comElevatorType").setValue("13", "立体停车库");
                break;
        }

        switch (response.data.StartKey) {
            case "A":
                top.Ext.getCmp("ComStartKey").setValue("A", "A项");
                break;
            case "B":
                top.Ext.getCmp("ComStartKey").setValue("B", "B项");
                break;
            case "C":
                top.Ext.getCmp("ComStartKey").setValue("C", "C项");
                break;
            case "D":
                top.Ext.getCmp("ComStartKey").setValue("D", "D项");
                break;
        }
//        if (response.data.StartKey == "A") {
//            top.Ext.getCmp("ComStartKey").setValue("A", "A项");
//        }
//        else {
//            top.Ext.getCmp("ComA_Choise").setValue("1", "双月");
//        }

//        if (parseInt(response.data.B_Choise) == 0) {
//            top.Ext.getCmp("ComB_Choise").setValue("0", "单月");
//        }
//        else {
//            top.Ext.getCmp("ComB_Choise").setValue("1", "双月");
//        }

//        if (parseInt(response.data.C_Choise) == 0) {
//            top.Ext.getCmp("ComC_Choise").setValue("0", "单月");
//        }
//        else {
//            top.Ext.getCmp("ComC_Choise").setValue("1", "双月");
//        }

//        if (parseInt(response.data.D_Choise) == 0) {
//            top.Ext.getCmp("ComD_Choise").setValue("0", "单月");
//        }
//        else {
//            top.Ext.getCmp("ComD_Choise").setValue("1", "双月");
//        }


        win.show();

        url = '/ElevatorManage/SaveData?Id=' + key + '&modify=1';
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
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要删除电梯吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push("'" + rows[i].data["Id"] + "'");
                }
                Ext.Ajax.request({
                    url: '/ElevatorManage/DeleteData',
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
//保存
var SaveDate = function () {
    var grid = Ext.getCmp("gg");
    var formPanel = top.Ext.getCmp("formPanel");
    var win = top.Ext.getCmp("window");
    if (formPanel.getForm().isValid()) {//如果验证通过

        //获取物业和品牌和型号
        var comCustomer = top.Ext.getCmp("comCustomer").getValue();
        var comBrand = top.Ext.getCmp("comBrand").getValue();
        var comModel = top.Ext.getCmp("comModel").getValue();

        //电梯类型
        var comElevatorType = top.Ext.getCmp("comElevatorType").getValue();

        //单双月
//        var ComA_Choise = top.Ext.getCmp("ComA_Choise").getValue();
//        var ComB_Choise = top.Ext.getCmp("ComB_Choise").getValue();
//        var ComC_Choise = top.Ext.getCmp("ComC_Choise").getValue();
//        var ComD_Choise = top.Ext.getCmp("ComD_Choise").getValue();

        var ComStartKey = top.Ext.getCmp("ComStartKey").getValue();
        var Time = top.Ext.getCmp("Install").getValue().format('Y-m-d');
        var startvalue = top.Ext.getCmp("ComStartTime").getValue().format('Y-m-d');

        //获取
        var para = { ModelId: comModel, Brand: comBrand, CustomerId: comCustomer, ElevatorType: comElevatorType, InstallTime: Time, StartTime: startvalue, StartKey: ComStartKey };
        if (win.title == "编辑") {
            //得到选后的数据   
            var rows = grid.getSelectionModel().getSelections();
            para = { Id: rows[0].get("Id"), ModelId: comModel, Brand: comBrand, CustomerId: comCustomer, ElevatorType: comElevatorType, InstallTime: Time, StartTime: startvalue, StartKey: ComStartKey };
        }
        formPanel.getForm().submit({
            waitTitle: '系统提示', //标题
            waitMsg: '正在提交数据,请稍后...', //提示信息
            submitEmptyText: false,
            method: "POST",
            url: url,
            params: para,
            success: function (form, action) {
                //成功后
                var flag = action.result;
                if (flag.success) {
                    MessageInfo("保存成功！", "right");
                    win.close();
                    Ext.getCmp("gg").store.reload();
                } else {
                    MessageInfo(flag.msg, "error");
                }
            },
            failure: function (form, action) {
                MessageInfo("保存失败！", "error");
            }
        });
    }
};

//查看电梯维护情况
function clickDetail() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].data["Id"];
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }


}

//同步
function GTPSynchronization() {
    var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要同步T+最新商品信息吗?', function (e) {
        if (e == "yes") {
            var success = false;
            top.Ext.MessageBox.show({
                title: '请等待',
                msg: '系统正在同步...',
                progressText: 'Initializing...',
                width: 300,
                progress: true,
                closable: false
            });
            var f = function (v) {
                return function () {
                    if (v !=100) {
                        var i = v /100;
                        top.Ext.MessageBox.updateProgress(i, Math.round(100 * i) + '% 已完成');
                    }
                };
            };
            for (var i = 1; i <100; i++) {
                setTimeout(f(i), i * 200);
            }
            Ext.Ajax.request({
                url: '/ShopGoods/GTPSynchronization',
                success: function (response) {
                    var rs = eval('(' + response.responseText + ')');
                    if (rs.success) {
                        top.Ext.MessageBox.hide();
                        Ext.getCmp("gg").store.reload();
                        if (rs.msg == "0") {
                            top.Ext.Msg.show({ title: "信息提示", msg: "同步成功,没有要更新的记录.", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                        }
                        else {
                            top.Ext.Msg.show({ title: "信息提示", msg: "同步成功,更新记录条数:" + rs.msg + "条", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                        }
                    } else {
                        MessageInfo("同步失败！", "error");
                    }
                }
            });
        }
    });
}



























