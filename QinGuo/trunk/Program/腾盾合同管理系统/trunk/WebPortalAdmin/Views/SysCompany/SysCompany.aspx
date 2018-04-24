<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content Id="Content1" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>

<asp:Content Id="Content2" ContentPlaceHolderID="UrlContent" runat="server">
    <!--引用资源文件-->
    <script src="<%=Url.Content("~/Project/SysCompany/SysCompany.js?version=1.5") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Project/SysCompany/SysCompanyLayout.js?version=1.5") %>" type="text/javascript"></script>
    <!--引用资源文件-->
    <script src="<%=Url.Content("~/Common/grid.js?version=1.5") %>" type="text/javascript"></script>
</asp:Content>
