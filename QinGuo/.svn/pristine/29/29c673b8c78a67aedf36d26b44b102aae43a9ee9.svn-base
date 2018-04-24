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

    <script language="javascript"  type="text/javascript">
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
                labelAlign: 'right',
                labelWidth: 85,
                renderTo: Ext.getBody(),
                tbar: tb,
                items: [
                       {
                           xtype: "fieldset",
                           autoHeight: true,
                           title: "公司信息",
                           style: 'margin:10px',
                           items: [
                               {
                                   xytpe: 'panel',
                                   layout: 'form',
                                   border: false,
                                   defaultType: 'textfield',
                                   items: [
                                        {
                                            name: 'NameTitle',
                                            id: 'NameTitle',
                                            fieldLabel: '公司名称',
                                            emptyText: '填写公司名称', ////textfield自己的属性
                                            xtype: 'textfield',
                                            maxLength: 50,
                                            allowBlank: false,
                                            anchor: '50%',
                                            maxLengthText: '公司名称长度不能超过50个字符'
                                        },
                                        {
                                            name: 'Address',
                                            id: 'Address',
                                            fieldLabel: '公司地址',
                                            emptyText: '填写公司地址', ////textfield自己的属性
                                            xtype: 'textfield',
                                            maxLength: 20,
                                            anchor: '50%',
                                            maxLengthText: '车子品牌长度不能超过20个字符'
                                        },
                                        {
                                            name: 'Phone',
                                            id: 'Phone',
                                            fieldLabel: '客服电话',
                                            emptyText: '填写联系电话', ////textfield自己的属性
                                            xtype: 'textfield',
                                            maxLength: 20,
                                            anchor: '50%',
                                            maxLengthText: '联系电话长度不能超过20个字符'
                                        }
                            ]
                               }
        					]
                                },
                       {
                           xtype: "fieldset",
                           autoHeight: true,
                           title: "系统信息",
                           style: 'margin:10px',
                           items: [
                               {
                                   xytpe: 'panel',
                                   layout: 'form',
                                   border: false,
                                   defaultType: 'textfield',
                                   items: [
                                   {
                                       name: 'ReegistMoney',
                                         fieldLabel: '年费设置',
                                         xtype: 'numberfield',
                                         id: 'ReegistMoney',
                                         minValue: 0,
                                         maxValue: 99999999,
                                         width: 240,
                                         allowDecimals: false,
                                         anchor: '50%'
                                     }
                            ]
                               }
        					]
                       }
                     ]
                           });

           Init();
        });

        //初始化
        function Init() {
            var NameTitle = '<%=ViewData["NameTitle"] %>';
            var Address = '<%=ViewData["Address"] %>';
            var Phone = '<%=ViewData["Phone"] %>';
            var ReegistMoney = '<%=ViewData["Money"] %>';

            Ext.getCmp("NameTitle").setValue(NameTitle);
            Ext.getCmp("Address").setValue(Address);
            Ext.getCmp("Phone").setValue(Phone);
            Ext.getCmp("ReegistMoney").setValue(ReegistMoney);
        }


        //保存
        function SaveDate() {
            var formPanel = Ext.getCmp("formPanel");
            if (formPanel.getForm().isValid()) {//如果验证通过
                formPanel.getForm().submit({
                    waitTitle: '系统提示', //标题
                    waitMsg: '正在提交数据,请稍后...', //提示信息  
                    submitEmptyText: false,
                    url: '/KeyWords/SavaSetting', //记录表单提交的路径
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
