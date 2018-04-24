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
        <script type="text/javascript" src="http://api.map.baidu.com/api?v=2.0&ak=DuKQ1S1pogxGH02bFGVM1nPl"></script>
    <style type="text/css">
        *
        {
            margin: 0;
            padding: 0;
            list-style-type: none;
        }
        a, img
        {
            border: 0;
        }
        .formbox
        {
            margin: 20px auto;
            padding-right: 80px;
        }
        .formcon
        {
            padding: 30px 0;
        }
        .formcon table td
        {
            padding: 5px;
            line-height: 24px;
        }
        /* flow_steps */.flow_steps
        {
            height: 23px;
            overflow: hidden;
        }
        .flow_steps li
        {
            float: left;
            height: 23px;
            padding: 0 50px 0 40px;
            line-height: 23px;
            text-align: center;
            background: url(img/barbg.png) no-repeat 100% 0 #E4E4E4;
            font-weight: bold;
        }
        .flow_steps li.done
        {
            background-position: 100% -46px;
            background-color: #FFEDA2;
        }
        .flow_steps li.current_prev
        {
            background-position: 100% -23px;
            background-color: #FFEDA2;
        }
        .flow_steps li.current
        {
            color: #fff;
            background-color: #990D1B;
        }
        .flow_steps li#qzfs.current, .flow_steps li.last
        {
            background-image: none;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            InitCompany();
            //用户名离开事件
            $("#txtRegisterName").bind("blur", function () {
                var txtRegisterName = $("#txtRegisterName").val().trim();
                if (txtRegisterName != "") {
                    if (!txtRegisterName.match(/^(((13[0-9]{1})|(15[0-9]{1})|(18[0-9]{1}))+\d{8})$/)) {
                        $("#oneMsg").html("手机号码格式不正确!");
                        $("#Msgone").css("display", "block");
                        $("#txtRegisterName").focus();
                        return false;
                    }
                    //验证用户手机是否已注册
                    if (VilivaName("", txtRegisterName)) {
                        $("#oneMsg").html("该手机号已经注册!");
                        $("#Msgone").css("display", "block");
                        $("#txtRegisterName").focus();
                        return false;
                    }
                    else {
                        //隐藏状态
                        $("#oneMsg").html("");
                        $("#Msgone").css("display", "none");
                    }
                }
            });


            //密码离开事件
            $("#txtRegisterPwd").bind("blur", function () {
                var txtRegisterPwd = $("#txtRegisterPwd").val().trim();
                if (txtRegisterPwd != "") {
                    //验证用户手机是否已注册
                    if (txtRegisterPwd.length < 6 || txtRegisterPwd.length > 20) {
                        $("#oneMsg").html("密码长度请填写6~20位!");
                        $("#Msgone").css("display", "block");
                        $("#txtRegisterName").focus();
                        return false;
                    }
                    else {
                        //隐藏状态
                        $("#oneMsg").html("");
                        $("#Msgone").css("display", "none");
                    }
                }
            });
        
        });
        //普通用户选择单位
        function InitCompany() {
            $("#CompanyName").empty();
            $.ajax({
                url: '/SysCompany/GetCompanyList',
                async: false,
                dataType: "json",
                success: function (result) {
                    if (result.length > 0) {
                        $("#CompanyName").append("<option value=''>==请选择单位==</option>");
                        for (var i = 0; i < result.length; i++) {
                            $("#CompanyName").append("<option value='" + result[i].ID + "'>&nbsp;&nbsp;&nbsp;" + result[i].Name + "</option>");
                        }
                    }
                    else {
                        $("#CompanyName").append("<option value=''>==暂无单位==</option>");
                    }
                },
                error: function () {
                }
            });
        }

        function setDeptCode(obj) {
            $("#HidDeptCode").val(obj.value);
        }
        //绑定部门
        function GetDeptList() {
            $("#DeptName").empty();
            $.ajax({
                url: '/SysCompany/GetDeptList?OrgId=' + $("#HidCompanyCode").val() + "&data=" + new Date(),
                async: false,
                dataType: "json",
                success: function (result) {
                    if (result.length > 0) {
                        $("#DeptName").append("<option value=''>==请选择部门==</option>");
                        for (var i = 0; i < result.length; i++) {
                            $("#DeptName").append("<option value='" + result[i].ID + "'>&nbsp;&nbsp;&nbsp;" + result[i].Name + "</option>");
                        }
                    } else {
                        $("#DeptName").append("<option value=''>==暂无部门==</option>");
                    }
                },
                error: function () {
                }
            });
        }

        //切换面板1
        function one() {
            var txtUserName = $("#txtUserName").val().trim();//用户姓名
            var txtRegisterName = $("#txtRegisterName").val().trim();
            var txtRegisterPwd = $("#txtRegisterPwd").val().trim();
            var txtRegisterPwd2 = $("#txtRegisterPwd2").val().trim();
            if (txtUserName == "") {
                $("#oneMsg").html("用户姓名不能为空!");
                $("#Msgone").css("display", "block");
                $("#txtUserName").focus();
                return false;
            }
            if (txtRegisterName == "") {
                $("#oneMsg").html("手机号码不能为空!");
                $("#Msgone").css("display", "block");
                $("#txtRegisterName").focus();
                return false;
            }
            if (!txtRegisterName.match(/^(((13[0-9]{1})|(15[0-9]{1})|(18[0-9]{1}))+\d{8})$/)) {
                $("#oneMsg").html("手机号码格式不正确!");
                $("#Msgone").css("display", "block");
                $("#txtRegisterName").focus();
                return false;
            }
            if (txtRegisterName == "") {
                $("#oneMsg").html("用户名不能为空!");
                $("#Msgone").css("display", "block");
                return false;
            }
            if (txtRegisterPwd == "" || txtRegisterPwd2 == "") {
                $("#oneMsg").html("密码不能为空!");
                $("#Msgone").css("display", "block");
                return false;
            }
            if (txtRegisterPwd !== txtRegisterPwd2) {
                $("#oneMsg").html("两次密码输入不相同!");
                $("#Msgone").css("display", "block");
                return false;
            }
            //隐藏状态
            $("#oneMsg").html("");
            $("#Msgone").css("display", "none");

            //显示用户类型
            $("#UseType1").show();
            $("#UseType2").hide();


            $("#one").hide();
            $("#two").show();
            $("#grxx").attr("class", "current_prev");
            $("#zjxx").attr("class", "current");
        }

        //社会类型选择
        function AdminTypeChk() {
            var val = $('input:radio[name="CompanyType"]:checked').val();
            $("#HidCompanyType").val(val);
        }
        //切换单位类型
        function UserTypeTwo() {
            $("#UseType1").hide();
            $("#UseType2").show();
        }
        //返回单位类型
        function reUserTypeTwo() {
            $("#UseType1").show();
            $("#UseType2").hide();
        }
        //切换面板2
        function two() {
            var HidUserType = $("#HidUserType").val(); //用户类型
            var HidCompanyType = $("#HidCompanyType").val();
            if (parseInt(HidUserType) == 1) {
                //根据社会类型判断选择单位
                switch (parseInt(HidCompanyType)) {
                    case 1: //社会单位
                        $("#dept1").show();
                        $("#dept2").hide();
                        $("#dept3").hide();
                        break;
                    case 3:
                    case 4:
                    case 5:
                        //调用单位
                        GetCompanyByAttrList();
                        $("#dept1").hide();
                        $("#dept2").show();
                        $("#dept3").hide();
                        break;
                }
            }
            else {
               
                $("#dept1").hide();
                $("#dept2").hide();
                $("#dept3").show();
            }
           
            $("#two").hide();
            $("#three").show();
            $("#grxx").attr("class", "done");
            $("#zjxx").attr("class", "current_prev");
            $("#qzxx").attr("class", "current");
        }

        //普通用户选择单位
        function GetCompanyByAttrList() {
            $("#DeptCompany").empty();
            $.ajax({
                url: '/SysCompany/GetCompanyByAttrList?attr=' + $("#HidCompanyType").val() + "&data=" + new Date(),
                async: false,
                dataType: "json",
                success: function (result) {
                    if (result.length > 0) {
                        $("#DeptCompany").append("<option value=''>==请选择单位==</option>");
                        for (var i = 0; i < result.length; i++) {
                            $("#DeptCompany").append("<option value='" + result[i].ID + "'>&nbsp;&nbsp;&nbsp;" + result[i].Name + "</option>");
                        }
                    }
                    else {
                        $("#DeptCompany").append("<option value=''>==暂无单位==</option>");
                    }
                },
                error: function () {
                }
            });
        }

        //其他单位选择
        function setCompanyCode(obj) {
            $("#HidCompanyCode").val(obj.value);
        }

        //切换单位信息
        function setCode(obj) {
            $("#HidCompanyCode").val(obj.value);
            $("#HidDeptCode").val("");
            GetDeptList();
        }


        //切换面板3
        function three() {
            var HidUserType = $("#HidUserType").val(); //用户类型
            var HidCompanyType = $("#HidCompanyType").val();//单位类型
            if (parseInt(HidUserType) == 1) {
                //根据社会类型判断选择单位
                switch (parseInt(HidCompanyType)) {
                    case 1: //社会单位
                        var HidCompanyCode = $("#HidCompanyCode").val().trim();
                        var HidDeptCode = $("#HidDeptCode").val().trim();
                        if (HidCompanyCode == "") {
                            $("#threeMsg").html("请选择单位名称!");
                            $("#Msgthree").css("display", "block");
                            return false;
                        }
                        if (HidDeptCode == "") {
                            $("#threeMsg").html("请选择部门名称!");
                            $("#Msgthree").css("display", "block");
                            return false;
                        }
                        //隐藏状态
                        $("#threeMsg").html("");
                        $("#Msgthree").css("display", "none");
                        break;
                    case 3:
                    case 4:
                    case 5:
                        var CompanyCode = $("#HidCompanyCode").val().trim();
                        if (CompanyCode == "") {
                            $("#DivDeptMsg").html("请选择单位名称!");
                            $("#DivDept").css("display", "block");
                            return false;
                        }
                        //隐藏状态
                        $("#DivDeptMsg").html("");
                        $("#DivDept").css("display", "none");
                        break;
                }
            }
            else {
                var CompanyCode = $("#HidCompanyCode").val().trim();
                if (CompanyCode == "") {
                    $("#DivDeptMsg").html("请选择单位名称!");
                    $("#DivDept").css("display", "block");
                    return false;
                }
                //隐藏状态
                $("#DivDeptMsg").html("");
                $("#DivDept").css("display", "none");
            }
            SaveDate();
           
        }
        //保存数据
        function SaveDate() {
            var txtUserName = $("#txtUserName").val().trim(); //用户姓名
            var HidUserType = $("#HidUserType").val(); //用户类型
            var HidCompanyType = $("#HidCompanyType").val(); //单位类型
            var UserName = $("#txtRegisterName").val().trim(); //用户名
            var Pwd = $("#txtRegisterPwd").val().trim(); //密码
            var HidCompanyCode = $("#HidCompanyCode").val(); //单位编码
            var HidDeptCode = $("#HidDeptCode").val(); //部门编码

            var txtCompanyName = $("#txtCompanyName").val().trim(); //公司名称
            var txtLinkUser = $("#txtLinkUser").val();//联系人
            var txtLinkPhone = $("#txtLinkPhone").val();//联系电话
            var Bx = $("#Bx").val().trim(); //经度
            var By = $("#By").val().trim(); //纬度
            var Address = $("#Address").val().trim(); //单位地址
            loadingOpen("正在提交数据,请稍候。。。");
            //保存数据
            Ext.Ajax.request({
                url: '/Home/SaveRegedit',
                params: {UserName: txtUserName, LoginName: UserName, pwd: Pwd, CompanyCode: HidCompanyCode, DeptCode: HidDeptCode, CompanyName: txtCompanyName, Bx: Bx, By: By, Address: Address, UserType: HidUserType, CompanyType: HidCompanyType, LinkUser: txtLinkUser, LinkPhone: txtLinkPhone },
                success: function (response) {
                    loadingClose(); //取消遮罩
                    var rs = eval('(' + response.responseText + ')');
                    if (rs.success) {
                        $("#three").hide();
                        $("#four").show();

                        $("#grxx").attr("class", "done");
                        $("#zjxx").attr("class", "done");
                        $("#qzxx").attr("class", "current_prev");
                        $("#qzfs").attr("class", "current");
                    } else {
                        alert(rs.msg);
                        return false;
                    }
                },
                failure: function (form, action) {
                    loadingClose(); //取消遮罩
                    alert("注册失败!");
                }
            });
        }

        //新建单位
        function NewCreate() {
            $("#dept1").hide();
            $("#dept2").hide();
            $("#dept3").show();
        }
        //第二个面板上一步
        function reone() {
            $("#one").show();
            $("#two").hide();
            $("#grxx").attr("class", "current");
            $("#zjxx").attr("class", "");
        }
        //第三个面板上一步
        function retwo() {

            $("#HidIsCreate").val("0"); //是否新建 0:不是 1:是
            $("#HidCompanyCode").val("");
            $("#HidDeptCode").val("");


            $("#three").hide();
            $("#two").show();
            $("#grxx").attr("class", "current_prev");
            $("#zjxx").attr("class", "current");
            $("#qzxx").attr("class", "");
        }
       
        //确定
        function successBtn() {
            window.location.href = "/Home/Login";
        }
        ///验证用户手机号是否注册
        function VilivaName(key, value) {
            var respon = Ext.lib.Ajax.getConnectionObject().conn;
            respon.open("post", "/SysMaster/ExitsMaster?key=" + key + "&code=" + encodeURIComponent(value.trim()), false);
            respon.send(null);
            var response = Ext.util.JSON.decode(respon.responseText);
            if (response.success) {

                return true;
            }
            else {
                return false;
            }
        }

        //用户类型选择
        function UserTypeChk(obj) {
            var val = $('input:radio[name="UserType"]:checked').val();
            $("#HidUserType").val(val); //单位管理员
        }
        //地图选择
        function SelectMap() {
            WindowMap('', '').show();
        }

        //调用地图窗口
        function WindowMap(x, y) {
            var win = new top.Ext.Window({
                id: 'BaiDuMap',
                title: "地图",
                width: 850,
                height: 458,
                layout: 'fit',
                shadow: false,
                stateful: false,
                border: false,
                modal: true,
                autoScroll: false,
                closeAction: 'close',
                items: {
                    autoScroll: true,
                    autoLoad: { url: '../../Project/Html/Map.htm', scripts: true, nocache: true },
                    params: { x: x, y: y, address: top.$("#Address").val() }
                },
                buttons: [{
                    text: '确定',
                    iconCls: 'GTP_submit',
                    handler: function () {
                        top.$("#Bx").val(top.$("#x").val());
                        top.$("#By").val(top.$("#y").val());
                        $("#Address").val(top.$("#address").val());
                        top.Ext.getCmp("BaiDuMap").close(); //直接销毁
                    }
                }, {
                    text: '取消',
                    iconCls: 'GTP_cancel',
                    handler: function () {
                        top.Ext.getCmp("BaiDuMap").close(); //直接销毁
                    }
                }]
            });
            return win;
        }

        //保存单位注册
        function SaveCompany() {
            var txtCompanyName = $("#txtCompanyName").val();
            if (txtCompanyName == "") {
                $("#CityTipMsg").html("单位名称不能为空!");
                $("#CityTip").css("display", "block");
                return false;
            }
            var txtLinkUser = $("#txtLinkUser").val();
            if (txtLinkUser == "") {
                $("#CityTipMsg").html("联系人名称不能为空!");
                $("#CityTip").css("display", "block");
                return false;
            }
            var txtLinkPhone = $("#txtLinkPhone").val();
            if (txtLinkPhone == "") {
                $("#CityTipMsg").html("联系电话不能为空!");
                $("#CityTip").css("display", "block");
                return false;
            }
//            var Bx = $("#Bx").val();
//            var By = $("#By").val();
//            if (By == "" || Bx == "") {
//                $("#CityTipMsg").html("请选择单位所在地址!");
//                $("#CityTip").css("display", "block");
//                return false;
//            }

            $("#HidIsCreate").val("1"); //是否新建 0:不是 1:是

            SaveDate();
//            $("#three").hide();
//            $("#four").show();

//            $("#grxx").attr("class", "done");
//            $("#zjxx").attr("class", "done");
//            $("#qzxx").attr("class", "current_prev");
//            $("#qzfs").attr("class", "current");
        }

        //多选单位
        function SelectMulite() {
            //获取选中的信息
            var selectList = $("#HidDeptCompanyId").val();
            new top.Ext.Window({
                id: "WinMasterList",
                width: 320,
                height: 470,
                closable: false,
                border: false,
                shadow: false,
                modal: true,
                layout: 'fit',
                plain: true,
                autoDestroy: true,
                closeAction: 'close',
                title: '单位选择',
                items: {
                    autoScroll: true,
                    autoLoad: { url: '../../Project/Html/SelectCompany.htm', scripts: true, nocache: true },
                    params: { Attribute: $("#HidCompanyType").val(), select: selectList }
                },
                buttons: [
                    {
                        text: '确定',
                        iconCls: 'GTP_submit',
                        tooltip: '保存当前的选择',
                        handler: function () {
                            SaveSelect();
                        }
                    },
                    {
                        text: '取消',
                        iconCls: 'GTP_cancel',
                        tooltip: '取消当前的操作',
                        handler: function () {
                            top.Ext.getCmp('WinMasterList').close();
                        }
                    }]
            }).show();
        }

        //保存单位选择
        function SaveSelect() {
            var checkid = '';
            var checkName = '';
            var rt = top.rmtree.getChecked(); //得到所有所选的子节点
            var count = 0;
            if (rt.length > 0) {
                for (var i = 0; i < rt.length; i++) {//除去最顶端的根节点(模块功能分配)
                    if (rt[i].getUI().checkbox) {
                        if (!rt[i].getUI().checkbox.indeterminate) {
                            checkid += rt[i].id + ',';
                            checkName += rt[i].text + ',';
                            count++;
                        }
                    }
                }
            }
            else {
                top.Ext.Msg.show({ title: "信息提示", msg: '请先进行选择', buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                return false;
            }
            if (count > 10) {
                top.Ext.Msg.show({ title: "信息提示", msg: '最多只能选择10个单位,请重新选择.', buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                return false;
            }

            $("#HidIsCreate").val("0"); //是否新建 0:不是 1:是

            checkid = checkid.substring(0, checkid.length - 1);
            checkName = checkName.substring(0, checkName.length - 1);
            $("#HidDeptCompanyId").val(checkid);
            $("#DeptCompany").val(checkName);
            top.Ext.getCmp('WinMasterList').close();
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
</script>
</head>
<body>
    <!--模版2-->
    <div class="header">
        <div class="navbar cf">
            <img class="logo" src="../../Resource/img/login/temple1/logo.png" style="width: 54px;"
                alt="">
            <h2 class="title">
                欢迎登录乐消消防后台信息管理系统</h2>
            <ul class="nav cf" id="head-nav-menu">
                <li class="active"><a class="nav-item-link" href="/Home/Login">首页</a></li>
                <%--<li><a class="nav-item-link" href="#" target="_blank">公司简介</a></li>
                <li><a class="nav-item-link" href="#" target="_blank">产品介绍</a></li>
                <li><a class="nav-item-link" href="#" target="_blank">精品案例</a></li>
                <li><a class="nav-item-link" href="#" target="_blank">行业方案</a></li>
                <li><a class="nav-item-link" href="#" target="_blank">渠道合作</a></li>
                <li><a class="nav-item-link" href="http://aspx.taobao.com" target="_blank">联系我们</a></li>--%>
            </ul>
        </div>
    </div>
    <script>        $('#head-nav-menu li:eq(0)').addClass('active');</script>
    <div class="wrapper">
        <div class="main sign-up">
            <div class="hd">
                <p class="description">
                    Hi，新朋友，欢迎注册乐消消防平台，以下各项均必填。如注册遇到困难，请联系我们哦.
                </p>
            </div>
            <div class="bd">
                <div class="section cf">
                    <div class="left-mod">
                        <div class="sign-up-mod">
                            <div class="formbox">
                                <div class="flow_steps">
                                    <ul>
                                        <li id="grxx" class="current">个人信息</li>
                                        <li id="zjxx">注册类型</li>
                                        <li id="qzxx">单位选择</li>
                                        <li id="qzfs" class="last">成功注册</li>
                                    </ul>
                                </div>
                                <div class="formcon">
                                    <div id="one">
                                        <table align="center" style="width: 70%">
                                            <tbody>
                                                 <tr>
                                                    <td align="right" width="140px">
                                                        <span class="required">*</span>真实姓名：
                                                    </td>
                                                    <td>
                                                        <input class="form-control" style="width: 100%" type="text" name="txtUserName"
                                                            id="txtUserName" placeholder="真实姓名" title="输入真实姓名" onkeydown="return bubufx_enterpress(event);">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" width="140px">
                                                        <span class="required">*</span>登录帐号：
                                                    </td>
                                                    <td>
                                                        <input class="form-control" style="width: 100%" type="text" name="txtRegisterName"
                                                            id="txtRegisterName" placeholder="手机号" title="输入手机号" onkeydown="return bubufx_enterpress(event);">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <span class="required">*</span>密码：
                                                    </td>
                                                    <td>
                                                        <input class="form-control" style="width: 100%" type="password" name="txtRegisterPwd"
                                                            id="txtRegisterPwd" placeholder="密码" title="输入登录密码" onkeydown="return bubufx_enterpress(event);">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <span class="required">*</span>确认密码：
                                                    </td>
                                                    <td>
                                                        <input class="form-control" style="width: 100%" type="password" name="txtRegisterPwd2"
                                                            id="txtRegisterPwd2" placeholder="确认密码" title="输入登录密码" onkeydown="return bubufx_enterpress(event);">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <div id="Msgone" class="" style="height: 25px; display: none">
                                                            <img class="g-tips-ico" alt="" src="../../Resource/Img/login/s.gif" />
                                                            <div class="g-tips-content g-font-red" id="oneMsg">
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <br />
                                                        <input name="button" onclick="one()" type="button" class="button button-primary button-metro"
                                                            value="下一步">
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <div id="two" style="display: none">
                                        <!--用户类型-->
                                         <input type="hidden" id="HidUserType" value="1" />
                                          <!--单位类型-->
                                        <input type="hidden" id="HidCompanyType" value="1" />
                                        <!--社会单位-->
                                        <div id="UseType1" tyle="display: none">
                                            <table align="center" style="width: 90%">
                                                <tbody>
                                                    <tr>
                                                        <td colspan="3" style="color: Orange">
                                                            <p style="margin-left: 50px;">
                                                                请选择注册类型,一旦成功创建帐号,类型不可更改.</p>
                                                            <br />
                                                            <hr style=" color:Silver; border-style:dashed silver;" />
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" width="140px">
                                                            用户类型:
                                                        </td>
                                                        <td align="left" style="text-align: left">
                                                           
                                                                <input type="radio" name="UserType" id="chk1" value="1" checked onclick="UserTypeChk(this)" /><label
                                                                    for="chk1">普通用户
                                                                </label>
                                                           
                                                        </td>
                                                        <td>
                                                         &nbsp;&nbsp;&nbsp;
                                                                <input type="radio" name="UserType" id="chk2" value="2" onclick="UserTypeChk(this)" /><label
                                                                    for="chk2">
                                                                    单位管理员
                                                                </label>
                                                           
                                                        </td>
                                                    </tr>
                                                    <tr style="color:#6a6a6a">
                                                        <td>
                                                        </td>
                                                        <td style=" border-right:1px dashed">
                                                            单位的安全员，消防参谋，维护公司员工，器材商员工请选择本模块完成注册.
                                                        </td>
                                                        <td>
                                                             &nbsp;&nbsp;&nbsp;新单位创建，单位信息注册请由单位消防主管部门人员选择本模块完成注册，单位不能重复注册，请确认没有单位其他人员在本系统注册过单位.
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td colspan="2">
                                                            <br />
                                                            <input name="button" onclick="reone()" type="button" class="button button-metro"
                                                                value="上一步">
                                                            &nbsp; &nbsp; &nbsp;
                                                            <input name="button" onclick="UserTypeTwo()" type="button" class="button button-primary button-metro"
                                                                value="下一步">
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                        <div id="UseType2" style="display: none">
                                            <table align="center" style="width: 90%">
                                                <tbody>
                                                    <tr>
                                                        <td align="right" width="120px">
                                                            单位类型:
                                                        </td>
                                                        <td align="left">
                                                            <input type="radio" name="CompanyType" id="Radio7" value="1" checked onclick="AdminTypeChk(this)" /><label
                                                                for="Radio7">社会单位</label>&nbsp;&nbsp;
                                                        </td>
                                                        <td>
                                                          <input type="radio" name="CompanyType" id="Radio8" value="3" onclick="AdminTypeChk(this)" /><label
                                                                for="Radio8">消防单位</label>&nbsp;&nbsp;
                                                        </td>
                                                         <td>
                                                        <input type="radio" name="CompanyType" id="Radio9" value="4" onclick="AdminTypeChk(this)" /><label
                                                                for="Radio9">维保公司</label>&nbsp;&nbsp;
                                                        </td>
                                                         <td>
                                                          <input type="radio" name="CompanyType" id="Radio10" value="5" onclick="AdminTypeChk(this)" /><label
                                                                for="Radio10">器材供应商</label>
                                                        </td>
                                                    </tr>
                                                   <tr style="color:#6a6a6a">
                                                        <td align="right" width="120px">
                                                           
                                                        </td>
                                                        <td style=" border-right:1px dashed">
                                                           企事业单位、公司等需要使用本信息系统的单位请选择.
                                                        </td>
                                                       <td style=" border-right:1px dashed">
                                                        消防主管部门，负责监督指导社会单位消防工作的单位请选择.
                                                        </td>
                                                       <td style=" border-right:1px dashed">
                                                       承担消防系统维护保养的单位、公司请选择.
                                                        </td>
                                                         <td>
                                                        销售、维护、回收消防器材、设备的公司请选择.
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td colspan="4">
                                                            <br />
                                                            <input name="button" onclick="reUserTypeTwo()" type="button" class="button button-metro"
                                                                value="上一步">
                                                            &nbsp; &nbsp; &nbsp;
                                                            <input name="button" onclick="two()" type="button" class="button button-primary button-metro"
                                                                value="下一步">
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                    <div id="three" style="display: none">
                                        <input type="hidden" id="HidCompanyCode" />
                                        <input type="hidden" id="HidDeptCode" />
                                        <input type="hidden" id="HidIsCreate" value="0" />
                                        <!--社会单位-->
                                        <div id="dept1" style="display: none">
                                            <table align="center" style="width: 70%">
                                                <tbody>
                                                    <tr>
                                                        <td align="right" width="140px">
                                                            <span class="required">*</span>单位名称:
                                                        </td>
                                                        <td>
                                                            <select id="CompanyName" class="form-control m-b" style="width: 100%" name="CompanyName"
                                                                onchange="setCode(this)">
                                                            </select>
                                                        </td>
                                                        <td>
                                                           <%-- <a href="#" onclick="NewCreate()" title="没有选择吗?赶快新建一个">新建</a>--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <span class="required">*</span>部门名称:
                                                        </td>
                                                        <td>
                                                            <select id="DeptName" class="form-control m-b" style="width: 100%" name="DeptName"
                                                                onchange="setDeptCode(this)">
                                                            </select>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <div id="Msgthree" class="" style="height: 25px; display: none">
                                                                <img class="g-tips-ico" alt="" src="../../Resource/Img/login/s.gif" />
                                                                <div class="g-tips-content g-font-red" id="threeMsg">
                                                                </div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <br />
                                                            <input name="button" onclick="retwo()" type="button" class="button button-metro"
                                                                value="上一步">
                                                            &nbsp; &nbsp; &nbsp;
                                                            <input name="button" onclick="three()" type="button" class="button button-primary button-metro"
                                                                value="下一步">
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                        <!--消防,维保,供应商单位-->
                                        <div id="dept2" style="display: none">
                                            <table align="center" style="width:70%">
                                                <tbody>
                                                    <tr>
                                                        <td align="right" width="140px">
                                                            <span class="required">*</span>单位名称:
                                                        </td>
                                                        <td>
                                                           <select id="DeptCompany" class="form-control m-b" style="width: 100%" name="DeptCompany"
                                                                onchange="setCompanyCode(this)">
                                                            </select>
                                                        
                                                        </td>
                                                       
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <div id="DivDept" class="" style="height: 25px; display: none">
                                                                <img class="g-tips-ico" alt="" src="../../Resource/Img/login/s.gif" />
                                                                <div class="g-tips-content g-font-red" id="DivDeptMsg">
                                                                </div>
                                                            </div>
                                                        </td>
                                                       
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <br />
                                                            <input name="button" onclick="retwo()" type="button" class="button button-metro"
                                                                value="上一步">
                                                            &nbsp; &nbsp; &nbsp;
                                                            <input name="button" onclick="three()" type="button" class="button button-primary button-metro"
                                                                value="下一步">
                                                        </td>
                                                       
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                        <!--单位新增-->
                                        <div id="dept3" style="display: none; width: 100%; text-align: center">
                                            <table align="center" style="width: 70%; margin-left: 50px">
                                                <tbody>
                                                    <tr>
                                                        <td align="right" width="140px">
                                                            <span class="required">*</span>单位名称：
                                                        </td>
                                                        <td colspan="3" align="left">
                                                            <div class="form-group">
                                                                <input class="form-control" type="text" name="txtCompanyName" id="txtCompanyName"
                                                                    placeholder="单位名称" onkeydown="return bubufx_enterpress(event);">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr style="display:none">
                                                        <td align="right" width="140px">
                                                            经纬度：
                                                        </td>
                                                        <td align="left">
                                                            <div class="form-group">
                                                                <input type="text" style="padding: 8px 5px; width: 80px;" name="Bx" id="Bx" placeholder="经度X"
                                                                    readonly>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="form-group">
                                                                <input type="text" style="padding: 8px 5px; width: 80px;" name="By" id="By" placeholder="经度Y"
                                                                    readonly>
                                                            </div>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                     <tr>
                                                        <td align="right" width="140px">
                                                            <span class="required">*</span>联系人：
                                                        </td>
                                                        <td colspan="3" align="left">
                                                            <div class="form-group">
                                                                <input class="form-control" type="text" name="txtLinkUser" id="txtLinkUser"
                                                                    placeholder="联系人名称" onkeydown="return bubufx_enterpress(event);">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                     <tr>
                                                        <td align="right" width="140px">
                                                            <span class="required">*</span>联系电话：
                                                        </td>
                                                        <td colspan="3" align="left">
                                                            <div class="form-group">
                                                                <input class="form-control" type="text" name="txtLinkPhone" id="txtLinkPhone"
                                                                    placeholder="联系电话" onkeydown="return bubufx_enterpress(event);">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" width="140px">
                                                            所在地址：
                                                        </td>
                                                        <td colspan="2" align="left">
                                                            <div class="form-group">
                                                                <input class="form-control" style="width:210px;" type="text" name="Address" id="Address" placeholder="所在地址">
                                                            </div>
                                                        </td>
                                                        <td>
                                                          <div class="form-group">
                                                                <input name="button" type="button" class="button button-metro" value="地 图" onclick="SelectMap();">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" width="140px">
                                                        </td>
                                                        <td align="left" colspan="3">
                                                            <div id="CityTip" class="" style="height: 25px; display: none">
                                                                <img class="g-tips-ico" alt="" src="../../Resource/Img/login/s.gif" />
                                                                <div class="g-tips-content g-font-red" id="CityTipMsg">
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" width="140px">
                                                        </td>
                                                        <td colspan="3" align="left">
                                                            <div class="form-group">
                                                                <br />
                                                                <input name="button" onclick="retwo()" type="button" class="button button-metro"
                                                                    value="上一步">
                                                                &nbsp; &nbsp; &nbsp;
                                                                <input id="Button2" name="button" type="button" class="button button-primary button-metro"
                                                                    value="下一步" onclick="SaveCompany();">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                    <div id="four" style="display: none">
                                        <table align="center">
                                            <tbody>
                                                <tr>
                                                    <td align="center" width="140px" rowspan="2" valign="top">
                                                        <img src="../../Resource/img/rightL.jpg" style="width: 50px; margin-top: 10px" />
                                                        <br />
                                                        <br />
                                                    </td>
                                                    <td>
                                                        <h3 style="color: Green">
                                                            恭喜您,注册成功!</h3>
                                                        <br />
                                                        您提交的信息将在1~3个工作日内审核完毕,审核后将短信通知您.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <br />
                                                        <br />
                                                        <input name="button" onclick="successBtn()" type="button" class="button button-primary button-metro"
                                                            value="返回登录">
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="right-mod">
                        <div class="sign-in-mod">
                            <h3 class="title">
                                已注册请登录</h3>
                            <div id="divmsg" class="" style="height: 25px; display: none;">
                                <img class="g-tips-ico" alt="" src="../../Resource/Img/login/s.gif" />
                                <div class="g-tips-content g-font-red" id="msg">
                                </div>
                            </div>
                            <form id="loginForm" action="/Home/SubmitLogin" method="post">
                            <div class="form-group">
                                <input class="form-control" type="text" name="txtLoginName" id="txtLoginName" placeholder="用户名"
                                    onkeydown="return bubufx_enterpress(event);">
                            </div>
                            <div class="form-group">
                                <input class="form-control" type="password" name="txtPwd" id="txtPwd" placeholder="密码"
                                    onkeydown="return bubufx_enterpress(event);">
                            </div>
                            <div class="form-group">
                                <input id="btLogin" name="button" type="button" class="button button-primary button-metro"
                                    value="登录" onclick="return checkData();">
                                <a class="normal-link" href="tencent://message/?uin=1101350716&amp;Menu=yes">忘记密码？</a>
                            </div>
                            </form>
                            <div class="ad">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="footer footer-temp">
        Copyright @ 2016
        <%= ViewData["copyright"] %>
        版权所有  <%= ViewData["Version"]%></div>
</body>
</html>
