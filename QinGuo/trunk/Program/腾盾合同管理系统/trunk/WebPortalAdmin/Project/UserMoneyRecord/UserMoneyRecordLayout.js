Ext.onReady(function () {

    //转义列
    var UserMoneyRecord = Ext.data.Record.create([
              { name: "Id", type: "string", mapping: "Id" },
              { name: "MoneyNum", type: "string", mapping: "MoneyNum" },
              { name: "BankName", type: "string", mapping: "BankName" },
              { name: "Card", type: "string", mapping: "Card" },
              { name: "LoginName", type: "string", mapping: "LoginName" },
               { name: "Alipay", type: "string", mapping: "Alipay" },
              { name: "Status", type: "int", mapping: "Status" },
              { name: "CreateTime", type: "datetime", mapping: "CreateTime" },
              { name: "AgreeTim", type: "datetime", mapping: "AgreeTim" },
              { name: "UserName", type: "string", mapping: "UserName" },
              { name: "Remark", type: "string", mapping: "Remark" }
            ]);
    //数据源
    var store = GridStore(UserMoneyRecord, '/UserMoneyRecord/SearchData', 'UserMoneyRecord');
    //快捷查询,如果不需要,可以不用定义
    var className = ''; //页面类名
    if (this.frameElement) {
        className = this.frameElement.name
    }
    //快捷查询,如果不需要,可以不用定义
    searchData = [['全查询', ''], ['登录名', 'Name'], ['联系电话', 'Tel'], ['邮箱', 'Email']];

    var sm = new Ext.grid.CheckboxSelectionModel();

    var center = new Ext.grid.GridPanel({
        id: 'gg',
        region: 'center',
        layout: 'fit',
        store: store,
        sm: sm,
        stripeRows: true, //隔行颜色不同
        //enableDragDrop: true, //禁用才能选择复选框
        border: false,
        loadMask: { msg: '数据请求中，请稍后...' },
        defult: {
            sortable: false
        },
        columns: [
                new Ext.grid.RowNumberer({ header: '', width: 25 }), //设置行号
                 sm, //设置多选
                {
                header: "用户名称",
                dataIndex: 'UserName',
                width: 80,
                sortable: true,
                menuDisabled: true
            },
//            {
//                header: "用户名",
//                dataIndex: 'UserName',
//                width: 100,
//                menuDisabled: true
//            },
              {
                  header: "来源",
                  dataIndex: 'BankName',
                  width: 80,
                  sortable: true,
                  menuDisabled: true
              },  {
                  header: "金额",
                  dataIndex: 'MoneyNum',
                  width:80,
                  menuDisabled: true,
                  sortable: true,
                  renderer: function (data, metadata, record, rowIndex, columnIndex, store) {
                      return "<span style='color:red'>" + data.replace('-', '') + "</span>";
                  }
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
                header: "备注",
                dataIndex: 'Remark',
                align: 'center',
                width: 70,
                sortable: true,
                menuDisabled: true
            },
           {
               header: '操作时间',
               dataIndex: 'CreateTime',
               flex: 3,
               sortable: true,
               menuDisabled: true,
               renderer: renderCreateTime
           }
                ],
        tbar: tbar(className),
        bbar: bbar(store), //分页工具条
        viewConfig: {
            forceFit: true,
            enableRowBody: true,
            scrollOffset: 0 //不加这个的话，会在grid的最右边有个空白，留作滚动条的位置
        },
        listeners: {
            beforerender: function () {
                //Ext.getCmp("tree").getRootNode();
            }
        }
    });

    //处理checkbox的勾选事件 
    center.getSelectionModel().on('rowselect', function (sm, rowIdx, r) {
        //alert('勾选了checkbox后，获得选中行的name:' + center.store.getAt(rowIdx).get('Type1Name'));
    });
    //处理checkbox的取消勾选事件 
    center.getSelectionModel().on('rowdeselect', function (sm, rowIdx, r) {
      
    });


    //viewport
    var viewport = new Ext.Viewport({
        layout: 'border',
        items: [center],
        listeners: {
            afterrender: function () {

            }
        }
    });
});
