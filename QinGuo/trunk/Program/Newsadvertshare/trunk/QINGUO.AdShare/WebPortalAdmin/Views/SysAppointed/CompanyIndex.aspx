<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
 <!--数据导出-->
 <form method="post" action="#" class="hide hideform"></form>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="UrlContent" runat="server">
 <!--引用资源文件-->
    <script src="<%=Url.Content("~/Common/grid.js?version=1.5") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Project/SysAppointed/SysAppointed.js?version=1.5") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Project/SysAppointed/SysAppointedLayout.js?version=1.5") %>" type="text/javascript"></script>
   <style type="text/css">
       .x-grid-record-red {  
    background: #CD950C;!important;   //修改背景颜色
    color:#000000;!important;  //修改字体颜色
}
.LostTime
{
   background:  #CD0000;!important;  
    }
    </style>
</asp:Content>
