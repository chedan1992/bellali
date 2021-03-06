﻿<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>您出错了</title>
    <script src="../../Content/jquery/jquery-1.8.0.min.js" type="text/javascript"></script>
    <style type="text/css">
        /* CSS Document */
        /***
 */
        *
        {
            font-size: 9pt;
            border: 0;
            margin: 0;
            padding: 0;
        }
        body
        {
            font-family: '微软雅黑';
            margin: 0 auto;
            min-width: 980px;
        }
        ul
        {
            display: block;
            margin: 0;
            padding: 0;
            list-style: none;
        }
        li
        {
            display: block;
            margin: 0;
            padding: 0;
            list-style: none;
        }
        img
        {
            border: 0;
        }
        dl, dt, dd, span
        {
            margin: 0;
            padding: 0;
            display: block;
        }
        a, a:focus
        {
            text-decoration: none;
            color: #000;
            outline: none;
            blr: expression(this.onFocus=this.blur());
        }
        a:hover
        {
            color: #00a4ac;
            text-decoration: none;
        }
        table
        {
            border-collapse: collapse;
            border-spacing: 0;
        }
        cite
        {
            font-style: normal;
        }
        h2
        {
            font-weight: normal;
        }
        
        /*cloud*/
        
        #mainBody
        {
            width: 100%;
            height: 100%;
            position: absolute;
            z-index: -1;
        }
        /*error 404*/
        .error
        {
            background: url(../../Resource/img/404.png) no-repeat;
            width: 490px;
            margin-top:155px;
            padding-top: 65px;
        }
        .error h2
        {
            font-size: 22px;
            padding-left: 154px;
        }
        .error p
        {
            padding-left: 154px;
            line-height: 35px;
            color: #717678;
        }
        .reindex
        {
            padding-left: 154px;
        }
        .reindex a
        {
            width: 115px;
            height: 35px;
            font-size: 14px;
            font-weight: bold;
            color: #fff;
            background: #3c95c8;
            display: block;
            line-height: 35px;
            text-align: center;
            border-radius: 3px;
            margin-top: 20px;
        }
    </style>
    <script type="text/javascript" language="javascript">
        $(function () {
            $('.error').css({ 'position': 'absolute', 'left': ($(window).width() - 490) / 2 });
            $(window).resize(function () {
                $('.error').css({ 'position': 'absolute', 'left': ($(window).width() - 490) / 2 });
            })
        });  
    </script>
</head>
<body style="background: #edf6fa;">
    <div class="error">
        <h2>
            非常遗憾，您的用户信息已过期！</h2>
        <p>
            看到这个提示，就自认倒霉吧!</p>
        <div class="reindex">
            <a href="/Home/Login" target="_parent">返回登录</a></div>
    </div>
</body>
</html>
