<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id="MicrositeRt" class="MicrositeTxt">
        <div id="maintj" class="maintj" style="width: 100%; height:50%">
            <h1 id="h1Title" style="margin:0 auto; position:absolute; left:200">请点击左侧单位信息</h1>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="UrlContent" runat="server">
    <script src="<%=Url.Content("~/Common/grid.js?version=1.5") %>" type="text/javascript"></script>

  <script src="../../Content/echart/echarts.min.js" type="text/javascript"></script>
    <style type="text/css">
        .MicrositeTxt
        {
            margin-top: 20px;
            height: 100%, text-align: center;
            padding-left: 20px;
            height:100%;
            padding-right: 20px;
            color: #6a6a6a;
            font-size: 14px;
            font-family: "Helvetica Neue" , "Luxi Sans" , "DejaVu Sans" ,Tahoma, "Hiragino Sans GB" ,STHeiti, "Microsoft YaHei" ,Arial,sans-serif;
        }
    </style>
    <script type="text/javascript">
        Ext.onReady(function () {
            /*
            * ================页面布局=======================
            */
            var tree = new Ext.tree.TreePanel({
                region: 'center',
                id: 'tr',
                layout: 'fit',
                layoutConfig: {
                    animate: true
                },
                autoScroll: true,
                animate: true, //动画效果  
                rootVisible: true, //根节点是否可见  
                lines: true, //显示树形控件的前导线
                containerScroll: true,
                border: false,
                bodyStyle: 'padding-top:5px',
                loader: new Ext.tree.TreeLoader({
                    dataUrl: '/Organizational/SearchData'
                }),
                root: {
                    nodeType: 'async',
                    text: '组织架构',
                    iconCls: 'GTP_org',
                    draggable: false,
                    id: 'top'//区分是否根节点
                },
                tbar: [
                {
                    xtype: 'tbtext',
                    height: 21,
                    text: ''
                }],
                listeners: {
                    click: treeitemclick
                }
            });
            tree.expandAll();

            var viewport = new Ext.Viewport({
                layout: 'border',
                id: 'viewport',
                items: [
                        {
                            region: 'center',
                            bodyStyle: 'border-top:0px;border-bottom:0px',
                            layout: 'fit',
                            tbar: [
                            {
                                xtype: 'tbtext',
                                height: 21,
                                text: ''
                            }],
                            contentEl: 'MicrositeRt'
                        },
                        new Ext.Panel({
                            region: 'west',
                            layout: 'fit',
                            collapsible: false,
                            collapsed: false, //默认折叠
                            collapseMode: 'mini', //出现小箭头
                            split: true,
                            width: 230,
                            minSize: 175,
                            bodyStyle: 'border-left:0px;border-top:0px;border-bottom:0px',
                            deferredRender: false,
                            items: [tree]
                        })
                        ]
            });
        });

        //点击组织架构
        function treeitemclick(node, e) {
            if (node.attributes.text != "组织架构") {
                Ext.getDom("maintj").innerHTML = "";
                treeNodeId = node.attributes.id;
                //重新加载图表
                CharPart(treeNodeId, node.attributes.text);
            }
        }

        function CharPart(treeNodeId, title) {
            var Jsondata = [];
            var Title = ['已过期数量', '异常状态设备量', '半年内即将过期数量', '正常数量'];
            generateData();
            //统计每月订单数量
            function generateData() {
                //请求图表统计
                var respon = Ext.lib.Ajax.getConnectionObject().conn;
                respon.open("post", "/Chart/ChartPart?CompanyId=" + treeNodeId, false); //获取页面按钮
                respon.send(null);
                var result = Ext.util.JSON.decode(respon.responseText);
                if (result.success) {
                    for (var i = 0; i < result.data.length; ++i) {
                        Jsondata.push({ value: result.data[i].已过期数量, name: "已过期数量", itemStyle: {
                            normal: { color: 'red' }
                        }
                        });
                        Jsondata.push({ value: result.data[i].异常状态设备量, name: "异常状态设备量", itemStyle: {
                            normal: { color: 'orange' }
                        }
                        });
                        Jsondata.push({ value: result.data[i].半年内即将过期数量, name: "半年内即将过期数量", itemStyle: {
                            normal: { color: 'blue' }
                        }
                        });
                        Jsondata.push({ value: result.data[i].正常数量, name: "正常数量", itemStyle: {
                            normal: { color: 'green' }
                        }
                        });
                    }
                }
                return Jsondata;
            }
            // 基于准备好的dom，初始化echarts实例
            var myChart = echarts.init(document.getElementById('maintj'));
            // 指定图表的配置项和数据
            option = {
                title: {
                    text: title + '设备监控统计',
                    subtext: '设备统计量',
                    x: 'center'
                },
                tooltip: {
                    trigger: 'item',
                    formatter: "{a} <br/>{b} : {c}"
                },
                legend: {
                    orient: 'vertical',
                    x: 'left',
                    data: Title
                },
                toolbox: {
                    show: true,
                    feature: {
                        mark: { show: true },
                        dataView: { show: true, readOnly: false },
                        magicType: {
                            show: true,
                            type: ['pie', 'funnel'],
                            option: {
                                funnel: {
                                    x: '25%',
                                    width: '50%',
                                    funnelAlign: 'left',
                                    max: 1548
                                }
                            }
                        },
                        restore: { show: true },
                        saveAsImage: { show: true }
                    }
                },
                calculable: true,
                series: [
                    {
                        name: '过期设备',
                        type: 'pie',
                        radius: '55%',
                        center: ['50%', '60%'],
                        data: Jsondata
                    }
                ]
            };
            // 使用刚指定的配置项和数据显示图表。
            myChart.setOption(option);

            //echarts图表点击跳转
            myChart.on('click', function (param) {
                var name = param.name;
                if (name == "已过期数量") {
                    OpenInfo(1, treeNodeId, "已过期数量");
                } else if (name == "异常状态设备量") {
                    OpenInfo(2, treeNodeId, "异常状态设备量");
                } else if (name == "半年内即将过期数量") {
                    OpenInfo(3, treeNodeId, "半年内即将过期数量");
                } else if (name == "正常数量") {
                    OpenInfo(4, treeNodeId, "正常数量");
                }
            });
        }

        //弹框详细设备信息
        function OpenInfo(type,CompanyId,title) {
            var shop = new top.Ext.Window({
                width:1000,
                id: 'WinMasterList',
                height: 540,
                closable: false,
                shadow: false,
                stateful: false,
                border: false,
                modal: true,
                layout: 'fit',
                plain: true,
                autoDestroy: true,
                closeAction: 'close',
                title:title+'设备列表',
                items: {
                    autoScroll: true,
                    border: false,
                    params: { CID: CompanyId, TypeShow: type, ResponsibleId: "" },
                    autoLoad: { url: '../../Project/Html/SysAppointedGrid.htm', scripts: true, nocache: true }
                },
                buttons: [
                    {
                        text: '取消',
                        iconCls: 'GTP_cancel',
                        tooltip: '取消当前的操作',
                        handler: function () {
                            top.Ext.getCmp("WinMasterList").close();
                        }
                    }]
            }).show();
        }
    </script>
</asp:Content>
