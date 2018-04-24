Ext.onReady(function () {

    //数据源
    var className = ''; //页面类名
    if (this.frameElement) {
        className = this.frameElement.name
    }
    //转义列
    var EDynamic = Ext.data.Record.create([
              { name: "Id", type: "string", mapping: "Id" },
              { name: "Name", type: "string", mapping: "Name" },
              { name: "Mark", type: "string", mapping: "Mark" },
              { name: "ReadNum", type: "int", mapping: "ReadNum" },
              { name: "Status", type: "int", mapping: "Status" },
              { name: "Sort", type: "int", mapping: "Sort" },
              { name: "GroupId", type: "string", mapping: "GroupId" },
              { name: "IsTop", type: "bit", mapping: "IsTop" },
              { name: "GroupName", type: "string", mapping: "GroupName" },
              { name: "CreateTime", type: "datetime", mapping: "CreateTime" },
              { name: "ActiveStartTime", type: "datetime", mapping: "ActiveStartTime" },
              { name: "ActiveEndTime", type: "datetime", mapping: "ActiveEndTime" }
    ]);
    //数据源
    var store = GridStore(EDynamic, '/EDynamic/SearchData', className);

    //快捷查询,如果不需要,可以不用定义
    searchData = [['全查询', ''], ['文章标题', 'Name'], ['简介', 'Mark']];

    var grid = new Ext.grid.GridPanel({
        region: 'center',
        layout: 'fit',
        id: 'gg',
        store: store,
        stripeRows: true, //隔行颜色不同
        border: false,
        enableDragDrop: true, //禁用才能选择复选框
        loadMask: { msg: '数据请求中，请稍后...' },
        columns: [
                new Ext.grid.RowNumberer({ header: '', width: 25 }), //设置行号
                {
                    header: '操作',
                    dataIndex: 'Id',
                    width: 20,
                    align: 'center',
                    menuDisabled: true,
                    renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                        return "<a herf='#' onclick='LookInfo(\"" + value + "\")'><span style='color:blue;cursor:pointer'>查看</span></a>"
                    }
                },
                {
                    header: "新闻标题",
                    dataIndex: 'Name',
                    width: 150,
                    menuDisabled: true,
                    sortable: true,
                    renderer: function (data, metadata, record, rowIndex, columnIndex, store) {
                        if (data) {
                            metadata.attr = 'ext:qtip="' + data + '"';
                            return data;
                        }
                    }
                },
             {
                 header: '有效时间', //开始时间进行排序
                 dataIndex: 'ActiveStartTime',
                 width: 150,
                 menuDisabled: true,
                 sortable: true,
                 renderer: renderTime
             },
                 {
                     header: '所属类别',
                     dataIndex: 'GroupName',
                     width: 40,
                     sortable: true,
                     menuDisabled: true
                 },
               {
                   header: '阅读量',
                   dataIndex: 'ReadNum',
                   width: 40,
                   sortable: true,
                   menuDisabled: true
               },
                {
                    header: '状态',
                    dataIndex: 'Status',
                    width: 30,
                    sortable: true,
                    menuDisabled: true,
                    renderer: formartEnableOrDisable
                },
               {
                   header: '是否置顶', 
                   dataIndex: 'IsTop',
                   width: 30,
                   sortable: true,
                   menuDisabled: true,
                   renderer: formartIsTop
               },
               {
                   header: '排序', //开始时间进行排序
                   dataIndex: 'Sort',
                   width: 30,
                   sortable: true,
                   menuDisabled: true
                },
                {
                    header: '添加时间',
                    dataIndex: 'CreateTime',
                    width: 50,
                    sortable: true,
                    menuDisabled: true
                },
               {
                   header: '排序操作', //开始时间进行排序
                   dataIndex: 'Id',
                   sortable: true,
                   menuDisabled: true,
                   renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                       if (record.data.IsTop) {
                           return '<button type="button" id="" class="x-btn-text mybtn" onclick="sort(\'' + value + '\',-1)">上移</button>&nbsp;&nbsp;&nbsp;<button type="button" id="" class="x-btn-text mybtn mybtn2"  onclick="sort(\'' + value + '\',1)">下移</button>&nbsp;&nbsp;&nbsp;<button type="button" id="" class="x-btn-text mybtn mybtn4"  onclick="sort(\'' + value + '\',2)">取消置顶</button>'
                       } else {
                           return '<button type="button" id="" class="x-btn-text mybtn" onclick="sort(\'' + value + '\',-1)">上移</button>&nbsp;&nbsp;&nbsp;<button type="button" id="" class="x-btn-text mybtn mybtn2"  onclick="sort(\'' + value + '\',1)">下移</button>&nbsp;&nbsp;&nbsp;<button type="button" id="" class="x-btn-text mybtn mybtn3"  onclick="sort(\'' + value + '\',2)">置顶</button>'
                       }
                   }
               }

        ],
        tbar: tbar(className),
        bbar: bbar(store), //分页工具条
        viewConfig: {
            forceFit: true,
            enableRowBody: true,
            scrollOffset: 0 //不加这个的话，会在grid的最右边有个空白，留作滚动条的位置
        }
    });

    // 响应加载前事件，传递node参数 
    grid.store.on('beforeload', function (node) {
        grid.getStore().proxy.conn.url = '/EDynamic/SearchData?GroupId=' + treeNodeId;
    });
    //默认选中第一行
    grid.store.on("load", function () {
        grid.getSelectionModel().selectFirstRow(); //用这个就可以了，前提是你要有SelectionModel的实现
    });



    var tree = new Ext.tree.TreePanel({
        width: 180,
        layout: 'fit',
        id: 'tree',
        autoScroll: true,
        animate: true,
        enableDD: false,
        rootVisible: false, //根节点是否可见  
        lines: true, //显示树形控件的前导线
        bodyStyle: 'padding-top:5px; font-size:12px;',
        containerScroll: true,
        loader: new Ext.tree.TreeLoader({
            dataUrl: '/EDynamic/SearchProvinceByTree?Type=0'
        }),
        containerScroll: true,
        root: {
            text: '分级列表',
            expanded: true,
            draggable: false,
            expanded: true
        },
        minSize: 175,
        maxWidth: 200,
        margins: '0 2 0 0',
        border: false,
        listeners: {
            afterrender: function (tree) {
                // tree.getRootNode().childNodes[0].select(); //默认选中第一个节点
            },
            click: treeitemclick
        }
    });

    var viewport = new Ext.Viewport({
        layout: 'border',
        items: [
            new Ext.Panel({
            region: 'west',
            collapsible: true,
            title: '新闻类别',
            collapsed: false, //默认折叠
            collapseMode: 'mini', //出现小箭头
            split: true,
            width: 200,
            minSize: 175,
            bodyStyle: 'border-left:0px;border-top:0px;border-bottom:0px',
            deferredRender: false,
            flex: 1,
            items: [tree]
        }), {
            region: 'center',
            layout: 'fit',
            id: 'eastOrganization',
            bodyStyle: 'border-top:0px;border-bottom:0px',
            flex: 1,
            items: [grid]
        }, {
            xtype: 'panel',
            region: 'east',
            flex: 1,
            title: '模板预览',
            width: 400,
            minSize: 320,
            split: true,
            collapsible: true,
            collapsed: false, //默认折叠
            collapseMode: 'mini', //出现小箭头
            deferredRender: false,
            html: "<iframe id='mbid' src='http://localhost:28130/Webcode/view/sort.html?dircid=' style='width:100%; height:100%;border:0px;'></iframe>"
        }]
    });
});