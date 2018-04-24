<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="UrlContent" runat="server">
    <!--文件区域-->
    <script src="<%=Url.Content("~/Common/grid.js?version=1.5") %>" type="text/javascript"></script>
    <script src="../../Project/SysOrganizational/SysOrganizationalLayout.js?version=1.5"
        type="text/javascript"></script>
    <script src="../../Project/SysOrganizational/SysOrganizational.js?version=1.5" type="text/javascript"></script>
    <style type="text/css">
        /*重写样式,取消title边框*/
        .x-panel-header
        {
            border-top: 0px;
        }
    </style>

</asp:Content>
