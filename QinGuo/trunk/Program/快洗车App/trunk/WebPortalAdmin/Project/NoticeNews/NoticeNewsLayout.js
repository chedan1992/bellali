Ext.onReady(function () {
    //转义列
    var Advertise = Ext.data.Record.create([
              { name: "Id", type: "string", mapping: "Id" },
              { name: "ActiveName", type: "string", mapping: "ActiveName" },
              { name: "ActionTypeName", type: "string", mapping: "ActionTypeName" },
              { name: "Type", type: "int", mapping: "Type" },
              { name: "IsTop", type: "bit", mapping: "IsTop" },
              { name: "IsPersonalMsg", type: "bit", mapping: "IsPersonalMsg" },
              { name: "ActionType", type: "int", mapping: "ActionType" },
                 { name: "ShowType", type: "int", mapping: "ShowType" },

              { name: "Status", type: "int", mapping: "Status" },
              { name: "CreateTime", type: "datetime", mapping: "CreateTime" },
              { name: "ActiveStartTime", type: "datetime", mapping: "ActiveStartTime" },
              { name: "ActiveEndTime", type: "datetime", mapping: "ActiveEndTime" }
            ]);
    //数据源
    var className = ''; //页面类名
    if (this.frameElement) {
        className = this.frameElement.name
    }
    //数据源
    var store = GridStore(Advertise, '/NoticeNews/SearchData', 'NoticeNews', "", "");


    //快捷查询,如果不需要,可以不用定义
    searchData = [['全查询', ''], ['公告标题', 'ActiveName'], ['状态', 'Status'], ['添加时间', 'CreateTime']];
    var gridId = 'gg';
    var grid = new Ext.grid.GridPanel({
        region: 'center',
        layout: 'fit',
        id: gridId,
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
                    width: 60,
                    align: 'center',
                    menuDisabled: true,
                    renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                        return "<a herf='#' onclick='LookInfo(\"" + value + "\")'><span style='color:blue;cursor:pointer'>查看</span></a>"
                    }
                },
                {
                header: "公告标题",
                dataIndex: 'ActiveName',
                width: 220,
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
                    header: '公告类型',
                    dataIndex: 'ActionTypeName',
                    width: 70,
                    sortable: true,
                    menuDisabled: true
                },
        //                 {
        //                     header: '所属分类',
        //                     dataIndex: 'ActionTypeName',
        //                     width: 70,
        //                     menuDisabled: true
        //                 },
                  {
                  header: '状态',
                  dataIndex: 'Status',
                  align: 'center',
                  sortable: true,
                  width: 60,
                  menuDisabled: true,
                  renderer: formartEnableOrDisable
              },
               {
                   header: '有效时间', //开始时间进行排序
                   dataIndex: 'ActiveStartTime',
                   width: 250,
                   menuDisabled: true,
                   sortable: true,
                   sortable: true,
                   renderer: renderTime
               },
                {
                    header: '添加时间',
                    dataIndex: 'CreateTime',
                    width: 120,
                    menuDisabled: true,
                    sortable: true,
                    renderer: renderCreateTime
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
                            if (rs.data != "") {
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
                    }
                });
            }
        }
    });

});