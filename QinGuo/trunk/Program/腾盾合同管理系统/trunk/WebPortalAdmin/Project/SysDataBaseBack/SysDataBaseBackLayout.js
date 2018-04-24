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
    var className = ''; //页面类名
   
    if (this.frameElement) {
        className = this.frameElement.name
    }
    var store = GridStore(Member, '/SysDataBaseBack/SearchData', className, "", "");
    var gridId = 'gg';
    var grid = new Ext.grid.GridPanel({
        region: 'center',
        layout: 'fit',
        id: gridId,
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
           }, {
               header: "文件大小",
               dataIndex: 'Size',
               width: 100,
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
        bbar: bbar(store, gridId, className), //分页工具条
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
                //获取当前用户所能看到的列信息
                Ext.Ajax.request({
                    url: '/RoleManage/GetGridColumn',
                    params: { moudel: className },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            var grid = Ext.getCmp(gridId);
                            var result = eval('(' + rs.data + ')');
                            columnHide = result.ColumnId;
                            var ColumnId = result.ColumnId.split(',');
                            for (var j = 0; j < ColumnId.length; j++) {
                                if (ColumnId[j] != "") {
                                    //获取列的索引
                                    var index = getGridIndex(ColumnId[j]);
                                    grid.getColumnModel().setHidden(index, true);
                                }
                            }
                            function getGridIndex(dataIndex) {
                                var column = grid.getColumnModel();
                                for (var i = 0; i < column.config.length; i++) {
                                    if (column.config[i].dataIndex != "") {
                                        if (column.config[i].dataIndex == dataIndex) {
                                            return i;
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
            }
        }
    });
});