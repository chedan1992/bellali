Ext.onReady(function () {
    /* ================ 菜单导航 =======================
    */
    /*树形菜单*/
    var tree = new Ext.tree.TreePanel({
        title: '树形菜单',
        width: 200,
        iconCls: 'icon-tree',
        autoScroll: true,
        layout: 'fit',
        rootVisible: false,
        animate: true,
        bodyStyle: 'padding-top:4px',
        lines: true, //显示树形控件的前导线
        containerScroll: true,
        tbar: [
                {
                    xtype: 'textfield',
                    emptyText: '查找菜单....',
                    id: 'IndexKeyField',
                    width: 90,
                    enableKeyEvents: true,
                    listeners: {
                        'keyup': function (val) {
                            if (val.getValue()) {
                                tree.loader.dataUrl = "/Home/GetTree?id=0&FunName=" + encodeURIComponent(val.getValue());
                            }
                            else {
                                tree.loader.dataUrl = "/Home/GetTree?id=0";
                            }
                            tree.root.reload();
                        }
                    }
                },
                '->',
                {
                    iconCls: 'GTP_allfold', //收缩按钮
                    tooltip: '收缩',
                    menu: new Ext.menu.Menu({
                        items: [
                                 {
                                     text: '收缩全部',
                                     tooltip: '收缩全部',
                                     handler: function () {
                                         tree.collapseAll();
                                     }
                                 },
                                '-',
                                    {
                                        text: '收缩第1级',
                                        tooltip: '收缩第1级',
                                        handler: function () {
                                            var node = tree.getRootNode().childNodes;
                                            for (var i = 0; i < node.length; i++) {
                                                node[i].collapse();
                                            }
                                        }

                                    }
                                    , {
                                        text: '收缩第2级',
                                        tooltip: '收缩第2级',
                                        handler: function () {
                                            var node = tree.getRootNode().childNodes;
                                            for (var i = 0; i < node.length; i++) {
                                                var childNode = node[i].childNodes;
                                                for (var j = 0; j < childNode.length; j++) {
                                                    childNode[j].collapse();
                                                }
                                            }
                                        }

                                    }
                                ]
                    })

                },
                '-',
                {
                    iconCls: 'GTP_allunfold', //展开按钮
                    tooltip: '展开',
                    menu: new Ext.menu.Menu({
                        items: [
                                 {
                                     text: '展开全部',
                                     tooltip: '展开全部',
                                     handler: function () {
                                         tree.expandAll();
                                     }
                                 },
                                '-',
                                        {
                                            text: '展开第1级',
                                            tooltip: '展开第1级',
                                            handler: function () {
                                                var node = tree.getRootNode().childNodes;
                                                for (var i = 0; i < node.length; i++) {
                                                    node[i].expand();
                                                }
                                            }
                                        }
                                        , {
                                            text: '展开第2级',
                                            tooltip: '展开第2级',
                                            handler: function () {
                                                var node = tree.getRootNode().childNodes;
                                                for (var i = 0; i < node.length; i++) {
                                                    node[i].expand();
                                                    var childNode = node[i].childNodes;
                                                    for (var j = 0; j < childNode.length; j++) {
                                                        childNode[j].expand();
                                                    }
                                                }
                                            }
                                        }
                                ]
                    })

                },
                    '-',
                {
                    iconCls: 'GTP_refresh',
                    tooltip: '刷新菜单',
                    handler: function () {
                        var keyfield = Ext.getCmp("IndexKeyField").getValue().trim();
                        if (keyfield != "") {
                            tree.loader.dataUrl = "/Home/GetTree?id=0&FunName=" + encodeURIComponent(keyfield);
                        }
                        else {
                            tree.loader.dataUrl = "/Home/GetTree?id=0";
                        }
                        tree.root.reload();
                    }
                }
            ],
        loader: new Ext.tree.TreeLoader({
            dataUrl: '/Home/GetTree?id=0'
        }),
        root: {
            nodeType: 'async',
            text: '菜单导航',
            draggable: false
        },
        listeners: {
            click: treeitemclick
        }
    });
    /*层叠菜单*/
    var accordionPanel = new Ext.Panel({
        title: '层叠菜单',
        autoScroll: false,
        iconCls: 'icon-accordion',
        layout: 'accordion',
        defaultType: 'treepanel',
        activeItem: 0,
        layoutConfig: { animate: true }
    });
    var north = new Ext.Panel({
        height: 75,
        contentEl: 'title',
        region: 'north',
        id: 'ShareNorth',
        //frame: true,
        border: false,
        bodyStyle: 'padding-bottom:3px'
        //        items: [
        //            {
        //                xtype: 'box', //或者xtype: 'component',
        //                style: 'margin-top:-1px;display:inline;float:left;',
        //                id: 'ThemeBaner',
        //                autoEl: {
        //                    tag: 'img',    //指定为img标签
        //                    src: '../../Content/Extjs/resources/images/default/s.gif'    //指定url路径
        //                }
        //            }
        //        ],
        // bbar: northBar(),
        //bodyStyle: 'line-height:50px;padding-left:0px;font-size:22px;color:#000000;font-family:黑体;font-weight:bolder;'
    });
    var south = new Ext.Panel({
        region: 'south',
        contentEl: 'south',
        id: 'ShareSouth',
        height: 30,
        frame: true,
        minSize: 100,
        maxSize: 200,
        margins: '0 0 0 0',
        id: 'south-panel'
    });
    var west = new Ext.Panel({
        region: 'west',
        title: '菜单导航',
        id: 'ShareWest',
        iconCls: "GTP_menu",
        layoutConfig: {
            animate: true
        },
        Collapsed: true, //是否可折叠 true:折叠 false不折叠
        collapsible: true, //是否显示折叠按钮 true:显示 false:不显示
        collapsible: true, //是否显示关闭和展开模块的按钮;
        collapseMode: 'mini', //出现小箭头
        animCollapse: true,
        split: true,
        width: 200,
        minSize: 175,
        maxSize: 400,
        margins: '0 5 0 0',
        layout: 'fit',
        bodyStyle: 'border-left:0px;border-top:0px;border-bottom:0px;',
        items: new Ext.TabPanel({//左边树tab
            border: false,
            activeTab: 0,
            tabPosition: 'bottom',
            items: [accordionPanel, tree]
        })
    });
    var center = new Ext.TabPanel({
        region: 'center',
        deferredRender: false,
        animScroll: true,              //使用动画滚动效果   
        enableTabScroll: true,         //tab标签过宽时自动显示滚动条
        id: 'ShareCenter',
        activeTab: 0,
        plugins: new Ext.ux.TabCloseMenu(),
        bodyStyle: 'border-top:0px;border-bottom:0px;',
        items: [
                {
                    xtype: 'panel',
                    id: 'myportal',
                    iconCls: 'GTP_home',
                    title: '平台首页',
                    margins: '3 3 3 3',
                    layout: 'fit',
                    autoScroll: true,
                    //html: '<iframe id="frame_content" width="100%" height="100%" scrolling="no" name="" frameborder="0" src="/Home/Content"></iframe>'
                    contentEl: 'center'
                }
        //        {
        //        xtype: 'portal',
        //        id: 'myportal',
        //        iconCls: 'GTP_home',
        //        title: '平台首页',
        //        region: 'center',
        //        margins: '0 3 5 5',
        //        autoScroll: false,
        //        items: [
        //                    {
        //                        columnWidth: 1,
        //                        style: 'padding:4px 4px 4px 4px',
        //                        items: []
        //                    },
        //                    {
        //                        columnWidth: 1,
        //                        style: 'padding:4px 4px 4px 4px',
        //                        items: []
        //                    },
        //                    {
        //                        columnWidth: 1,
        //                        style: 'padding:4px 4px 4px 4px',
        //                        items: []
        //                    },
        //                    {
        //                        columnWidth: 1,
        //                        style: 'padding:4px 4px 4px 4px',
        //                        items: []
        //                    }

        //            ]
        //    }
        ],
        initEvents: function () {
            Ext.TabPanel.superclass.initEvents.call(this);
            this.on('remove', this.onRemove, this, { target: this });
            this.mon(this.strip, 'mousedown', this.onStripMouseDown, this);
            this.mon(this.strip, 'contextmenu', this.onStripContextMenu, this);
            if (this.enableTabScroll) {
                this.mon(this.strip, 'mousewheel', this.onWheel, this);
            }
            this.mon(this.strip, 'dblclick', this.onTitleDbClick, this);
            this.mon(this.strip, 'contextmenu', this.onStripContextMenu, this);
        },
        onTitleDbClick: function (e, target, o) {
            var t = this.findTargets(e);
            if (t.item) {
                if (t.item.title != "平台首页") {
                    if (t.item.fireEvent('beforeclose', t.item) !== false) {
                        t.item.fireEvent('close', t.item);
                        this.remove(t.item);
                    }
                }
            }
        },
        listeners: {
            tabchange: function (thisTab, Activetab) {
            }
        }
    });
    var viewport = new Ext.Viewport({
        layout: 'border',
        border: false,
        id: 'indexViewport',
        items: [north, south, west, center],
        listeners: {
            afterrender: function () {
                //每隔10秒执行一次GetServices方法
                // setInterval(GetServices, 8000);
                SetThemeBaner(); //设置主题logo Baner

                //            if (Ext.getCmp("iconUser")) {
                //                if (LoginUser) {
                //                    var CompanyName = LoginUser.CompanyName;
                //                    if (CompanyName.length > 6) {
                //                        CompanyName = CompanyName.substring(0, 5) + "..";
                //                    }
                //                    Ext.getCmp("iconUser").setText("您好:" + LoginUser.UserName + "(" + CompanyName + ")");
                //                }
                //            }
                if (Ext.getDom("iconUser")) {
                    if (LoginUser) {
                        var CompanyName = LoginUser.CompanyName;
                        //                        if (CompanyName.length > 6) {
                        //                            CompanyName = CompanyName.substring(0, 5) + "..";
                        //                        }
                        Ext.getDom("iconUser").innerText = "您好:" + LoginUser.UserName;
                        Ext.getDom("iconUser").innerHTML = "您好:" + LoginUser.UserName;

                        //Ext.getDom("CompanyName").innerHTML = ("" + CompanyName + "");
                    }
                }
                //                //获取通知信息
                //                var notice = Ext.getDom("Notice");
                //                if (notice.value.trim() != "") {//显示通知
                //                    Ext.getDom("noticeValue").innerText = notice.value;
                //                    Ext.getDom("noticeValue").innerHTML = notice.value;
                //                }
                //                else {//显示时间
                //                    Ext.getDom("noticeTxt").innerText = "";
                dispdate();
                updateTime();
                //                }

                Ext.getBody().mask('努力加载中....');

                if (Ajax) {
                    Ajax({
                        url: "/Home/GetTree?id=0", //默认查询根节点
                        callback: addTree
                    });
                }
            }
        }
    });
    /*
    * ================内置方法=======================
    */
    function addTree(data) {
        Ext.getBody().unmask(); //取消遮罩
        if (data) {
            if (data.success == false) {
                if (data.msg == "操作成功") {
                    top.Ext.Msg.show({ title: "信息提示", msg: "用户信息已过期,请重新登录", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                }
                else {
                    top.Ext.Msg.show({ title: "信息提示", msg: data.msg, buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                }
                return false;
            }
        }
        for (var i = 0; i < data.length; i++) {
            if (data[i].children) {
                for (var j = 0; j < data[i].children.length; j++) {
                    data[i].children[j].expanded = true;
                }
            }
            var treepanel = new Ext.tree.TreePanel({
                title: data[i].text,
                iconCls: data[i].iconCls,
                autoScroll: false,
                lines: true, //显示树形控件的前导线
                rootVisible: false,
                bodyStyle: 'padding-top:4px;',
                border: false,
                margins: '0 0 15 0',
                viewConfig: {
                    loadingText: "努力加载中..."
                },
                loader: new Ext.tree.TreeLoader(),
                root: new Ext.tree.AsyncTreeNode({
                    expanded: true,
                    children: data[i].children
                }),
                //margins: '5',
                listeners: {
                    click: treeitemclick
                }
            });
            accordionPanel.add(treepanel);
            accordionPanel.doLayout();
        }
    }
    /*
    * ================表单 密码修改=======================
    */
    //用户详细信息
    new Ext.ToolTip({
        title: '登录信息',
        id: 'content-anchor-tip',
        target: 'iconUser',
        anchor: 'left',
        width: 220,
        autoHide: true,
        listeners: {
            'render': function () {
                if (LoginUser) {
                    var html = '<div id="content-tip"><div style="padding-top:10px;border-top:1px"><ul>';
                    html += '<li style="margin-top: 10px">公司名称:' + LoginUser.CompanyName + '</li>';
                    html += '<li style="margin-top: 10px">上次登录IP:' + LoginUser.LoginIp + '</li>';
                    html += ' <li style="margin-top: 10px">上次登录时间:' + LoginUser.LoginTime + '</li>';
                    html += '  <li style="margin-top: 10px">总计登录次数:' + LoginUser.LoginNum + '</li>';
                    html += '</ul></div></div>';
                    Ext.getCmp("content-anchor-tip").html = html;
                }
                else {
                    var html = '<div id="content-tip"><div style="padding-top:10px;border-top:1px"><ul>';
                    html += '  <li style="margin-top: 10px">提示:您的信息已过期,请重新登录</li>';
                    html += '  <li style="margin-top: 10px">&nbsp;</li>';
                    html += '</ul></div></div>';
                    Ext.getCmp("content-anchor-tip").html = html;
                }
            }
        }
    });

    /*
    * ================首页=======================
    */
    var tools = [
    //    {
    //        id: 'gear',
    //        handler: function () {
    //            Ext.Msg.alert('Message', 'The Settings tool was clicked.');
    //        }
    //    }, 
    {
    id: 'close',
    tip: '关闭',
    handler: function (e, target, panel) {
        panel.ownerCt.remove(panel, true);
    }
}];

var getCookie = function (cookieName) {
    var allcookies = document.cookie;
    var cookie_pos = allcookies.indexOf(cookieName);
    if (cookie_pos != -1) {
        cookie_pos += cookieName.length + 1;
        var cookie_end = allcookies.indexOf(";", cookie_pos);
        var rankInfo = allcookies.substring(cookie_pos, cookie_end);
        return rankInfo;
    }
    return null;
};
var delCookie = function (cookieName) {
    var exp = new Date();
    exp.setTime(exp.getTime() - 1);
    var cval = getCookie(cookieName);
    if (cval != null) document.cookie = cookieName + "=" + cval + ";expires=" + exp.toGMTString();
};
var getDataById = function (id) {
    var result = {};
    for (var i = 0; i < data.length; i++) {
        if (data[i].id == id) {
            result = data[i];
            break;
        }
    }
    return result;
};
var removeAll = function (portal) {
    var items = portal.items;
    for (var i = 0; i < items.getCount(); i++) {
        var c = items.get(i);
        c.items.each(function (portlet) {
            c.remove(portlet);
        });
    }
};

});
//var init = function () {
//    //统计每月订单数量
//    function generateData() {
//        var Jsondata = [];
//        //请求图表统计
////        var respon = Ext.lib.Ajax.getConnectionObject().conn;
////        respon.open("post", "/ETask/ChartOrder", false); //获取页面按钮
////        respon.send(null);
////        var result = Ext.util.JSON.decode(respon.responseText);
////        if (result.success) {
////            for (var i = 0; i < 12; ++i) {
////                //Jsondata.push(Date.monthNames[i]);
////                Jsondata.push(result.data[i + 1]);
////            }
////        }
//        return Jsondata;
//    }

//    // 基于准备好的dom，初始化echarts实例
//    var myChart = echarts.init(document.getElementById('maintj'));
//    //var title = new Date().getFullYear() + '年成交订单量统计(月份)';
//    //Ext.getDom("chartTitle").innerHTML = title;
//    // 指定图表的配置项和数据
//    var option = {
//        title: {
//            text: new Date().getFullYear() + '年',
//            subtext: '成交订单量统计(月份)'
//        },
//        tooltip: {
//            trigger: 'axis'
//        },
//        toolbox: {
//            show: true,
//            feature: {
//                mark: { show: true },
//                //dataView: { show: true, readOnly: false },
//                magicType: { show: true, type: ['line', 'bar'] },
//                restore: { show: true },
//                saveAsImage: { show: true }
//            }
//        },
//        legend: {
//            data: ['数量']
//        },
//        calculable: true,
//        xAxis: {
//            data: ["1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月"]
//        },
//        yAxis: [
//        {
//            type: 'value',
//            axisLabel: {
//                formatter: '{value}单'
//            }
//        }
//        ],
//        series: [{
//            name: '销量',
//            type: 'bar',
//            data: generateData(),
//            markPoint: {
//                data: [
//                    { type: 'max', name: '最大值' },
//                    { type: 'min', name: '最小值' }
//                ]
//            }
//        }]
//    };

//    // 使用刚指定的配置项和数据显示图表。
//    myChart.setOption(option);

////    //待分配任务
////    PageSize = 5;
////    //转义列
////    var ETask = Ext.data.Record.create([
////              { name: "Id", type: "string", mapping: "Id" },
////              { name: "CEId", type: "string", mapping: "CEId" },
////              { name: "SysId", type: "string", mapping: "SysId" },
////              { name: "Mark", type: "string", mapping: "Mark" },
////              { name: "Contacts", type: "string", mapping: "Contacts" },
////              { name: "ContactsPhone", type: "string", mapping: "ContactsPhone" },
////              { name: "Price", type: "string", mapping: "Price" },
////              { name: "OrderType", type: "string", mapping: "OrderType" },
////              { name: "Ordercode", type: "string", mapping: "Ordercode" },
////              { name: "CauseReason", type: "string", mapping: "CauseReason" },
////              { name: "DayType", type: "string", mapping: "DayType" },
////              { name: "IsRepair", type: "bool", mapping: "IsRepair" },
////              { name: "CEName", type: "string", mapping: "CEName" },
////              { name: "SysName", type: "string", mapping: "SysName" },
////              { name: "EngineerName", type: "string", mapping: "EngineerName" },
////              { name: "NFC_Code", type: "string", mapping: "NFC_Code" },
////              { name: "OrderType", type: "string", mapping: "OrderType" },
////              { name: "GrabSingleTime", type: "datetime", mapping: "GrabSingleTime" },
////              { name: "CreateTime", type: "datetime", mapping: "CreateTime" },
////              { name: "Status", type: "string", mapping: "Status" }
////            ]);
////    //数据源
////    var Gridstore = GridStore(ETask, '/ETask/SearchTopData?Type=1', 'ETask');
////    var grid = new Ext.grid.GridPanel({
////        layout: 'fit',
////        store: Gridstore,
////        stripeRows: true, //隔行颜色不同
////        border: false,
////        id: 'topGrid',
////        height: 220,
////        anchor: '100%',
////        renderTo: 'gridList',
////        loadMask: { msg: '数据请求中，请稍后...' },
////        columns: [
////            new Ext.grid.RowNumberer({ header: '', width: 25 }), //设置行号
////            {
////            header: "电梯名称",
////            dataIndex: 'CEName',
////            width: 120,
////            sortable: false,
////            menuDisabled: true
////        }, {
////            header: 'NFC编码',
////            sortable: false,
////            dataIndex: 'NFC_Code',
////            menuDisabled: true,
////            width: 100
////        },
////         {
////             header: '类型',
////             sortable: false,
////             dataIndex: 'OrderType',
////             menuDisabled: true,
////             width: 60,
////             renderer: formartOrderType
////         },
////        {
////            header: "价格",
////            width: 70,
////            dataIndex: 'Price',
////            sortable: false,
////            menuDisabled: true,
////            renderer: formartPrice
////        },

////         {
////             header: "物业联系人",
////             dataIndex: 'Contacts',
////             width: 80,
////             sortable: false,
////             menuDisabled: true
////         },
////            {
////                header: '联系人电话',
////                sortable: false,
////                dataIndex: 'ContactsPhone',
////                menuDisabled: true,
////                width: 80
////            }
////             ,
////            {
////                header: "所属公司",
////                width: 100,
////                dataIndex: 'SysName',
////                sortable: false,
////                menuDisabled: true
////            },
////              {
////                  header: '订单时间',
////                  sortable: false,
////                  dataIndex: 'GrabSingleTime',
////                  flex: 4,
////                  hidden: true,
////                  menuDisabled: true
////              },
////              {
////                  header: '发布时间',
////                  sortable: false,
////                  dataIndex: 'CreateTime',
////                  flex: 4,
////                  menuDisabled: true
////              }
////        ],
////        bbar: bbar(Gridstore), //分页工具条
////        tbar: tabbar(),
////        viewConfig: {
////            forceFit: true,
////            enableRowBody: true,
////            scrollOffset: 0 //不加这个的话，会在grid的最右边有个空白，留作滚动条的位置
////        }
////    });
////    treeNodeId = 1;
////    grid.store.on('beforeload', function (node) {
////        grid.getStore().proxy.conn.url = '/ETask/SearchTopData?Type=' + treeNodeId;
////    });
////    function tabbar() {
////        var tb = new Ext.Toolbar();
////        tb.add('->');
////        tb.add({
////            text: '待分配',
////            id: 'waitBtn',
////            pressed: true, //设置按钮是否已经被按下，默认是false 
////            handler: function () {
////                treeNodeId = 1;
////                var filestore = Ext.getCmp('topGrid').getStore();
////                filestore.load({ params: { start: 0, limit: PageSize} });

////                //filestore.reload();

////                if (Ext.getCmp("GoingBtn").pressed == true) {
////                    Ext.getCmp("GoingBtn").toggle();
////                }
////                if (Ext.getCmp("ComplateBtn").pressed == true) {
////                    Ext.getCmp("ComplateBtn").toggle();
////                }
////                if (Ext.getCmp("waitBtn").pressed == false) {
////                    Ext.getCmp("waitBtn").toggle();
////                }

////                grid.getColumnModel().setHidden(9, false);
////                grid.getColumnModel().setHidden(8, true);

////            }
////        });
////        tb.add('-');
////        tb.add({
////            text: '进行中',
////            id: 'GoingBtn',
////            pressed: false,
////            handler: function () {
////                treeNodeId = 2;

////                var filestore = Ext.getCmp('topGrid').getStore();
////                filestore.load({ params: { start: 0, limit: PageSize} });

////                if (Ext.getCmp("GoingBtn").pressed == false) {
////                    Ext.getCmp("GoingBtn").toggle();
////                }
////                if (Ext.getCmp("ComplateBtn").pressed == true) {
////                    Ext.getCmp("ComplateBtn").toggle();
////                }
////                if (Ext.getCmp("waitBtn").pressed == true) {
////                    Ext.getCmp("waitBtn").toggle();
////                }
////                //索引从1开始
////                grid.getColumnModel().setHidden(8, false);
////                grid.getColumnModel().setHidden(9, true);
////            }
////        });
////        tb.add('-');
////        tb.add({
////            text: '已完成',
////            id: 'ComplateBtn',
////            pressed: false,
////            handler: function () {
////                treeNodeId = 3;
////                var filestore = Ext.getCmp('topGrid').getStore();
////                filestore.load({ params: { start: 0, limit: PageSize} });

////                if (Ext.getCmp("GoingBtn").pressed == true) {
////                    Ext.getCmp("GoingBtn").toggle();
////                }
////                if (Ext.getCmp("ComplateBtn").pressed == false) {
////                    Ext.getCmp("ComplateBtn").toggle();
////                }
////                if (Ext.getCmp("waitBtn").pressed == true) {
////                    Ext.getCmp("waitBtn").toggle();
////                }
////                grid.getColumnModel().setHidden(8, false);
////                grid.getColumnModel().setHidden(9, true);
////            }
////        });
////        return tb;
////    }
//};

////根据权限动态加载
//if (LoginUser) {
//    var centerPanel = Ext.getCmp("ShareCenter");
//    if (!centerPanel) {
//        centerPanel = parent.Ext.getCmp("ShareCenter");
//    }
//    //菜单配置员
//    if (LoginUser.Attribute == 0) {
////        var tabs = centerPanel.add({
////            id: 'myportal',
////            iconCls: 'GTP_home',
////            title: '平台首页',
////            margins: '3 3 3 3',
////            layout: 'fit',
////            autoScroll: true,
////            contentEl: 'mapDiv'
////        });
////        centerPanel.setActiveTab(tabs);
////        InitMap();
//    }
//    //系统管理员
//    else if (LoginUser.Attribute == 1) {
//        var tabs = centerPanel.add({
//            id: 'myportal',
//            iconCls: 'GTP_home',
//            title: '平台首页',
//            margins: '3 3 3 3',
//            layout: 'fit',
//            autoScroll: true,
//            contentEl: 'center'
//        });
//        centerPanel.setActiveTab(tabs);
//        init();
//    }
//    else if (LoginUser.Attribute == 2) {//单位管理员
//        var tabs = centerPanel.add({
//            id: 'myportal',
//            iconCls: 'GTP_home',
//            title: '平台首页',
//            margins: '3 3 3 3',
//            layout: 'fit',
//            autoScroll: true,
//            contentEl: 'center'
//        });
//        centerPanel.setActiveTab(tabs);
//        init();
//    } 
//    Ext.getCmp("indexViewport").doLayout();
//}
//});

////渲染地图
//function InitMap() {
//    // 基于准备好的dom，初始化echarts实例
//    var myChart = echarts.init(document.getElementById('mapDiv'));
//    option = {
//        title: {
//            text: '全国主要城市空气质量（pm2.5）',
//            subtext: 'data from PM25.in',
//            x: 'center'
//        },
//        tooltip: {
//            trigger: 'item'
//        },
//        legend: {
//            orient: 'vertical',
//            x: 'left',
//            data: ['pm2.5']
//        },
//        dataRange: {
//            min: 0,
//            max: 500,
//            calculable: true,
//            color: ['maroon', 'purple', 'red', 'orange', 'yellow', 'lightgreen']
//        },
//        toolbox: {
//            show: true,
//            orient: 'vertical',
//            x: 'right',
//            y: 'center',
//            feature: {
//                mark: { show: true },
//                dataView: { show: true, readOnly: false },
//                restore: { show: true },
//                saveAsImage: { show: true }
//            }
//        },
//        series: [
//        {
//            name: 'pm2.5',
//            type: 'map',
//            mapType: 'china',
//            hoverable: false,
//            roam: true,
//            data: [],
//            markPoint: {
//                symbolSize: 5,       // 标注大小，半宽（半径）参数，当图形为方向或菱形则总宽度为symbolSize * 2
//                itemStyle: {
//                    normal: {
//                        borderColor: '#87cefa',
//                        borderWidth: 1,            // 标注边线线宽，单位px，默认为1
//                        label: {
//                            show: false
//                        }
//                    },
//                    emphasis: {
//                        borderColor: '#1e90ff',
//                        borderWidth: 5,
//                        label: {
//                            show: false
//                        }
//                    }
//                },
//                data: [
//                    { name: "海门", value: 9 },
//                    { name: "鄂尔多斯", value: 12 },
//                    { name: "招远", value: 12 },
//                    { name: "舟山", value: 12 },
//                    { name: "齐齐哈尔", value: 14 },
//                    { name: "盐城", value: 15 },
//                    { name: "赤峰", value: 16 },
//                    { name: "青岛", value: 18 },
//                    { name: "乳山", value: 18 },
//                    { name: "金昌", value: 19 },
//                    { name: "泉州", value: 21 },
//                    { name: "莱西", value: 21 },
//                    { name: "日照", value: 21 },
//                    { name: "胶南", value: 22 },
//                    { name: "南通", value: 23 },
//                    { name: "拉萨", value: 24 },
//                    { name: "云浮", value: 24 },
//                    { name: "梅州", value: 25 },
//                    { name: "文登", value: 25 },
//                    { name: "上海", value: 25 },
//                    { name: "攀枝花", value: 25 },
//                    { name: "威海", value: 25 },
//                    { name: "承德", value: 25 },
//                    { name: "厦门", value: 26 },
//                    { name: "汕尾", value: 26 },
//                    { name: "潮州", value: 26 },
//                    { name: "丹东", value: 27 },
//                    { name: "太仓", value: 27 },
//                    { name: "曲靖", value: 27 },
//                    { name: "烟台", value: 28 },
//                    { name: "福州", value: 29 },
//                    { name: "瓦房店", value: 30 },
//                    { name: "即墨", value: 30 },
//                    { name: "抚顺", value: 31 },
//                    { name: "玉溪", value: 31 },
//                    { name: "张家口", value: 31 },
//                    { name: "阳泉", value: 31 },
//                    { name: "莱州", value: 32 },
//                    { name: "湖州", value: 32 },
//                    { name: "汕头", value: 32 },
//                    { name: "昆山", value: 33 },
//                    { name: "宁波", value: 33 },
//                    { name: "湛江", value: 33 },
//                    { name: "揭阳", value: 34 },
//                    { name: "荣成", value: 34 },
//                    { name: "连云港", value: 35 },
//                    { name: "葫芦岛", value: 35 },
//                    { name: "常熟", value: 36 },
//                    { name: "东莞", value: 36 },
//                    { name: "河源", value: 36 },
//                    { name: "淮安", value: 36 },
//                    { name: "泰州", value: 36 },
//                    { name: "南宁", value: 37 },
//                    { name: "营口", value: 37 },
//                    { name: "惠州", value: 37 },
//                    { name: "江阴", value: 37 },
//                    { name: "蓬莱", value: 37 },
//                    { name: "韶关", value: 38 },
//                    { name: "嘉峪关", value: 38 },
//                    { name: "广州", value: 38 },
//                    { name: "延安", value: 38 },
//                    { name: "太原", value: 39 },
//                    { name: "清远", value: 39 },
//                    { name: "中山", value: 39 },
//                    { name: "昆明", value: 39 },
//                    { name: "寿光", value: 40 },
//                    { name: "盘锦", value: 40 },
//                    { name: "长治", value: 41 },
//                    { name: "深圳", value: 41 },
//                    { name: "珠海", value: 42 },
//                    { name: "宿迁", value: 43 },
//                    { name: "咸阳", value: 43 },
//                    { name: "铜川", value: 44 },
//                    { name: "平度", value: 44 },
//                    { name: "佛山", value: 44 },
//                    { name: "海口", value: 44 },
//                    { name: "江门", value: 45 },
//                    { name: "章丘", value: 45 },
//                    { name: "肇庆", value: 46 },
//                    { name: "大连", value: 47 },
//                    { name: "临汾", value: 47 },
//                    { name: "吴江", value: 47 },
//                    { name: "石嘴山", value: 49 },
//                    { name: "沈阳", value: 50 },
//                    { name: "苏州", value: 50 },
//                    { name: "茂名", value: 50 },
//                    { name: "嘉兴", value: 51 },
//                    { name: "长春", value: 51 },
//                    { name: "胶州", value: 52 },
//                    { name: "银川", value: 52 },
//                    { name: "张家港", value: 52 },
//                    { name: "三门峡", value: 53 },
//                    { name: "锦州", value: 54 },
//                    { name: "南昌", value: 54 },
//                    { name: "柳州", value: 54 },
//                    { name: "三亚", value: 54 },
//                    { name: "自贡", value: 56 },
//                    { name: "吉林", value: 56 },
//                    { name: "阳江", value: 57 },
//                    { name: "泸州", value: 57 },
//                    { name: "西宁", value: 57 },
//                    { name: "宜宾", value: 58 },
//                    { name: "呼和浩特", value: 58 },
//                    { name: "成都", value: 58 },
//                    { name: "大同", value: 58 },
//                    { name: "镇江", value: 59 },
//                    { name: "桂林", value: 59 },
//                    { name: "张家界", value: 59 },
//                    { name: "宜兴", value: 59 },
//                    { name: "北海", value: 60 },
//                    { name: "西安", value: 61 },
//                    { name: "金坛", value: 62 },
//                    { name: "东营", value: 62 },
//                    { name: "牡丹江", value: 63 },
//                    { name: "遵义", value: 63 },
//                    { name: "绍兴", value: 63 },
//                    { name: "扬州", value: 64 },
//                    { name: "常州", value: 64 },
//                    { name: "潍坊", value: 65 },
//                    { name: "重庆", value: 66 },
//                    { name: "台州", value: 67 },
//                    { name: "南京", value: 67 },
//                    { name: "滨州", value: 70 },
//                    { name: "贵阳", value: 71 },
//                    { name: "无锡", value: 71 },
//                    { name: "本溪", value: 71 },
//                    { name: "克拉玛依", value: 72 },
//                    { name: "渭南", value: 72 },
//                    { name: "马鞍山", value: 72 },
//                    { name: "宝鸡", value: 72 },
//                    { name: "焦作", value: 75 },
//                    { name: "句容", value: 75 },
//                    { name: "北京", value: 79 },
//                    { name: "徐州", value: 79 },
//                    { name: "衡水", value: 80 },
//                    { name: "包头", value: 80 },
//                    { name: "绵阳", value: 80 },
//                    { name: "乌鲁木齐", value: 84 },
//                    { name: "枣庄", value: 84 },
//                    { name: "杭州", value: 84 },
//                    { name: "淄博", value: 85 },
//                    { name: "鞍山", value: 86 },
//                    { name: "溧阳", value: 86 },
//                    { name: "库尔勒", value: 86 },
//                    { name: "安阳", value: 90 },
//                    { name: "开封", value: 90 },
//                    { name: "济南", value: 92 },
//                    { name: "德阳", value: 93 },
//                    { name: "温州", value: 95 },
//                    { name: "九江", value: 96 },
//                    { name: "邯郸", value: 98 },
//                    { name: "临安", value: 99 },
//                    { name: "兰州", value: 99 },
//                    { name: "沧州", value: 100 },
//                    { name: "临沂", value: 103 },
//                    { name: "南充", value: 104 },
//                    { name: "天津", value: 105 },
//                    { name: "富阳", value: 106 },
//                    { name: "泰安", value: 112 },
//                    { name: "诸暨", value: 112 },
//                    { name: "郑州", value: 113 },
//                    { name: "哈尔滨", value: 114 },
//                    { name: "聊城", value: 116 },
//                    { name: "芜湖", value: 117 },
//                    { name: "唐山", value: 119 },
//                    { name: "平顶山", value: 119 },
//                    { name: "邢台", value: 119 },
//                    { name: "德州", value: 120 },
//                    { name: "济宁", value: 120 },
//                    { name: "荆州", value: 127 },
//                    { name: "宜昌", value: 130 },
//                    { name: "义乌", value: 132 },
//                    { name: "丽水", value: 133 },
//                    { name: "洛阳", value: 134 },
//                    { name: "秦皇岛", value: 136 },
//                    { name: "株洲", value: 143 },
//                    { name: "石家庄", value: 147 },
//                    { name: "莱芜", value: 148 },
//                    { name: "常德", value: 152 },
//                    { name: "保定", value: 153 },
//                    { name: "湘潭", value: 154 },
//                    { name: "金华", value: 157 },
//                    { name: "岳阳", value: 169 },
//                    { name: "长沙", value: 175 },
//                    { name: "衢州", value: 177 },
//                    { name: "廊坊", value: 193 },
//                    { name: "菏泽", value: 194 },
//                    { name: "合肥", value: 229 },
//                    { name: "武汉", value: 273 },
//                    { name: "大庆", value: 279 }
//                ]
//            },
//            geoCoord: {
//                "海门": [121.15, 31.89],
//                "鄂尔多斯": [109.781327, 39.608266],
//                "招远": [120.38, 37.35],
//                "舟山": [122.207216, 29.985295],
//                "齐齐哈尔": [123.97, 47.33],
//                "盐城": [120.13, 33.38],
//                "赤峰": [118.87, 42.28],
//                "青岛": [120.33, 36.07],
//                "乳山": [121.52, 36.89],
//                "金昌": [102.188043, 38.520089],
//                "泉州": [118.58, 24.93],
//                "莱西": [120.53, 36.86],
//                "日照": [119.46, 35.42],
//                "胶南": [119.97, 35.88],
//                "南通": [121.05, 32.08],
//                "拉萨": [91.11, 29.97],
//                "云浮": [112.02, 22.93],
//                "梅州": [116.1, 24.55],
//                "文登": [122.05, 37.2],
//                "上海": [121.48, 31.22],
//                "攀枝花": [101.718637, 26.582347],
//                "威海": [122.1, 37.5],
//                "承德": [117.93, 40.97],
//                "厦门": [118.1, 24.46],
//                "汕尾": [115.375279, 22.786211],
//                "潮州": [116.63, 23.68],
//                "丹东": [124.37, 40.13],
//                "太仓": [121.1, 31.45],
//                "曲靖": [103.79, 25.51],
//                "烟台": [121.39, 37.52],
//                "福州": [119.3, 26.08],
//                "瓦房店": [121.979603, 39.627114],
//                "即墨": [120.45, 36.38],
//                "抚顺": [123.97, 41.97],
//                "玉溪": [102.52, 24.35],
//                "张家口": [114.87, 40.82],
//                "阳泉": [113.57, 37.85],
//                "莱州": [119.942327, 37.177017],
//                "湖州": [120.1, 30.86],
//                "汕头": [116.69, 23.39],
//                "昆山": [120.95, 31.39],
//                "宁波": [121.56, 29.86],
//                "湛江": [110.359377, 21.270708],
//                "揭阳": [116.35, 23.55],
//                "荣成": [122.41, 37.16],
//                "连云港": [119.16, 34.59],
//                "葫芦岛": [120.836932, 40.711052],
//                "常熟": [120.74, 31.64],
//                "东莞": [113.75, 23.04],
//                "河源": [114.68, 23.73],
//                "淮安": [119.15, 33.5],
//                "泰州": [119.9, 32.49],
//                "南宁": [108.33, 22.84],
//                "营口": [122.18, 40.65],
//                "惠州": [114.4, 23.09],
//                "江阴": [120.26, 31.91],
//                "蓬莱": [120.75, 37.8],
//                "韶关": [113.62, 24.84],
//                "嘉峪关": [98.289152, 39.77313],
//                "广州": [113.23, 23.16],
//                "延安": [109.47, 36.6],
//                "太原": [112.53, 37.87],
//                "清远": [113.01, 23.7],
//                "中山": [113.38, 22.52],
//                "昆明": [102.73, 25.04],
//                "寿光": [118.73, 36.86],
//                "盘锦": [122.070714, 41.119997],
//                "长治": [113.08, 36.18],
//                "深圳": [114.07, 22.62],
//                "珠海": [113.52, 22.3],
//                "宿迁": [118.3, 33.96],
//                "咸阳": [108.72, 34.36],
//                "铜川": [109.11, 35.09],
//                "平度": [119.97, 36.77],
//                "佛山": [113.11, 23.05],
//                "海口": [110.35, 20.02],
//                "江门": [113.06, 22.61],
//                "章丘": [117.53, 36.72],
//                "肇庆": [112.44, 23.05],
//                "大连": [121.62, 38.92],
//                "临汾": [111.5, 36.08],
//                "吴江": [120.63, 31.16],
//                "石嘴山": [106.39, 39.04],
//                "沈阳": [123.38, 41.8],
//                "苏州": [120.62, 31.32],
//                "茂名": [110.88, 21.68],
//                "嘉兴": [120.76, 30.77],
//                "长春": [125.35, 43.88],
//                "胶州": [120.03336, 36.264622],
//                "银川": [106.27, 38.47],
//                "张家港": [120.555821, 31.875428],
//                "三门峡": [111.19, 34.76],
//                "锦州": [121.15, 41.13],
//                "南昌": [115.89, 28.68],
//                "柳州": [109.4, 24.33],
//                "三亚": [109.511909, 18.252847],
//                "自贡": [104.778442, 29.33903],
//                "吉林": [126.57, 43.87],
//                "阳江": [111.95, 21.85],
//                "泸州": [105.39, 28.91],
//                "西宁": [101.74, 36.56],
//                "宜宾": [104.56, 29.77],
//                "呼和浩特": [111.65, 40.82],
//                "成都": [104.06, 30.67],
//                "大同": [113.3, 40.12],
//                "镇江": [119.44, 32.2],
//                "桂林": [110.28, 25.29],
//                "张家界": [110.479191, 29.117096],
//                "宜兴": [119.82, 31.36],
//                "北海": [109.12, 21.49],
//                "西安": [108.95, 34.27],
//                "金坛": [119.56, 31.74],
//                "东营": [118.49, 37.46],
//                "牡丹江": [129.58, 44.6],
//                "遵义": [106.9, 27.7],
//                "绍兴": [120.58, 30.01],
//                "扬州": [119.42, 32.39],
//                "常州": [119.95, 31.79],
//                "潍坊": [119.1, 36.62],
//                "重庆": [106.54, 29.59],
//                "台州": [121.420757, 28.656386],
//                "南京": [118.78, 32.04],
//                "滨州": [118.03, 37.36],
//                "贵阳": [106.71, 26.57],
//                "无锡": [120.29, 31.59],
//                "本溪": [123.73, 41.3],
//                "克拉玛依": [84.77, 45.59],
//                "渭南": [109.5, 34.52],
//                "马鞍山": [118.48, 31.56],
//                "宝鸡": [107.15, 34.38],
//                "焦作": [113.21, 35.24],
//                "句容": [119.16, 31.95],
//                "北京": [116.46, 39.92],
//                "徐州": [117.2, 34.26],
//                "衡水": [115.72, 37.72],
//                "包头": [110, 40.58],
//                "绵阳": [104.73, 31.48],
//                "乌鲁木齐": [87.68, 43.77],
//                "枣庄": [117.57, 34.86],
//                "杭州": [120.19, 30.26],
//                "淄博": [118.05, 36.78],
//                "鞍山": [122.85, 41.12],
//                "溧阳": [119.48, 31.43],
//                "库尔勒": [86.06, 41.68],
//                "安阳": [114.35, 36.1],
//                "开封": [114.35, 34.79],
//                "济南": [117, 36.65],
//                "德阳": [104.37, 31.13],
//                "温州": [120.65, 28.01],
//                "九江": [115.97, 29.71],
//                "邯郸": [114.47, 36.6],
//                "临安": [119.72, 30.23],
//                "兰州": [103.73, 36.03],
//                "沧州": [116.83, 38.33],
//                "临沂": [118.35, 35.05],
//                "南充": [106.110698, 30.837793],
//                "天津": [117.2, 39.13],
//                "富阳": [119.95, 30.07],
//                "泰安": [117.13, 36.18],
//                "诸暨": [120.23, 29.71],
//                "郑州": [113.65, 34.76],
//                "哈尔滨": [126.63, 45.75],
//                "聊城": [115.97, 36.45],
//                "芜湖": [118.38, 31.33],
//                "唐山": [118.02, 39.63],
//                "平顶山": [113.29, 33.75],
//                "邢台": [114.48, 37.05],
//                "德州": [116.29, 37.45],
//                "济宁": [116.59, 35.38],
//                "荆州": [112.239741, 30.335165],
//                "宜昌": [111.3, 30.7],
//                "义乌": [120.06, 29.32],
//                "丽水": [119.92, 28.45],
//                "洛阳": [112.44, 34.7],
//                "秦皇岛": [119.57, 39.95],
//                "株洲": [113.16, 27.83],
//                "石家庄": [114.48, 38.03],
//                "莱芜": [117.67, 36.19],
//                "常德": [111.69, 29.05],
//                "保定": [115.48, 38.85],
//                "湘潭": [112.91, 27.87],
//                "金华": [119.64, 29.12],
//                "岳阳": [113.09, 29.37],
//                "长沙": [113, 28.21],
//                "衢州": [118.88, 28.97],
//                "廊坊": [116.7, 39.53],
//                "菏泽": [115.480656, 35.23375],
//                "合肥": [117.27, 31.86],
//                "武汉": [114.31, 30.52],
//                "大庆": [125.03, 46.58]
//            }
//        },
//        {
//            name: 'Top5',
//            type: 'map',
//            mapType: 'china',
//            data: [],
//            markPoint: {
//                symbol: 'emptyCircle',
//                symbolSize: function (v) {
//                    return 10 + v / 100
//                },
//                effect: {
//                    show: true,
//                    shadowBlur: 0
//                },
//                itemStyle: {
//                    normal: {
//                        label: { show: false }
//                    }
//                },
//                data: [
//                    { name: "廊坊", value: 193 },
//                    { name: "菏泽", value: 194 },
//                    { name: "合肥", value: 229 },
//                    { name: "武汉", value: 273 },
//                    { name: "大庆", value: 279 }
//                ]
//            }
//        }
//    ]
//    };
//    // 使用刚指定的配置项和数据显示图表。
//    myChart.setOption(option);
//}