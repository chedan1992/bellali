Ext.onReady(function () {
    //转义列
    var Member = Ext.data.Record.create([
              { name: "Id", type: "string", mapping: "Id" },
              { name: "Name", type: "string", mapping: "Name" },
              { name: "NameTip", type: "string", mapping: "NameTip" },
              { name: "ActionName", type: "string", mapping: "ActionName" },
              { name: "IConName", type: "string", mapping: "IConName" },
              { name: "BtnType", type: "int", mapping: "BtnType" },
              { name: "CreateTime", type: "datetime", mapping: "CreateTime" }
    ]);
    //数据源
    var store = GridStore(Member, '/SysBtn/SearchData');
    var className = '';//页面类名
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
                     header: "图标",
                     dataIndex: 'IConName',
                     width: 25,
                     align: 'center',
                     menuDisabled: true,
                     renderer: function (value, meta, record, rowIdx, colIdx, store) {
                         var content = '<div><span style="vertical-align: middle;"><img style="vertical-align: middle; width:18px; height:18px;border:0" class="' + record.get("IConName") + '"  src="../../Content/Extjs/resources/images/default/s.gif"/></span></div>';
                         return content;
                     }
                 },
                {
                    header: "按钮名称",
                    dataIndex: 'Name',
                    flex: 3,
                    menuDisabled: true
                }, {
                    header: "按钮Tip",
                    dataIndex: 'NameTip',
                    flex: 3,
                    menuDisabled: true
                }, {
                    header: "方法名称",
                    dataIndex: 'ActionName',
                    flex: 2,
                    menuDisabled: true
                }, {
                    header: "图标名称",
                    dataIndex: 'IConName',
                    flex: 2,
                    menuDisabled: true
                },
                  {
                      header: '类型',
                      dataIndex: 'BtnType',
                      menuDisabled: true,
                      flex: 2,
                      renderer: function (value, meta, record, rowIdx, colIdx, store) {
                          switch (value) {
                              case 0:
                                  return "工具条";
                                  break;
                          }
                      }
                  },
                    {
                        header: '添加时间',
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

    grid.addListener('rowdblclick', dbGridClick);
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