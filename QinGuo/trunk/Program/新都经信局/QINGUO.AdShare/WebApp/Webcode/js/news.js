function news(obj) {
    var name = "default";
    return obj == undefined || obj == null ? null :
    {
        init: function () {
            obj.data.author = obj.data.author != "" && obj.data.author != null ? "-" + obj.data.author : "";
            obj.data.time = "<span style='font-size:13px;color: #999;'>【" + getDateDiff(new Date(obj.data.createTime).getTime()) + obj.data.author + "】</span>";
            obj.data.name = obj.data.name + obj.data.time;
            switch (obj.style) {//新闻展示样式表
                case 1://大图640*300
                    style1();
                    break;
                case 2://大图加轮循 640*300
                    style2();
                    break;
                case 3://三张小图 220*150
                    style3();
                    break;
                case 4://两张小图 320*150
                    style3();
                    break;
                case 5://左图右文 150*150
                    style5();
                    break;
                case 6://左文右图 150*150
                    style6();
                    break;
                case 7://视频 640*300
                    style7();
                    break;
                default:
                    break;
            }
        }
    }
    function style1() {
        var _picurl = "";
        if (obj.data.imageList.length > 0)
            _picurl = comm.url + obj.data.imageList[0]._picurl;

        $(obj.id).append('<div class="marbom5 styl1" onclick="gonews(\'news.html?id=' + obj.data.id + '\')"><img class="lazy-load" style="widht:100%;min-height:200px;" src="' + _picurl + '" /><span class="title">' + obj.data.name + '</span></div>');

    }
    function style2() {
        var imglist = "";
        $.each(obj.data.imageList, function (i, e) {
            imglist += '<div class="swiper-slide"><img src="' + comm.url + e._picurl + '" style="width:100%;max-height:250px"/></div>';
        });
        $(obj.id).append('<div class="swiper-container styl3 marbom5" id="swiper_' + obj.data.id + '"  onclick="gonews(\'news.html?id=' + obj.data.id + '\')">\
                              <div class="swiper-wrapper">' + imglist + '\
                              </div>\
                              <div class="swiper-pagination"></div><span class="title">' + obj.data.name + '</span>\
                        </div>');
        $("#swiper_" + obj.data.id).swiper({
            loop: true,
            autoplay: 3000
        });
    }
    function style3() {
        var imglist = "";
        $.each(obj.data.imageList, function (i, e) {
            imglist += '<div class="weui-flex__item"><div class="placeholder"><img src="' + comm.url + e._picurl + '"  /></div></div>';
        });
        $(obj.id).append('<div class="weui-flex styl4" onclick="gonews(\'news.html?id=' + obj.data.id + '\')"">\
                                  <div class="weui-flex__item"><div class="title">' + obj.data.name + '</div></div>\
                             </div>\
                             <div class="weui-flex styl4 paddingbom10 borderbom">' + imglist + '\
                            </div>');

    }
    function style5() {
        var _picurl = "";
        if (obj.data.imageList.length > 0)
            _picurl = comm.url + obj.data.imageList[0]._picurl;
        $(obj.id).append('<div class="weui-panel__bd borderbom" onclick="gonews(\'news.html?id=' + obj.data.id + '\')">\
                          <a href="javascript:void(0);" class="weui-media-box weui-media-box_appmsg">\
                            <div class="weui-media-box__hd">\
                              <img class="weui-media-box__thumb" src="' + _picurl + '" alt="" >\
                            </div>\
                            <div class="weui-media-box__bd">\
                              <h4 class="weui-media-box__title" style="white-space: inherit;text-align: left;">' + obj.data.name + '</h4>\
                            </div>\
                          </a>\
                        </div>');
        //<p class="weui-media-box__desc lineheight20">' + obj.data.mark + '</p>\
    }
    function style6() {
        var _picurl = "";
        if (obj.data.imageList.length > 0)
            _picurl = comm.url + obj.data.imageList[0]._picurl;
        $(obj.id).append('<div class="weui-panel__bd borderbom" onclick="gonews(\'news.html?id=' + obj.data.id + '\')">\
                          <a href="javascript:void(0);" class="weui-media-box weui-media-box_appmsg">\
                            <div class="weui-media-box__bd">\
                              <h4 class="weui-media-box__title" style="white-space: inherit;text-align: left;">' + obj.data.name + '</h4>\
                            </div>\
                            <div class="weui-media-box__hd"  onclick="gonews(\'news.html\')">\
                              <img class="weui-media-box__thumb" src="' + _picurl + '" alt="">\
                            </div>\
                          </a>\
                        </div>');
    }
    function style7() {
        $(obj.id).append('<div class="marbom5 styl1" onclick="gonews(\'news.html?id=' + obj.data.id + '\')"><img src="images/blank.png" data-original="' + comm.url + obj.data.img + '" /><span class="title">' + obj.data.name + '</span></div>');
    }

    function getDateDiff(dateTimeStamp) {
        var minute = 1000 * 60;
        var hour = minute * 60;
        var day = hour * 24;
        var halfamonth = day * 15;
        var month = day * 30;
        var now = new Date().getTime();
        var diffValue = now - dateTimeStamp;
        if (diffValue < 0) { return; }
        var monthC = diffValue / month;
        var weekC = diffValue / (7 * day);
        var dayC = diffValue / day;
        var hourC = diffValue / hour;
        var minC = diffValue / minute;
        if (monthC >= 1) {
            result = "" + parseInt(monthC) + "月前";
        }
        else if (weekC >= 1) {
            result = "" + parseInt(weekC) + "周前";
        }
        else if (dayC >= 1) {
            result = "" + parseInt(dayC) + "天前";
        }
        else if (hourC >= 1) {
            result = "" + parseInt(hourC) + "小时前";
        }
        else if (minC >= 1) {
            result = "" + parseInt(minC) + "分钟前";
        } else
            result = "刚刚";
        return result;
    }
};

