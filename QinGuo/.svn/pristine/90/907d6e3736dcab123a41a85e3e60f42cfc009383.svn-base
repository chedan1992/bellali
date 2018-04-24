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

            //下载路劲
            var downMoBanUlr = "/Project/Template/设备模版.xls";

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
                                     style: 'padding-top:3px;color:blue',
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
                                  style: 'padding-top:4px;color:#acacac',
                                  anchor: '90%',
                                  html: '1  设备编号：导入的设备已经存在二维码信息，需要填写二维码编号即设备编号,如果不存在,默认为空<br/>'
                                       + '2  设备名称：必填<br/>'
                                       + '3  设备位置：必填<br/>'
                                       + '4  设备型号：必填<br/>'
                                       + '5  设备规格：必填<br/>'
                                       + '6  设备数量：选填(数量，为空默认为0)<br/>'
                                       + '7  生产日期：必填(日期类型)<br/>'
                                       + '8  维修日期：选填(日期类型)<br/>'
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

        //验证文件上传
        function ExcelUploadAction(o) {
            //验证同文件的正则  
            var img_reg = /\.([xX][lL][sS]){1}$$/;
            if (!img_reg.test(o.value)) {
                top.Ext.Msg.show({ title: "信息提示", msg: "文件类型错误,请选择文件(xls)", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                o.setRawValue('');
                return;
            }
        }

        //保存后关闭
        function CloseWindow() {
            var id = parent.GetActiveTabId();
            if (id) {
                parent.CloseTabPanel(id); //传递页面id
            }
        }

        //保存
        function SaveDate() {
            var formPanel = Ext.getCmp("formPanel");
            if (formPanel.getForm().isValid()) {//如果验证通过
                formPanel.getForm().submit({
                    waitTitle: '系统提示', //标题
                    waitMsg: '正在导入数据,请稍后...', //提示信息  
                    submitEmptyText: false,
                    url: '/SysAppointed/ImportDate', //记录表单提交的路径
                    method: "POST",
                    success: function (form, action) {
                        var flag = action.result; //成功后
                        if (flag.success) {
                            MessageInfo("导入成功！", "right");
                            var a = '';
                            if (flag.data.length > 0) {
                                a += '<a style="color:Red" target=blank href="' + flag.data + '">下载错误数据</a><br/>';
                            }
                            a += '(' + flag.msg + ')';
                            Ext.getCmp('downerror').show();
                            Ext.getCmp("downerror").update(a);
                        } else {
                            MessageInfo(flag.msg, "error");
                        }
                    },
                    failure: function (form, action) {
                        MessageInfo("导入失败！", "error");
                    }
                });
            }
        }

    </script>
</asp:Content>
