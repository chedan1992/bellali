<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id="title" class="x-hide-display" style="width: 100%; height: 300px">
    </div>
    <div id="msg_info" class="msg_info x-hide-display">
        (试题策略绿色部分双击可编辑)
    </div>
    <script type="text/javascript" language="javascript">
        var map = new BMap.Map("title");   // 创建地图实例

        map.enableScrollWheelZoom();    //启用滚轮放大缩小，默认禁用
        map.enableContinuousZoom();     //启用地图惯性拖拽，默认禁用
        //map.addControl(new BMap.NavigationControl());  //添加默认缩放平移控件
        map.addControl(new BMap.OverviewMapControl()); //添加默认缩略地图控件
        //map.addControl(new BMap.OverviewMapControl({ isOpen: true, anchor: BMAP_ANCHOR_BOTTOM_RIGHT }));   //右下角，打开
        //搜索
        var local = new BMap.LocalSearch(map);
        local.enableAutoViewport(); //允许自动调节窗体大小

        var icon = new BMap.Icon('../../Resource/img/MapMark.png', new BMap.Size(20, 32),
            { anchor: new BMap.Size(10, 30), infoWindowAnchor: new BMap.Size(10, 0) }
        );

        // 百度地图API功能
        var point;
        point = new BMap.Point(104.067923, 30.679943); //初始化地址:成都
        map.centerAndZoom(point, 12);

        var myCity = new BMap.LocalCity();
        myCity.get(myFun);
        function myFun(result) {
            map.clearOverlays(); //清空原来的标注
            var cityName = result.name;
            map.setCenter(cityName);   //关于setCenter()可参考API文档---”传送门“
            point = new BMap.Point(result.center.lng, result.center.lat);
            marker = new BMap.Marker(point, { icon: icon });
            marker.enableDragging();    //可拖拽 
            map.enableScrollWheelZoom(); //滑动改变大小
            map.addOverlay(marker);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="UrlContent" runat="server">
    <!--文件区域-->
    <script src="../../Common/grid.js?version=1.5" type="text/javascript"></script>
    <script src="../../Project/DeptSet/DeptSetLayout.js?version=1.5" type="text/javascript"></script>
    <script src="../../Project/DeptSet/DeptSet.js?version=1.5" type="text/javascript"></script>
    <%--引入地图--%>
    <script type="text/javascript" src="http://api.map.baidu.com/api?v=2.0&ak=DuKQ1S1pogxGH02bFGVM1nPl"></script>
    <style type="text/css">
        /*隐藏百度地图logo*/
        .anchorBL
        {
            display: none;
        }
        .msg_info
        {
            font-size:11px;
            text-align: left;
            padding:3px 8px 3px 32px;
            border: 1px dashed #f6981e;
            background-color: #ffc;
            background-position: 10px 8px;
            background-repeat: no-repeat;
            -moz-border-radius: 3px;
            -webkit-border-radius: 3px;
            border-radius: 3px;
            background-image:url(../../Resource/css/icons/info.gif); background-position:10px 2px; 
        }
    </style>
</asp:Content>
