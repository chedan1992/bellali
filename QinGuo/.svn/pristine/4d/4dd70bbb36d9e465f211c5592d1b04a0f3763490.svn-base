Ext.onReady(function () {
    var className = ''; //页面类名
    if (this.frameElement) {
        className = this.frameElement.name
    }
    //转义列
    var Feedback = Ext.data.Record.create([
              { name: "Id", type: "string", mapping: "Id" },
              { name: "BackInfo", type: "string", mapping: "BackInfo" },
              { name: "PhoneType", type: "string", mapping: "PhoneType" },
              { name: "UNickname", type: "string", mapping: "UNickname" },
              { name: "LoginName", type: "string", mapping: "LoginName" },
              { name: "CreateTime", type: "datetime", mapping: "CreateTime" },
              { name: "IsAccept", type: "bool", mapping: "IsAccept" }
            ]);
    //数据源
    var store = GridStore(Feedback, '/Advise/SearchData');

    //快捷查询,如果不需要,可以不用定义
    searchData = [['全查询', ''], ['意见内容', 'BackInfo'], ['手机类型', 'PhoneType'], ['联系电话', 'PhoneNum'], ['用户名称', 'UNickname']];

    var grid = new Ext.grid.GridPanel({
        region: 'center',
        layout: 'fit',
        id: 'gg',
        store: store,
        stripeRows: true, //隔行颜色不同
        border: false,
        loadMask: { msg: '数据请求中，请稍后...' },
        defult: {
            sortable: false,
            menuDisabled: true
        },
        columns: [
                new Ext.grid.RowNumberer({ header: '', width: 25 }), //设置行号
                {
                header: "意见内容",
                dataIndex: 'BackInfo',
                flex: 4
            },
            {
                header: "手机类型",
                dataIndex: 'PhoneType',
                flex: 2,
                renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                    if (value) {
                        //1是苹果，2安卓
                        if (parseInt(value) == 1) {
                            return "苹果";
                        }
                        else {
                            return "安卓";
                        }
                        return value;
                    }
                    else {
                        return "(未获取)";
                    }
                }
            }, {
                header: "联系电话",
                dataIndex: 'LoginName',
                flex: 2
            }, {
                header: "用户名称",
                dataIndex: 'UNickname',
                flex: 2
            },
            {
                header: '反馈时间',
                dataIndex: 'CreateTime',
                flex: 4
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