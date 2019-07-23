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
              { name: "GroupId", type: "int", mapping: "GroupId" },
              { name: "CreateTime", type: "datetime", mapping: "CreateTime" }
            ]);
    //数据源
    var store = GridStore(EDynamic, '/EDynamic/SearchData', className);
    
    //快捷查询,如果不需要,可以不用定义
    searchData = [['全查询', ''], ['文章标题', 'Name'],['简介', 'Mark']];

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
                    width:40,
                    align: 'center',
                    menuDisabled: true,
                    renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                        return "<a herf='#' onclick='LookInfo(\"" + value + "\")'><span style='color:blue;cursor:pointer'>查看</span></a>"
                    }
                },
                {
                header: "文章标题",
                dataIndex: 'Name',
                width: 180,
                menuDisabled: true,
                renderer: function (data, metadata, record, rowIndex, columnIndex, store) {
                    if (data) {
                        metadata.attr = 'ext:qtip="' + data + '"';
                        return data;
                    }
                }
            },
                {
                    header: '简介',
                    dataIndex: 'Mark',
                    width:130,
                    menuDisabled: true
                },
                 {
                     header: '所属类别',
                     dataIndex: 'GroupId',
                     width:60,
                     menuDisabled: true,
                     renderer: formartGroupId
                 },

                  {
                      header: '状态',
                      dataIndex: 'Status',
                      align: 'center',
                      width:60,
                      menuDisabled: true,
                      renderer: formartEnableOrDisable
                  },
               {
                   header: '阅读量',//开始时间进行排序
                   dataIndex: 'ReadNum',
                   width:60,
                   menuDisabled: true
               },
                {
                    header: '添加时间',
                    dataIndex: 'CreateTime',
                    width: 120,
                    menuDisabled: true
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



//    var tree = new Ext.tree.TreePanel({
//        width:180,
//        layout: 'fit',
//        autoScroll: true,
//        animate: true,
//        enableDD: false,
//        rootVisible: false, //根节点是否可见  
//        lines: true, //显示树形控件的前导线
//        bodyStyle: 'padding-top:5px; font-size:12px;',
//        containerScroll: true,
//        root: {
//            text: '分级列表',
//            expanded: true,
//            children: [
//                        {
//                            text: '查看所有',
//                            id: '0',
//                            leaf: true
//                        },
//                        {
//                            text: '消防知识',
//                            id: '2',
//                            leaf: true
//                        },
//                        {
//                            text: '新闻管理',
//                            id: '3',
//                            leaf: true
//                        },
//                        {
//                            text: '法律法规',
//                            id: '4',
//                            leaf: true
//                        }
//                    ]
//        },
//        minSize: 175,
//        maxWidth: 200,
//        margins: '0 2 0 0',
//        border:false,
//        listeners: {
//            afterrender: function (tree) {
//                tree.getRootNode().childNodes[0].select(); //默认选中第一个节点
//            },
//            click: treeitemclick
//        }
//    });

    var viewport = new Ext.Viewport({
        layout: 'border',
        items: [grid],
        listeners: {
            afterrender: function () {

            }
        }
    });

});