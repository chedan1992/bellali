//切换面板
function changePanel(value) {
    switch (parseInt(value)) {
        case 1:
            Ext.getCmp("Type1").show();
            Ext.getCmp("Type2").hide();
            Ext.getCmp("Type3").hide();
            break;
        case 2:
            Ext.getCmp("Type2").show();
            Ext.getCmp("Type1").hide();
            Ext.getCmp("Type3").hide();
            break;
        case 3:
            Ext.getCmp("Type3").show();
            Ext.getCmp("Type1").hide();
            Ext.getCmp("Type2").hide();
            break;
    }
}

//点击组织架 构
function treeitemclick(node, e) {
    treeNodeId = node.attributes.id;
    ChangeModel(treeNodeId);
    Ext.getCmp("comModel").setValue(treeNodeId);
    Ext.getCmp("comModel").setRawValue(node.attributes.text);
}
//切换岗位
function ChangeModel(key) {
    var form = Ext.getCmp('formPanel');
    //var key = record.get('id');
    var respon = Ext.lib.Ajax.getConnectionObject().conn;
    respon.open("post", "/DeptSet/GetData?id=" + key + "&SetType=1", false); //配置类型(1:岗位配置 2:特殊人员配置)
    respon.send(null);
    var response = Ext.util.JSON.decode(respon.responseText);
    if (response != null) {
        form.form.loadRecord(response);
        // 创建Map实例
        var mPoint = new BMap.Point(response.ComPLon, response.CompLat);
        map.enableScrollWheelZoom();
        map.centerAndZoom(mPoint, 15);
        var tip = "无范围限制签到";
        if (response.Limit > 0) {
            Ext.getCmp("Limit").setValue(response.Limit);
            tip = "范围" + response.Limit + "米内可正常签到";
            var circle = new BMap.Circle(mPoint, response.Limit, { fillColor: "Red", strokeWeight: 1, fillOpacity: 0.3, strokeOpacity: 0.3 });
            map.addOverlay(circle);
        }
        else {
            Ext.getCmp("ComSignType").setValue(1);
            Ext.getCmp("Limit").setValue(0);
        }

        var marker = new BMap.Marker(mPoint);    // 创建标注
        var station = new BMap.Point(response.ComPLon, response.CompLat);
        marker = new BMap.Marker(station);
        map.addOverlay(marker);
        var infoWindow = new BMap.InfoWindow("<p style='font-size:12px;'>您的坐标:(" + response.ComPLon + "," + response.CompLat + ")</p><p style='font-size:12px;'>地址:" + response.Address + "</p><br/><p style='font-size:11px;color:orange'>" + tip + "。</p>");
        map.openInfoWindow(infoWindow, mPoint); //开启信息窗口
        //岗位还未配置
        changePanel(response.SignType); //切换面板

        if (response.DeptSetId == "" || response.DeptSetId == null) {
            url = "/DeptSet/SaveData?1=1";
        }
        else {
            url = '/DeptSet/SaveData?Id=' + response.DeptSetId + '&modify=1';

            Ext.getCmp("ComSignType").setValue(response.SignType);
        }
        Ext.getCmp("ComPLon").setValue(response.ComPLon);
        Ext.getCmp("CompLat").setValue(response.CompLat);
        Ext.getCmp("Address").setValue(response.Address);
        //绑定配置时间
        var list = response.listSet;
        if (list.length > 0) {
            switch (parseInt(response.SignType)) {
                case 1:
                    Ext.getCmp("Type1Id").setValue(list[0].Id);
                    Ext.getCmp("Type1StartTimeBeginAM").setValue(new Date(formartTime(list[0].TimeBeginAM)).format('H:i'));
                    Ext.getCmp("Type1StartTimeEndAM").setValue(new Date(formartTime(list[0].TimeEndAM)).format('H:i'));
                    Ext.getCmp("Type1StartTimeAM").setValue(new Date(formartTime(list[0].TimeAM)).format('H:i'));
                    Ext.getCmp("Type1StartTimeBeginBM").setValue(new Date(formartTime(list[0].TimeBeginBM)).format('H:i'));
                    Ext.getCmp("Type1StartTimeEndBM").setValue(new Date(formartTime(list[0].TimeEndBM)).format('H:i'));
                    Ext.getCmp("Type1StartTimeBM").setValue(new Date(formartTime(list[0].TimeBM)).format('H:i'));
                    break;
                case 2:
                    Ext.getCmp("1Type2Id").setValue(list[0].Id);
                    Ext.getCmp("1Type2StartTimeBeginAM").setValue(new Date(formartTime(list[0].TimeBeginAM)).format('H:i'));
                    Ext.getCmp("1Type2StartTimeEndAM").setValue(new Date(formartTime(list[0].TimeEndAM)).format('H:i'));
                    Ext.getCmp("1Type2StartTimeAM").setValue(new Date(formartTime(list[0].TimeAM)).format('H:i'));
                    Ext.getCmp("1Type2StartTimeBeginBM").setValue(new Date(formartTime(list[0].TimeBeginBM)).format('H:i'));
                    Ext.getCmp("1Type2StartTimeEndBM").setValue(new Date(formartTime(list[0].TimeEndBM)).format('H:i'));
                    Ext.getCmp("1Type2StartTimeBM").setValue(new Date(formartTime(list[0].TimeBM)).format('H:i'));

                    Ext.getCmp("2Type2Id").setValue(list[1].Id);
                    Ext.getCmp("2Type2StartTimeBeginAM").setValue(new Date(formartTime(list[1].TimeBeginAM)).format('H:i'));
                    Ext.getCmp("2Type2StartTimeEndAM").setValue(new Date(formartTime(list[1].TimeEndAM)).format('H:i'));
                    Ext.getCmp("2Type2StartTimeAM").setValue(new Date(formartTime(list[1].TimeAM)).format('H:i'));
                    Ext.getCmp("2Type2StartTimeBeginBM").setValue(new Date(formartTime(list[1].TimeBeginBM)).format('H:i'));
                    Ext.getCmp("2Type2StartTimeEndBM").setValue(new Date(formartTime(list[1].TimeEndBM)).format('H:i'));
                    Ext.getCmp("2Type2StartTimeBM").setValue(new Date(formartTime(list[1].TimeBM)).format('H:i'));
                    break;
                case 3:
                    Ext.getCmp("1Type3Id").setValue(list[0].Id);
                    Ext.getCmp("1Type3StartTimeBeginAM").setValue(new Date(formartTime(list[0].TimeBeginAM)).format('H:i'));
                    Ext.getCmp("1Type3StartTimeEndAM").setValue(new Date(formartTime(list[0].TimeEndAM)).format('H:i'));
                    Ext.getCmp("1Type3StartTimeAM").setValue(new Date(formartTime(list[0].TimeAM)).format('H:i'));
                    Ext.getCmp("1Type3StartTimeBeginBM").setValue(new Date(formartTime(list[0].TimeBeginBM)).format('H:i'));
                    Ext.getCmp("1Type3StartTimeEndBM").setValue(new Date(formartTime(list[0].TimeEndBM)).format('H:i'));
                    Ext.getCmp("1Type3StartTimeBM").setValue(new Date(formartTime(list[0].TimeBM)).format('H:i'));

                    Ext.getCmp("2Type3Id").setValue(list[1].Id);
                    Ext.getCmp("2Type3StartTimeBeginAM").setValue(new Date(formartTime(list[1].TimeBeginAM)).format('H:i'));
                    Ext.getCmp("2Type3StartTimeEndAM").setValue(new Date(formartTime(list[1].TimeEndAM)).format('H:i'));
                    Ext.getCmp("2Type3StartTimeAM").setValue(new Date(formartTime(list[1].TimeAM)).format('H:i'));
                    Ext.getCmp("2Type3StartTimeBeginBM").setValue(new Date(formartTime(list[1].TimeBeginBM)).format('H:i'));
                    Ext.getCmp("2Type3StartTimeEndBM").setValue(new Date(formartTime(list[1].TimeEndBM)).format('H:i'));
                    Ext.getCmp("2Type3StartTimeBM").setValue(new Date(formartTime(list[1].TimeBM)).format('H:i'));

                    Ext.getCmp("3Type3Id").setValue(list[2].Id);
                    Ext.getCmp("3Type3StartTimeBeginAM").setValue(new Date(formartTime(list[2].TimeBeginAM)).format('H:i'));
                    Ext.getCmp("3Type3StartTimeEndAM").setValue(new Date(formartTime(list[2].TimeEndAM)).format('H:i'));
                    Ext.getCmp("3Type3StartTimeAM").setValue(new Date(formartTime(list[2].TimeAM)).format('H:i'));
                    Ext.getCmp("3Type3StartTimeBeginBM").setValue(new Date(formartTime(list[2].TimeBeginBM)).format('H:i'));
                    Ext.getCmp("3Type3StartTimeEndBM").setValue(new Date(formartTime(list[2].TimeEndBM)).format('H:i'));
                    Ext.getCmp("3Type3StartTimeBM").setValue(new Date(formartTime(list[2].TimeBM)).format('H:i'));
                    break;
            }

        }
    }
}
//格式化 启用 or 禁用
function formartEnableOrDisable(val, row) {
    if (val == 1)
        return "<span style='color:green'>启用</span>";
    else if (val == 0) {
        return "<span style='color:Red'>禁用</span>";
    }
    else if (val == -1) {
        return "<span style='color:silver'>删除</span>";
    }
    else if (val == -2) {
        return "<span style='color:999999'>待审核</span>";
    } else {
        return "<span style='color:#999999'>已过期</span>";
    }
}
//保存
function SaveDate() {
    var formPanel =Ext.getCmp("formPanel");
    if (formPanel.getForm().isValid()) {//如果验证通过
        //获取岗位类型
        var comModel = Ext.getCmp("comModel").getValue();
        var LimitNum = Ext.getCmp("Limit").getValue(); //签到范围
        var SignTypeNum = Ext.getCmp("ComSignType").getValue(); 
        var TestStrategy = [];
        switch (parseInt(SignTypeNum)) {
            case 1: //类型1
                var Type1StartTimeBeginAM = Ext.getCmp("Type1StartTimeBeginAM").getValue();
                var Type1StartTimeEndAM = Ext.getCmp("Type1StartTimeEndAM").getValue();
                var Type1StartTimeAM = Ext.getCmp("Type1StartTimeAM").getValue();
                var Type1StartTimeBeginBM = Ext.getCmp("Type1StartTimeBeginBM").getValue();
                var Type1StartTimeEndBM = Ext.getCmp("Type1StartTimeEndBM").getValue();
                var Type1StartTimeBM = Ext.getCmp("Type1StartTimeBM").getValue();
                //SetType配置类型(1:岗位配置 2:特殊人员配置)
                //SignType签到类型(1,2,3)
                //ShiftNum班次
                var result = {
                    Id: Ext.getCmp("Type1Id").getValue(),
                    LinkId: comModel,
                    Limit: LimitNum,
                    SignType: 1,
                    SetType: 1,
                    Title: '早晚班(1班次)',
                    ShiftNum: 1,//班次
                    TimeBeginAM: Type1StartTimeBeginAM,
                    TimeEndAM: Type1StartTimeEndAM,
                    TimeAM: Type1StartTimeAM,
                    TimeBeginBM: Type1StartTimeBeginBM,
                    TimeEndBM: Type1StartTimeEndBM,
                    TimeBM: Type1StartTimeBM,
                    Sort:0
                 }
                TestStrategy.push(result);
                break;
            case 2: //类型2
                var Type2StartTimeBeginAM1 = Ext.getCmp("1Type2StartTimeBeginAM").getValue();
                var Type2StartTimeEndAM1 = Ext.getCmp("1Type2StartTimeEndAM").getValue();
                var Type2StartTimeAM1 = Ext.getCmp("1Type2StartTimeAM").getValue();
                var Type2StartTimeBeginBM1 = Ext.getCmp("1Type2StartTimeBeginBM").getValue();
                var Type2StartTimeEndBM1 = Ext.getCmp("1Type2StartTimeEndBM").getValue();
                var Type2StartTimeBM1 = Ext.getCmp("1Type2StartTimeBM").getValue();
                //SetType配置类型(1:岗位配置 2:特殊人员配置)
                //SignType签到类型(1,2,3)
                //ShiftNum班次
                var result = {
                    Id: Ext.getCmp("1Type2Id").getValue(),
                    LinkId: comModel,
                    Limit: LimitNum,
                    SignType:2,
                    SetType:1,
                    Title: '早班',
                    ShiftNum: 1, //班次
                    TimeBeginAM: Type2StartTimeBeginAM1,
                    TimeEndAM: Type2StartTimeEndAM1,
                    TimeAM: Type2StartTimeAM1,
                    TimeBeginBM: Type2StartTimeBeginBM1,
                    TimeEndBM: Type2StartTimeEndBM1,
                    TimeBM: Type2StartTimeBM1,
                    Sort: 0
                }
                TestStrategy.push(result);

                var Type2StartTimeBeginAM2 = Ext.getCmp("2Type2StartTimeBeginAM").getValue();
                var Type2StartTimeEndAM2 = Ext.getCmp("2Type2StartTimeEndAM").getValue();
                var Type2StartTimeAM2 = Ext.getCmp("2Type2StartTimeAM").getValue();
                var Type2StartTimeBeginBM2 = Ext.getCmp("2Type2StartTimeBeginBM").getValue();
                var Type2StartTimeEndBM2 = Ext.getCmp("2Type2StartTimeEndBM").getValue();
                var Type2StartTimeBM2 = Ext.getCmp("2Type2StartTimeBM").getValue();
                //SetType配置类型(1:岗位配置 2:特殊人员配置)
                //SignType签到类型(1,2,3)
                //ShiftNum班次
                var result = {
                    Id: Ext.getCmp("2Type2Id").getValue(),
                    LinkId: comModel,
                    Limit: LimitNum,
                    SignType:2,
                    SetType:1,
                    Title: '晚班',
                    ShiftNum:2, //班次
                    TimeBeginAM: Type2StartTimeBeginAM2,
                    TimeEndAM: Type2StartTimeEndAM2,
                    TimeAM: Type2StartTimeAM2,
                    TimeBeginBM: Type2StartTimeBeginBM2,
                    TimeEndBM: Type2StartTimeEndBM2,
                    TimeBM: Type2StartTimeBM2,
                    Sort:1
                }
                TestStrategy.push(result);

                break;
            case 3: //类型3
                var Type3StartTimeBeginAM1 = Ext.getCmp("1Type3StartTimeBeginAM").getValue();
                var Type3StartTimeEndAM1 = Ext.getCmp("1Type3StartTimeEndAM").getValue();
                var Type3StartTimeAM1 = Ext.getCmp("1Type3StartTimeAM").getValue();
                var Type3StartTimeBeginBM1 = Ext.getCmp("1Type3StartTimeBeginBM").getValue();
                var Type3StartTimeEndBM1 = Ext.getCmp("1Type3StartTimeEndBM").getValue();
                var Type3StartTimeBM1 = Ext.getCmp("1Type3StartTimeBM").getValue();
                //SetType配置类型(1:岗位配置 2:特殊人员配置)
                //SignType签到类型(1,2,3)
                //ShiftNum班次
                var result = {
                    Id: Ext.getCmp("1Type3Id").getValue(),
                    LinkId: comModel,
                    Limit: LimitNum,
                    SignType:3,
                    SetType:1,
                    Title: '早班',
                    ShiftNum: 1, //班次
                    TimeBeginAM: Type3StartTimeBeginAM1,
                    TimeEndAM: Type3StartTimeEndAM1,
                    TimeAM: Type3StartTimeAM1,
                    TimeBeginBM: Type3StartTimeBeginBM1,
                    TimeEndBM: Type3StartTimeEndBM1,
                    TimeBM: Type3StartTimeBM1,
                    Sort: 0
                }
                TestStrategy.push(result);

                var Type3StartTimeBeginAM2 = Ext.getCmp("2Type3StartTimeBeginAM").getValue();
                var Type3StartTimeEndAM2 = Ext.getCmp("2Type3StartTimeEndAM").getValue();
                var Type3StartTimeAM2 = Ext.getCmp("2Type3StartTimeAM").getValue();
                var Type3StartTimeBeginBM2 = Ext.getCmp("2Type3StartTimeBeginBM").getValue();
                var Type3StartTimeEndBM2 = Ext.getCmp("2Type3StartTimeEndBM").getValue();
                var Type3StartTimeBM2 = Ext.getCmp("2Type3StartTimeBM").getValue();
                //SetType配置类型(1:岗位配置 2:特殊人员配置)
                //SignType签到类型(1,2,3)
                //ShiftNum班次
                var result = {
                    Id: Ext.getCmp("2Type3Id").getValue(),
                    LinkId: comModel,
                    Limit: LimitNum,
                    SignType:3,
                    SetType:1,
                    Title: '中班',
                    ShiftNum: 2, //班次
                    TimeBeginAM: Type3StartTimeBeginAM2,
                    TimeEndAM: Type3StartTimeEndAM2,
                    TimeAM: Type3StartTimeAM2,
                    TimeBeginBM: Type3StartTimeBeginBM2,
                    TimeEndBM: Type3StartTimeEndBM2,
                    TimeBM: Type3StartTimeBM2,
                    Sort: 1
                }
                TestStrategy.push(result);


                var Type3StartTimeBeginAM3 = Ext.getCmp("3Type3StartTimeBeginAM").getValue();
                var Type3StartTimeEndAM3 = Ext.getCmp("3Type3StartTimeEndAM").getValue();
                var Type3StartTimeAM3 = Ext.getCmp("3Type3StartTimeAM").getValue();
                var Type3StartTimeBeginBM3 = Ext.getCmp("3Type3StartTimeBeginBM").getValue();
                var Type3StartTimeEndBM3 = Ext.getCmp("3Type3StartTimeEndBM").getValue();
                var Type3StartTimeBM3 = Ext.getCmp("3Type3StartTimeBM").getValue();
                //SetType配置类型(1:岗位配置 2:特殊人员配置)
                //SignType签到类型(1,2,3)
                //ShiftNum班次
                var result = {
                    Id: Ext.getCmp("3Type3Id").getValue(),
                    LinkId: comModel,
                    Limit: LimitNum,
                    SignType:3,
                    SetType:1,
                    Title: '晚班',
                    ShiftNum:3, //班次
                    TimeBeginAM: Type3StartTimeBeginAM3,
                    TimeEndAM: Type3StartTimeEndAM3,
                    TimeAM: Type3StartTimeAM3,
                    TimeBeginBM: Type3StartTimeBeginBM3,
                    TimeEndBM: Type3StartTimeEndBM3,
                    TimeBM: Type3StartTimeBM3,
                    Sort:2
                }
                TestStrategy.push(result);
                break;
        }
        //配置信息
        var Strategy = Ext.encode(TestStrategy);
        var para = { DeptId: comModel, Limit: LimitNum, SignType: SignTypeNum, SetType: 1, Detail: Strategy, CanSave: true };
        formPanel.getForm().submit({
            waitTitle: '系统提示', //标题
            waitMsg: '正在提交数据,请稍后...', //提示信息
            submitEmptyText: false, //If set to true, the emptyText value will be sent with the form when it is submitted. Defaults to true                                   
            url: url, //记录表单提交的路径
            method: "POST",
            params: para,
            success: function (form, action) {
                //成功后
                var flag = action.result;
                if (flag.success) {
                    MessageInfo("保存成功！", "right");
                    ChangeModel(flag.msg);
                } else {
                    MessageInfo("保存失败！", "error");
                }
            },
            failure: function (form, action) {
                MessageInfo("保存失败！", "error");
            }
        });
    }
}

