<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

    <asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    </asp:Content>
    <asp:Content ID="Content3" ContentPlaceHolderID="UrlContent" runat="server">
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
        <script src="<%=Url.Content("~/Common/grid.js?version=1.3") %>" type="text/javascript"></script>
        <script src="<%=Url.Content("~/Project/NoticeNews/NoticeNewsEdit.js?version=1.3") %>"
            type="text/javascript"></script>
        <script language="javascript" type="text/javascript">

            Ext.onReady(function () {

                var north = new Ext.Panel({
                    region: 'north',
                    tbar: Toolbar(),
                    border: false
                });

                /*
                公告类型
                */
                var Model = new Ext.data.Store({
                    proxy: new Ext.data.HttpProxy({
                        url: '/SysGroup/GetGroup?Type=1',
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
                         fieldLabel: '<span class="required">*</span>公告标题',
                         xtype: 'textfield',
                         id: 'ActiveName',
                         maxLength: 50,
                         allowBlank: false,
                         emptyText: '请输入公告标题名称',
                         maxLengthText: '公告标题长度不能超过50个字符',
                         anchor: '60%'
                     },
                     {
                         xtype: 'combo',
                         triggerAction: 'all',
                         id: 'comActionFormId',
                         fieldLabel: '<span class="required">*</span>公告类型',
                         emptyText: '所属类型',
                         forceSelection: true,
                         editable: false,
                         allowBlank: false,
                         displayField: 'text',
                         valueField: 'id',
                         hiddenName: 'text',
                         anchor: '30%',
                         store: Model
                     },
                      {
                          xtype: 'radiogroup',
                          fieldLabel: '<span class="required">*</span>公告方式',
                          id: 'ActionType',
                          allowBlank: false,
                          width: 220,
                          items: [
						        { boxLabel: '无时间限制', name: 'ActionType', id: 'Type3', inputValue: 1, checked: true },
                                { boxLabel: '自动下架', name: 'ActionType', id: 'Type4', inputValue: 2 }
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
                          fieldLabel: '是否私信',
                          hidden: true,
                          items: [
                            {
                                xtype: 'checkbox',
                                fieldLabel: '是否私信',
                                id: 'ChkIsPersonalMsg',
                                checked: false,
                                listeners: {
                                    check: function (field, newValue, oldValue, eOpts) {
                                        if (newValue) {
                                            Ext.getCmp("txtPersonalName").allowBlank = false;
                                            Ext.getCmp("txtPersonalIdList").show();
                                            Ext.getCmp("ChkIsTop").hide();
                                            Ext.getCmp("ChkIsTop").setValue(false);
                                        }
                                        else {
                                            Ext.getCmp("txtPersonalName").allowBlank = true;
                                            Ext.getCmp("txtPersonalIdList").hide();
                                            Ext.getCmp("ChkIsTop").show();
                                            if (Ext.getCmp("txtPersonalName").getValue() == "") {
                                                Ext.getCmp("txtPersonalName").setValue("");
                                            }
                                        }
                                        Ext.getCmp("formPanel").doLayout();
                                    }
                                }
                            }
					     ]
                      },
                      {
                          xtype: 'compositefield',
                          fieldLabel: '私信人员',
                          hidden: true,
                          id: 'txtPersonalIdList',
                          items: [
                                            {
                                                xtype: 'textfield',
                                                name: 'txtPersonalId',
                                                id: 'txtPersonalId',
                                                hidden: true
                                            },
						                    {
						                        xtype: 'textfield',
						                        name: 'txtPersonalName',
						                        id: 'txtPersonalName',
						                        height: 22, //固定高度
						                        emptyText: '右边选择私信人员',
						                        allowBlank: true,
						                        width: 300,
						                        readOnly: true
						                    },
							                new Ext.Button({
							                    text: '选择',
							                    width: 40,
							                    handler: SelectJoinType
							                }),
                                             {
                                                 xtype: 'tbtext',
                                                 style: 'font-size:11px;margin-top:5px;',
                                                 text: ' (最多10人)'
                                             }
					                    ]
                      },
                   {
                       xtype: 'checkbox',
                       fieldLabel: '是否置顶',
                       id: 'ChkIsTop',
                       name: 'ChkIsTop',
                       anchor: '90%',
                       checked: false,
                       listeners: {
                           check: function (field, newValue, oldValue, eOpts) {

                           }
                       }
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
                    url = '/NoticeNews/SaveData?Id=' + Id + '&modify=1';
                    var respon = Ext.lib.Ajax.getConnectionObject().conn;
                    respon.open("post", "/NoticeNews/LoadData?Id=" + Id, false);
                    respon.send(null);
                    var result = Ext.util.JSON.decode(respon.responseText);
                    if (result) {
                        Ext.getCmp("ActionType").setValue(result.ActionType); //公告类型
                        Ext.getCmp("ActiveName").setValue(result.ActiveName);
                        var editor = UE.getEditor('Info');
                        editor.ready(function () {//页面渲染完成后才能赋值
                            editor.setContent(result.Info);
                        })
                        switch (result.ActionType) {
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
                        Ext.getCmp("comActionFormId").setValue(result.ActionFormId); //value
                        Ext.getCmp("comActionFormId").setRawValue(result.ActionTypeName); //text

                        //是否私信
                        Ext.getCmp("ChkIsPersonalMsg").setValue(result.IsPersonalMsg);
                        if (result.IsPersonalMsg) {
                            Ext.getCmp("txtPersonalId").setValue(result.PersonalId);
                            Ext.getCmp("txtPersonalName").setValue(result.PersonalName);
                        }
                        //是否置顶
                        Ext.getCmp("ChkIsTop").setValue(result.IsTop);
                    }
                }
                else {
                    url = '/NoticeNews/SaveData?1=1';
                }
                Ext.getCmp("formPanel").doLayout();
            }

            //保存数据
            function SaveDate() {
                var formPanel = Ext.getCmp("formPanel");
                if (formPanel.getForm().isValid()) {//如果验证通过
                    //查看公告方式
                    var AdType = Ext.getCmp("ActionType").getValue().inputValue;
                    if (AdType == 2) {
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
                        url += "&ActiveStartTime=" + sbeginDate + "&ActiveEndTime=" + sendDate; //传递时间
                    }
                    //获取公告类型
                    var comModel = Ext.getCmp("comActionFormId").getValue();
                    url += "&ActionFormId=" + comModel;
                    // var para = { ActionFormId: comModel };

                    var ChkIsTop = Ext.getCmp("ChkIsTop").getValue();
                    var IsPersonalMsg = Ext.getCmp("ChkIsPersonalMsg").getValue();
                    if (IsPersonalMsg) {
                        var txtPersonalId = Ext.getCmp("txtPersonalId").getValue();
                        url += "&IsTop=" + ChkIsTop + "&IsPersonalMsg=" + IsPersonalMsg + "&PersonalId=" + txtPersonalId;
                    }
                    else {
                        url += "&IsTop=" + ChkIsTop + "&IsPersonalMsg=" + IsPersonalMsg;
                    }

                    formPanel.getForm().submit({
                        waitTitle: '系统提示', //标题
                        waitMsg: '正在提交数据,请稍后...', //提示信息  
                        submitEmptyText: false,
                        url: url, //记录表单提交的路径
                        method: "POST",
                        //params: para,
                        success: function (form, action) {
                            var flag = action.result; //成功后
                            if (flag.success) {

                                url = flag.msg;
                                MessageInfo("提交成功！", "right");
                            } else {
                                if (url.indexOf('modify') > 0) {
                                    var ID = '<%= ViewData["Id"] %>';
                                    url = '/NoticeNews/SaveData?Id=' + Id + '&modify=1';
                                } else {
                                    url = '/NoticeNews/SaveData?1=1';
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
                                url = '/NoticeNews/SaveData?Id=' + Id + '&modify=1';
                            } else {
                                url = '/NoticeNews/SaveData?1=1';
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