//新闻详情地址跳转处理
function gonews(url) {
    var user = comm.getStorage("user");
    if (user != null && user != "" && user.ismember) {//是会员
        url += "&oid=" + user.id;
    }
    //判断是否带入广告
    //location.href = url;
    mui.openWindow({
        url: url,
        id: url,
        createNew: false,//是否重复创建同样id的webview，默认为false:不重复创建，直接显示
        show: {
            autoShow: true,//页面loaded事件发生后自动显示，默认为true
            aniShow: "slide-in-right",//页面显示动画，默认为”slide-in-right“；
            duration: 100,//页面动画持续时间，Android平台默认100毫秒，iOS平台默认200毫秒；
            event: 'titleUpdate',//页面显示时机，默认为titleUpdate事件时显示
            extras: {}//窗口动画是否使用图片加速
        }
    });
}
//
function go(url) {
    //判断是否带入广告
    if (url != "")
        mui.openWindow({
            url: url,
            id: url,
            createNew: false,//是否重复创建同样id的webview，默认为false:不重复创建，直接显示
            show: {
                autoShow: true,//页面loaded事件发生后自动显示，默认为true
                aniShow: "slide-in-right",//页面显示动画，默认为”slide-in-right“；
                duration: 100,//页面动画持续时间，Android平台默认100毫秒，iOS平台默认200毫秒；
                event: 'titleUpdate',//页面显示时机，默认为titleUpdate事件时显示
                extras: {}//窗口动画是否使用图片加速
            }
        });
}
//采用正则表达式获取地址栏参数
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}
var comm = {
    url: "",
    url: "http://10.16.6.48:5009/",
    jsonToString: function (d) { return JSON.stringify(d); },
    stringToJson: function (d) { if (d) return eval('(' + d + ')'); return null; },
    getStorage: function (name) { return comm.stringToJson(top.window.localStorage[name]); },//获取本地数据缓存
    setStorage: function (name, data) { top.window.localStorage[name] = comm.jsonToString(data); },//插入本地数据缓存
    h: document.documentElement.clientHeight || document.body.clientHeight,
    w: document.documentElement.clientWidth || document.body.clientWidth,
    addCookie: function (name, value, expires, path, domain) {
        var str = name + "=" + escape(value);
        if (expires != "") {
            var date = new Date();
            date.setTime(date.getTime() + expires * 24 * 3600 * 1000);//expires单位为天 
            str += ";expires=" + date.toGMTString();
        }
        if (path != "") {
            str += ";path=" + path;//指定可访问cookie的目录 
        }
        if (domain != "") {
            str += ";domain=" + domain;//指定可访问cookie的域 
        }
        document.cookie = str;
    },
    getCookie: function (name) {//取得cookie 

        if (document.cookie.length > 0) {　　//先查询cookie是否为空，为空就return ""
            c_start = document.cookie.indexOf(name + "=")　　//通过String对象的indexOf()来检查这个cookie是否存在，不存在就为 -1　　
            if (c_start != -1) {
                c_start = c_start + name.length + 1　　//最后这个+1其实就是表示"="号啦，这样就获取到了cookie值的开始位置
                c_end = document.cookie.indexOf(";", c_start)　　//其实我刚看见indexOf()第二个参数的时候猛然有点晕，后来想起来表示指定的开始索引的位置...这句是为了得到值的结束位置。因为需要考虑是否是最后一项，所以通过";"号是否存在来判断
                if (c_end == -1) c_end = document.cookie.length
                return unescape(document.cookie.substring(c_start, c_end))　　//通过substring()得到了值。想了解unescape()得先知道escape()是做什么的，都是很重要的基础，想了解的可以搜索下，在文章结尾处也会进行讲解cookie编码细节
            }
        }
        return "";
    },
    delCookie: function (name) {//删除cookie 	
        var exp = new Date();
        exp.setTime(exp.getTime() - 1);
        var cval = getCookie(name);
        if (cval != null) document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString();
    },
    errorimg: function (a) {
        $(a).attr("src", "");
    }
}

