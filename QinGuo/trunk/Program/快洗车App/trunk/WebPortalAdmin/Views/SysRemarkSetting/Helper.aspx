<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="UrlContent" runat="server">
<!--百度编辑器-->
    <link href="../../Content/ueditor/themes/default/css/ueditor.css" rel="stylesheet"
        type="text/css" />
    <script src="<%=Url.Content("~/Content/ueditor/ueditor.all.js") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Content/ueditor/ueditor.config.js") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Content/ueditor/lang/zh-cn/zh-cn.js") %>" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        Ext.onReady(function () {
            var tb = new Ext.Toolbar();
            tb.add({
                text: '保存',
                iconCls: 'GTP_save',
                tooltip: '提交保存',
                handler: SaveDate
            });


            var form = new Ext.FormPanel({
                id: "formPanel",
                fileUpload: true, //有需文件上传的,需填写该属性
                border: false,
                labelAlign: 'left',
                labelWidth: 65,
                renderTo: Ext.getBody(),
                tbar: tb,
                //style: 'margin:5px',
                items: [
                        {
                            xytpe: 'panel',
                            layout: 'form',
                            border: false,
                            defaultType: 'textfield',
                            bodyStyle: 'padding:20px',
                            items: [
                                {
                                    fieldLabel: '<span class="required">*</span>帮助中心',
                                    xtype: 'textarea',
                                    style: 'margin-top:10px',
                                    id: 'txteditor',
                                    name: 'txteditor',
                                    allowBlank: true,
                                    height: 250,
                                    anchor: '90%'
                                }
                            ]
                        }

					]
            });

            var editor = new baidu.editor.ui.Editor({
                autoClearinitialContent: false, //自动清空内容区域
                URL: '~/UploadFile/ueditor/',
                initialContent: '', //初始化编辑器的内容            
                minFrameHeight: 200, //设置高度            
                textarea: 'txteditor' //设置提交时编辑器内容的名字，之前我们用的名字是默认的editorValue       
            });
            editor.render('txteditor'); //渲染

            Init();
        });

        //初始化
        function Init() {
            var NewsInfo = '<%= ViewData["Helper"] %>';
            var editor = UE.getEditor('txteditor');
            editor.ready(function () {//页面渲染完成后才能赋值
                editor.setContent(NewsInfo);
            })

        }

        //保存
        function SaveDate() {
            var formPanel = Ext.getCmp("formPanel");
            if (formPanel.getForm().isValid()) {//如果验证通过
                formPanel.getForm().submit({
                    waitTitle: '系统提示', //标题
                    waitMsg: '正在提交数据,请稍后...', //提示信息  
                    submitEmptyText: false,
                    url: '/SysRemarkSetting/SavaHelper', //记录表单提交的路径
                    method: "POST",
                    success: function (form, action) {
                        var flag = action.result; //成功后
                        if (flag.success) {
                            MessageInfo("保存成功！", "right");
                        } else {
                            MessageInfo(flag.msg, "error");
                        }
                    },
                    failure: function (form, action) {
                        var rs = eval('(' + action.response.responseText + ')');
                        MessageInfo(rs.msg, "error");
                    }
                });
            }
        }
    </script>

</asp:Content>
