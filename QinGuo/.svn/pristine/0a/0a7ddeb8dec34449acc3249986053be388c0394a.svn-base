<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
 <!--
    /* 不留大名, 只留联系方式    青果工作室
* 开发:pocket163  QQ:1101350716
* 开发:小男人  QQ:923227829
*
*      
                           _ooOoo_
                          o8888888o
                          88" . "88
                          (| -_- |)
                          O\  =  /O
                       ____/`---'\____
                     .'  \\|     |//  `.
                    /  \\|||  :  |||//  \
                   /  _||||| -:- |||||-  \
                   |   | \\\  -  /// |   |
                   | \_|  ''\---/''  |   |
                   \  .-\__  `-`  ___/-. /
                 ___`. .'  /--.--\  `. . __
              ."" '<  `.___\_<|>_/___.'  >'"".
             | | :  `- \`.;`\ _ /`;.`/ - ` : | |
             \  \ `-.   \_ __\ /__ _/   .-` /  /
        ======`-.____`-.___\_____/___.-`____.-'======
                           `=---='
        ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
                 佛祖保佑       永无BUG                     
*
*/
    -->
<!DOCTYPE html>
<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title><%=ViewData["WebSiteName"] %></title>
    <meta name="keywords" content="新都经信局微信管理系统,青果科技定制系统,联系QQ:1101350716;923227829" />
    <meta name="description" content="青果科技专注互联网移动建设,移动网站建设,业务系统研发,APP定制开发,系统运维,微信小程序开发,联系QQ:1101350716;923227829" />
    <!--引入本地文件-->
    <link rel="shortcut icon" href="/logo.gif" /> 
    <script src="<%=Url.Content("~/Content/Extjs/adapter/ext/ext-base.js") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Content/Extjs/ext-all.js") %>" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="<%=Url.Content("~/Project/Login/global.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=Url.Content("~/Project/Login/normalize.css")%>" />
    <script src="<%=Url.Content("~/Content/jquery/jquery-1.8.0.min.js") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Project/Login/Login.js?version=1.5") %>" type="text/javascript"></script>
    <%-- <link rel="stylesheet" type="text/css" href="<%=Url.Content("~/Project/Login/login.css")%>" />--%>
    <style>
        *
        {
            margin: 0;
            padding: 0;
            list-style: none;
        }
        a:hover
        {
            color: #1974A1;
        }
        #browser_ie
        {
            background-color: #f6f6b4; /* DISPLAY: none; */
            height: 95px;
            color: #000;
            position: absolute;
            z-index: 1000;
            width: 100%;
            font-size: 15px;
        }
        #browser_ie .brower_info
        {
            margin: 0px auto;
            width: 800px;
            padding-top: 17px;
        }
        #browser_ie .brower_info .notice_info
        {
            position: relative;
            margin-top: 5px;
            float: left;
        }
        #browser_ie .brower_info .notice_info P
        {
            text-align: left;
            line-height: 25px;
            width: 360px;
            display: inline-block;
        }
        #browser_ie .browser_list
        {
            position: relative;
            float: left;
        }
        #browser_ie .browser_list IMG
        {
            width: 40px;
            height: 40px;
        }
        #browser_ie .browser_list SPAN
        {
            text-align: center;
            width: 80px;
            display: inline-block;
        }
        .wrapper-sign-in .brandBox
        {
            position: absolute;
            top: 150px;
            left: 60px;
            max-width: 600px;
        }
        .wrapper-sign-in .brandBox .slogan
        {
            color: #fff;
            width: 450px;
            font-size: 38px;
            padding: 0;
            margin: 0;
            margin-bottom: 11px;
        }
        .wrapper-sign-in .brandBox .description
        {
            color: #fff;
            font-size: 18px;
            padding: 0;
            margin: 0;
        }
    </style>
