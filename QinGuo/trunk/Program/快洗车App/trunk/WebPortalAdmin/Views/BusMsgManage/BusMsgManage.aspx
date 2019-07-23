<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="UrlContent" runat="server">
 <!--文件区域-->
     <script src="<%=Url.Content("~/Common/grid.js") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Project/BusMsgManage/BusMsgManageLayout.js") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Project/BusMsgManage/BusMsgManage.js") %>" type="text/javascript"></script>
</asp:Content>
