var config = {
    root: "http://localhost:58995",
    pageSize: 10,
    base: "/manage/"
};

var AJAX = new function () {
    var _xhr,
        K = "_token",
        abs = null,
        t = this;

    function finish(v, cb) {
        if (cb == null) return;
        var o;
        if (v && v.length > 1) {
            try {
                o = JSON.parse(v);
            } catch (e) {
                o = v;
            }
        }
        if (o && o.code < 0) {
            Comm.message(o.msg);
        }
        if (o && o.code == 110) {
            Comm.msg("登录已过期，请重新登录", 5);
            setTimeout(function () {
                AJAX.clearTag();
                if (self == top) {
                    location.href = config.base + 'login.html';
                } else {
                    top.location.href = config.base + 'login.html';
                }
            }, 2000);
            return;
        }
        cb(o);
    }

    function ab() {
        if (abs == null) {
            abs = top.Comm.db(K);
            if (abs == null) abs = "";
        }
        return abs;
    }

    function repair(api) {
        var a = ab();
        if (a)
            api += (api.indexOf("?") > 0 ? "&" : "?") + K + "=" + a + "&timespan=" + Math.random();
        return api;
    }

    function deobj(obj) {
        if (obj == null) return "";
        var s = [];
        for (var i in obj) {
            if (typeof obj[i] == typeof "") {
                if (obj[i].indexOf("%") > 0) obj[i] = obj[i].replace(/%/g, "%25");
                if (obj[i].indexOf("&") > 0) obj[i] = obj[i].replace(/\&/g, "%26");
                if (obj[i].indexOf("+") > 0) obj[i] = obj[i].replace(/\+/g, "%2B");
            }
            s.push(i + "=" + encodeURIComponent(obj[i]));
        }
        return s.join("&");
    }

    function error(code, cb) {
        Comm.loading();
        cb && cb({ code: -1, msg: "服务器异常" });
    }

    function init(post, url, data, cb, asyn) {
        url = t.Uri() + repair(url);
        if (asyn == null) asyn = true;
        var xhr = new XMLHttpRequest();
        xhr.onreadystatechange = function () {
            if (this.readyState == 4) {
                if (this.status == 200) {
                    finish(this.responseText, cb);
                } else {
                    error(this.status, cb);
                }
            }
        };
        xhr.open(post ? "POST" : "GET", url, asyn);
        if (post) {
            data = deobj(data);
            xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        }
        xhr.send(data);
    }

    /*----AJAX公用方法-----*/

    /*获取服务器接口根路径*/
    t.Uri = function () {
        return window["config"] && window["config"]["root"] ? config.root : "";
    };

    /*获取服务器调试页面根路径*/
    t.WebRoot = function () {
        return window["config"] && window["config"]["webroot"] ? config.webroot : "";
    };

    /*调用用户登录token*/
    t.setTag = function (a) {
        abs = a;
        Comm.db(K, abs);
    };

    /*用户登录成功*/
    t.setLogin = function (d) {
        var u = d.data.customerInfo;
        var s = d.data.storeInfo;
        AJAX.setTag(u.token);
        Comm.db("_userinfo", {
            id: u.userId,
            p: u.phone,
            status: u.status,
            headImg: s.companyLogo,
            nickname: u.nickname,
            realname: u.realname,
            phone: s.phone
        });
    };

    /*退出登录清除token*/
    t.clearTag = function () {
        Comm.db(K, null);
        Comm.db("_userinfo", null);
    };

    /*执行GET方法，一般用于从服务器获取数据，api长度尽量不超过1000字节*/
    t.GET = function (api, data, cb, asyn) {
        //init(false, api, null, cb, asyn);
        $.ajax({
            url: config.root + api,
            data: data,
            type: "GET",
            dataType: "json",
            //contentType:"application/json; charset=utf-8",
            success: function (res) {
                cb & cb(res)
            },
            error: function () {
                Comm.loading();
                cb && cb({ code: -1, msg: "服务器异常" });
            }
        })
    };
    /*执行POST方法，一般用于向服务器提交数据，data建议不为空*/
    t.POST = function (api, data, cb, asyn) {
        //init(true, api, data, cb, true);
        $.ajax({
            url: config.root + api,
            data: data,
            type: "POST",
            dataType: "json",
            //contentType:"application/json;charset=utf-8",
            success: function (res) {
                cb & cb(res)
            },
            error: function () {
                Comm.loading();
                cb && cb({ code: -1, msg: "服务器异常" });
            }
        })
    };
    /*根据用户凭证判断用户是否登录*/
    t.isLogin = function () {
        return ab().length > 0;
    };
}();
var Comm = {
    Cookie: {
        set: function (name, value) {
            document.cookie = name + "=" + escape(value);
        },
        get: function (name) {
            var arr,
                reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
            if ((arr = document.cookie.match(reg))) return unescape(arr[2]);
            else return "";
        }
    },
    parse: function (s) {
        var o;
        try {
            o = JSON.parse(s);
        } catch (e) {
            o = s;
        }
        return o;
    },
    deData: function (s) {
        if (s && s.indexOf("/") > -1) {
            s = decodeURIComponent(s.replace(/\//g, "%"));
        }
        return Comm.parse(s);
    },
    enData: function (o) {
        return encodeURIComponent(JSON.stringify(o)).replace(/\%/g, "/");
    },
    db: function (t, v) {
        if (v == null) {
            if (arguments.length === 1) {
                return Comm.deData(Comm.Cookie.get(t));
            } else {
                Comm.Cookie.set(t, "");
            }
        } else {
            Comm.Cookie.set(t, Comm.enData(v));
        }
    },
    //调用方法Comm.OSS.getImgUrl(uri,'l');图片前缀更新方法
    OSS: {
        /*阿里云oss工具*/
        host: function () {
            return window["config"] && window.config["ossroot"] ? config.ossroot : "";
        },
        inithtml: function (d, cb) {
            top.Comm.laytpl(top.$("#upimgTpl").html()).render(d, function (html) {
                cb && cb(html)
            })
        },
        /*oss访问地址*/
        /*
            获取图片访问地址
            uri:数据库中保存的文件地址
            type:显示类型 	取值:s|m|l
             */
        getImgUrl: function (uri, type) {

            return config.root + uri;

            if (uri == null) return "-----------error";
            if (uri.length >= 4 && uri.indexOf("http") > -1) return uri;
            var url = Comm.OSS.host() + uri;
            if (type) {
                switch (type) {
                    case "l":
                        url += "/800";
                        break;
                    case "f":
                        url += "/400";
                        break;
                    case "m":
                        url += "/250";
                        break;
                    case "s":
                        url += "/120";
                        break;
                    default:
                        break;
                }
            }
            return url;
        }
    },
    //创建对象，并追加到f里面，attr:对象属性
    create: function (att, f, tag) {
        var a = document.createElement(tag ? tag : "div");
        if (att) for (var i in att) a[i] = att[i];
        (Comm.g(f) || document.body).appendChild(a);
        return a;
    },
    g: function (o) {
        if (typeof o == typeof {}) return o;
        return document.getElementById(o);
    },
    //尝试执行根方法
    exec: function (m) {
        if (window[m]) {
            var a = [];
            for (var i = 1; i < arguments.length; i++)
                a[i - 1] = arguments[i];
            window[m].apply(null, a);
        }
    },
    //显示加载s为需要显示的内容,s为false则表示关闭
    loading: function (s) {
        if (s) {
            layer.load();
        } else {
            layer.closeAll('loading');
        }
    },
    msg: function (msg, type) {
        Comm.layer.msg(msg, {
            icon: type
        });
    },
    alert: function (str) {
        Comm.layer.alert(str, { icon: 1 });
    },
    //confirm cb返回true,false
    confirm: function (str, cb) {
        Comm.layer.confirm(str, {
            icon: 3,
            title: "提示"
        }, function (index) {
            cb && cb(1);
            layer.close(index);
        })
    },
    //查询url参数，n:名称
    query: function (n, u) {
        var s = u;
        if (s == null) s = self.location.href;
        if (n) {
            var g = new RegExp("(\\?|&)" + n + "=([^&|#]*)");
            var r = s.match(g);
            if (r) {
                try {
                    return decodeURIComponent(r[2]);
                } catch (err) {
                    return unescape(r[2]);
                }

            }
            else return "";
        } else {
            var i = s.indexOf("?");
            if (i === -1) return "";
            return s.substr(i + 1);
        }
    },
    go: function (r) {
        location.href = r;
    },
    price: function (v) {
        if (v == 0) {
            return v;
        }
        if (v != null && v != "" && v != undefined) {
            return (Number(v) / 100).toFixed(2);
        }
    },
    //时间转换函数
    format: function (stamp, type) {
        type = type && type.match(/\w+/g);
        var now = new Date(stamp),
            year = now.getFullYear(),
            month = now.getMonth() + 1,
            date = now.getDate(),
            hour = now.getHours(),
            minute = now.getMinutes(),
            second = now.getSeconds(),
            o = [],
            arr = {
                yyyy: year,
                mm: check(month),
                dd: check(date),
                h: check(hour),
                m: check(minute),
                s: check(second)
            };
        if (type) {
            for (var i = 0; i < type.length; i++) {
                var tmp = type[i];
                tmp = tmp.toLocaleLowerCase();
                if (arr[type[i]])
                    o.push(type[i] == "h" ? " " + arr[type[i]] : "-" + arr[type[i]]);
            }
            if (o.length) return o.join("").slice(1);
        } else
            return (
                arr.yyyy +
                "-" +
                arr.mm +
                "-" +
                arr.dd +
                " " +
                arr.h +
                ":" +
                arr.m +
                ":" +
                arr.s
            );

        function check(num) {
            if (num < 10) {
                return "0" + num;
            }
            return num;
        }
    },
    wait: 60,
    timeCountDownclick: true,
    timeCountDown: function (o, phone, type, imgcode) {
        /*发送验证码公用方法*/
        /*o 点击发送验证码按钮*/
        /*phone 发送短信手机号*/
        /*type 验证码类型*/
        /*imgcode 图片验证码，可不传*/
        /*调用 app.timeCountDown(this,15928877394,1,5421)*/
        //按钮倒计时
        imgcode = imgcode == undefined ? "" : imgcode;
        if (!phone) {
            layer.msg("请输入手机号");
            return;
        }
        var reg = /^1\d{10}$/;
        if (phone && !reg.test(phone)) {
            layer.msg("手机格式错误");
            return;
        }
        if (Comm.timeCountDownclick) {
            Comm.timeCountDownclick = false;
            o.setAttribute("disabled", true);
            AJAX.GET("/api/customer/sendSMS?phone=" + phone + "&type=" + type + "&_device=" + 1,
                function (d) {
                    if (d.code == 1) {
                        layer.msg("短信已发送，请注意查收");
                        var i = setInterval(function () {
                            if (Comm.wait == 0) {
                                o.removeAttribute("disabled");
                                o.value = "重新发送";
                                Comm.wait = 60;
                                clearInterval(i);
                            } else {
                                o.value = Comm.wait + "秒后重发";
                                Comm.wait--;
                            }
                        }, 1000);
                    } else {
                        o.removeAttribute("disabled");
                        layer.msg(d.msg);
                    }
                    Comm.timeCountDownclick = true;
                }
            );
        }
    },
    GetData: function (id) {
        var p = {};
        var id = id == undefined || id == "" ? "form" : id;
        $("#" + id).find("input,textarea,select").each(function (i, e) {
            if ($(e).attr("name") != undefined) {
                if ($(e).attr('laydate') != undefined) {
                    p[$(e).attr("name")] = $(e).val();
                } else {
                    p[$(e).attr("name")] = $(e).val();
                }
            }
        })
        return p;
    },
    SetData: function (p, id) {
        var id = id == undefined || id == "" ? "form" : id;

        $("#" + id).find("input,textarea,select").each(function (i, e) {
            if ($(e).attr("name") != undefined && $(e).attr("type") != "radio") {
                var v = p[$(e).attr("name")]
                if (void 0 !== v && null !== v && "null" !== v || (v = "")) {
                    $(e).val(v);
                }
                if (e.hasAttribute('laydate')) {
                    Comm.laydateinit(e);
                }
            }
        })
        return p;
    },
    isexit: [],
    laydateinit: function (e) {
        var c = {
            elem: "#" + $(e).attr("id"),
            type: $(e).attr("type") != undefined && $(e).attr("type") != '' ? $(e).attr("type") : 'date',
            range: $(e).attr("range") != undefined ? true : false,
        }
        if (e.hasAttribute("min")) {
            c.min = $(e).attr("min")
        }
        if (e.hasAttribute("max")) {
            c.max = $(e).attr("max")
        }
        if (e.hasAttribute("format")) {
            c.format = $(e).attr("format");
        }
        if ($(e).val() != "") {
            c.value = $(e).val();
        }
        $(e).attr("type", 'text');
        if (Comm.isexit.indexOf(c.elem) < 0) {
            Comm.isexit.push(c.elem)
            Comm.laydate.render(c);
        }
    },
    showimg: function (a) {
        Comm.layer.open({
            type: 1,
            title: '查看图片',
            maxmin: true,
            area: ['500px', '500px'],
            shade: 0.3,
            content: '<div style="text-align: center;"><img src="' + $(a).attr('src') + '" width="100%"/></div>'
        })
    },
    bitunencode: function (t, w) {//反解析 位运算的值
        var l = [];
        for (var i = 0; i < w; i++) {
            if (t & Math.round(Math.pow(2, i))) {
                console.log(i + 1)
                l.push(i + 1);
            }
        }
        return l;
    },
    bitencode: function (d) {
        //d:[1, 2, 4, 9, 12, 20, 26, 29, 30, 31]; 需要位运算的值
        var t = 0;
        for (var i = 0; i < d.length; i++) {
            t += Math.round(Math.pow(2, d[i] - 1))
            console.log(t)
        }
        return t;
    },
    //年级转换
    nclass: function (d) {
        var ncla = [];
        var a = [];
        console.log(d)
        if (typeof (d) == 'number') {
            a.push(d);
        } else {
            a = d;
        }
        for (var i = 0; i < a.length; i++) {
            if (a[i] == 1) {
                ncla.push('一年级');
            } else if (a[i] == 2) {
                ncla.push('二年级');
            } else if (a[i] == 3) {
                ncla.push('三年级');
            } else if (a[i] == 4) {
                ncla.push('四年级');
            } else if (a[i] == 5) {
                ncla.push('五年级');
            } else if (a[i] == 6) {
                ncla.push('六年级');
            } else if (a[i] == 7) {
                ncla.push('初一');
            } else if (a[i] == 8) {
                ncla.push('初二');
            } else if (a[i] == 9) {
                ncla.push('初三');
            } else if (a[i] == 10) {
                ncla.push('高一');
            } else if (a[i] == 11) {
                ncla.push('高二');
            } else if (a[i] == 12) {
                ncla.push('高三');
            } else {
                ncla.push('暂无年级');
            }
        }
        console.log(ncla)
        return ncla;
    },
    //计算年级班级
    getGradeByStudentYearAndTime: function (v) {
        //debugger;
        if (!v) {
            return ''
        }
        function getMonthAddDay(month, day) {
            var monthStr = "" + month;
            var dayStr = day >= 10 ? ("" + day) : ("0" + day);
            var monthDay = monthStr + dayStr;
            return monthDay / 1;
        }

        function comp(studentYear) {
            var now = new Date();

            var year = now.getFullYear();
            var month = now.getMonth() + 1;
            var day = now.getDate();
            var monthDay = getMonthAddDay(month, day);

            if (monthDay < 815) {
                return year - studentYear;
            }
            return year - studentYear + 1;
        }

        var r = [];
        var dd = v.split(',');
        for (var i = 0; i < dd.length; i++) {
            if (dd[i] !== "") {
                var str = [];
                if (dd[i].indexOf("@") >= 0) {
                    str = dd[i].split("@");
                } else {
                    str.push(dd[i]);
                }
                var startYear = str[0] * 1;//开始学年
                //根据 开始学年 计算 年级
                var nj = comp(startYear);
                var rs = Comm.nclass(nj);
                if (str.length > 1) {
                    //班级
                    rs = rs + str[1] + "班";
                }

                var robj = { rs: rs, nj: nj, bj: str[1] };
                r.push(robj);
            }
        }
        return r;
    },
    //计算年级班级-增减餐申请
    getbn: function (v) {
        function getMonthAddDay(month, day) {
            var monthStr = "" + month;
            var dayStr = day >= 10 ? ("" + day) : ("0" + day);
            var monthDay = monthStr + dayStr;
            return monthDay / 1;
        }

        function comp(studentYear) {
            var now = new Date();

            var year = now.getFullYear();
            var month = now.getMonth() + 1;
            var day = now.getDate();
            var monthDay = getMonthAddDay(month, day);
            if (monthDay < 215) {
                return year - studentYear;
            }
            if (monthDay > 215 && monthDay < 815) {
                return year - studentYear + 1;
            }
            return year - studentYear + 1;
        }

        var r = [];
        var dd = v.split(',');
        for (var i = 0; i < dd.length; i++) {
            var str = dd[i].split("@");
            var startYear = str[0] * 1;//开始学年
            //班级
            var bj = str[1];
            //根据 开始学年 计算 年级
            var nj = comp(startYear);
            r.push(Comm.nclass(nj));
        }
        return r;
    },
    yclx: function (d) {
        console.log(d)
        var t = [];
        for (var i = 0; i < d.length; i++) {
            if (d[i] == 1) {
                t.push('早餐');
            } else if (d[i] == 2) {
                t.push('午餐');
            } else if (d[i] == 3) {
                t.push('晚餐');
            }
        }
        return t;
    },
}


var $ = null;
//初始化页面
window.onload = function () {
    layui.config({
        base: 'inc/'
    });
    /**
    * 对layui进行全局配置
    */
    layui.use(['table', 'laydate', 'form', 'laytpl', 'layer', 'jquery', 'upload', 'element', 'layedit'], function () {
        //初始化,全局变量 不可污染
        Comm.table = layui.table, Comm.layedit = layui.layedit, Comm.laytpl = layui.laytpl, Comm.form = layui.form, Comm.laydate = layui.laydate, Comm.layer = layui.layer, $ = layui.jquery, Comm.upload = layui.upload;

        /*处理时间选择器*/
        $("input[laydate]").each(function (i, e) {
            Comm.laydateinit(e);
        })

        //处理页面table
        /*search*/
        Comm.search = function (id, table) {
            id = id ? id : 'search-form'
            table = table ? table : 'table'
            //执行重载
            Comm.table.reload(table, {
                page: { curr: 1 },//重新从第 1 页开始
                where: Comm.GetData(id)
            });
        }
        /*sort*/
        Comm.table.on('sort(table)', function (obj) {
            var p = Comm.GetData("search-form");
            p[obj.field] = obj.type
            Comm.table.reload('table', {
                page: { curr: 1 },//重新从第 1 页开始
                initSort: obj,
                where: p
            });
        })

        Comm.exec("pageload");
    });
};


//发送消息 sendMsg('1','aaaa','bbbbbbbb','3')
//msgType:
var sendMsg = function (msgType, title, content, itemType, customerId, itemId) {
    var opt = {
        messType: msgType,
        customerId: customerId,
        messTitle: title,
        content: content,
        itemType: itemType,
        itemId: itemId,
        userType: '1'
    }
    AJAX.POST('/api/mess/send', opt, function (d) {
        if (d.code == 1) {

        } else {
            Comm.message(d.msg);
        }

    });
}

var mtitle = {
    0: '增减餐审核通知',
    1: '终止合同审核通知',
    2: '售后审核通知',
    3: '校商供应关系通知'
}







