(function ($) {
    $.fn.extend({
        //状态提示
        tip: function (options) {
            var defaults = {
                width: '490',
                height: '30',
                status: 'statusing', //操作状态提示 statusing(操作进行中) , error(操作错误) ,right (操作正确)
                content: '这里是提示内容', //这里是提示内容
                position: 'fixed', //定位方式 fixed，absolute ,relative(relative暂未开放，用得到再加)
                ynclose: 'y', //是否开启自动关闭；
                dis_time: 1500, //需要显示时间
                top: 0,
                left: 50
            };

            var _seltip = this.selector; //当前调用的对象
            var options = $.extend({}, defaults, options);
            return this.each(function () {
                if (0 < $("#tip-box").length) return;
                var o = options, _this = this;
                d_status = o.status, d_width = o.width, d_height = o.height, d_content = o.content, d_position = o.position, d_top = o.top, d_left = o.left, d_ynclose = o.ynclose, d_dis_time = o.dis_time;
                var html = '<div id="tip-box" class="tip-box"><em id="closess-status" style="display:inline" class="closess-status" title="关闭"></em><span>' + d_content + '</span><em  style="display:inline" id="tip-status" class="tip-status"></em></div>';
                parent.$('body').append(html);

                var item = $(".tip-box");
                item.width(d_width).height(d_height).css({ display: 'none', fontFamily: '&#23435;&#20307;', fontSize: '12' + 'px', color: '#fff', borderRadius: '5px', lineHeight: d_height + 'px' });

                if (d_status == 'statusing') {//提示
                   item.css({ 'background-color': 'hsl(208, 83%, 71%)', boxShadow: '0 4px 4px rgba(0, 0, 0, 0.2)', border: '1px solid #006096' });
                    parent.$('.tip-status').width(16).height(16).css({ display: 'block', float: 'left', background: 'url(../../Resource/Img/micon.png) 0 -132px no-repeat', margin: '7' + 'px' });
                    parent.$('.closess-status').width(8).height(8).css({ display: 'block', float: 'right', background: 'url(../../../Resource/Img/mclose.png) no-repeat', margin: '11' + 'px', cursor: 'pointer' });
                } else if (d_status == 'error') {//错误
                    item.css({ backgroundColor: '#CD3237', boxShadow: '0 4px 4px rgba(0, 0, 0, 0.2)', border: '1px solid #CA3E3E' });
                    parent.$('.tip-status').width(16).height(16).css({ display: 'block', float: 'left', background: 'url(../../Resource/Img/micon.png) no-repeat', margin: '7' + 'px' });
                    parent.$('.closess-status').width(8).height(8).css({ display: 'block', float: 'right', background: 'url(../../Resource/Img/mclose.png) -8px 0 no-repeat', margin: '11' + 'px', cursor: 'pointer' });
                } else {
                    if (d_status == 'right') {//正确
                        item.css({ backgroundColor: '#43AB00', boxShadow: '0 4px 4px rgba(0, 0, 0, 0.2)', border: '1px solid #338100' });
                        parent.$('.tip-status').width(16).height(16).css({ display: 'block', float: 'left', background: 'url(../../Resource/Img/micon.png) 0 -66px no-repeat', margin: '7' + 'px' });
                        parent.$('.closess-status').width(8).height(8).css({ display: 'block', float: 'right', background: 'url(../../Resource/Img/mclose.png) -16px 0 no-repeat', margin: '11' + 'px', cursor: 'pointer' });
                    }
                }
                //出现位置
                if (d_position == 'fixed') {
                   item.css({ position: d_position, top: d_top + 'px', left: d_left + '%', marginLeft: -d_width / 2 + 'px' });
                } else if (d_position == 'absolute') {
                    item.css({ position: d_position, top: d_top + 'px', left: d_left + '%', marginLeft: -d_width / 2 + 'px' });
                } else {
                    if (d_position == 'relative') {
                        return true;
                    }
                }


                //出现方式
                item.slideDown("fast", function () {
                    d_ynclose == 'y' &&
						setTimeout(function () {
						    item.slideUp('slow', function () {
						        item.remove();
						    });
						}, d_dis_time);

                });

                //关闭方式
                $("#closess-status").die().live("click", function () {
                    $(this).parent().slideUp('slow', function () {
                       // item.remove();
                    });

                });
            });
        },

        //动态监听		
        event_s: function (options) {
            var defaults = {
                defnub: 5
            };
            var eventobj = this.selector;
            var options = $.extend({}, defaults, options);

            return this.each(function () {
                var o = options;
                e_defnub = o.defnub;
                $(eventobj).wrap('<div id="popup_event_s"></div>');
                var item = '<label>还可输入<span id="nub">' + e_defnub + '</span>字</label>';
                $(eventobj).after(item);
                $(eventobj).attr({ "oninput": "OnInput(event)", "onpropertychange": "OnPropChanged(event)" });
            });
        }
    });
})(jQuery);


//非ie下监听OnPropChanged
function OnInput(event) {// event.target 等同于this
    var testvalue = event.target.value;
    var testval = event.target.value.length;
    var test = e_defnub - parseInt(testval);
    var Nub = $(event.target).next().find("#nub");
    if (0 <= test) {
        Nub.html(test);
    } else {
        var newvalue = testvalue.toString().substring(0, e_defnub);
        $(event.target).val(newvalue);
    }
}

//ie下监听OnPropChanged
function OnPropChanged(event) { //event.srcElement等同于this
    if (event.propertyName.toLowerCase() == "value") {
        var testvalue = event.srcElement.value;
        var testval = event.srcElement.value.length;
        var test = e_defnub - parseInt(testval);
        var Nub = $(event.srcElement).next().find("#nub");
        if (0 <= test) {
            Nub.html(test);
        } else {
            var newvalue = testvalue.toString().substring(0, e_defnub);
            $(event.srcElement).val(newvalue);
        }
    }

}