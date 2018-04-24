//单位管理员
Ext.onReady(function () {
    setWidth();
    $(window).resize(function () {
        setWidth();
    });
    function setWidth() {
        var width = ($('.leftinfos').width() - 12) / 2;
        $('.infoleft').width(width - 100);
        $('.inforight').width(width + 100);
    }
    //设备月份巡检统计
    var ChartColumn = function () {
        //统计每月订单数量
        function generateData() {
            var Jsondata = [];
            //请求图表统计
            var respon = Ext.lib.Ajax.getConnectionObject().conn;
            respon.open("post", "/SysAppointed/ChartOrder", false); //获取页面按钮
            respon.send(null);
            var result = Ext.util.JSON.decode(respon.responseText);
            if (result.success) {
                for (var i = 0; i < 12; ++i) {
                    Jsondata.push(result.data[i + 1]);
                }
            }
            return Jsondata;
        }
        // 基于准备好的dom，初始化echarts实例
        var myChart = echarts.init(document.getElementById('maintj'));
        // 指定图表的配置项和数据
        var option = {
            title: {
                text: new Date().getFullYear() + '年',
                subtext: '单位设备月份巡检统计(月份)'
            },
            tooltip: {
                trigger: 'axis'
            },
            toolbox: {
                show: true,
                feature: {
                    mark: { show: true },
                    dataView: { show: true, readOnly: false },
                    magicType: { show: true, type: ['line', 'bar'] },
                    restore: { show: true },
                    saveAsImage: { show: true }
                }
            },
            legend: {
                data: ['巡检数']
            },
            calculable: true,
            xAxis: {
                type: 'category',
                data: ["1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月"]
            },
            yAxis: [
            {
                type: 'value',
                axisLabel: {
                    formatter: '{value}'
                }
            }
        ],
            series: [
            {
                name: '巡检数',
                type: 'bar',
                data: generateData(),
                markPoint: {
                    data: [
                        { type: 'max', name: '最大值' },
                        { type: 'min', name: '最小值' }
                    ]
                }
            }
          ]
        };

        // 使用刚指定的配置项和数据显示图表。
        myChart.setOption(option);

        myChart.on('click', function (param) {
            if (param.name == "最大值" || param.name == "最小值") {
                return;
            }
            var value = param.value;
            if (value > 0) {
                var month = param.name.replace("月", ""); //月份
                OpenInfo(month,2);
            }
            else {
                top.Ext.Msg.show({ title: "信息提示", msg: "本月没有巡检记录", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
            }
        });
    };
    ChartColumn();
});
