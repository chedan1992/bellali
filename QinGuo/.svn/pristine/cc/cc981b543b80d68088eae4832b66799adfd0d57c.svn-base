<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>
        <%=ViewData["WebSiteName"] %></title>
    <!--引入本地文件-->
    <link rel="stylesheet" type="text/css" href="<%=Url.Content("~/Resource/css/icon.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%=Url.Content("~/Content/Extjs/resources/css/ext-all.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%=Url.Content("~/Project/Login/global.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=Url.Content("~/Project/Login/normalize.css")%>" />
    <script src="<%=Url.Content("~/Content/jquery/jquery-1.8.0.min.js") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Project/Login/Login.js?version=1.5") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Content/Extjs/adapter/ext/ext-base.js") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Content/Extjs/ext-all.js") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Content/Extjs/ext-basex.js") %>" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="<%=Url.Content("~/Content/Extjs/resources/css/gtp-blue.css") %>"
        id="frameThemes" />
</head>
<body>
    <!--模版2-->
    <div class="header">
        <div class="navbar cf">
            <a href="/Home/Login">
                <img class="logo" src="../../Resource/img/login/temple1/logo.png" style="width: 54px;margin-top: 28px;"
                    alt=""></a>
            <h2 class="title">
                欢迎登录腾盾合同信息管理系统</h2>
            <ul class="nav cf" id="head-nav-menu">
                <li class="active"><a class="nav-item-link" href="/Home/Login">首页</a></li>
                <%--      <li><a class="nav-item-link" href="http://www.91qingguo.cn" target="_blank">公司简介</a></li>
                <li><a class="nav-item-link" href="http://www.91qingguo.cn" target="_blank">精品案例</a></li>
                <li><a class="nav-item-link" href="http://www.91qingguo.cn" target="_blank">行业方案</a></li>
                <li><a class="nav-item-link" href="http://www.91qingguo.cn" target="_blank">渠道合作</a></li>
                <li><a class="nav-item-link" href="http://www.91qingguo.cn" target="_blank">联系我们</a></li>--%>
            </ul>
        </div>
    </div>
    <script>        $('#head-nav-menu li:eq(0)').addClass('active');</script>
    <div class="wrapper">
        <div class="main sign-up">
            <div class="hd">
                <p class="description">
                    Hi，朋友，密码忘记了怎么办，输入电话号码。根据提示,完成密码找回吧 .
                </p>
            </div>
            <div class="bd">
                <div class="section cf">
                    <div class="sign-in-mod">
                        <h3 class="title">
                            密码找回</h3>
                        <div id="divmsg" class="" style="height: 25px; display: none;">
                            <img class="g-tips-ico" alt="" src="../../Resource/Img/login/s.gif" />
                            <div class="g-tips-content g-font-red" id="msg">
                            </div>
                        </div>
                        <form id="loginForm" action="/Home/SubmitLogin" method="post">
                        <div class="form-group">
                            <input class="form-control" type="text" name="txtLoginName" id="txtLoginName" placeholder="登录帐号"
                                onkeydown="return bubufx_enterpress(event);">
                        </div>
                        <div class="form-group">
                            <input class="form-control" type="text" name="txtPwd" id="txtPwd" placeholder="联系电话"
                                onkeydown="return bubufx_enterpress(event);">
                        </div>
                        <div class="form-group">
                            <input id="btLogin" name="button" disabled type="button" class="button button-primary button-metro"
                                value="功能暂未开通" onclick="return checkData();">
                        </div>
                        </form>
                        <div class="ad">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="footer footer-temp">
        Copyright @ 2017
        <%= ViewData["copyright"] %>
        版权所有
        <%= ViewData["Version"]%>
        <br />
        技术支持:成都青果</div>
</body>
</html>
