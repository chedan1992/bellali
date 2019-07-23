Ext.onReady(function () {

    Ext.QuickTips.init();
    /*
    * ================页面布局=======================
    */
    //转义列
    var Member = Ext.data.Record.create([
              { name: "Id", type: "string", mapping: "Id" },
              { name: "OrderCode", type: "string", mapping: "OrderCode" },
              { name: "Nickname", type: "string", mapping: "Nickname" },
              { name: "Address", type: "string", mapping: "Address" },
              { name: "CreateTime", type: "string", mapping: "CreateTime" },
              { name: "Infomation", type: "string", mapping: "Infomation" },
              { name: "Amount", type: "string", mapping: "Amount" },
              { name: "Address", type: "string", mapping: "Address" },
              { name: "PushType", type: "string", mapping: "PushType" },
              { name: "FlowStatus", type: "string", mapping: "FlowStatus" },
              { name: "OrderTime", type: "datetime", mapping: "OrderTime" }
            ]);
    //数据源
    var store = GridStore(Member, '/OrderBack/SearchData');


    //快捷查询,如果不需要,可以不用定义
    searchData = [['全查询', ''], ['订单用户', 'UName'], ['订单总价', 'AccountPrice']];

    var grid1 = new Ext.grid.GridPanel({
        store: store,
        cm: new Ext.grid.ColumnModel({
            defaults: {
                width: 20,
                sortable: false
            },
            columns: [
            new Ext.grid.RowNumberer({ header: '', width: 25 }), //设置行号
                { header: "订单用户", width:80, menuDisabled: true, dataIndex: 'Nickname',
                    renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                        if (value) {
                            return value;
                        }
                        else {
                            if (record.data.Tel) {
                                return record.data.Tel;
                            }
                            else {
                                return "<span style='color:#7A7A7A'>(已删除)</span>";
                            }
                        }
                    }
                },
                { header: "订单时间", width: 130, menuDisabled: true, dataIndex: 'CreateTime', renderer: formartTreeGridTime },
                { header: "订单类型", width:80, menuDisabled: true, dataIndex: 'PushType',
                    renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {//订单类型（1、堂食，2、外卖，3、送货上门，4、到店取货，5、现场消费）
                        if (value ==0) {
                            return "跑腿订单";
                        }
                        else if (value ==1) { return "打车订单"; }

                    }
                },
                { header: "订单总价", menuDisabled: true,width:80, dataIndex: 'Amount',
                    renderer: function (value, metaData, record, rowIndex, colIndex, store) {
                        var value = Ext.util.Format.usMoney(value);
                        if (value) {
                            return value.replace('$', '￥');
                        }
                    }
                },
                 { header: "订单地址", width: 150, menuDisabled: true, dataIndex: 'Address' },
                { header: "订单需求", width: 160, menuDisabled: true, dataIndex: 'Infomation' }
            ]
        }),
        region: 'center',
        layout: 'fit',
        id: 'gg',
        border: false,
        loadMask: { msg: '数据请求中，请稍后...' },
        animCollapse: false,
        stripeRows: true, //隔行颜色不同
        tbar: tbar('MoneyBack'),
        bbar: bbar(store), //分页工具条
        viewConfig: {
            forceFit: true,
            enableRowBody: true,
            scrollOffset: 0 //不加这个的话，会在grid的最右边有个空白，留作滚动条的位置
        },
        listeners: {
            rowdblclick: function (grid, row) {//双击
                //得到选后的数据   
                var rows = grid.getSelectionModel().getSelections();
                if (rows[0]) {
                    load(rows[0].get("OrderCode"));
                }
            }
        }
    });
    //默认选中第一行
    grid.store.on("load", function () {
        grid.getSelectionModel().selectFirstRow(); //用这个就可以了，前提是你要有SelectionModel的实现
    }); 
    //viewport
    var viewport = new Ext.Viewport({
        layout: 'border',
        items: [grid1],
        listeners: {
            afterrender: function () {
            }
        }
    });
});






