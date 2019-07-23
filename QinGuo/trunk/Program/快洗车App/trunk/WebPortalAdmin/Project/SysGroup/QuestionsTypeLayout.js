var className = 'QuestionsType'; //页面类名
Ext.onReady(function () {
    //转义列
    var Member = Ext.data.Record.create([
              { name: "Id", type: "string", mapping: "Id" },
              { name: "Name", type: "string", mapping: "Name" },
              { name: "Code", type: "string", mapping: "Code" },
              { name: "OrderNum", type: "string", mapping: "OrderNum" },
              { name: "Status", type: "int", mapping: "Status" },
              { name: "CreateTime", type: "datetime", mapping: "CreateTime" }
    ]);
    //数据源
    var store = GridStore(Member, '/SysGroup/SearchDataQuestions');
   
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
                header: "维保项目",
                dataIndex: 'Name',
                flex: 3,
                menuDisabled: true
            },
              {
                  header: "有效期(天)",
                  dataIndex: 'Code',
                  flex: 3,
                  menuDisabled: true
              },
               {
                   header: "排序",
                   dataIndex: 'OrderNum',
                   flex: 3,
                   menuDisabled: true
               },
                {
                    header: "状态",
                    dataIndex: 'Status',
                    align: 'center',
                    width: 70,
                    renderer: formartEnableOrDisable,
                    sortable: true,
                    menuDisabled: true
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