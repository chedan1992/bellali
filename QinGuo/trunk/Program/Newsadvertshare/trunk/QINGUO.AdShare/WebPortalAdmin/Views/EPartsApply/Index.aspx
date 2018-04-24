<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="UrlContent" runat="server">
    <!--引用资源文件-->
    <script src="<%=Url.Content("~/Common/grid.js?version=1.5") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Project/EPartsApply/EPartsApply.js?version=1.5") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Project/EPartsApply/EPartsApplyLayout.js?version=1.5") %>" type="text/javascript"></script>
      <script src="../../Content/imgbox/jquery.min.js" type="text/javascript"></script>
    <script src="../../Content/imgbox/jquery.imgbox.pack.js" type="text/javascript"></script>
     <link href="../../Resource/css/lrtk.css" rel="stylesheet" type="text/css" />
</asp:Content>
