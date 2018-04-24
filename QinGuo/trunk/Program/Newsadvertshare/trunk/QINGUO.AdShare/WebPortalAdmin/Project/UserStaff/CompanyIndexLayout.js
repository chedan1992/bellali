Ext.onReady(function () {

    var className = ''; //页面类名
    if (this.frameElement) {
        className = this.frameElement.name
    }
    //转义列
    var Master = Ext.data.Record.create([
              { name: "Id", type: "string", mapping: "Id" },
              { name: "LoginName", type: "string", mapping: "LoginName" },
               { name: "Cid", type: "string", mapping: "Cid" },
              
              { name: "UserName", type: "string", mapping: "UserName" },
              { name: "OperateNum", type: "string", mapping: "OperateNum" },
              { name: "CardNum", type: "string", mapping: "CardNum" },
              { name: "Sex", type: "int", mapping: "Sex" },
              { name: "Pwd", type: "string", mapping: "Pwd" },
              { name: "Email", type: "string", mapping: "Email" },
              { name: "Phone", type: "string", mapping: "Phone" },
              { name: "Name", type: "string", mapping: "Name" },
              { name: "Money", type: "string", mapping: "Money" },
              { name: "HeadImg", type: "string", mapping: "HeadImg" },
              { name: "Status", type: "int", mapping: "Status" },
              { name: "IsMain", type: "bool", mapping: "IsMain" },
              { name: "CreateTime", type: "datetime", mapping: "CreateTime" },
              { name: "CreaterName", type: "string", mapping: "CreaterName" },
              { name: "TotalBalance", type: "string", mapping: "TotalBalance" },
               { name: "Alipay", type: "string", mapping: "Alipay" },
              { name: "InCome", type: "string", mapping: "InCome" },
              { name: "OrderCount", type: "string", mapping: "OrderCount" }
            ]);

    //快捷查询,如果不需要,可以不用定义
    searchData = [['全查询', ''], ['用户名称', 'UserName'], ['登录账号', 'LoginName'], ['身份证号码', 'CardNum'], ['公司名称', 'Name']];

    //数据源
    var store = GridStore(Master, '/UserStaff/SearchDataByCompany?Id=-1');

    var grid = new Ext.grid.GridPanel({
        region: 'center',
        layout: 'fit',
        id: 'gg',
        store: store,
        stripeRows: true, //隔行颜色不同
        border: false,
        defult: {
            sortable: false,
            menuDisabled: true
        },
        loadMask: { msg: '数据请求中，请稍后...' },
        columns: [
                new Ext.grid.RowNumberer({ header: '', width: 25 }), //设置行号
                {
                header: "用户名称",
                dataIndex: 'UserName',
                menuDisabled: true,
                width: 60,
                renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                    if (value) {
                        return value;
                    }
                    else {
                        return '<span style="color:silver">(' + record.data.LoginName + ')</span>';
                    }
                }
            }, {
                header: "登录账号",
                dataIndex: 'LoginName',
                width: 80,
                menuDisabled: true
            },
              {
                  header: "身份证号码",
                  dataIndex: 'CardNum',
                  flex: 4,
                  menuDisabled: true
              },
              {
                  header: "性别",
                  dataIndex: 'Sex',
                  width: 50,
                  renderer: formartSex,
                  menuDisabled: true
              },
              {
                  header: "邮箱",
                  dataIndex: 'Email',
                  flex: 2,
                  menuDisabled: true
              },
        //            {
        //                header: "所属公司",
        //                dataIndex: 'Name',
        //                flex: 2,
        //                menuDisabled: true,
        //                renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
        //                    if (value) {
        //                        return value;
        //                    }
        //                    else {
        //                        return '<span style="color:silver">(暂未绑定公司)</span>';
        //                    }
        //                }
        //            },
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
        }
    });

    // 响应加载前事件，传递node参数 
    grid.store.on('beforeload', function (node) {
        grid.getStore().proxy.conn.url = '/UserStaff/SearchDataByCompany?Id=' + treeNodeId;
    });
    //默认选中第一行
    grid.store.on("load", function () {
        grid.getSelectionModel().selectFirstRow(); //用这个就可以了，前提是你要有SelectionModel的实现
    });

    var viewport = new Ext.Viewport({
        layout: 'border',
        id: 'viewport',
        items: [
            {
                region: 'center',
                id: 'eastOrganization',
                bodyStyle: 'border-top:0px;border-bottom:0px',
                border: false,
                layout: 'fit',
                items: [grid]
            }]
    });

});


//新增
function AddDate() {
    var window = CreateFromWindow("新增", "");
    var form = top.Ext.getCmp('formPanel');
    form.form.reset();
    top.Ext.getCmp("Pwd").setVisible(true); //显示密码框
    url = '/UserStaff/SaveParentData';
    window.show();
    init();
}