<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>乐消消防手机端下载</title>
    <link href="../../Resource/css/downLoad.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function isWeiXin() {
            var ua = window.navigator.userAgent.toLowerCase();
            if (ua.match(/MicroMessenger/i) == 'micromessenger') {
                return true;
            } else {
                return false;
            }
        }
        window.onload = function () {
            if (isWeiXin()) {
                alert('您使用的微信不支持下载哦.请用其他浏览器打开,并下载!');
            }
        }  
    </script>
</head>
<body style="overflow: hidden">
    <div class="main">
        <div class="page page1">
            <h1 style="top: 10%">
                乐消消防手机客户端</h1>
            <p class="title01" style="top: 11%">
                至轻至简 · 闪耀登场</p>
            <dl style="left: 62%; top: 18%">
                <dt></dt>
                <dd>
                    <a class="btn" href="../../Apk/android.apk">
                        <img src="../../Resource/img/dowload/android.png" alt="android">
                        <span>Android版下载</span> </a><a class="btn" href="https://itunes.apple.com/cn/app/id1182308299?mt=8">
                            <img src="../../Resource/img/dowload/ios.png" alt="ios">
                            <span>iPhone版下载</span> </a>
                </dd>
            </dl>
        </div>
        <div class="page page2">
        </div>
    </div>
</body>
</html>
