<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
 <!--数据导出-->
 <form method="post" action="#" class="hide hideform"></form>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="UrlContent" runat="server">
     <!--引用资源文件-->
    <script src="<%=Url.Content("~/Common/grid.js?version=1.5") %>" type="text/javascript"></script>
    <script src="../../../../Content/Extjs/ux/imagePreview.js" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Areas/CACAI/Project/InComeBill/InComeBill.js?version=1.5") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Areas/CACAI/Project/InComeBill/InComeBillLayout.js?version=1.5") %>" type="text/javascript"></script>
        <script src="../../../../Content/Extjs/VTypes/ExtVTypes.js" type="text/javascript"></script>

    <style type="text/css">
    input{ vertical-align:middle; margin:0; padding:0}
    .file-box{ position:relative;width:340px}
    .txt{ height:22px; border:1px solid #cdcdcd; width:180px;}
    .btn{ background-color:#FFF; border:1px solid #CDCDCD;height:24px; width:70px;}
    .file{ position:absolute; top:0; right:80px; height:24px; filter:alpha(opacity:0);opacity: 0;width:260px }
    .textFiled{ background-color:Silver}
    </style>
</asp:Content>
