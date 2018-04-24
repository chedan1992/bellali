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
            var downMoBanUlr = "/Project/Template/导入模版.xls";

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
                                  html: '1  产品线:系统会检验产品线名称是否存在数据库中,如果不存在,则会添加<br/>'
                                       + '2证书类型:同上<br/>'
                                       + '3  知识点:同上<br/>'
                                       + '4题型类型:只能是单选题、多选题、判断题、填空题,问答题,如有其他选项,将视为错误数据<br/>'
                                       + '5   分级:目前只有3级初级、中级、高级<br/>'
                                       + '6试题分数:只能是整形的数字,如为空,默认为1分<br/>'
                                       + '7试题内容:不能为空,如有其他选项,将视为错误数据<br/>'
                                       + '8试题选项:当类型为单选或多选时,不能为空,多个选项用 | 隔开<br/>'
                                       + '9标准答案:答案需和试题选项对应,如果类型为判断题,答案用正确、错误表示<br/>'
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
            var StoreType = '<%= ViewData["StoreType"] %>';
            var result = [];
            var formPanel = Ext.getCmp("formPanel");
            if (formPanel.getForm().isValid()) {//如果验证通过
                formPanel.getForm().submit({
                    waitTitle: '系统提示', //标题
                    waitMsg: '正在导入数据,请稍后...', //提示信息  
                    submitEmptyText: false,
                    url: '/TestStore/ImportDate?StoreType=' + StoreType, //记录表单提交的路径
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
