<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="UrlContent" runat="server">
    <!--百度编辑器-->
    <style type="text/css">
        .top
        {
            font-family: "微软雅黑";
            font-size: 24px;
            font-weight: bold;
            color: #c1c1c1;
        }
        .MicrositeTxt
        {
            margin-right: 50px;
            margin-top: 20px;
            float: right;
            width: 260px;
            min-height: 450px;
            text-align: center;
        }
        .footer
        {
            background-color: #f9f9f9;
            margin: 0 0 0 0;
            padding: 0px;
        }
        #pn td
        {
            padding: 0px;
        }
    </style>
    <link href="../../Content/ueditor/themes/default/css/ueditor.css" rel="stylesheet"
        type="text/css" />
    <script src="<%=Url.Content("~/Content/ueditor/ueditor.all.js") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Content/ueditor/ueditor.config.js") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Content/ueditor/lang/zh-cn/zh-cn.js") %>" type="text/javascript"></script>
    <!--引用fileUpLoad-->
    <script src="<%=Url.Content("~/Content/Extjs/ux/fileuploadfield/FileUploadField.js") %>"
        type="text/javascript"></script>
    <link href="../../Content/Extjs/ux/fileuploadfield/css/fileuploadfield.css" rel="stylesheet"
        type="text/css" />
    <!--文件区域-->
    <script src="../../Content/Extjs/VTypes/ExtVTypes.js" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Common/grid.js?version=1.5") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Project/Advertise/AdvertiseEdit.js?version=1.5") %>"
        type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        Ext.onReady(function () {
            var north = new Ext.Panel({
                region: 'north',
                tbar: Toolbar(),
                border: false
            });
            var form = new Ext.FormPanel({
                id: "formPanel",
                fileUpload: true, //有需文件上传的,需填写该属性
                border: false,
                labelAlign: 'right',
                autoScroll: true,
                labelWidth: 75,
                bodyStyle: 'margin:5px;padding:15px;',
                items: [
                     {
                         name: 'ActionFormId',
                         fieldLabel: '关联字段',
                         xtype: 'textfield',
                         id: 'ActionFormId',
                         hidden: true
                     },
                     {
                         name: 'ActiveName',
                         fieldLabel: '<span class="required">*</span>广告标题',
                         xtype: 'textfield',
                         id: 'ActiveName',
                         maxLength: 50,
                         allowBlank: false,
                         emptyText: '请输入标题名称',
                         maxLengthText: '标题长度不能超过50个字符',
                         anchor: '60%'
                     },
                      {
                          xtype: 'radiogroup',
                          fieldLabel: '<span class="required">*</span>展示方式',
                          id: 'ShowType',
                          allowBlank: false,
                          width: 220,
                          items: [
						        { boxLabel: '无时间限制', name: 'ShowType', id: 'ShowType1', inputValue: 1, checked: true },
                                { boxLabel: '自动下架', name: 'ShowType', id: 'ShowType2', inputValue: 2 }
                            ],
                          listeners: {
                              'change': function (checked) {
                                  ChangeType();
                              }
                          }
                      },
                     {
                         xtype: 'compositefield',
                         fieldLabel: '<span class="required">*</span>有效时间',
                         id: 'timeLimit',
                         hidden: true,
                         combineErrors: false,
                         items: [
							    {
							        xtype: 'textfield',
							        hidden: true
							    },
                                {
                                    name: 'StartDate',
                                    id: 'StartDate',
                                    xtype: 'datefield',
                                    allowBlank: false,
                                    value: new Date(),
                                    width: 130,
                                    emptyText: '选择开始时间',
                                    format: 'Y-m-d(周l)',
                                    vtype: 'daterange',
                                    endDateField: 'EndDate'
                                },
                                 new Ext.form.TimeField({
                                     width: 70,
                                     id: 'StartTime',
                                     maxValue: '23:59',                      //最大时间   
                                     maxText: '所选时间小于{0}',
                                     minValue: '00:00',                      //最小时间   
                                     minText: '所选时间大于{0}',
                                     maxHeight: 180,
                                     allowBlank: false,
                                     value: new Date().getHours() + 1,
                                     increment: 15,                          //时间间隔   
                                     format: 'G:i',                    //时间格式   
                                     invalidText: '时间格式无效'
                                 }),
                                {
                                    xtype: 'tbtext',
                                    text: '~'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'EndDate',
                                    id: 'EndDate',
                                    value: new Date((new Date() / 1000 + 86400 * 10) * 1000),
                                    allowBlank: false,
                                    format: 'Y-m-d(周l)',
                                    emptyText: '选择结束时间',
                                    width: 130,
                                    vtype: 'daterange',
                                    startDateField: 'StartDate'
                                },
                                    new Ext.form.TimeField({
                                        width: 70,
                                        id: 'EndTime',
                                        maxValue: '23:59',                      //最大时间   
                                        maxText: '所选时间小于{0}',
                                        minValue: '00:00',                      //最小时间   
                                        minText: '所选时间大于{0}',
                                        maxHeight: 180,
                                        allowBlank: false,
                                        value: new Date().getHours(),
                                        increment: 15,                          //时间间隔   
                                        format: 'G:i',                    //时间格式   
                                        invalidText: '时间格式无效'
                                    })
					            ]
                     },
                                 {
                                     xtype: 'compositefield',
                                     fieldLabel: '广告图',
                                     id: 'compositefieldPic',
                                     combineErrors: false,
                                     items: [
							{
							    fieldLabel: '',
							    hidden: true,
							    xtype: 'textfield'
							},
                            {
                                xtype: 'fileuploadfield',
                                id: 'Img',
                                emptyText: '可上传广告标识图',
                                name: 'Img',
                                buttonText: '',
                                buttonCfg: {
                                    iconCls: 'image_add',
                                    tooltip: '图片选择'
                                },
                                width: 400,
                                listeners: {
                                    'fileselected': {
                                        fn: this.FileUploadAction,
                                        scope: this
                                    }
                                }
                            },

                            {
                                xtype: 'box', //或者xtype: 'component',
                                id: 'browseImage',
                                width: 60, //图片宽度
                                height: 25, //图片高度
                                autoEl: {
                                    tag: 'div',    //指定为img标签
                                    style: "filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=scale);"
                                }
                            },
                            {
                                xtype: 'box', //或者xtype: 'component',
                                id: 'browseImage_img',
                                width: 60, //图片宽度
                                hidden: true,
                                height: 25, //图片高度
                                autoEl: {
                                    tag: 'img',    //指定为img标签
                                    src: '../../Content/Extjs/resources/images/default/s.gif'
                                }
                            }
						]
                                 },
                     {
                         xtype: 'radiogroup',
                         fieldLabel: '<span class="required">*</span>广告类型',
                         id: 'ActionType',
                         allowBlank: false,
                         width: 220,
                         items: [
						        { boxLabel: '内部广告', name: 'ActionType', id: 'ActionType1', inputValue: 1, checked: true },
                                { boxLabel: '外部广告', name: 'ActionType', id: 'ActionType2', inputValue: 2 },
                                { boxLabel: '资讯广告', name: 'ActionType', id: 'ActionType3', inputValue: 3 }
                            ],
                         listeners: {
                             'change': function (checked) {
                                 ChangeActionType();
                             }
                         }
                     },

                     {
                         name: 'Link',
                         fieldLabel: '<span class="required">*</span>链接地址',
                         xtype: 'textfield',
                         id: 'Link',
                         hidden: true,
                         allowBlank: true,
                         emptyText: '请输入链接地址(完整链接地址需含http 如:http://www.baidu.com)',
                         anchor: '60%'
                     },
                     {
                            xtype: 'compositefield',
                            fieldLabel: '<span class="required">*</span>行业资讯',
                            combineErrors: false,
                            hidden: true,
                            id: 'NewList',
                            width: 350,
                                items: [
						        {
						            name: 'RelateShopName',
						            xtype: 'textfield',
						            flex: 1,
						            height: 22, //固定高度
						            id: 'RelateName',
						            emptyText: '右边选择资讯信息',
						            allowBlank: true,
						            readOnly: true
						        },
							        new Ext.Button({
							            text: '选择',
							            flex: 1,
							            width: 40,
							            handler: openMulit
							        })
					        ]
                        },
                     {
                         fieldLabel: '<span class="required">*</span>描述信息',
                         xtype: 'textarea',
                         id: 'Info',
                         name: 'Info',
                         allowBlank: true,
                         height: 250,
                         anchor: '90%'
                     }
                ]
            });
            var viewport = new Ext.Viewport({
                layout: 'border',
                id: 'viewMain',
                items: [
                        north,
                        new Ext.Panel({
                            region: 'center',
                            border: false,
                            layout: 'fit',
                            deferredRender: false,
                            items: [form]
                        })
                ]
            });

            var editor = new baidu.editor.ui.Editor({
                autoClearinitialContent: false,
                initialContent: '', //初始化编辑器的内容            
                minFrameHeight: 300, //设置高度            
                textarea: 'Info' //设置提交时编辑器内容的名字，之前我们用的名字是默认的editorValue       
            });
            editor.render('Info'); //渲染

            initVisible(); //初始化
        });


        //初始化
        function initVisible() {
            var Id = '<%=ViewData["Id"] %>';
            if (Id) {//编辑
                url = '/Advertise/SaveData?Id=' + Id + '&modify=1';
                var respon = Ext.lib.Ajax.getConnectionObject().conn;
                respon.open("post", "/Advertise/LoadData?Id=" + Id, false);
                respon.send(null);
                var result = Ext.util.JSON.decode(respon.responseText);
                if (result) {
                    Ext.getCmp("ActiveName").setValue(result.ActiveName); //value
                    Ext.getCmp("ShowType").setValue(result.ShowType); //展示方式
                    Ext.getCmp("ActionType").setValue(result.ActionType); //广告类型
                   
                    switch (result.ShowType) {
                        case 1: //无时间限制
                            break;
                        case 2: //自动下架
                            var StartDate = new Date(formartTime(result.ActiveStartTime)).format('Y-m-d');
                            var StartTime = new Date(formartTime(result.ActiveStartTime)).format('H:i');
                            var EndDate = new Date(formartTime(result.ActiveEndTime)).format('Y-m-d');
                            var EndTime = new Date(formartTime(result.ActiveEndTime)).format('H:i');
                            Ext.getCmp("StartDate").setValue(StartDate);
                            Ext.getCmp("StartTime").setValue(StartTime);
                            Ext.getCmp("EndDate").setValue(EndDate);
                            Ext.getCmp("EndTime").setValue(EndTime);
                            break;
                    };
                    if (result.Img != null) {//标识图展示
                        if (result.Img.indexOf('null') == -1) {
                            Ext.getCmp("Img").setValue(result.Img);
                            SetPicView(result.Img);
                        }
                    }
                    switch (result.ActionType) {
                        case 1:
                            var editor = UE.getEditor('Info');
                            editor.ready(function () {//页面渲染完成后才能赋值
                                editor.setContent(result.Info);
                            })
                            break;
                        case 2:
                            Ext.getCmp("Link").setValue(result.Info); 
                            break;
                        case 3:
                            Ext.getCmp("RelateName").setValue(result.ActionFormName);
                            Ext.getCmp("ActionFormId").setValue(result.ActionFormId); 
                            break;
                    }
                }
            }
            else {
                url = '/Advertise/SaveData?1=1';
            }
            Ext.getCmp("formPanel").doLayout();
        }

        //保存数据
        function SaveDate() {
            var formPanel = Ext.getCmp("formPanel");
            if (formPanel.getForm().isValid()) {//如果验证通过
                //查看广告类型
                var ActionType = Ext.getCmp("ActionType").getValue().inputValue;
                //展示方式
                var ShowType = Ext.getCmp("ShowType").getValue().inputValue;

                var para = {  };

                if (ShowType == 2) {
                    //组合时间
                    var BeginTime = Ext.getCmp("StartTime").getValue();
                    var bHours = BeginTime.split(':')[0].length > 1 ? BeginTime.split(':')[0] : "0" + BeginTime.split(':')[0]; //小时
                    var bMiuns = BeginTime.split(':')[1]; //分钟
                    BeginTime = bHours + ":" + bMiuns;

                    var EndTime = Ext.getCmp("EndTime").getValue();
                    var eHours = EndTime.split(':')[0].length > 1 ? EndTime.split(':')[0] : "0" + EndTime.split(':')[0]; //小时
                    var eMiuns = EndTime.split(':')[1]; //分钟
                    EndTime = eHours + ":" + eMiuns;

                    var sbeginDate = Ext.getCmp("StartDate").getValue().format('Y-m-d') + ' ' + BeginTime;
                    var sendDate = Ext.getCmp("EndDate").getValue().format('Y-m-d') + ' ' + EndTime;
                    var beginDate = Date.parseDate(sbeginDate, "c"); //开始时间
                    var endDate = Date.parseDate(sendDate, "c"); //结束时间

                    if (endDate <= beginDate) {
                        MessageInfo("结束时间需要大于开始时间！", "statusing");
                        return false;
                    }
                    para = { ActiveStartTime: sbeginDate, ActiveEndTime: sendDate };
                }
               
                formPanel.getForm().submit({
                    waitTitle: '系统提示', //标题
                    waitMsg: '正在提交数据,请稍后...', //提示信息  
                    submitEmptyText: false,
                    url: url, //记录表单提交的路径
                    method: "POST",
                    params: para,
                    success: function (form, action) {
                        var flag = action.result; //成功后
                        if (flag.success) {

                            url = flag.msg;
                            MessageInfo("提交成功！", "right");
                        } else {
                            if (url.indexOf('modify') > 0) {
                                var ID = '<%= ViewData["Id"] %>';
                                url = '/Advertise/SaveData?Id=' + Id + '&modify=1';
                            } else {
                                url = '/Advertise/SaveData?1=1';
                            }
                            top.Ext.MessageBox.show({
                                title: '信息提示',
                                msg: flag.msg,
                                buttons: Ext.MessageBox.OK,
                                icon: Ext.MessageBox.INFO
                            });
                        }
                        CloseWindow();
                    },
                    failure: function (form, action) {
                        if (url.indexOf('modify') > 0) {
                            var ID = '<%= ViewData["Id"] %>';
                            url = '/Advertise/SaveData?Id=' + Id + '&modify=1';
                        } else {
                            url = '/Advertise/SaveData?1=1';
                        }
                        var rs = eval('(' + action.response.responseText + ')');
                        top.Ext.MessageBox.show({
                            title: '信息提示',
                            msg: rs.msg,
                            buttons: Ext.MessageBox.OK,
                            icon: Ext.MessageBox.INFO
                        });
                        return false
                    }
                });
            }
        }
    </script>
</asp:Content>
