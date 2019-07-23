<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .cn
        {
            font-size: 22px;
            color: #fff;
            letter-spacing: 1px;
            text-shadow: 1px 1px 2px #237AC5;
        }
        .en
        {
            color: #E9EFF1;
            font-size: xx-small;
        }
    </style>
    <!--顶部-->
    <div id="title" class="x-hide-display">
        <div style="background: url(../Resource/img/Index/topbg.gif) repeat-x; height: 100%;
            width: 100%;">
            <div class="topleft" style="margin-left: 35px;">
                <img src="../../Resource/img/login/temple1/logo.png" width="47" style="margin: 0px;
                    margin-right: 8px; float: left;" />
                <div class="cn">
                    快洗车信息管理系统</div>
                <div class="en">
                    Happy Fire Information System</div>
            </div>
            <div style="color: White; font-size: 12px; position: absolute; left: 30%; bottom: 20px">
                <%--  <div id="noticeTxt">
                </div>
                <div id="nnowdate">
                </div>
                <div id="timedate">
                </div>--%>
                <div style="display: inline-block; padding-right: 10px; font-size: 36px; line-height: 35px;
                    padding-left: -10px; white-space: nowrap; border-radius: 30px">
                    <span id="CompanyName">加载中..</span>
                </div>
            </div>
            <div class="topright">
                <ul>
                    <li onclick="Help()"><span>
                        <img alt="" src="../../Resource/css/icons/toolbar/GTP_userhelp.png" title="帮助" class="helpimg" /></span><a
                            href="#">帮助</a></li>
                    <li><a href="#" onclick="UpPassword()">密码修改</a></li>
                    <li><a href="#" onclick="logout()" id="logout">注销</a></li>
                </ul>
                <div class="user">
                    <span id="iconUser">加载中..</span>
                </div>
            </div>
            <div style="display: none">
                <script src='http://v7.cnzz.com/stat.php?id=155540&web_id=155540' language='JavaScript'
                    charset='gb2312'></script>
            </div>
        </div>
    </div>
    <!--内容区域-->
    <div id="south" class="x-hide-display">
        <div style="display: inline; float: left; width: 40%; color: Blue">
            <%-- <a target="blank" href="tencent://message/?uin=1101350716&Site=&Menu=yes"><img border="0" SRC=http://wpa.qq.com/pa?p=1:564267525:8 alt="点击这里给我发消息"></a>--%>
        </div>
        <div style="display: inline; float: right; text-align: left; font-size: 12px; font-weight: 100;
            width: 65%;">
            Copyright @
            <%= ViewData["copyright"] %>
            版权所有 <%= ViewData["Version"]%>
        </div>
    </div>
    <!--平台和电梯公司-->
    <div id="center" class="mainbox x-hide-display">
        <div class="mainleft">
            <div class="leftinfo">
                <div class="listtitle">
                    <a href="#" class="more1"></a>过期设备时实监控数量统计</div>
                <div id="maintj" class="maintj" style="width: 100%; height: 300px">
                </div>
            </div>
            <!--leftinfo end-->
            <div class="leftinfos">
                <div class="infoleft">
                    <div class="listtitle">
                        <a href="#" class="more1"></a>待办任务</div>
                   <ul class="newlist">
                       <% var SysFlow = ViewData["Flowlist"] as List<QINGUO.Model.ModSysFlow>;
                          if (SysFlow.Count > 0)
                          {
                              for (int i = 0; i < SysFlow.Count; i++)
                              {
                    %>
                    <li><a href="#" id="A1">
                        <%if (SysFlow[i].Title.Length > 20)
                          {
                              var newTitle = SysFlow[i].Title.ToString().Substring(0, 20) + "....";
                        %>
                        <%=newTitle%>
                        <%}
                          else
                          { %>
                        <%=SysFlow[i].Title%>
                        <%} %></a><b><%=Convert.ToDateTime(SysFlow[i].ApprovalTime).ToString("MM-dd")%></b></li>
                    <% }
                          }
                          else { %>
                          <span style="color:Silver"><h2>暂无信息</h2></span>
                        <%  }
                    %>
                    </ul>
                </div>
                <div class="inforight">
                    <div class="listtitle">
                        <a href="#" class="more1"></a>常用操作</div>
                         <ul class="tooli">
                            <li onclick="showYiChang()"><span>
                                <img src="/Resource/img/Index/d02.png" /></span><p>
                                <a href="#">异常设备</a></p>
                            </li>
                             <li onclick="showGuoQi()"><span>
                                <img src="/Resource/img/Index/d06.png" /></span><p>
                                <a href="#">过期设备</a></p>
                            </li>
                        </ul>
                </div>
            </div>
        </div>
        <!--mainleft end-->
        <div class="mainright">
            <div class="dflist">
                <div class="listtitle">
                    <a href="#" class="more1"></a>最新信息</div>
                <ul class="newlist">
                    <% var Dynamic = ViewData["list"] as List<QINGUO.Model.ModEDynamic>;
                       if (Dynamic.Count > 0)
                       {
                           for (int i = 0; i < Dynamic.Count; i++)
                           {
                    %>
                    <li><a href="#" id="<%=Dynamic[i].Id%>" title="<%=Dynamic[i].Name%>" onclick="LookDynamic(this)">
                        <%if (Dynamic[i].Name.Length > 20)
                          {
                              var newTitle = Dynamic[i].Name.ToString().Substring(0, 20) + "....";
                        %>
                        <%=newTitle%>
                        <%}
                          else
                          { %>
                        <%=Dynamic[i].Name%>
                        <%} %></a></li>
                    <% }
                       }
                       else
                       { %>
                    <span style="color: Silver">
                        <h2>
                            暂无信息</h2>
                    </span>
                    <%  }
                    %>
                </ul>
            </div>
            <div class="dflist1">
                <div class="listtitle">
                    <a href="#" class="more1"></a>天气预报</div>
                <iframe width="130" scrolling="no" height="120" style="padding: 30px;" frameborder="0"
                    allowtransparency="true" src="http://i.tianqi.com/index.php?c=code&id=4&bdc=%23&icon=5&wind=1&num=6">
                </iframe>
            </div>
        </div>
        <!--mainright end-->
    </div>
    <input type="hidden" id="Notice" value="<%=ViewData["Notice"]%>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="UrlContent" runat="server">
    <!--文件区域-->
  
    <script src="<%=Url.Content("~/Common/grid.js?version=1.5") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Content/Extjs/ux/RowExpander.js?version=1.5") %>"
        type="text/javascript"></script>
    <!--引用fileUpLoad-->
    <link href="<%=Url.Content("~/Content/Extjs/ux/fileuploadfield/css/fileuploadfield.css")%>"
        rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="<%=Url.Content("~/Content/Extjs/ux/fileuploadfield/FileUploadField.js") %>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Content/Extjs/ux/TabCloseMenu.js") %>"></script>
    <!--验证-->
    <script type="text/javascript" src="<%=Url.Content("~/Content/Extjs/VTypes/ExtVTypes.js") %>"></script>
    <script src="<%=Url.Content("~/Content/Extjs/ux/DataView-more.js") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Content/Extjs/ux/DataViewTransition.js") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Content/Extjs/ux/ComboBoxTree/form.ComboBoxTree.js") %>"
        type="text/javascript"></script>
    <%--引入地图--%>
    <script type="text/javascript" src="http://api.map.baidu.com/api?v=2.0&ak=uf2Hh2GiYHoavUNgUoxm3PIgd2E8cRzO"></script>
    <script src="../../Content/echart/echarts.min.js" type="text/javascript"></script>
    <!--弹框上传上传-->
    <link href="<%=Url.Content("~/Content/UploadDialog/css/Ext.ux.UploadDialog.css") %>"
        rel="stylesheet" type="text/css" />
    <script src="<%=Url.Content("~/Content/UploadDialog/UploadDialog.js") %>" type="text/javascript"
        charset="utf-8"></script>
    <script src="<%=Url.Content("~/Content/UploadDialog/locale/ru.utf-8.js") %>" type="text/javascript"
        charset="utf-8"></script>
    <%--图片上传--%>
    <script src="../../Content/ajaxupload/ajaxupload.js" type="text/javascript"></script>
    <script src="../../Content/Extjs/ux/Portal.js" type="text/javascript"></script>
    <script src="../../Content/Extjs/ux/PortalColumn.js" type="text/javascript"></script>
    <script src="../../Content/Extjs/ux/Portlet.js" type="text/javascript"></script>
    <link href="../../Content/Extjs/ux/css/Portal.css" rel="stylesheet" type="text/css" />
    <script src="<%=Url.Content("~/Content/Extjs/explain/multiselect.js") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Common/comonfun.js?version=1.5") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Project/Index/index.js?version=1.5") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Project/Index/IndexLayout.js?version=1.5") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Project/Index/CompanyIndex.js?version=1.5") %>" type="text/javascript"></script>
    <link href="../../Project/Index/style.css" rel="stylesheet" type="text/css" />
</asp:Content>