</head>
<body>
    <div style=" display: none " id="browser_ie">
        <div class="brower_info">
            <div class="notice_info">
                <p>
                    你的浏览器版本过低，可能导致网站不能正常访问！<br>
                    为了你能正常使用网站功能，请使用这些浏览器。</p>
            </div>
            <div class="browser_list">
                <span><a href="http://www.google.cn/chrome/" target="_blank">
                    <img src="../../Resource/img/Chrome.png"><br>
                    Chrome </a></span><span><a href="http://www.firefox.com.cn/" target="_blank">
                        <img src="../../Resource/img/Firefox.png"><br>
                        Firefox </a></span><span><a href="http://support.apple.com/kb/DL1531" target="_blank">
                            <img src="../../Resource/img/Safari.png"><br>
                            Safari </a></span><span><a href="http://windows.microsoft.com/zh-cn/internet-explorer/download-ie"
                                target="_blank">
                                <img src="../../Resource/img/IE.png"><br>
                                IE9及以上 </a></span>
            </div>
        </div>
    </div>
    <!--模版2-->
    <div class="header">
        <div class="navbar cf">
            <img class="logo" src="../../Resource/img/login/temple1/logo.png" style="width: 54px;margin-top: 23px;"
                alt="">
            <h2 class="title" style="color:#9AD4EA;">
                欢迎登录新都经信局微信管理系统</h2>
            <%--<ul class="nav cf" id="head-nav-menu">
                <li class="active"><a class="nav-item-link" href="/Home/Login">首页</a></li>
                <li><a class="nav-item-link" href="#" target="_blank">公司简介</a></li>
                <li><a class="nav-item-link" href="#" target="_blank">产品介绍</a></li>
                <li><a class="nav-item-link" href="#" target="_blank">精品案例</a></li>
                <li><a class="nav-item-link" href="#" target="_blank">行业方案</a></li>
                <li><a class="nav-item-link" href="#" target="_blank">渠道合作</a></li>
                <li><a class="nav-item-link" href="http://aspx.taobao.com" target="_blank">联系我们</a></li>
            </ul>--%>
        </div>
    </div>
    <script>        
    
        $('#head-nav-menu li:eq(0)').addClass('active');


        if (!$.support.leadingWhitespace) {
            $("#browser_ie").show();
        }
    
    
    </script>
    <div class="wrapper-sign-in">
        <div class="main-sigh-in cf">
         <div class="brandBox">
                <p class="slogan">
                    新都经信局微信管理系统</p>
                <p class="description">
                    创新内容分享工具，开启社群经济新时代.<br />
                    <br />
                    旨在把知识产品化，而用户本身就是将知识产品化的负责人，即由使用者将自己的观点、知识、信息通过语音、视频、文字、图片等形式发布出来。
                     <br />
                     <br />
                    未来,只会属于你！</p>
            </div>
            <div class="sign-in-mod">
                <div class="title">
                    <span>账户登录</span> <span><%--|</span> <a class="normal-link" href="/Home/Regedit">注册账户</a>--%>
                    <div id="divmsg" class="g-exclamation g-tips login_tips" style="height: 25px; display: none;">
                        <img class="g-tips-ico" alt="" src="../../Resource/Img/login/s.gif" />
                        <div class="g-tips-content g-font-red" id="msg">
                        </div>
                    </div>
                </div>
                <form id="loginForm" method="post" action="/Home/SubmitLogin">
                <div class="form-item">
                    <div class="form-group">
                        <input class="form-control" style="width: 100%" type="text" name="txtLoginName" id="txtLoginName" value=""
                            placeholder="用户名" title="输入登录账号" onkeydown="return bubufx_enterpress(event);">
                    </div>
                </div>
                <div class="form-item">
                    <div class="form-group">
                        <input class="form-control" style="width: 100%" type="password" name="txtPwd" id="txtPwd" value=""
                            placeholder="密码" title="输入登录密码" onkeydown="return bubufx_enterpress(event);">
                    </div>
                </div>
                <div class="form-item">
                    <label>
                        <input type="checkbox" name="IsRemenberLoginStates" id="LoginStates">&nbsp;记住账号</label>
                </div>
                <div class="form-item">
                    <input id="btLogin" name="button" onclick="return checkData();" type="button" class="button button-primary button-metro"
                        value="登录">
                    <input type="hidden" name="dosubmit" value="1">
                    <a class="normal-link" href="/Home/ForgetPwd" style="margin-left: 20px">
                        忘记密码？</a>
                </div>
                </form>
                <div class="qrcode cf">
                    <img class="image" width="90" height="90" src="../../Resource/img/login/temple1/liantu.png"
                        alt="">
                    <div class="description">
                        <p>
                            微信扫描，关注<span style=" color:red">成都青果</span></p>
                        <p>
                            简单易用，助您开启物联网时代的智慧工作</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="footer footer-temp">
        Copyright @ 2018-2025
        <%= ViewData["copyright"] %>
        版权所有 <%= ViewData["Version"]%>  <br />
           技术支持:成都青果</div>
    <style>
        *
        {
            margin: 0;
            padding: 0;
            list-style: none;
        }
        a:hover
        {
            color: #1974A1;
        }
        
        #browser_ie
        {
            background-color: #f6f6b4; /* DISPLAY: none; */
            height:95px;
            color: #000;
            position:absolute;
            z-index:1000;
            width:100%;
            font-size: 15px;
        }
        #browser_ie .brower_info
        {
            margin: 0px auto;
            width: 800px;
            padding-top: 17px;
        }
        #browser_ie .brower_info .notice_info
        {
            position: relative;
            margin-top: 5px;
            float: left;
        }
        #browser_ie .brower_info .notice_info P
        {
            text-align: left;
            line-height: 25px;
            width: 360px;
            display: inline-block;
        }
        #browser_ie .browser_list
        {
            position: relative;
            float: left;
        }
        #browser_ie .browser_list IMG
        {
            width: 40px;
            height: 40px;
        }
        #browser_ie .browser_list SPAN
        {
            text-align: center;
            width: 80px;
            display: inline-block;
        }
    </style>
</body>
</html>
