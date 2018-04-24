Ext.onReady(function () {
    //转义列
    var Role = Ext.data.Record.create([
              { name: "Id", type: "string", mapping: "Id" },
              { name: "Name", type: "string", mapping: "Name" },
              { name: "PayName", type: "string", mapping: "PayName" },
              { name: "TypeName", type: "string", mapping: "TypeName" },
              { name: "PayType", type: "string", mapping: "PayType" },
              { name: "Seller", type: "string", mapping: "Seller" },
              { name: "PayType", type: "string", mapping: "PayType" },
              { name: "Key", type: "string", mapping: "Key" },
              { name: "RoleSort", type: "int", mapping: "RoleSort" },
			  { name: "AddTime", type: "datetime", mapping: "AddTime" }
              
            ]);
    //数据源
    var store = GridStore(Role, '/SysCompanyPaySet/SearchData');

    //快捷查询,如果不需要,可以不用定义
    searchData = [['全查询', ''], ['方式名称', 'TypeName']];

    var grid = new Ext.grid.GridPanel({
        region: 'center',
        layout: 'fit',
        id: 'gg',
        store: store,
        stripeRows: true, //隔行颜色不同
        border: false,
        enableDragDrop: true, //禁用才能选择复选框
        loadMask: { msg: '数据请求中，请稍后...' },
        defult: {
            sortable: false
        },
        columns: [
                new Ext.grid.RowNumberer({ header: '', width: 25 }), //设置行号
                {
                header: "方式名称",
                dataIndex: 'TypeName',
                width: 140,
                menuDisabled: true

            },
              {
                  header: '方式类型',
                  dataIndex: 'PayName',
                  width: 160,
                  menuDisabled: true
              },
                 {
                     header: '支付宝账号',
                     dataIndex: 'Seller',
                     width: 160,
                     menuDisabled: true
                 },
                  {
                      header: '开户名',
                      dataIndex: 'Name',
                      width: 160,
                      menuDisabled: true
                  },

			 {
			     header: '添加时间',
			     dataIndex: 'AddTime',
			     flex: 3,
			     menuDisabled: true
			 }
                ],
	    tbar: tbar('SysCompanyPaySet'),
        bbar: bbar(store), //分页工具条
        viewConfig: {
            forceFit: true,
            enableRowBody: true,
            scrollOffset: 0 //不加这个的话，会在grid的最右边有个空白，留作滚动条的位置
        }
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