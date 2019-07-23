<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
    <!--引用资源-->
    <link href="../../Content/bootstrap/CSS/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/bootstrap/CSS/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <!--动画效果-->
    <link href="../../Content/bootstrap/CSS/animate.css" rel="stylesheet" type="text/css" />
    <!--手机App-->
    <link href="../../Content/bootstrap/CSS/app.css" rel="stylesheet" type="text/css" />
    <!--[if lt IE 9]>
        <script src="../../Content/bootstrap/js/ie/html5shiv.js"></script>
        <script src="../../Content/bootstrap/js/ie/respond.min.js"></script>
        <script src="../../Content/bootstrap/js/ie/excanvas.js"></script>
    <![endif]-->
    <!--引用资源-->
    <script src="../../Content/bootstrap/JS/jquery.min.js" type="text/javascript"></script>
    <script src="../../Content/bootstrap/JS/bootstrap.js" type="text/javascript"></script>
    <!--手机App-->
    <script src="../../Content/bootstrap/JS/app.js" type="text/javascript"></script>
    <!--插件资源-->
    <script src="../../Content/bootstrap/JS/parsley.min.js" type="text/javascript" charset="gbk"></script>
    <script src="../../Content/bootstrap/JS/parsley.extend.js" type="text/javascript"
        charset="gbk"></script>
    <script src="../../Content/bootstrap/JS/bootbox.js" type="text/javascript" charset="gbk"></script>
    <script type="text/javascript" src="../../Content/Statistics/jquery.flot.min.js"></script>
    <script type="text/javascript" src="../../Content/Statistics/jquery.flot.tooltip.min.js"></script>
    <script type="text/javascript" src="../../Content/Statistics/jquery.flot.resize.js"></script>
    <script type="text/javascript" src="../../Content/Statistics/jquery.flot.orderBars.js"></script>
    <script type="text/javascript" src="../../Content/Statistics/jquery.flot.pie.min.js"></script>
    <script type="text/javascript" src="../../Content/Statistics/jquery.flot.grow.js"></script>
</head>
<body style="overflow: hidden">
    <section class="vbox animated fadeInUp">
        <section class="scrollable padder">
            <section class="panel panel-default">
                <div class="row m-l-none m-r-none bg-light lter">
                   
                    <div class="col-sm-6 col-md-3 padder-v b-r b-light lt">
                        <span class="fa-stack fa-2x pull-left m-r-sm"><i class="fa fa-circle fa-stack-2x text-warning">
                        </i><i class="fa fa-bug fa-stack-1x text-white"></i></span><a style="cursor: default"
                            class="clear" href="#"><span class="h3 block m-t-xs"><strong id="bugs"><%=ViewData["TakingCashCount"]%></strong>
                            </span><small class="text-muted text-uc">交易金额</small> </a>
                    </div>
                    <div class="col-sm-6 col-md-3 padder-v b-r b-light">
                        <span class="fa-stack fa-2x pull-left m-r-sm"><i class="fa fa-circle fa-stack-2x text-danger">
                        </i><i class="fa fa-fire-extinguisher fa-stack-1x text-white"></i></span><a style="cursor: default"
                            class="clear" href="#"><span class="h3 block m-t-xs"><strong id="firers"><%=ViewData["RunCount"]%></strong>
                            </span><small class="text-muted text-uc">成交金额</small> </a>
                    </div>
                    <div class="col-sm-6 col-md-3 padder-v b-r b-light lt">
                        <span class="fa-stack fa-2x pull-left m-r-sm"><i class="fa fa-circle fa-stack-2x icon-muted">
                        </i><i class="fa fa-clock-o fa-stack-1x text-white"></i></span><a class="clear" href="#">
                            <span class="h3 block m-t-xs"><strong style="cursor: default"><%=ViewData["CarCount"]%></strong>
                            </span><small style="cursor: default" class="text-muted text-uc">订单数量</small>
                        </a>
                    </div>
                     <div class="col-sm-6 col-md-3 padder-v b-r b-light">
                        <span class="fa-stack fa-2x pull-left m-r-sm"><i class="fa fa-circle fa-stack-2x text-info">
                        </i><i class="fa fa-male fa-stack-1x text-white"></i></span><a style="cursor: default"
                            class="clear" href="#"><span class="h3 block m-t-xs"><strong><%=ViewData["UserCount"]%></strong>
                            </span><small class="text-muted text-uc">工程师数量</small> </a>
                    </div>
                </div>
            </section>
           <%-- <div class="row">
                <div class="col-md-12">
                    <section class="panel panel-default">
                        <footer class="panel-footer bg-white no-padder">
                            <div class="row text-center no-gutter">
                                <div class="col-xs-3 b-r b-light">
                                    <span class="h4 font-bold m-t block"><%=ViewData["SumAmount"]%></span> <small class="text-muted m-b block">
                                        订单总金额</small>
                                </div>
                                <div class="col-xs-3 b-r b-light">
                                    <span class="h4 font-bold m-t block"><%=ViewData["TakingSumAmount"]%></span> <small class="text-muted m-b block">
                                        取现总金额</small>
                                </div>
                                <div class="col-xs-3 b-r b-light">
                                    <span class="h4 font-bold m-t block"><%=ViewData["OrderCount"]%></span> <small class="text-muted m-b block">
                                        订单总数量</small>
                                </div>
                                <div class="col-xs-3">
                                    <span class="h4 font-bold m-t block"><%=ViewData["OrderCancelCount"]%></span> <small class="text-muted m-b block">
                                        取消订单数量</small>
                                </div>
                            </div>
                        </footer>
                    </section>
                </div>
            </div>--%>
        </section>
</section>
</body>
</html>
