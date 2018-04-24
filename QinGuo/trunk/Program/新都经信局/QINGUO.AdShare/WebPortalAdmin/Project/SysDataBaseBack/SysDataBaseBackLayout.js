Ext.onReady(function () {
    //转义列
    var Member = Ext.data.Record.create([
              { name: "Id", type: "string", mapping: "Id" },
              { name: "LinkUrl", type: "string", mapping: "LinkUrl" },
              { name: "Remark", type: "string", mapping: "Remark" },
               { name: "Size", type: "string", mapping: "Size" },
              { name: "CreateTime", type: "datetime", mapping: "CreateTime" }
    ]);
    //数据源
    var store = GridStore(Member, '/SysDataBaseBack/SearchData');
    var className = ''; //页面类名
    if (this.frameElement) {
        className = this.frameElement.name
    }
    var grid = new Ext.grid.GridPanel({
        region: 'center',
        layout: 'fit',
        id: 'gg',
        store: store,
        stripeRows: true, //隔行颜色不同
        border: false,
        defult: {
            sortable: false
        },
        loadMask: { msg: '数据请求中，请稍后...' },
        columns: [
                new Ext.grid.RowNumberer({ header: '', width: 25 }), //设置行号
               {
               header: "文件名称",
               dataIndex: 'Remark',
               flex: 3,
               menuDisabled: true
           },   {
               header: "文件大小",
               dataIndex: 'Size',
              width:100,
               menuDisabled: true
           },
                    {
                        header: '备份时间',
                        dataIndex: 'CreateTime',
                        flex: 4,
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

    //默认选中第一行
    grid.store.on("load", function () {
        grid.getSelectionModel().selectFirstRow(); //用这个就可以了，前提是你要有SelectionModel的实现
    }); 
    //viewport
    var viewport = new Ext.Viewport({
        layout: 'border',
        items: [grid],
        listeners: {
            afterrender: function () {

            }
        }
    });
});