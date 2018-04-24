<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id="MicrositeRt" class="MicrositeTxt">
        <div class="top">
            <p>
                二维码预览图区域</p>
        </div>
        <div class="footer" id="Microsite">
            <img id="showImg" src="../../Resource/img/null.jpg" />
        </div>
       <%-- <div>
            <input name="button" type="button" class="button button-metro" value="下 载" style=" margin-top:20px;">
        </div>--%>
    </div>
    <div id="form_panel" class="x-hide-display">
    </div>
      <!--数据导出-->
 <form method="post" action="#" class="hide hideform"></form>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="UrlContent" runat="server">
    <!--引用资源文件-->
    <script src="<%=Url.Content("~/Common/grid.js") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Project/SysQRCode/SysQRCode.js?version=1.5") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Project/SysQRCode/SysQRCodeLayout.js?version=1.5") %>" type="text/javascript"></script>
    <style type="text/css">
        .top
        {
            margin-bottom: 10px;
        }
        .MicrositeTxt
        {
            margin-top: 20px;
            float: right;
            text-align: center;
            padding-left: 20px;
            padding-right: 20px;
            color: #6a6a6a;
            font-size: 14px;
            font-family: "Helvetica Neue" , "Luxi Sans" , "DejaVu Sans" ,Tahoma, "Hiragino Sans GB" ,STHeiti, "Microsoft YaHei" ,Arial,sans-serif;
        }
        .footer
        {
            margin: 0 0 0 0;
            padding: 0px;
            border: 1px solid silver;
        }
        .button
        {
            cursor:pointer;
            background-color: #eee;
            display: -moz-inline-stack;
            display: inline-block;
            vertical-align: middle;
            zoom: 1;
            border-width: 1px;
            border-style: solid;
            height: 32px;
            line-height: 30px;
            padding-top: 0;
            padding-bottom: 0;
            padding-left: 20px;
            padding-right: 20px;
            font-size: 14px;
            font-family: "Microsoft YaHei" ,SimSun,Tahoma,Verdana,Arial,sans-serif;
            color: #666;
            margin: 0;
            text-decoration: none;
            text-align: center;
        }
        .button-large
        {
            height: 34px;
            line-height: 34px;
            font-size: 16px;
            font-weight: 700;
        }
        .button-small
        {
            height: 28px;
            line-height: 26px;
            font-size: 14px;
        }
        .button-extra-small
        {
            height: 22px;
            line-height: 22px;
            font-size: 12px;
        }
        .button-default
        {
            color: #333;
            background-color: #fff;
            border-color: #ccc;
        }
        .button-default:hover
        {
            color: #333;
            background-color: #ebebeb;
            border-color: #adadad;
        }
        .button-primary
        {
            color: #fff;
            background-color: #2378cd;
            border-color: #26a;
            text-decoration: none;
        }
        .button-hover, .button-primary:hover
        {
            color: #fff;
            background-color: #fa0;
            border-color: #e67a00;
            text-decoration: none;
        }
        .button-warning:hover
        {
            background-color: #ff9100;
            border-color: #e66b00;
        }
        .button-link
        {
            color: #06c;
            border-color: transparent;
            background-color: transparent;
        }
        .button-link:hover
        {
            text-decoration: underline;
        }
        .button-metro
        {
            border-radius: 0;
        }
        .button-rounded
        {
            border-radius: 3px;
            -webkit-box-shadow: 0 1px 0 #ababab;
            -moz-box-shadow: 0 1px 0 #ababab;
            box-shadow: 0 1px 0 #ababab;
        }
        .button-disabled, .button-disabled:hover
        {
            color: #ababab;
            background-color: #e6e6e6;
            border-color: #ccc;
        }
        .button-groups .button
        {
            margin-right: 5px;
        }
    </style>
</asp:Content>
