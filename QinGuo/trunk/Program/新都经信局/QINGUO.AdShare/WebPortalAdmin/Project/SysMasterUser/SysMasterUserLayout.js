Ext.onReady(function () {
    //转义列
    var SysBusinessCircle = Ext.data.Record.create([
              { name: "Id", type: "string", mapping: "Id" },
              { name: "LoginName", type: "string", mapping: "LoginName" },
              { name: "UserName", type: "string", mapping: "UserName" },
              { name: "Sex", type: "int", mapping: "Sex" },
              { name: "Pwd", type: "string", mapping: "Pwd" },
              { name: "Email", type: "string", mapping: "Email" },
              { name: "Phone", type: "string", mapping: "Phone" },
              { name: "Status", type: "int", mapping: "Status" },
              { name: "CreateTime", type: "datetime", mapping: "CreateTime" },
              { name: "CreaterName", type: "string", mapping: "CreaterName" },
              { name: "RoleName", type: "string", mapping: "RoleName" }
            ]);
   
    var store = GridStore(SysBusinessCircle, '/SysMasterUser/SearchData', 'SysMaster');
    //快捷查询,如果不需要,可以不用定义
    var className = ''; //页面类名
    if (this.frameElement) {
        className = this.frameElement.name
    }
    //快捷查询,如果不需要,可以不用定义
    searchData = [['全查询', ''], ['用户名', 'UserName'], ['登录名', 'LoginName'], ['联系电话', 'Phone'], ['邮箱', 'Email']];

    var center = new Ext.grid.GridPanel({
        id: 'gg',
        region: 'center',
        layout: 'fit',
        store: store,
        stripeRows: true, //隔行颜色不同
        enableDragDrop: true, //禁用才能选择复选框
        loadMask: { msg: '数据请求中，请稍后...' },
        defult: {
            sortable: false
        },
        columns: [
                new Ext.grid.RowNumberer({ header: '', width: 25 }), //设置行号
                    {
                    header: "用户名",
                    dataIndex: 'UserName',
                    width: 140,
                    menuDisabled: true,
                    renderer: renderUserName
                }, {
                    header: "登录名",
                    dataIndex: 'LoginName',
                    width: 100,
                    menuDisabled: true
                },
      
              {
              header: "性别",
              dataIndex: 'Sex',
              width: 100,
              menuDisabled: true,
              renderer: formartSex
          }, {
              header: "联系电话",
              dataIndex: 'Phone',
              width: 100,
              menuDisabled: true
          }, {
              header: "邮箱",
              dataIndex: 'Email',
              width: 100,
              menuDisabled: true
          },
           {
               header: "状态",
               dataIndex: 'Status',
               align: 'center',
               width: 70,
               renderer: formartEnableOrDisable,
               menuDisabled: true
           },
            {
                header: '添加时间',
                dataIndex: 'CreateTime',
                flex: 4,
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

    // 响应加载前事件，传递node参数 
    center.store.on('beforeload', function (node) {
        center.getStore().proxy.conn.url = '/SysMasterUser/SearchData?TypeId=' + treeNodeId;
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


//双击分组
function GridClickwest(grid, rowindex, e) {
    var grid = Ext.getCmp("ggwest");
    var rows = grid.getSelectionModel().getSelected();
    treeNodeId = rows.data.Id;
    Ext.getCmp("gg").getStore().reload();
}