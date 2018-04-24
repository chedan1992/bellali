<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
 <div id="info" class="grid_msg">
       <div style="vertical-align: middle" class="grid_msg_text">
          <span>
          <img alt="" style="width:15px;height:15px;vertical-align: middle" src="../../Resource/css/icons/toolbar/GTP_prompt.png" />
          <label style="vertical-align: middle">提示:一级节点为产品线,二级节点为证书类型,三级节点为技术领域,四级节点为知识点!</label>
           </span>
       </div>
       <div class="grid_msg_btn">
           <a href="#" class="info" onclick="SetInfo();">我知道了</a>
       </div>
   </div>
    <div id="imgdiv" class="x-hide-display">
          <img id="Img" src="" alt="" style="width:250px;height:140px;margin:0;padding:0;overflow:hidden"/>
   </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="UrlContent" runat="server">
    <link href="../../Content/Extjs/ux/treegrid/treegrid.css" rel="stylesheet" type="text/css" />
     <link rel="stylesheet" type="text/css" href="<%=Url.Content("~/Resource/css/page.css") %>" />
    <!--引用treegrid-->
    <script src="<%=Url.Content("~/Content/Extjs/ux/treegrid/TreeGridSorter.js") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Content/Extjs/ux/treegrid/TreeGridColumnResizer.js") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Content/Extjs/ux/treegrid/TreeGridNodeUI.js") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Content/Extjs/ux/treegrid/TreeGridLoader.js") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Content/Extjs/ux/treegrid/TreeGridColumns.js") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Content/Extjs/ux/treegrid/TreeGrid.js") %>" type="text/javascript"></script>

<!--文件区域-->
     <script src="<%=Url.Content("~/Common/grid.js?version=1.5") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Project/SysCategory/SysCategoryLayout.js?version=1.5") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Project/SysCategory/SysCategory.js?version=1.5") %>" type="text/javascript"></script>
</asp:Content>