//创建表单弹框
function CreateFromWindow(title, key) {

    var center = new top.Ext.TabPanel({
        deferredRender: false,
        animScroll: true,              //使用动画滚动效果   
        enableTabScroll: true,         //tab标签过宽时自动显示滚动条
        id: 'TabPanel',
        activeTab: 0,
        border: false,
        listeners: {
            tabchange: function (thisTab, Activetab) {
            }
        }
    });

    var form = new top.Ext.FormPanel({
        labelWidth: 75,
        frame: true,
        border: false,
        layout: 'fit',
        labelAlign: 'right',
        id: 'formPanel',
        layout: 'column',
        items: [
            {
                xtype: 'panel',
                defaultType: 'textfield',
                autoHeight: true,
                anchor: '100%',
                border: false,
                columnWidth: .7,
                style: 'margin-top:10px;',
                layout: 'form',
                items: [
                      {
                          name: 'UserName',
                          id: 'UserName',
                          fieldLabel: '<span class="required">*</span>用户名称',
                          emptyText: '填写用户名称', ////textfield自己的属性
                          allowBlank: false,
                          maxLength: 20,
                          anchor: '90%',
                          maxLengthText: '用户名称长度不能超过20个字符',
                          enableKeyEvents: true,
                          listeners: {
                              'blur': function (val) {
                                  if (val.getValue().trim() != "") {
                                      var code = codeConvert(top.Ext.getCmp("UserName").getValue());
                                      return VilivaName(key, code);
                                  }
                              }
                          }

                      },
                      {
                          name: 'LoginName',
                          id: 'LoginName',
                          fieldLabel: '<span class="required">*</span>登录账号',
                          emptyText: '请输入手机号', ////textfield自己的属性
                          allowBlank: false,
                          maxLength: 12,
                          regex: /^\d+$/,
                          emptyText: '请输入有效的电话号码',
                          anchor: '90%',
                          enableKeyEvents: true,
                          listeners: {
                              'blur': function (val) {
                                  if (val.getValue()) {
                                      return VilivaName(key, val.getValue());
                                  }
                              }
                              //                              'keyup': function (val) {
                              //                                  if (val.getValue().trim() != "") {
                              //                                      top.Ext.getCmp("Phone").setValue(val.getValue());
                              //                                  }
                              //                              }
                          }
                      },
                      {
                          name: 'Pwd',
                          id: 'Pwd',
                          fieldLabel: '登录密码',
                          emptyText: '默认登录密码:666666', ////textfield自己的属性
                          maxLength: 20,
                          anchor: '90%',
                          maxLengthText: '登录密码长度不能超过12个字符'
                      },
                      {
                          name: 'Sex',
                          xtype: 'radiogroup',
                          fieldLabel: '性别',
                          id: 'Sex',
                          width: 100,
                          items: [
                                { boxLabel: '男', name: 'Sex', inputValue: 0, checked: true, width: 100, height: 25 },
                                { boxLabel: '女', name: 'Sex', inputValue: 1, width: 100, height: 25 }
                           ],
                          listeners: {
                              'change': function (val) {
                                  var result = top.Ext.getCmp("Sex").getValue();
                                  var hasHPact = top.Ext.getCmp('HPact').getValue();
                                  if (hasHPact == "" || hasHPact == null) {
                                      if (result.inputValue == 1) {//女
                                          top.Ext.getCmp("headImg").getEl().dom.src = '../../Resource/css/icons/head/GTP_hfemale_big.jpg';
                                      }
                                      else {
                                          top.Ext.getCmp("headImg").getEl().dom.src = '../../Resource/css/icons/head/GTP_hmale_big.jpg';
                                      }
                                  }
                              }
                          }
                      },
//                      {
//                          name: 'OperateNum',
//                          id: 'OperateNum',
//                          fieldLabel: '操作证号',
//                          anchor: '90%'
//                      },
                      {
                          name: 'Email',
                          id: 'Email',
                          fieldLabel: '邮箱',
                         anchor: '90%',
                          xtype: 'textfield',
                          vtype: 'email'
                      },
                      {
                          name: 'CardNum',
                          id: 'CardNum',
                          fieldLabel: '身份证号码',
                          anchor: '90%',
                          enableKeyEvents: true,
                          listeners: {
                              'blur': function (val) {
                                  if (val.getValue()) {
                                      return validId(val);
                                  }
                              }
                          }
                      }
                    ]
            },
            {
                xtype: 'panel',
                columnWidth: .3,
                layout: 'form',
                border: false,
                isFormField: true,
                items: [
                     {
                         xtype: 'fieldset',
                         layout: 'form',
                         style: 'margin-top:10px;padding:10px',
                         defaultType: 'textfield',
                         height: 160, //图片高度
                         width: 120, //图片宽度
                         items: [
                                    {
                                        xtype: 'box', //或者xtype: 'component',
                                        width: 100, //图片宽度
                                        align: 'center',
                                        id: 'headImg',
                                        height: 110, //图片高度
                                        autoEl: {
                                            tag: 'img',    //指定为img标签
                                            src: '../../Resource/css/icons/head/GTP_hmale_big.jpg'    //指定url路径
                                        }
                                    },
                                     {
                                         xtype: 'tbtext',
                                         id: 'showImg',
                                         style: 'margin-left:30px;color:blue;cursor:pointer;margin-top:6px',
                                         text: '头像上传'
                                     },
                                    {
                                        name: 'HPact',
                                        id: 'HPact',
                                        xtype: 'textfield',
                                        hidden: true
                                    }
                            ]

                     }
                ]
            }
//            {
//                columnWidth: 1,
//                layout: 'form',
//                border: false,
//                autoHeight: true,
//                items: [
//                         
//                          {
//                              name: 'Phone',
//                              fieldLabel: '支付宝账号',
//                              xtype: 'textfield',
//                              anchor: '100%',
//                              id: 'Phone',
//                              emptyText: '请输入支付宝账号'
//                          }
//                    ]
//            }
        ]
    });

    var tabs1 = center.add({
        tabTip: '基本信息',
        layout: 'fit',
        title: '基本信息',
        border: false,
        items: [form]
    });
    center.setActiveTab(tabs1);
    //窗体
    var win = Window("window", title, center);
    win.width = 570;
    win.height = 450;
    return win;
}

