<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>您出错了</title>
    <script src="../Content/jquery/jquery-1.8.0.min.js" type="text/javascript"></script>
    <style type="text/css">
        .datagrid-mask
        {
            background: #ccc;
        }
        
        .datagrid-mask-msg
        {
            background: #ffffff url('../../Resource/css/icons/loading.gif') no-repeat scroll 5px center;
        }
        
        .datagrid-mask
        {
            position: absolute;
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;
            opacity: 0.3;
            filter: alpha(opacity=30);
            display: none;
        }
        
        .datagrid-mask-msg
        {
            border-color: #95B8E7;
        }
        
        .datagrid-mask-msg
        {
            position: absolute;
            top: 50%;
            margin-top: -20px;
            padding: 12px 5px 10px 30px;
            width: auto;
            height: 16px;
            border-width: 2px;
            border-style: solid;
            display: none;
        }
        
        A
        {
            text-decoration: none;
        }
        
        A:link
        {
            color: #001111;
            font-family: 宋体;
            text-decoration: none;
        }
        
        A:visited
        {
            color: #001111;
            font-family: 宋体;
            text-decoration: none;
        }
        
        A:active
        {
            font-family: 宋体;
            text-decoration: none;
        }
        
        A:hover
        {
            border-top-width: 1px;
            border-left-width: 1px;
            font-size: 12px;
            color: #ff0000;
            border-bottom: 1px dotted;
            border-right-width: 1px;
            text-decoration: none;
        }
        
        BODY
        {
            scrollbar-face-color: #e8e7e7;
            font-size: 12px;
            scrollbar-highlight-color: #ffffff;
            scrollbar-shadow-color: #ffffff;
            color: #001111;
            scrollbar-3dlight-color: #cccccc;
            scrollbar-arrow-color: #ff6600;
            scrollbar-track-color: #efefef;
            font-family: 宋体;
            scrollbar-darkshadow-color: #b2b2b2;
            scrollbar-base-color: #000000;
            background-color: #ffffff;
        }
        
        TABLE
        {
            font-size: 9pt;
            font-family: 宋体;
            border-collapse: collapse;
        }
        
        .button
        {
            border-right: #5589aa 1px solid;
            border-top: #5589aa 1px solid;
            font-size: 9pt;
            background: url(image/ButtonBg.gif) #f6f6f9;
            border-left: #5589aa 1px solid;
            width: 50px;
            color: #000000;
            border-bottom: #5589aa 1px solid;
            height: 18px;
        }
        
        .lanyu
        {
            border-right: #5589aa 1px solid;
            border-top: #5589aa 1px solid;
            font-size: 12px;
            border-left: #5589aa 1px solid;
            color: #000000;
            border-bottom: #5589aa 1px solid;
        }
        
        .font
        {
            font-size: 9pt;
            filter: DropShadow(Color=#cccccc, OffX=2, OffY=1, Positive=2);
            text-decoration: none;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function countDown(secs) {
                        $(".timeOn")[0].innerHTML = secs;
                        if (--secs > 0) {
                            if (secs < 2) {
                                loadingOpen('正在跳转,请稍后....');
                            }
                            setTimeout("countDown(" + secs + ")", 1000);
                        }
                        else {
                            window.location.href = '/Home/Login';
                        }
        }

        //遮罩加载框
        function loadingOpen(msg) {//打开遮罩
            $("<div class=\"datagrid-mask\"></div>").css({ display: "block", width: "100%", height: $(window).height() }).appendTo("body");
            $("<div class=\"datagrid-mask-msg\"></div>").html(msg).appendTo("body").css({
                display: "block",
                left: ($(document.body).outerWidth(true) - 190) / 2,
                top: ($(window).height() - 45) / 2
            });
        }
    </script>
</head>
<body onload="countDown(5)" bgcolor="#ffffff" leftmargin="0" topmargin="0">
    <table height="100%" style="margin-top: 100px" cellspacing="0" cellpadding="0" width="100%"
        align="center" border="0">
        <tbody>
            <tr>
                <td valign="center" align="middle">
                    <div align="center">
                        <center>
                            <table style="border-collapse: collapse; color:#A3A3A3 " bordercolor="#111111" height="340" cellspacing="0"
                                cellpadding="0" width="700" border="0">
                                <tbody>
                                    <tr>
                                        <td valign="center" width="700" height="340">
                                            <p align="center">
                                                <img src="../Resource/img/4042.jpg" border="0"></p>
<%--                                            <p align="center">
                                                <span lang="zh-cn"><b><font color="#ff0000">出错啦：</font></b> <font color="#ff0000">您还未登录或信息已过期！</font></span></p>--%>
                                            <p align="center">
                                                <font><b>建议：&nbsp;&nbsp; </b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                </font>
                                            </p>
                                            <p align="left" style="margin-left:205px">
                                                1.你可以刷新（F5）试下看看，不过估计效果不大。
                                            </p>
                                            <p align="left" style="margin-left: 205px">
                                                2.系统<span class="timeOn" style="color: red">4</span>秒后自动返回登录首页&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </p>
                                            <p align="center">
                                            </p>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </center>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
</body>
</html>
