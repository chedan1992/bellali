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
        height:75,
        contentEl: 'title',
        region: 'north',
        id: 'ShareNorth',
        //frame: true,
        border: false,
        bodyStyle:'padding-bottom:3px'
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
                    if (CompanyName.length > 6) {
                        CompanyName = CompanyName.substring(0, 5) + "..";
                    }
                    Ext.getDom("iconUser").innerText = ("您好:" + LoginUser.UserName + "(" + CompanyName + ")");
                    Ext.getDom("iconUser").innerHTML = ("您好:" + LoginUser.UserName + "(" + CompanyName + ")");
                }
            }
            //获取通知信息
            var notice = Ext.getDom("Notice");
            if (notice.value.trim() != "") {//显示通知
                Ext.getDom("noticeValue").innerText = notice.value;
                Ext.getDom("noticeValue").innerHTML = notice.value;
            }
            else {//显示时间
                Ext.getDom("noticeTxt").innerText = "";
                dispdate();
                updateTime();
            }

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

var init = function () {

    //统计每月订单数量
    function generateData() {
        var Jsondata = [];
        //请求图表统计
        var respon = Ext.lib.Ajax.getConnectionObject().conn;
        respon.open("post", "/ETask/ChartOrder", false); //获取页面按钮
        respon.send(null);
        var result = Ext.util.JSON.decode(respon.responseText);
        if (result.success) {
            for (var i = 0; i < 12; ++i) {
                Jsondata.push([Date.monthNames[i], result.data[i + 1]]);
            }
        }
        return Jsondata;
    }

    var store = new Ext.data.ArrayStore({
        fields: ['month', 'hits'],
        data: generateData()
    });

    //待分配任务
    PageSize = 5;
    //转义列
    var ETask = Ext.data.Record.create([
              { name: "Id", type: "string", mapping: "Id" },
              { name: "CEId", type: "string", mapping: "CEId" },
              { name: "SysId", type: "string", mapping: "SysId" },
              { name: "Mark", type: "string", mapping: "Mark" },
              { name: "Contacts", type: "string", mapping: "Contacts" },
              { name: "ContactsPhone", type: "string", mapping: "ContactsPhone" },
              { name: "Price", type: "string", mapping: "Price" },
              { name: "OrderType", type: "string", mapping: "OrderType" },
              { name: "Ordercode", type: "string", mapping: "Ordercode" },
              { name: "CauseReason", type: "string", mapping: "CauseReason" },
              { name: "DayType", type: "string", mapping: "DayType" },
              { name: "IsRepair", type: "bool", mapping: "IsRepair" },
              { name: "CEName", type: "string", mapping: "CEName" },
              { name: "SysName", type: "string", mapping: "SysName" },
              { name: "EngineerName", type: "string", mapping: "EngineerName" },
              { name: "NFC_Code", type: "string", mapping: "NFC_Code" },
              { name: "OrderType", type: "string", mapping: "OrderType" },
              { name: "GrabSingleTime", type: "datetime", mapping: "GrabSingleTime" },
              { name: "CreateTime", type: "datetime", mapping: "CreateTime" },
              { name: "Status", type: "string", mapping: "Status" }
            ]);
    //数据源
    var Gridstore = GridStore(ETask, '/ETask/SearchTopData?Type=1', 'ETask');
    var grid = new Ext.grid.GridPanel({
        layout: 'fit',
        store: Gridstore,
        stripeRows: true, //隔行颜色不同
        border: false,
        id: 'topGrid',
        region: 'center',
        region: 'center',
        loadMask: { msg: '数据请求中，请稍后...' },
        columns: [
            new Ext.grid.RowNumberer({ header: '', width: 25 }), //设置行号
            {
            header: "电梯名称",
            dataIndex: 'CEName',
            width: 120,
            sortable: false,
            menuDisabled: true
        }, {
            header: 'NFC编码',
            sortable: false,
            dataIndex: 'NFC_Code',
            menuDisabled: true,
            width: 100
        },
         {
             header: '类型',
             sortable: false,
             dataIndex: 'OrderType',
             menuDisabled: true,
             width: 60,
             renderer: formartOrderType
         },
        //         {
        //             header: '维修时间',
        //             sortable: false,
        //             dataIndex: 'DayType',
        //             menuDisabled: true,
        //             width: 60,
        //             renderer: formartDayType
        //         },
        {
        header: "价格",
        width: 70,
        dataIndex: 'Price',
        sortable: false,
        menuDisabled: true,
        renderer: formartPrice
    },

         {
             header: "物业联系人",
             dataIndex: 'Contacts',
             width: 80,
             sortable: false,
             menuDisabled: true
         },
            {
                header: '联系人电话',
                sortable: false,
                dataIndex: 'ContactsPhone',
                menuDisabled: true,
                width: 80
            }
             ,
            {
                header: "所属公司",
                width: 100,
                dataIndex: 'SysName',
                sortable: false,
                menuDisabled: true
            },
              {
                  header: '订单时间',
                  sortable: false,
                  dataIndex: 'GrabSingleTime',
                  flex: 4,
                  hidden: true,
                  menuDisabled: true
              },
              {
                  header: '发布时间',
                  sortable: false,
                  dataIndex: 'CreateTime',
                  flex: 4,
                  menuDisabled: true
              }
        ],
    bbar: bbar(Gridstore), //分页工具条
    tbar: tabbar(),
    viewConfig: {
        forceFit: true,
        enableRowBody: true,
        scrollOffset: 0 //不加这个的话，会在grid的最右边有个空白，留作滚动条的位置
    }
});
treeNodeId = 1;
grid.store.on('beforeload', function (node) {
    grid.getStore().proxy.conn.url = '/ETask/SearchTopData?Type=' + treeNodeId;
});
function tabbar() {
    var tb = new Ext.Toolbar();
    tb.add('->');
    tb.add({
        text: '待分配',
        id: 'waitBtn',
        pressed: true, //设置按钮是否已经被按下，默认是false 
        handler: function () {
            treeNodeId = 1;
            var filestore = Ext.getCmp('topGrid').getStore();
            filestore.load({ params: { start: 0, limit: PageSize} });

            //filestore.reload();

            if (Ext.getCmp("GoingBtn").pressed == true) {
                Ext.getCmp("GoingBtn").toggle();
            }
            if (Ext.getCmp("ComplateBtn").pressed == true) {
                Ext.getCmp("ComplateBtn").toggle();
            }
            if (Ext.getCmp("waitBtn").pressed == false) {
                Ext.getCmp("waitBtn").toggle();
            }

            grid.getColumnModel().setHidden(9, false);
            grid.getColumnModel().setHidden(8, true);

        }
    });
    tb.add('-');
    tb.add({
        text: '进行中',
        id: 'GoingBtn',
        pressed: false,
        handler: function () {
            treeNodeId = 2;

            var filestore = Ext.getCmp('topGrid').getStore();
            filestore.load({ params: { start: 0, limit: PageSize} });

            if (Ext.getCmp("GoingBtn").pressed == false) {
                Ext.getCmp("GoingBtn").toggle();
            }
            if (Ext.getCmp("ComplateBtn").pressed == true) {
                Ext.getCmp("ComplateBtn").toggle();
            }
            if (Ext.getCmp("waitBtn").pressed == true) {
                Ext.getCmp("waitBtn").toggle();
            }
            //索引从1开始
            grid.getColumnModel().setHidden(8, false);
            grid.getColumnModel().setHidden(9, true);
        }
    });
    tb.add('-');
    tb.add({
        text: '已完成',
        id: 'ComplateBtn',
        pressed: false,
        handler: function () {
            treeNodeId = 3;
            var filestore = Ext.getCmp('topGrid').getStore();
            filestore.load({ params: { start: 0, limit: PageSize} });

            if (Ext.getCmp("GoingBtn").pressed == true) {
                Ext.getCmp("GoingBtn").toggle();
            }
            if (Ext.getCmp("ComplateBtn").pressed == false) {
                Ext.getCmp("ComplateBtn").toggle();
            }
            if (Ext.getCmp("waitBtn").pressed == true) {
                Ext.getCmp("waitBtn").toggle();
            }
            grid.getColumnModel().setHidden(8, false);
            grid.getColumnModel().setHidden(9, true);
        }
    });
    return tb;
}

var data = [
                   {
                       title: '统计<small>(系统所有统计详情)</small>',
                       tools: tools,
                       //                       html: '<div class="row m-l-none m-r-none bg-light lter">'
                       //                    + '<div class="col-sm-6 col-md-3 padder-v b-r b-light">'
                       //                        + '<span class="fa-stack fa-2x pull-left m-r-sm"><i class="fa fa-circle fa-stack-2x text-info">'
                       //                        + '</i><i class="fa fa-male fa-stack-1x text-white"></i></span><a style="cursor: default"'
                       //                           + ' class="clear" href="#"><span class="h3 block m-t-xs"><strong>0</strong>'
                       //                           + ' </span><small class="text-muted text-uc">工程师数量</small> </a>'
                       //                   + ' </div>'
                       //                    + '<div class="col-sm-6 col-md-3 padder-v b-r b-light lt">'
                       //                        + '<span class="fa-stack fa-2x pull-left m-r-sm"><i class="fa fa-circle fa-stack-2x text-warning">'
                       //                       + ' </i><i class="fa fa-bug fa-stack-1x text-white"></i></span><a style="cursor: default"'
                       //                            + 'class="clear" href="#"><span class="h3 block m-t-xs"><strong id="bugs">0</strong>'
                       //                          + '  </span><small class="text-muted text-uc">交易金额</small> </a>'
                       //                   + ' </div>'
                       //                    + '<div class="col-sm-6 col-md-3 padder-v b-r b-light">'
                       //                        + '<span class="fa-stack fa-2x pull-left m-r-sm"><i class="fa fa-circle fa-stack-2x text-danger">'
                       //                       + ' </i><i class="fa fa-fire-extinguisher fa-stack-1x text-white"></i></span><a style="cursor: default"'
                       //                          + '  class="clear" href="#"><span class="h3 block m-t-xs"><strong id="firers">0</strong>'
                       //                          + '  </span><small class="text-muted text-uc">生成订单</small> </a>'
                       //                    + '</div>'
                       //                   + ' <div class="col-sm-6 col-md-3 padder-v b-r b-light lt">'
                       //                       + ' <span class="fa-stack fa-2x pull-left m-r-sm"><i class="fa fa-circle fa-stack-2x icon-muted">'
                       //                       + ' </i><i class="fa fa-clock-o fa-stack-1x text-white"></i></span><a class="clear" href="#">'
                       //                           + ' <span class="h3 block m-t-xs"><strong style="cursor: default">0</strong>'
                       //                           + ' </span><small style="cursor: default" class="text-muted text-uc">打车订单数量</small>'
                       //                        + '</a>'
                       //                   + ' </div>'
                       //              + '  </div>',
                       html: '<iframe id="frame_content" width="100%" height="100%" scrolling="no" name="" frameborder="0" src="/Home/Content"></iframe>',
                       anchor: '100%',
                       height: 110,
                       frame: true,
                       draggable: true,
                       cls: 'x-portlet'
                   },
                   {
                       anchor: '100%',
                       frame: true,
                       collapsible: true,
                       draggable: true,
                       tools: tools,
                       height: 240,
                       cls: 'x-portlet',
                       title: new Date().getFullYear() + '年成交订单量统计(月份)',
                       items: {
                           xtype: 'columnchart',
                           store: store,
                           yField: 'hits',
                           url: '../../Content/Extjs/resources/charts.swf',
                           xField: 'month',
                           xAxis: new Ext.chart.CategoryAxis({
                               title: '月份'
                           }),
                           yAxis: new Ext.chart.NumericAxis({
                               title: '数量'
                           }),
                           extraStyle: {
                               xAxis: {
                                   labelRotation: 0
                               }
                           }
                       }
                   },
                   {
                       title: '任务列表',
                       anchor: '100%',
                       frame: true,
                       draggable: true,
                       tools: tools,
                       height: 240,
                       layout: 'border',
                       cls: 'x-portlet',
                       items: [
                             grid
                       ]
                   }
         ];

var initData = Ext.decode(getCookie("layoutInfo"));

var portal = Ext.getCmp("myportal");
var items = portal.items;
if (!initData) {
    for (var i = 0; i < items.getCount(); i++) {
        var c = items.get(i);
        c.add(data.slice(i * 3, (i + 1) * 3));
        //c.add(data[i]);
    }

} else {
    Ext.each(initData, function (item, index) {
        portal.items.get(item.col).add(getDataById(item.id));
    }, this);
}
portal.doLayout();
};



//初始化
//init();


});