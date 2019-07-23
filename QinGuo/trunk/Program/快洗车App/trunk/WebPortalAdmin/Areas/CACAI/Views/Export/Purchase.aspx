<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="UrlContent" runat="server">
 <!--引用fileUpLoad-->
    <link href="<%=Url.Content("~/Content/Extjs/ux/fileuploadfield/css/fileuploadfield.css")%>"
        rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="<%=Url.Content("~/Content/Extjs/ux/fileuploadfield/FileUploadField.js") %>"></script>
    <script language="javascript" type="text/javascript">
        Ext.onReady(function () {
            var tb = new Ext.Toolbar();
            tb.add({
                text: '开始导入',
                iconCls: 'GTP_submit',
                tooltip: '提交导入',
                handler: SaveDate
            });
            tb.add('-');
            tb.add({
                text: '关闭',
                iconCls: 'GTP_cancel',
                tooltip: '关闭取消',
                handler: function (sender) {
                    CloseWindow();
                }
            });

            //下载路径
            var downMoBanUlr = "/Project/Template/采购退货单模板.xlsx";

            var form = new Ext.FormPanel({
                id: "formPanel",
                fileUpload: true, //有需文件上传的,需填写该属性
                border: false,
                labelAlign: 'left',
                renderTo: Ext.getBody(),
                tbar: tb,
                labelWidth: 60,
                items: [
                        {
                            xytpe: 'panel',
                            layout: 'form',
                            border: false,
                            defaultType: 'textfield',
                            bodyStyle: 'padding:30px',
                            items: [
                                 {
                                     id: 'moban',
                                     name: 'moban',
                                     bodyStyle: 'border:0px;',
                                     fieldLabel: '导入模板',
                                     style: 'padding-top:3px;',
                                     xtype: 'panel',
                                     width: 300,
                                     anchor: '90%',
                                     html: '您可以在这里下载&nbsp;<a target=blank href="' + downMoBanUlr + '">模板下载</a>'
                                 },
                                {
                                    xtype: 'tbtext',
                                    text: '<br/>'
                                },
                                  {
                                      xtype: 'compositefield',
                                      fieldLabel: '附件文件',
                                      id: 'compositefieldPic',
                                      combineErrors: false,
                                      anchor: '85%',
                                      items: [

                                                    {
                                                        xtype: 'fileuploadfield',
                                                        id: 'excel',
                                                        emptyText: '只能上传Excel格式文件',
                                                        name: 'excel',
                                                        buttonText: '',
                                                        allowBlank: false,
                                                        buttonCfg: {
                                                            iconCls: 'image_add',
                                                            tooltip: '附件选择'
                                                        },
                                                        width: 400,
                                                        listeners: {
                                                            'fileselected': {
                                                                fn: this.ExcelUploadAction,
                                                                scope: this
                                                            }
                                                        }
                                                    }
						                        ]
                                  },
                                {
                                    xtype: 'tbtext',
                                    text: '<br/>'
                                },

                              {
                                  fieldLabel: '提示说明',
                                  xtype: 'panel',
                                  border: false,
                                  width: 300,
                                  style: 'padding-top:4px;color:orange;font-size:13px;padding:1px;',
                                  anchor: '90%',
                                  html: '1 请按照模板要求进行填写，否则导入会不成功。<br/>'
                                         + '2 退货单号，仓库，供应商不能为空。<br/>'
                                         + '3 退货状态为已退货和未退货。<br/>'
                                         + '4 金额请输入有效数字格式。<br/>'
                              },
                            {
                                name: 'downerror',
                                style: 'color:Red',
                                bodyStyle: 'border:0px;',
                                id: 'downerror',
                                fieldLabel: '信息提示',
                                style: 'padding-left:5px;padding-top:3px;',
                                xtype: 'panel',
                                width: 300,
                                anchor: '90%',
                                hidden: true,
                                html: ''
                            }

                            ]
                        }
					]
            });
        });

    
        //保存后关闭
        function CloseWindow() {
            var id = parent.GetActiveTabId();
            if (id) {
                parent.CloseTabPanel(id); //传递页面id
            }
        }

        //保存
        function SaveDate() {
            var result = [];
            var formPanel = Ext.getCmp("formPanel");
            if (formPanel.getForm().isValid()) {//如果验证通过
                formPanel.getForm().submit({
                    waitTitle: '系统提示', //标题
                    waitMsg: '系统正在导入,请稍后...', //提示信息  
                    submitEmptyText: false,
                    url: '/CACAI/Export/ImportDate?CType=3', //记录表单提交的路径
                    method: "POST",
                    success: function (form, action) {
                        var a = '';
                        var flag = action.result; //成功后
                        if (flag.success) {
                            a = '<a style="color:green">导入成功</a>';
                            MessageInfo("导入成功！", "right");
                        } else {
                            MessageInfo(flag.msg, "error");
                        }


                        if (flag.data.length > 0) {
                            a += '<a style="color:Red" target=blank href="' + flag.data + '">下载错误数据</a><br/>';
                            a += '(' + flag.msg + ')';
                        }

                        Ext.getCmp('downerror').show();
                        Ext.getCmp("downerror").update(a);
                    },
                    failure: function (form, action) {
                        var flag = action.response.responseText; //成功后
                        flag = eval('(' + flag + ')');
                        var a = '';
                        if (flag.success) {
                            a = '<a style="color:green">导入成功</a>';
                            MessageInfo("导入成功！", "right");
                        }
                        else {
                            MessageInfo(flag.msg, "error");
                        }
                        if (flag.data.length > 0) {
                            a += '<a style="color:Red" target=blank href="' + flag.data + '">下载错误数据</a><br/>';
                            a += '(' + flag.msg + ')';
                            Ext.getCmp('downerror').show();
                            Ext.getCmp("downerror").update(a);
                        }
                    }
                });
            }
        }

    </script>
</asp:Content>
