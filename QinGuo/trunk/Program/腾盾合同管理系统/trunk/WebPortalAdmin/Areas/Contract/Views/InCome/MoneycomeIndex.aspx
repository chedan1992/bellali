<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!--数据导出-->
 <form method="post" action="#" class="hide hideform"></form>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="UrlContent" runat="server">
     <!--引用资源文件-->
    <script src="<%=Url.Content("~/Common/grid.js?version=1.5") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Areas/Contract/Project/Moneycome/Moneycome.js?version=1.5") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Areas/Contract/Project/Moneycome/MoneycomeLayout.js?version=1.5") %>" type="text/javascript"></script>

</asp:Content>
