<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id="form_panel" class="x-hide-display">
    </div>
    <input type="hidden" id="HidKey" value="<%=ViewData["Id"]%>" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="UrlContent" runat="server">
    <style type="text/css">
       /*解决IE11无法显示*/
        .x-box-layout-ct,.x-box-inner
        {
            overflow:visible;
        }
        textarea .x-form-field
        {
            padding:0px;
        }
    </style>
    <script src="<%=Url.Content("~/Common/grid.js") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Areas/Contract/Project/InCome/InComeEdit.js?version=1.5") %>" type="text/javascript"></script>
    <script type="text/javascript">
        Ext.onReady(function () {
            top.HasSave = false; //清空页面初始状态,未保存
            //列模型
            var north = new Ext.Panel({
                region: 'north',
                tbar: Toolbar(),
                border: false
            });
            var form = CreateForm();
            var viewport = new Ext.Viewport({
                layout: 'border',
                id: 'viewMain',
                items: [
                            north,
                            new Ext.Panel({
                                region: 'center',
                                border: false,
                                layout: 'fit',
                                autoScroll: true,
                                deferredRender: false,
                                items: [form]
                            })
                    ]
            });

            init();
        });

        //初始化
        function init() {
            var CType = '<%= ViewData["CType"] %>'; //（1：收入合同 2：支出合同）
            var Id = '<%= ViewData["Id"] %>';
            if (Id) {//编辑
                url = '/Contract/InCome/SaveData?Id=' + Id + '&modify=1&CType=' + CType;
                var respon = Ext.lib.Ajax.getConnectionObject().conn;
                respon.open("post", "/Contract/InCome/LoadData?Id=" + Id, false);
                respon.send(null);
                var result = Ext.util.JSON.decode(respon.responseText);
                if (result) {
                    var form = Ext.getCmp('formPanel');
                    form.getForm().loadRecord(result); //再加载数据   
                }
            }
            else {
                url = '/Contract/InCome/SaveData?1=1&CType=' + CType;
            }
        }

        //保存数据
        function SaveDate() {
            if (top.HasSave) {
                top.Ext.Msg.show({ title: "信息提示", msg: "您已经保存过了,请刷新重新操作!", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                return false;
            }
            var formPanel = Ext.getCmp("formPanel");
            if (formPanel.getForm().isValid()) {//如果验证通过
               
                formPanel.getForm().submit({
                    waitTitle: '系统提示', //标题
                    waitMsg: '正在提交数据,请稍后...', //提示信息  
                    submitEmptyText: false,
                    url: url, //记录表单提交的路径 
                    method: "post",
                    success: function (form, action) {
                        var Id = '<%= ViewData["Id"] %>';
                        var CType = '<%= ViewData["CType"] %>';
                        if (Id) {//编辑
                            url = '/Contract/InCome/SaveData?Id=' + Id + '&modify=1&CType=' + CType;
                        }
                        else {
                            url = '/Contract/InCome/SaveData?1=1&CType=' + CType;
                        }
                        var flag = action.result; //成功后
                        if (flag.success) {
                            if (flag.errorcode == "2") {
                                MessageInfo(flag.msg, "info");
                            }
                            else {
                                MessageInfo("保存成功！", "right");
                                top.HasSave = true;
                            }
                        }
                        else {
                            MessageInfo(flag.msg, "error");
                        }

                        CloseWindow();
                    },
                    failure: function (form, action) {
                        var Id = '<%= ViewData["Id"] %>';
                        var CType = '<%= ViewData["CType"] %>';
                        if (Id) {//编辑
                            url = '/Contract/InCome/SaveData?Id=' + Id + '&modify=1&CType=' + CType;
                        }
                        else {
                            url = '/Contract/InCome/SaveData?1=1&CType=' + CType;
                        }
                        MessageInfo("保存失败！", "error");
                    }
                });
            }
        }
    </script>
</asp:Content>
