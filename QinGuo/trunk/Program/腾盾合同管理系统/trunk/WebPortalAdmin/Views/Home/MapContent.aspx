<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>

<html>
<head runat="server">
   <meta name="Generator" content="EditPlus®">
  <meta name="Author" content="">
  <meta name="Keywords" content="">
  <meta name="Description" content="">
  <meta name="viewport" content="initial-scale=1.0, user-scalable=no" /> 
    <script src="../../Content/jquery/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://api.map.baidu.com/api?v=2.0&ak=59yEfHEXfi2Ofn0GTwoRVazM"></script>
    <title>地图导航</title>
</head>
<body>
  <div id="allmap" style="width:100%; height:100%">
  </div>
</body>
</html>
 <script type="text/javascript" language="javascript">
     var map = new BMap.Map("allmap");          // 创建地图实例  
     var point = new BMap.Point(116.404, 39.915);
     map.centerAndZoom(point, 15)
     // 百度地图API功能
     //map.centerAndZoom("重庆", 12);  
     map.addControl(new BMap.NavigationControl());  //添加默认缩放平移控件
     map.addControl(new BMap.NavigationControl({ anchor: BMAP_ANCHOR_TOP_RIGHT, type: BMAP_NAVIGATION_CONTROL_SMALL }));  //右上角，仅包含平移和缩放按钮
     map.addControl(new BMap.NavigationControl({ anchor: BMAP_ANCHOR_BOTTOM_LEFT, type: BMAP_NAVIGATION_CONTROL_PAN }));  //左下角，仅包含平移按钮
     map.addControl(new BMap.NavigationControl({ anchor: BMAP_ANCHOR_BOTTOM_RIGHT, type: BMAP_NAVIGATION_CONTROL_ZOOM }));  //右下角，仅包含缩放按钮

     var marker = new BMap.Marker(point);  // 创建标注


     marker.addEventListener("click", function () { this.openInfoWindow(infoWindow); });
     SetMark(point, '');

     var myCity = new BMap.LocalCity();
     myCity.get(myFun);
     function myFun(result) {
         var cityName = result.name;
         map.setCenter(cityName);   //关于setCenter()可参考API文档---”传送门“
         alert(cityName);
     }


     map.addOverlay(marker);              // 将标注添加到地图中
     //marker.enableDragging();    //可拖拽 
     map.enableScrollWheelZoom(); //滑动改变大小

     //点击事件
     map.addEventListener("click", function (e) {
         map.removeOverlay(marker);
         point = new BMap.Point(e.point.lng, e.point.lat);
         marker = new BMap.Marker(point);
         map.addOverlay(marker);
         SetMark(point, '');
     });


     //创建自定义窗体
     function SetMark(point, address) {
         infoWindow = new BMap.InfoWindow("<p style='font-size:13px;'>选择坐标:</p><br/><p style='font-size:11px;color:orange'>点击地图其他位置，可更换位置.</p>");
         map.openInfoWindow(infoWindow, point); //开启信息窗口
     }
  </script>