//设置上传用户头像文件状态
function HPactFileUploadAction(o) {
    //验证同文件的正则  
    var img_reg = /\.([jJ][pP][gG]){1}$|\.([jJ][pP][eE][gG]){1}$|\.([gG][iI][fF]){1}$|\.([pP][nN][gG]){1}$|\.([bB][mM][pP]){1}$/;
    if (!img_reg.test(o.value)) {
        top.Ext.Msg.show({ title: "信息提示", msg: "文件类型错误,请选择文件(jpg/jpeg/gif/png/bmp)", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
        o.setRawValue('');
        return;
    }
}

///验证用户名是否重复
function VilivaName(key, value) {
    var respon = Ext.lib.Ajax.getConnectionObject().conn;
    respon.open("post", "/UserStaff/ExitsMaster?key=" + key + "&code=" + encodeURIComponent(value.trim()), false);
    respon.send(null);
    var response = Ext.util.JSON.decode(respon.responseText);
    if (response.success) {
        top.Ext.Msg.show({
            title: "信息提示",
            msg: "该登录账号已被注册使用,请重新输入",
            buttons: Ext.Msg.OK,
            icon: Ext.MessageBox.INFO,
            fn: function () {
                top.Ext.getCmp("LoginName").setValue("");
            }
        });
        return false;
    }
}

//启用
function EnableUse() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        if (rows[0].data["Status"] == 1) {
            MessageInfo("该用户已经启用！", "statusing");
            return;
        }
        if (rows[0].data["Status"] == -2) {
            MessageInfo("该用户还未审核通过,不能操作！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要启用该用户吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                Ext.Ajax.request({
                    url: '/SysMaster/EnableUse',
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
            MessageInfo("该用户已经禁用！", "statusing");
            return;
        }
        if (rows[0].data["Status"] == -2) {
            MessageInfo("该用户还未审核通过,不能操作！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '禁用后,该用户下的所有信息将被禁用.<br/><br/>确认要禁用该用户吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                Ext.Ajax.request({
                    url: '/SysMaster/DisableUse',
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

//密码重置
function ReSetPassword() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        if (rows[0].data["Status"] == -2) {
            MessageInfo("该用户还未审核通过,不能操作！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要重置该用户的登录密码吗? 重置后密码为:666666', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                Ext.Ajax.request({
                    url: '/Home/ReSetPwd',
                    params: { id: ids.join(",") },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            Ext.getCmp("gg").store.reload();
                            MessageInfo("重置成功！", "right");
                        } else {
                            MessageInfo("重置失败！", "error");
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

//批量导入
function ImportDate() {
    if (parent) {
        var grid = Ext.getCmp("tr");
        var rows = grid.getSelectionModel().getSelectedNode();
        var Cid = '0';
        if (rows) {
            if (rows.attributes.Attribute == 5) {
                Cid = rows.attributes.id;
            }
            else {
                MessageInfo("请先选择部门！", "statusing");
                return false;
            }
        }
        else {
            MessageInfo("请先选择部门！", "statusing");
            return false;
        }

        var tabId = parent.GetActiveTabId(); //公共变量
        parent.AddTabPanel('批量导入', 'TestExport', tabId, '/SysMaster/Export?Cid=' + Cid);
    }
}

//审核
function GTPAudit() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        if (rows[0].data["Status"] != -2) {
            MessageInfo("该用户无需审核！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要审核通过该用户吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                Ext.Ajax.request({
                    url: '/UserStaff/GTPAudit',
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

//金额详细
function MoneyDetails() {
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].data["Id"];
        LookDetails(key);
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}

//选择员工
function LookDetails(key) {
    var shop = new top.Ext.Window({
        width: 680,
        id: 'shoper',
        height: 394,
        closable: true,
        shadow: false,
        stateful: false,
        border: false,
        modal: true,
        layout: 'fit',
        plain: true,
        autoDestroy: true,
        closeAction: 'close',
        title: '个人流水信息',
        items: {
            autoScroll: true,
            border: false,
            params: { Eid: key },
            autoLoad: { url: '../../Project/Html/TotalMaster.htm', scripts: true, nocache: true }
        },
        buttons: [
                    {
                        text: '确定',
                        tooltip: '关闭窗体',
                        handler: function () {
                            top.Ext.getCmp("shoper").close();
                        }
                    }]
    }).show();
            }

//审核通过
function GTPsubmit() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        if (rows[0].data["Status"] != -2) {
            MessageInfo("该用户无需审核！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要审核通过该用户吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                Ext.Ajax.request({
                    url: '/UserStaff/GTPAudit',
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

//取消审核
function GTPcancel() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        if (rows[0].data["Status"] != -2) {
            MessageInfo("该用户无需审核！", "statusing");
            return;
        }
        var key = rows[0].data["Id"];
        //弹出框
        var window = CreateFromAuditWindow('审核不通过',key);
        window.show();
//        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要取消该用户申请吗?', function (e) {
//            if (e == "yes") {
//                var ids = [];
//                for (var i = 0; i < rows.length; i++) {
//                    ids.push(rows[i].data["Id"]);
//                }
//                Ext.Ajax.request({
//                    url: '/UserStaff/GTPAuditcancel',
//                    params: { id: ids.join(",") },
//                    success: function (response) {
//                        var rs = eval('(' + response.responseText + ')');
//                        if (rs.success) {
//                            Ext.getCmp("gg").store.reload();
//                            MessageInfo("取消成功！", "right");
//                        } else {
//                            MessageInfo("取消失败！", "error");
//                        }
//                    }
//                });
//            }
//        });
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}

//创建表单弹框
function CreateFromAuditWindow(title, key) {
    var form = new top.Ext.FormPanel({
        labelWidth: 65,
        frame: true,
        border: false,
        labelAlign: 'right',
        bodyStyle: 'padding:5px 5px 0',
        id: 'formPanel',
        defaultType: 'textfield',
        items: [
                {
                    fieldLabel: '<span class="required">*</span>描述信息',
                xtype: 'textarea',
                id: 'Introduction',
                name: 'Introduction',
                height: 150,
                allowBlank: false,
                anchor: '95%',
                emptyText: '可输入审核失败缘由(70字符内)', ////textfield自己的属性
                maxLength:70,
                maxLengthText: '描述长度不能超过70个字符'
            }
        ]
        });

        //自定义button
    var button1= new top.Ext.Button({
        text: '提交',
        iconCls: 'GTP_submit',
        tooltip: '提交',
        handler: function () {
            var formPanel = top.Ext.getCmp("formPanel");
            if (formPanel.getForm().isValid()) {//如果验证通过
                formPanel.getForm().submit({
                    waitTitle: '系统提示', //标题
                    waitMsg: '正在提交数据,请稍后...', //提示信息
                    submitEmptyText: false, //If set to true, the emptyText value will be sent with the form when it is submitted. Defaults to true                                   
                    url:'/UserStaff/GTPAuditcancel?id=' + key,
                    method: "POST",
                    success: function (form, action) {
                        //成功后
                        var flag = action.result;
                        if (flag.success) {
                            Ext.getCmp("gg").store.reload();
                            MessageInfo("取消成功！", "right");
                        } else {
                            MessageInfo("取消失败！", "error");
                        }
                        top.Ext.getCmp('window').close();
                    },
                    failure: function (form, action) {
                        MessageInfo("取消失败！", "error");
                    }
                });
            }   
        }
    })
    //窗体
    var win = WindowDiy("window", title, form, button1);
    win.width =400;
    win.height =250;
    return win;
}