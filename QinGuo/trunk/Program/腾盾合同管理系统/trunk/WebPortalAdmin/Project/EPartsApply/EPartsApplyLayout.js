Ext.onReady(function () {
    //数据源
    var className = ''; //页面类名
    if (this.frameElement) {
        className = this.frameElement.name
    }
    //转义列
    var EPartsApply = Ext.data.Record.create([
              { name: "Id", type: "string", mapping: "Id" },
              { name: "Name", type: "string", mapping: "Name" },
              { name: "Spec", type: "string", mapping: "Spec" },
              { name: "Mark", type: "string", mapping: "Mark" },
              { name: "Img", type: "string", mapping: "Img" },
              { name: "Model", type: "string", mapping: "Model" },
              { name: "Num", type: "int", mapping: "Num" },
              { name: "BrandId", type: "string", mapping: "BrandId" },
              { name: "BrandName", type: "string", mapping: "BrandName" },
              { name: "EName", type: "string", mapping: "EName" },
              { name: "Status", type: "int", mapping: "Status" },
              { name: "feedback", type: "string", mapping: "feedback" },
              { name: "CreateTime", type: "datetime", mapping: "CreateTime" }
            ]);
    //数据源
    var store = GridStore(EPartsApply, '/EPartsApply/SearchData');
    
    //快捷查询,如果不需要,可以不用定义
    searchData = [['全查询', ''], ['配件名称', 'Name'], ['配件型号', 'Model'], ['配件规格', 'Spec']];

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
                header: "配件名称",
                dataIndex: 'Name',
                width: 180,
                menuDisabled: true,
                renderer: function (data, metadata, record, rowIndex, columnIndex, store) {
                    if (data) {
                        metadata.attr = 'ext:qtip="' + data + '"';
                        return data;
                    }
                }
//                renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
//                    return "<a herf='#' onclick='LookInfo()'><span style='color:blue ;cursor:pointer'>" + value + "</span></a>"
//			     }
            },
                {
                    header: '配件型号',
                    dataIndex: 'Model',
                    width:80,
                    menuDisabled: true
                },
                 {
                      header: '配件规格',
                      dataIndex: 'Spec',
                      width: 80,
                      menuDisabled: true
                  },
                 {
                    header: '所属品牌',
                    dataIndex: 'BrandName',
                    width:80,
                    menuDisabled: true
                },
                {
                    header: '数量', //开始时间进行排序
                    dataIndex: 'Num',
                    width: 60,
                    menuDisabled: true
                },
                {
                    header: '描述', //开始时间进行排序
                    dataIndex: 'Mark',
                    width:120,
                    menuDisabled: true
                },
                 {
                     header: '审批意见', //开始时间进行排序
                     dataIndex: 'feedback',
                     width: 120,
                     menuDisabled: true
                 },
                {
                    header: '状态',
                    dataIndex: 'Status',
                    align: 'center',
                    width:60,
                    menuDisabled: true,
                    renderer: formartFlowStatus
                },
                {
                    header: '申请时间',
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

    //双击
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