Ext.onReady(function () {
    /*
    * ================页面布局=======================
    */
    //定义树的加载器 
    var treeloader = new Ext.tree.TreeLoader({
        url: "/Organizational/SearchPostData"
    });

    var tree = new Ext.tree.TreePanel({
        region: 'center',
        id: 'tr',
        layout: 'fit',
        layoutConfig: {
            animate: true
        },
        autoScroll: true,
        animate: true, //动画效果  
        rootVisible: false, //根节点是否可见  
        lines: true, //显示树形控件的前导线
        containerScroll: true,
        border: false,
        bodyStyle: 'padding-top:5px',
        loader: new Ext.tree.TreeLoader({
            dataUrl: '/Organizational/SearchPostData'
        }),
        tbar: [
               {
                   xtype: 'textfield',
                   emptyText: '输入岗位名称....',
                   id: 'cityText',
                   width: 140,
                   enableKeyEvents: true,
                   listeners: {
                       'keyup': function (val) {
                           if (val.getValue()) {
                               tree.loader.dataUrl = "/Organizational/SearchPostData?BrandName=" + encodeURIComponent(val.getValue());
                           }
                           else {
                               tree.loader.dataUrl = "/Organizational/SearchPostData";
                           }
                           tree.root.reload();
                       }
                   }
               },
                '->',
                 {
                     iconCls: 'GTP_refresh',
                     text: '刷新',
                     tooltip: '刷新岗位',
                     handler: function () {
                         treeNodeId = -1;
                         var name = Ext.getCmp("cityText").getValue();
                         if (name) {
                             tree.loader.dataUrl = "/Organizational/SearchPostData?BrandName=" + encodeURIComponent(name);
                         }
                         else {
                             tree.loader.dataUrl = "/Organizational/SearchPostData";
                         }
                         tree.root.reload();
                     }
                 }
            ],
        root: {
            nodeType: 'async',
            text: '岗位列表',
            //iconCls: 'GTP_home',
            draggable: false,
            id: 'top'//区分是否根节点
        },
        listeners: {
            click: treeitemclick
        }
    });
    tree.expandAll();

    /*
    所属员工
    */
    var Model = new Ext.data.Store({
        proxy: new Ext.data.HttpProxy({
            url: '/DeptSet/GetTreeByMaster?OrganizaId=' + treeNodeId,
            method: 'POST'
        }),
        reader: new Ext.data.JsonReader({},
              Ext.data.Record.create(["id", "text"]
        ))
    });

    var form = new Ext.FormPanel({
        id: "formPanel",
        fileUpload: true, //有需文件上传的,需填写该属性
        border: false,
        labelAlign: 'right',
        autoScroll: true,
        labelWidth: 85,
        tbar: [
             {
                 iconCls: 'GTP_save',
                 text: '保存',
                 handler: function () {
                     SaveDate();
                 }
             }
            ],
        items: [
               {
                   xtype: 'panel',
                   border: false,
                   layout: 'column',
                   items: [
                        {
                            columnWidth: .6,
                            layout: 'form',
                            border: false,
                            bodyStyle: 'margin:5px;padding:14px;',
                            items: [
                             {
                                 xtype: 'combo',
                                 triggerAction: 'all',
                                 id: 'comModel',
                                 name: 'comModel',
                                 fieldLabel: '<span class="required">*</span>所属员工',
                                 emptyText: '所属员工',
                                 forceSelection: true,
                                 editable: false,
                                 allowBlank: false,
                                 displayField: 'text',
                                 valueField: 'id',
                                 hiddenName: 'text',
                                 anchor: '95%',
                                 store: Model,
                                 listeners: {
                                     select: {
                                         fn: function (combo, record, index) {
                                             var key = record.get('id');
                                             ChangeModel(key);
                                         }
                                     }
                                 }
                             },
                            {
                                xtype: 'compositefield',
                                fieldLabel: '<span class="required">*</span>签到范围',
                                combineErrors: false,
                                anchor: '100%',
                                items: [
                                             {
                                                 xtype: 'numberfield',
                                                 value: 0,
                                                 minValue: 0,
                                                 maxValue: 9999,
                                                 name: 'Limit',
                                                 fieldLabel: '<span class="required">*</span>签到范围',
                                                 id: 'Limit',
                                                 allowBlank: false,
                                                 width: 250,
                                                 enableKeyEvents: true,
                                                 listeners: {
                                                     'keyup': function (val) {
                                                         var code = Ext.getCmp("Limit").getValue();
                                                         var ComPLon = Ext.getCmp("ComPLon").getValue();
                                                         var tip = "无范围限制签到";
                                                         if (val.getValue() != "") {
                                                             if (code > 0 && ComPLon != "") {
                                                                 map.clearOverlays(); //清空原来的标注
                                                                 var mPoint = new BMap.Point(Ext.getCmp("ComPLon").getValue(), Ext.getCmp("CompLat").getValue());
                                                                 map.enableScrollWheelZoom();
                                                                 map.centerAndZoom(mPoint, 15);
                                                                 tip = "范围" + code + "米内可正常签到";
                                                                 var circle = new BMap.Circle(mPoint, code, { fillColor: "Red", strokeWeight: 1, fillOpacity: 0.3, strokeOpacity: 0.3 });
                                                                 map.addOverlay(circle);

                                                                 var marker = new BMap.Marker(mPoint);    // 创建标注
                                                                 var station = new BMap.Point(Ext.getCmp("ComPLon").getValue(), Ext.getCmp("CompLat").getValue());
                                                                 marker = new BMap.Marker(station);
                                                                 map.addOverlay(marker);
                                                                 var infoWindow = new BMap.InfoWindow("<p style='font-size:12px;'>您的坐标:(" + Ext.getCmp("ComPLon").getValue() + "," + Ext.getCmp("CompLat").getValue() + ")</p><p style='font-size:12px;'>地址:" + Ext.getCmp("Address").getValue() + "</p><br/><p style='font-size:11px;color:orange'>" + tip + "。</p>");
                                                                 map.openInfoWindow(infoWindow, mPoint); //开启信息窗口
                                                             }
                                                         }
                                                         else {
                                                             debugger
                                                             tip = "无范围限制签到";
                                                             if (ComPLon != "") {
                                                                 map.clearOverlays(); //清空原来的标注
                                                                 var mPoint = new BMap.Point(Ext.getCmp("ComPLon").getValue(), Ext.getCmp("CompLat").getValue());
                                                                 map.enableScrollWheelZoom();
                                                                 map.centerAndZoom(mPoint, 15);
                                                                 var marker = new BMap.Marker(mPoint);    // 创建标注
                                                                 var station = new BMap.Point(Ext.getCmp("ComPLon").getValue(), Ext.getCmp("CompLat").getValue());
                                                                 marker = new BMap.Marker(station);
                                                                 map.addOverlay(marker);
                                                                 var infoWindow = new BMap.InfoWindow("<p style='font-size:12px;'>您的坐标:(" + Ext.getCmp("ComPLon").getValue() + "," + Ext.getCmp("CompLat").getValue() + ")</p><p style='font-size:12px;'>地址:" + Ext.getCmp("Address").getValue() + "</p><br/><p style='font-size:11px;color:orange'>" + tip + "。</p>");
                                                                 map.openInfoWindow(infoWindow, mPoint); //开启信息窗口
                                                             }
                                                         }
                                                     }
                                                 }
                                             }, {
                                                 xtype: 'tbtext',
                                                 style: 'margin-top:5px',
                                                 text: '（米）'
                                             }
                                        ]
                            },
                            {
                                xtype: 'textfield',
                                name: 'ComPLon',
                                id: 'ComPLon',
                                hidden: true
                            },
                            {
                                xtype: 'textfield',
                                name: 'CompLat',
                                id: 'CompLat',
                                hidden: true
                            },
                            {
                                xtype: 'textfield',
                                name: 'Address',
                                id: 'Address',
                                hidden: true
                            },
                                 {
                                     xtype: 'combo',
                                     triggerAction: 'all',
                                     emptyText: '签到类型',
                                     forceSelection: true,
                                     allowBlank: false,
                                     displayField: 'text',
                                     fieldLabel: '签到类型',
                                     valueField: 'value',
                                     id: 'ComSignType',
                                     mode: 'local',
                                     editable: false,
                                     width: 250,
                                     store: new Ext.data.SimpleStore({
                                         fields: ['text', 'value'],
                                         data: [['类型1', '1'], ['类型2', '2'], ['类型3', '3']]
                                     }),
                                     value: '1',
                                     listeners: {
                                         select: {
                                             fn: function (combo, record, index) {
                                                 var value = record.get('value');
                                                 changePanel(value); //切换面板
                                             }
                                         }
                                     }
                                 },
                                
                                   {
                                       xtype: 'checkbox',
                                       fieldLabel: '<span class="required">*</span>开启考勤',
                                       id: 'AutomaticSave',
                                       name: 'AutomaticSave',
                                       anchor: '90%',
                                       //checked: true,
                                       listeners: {
                                           check: function (field, newValue, oldValue, eOpts) {
                                               if (newValue) {
                                                   changePanel(Ext.getCmp("ComSignType").getValue());
                                               }
                                               else {
                                                   Ext.getCmp("Type3").hide();
                                                   Ext.getCmp("Type1").hide();
                                                   Ext.getCmp("Type2").hide();
                                               }
                                           }
                                       }
                                   },
                                  {
                                      anchor: '95%',
                                      height: 20,
                                      width: 500,
                                      style: 'border-bottom:1px dashed #DBDBDB',
                                      border: false
                                  },
                                  {
                                      anchor: '95%',
                                      xtype: 'panel',
                                      id: 'Type1',
                                      hidden: true,
                                      border: false,
                                      layout: "form", // 从上往下的布局
                                      items: [
                                       {
                                           id: 'Type1Id',
                                           hidden: true,
                                           xtype: 'textfield'
                                       },
                                            {
                                                xtype: 'compositefield',
                                                fieldLabel: '<span class="required">*</span>(上班)签到',
                                                combineErrors: false,
                                                style: 'margin-top:10px',
                                                items: [

                                                    new Ext.form.TimeField({
                                                        width: 70,
                                                        id: 'Type1StartTimeBeginAM',
                                                        maxValue: '23:59',                      //最大时间   
                                                        maxText: '所选时间小于{0}',
                                                        minValue: '00:00',                      //最小时间   
                                                        minText: '所选时间大于{0}',
                                                        maxHeight: 180,
                                                        allowBlank: false,
                                                        editable: false,
                                                        value: "08:00",
                                                        increment: 15,                          //时间间隔   
                                                        format: 'G:i',                    //时间格式   
                                                        invalidText: '时间格式无效'
                                                    }),
                                                      {
                                                          xtype: 'tbtext',
                                                          text: '~'
                                                      },
                                             new Ext.form.TimeField({
                                                 width: 70,
                                                 id: 'Type1StartTimeEndAM',
                                                 maxValue: '23:59',                      //最大时间   
                                                 maxText: '所选时间小于{0}',
                                                 minValue: '00:00',                      //最小时间   
                                                 minText: '所选时间大于{0}',
                                                 maxHeight: 180,
                                                 allowBlank: false,
                                                 editable: false,
                                                 value: "10:00",
                                                 increment: 15,                          //时间间隔   
                                                 format: 'G:i',                    //时间格式   
                                                 invalidText: '时间格式无效'
                                             }),
                                              {
                                                  xtype: 'tbtext',
                                                  text: '- 上班时间',
                                                  style: 'margin-top:4px'
                                              },
                                                new Ext.form.TimeField({
                                                    width: 70,
                                                    id: 'Type1StartTimeAM',
                                                    maxValue: '23:59',                      //最大时间   
                                                    maxText: '所选时间小于{0}',
                                                    minValue: '00:00',                      //最小时间   
                                                    minText: '所选时间大于{0}',
                                                    maxHeight: 180,
                                                    allowBlank: false,
                                                    editable: false,
                                                    value: "09:00",
                                                    increment: 15,                          //时间间隔   
                                                    format: 'G:i',                    //时间格式   
                                                    invalidText: '时间格式无效'
                                                })

					            ]
                                            },
                                            {
                                                xtype: 'compositefield',
                                                fieldLabel: '<span class="required">*</span>(下班)签退',
                                                combineErrors: false,
                                                style: 'margin-top:10px',
                                                items: [

                                        new Ext.form.TimeField({
                                            width: 70,
                                            id: 'Type1StartTimeBeginBM',
                                            maxValue: '23:59',                      //最大时间   
                                            maxText: '所选时间小于{0}',
                                            minValue: '00:00',                      //最小时间   
                                            minText: '所选时间大于{0}',
                                            maxHeight: 180,
                                            allowBlank: false,
                                            editable: false,
                                            value: "17:30",
                                            increment: 15,                          //时间间隔   
                                            format: 'G:i',                    //时间格式   
                                            invalidText: '时间格式无效'
                                        }),
                                         {
                                             xtype: 'tbtext',
                                             text: '~'
                                         },
                                         new Ext.form.TimeField({
                                             width: 70,
                                             id: 'Type1StartTimeEndBM',
                                             maxValue: '23:59',                      //最大时间   
                                             maxText: '所选时间小于{0}',
                                             minValue: '00:00',                      //最小时间   
                                             minText: '所选时间大于{0}',
                                             maxHeight: 180,
                                             allowBlank: false,
                                             editable: false,
                                             value: "23:00",
                                             increment: 15,                          //时间间隔   
                                             format: 'G:i',                    //时间格式   
                                             invalidText: '时间格式无效'
                                         }),
                                          {
                                              xtype: 'tbtext',
                                              text: '- 下班时间',
                                              style: 'margin-top:4px'
                                          },
                                                new Ext.form.TimeField({
                                                    width: 70,
                                                    id: 'Type1StartTimeBM',
                                                    maxValue: '23:59',                      //最大时间   
                                                    maxText: '所选时间小于{0}',
                                                    minValue: '00:00',                      //最小时间   
                                                    minText: '所选时间大于{0}',
                                                    maxHeight: 180,
                                                    allowBlank: false,
                                                    editable: false,
                                                    value: "17:30",
                                                    increment: 15,                          //时间间隔   
                                                    format: 'G:i',                    //时间格式   
                                                    invalidText: '时间格式无效'
                                                })

					            ]
                                            }
                                      ]
                                  },
                                  {
                                      anchor: '95%',
                                      xtype: 'panel',
                                      id: 'Type2',
                                      hidden: true,
                                      border: false,
                                      layout: "form", // 从上往下的布局
                                      items: [
                                        {
                                            id: '1Type2Id',
                                            hidden: true,
                                            xtype: 'textfield'
                                        },
                                        {
                                            id: '2Type2Id',
                                            hidden: true,
                                            xtype: 'textfield'
                                        },
                                            {
                                                xtype: 'compositefield',
                                                fieldLabel: '<span class="required">*</span>(早班上)签到',
                                                combineErrors: false,
                                                style: 'margin-top:10px',
                                                items: [

                                        new Ext.form.TimeField({
                                            width: 70,
                                            id: '1Type2StartTimeBeginAM',
                                            maxValue: '23:59',                      //最大时间   
                                            maxText: '所选时间小于{0}',
                                            minValue: '00:00',                      //最小时间   
                                            minText: '所选时间大于{0}',
                                            maxHeight: 180,
                                            allowBlank: false,
                                            editable: false,
                                            value: "08:00",
                                            increment: 15,                          //时间间隔   
                                            format: 'G:i',                    //时间格式   
                                            invalidText: '时间格式无效'
                                        }),
                                             {
                                                 xtype: 'tbtext',
                                                 text: '~'
                                             },
                                             new Ext.form.TimeField({
                                                 width: 70,
                                                 id: '1Type2StartTimeEndAM',
                                                 maxValue: '23:59',                      //最大时间   
                                                 maxText: '所选时间小于{0}',
                                                 minValue: '00:00',                      //最小时间   
                                                 minText: '所选时间大于{0}',
                                                 maxHeight: 180,
                                                 allowBlank: false,
                                                 editable: false,
                                                 value: "10:00",
                                                 increment: 15,                          //时间间隔   
                                                 format: 'G:i',                    //时间格式   
                                                 invalidText: '时间格式无效'
                                             }),
                                              {
                                                  xtype: 'tbtext',
                                                  text: '- 上班时间',
                                                  style: 'margin-top:4px'
                                              },
                                                new Ext.form.TimeField({
                                                    width: 70,
                                                    id: '1Type2StartTimeAM',
                                                    maxValue: '23:59',                      //最大时间   
                                                    maxText: '所选时间小于{0}',
                                                    minValue: '00:00',                      //最小时间   
                                                    minText: '所选时间大于{0}',
                                                    maxHeight: 180,
                                                    allowBlank: false,
                                                    editable: false,
                                                    value: "09:00",
                                                    increment: 15,                          //时间间隔   
                                                    format: 'G:i',                    //时间格式   
                                                    invalidText: '时间格式无效'
                                                })

					                              ]
                                            },
                                            {
                                                xtype: 'compositefield',
                                                fieldLabel: '<span class="required">*</span>(早班下)签退',
                                                combineErrors: false,
                                                style: 'margin-top:10px',
                                                items: [

                                            new Ext.form.TimeField({
                                                width: 70,
                                                id: '1Type2StartTimeBeginBM',
                                                maxValue: '23:59',                      //最大时间   
                                                maxText: '所选时间小于{0}',
                                                minValue: '00:00',                      //最小时间   
                                                minText: '所选时间大于{0}',
                                                maxHeight: 180,
                                                allowBlank: false,
                                                editable: false,
                                                value: "12:00",
                                                increment: 15,                          //时间间隔   
                                                format: 'G:i',                    //时间格式   
                                                invalidText: '时间格式无效'
                                            }),
                                         {
                                             xtype: 'tbtext',
                                             text: '~'
                                         },
                                         new Ext.form.TimeField({
                                             width: 70,
                                             id: '1Type2StartTimeEndBM',
                                             maxValue: '23:59',                      //最大时间   
                                             maxText: '所选时间小于{0}',
                                             minValue: '00:00',                      //最小时间   
                                             minText: '所选时间大于{0}',
                                             maxHeight: 180,
                                             allowBlank: false,
                                             editable: false,
                                             value: "13:00",
                                             increment: 15,                          //时间间隔   
                                             format: 'G:i',                    //时间格式   
                                             invalidText: '时间格式无效'
                                         }),
                                          {
                                              xtype: 'tbtext',
                                              text: '- 下班时间',
                                              style: 'margin-top:4px'
                                          },
                                                new Ext.form.TimeField({
                                                    width: 70,
                                                    id: '1Type2StartTimeBM',
                                                    maxValue: '23:59',                      //最大时间   
                                                    maxText: '所选时间小于{0}',
                                                    minValue: '00:00',                      //最小时间   
                                                    minText: '所选时间大于{0}',
                                                    maxHeight: 180,
                                                    allowBlank: false,
                                                    editable: false,
                                                    value: "12:00",
                                                    increment: 15,                          //时间间隔   
                                                    format: 'G:i',                    //时间格式   
                                                    invalidText: '时间格式无效'
                                                })

					            ]
                                            },
                                            {
                                                anchor: '95%',
                                                height: 20,
                                                width: 500,
                                                style: 'border-bottom:1px dashed #DBDBDB',
                                                border: false
                                            },
                                            {
                                                xtype: 'compositefield',
                                                fieldLabel: '<span class="required">*</span>(晚班上)签到',
                                                combineErrors: false,
                                                style: 'margin-top:10px',
                                                items: [
                                                    new Ext.form.TimeField({
                                                        width: 70,
                                                        id: '2Type2StartTimeBeginAM',
                                                        maxValue: '23:59',                      //最大时间   
                                                        maxText: '所选时间小于{0}',
                                                        minValue: '00:00',                      //最小时间   
                                                        minText: '所选时间大于{0}',
                                                        maxHeight: 180,
                                                        allowBlank: false,
                                                        editable: false,
                                                        value: "13:00",
                                                        increment: 15,                          //时间间隔   
                                                        format: 'G:i',                    //时间格式   
                                                        invalidText: '时间格式无效'
                                                    }),
                                                   {
                                                       xtype: 'tbtext',
                                                       text: '~'
                                                   },
                                                    new Ext.form.TimeField({
                                                        width: 70,
                                                        id: '2Type2StartTimeEndAM',
                                                        maxValue: '23:59',                      //最大时间   
                                                        maxText: '所选时间小于{0}',
                                                        minValue: '00:00',                      //最小时间   
                                                        minText: '所选时间大于{0}',
                                                        maxHeight: 180,
                                                        allowBlank: false,
                                                        editable: false,
                                                        value: "14:00",
                                                        increment: 15,                          //时间间隔   
                                                        format: 'G:i',                    //时间格式   
                                                        invalidText: '时间格式无效'
                                                    }),
                                                      {
                                                          xtype: 'tbtext',
                                                          text: '- 上班时间',
                                                          style: 'margin-top:4px'
                                                      },
                                                     new Ext.form.TimeField({
                                                         width: 70,
                                                         id: '2Type2StartTimeAM',
                                                         maxValue: '23:59',                      //最大时间   
                                                         maxText: '所选时间小于{0}',
                                                         minValue: '00:00',                      //最小时间   
                                                         minText: '所选时间大于{0}',
                                                         maxHeight: 180,
                                                         allowBlank: false,
                                                         editable: false,
                                                         value: "13:00",
                                                         increment: 15,                          //时间间隔   
                                                         format: 'G:i',                    //时间格式   
                                                         invalidText: '时间格式无效'
                                                     })

					                              ]
                                            },
                                            {
                                                xtype: 'compositefield',
                                                fieldLabel: '<span class="required">*</span>(晚班下)签退',
                                                combineErrors: false,
                                                style: 'margin-top:10px',
                                                items: [

                                            new Ext.form.TimeField({
                                                width: 70,
                                                id: '2Type2StartTimeBeginBM',
                                                maxValue: '23:59',                      //最大时间   
                                                maxText: '所选时间小于{0}',
                                                minValue: '00:00',                      //最小时间   
                                                minText: '所选时间大于{0}',
                                                maxHeight: 180,
                                                allowBlank: false,
                                                editable: false,
                                                value: "17:30",
                                                increment: 15,                          //时间间隔   
                                                format: 'G:i',                    //时间格式   
                                                invalidText: '时间格式无效'
                                            }),
                                         {
                                             xtype: 'tbtext',
                                             text: '~'
                                         },
                                         new Ext.form.TimeField({
                                             width: 70,
                                             id: '2Type2StartTimeEndBM',
                                             maxValue: '23:59',                      //最大时间   
                                             maxText: '所选时间小于{0}',
                                             minValue: '00:00',                      //最小时间   
                                             minText: '所选时间大于{0}',
                                             maxHeight: 180,
                                             allowBlank: false,
                                             editable: false,
                                             value: "23:00",
                                             increment: 15,                          //时间间隔   
                                             format: 'G:i',                    //时间格式   
                                             invalidText: '时间格式无效'
                                         }),
                                          {
                                              xtype: 'tbtext',
                                              text: '- 下班时间',
                                              style: 'margin-top:4px'
                                          },
                                                new Ext.form.TimeField({
                                                    width: 70,
                                                    id: '2Type2StartTimeBM',
                                                    maxValue: '23:59',                      //最大时间   
                                                    maxText: '所选时间小于{0}',
                                                    minValue: '00:00',                      //最小时间   
                                                    minText: '所选时间大于{0}',
                                                    maxHeight: 180,
                                                    allowBlank: false,
                                                    editable: false,
                                                    value: "17:30",
                                                    increment: 15,                          //时间间隔   
                                                    format: 'G:i',                    //时间格式   
                                                    invalidText: '时间格式无效'
                                                })

					            ]
                                            }
                                      ]
                                  },
                                  {
                                      anchor: '95%',
                                      xtype: 'panel',
                                      id: 'Type3',
                                      hidden: true,
                                      border: false,
                                      layout: "form", // 从上往下的布局
                                      items: [
                                        {
                                            id: '1Type3Id',
                                            hidden: true,
                                            xtype: 'textfield'
                                        },
                                        {
                                            id: '2Type3Id',
                                            hidden: true,
                                            xtype: 'textfield'
                                        },
                                        {
                                            id: '3Type3Id',
                                            hidden: true,
                                            xtype: 'textfield'
                                        },
                                            {
                                                xtype: 'compositefield',
                                                fieldLabel: '<span class="required">*</span>(早班上)签到',
                                                combineErrors: false,
                                                style: 'margin-top:10px',
                                                items: [

                                        new Ext.form.TimeField({
                                            width: 70,
                                            id: '1Type3StartTimeBeginAM',
                                            maxValue: '23:59',                      //最大时间   
                                            maxText: '所选时间小于{0}',
                                            minValue: '00:00',                      //最小时间   
                                            minText: '所选时间大于{0}',
                                            maxHeight: 180,
                                            allowBlank: false,
                                            editable: false,
                                            value: "08:00",
                                            increment: 15,                          //时间间隔   
                                            format: 'G:i',                    //时间格式   
                                            invalidText: '时间格式无效'
                                        }),
                                             {
                                                 xtype: 'tbtext',
                                                 text: '~'
                                             },
                                             new Ext.form.TimeField({
                                                 width: 70,
                                                 id: '1Type3StartTimeEndAM',
                                                 maxValue: '23:59',                      //最大时间   
                                                 maxText: '所选时间小于{0}',
                                                 minValue: '00:00',                      //最小时间   
                                                 minText: '所选时间大于{0}',
                                                 maxHeight: 180,
                                                 allowBlank: false,
                                                 editable: false,
                                                 value: "10:00",
                                                 increment: 15,                          //时间间隔   
                                                 format: 'G:i',                    //时间格式   
                                                 invalidText: '时间格式无效'
                                             }),
                                              {
                                                  xtype: 'tbtext',
                                                  text: '- 上班时间',
                                                  style: 'margin-top:4px'
                                              },
                                                new Ext.form.TimeField({
                                                    width: 70,
                                                    id: '1Type3StartTimeAM',
                                                    maxValue: '23:59',                      //最大时间   
                                                    maxText: '所选时间小于{0}',
                                                    minValue: '00:00',                      //最小时间   
                                                    minText: '所选时间大于{0}',
                                                    maxHeight: 180,
                                                    allowBlank: false,
                                                    editable: false,
                                                    value: "09:00",
                                                    increment: 15,                          //时间间隔   
                                                    format: 'G:i',                    //时间格式   
                                                    invalidText: '时间格式无效'
                                                })

					                              ]
                                            },
                                            {
                                                xtype: 'compositefield',
                                                fieldLabel: '<span class="required">*</span>(早班下)签退',
                                                combineErrors: false,
                                                style: 'margin-top:10px',
                                                items: [

                                            new Ext.form.TimeField({
                                                width: 70,
                                                id: '1Type3StartTimeBeginBM',
                                                maxValue: '23:59',                      //最大时间   
                                                maxText: '所选时间小于{0}',
                                                minValue: '00:00',                      //最小时间   
                                                minText: '所选时间大于{0}',
                                                maxHeight: 180,
                                                allowBlank: false,
                                                editable: false,
                                                value: "12:00",
                                                increment: 15,                          //时间间隔   
                                                format: 'G:i',                    //时间格式   
                                                invalidText: '时间格式无效'
                                            }),
                                         {
                                             xtype: 'tbtext',
                                             text: '~'
                                         },
                                         new Ext.form.TimeField({
                                             width: 70,
                                             id: '1Type3StartTimeEndBM',
                                             maxValue: '23:59',                      //最大时间   
                                             maxText: '所选时间小于{0}',
                                             minValue: '00:00',                      //最小时间   
                                             minText: '所选时间大于{0}',
                                             maxHeight: 180,
                                             allowBlank: false,
                                             editable: false,
                                             value: "13:00",
                                             increment: 15,                          //时间间隔   
                                             format: 'G:i',                    //时间格式   
                                             invalidText: '时间格式无效'
                                         }),
                                          {
                                              xtype: 'tbtext',
                                              text: '- 下班时间',
                                              style: 'margin-top:4px'
                                          },
                                                new Ext.form.TimeField({
                                                    width: 70,
                                                    id: '1Type3StartTimeBM',
                                                    maxValue: '23:59',                      //最大时间   
                                                    maxText: '所选时间小于{0}',
                                                    minValue: '00:00',                      //最小时间   
                                                    minText: '所选时间大于{0}',
                                                    maxHeight: 180,
                                                    allowBlank: false,
                                                    editable: false,
                                                    value: "12:00",
                                                    increment: 15,                          //时间间隔   
                                                    format: 'G:i',                    //时间格式   
                                                    invalidText: '时间格式无效'
                                                })

					            ]
                                            },
                                            {
                                                anchor: '95%',
                                                height: 20,
                                                width: 500,
                                                style: 'border-bottom:1px dashed #DBDBDB',
                                                border: false
                                            },
                                            {
                                                xtype: 'compositefield',
                                                fieldLabel: '<span class="required">*</span>(中班上)签到',
                                                combineErrors: false,
                                                style: 'margin-top:10px',
                                                items: [

                                        new Ext.form.TimeField({
                                            width: 70,
                                            id: '2Type3StartTimeBeginAM',
                                            maxValue: '23:59',                      //最大时间   
                                            maxText: '所选时间小于{0}',
                                            minValue: '00:00',                      //最小时间   
                                            minText: '所选时间大于{0}',
                                            maxHeight: 180,
                                            allowBlank: false,
                                            editable: false,
                                            value: "13:00",
                                            increment: 15,                          //时间间隔   
                                            format: 'G:i',                    //时间格式   
                                            invalidText: '时间格式无效'
                                        }),
                                             {
                                                 xtype: 'tbtext',
                                                 text: '~'
                                             },
                                             new Ext.form.TimeField({
                                                 width: 70,
                                                 id: '2Type3StartTimeEndAM',
                                                 maxValue: '23:59',                      //最大时间   
                                                 maxText: '所选时间小于{0}',
                                                 minValue: '00:00',                      //最小时间   
                                                 minText: '所选时间大于{0}',
                                                 maxHeight: 180,
                                                 allowBlank: false,
                                                 editable: false,
                                                 value: "14:00",
                                                 increment: 15,                          //时间间隔   
                                                 format: 'G:i',                    //时间格式   
                                                 invalidText: '时间格式无效'
                                             }),
                                              {
                                                  xtype: 'tbtext',
                                                  text: '- 上班时间',
                                                  style: 'margin-top:4px'
                                              },
                                                new Ext.form.TimeField({
                                                    width: 70,
                                                    id: '2Type3StartTimeAM',
                                                    maxValue: '23:59',                      //最大时间   
                                                    maxText: '所选时间小于{0}',
                                                    minValue: '00:00',                      //最小时间   
                                                    minText: '所选时间大于{0}',
                                                    maxHeight: 180,
                                                    allowBlank: false,
                                                    editable: false,
                                                    value: "13:00",
                                                    increment: 15,                          //时间间隔   
                                                    format: 'G:i',                    //时间格式   
                                                    invalidText: '时间格式无效'
                                                })

					                              ]
                                            },
                                            {
                                                xtype: 'compositefield',
                                                fieldLabel: '<span class="required">*</span>(中班下)签退',
                                                combineErrors: false,
                                                style: 'margin-top:10px',
                                                items: [

                                            new Ext.form.TimeField({
                                                width: 70,
                                                id: '2Type3StartTimeBeginBM',
                                                maxValue: '23:59',                      //最大时间   
                                                maxText: '所选时间小于{0}',
                                                minValue: '00:00',                      //最小时间   
                                                minText: '所选时间大于{0}',
                                                maxHeight: 180,
                                                allowBlank: false,
                                                editable: false,
                                                value: "17:30",
                                                increment: 15,                          //时间间隔   
                                                format: 'G:i',                    //时间格式   
                                                invalidText: '时间格式无效'
                                            }),
                                         {
                                             xtype: 'tbtext',
                                             text: '~'
                                         },
                                         new Ext.form.TimeField({
                                             width: 70,
                                             id: '2Type3StartTimeEndBM',
                                             maxValue: '23:59',                      //最大时间   
                                             maxText: '所选时间小于{0}',
                                             minValue: '00:00',                      //最小时间   
                                             minText: '所选时间大于{0}',
                                             maxHeight: 180,
                                             allowBlank: false,
                                             editable: false,
                                             value: "18:30",
                                             increment: 15,                          //时间间隔   
                                             format: 'G:i',                    //时间格式   
                                             invalidText: '时间格式无效'
                                         }),
                                          {
                                              xtype: 'tbtext',
                                              text: '- 下班时间',
                                              style: 'margin-top:4px'
                                          },
                                                new Ext.form.TimeField({
                                                    width: 70,
                                                    id: '2Type3StartTimeBM',
                                                    maxValue: '23:59',                      //最大时间   
                                                    maxText: '所选时间小于{0}',
                                                    minValue: '00:00',                      //最小时间   
                                                    minText: '所选时间大于{0}',
                                                    maxHeight: 180,
                                                    allowBlank: false,
                                                    editable: false,
                                                    value: "17:30",
                                                    increment: 15,                          //时间间隔   
                                                    format: 'G:i',                    //时间格式   
                                                    invalidText: '时间格式无效'
                                                })

					            ]
                                            },
                                            {
                                                anchor: '95%',
                                                height: 20,
                                                width: 500,
                                                style: 'border-bottom:1px dashed #DBDBDB',
                                                border: false
                                            },
                                            {
                                                xtype: 'compositefield',
                                                fieldLabel: '<span class="required">*</span>(晚班上)签到',
                                                combineErrors: false,
                                                style: 'margin-top:10px',
                                                items: [

                                        new Ext.form.TimeField({
                                            width: 70,
                                            id: '3Type3StartTimeBeginAM',
                                            maxValue: '23:59',                      //最大时间   
                                            maxText: '所选时间小于{0}',
                                            minValue: '00:00',                      //最小时间   
                                            minText: '所选时间大于{0}',
                                            maxHeight: 180,
                                            allowBlank: false,
                                            editable: false,
                                            value: "19:00",
                                            increment: 15,                          //时间间隔   
                                            format: 'G:i',                    //时间格式   
                                            invalidText: '时间格式无效'
                                        }),
                                             {
                                                 xtype: 'tbtext',
                                                 text: '~'
                                             },
                                             new Ext.form.TimeField({
                                                 width: 70,
                                                 id: '3Type3StartTimeEndAM',
                                                 maxValue: '23:59',                      //最大时间   
                                                 maxText: '所选时间小于{0}',
                                                 minValue: '00:00',                      //最小时间   
                                                 minText: '所选时间大于{0}',
                                                 maxHeight: 180,
                                                 allowBlank: false,
                                                 editable: false,
                                                 value: "20:00",
                                                 increment: 15,                          //时间间隔   
                                                 format: 'G:i',                    //时间格式   
                                                 invalidText: '时间格式无效'
                                             }),
                                              {
                                                  xtype: 'tbtext',
                                                  text: '- 上班时间',
                                                  style: 'margin-top:4px'
                                              },
                                                new Ext.form.TimeField({
                                                    width: 70,
                                                    id: '3Type3StartTimeAM',
                                                    maxValue: '23:59',                      //最大时间   
                                                    maxText: '所选时间小于{0}',
                                                    minValue: '00:00',                      //最小时间   
                                                    minText: '所选时间大于{0}',
                                                    maxHeight: 180,
                                                    allowBlank: false,
                                                    editable: false,
                                                    value: "19:00",
                                                    increment: 15,                          //时间间隔   
                                                    format: 'G:i',                    //时间格式   
                                                    invalidText: '时间格式无效'
                                                })

					                              ]
                                            },
                                            {
                                                xtype: 'compositefield',
                                                fieldLabel: '<span class="required">*</span>(晚班下)签退',
                                                combineErrors: false,
                                                style: 'margin-top:10px',
                                                items: [

                                            new Ext.form.TimeField({
                                                width: 70,
                                                id: '3Type3StartTimeBeginBM',
                                                maxValue: '23:59',                      //最大时间   
                                                maxText: '所选时间小于{0}',
                                                minValue: '00:00',                      //最小时间   
                                                minText: '所选时间大于{0}',
                                                maxHeight: 180,
                                                allowBlank: false,
                                                editable: false,
                                                value: "22:30",
                                                increment: 15,                          //时间间隔   
                                                format: 'G:i',                    //时间格式   
                                                invalidText: '时间格式无效'
                                            }),
                                         {
                                             xtype: 'tbtext',
                                             text: '~'
                                         },
                                         new Ext.form.TimeField({
                                             width: 70,
                                             id: '3Type3StartTimeEndBM',
                                             maxValue: '23:59',                      //最大时间   
                                             maxText: '所选时间小于{0}',
                                             minValue: '00:00',                      //最小时间   
                                             minText: '所选时间大于{0}',
                                             maxHeight: 180,
                                             allowBlank: false,
                                             editable: false,
                                             value: "23:00",
                                             increment: 15,                          //时间间隔   
                                             format: 'G:i',                    //时间格式   
                                             invalidText: '时间格式无效'
                                         }),
                                          {
                                              xtype: 'tbtext',
                                              text: '- 下班时间',
                                              style: 'margin-top:4px'
                                          },
                                                new Ext.form.TimeField({
                                                    width: 70,
                                                    id: '3Type3StartTimeBM',
                                                    maxValue: '23:59',                      //最大时间   
                                                    maxText: '所选时间小于{0}',
                                                    minValue: '00:00',                      //最小时间   
                                                    minText: '所选时间大于{0}',
                                                    maxHeight: 180,
                                                    allowBlank: false,
                                                    editable: false,
                                                    value: "22:00",
                                                    increment: 15,                          //时间间隔   
                                                    format: 'G:i',                    //时间格式   
                                                    invalidText: '时间格式无效'
                                                })

					            ]
                                            }
                                      ]
                                  }

                                ]
                        },
                         {
                             columnWidth: .4,
                             layout: 'form',
                             border: false,
                             bodyStyle: 'margin:5px;padding:14px;',
                             items: [
                                new Ext.Panel({
                                    anchor: '100%',
                                    contentEl: 'title'
                                })
                                ]
                         }
                    ]
               }
                ]
    });


    var viewport = new Ext.Viewport({
        layout: 'border',
        id: 'viewport',
        items: [
            {
                region: 'center',
                id: 'eastOrganization',
                bodyStyle: 'border-top:0px;border-bottom:0px',
                layout: 'fit',
                items: [form]
            },
            new Ext.Panel({
                region: 'west',
                layout: 'fit',
                collapsible: false,
                collapsed: false, //默认折叠
                collapseMode: 'mini', //出现小箭头
                split: true,
                width: 230,
                minSize: 175,
                bodyStyle: 'border-left:0px;border-top:0px;border-bottom:0px',
                deferredRender: false,
                items: [tree]
            })]
    });

});