function XmlToJson() {
}
XmlToJson.prototype.setXml = function (xml) {
    if (xml && typeof xml == "string") {
        this.xml = document.createElement("div");
        this.xml.innerHTML = xml;
        this.xml = this.xml.getElementsByTagName("*")[0];
    }
    else if (typeof xml == "object") {
        this.xml = xml;
    }
};
XmlToJson.prototype.getXml = function () {
    return this.xml;
};
XmlToJson.prototype.parse = function (xml) {
    this.setXml(xml);
    return this.convert(this.xml);
};
XmlToJson.prototype.convert = function (xml) {
    if (xml.nodeType != 1) {
        return null;
    }
    var obj = {};
    obj.xtype = xml.nodeName.toLowerCase();
    var nodeValue = (xml.textContent || "").replace(/(\r|\n)/g, "").replace(/^\s+|\s+$/g, "");

    if (nodeValue && xml.childNodes.length == 1) {
        obj.text = nodeValue;
    }
    if (xml.attributes.length > 0) {
        for (var j = 0; j < xml.attributes.length; j++) {
            var attribute = xml.attributes.item(j);
            obj[attribute.nodeName] = attribute.nodeValue;
        }
    }
    if (xml.childNodes.length > 0) {
        var items = [];
        for (var i = 0; i < xml.childNodes.length; i++) {
            var node = xml.childNodes.item(i);
            var item = this.convert(node);
            if (item) {
                items.push(item);
            }
        }
        if (items.length > 0) {
            obj.items = items;
        }
    }
    return obj;
};

document.addEventListener('plusready', function () {
    var webview = plus.webview.currentWebview();
    plus.navigator.setStatusBarBackground("#f84e4e");//沉浸式状态栏
    plus.key.addEventListener('backbutton', function () {
        webview.canBack(function (e) {
            if (e.canBack) {
                webview.back();
            } else {
                //webview.close(); //hide,quit
                //plus.runtime.quit();
                mui.init();
                mui.plusReady(function () {
                    //处理逻辑：1秒内，连续两次按返回键，则退出应用；
                    var first = null;
                    plus.key.addEventListener('backbutton', function () {
                        //首次按键，提示‘再按一次退出应用’
                        if (!first) {
                            first = new Date().getTime();
                            //mui.toast('再按一次退出应用');
                            $.toptip('再按一次退出应用', 'warning');
                            setTimeout(function () {
                                first = null;
                            }, 1000);
                        } else {
                            if (new Date().getTime() - first < 1500) {
                                plus.runtime.quit();
                            }
                        }
                    }, false);
                });
            }
        })
    });
});




