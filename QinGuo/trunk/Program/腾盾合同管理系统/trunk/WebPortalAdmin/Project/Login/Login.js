var isRemaber = false; //判断是否选中记住登录账号

$(document).ready(function () {
    var remenberLoginStatus = getCookie("IsLoginStates");
    if (remenberLoginStatus == "on") {
        isRemaber = true;
        $("#LoginStates").attr("checked", "checked");
        var name = getCookie("CustomOrderLoginName");
        if (name != "") {
            $("#txtLoginName").val(name);
        }

        var pwd = getCookie("CustomOrderPwd");
        if (pwd != "") {
            $("#txtPwd").val(pwd);
        }
    } else {
        $("#LoginStates").removeAttr("checked");
    }
});

/*获取Cookie值*/
function getCookie(cookieName) {
    if (document.cookie.length > 0) {
        var arrCookie = document.cookie.split("; ");
        for (var i = 0; i < arrCookie.length; i++) {
            var arr = arrCookie[i].split("=");
            if (cookieName == arr[0]) {
                return arr[1];
            }
        }
    }
    return "";
}

//添加回车事件
function bubufx_enterpress(e) {
    if (isRemaber) {
        if (e.srcElement.id == "txtLoginName")//变动登录账号
        {
            $("#txtPwd").val("");
        }
    }
    var keynum;
    if (window.event) // IE   
    {
        keynum = e.keyCode;
    }
    else if (e.which) // Netscape/Firefox/Opera   
    {
        keynum = e.which;
    }
    if (keynum == 13) {
        checkData();
    }
}

//检验用户输入是否正确
function checkData() {
    var txtLoginName = $.trim($("#txtLoginName").val());
    var txtPwd = $.trim($("#txtPwd").val());
    var txtCode = $.trim($("#txtCode").val());
    if (txtLoginName == "") {
        $("#msg").html("用户名不能为空!");
        $("#divmsg").css("display", "block");
        return false;
    }
    else if (txtPwd == "") {
        $("#msg").html("密码不能为空!");
        $("#divmsg").css("display", "block");
        return false;
    }
    else if (txtCode == "") {
        $("#msg").html("验证码不能为空!");
        $("#divmsg").css("display", "block");
        return false;
    }
    else {
        loadingOpen("正在登录，请稍候。。。");
        Ext.Ajax.request({
            url: '/Home/LoginInfo',
            params: { loginName: txtLoginName, pwd: txtPwd, Code: txtCode },
            success: function (response) {
                var rs = eval('(' + response.responseText + ')');
                if (rs.success) {
                    document.getElementById("loginForm").submit();
                    return true; //进行登录
                } else {
                    loadingClose(); //取消遮罩
                    $("#msg").html(rs.msg);
                    $("#divmsg").css("display", "block");
                    refrushCode();
                    return false;
                }
            },
            failure: function (form, action) {
                loadingClose(); //取消遮罩
                $("#msg").html("登录失败!");
                $("#divmsg").css("display", "block");
            }
        });
    }
}

//刷新验证码
function refrushCode(val) {
    if (val) {
        return val.src = val.src + '?';
    }
    else {
        $("#ImageCode").click();
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
//取消遮罩
function loadingClose() {
    $(".datagrid-mask").css({
        display: "none"
    });
    $(".datagrid-mask-msg").css({
        display: "none"
    });
}

//设为首页
function SetHome(obj) {
    var i = location.href.lastIndexOf('/');
    var url = location.href.substring(0, i + 1);
    try {
        obj.style.behavior = 'url(#default#homepage)';
        obj.setHomePage(url);
    } catch (e) {
        if (window.netscape) {
            try {
                netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
            } catch (e) {
                alert("抱歉，此操作被浏览器拒绝！\n\n请在浏览器地址栏输入“about:config”并回车然后将[signed.applets.codebase_principal_support]设置为'true'");
            }
        } else {
            alert("抱歉，您所使用的浏览器无法完成此操作。\n\n您需要手动将【" + url + "】设置为首页。");
        }
    }
}

//收藏本站
function AddFavorite() {
    var i = location.href.lastIndexOf('/');
    var url = location.href.substring(0, i + 1);
    var title = document.title;
    try {
        window.external.addFavorite(url, title);
    }
    catch (e) {
        try {
            window.sidebar.addPanel(title, url, "");
        }
        catch (e) {
            alert("抱歉，您所使用的浏览器无法完成此操作。\n\n加入收藏失败，请使用Ctrl+D进行添加");
        }
    }
}



