Ext.onReady(function () {

    //快捷查询,如果不需要,可以不用定义
    var className = ''; //页面类名
    if (this.frameElement) {
        className = this.frameElement.name
    }

    var store = new Ext.data.ArrayStore({
        proxy: new Ext.data.MemoryProxy(),
        fields: ['Id', 'Name', 'Img']
    });
    var respon = Ext.lib.Ajax.getConnectionObject().conn;
    respon.open("post", "/ChannelManager/SearchData?className=" + className, false);
    respon.send(null);
    var result = Ext.util.JSON.decode(respon.responseText);
    if (result.rows.length > 0) {
        var Data = [];
        for (var i = 0; i < result.rows.length; i++) {
            var o = [result.rows[i].Id, result.rows[i].Name, result.rows[i].Img];
            Data.push(o);
        }
        store.loadData(Data);
    }
      

    //快捷查询,如果不需要,可以不用定义
    var dataview = new Ext.DataView({
        store: store,
        emptyText: '<div style="text-align:center; padding:20px">暂无数据</div>',
        tpl: new Ext.XTemplate(
                '<ul>',
                    '<tpl for=".">',
        // '<li class="phone" id="{Id}" onclick="loadGrid(this)">',
                        '<li class="phone" id="{Id}">',
                        '<img width="110" height="80" src="{Img}"/>',
                        '<strong>{Name}</strong>',
        //                        '<span>{LinkUrl}</span>',
                        '</li>',
                    '</tpl>',
                '</ul>'
            ),
        plugins: [
                new Ext.ux.DataViewTransition({
                    duration: 550,
                    idProperty: 'Id'
                })
            ],
        id: 'phones',
        itemSelector: 'li.phone', //undefined错误,必须配置  
        overClass: 'phone-hover',
        singleSelect: true,
        multiSelect: false,
        autoScroll: true
    });

    var panel = new Ext.Panel({
        layout: 'fit',
        items: dataview,
        // border: false,
        bodyStyle: 'border-top:0px;border-bottom:0px;border-left:0px;',
        id: 'viewpanel',
        tbar: tbar(className),
        region: 'center'
    });

    PageSize = 7;


    /*------------------------------------------*/

    //转义列
    var Member = Ext.data.Record.create([
              { name: "Id", type: "string", mapping: "Id" },
              { name: "ModelName", type: "string", mapping: "ModelName" },
              { name: "LinkId", type: "string", mapping: "LinkId" },
              { name: "Sort", type: "string", mapping: "Sort" },
              { name: "Status", type: "string", mapping: "Status" },
              { name: "Remark", type: "string", mapping: "Remark" },
              { name: "CompanyId", type: "string", mapping: "CompanyId" },
              { name: "CreateTime", type: "datetime", mapping: "CreateTime" }
    ]);
    //数据源
    var store = GridStore(Member, '/ChannelManager/SearchModelData');

   

    var grid = new Ext.grid.GridPanel({
        region: 'south',
        layout: 'fit',
        id: 'gg',
        height: 300,
        store: store,
        stripeRows: true, //隔行颜色不同
        defult: {
            sortable: false
        },
        loadMask: { msg: '数据请求中，请稍后...' },
        columns: [
                new Ext.grid.RowNumberer({ header: '', width: 25 }), //设置行号
                {
                header: "型号名称",
                dataIndex: 'ModelName',
                width: 100,
                menuDisabled: true
            },
                {
                    header: "排序",
                    dataIndex: 'Sort',
                    width: 50,
                    menuDisabled: true
                }, {
                    header: "备注",
                    dataIndex: 'Remark',
                    flex: 2,
                    menuDisabled: true,
                    renderer: IsNull
                },
                {
                    header: '添加时间',
                    dataIndex: 'CreateTime',
                    flex: 4,
                    menuDisabled: true
                }

        ],
        tbar: {
            items: [
                  {
                      text: '新增',
                      tooltip: '新增',
                      disabled: true,
                      id: 'AddGrid',
                      iconCls: 'GTP_add',
                      handler: AddGrid
                  },
                  '-',
                  {
                      text: '编辑',
                      tooltip: '编辑',
                      id: 'EditGrid',
                      disabled: true,
                      iconCls: 'GTP_edit',
                      handler: EditGrid
                  },
                   '-',
                   {
                       text: '删除',
                       tooltip: '删除',
                       id: 'DeleteGrid',
                       disabled: true,
                       iconCls: 'GTP_delete',
                       handler: DeleteGrid
                   }
                ]
        },
        bbar: bbar(store), //分页工具条
        viewConfig: {
            forceFit: true,
            enableRowBody: true,
            scrollOffset: 0 //不加这个的话，会在grid的最右边有个空白，留作滚动条的位置
        }
    });

  
    grid.store.on('beforeload', function (node) {
        grid.getStore().proxy.conn.url = '/ChannelManager/SearchModelData?Key=' + treeNodeId;
    });
    grid.store.on("load", function () {
        grid.getSelectionModel().selectFirstRow(); //用这个就可以了，前提是你要有SelectionModel的实现
    }); 
    grid.addListener('rowdblclick', dbGridClick);
    //viewport
    var viewport = new Ext.Viewport({
        layout: 'border',
        items: [panel,
//         {
//             region: 'east',
//             id: 'eastOrganization',
//             collapsible: true,
//             collapsed: true, //默认折叠
//             collapseMode: 'mini', //出现小箭头
//             split: true,
//             bodyStyle: 'border-top:0px;border-bottom:0px',
//             width: 550,
//             title: '型号管理',
//             minSize: 175,
//             border: false,
//             maxSize: 400,
//             margins: '0 5 0 0',
//             layout: 'fit',
//             items: [grid]
        //         }
],
        listeners: {
            afterrender: function () {
            }
        }
    });
});
