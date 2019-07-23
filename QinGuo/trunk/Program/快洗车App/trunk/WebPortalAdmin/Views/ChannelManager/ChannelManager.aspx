<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="UrlContent" runat="server">
    <!--引用资源文件-->
    <script src="<%=Url.Content("~/Common/grid.js") %>" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="<%=Url.Content("~/Content/Extjs/examples/animated-dataview.css") %>" />
    <script src="<%=Url.Content("~/Content/Extjs/ux/DataView-more.js") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Content/Extjs/ux/DataViewTransition.js") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Project/ChannelManager/ChannelManager.js?version=1.4") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Project/ChannelManager/ChannelManagerLayout.js?version=1.4") %>" type="text/javascript"></script>

    <style type="text/css">
            body
            {
                font-size:12px;
                }
        </style>
</asp:Content>
