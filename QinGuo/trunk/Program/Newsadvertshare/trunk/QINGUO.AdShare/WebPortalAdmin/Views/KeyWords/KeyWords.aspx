<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
 <div id="msg_info" class="msg_info x-hide-display">
        注: 敏感词相隔请用 , 号 隔开
    </div>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="UrlContent" runat="server">
<style type="text/css">
        .msg_info
        {
            font-size:12px;
            text-align: left;
            padding:5px 10px 5px 32px;
            border: 1px dashed #f6981e;
            background-color: #ffc;
            background-position: 10px 8px;
            background-repeat: no-repeat;
            -moz-border-radius: 3px;
            -webkit-border-radius: 3px;
            border-radius: 3px;
            background-image:url(../../Resource/css/icons/info.gif); background-position:10px 5px; 
        }
    </style>
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
                labelWidth:85,
                renderTo: Ext.getBody(),
                tbar: tb,
                //style: 'margin:5px',
                items: [
                         {
                             xtype: 'panel',
                             border: false,
                             contentEl: 'msg_info'
                         },
                        {
                            xytpe: 'panel',
                            layout: 'form',
                            border: false,
                            defaultType: 'textfield',
                            bodyStyle: 'padding:20px',
                            items: [
                                {
                                    fieldLabel: '<span class="required">*</span>敏感词设置',
                                    xtype: 'textarea',
                                    //style: 'margin-top:7px',
                                    id: 'txteditor',
                                    name: 'txteditor',
                                    allowBlank: true,
                                    height: 250,
                                    anchor: '90%',
                                    value: '<%= ViewData["KeyWords"] %>'
                                }
                            ]
                        }

					]
            });
        });

       
        //保存
        function SaveDate() {
            var formPanel = Ext.getCmp("formPanel");
            if (formPanel.getForm().isValid()) {//如果验证通过
                formPanel.getForm().submit({
                    waitTitle: '系统提示', //标题
                    waitMsg: '正在提交数据,请稍后...', //提示信息  
                    submitEmptyText: false,
                    url: '/KeyWords/SavaKeyWords', //记录表单提交的路径
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
