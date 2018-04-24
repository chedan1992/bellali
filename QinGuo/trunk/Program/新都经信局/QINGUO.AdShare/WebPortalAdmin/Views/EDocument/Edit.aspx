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
    <script src="<%=Url.Content("~/Project/EDocument/EDocumentEdit.js?version=1.5") %>" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        Ext.onReady(function () {

            var north = new Ext.Panel({
                region: 'north',
                tbar: Toolbar(),
                border: false
            });

            var BrandId = '<%=ViewData["BrandId"] %>';

            /*
            型号信息
            */
            var Model = new Ext.data.Store({
                proxy: new Ext.data.HttpProxy({
                    url: '/SysModel/LoadModel?code=' + BrandId,
                    method: 'POST'
                }),
                reader: new Ext.data.JsonReader({},
                        Ext.data.Record.create(["Id", "ModelName"]
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
                         name: 'Name',
                         fieldLabel: '<span class="required">*</span>文档标题',
                         xtype: 'textfield',
                         id: 'Name',
                         maxLength: 50,
                         allowBlank: false,
                         emptyText: '请输入文档标题名称',
                         maxLengthText: '文档标题长度不能超过50个字符',
                         anchor: '60%'
                     },
                     {
                         name: 'Mark',
                         fieldLabel: '文档简介',
                         xtype: 'textfield',
                         id: 'Mark',
                         maxLength: 100,
                         emptyText: '请输入文档文章简介',
                         maxLengthText: '文档简介长度不能超过100个字符',
                         anchor: '60%'
                     },
                     {
                         xtype: 'compositefield',
                         fieldLabel: '文档图',
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
                                emptyText: '可上传文档标识图',
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
                         xtype: 'tbtext',
                         style: 'margin-left:80px;font-size:11px;color:Green;margin-bootom:5px;',
                         text: '(建议尺寸:640px*320px,大小:500kb)'
                     },
                     {
                         xtype: 'combo',
                         triggerAction: 'all',
                         id: 'comModel',
                         name: 'comModel',
                         fieldLabel: '<span class="required">*</span>所属型号',
                         emptyText: '所属型号',
                         forceSelection: true,
                         editable: false,
                         allowBlank: false,
                         displayField: 'ModelName',
                         valueField: 'Id',
                         hiddenName: 'ModelName',
                         anchor: '60%',
                         store: Model
                     },
                     {
                         name: 'ReadNum',
                         fieldLabel: '阅读量',
                         xtype: 'numberfield',
                         id: 'ReadNum',
                         minValue: 0,
                         maxValue: 99999,
                         width: 240,
                         allowDecimals: false,
                         anchor: '80%'
                     },
                     {
                         fieldLabel: '<span class="required">*</span>描述信息',
                         xtype: 'textarea',
                         id: 'Content',
                         name: 'Content',
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
                textarea: 'Content' //设置提交时编辑器内容的名字，之前我们用的名字是默认的editorValue       
            });
            editor.render('Content'); //渲染

            initVisible(); //初始化
        });


        //初始化
        function initVisible() {
            var Id = '<%=ViewData["Id"] %>';
            var LookView = '<%=ViewData["LookView"] %>';
            var BrandId = '<%=ViewData["BrandId"] %>';
            if (LookView != "") {
                if (Ext.getCmp("GTP_save")) {
                    Ext.getCmp("GTP_save").hide();
                }
            }
            if (Id) {//编辑
                url = '/EDocument/SaveData?Id=' + Id + '&modify=1&BrandId=' + BrandId;
                var respon = Ext.lib.Ajax.getConnectionObject().conn;
                respon.open("post", "/EDocument/LoadData?Id=" + Id, false);
                respon.send(null);
                var result = Ext.util.JSON.decode(respon.responseText);
                if (result) {
                    Ext.getCmp("Name").setValue(result.Name);
                    if (result.Img != null) {//标识图展示
                        Ext.getCmp("Img").setValue(result.Img);
                        SetPicView(result.Img);
                    }
                    Ext.getCmp("ReadNum").setValue(result.ReadNum);
                    Ext.getCmp("Mark").setValue(result.Mark);

                    //型号
                    Ext.getCmp("comModel").setValue(result.ModelId); //value
                    Ext.getCmp("comModel").setRawValue(result.ModelName); //text

                    Ext.getCmp("comModel").getStore().proxy = new Ext.data.HttpProxy({ url: '/SysModel/LoadModel?code=' + result.BrandId, method: 'POST' });
                    Ext.getCmp("comModel").getStore().load();

                    var editor = UE.getEditor('Content');
                    editor.ready(function () {//页面渲染完成后才能赋值
                        editor.setContent(result.Content);
                    })
                }
            }
            else {
                url = '/EDocument/SaveData?1=1&BrandId=' + BrandId;
            }
            Ext.getCmp("formPanel").doLayout();
        }

        //保存数据
        function SaveDate() {
            var formPanel = Ext.getCmp("formPanel");
            var comModel =Ext.getCmp("comModel").getValue();
            var para = { ModelId: comModel};
            if (formPanel.getForm().isValid()) {//如果验证通过
                formPanel.getForm().submit({
                    waitTitle: '系统提示', //标题
                    waitMsg: '正在提交数据,请稍后...', //提示信息  
                    submitEmptyText: false,
                    url: url, //记录表单提交的路径
                    params: para,
                    method: "POST",
                    success: function (form, action) {
                        var flag = action.result; //成功后
                        if (flag.success) {
                            url = flag.msg;
                            MessageInfo("提交成功！", "right");
                        } else {
                            if (url.indexOf('modify') > 0) {
                                var ID = '<%= ViewData["Id"] %>';
                                url = '/EDocument/SaveData?Id=' + Id + '&modify=1';
                            } else {
                                url = '/EDocument/SaveData?1=1';
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
                            url = '/EDocument/SaveData?Id=' + Id + '&modify=1';
                        } else {
                            url = '/EDocument/SaveData?1=1';
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
