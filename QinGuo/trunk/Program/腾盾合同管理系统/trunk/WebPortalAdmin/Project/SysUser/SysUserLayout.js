Ext.onReady(function () {

    //转义列
    var SysBusinessCircle = Ext.data.Record.create([
              { name: "Id", type: "string", mapping: "Id" },
              { name: "Name", type: "string", mapping: "Name" },
              { name: "Nickname", type: "string", mapping: "Nickname" },
              { name: "Sex", type: "int", mapping: "Sex" },
              { name: "Pwd", type: "string", mapping: "Pwd" },
              { name: "Email", type: "string", mapping: "Email" },
              { name: "Tel", type: "string", mapping: "Tel" },
              { name: "Status", type: "int", mapping: "Status" },
              { name: "CreateTime", type: "datetime", mapping: "CreateTime" },
              { name: "CreaterName", type: "string", mapping: "CreaterName" },
              { name: "RoleName", type: "string", mapping: "RoleName" }
            ]);
    //数据源
    var store = GridStore(SysBusinessCircle, '/SysUser/SearchData', 'SysUser');
    //快捷查询,如果不需要,可以不用定义
    var className = ''; //页面类名
    if (this.frameElement) {
        className = this.frameElement.name
    }
    //快捷查询,如果不需要,可以不用定义
    searchData = [['全查询', ''], ['登录名', 'Name'], ['联系电话', 'Tel'], ['邮箱', 'Email']];

    var center = new Ext.grid.GridPanel({
        id: 'gg',
        region: 'center',
        layout: 'fit',
        store: store,
        stripeRows: true, //隔行颜色不同
        enableDragDrop: true, //禁用才能选择复选框
        border:false,
        loadMask: { msg: '数据请求中，请稍后...' },
        defult: {
            sortable: false
        },
        columns: [
                new Ext.grid.RowNumberer({ header: '', width: 25 }), //设置行号
                {
                header: "登录帐号",
                dataIndex: 'Name',
                width: 100,
                menuDisabled: true
            },
                    {
                    header: "用户名",
                    dataIndex: 'Nickname',
                    width: 140,
                    menuDisabled: true,
                    renderer: renderUserName
                }, 
              {
                  header: "性别",
                  dataIndex: 'Sex',
                  width: 100,
                  menuDisabled: true,
                  renderer: formartSex
              }, {
                  header: "联系电话",
                  dataIndex: 'Tel',
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
