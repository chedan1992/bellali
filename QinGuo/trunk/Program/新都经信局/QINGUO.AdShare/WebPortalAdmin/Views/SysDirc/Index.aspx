<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="UrlContent" runat="server">
  <link rel="stylesheet" type="text/css" href="<%=Url.Content("~/Content/Extjs/ux/treegrid/treegrid.css") %>" />

    <script src="<%=Url.Content("~/Project/SysDirc/SysDircLayout.js?version=1.5") %>"type="text/javascript"></script>
    <script src="<%=Url.Content("~/Project/SysDirc/SysDirc.js?version=1.5") %>"type="text/javascript"></script>

    <!--文件区域-->
    <script src="<%=Url.Content("~/Common/grid.js?version=1.5") %>" type="text/javascript"></script>
    

</asp:Content>
