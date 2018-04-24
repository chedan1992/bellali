//流程审核列表
Ext.onReady(function () {

    var className = ''; //页面类名
    if (this.frameElement) {
        className = this.frameElement.name
    }
    //转义列
    var ETask = Ext.data.Record.create([
              { name: "Id", type: "string", mapping: "Id" },
              { name: "Img", type: "string", mapping: "Img" },
              { name: "Url", type: "string", mapping: "Url" },
              { name: "Status", type: "Int", mapping: "Status" },
              { name: "CreateTime", type: "datetime", mapping: "CreateTime" },
              { name: "AuditorTime", type: "datetime", mapping: "AuditorTime" },
              { name: "Reamrk", type: "string", mapping: "Reamrk" },
              { name: "MLoginName", type: "string", mapping: "MLoginName" },
              { name: "MUserName", type: "string", mapping: "MUserName" },
              { name: "PUserName", type: "string", mapping: "PUserName" },
              { name: "PLoginName", type: "string", mapping: "PLoginName" },
              { name: "IsShow", type: "bool", mapping: "IsShow" },
              { name: "Clicknum", type: "string", mapping: "Clicknum" },
              { name: "Introduction", type: "string", mapping: "Introduction" },
              { name: "Type", type: "int", mapping: "Type" }
    ]);
    //数据源（第三个参数不传，此页面不需要受数据权限控制）
    var store = GridStore(ETask, '/AdAShare/SearchData');

    //快捷查询,如果不需要,可以不用定义
    searchData = [['全查询', ''], ['用户名称', 'm.UserName'], ['手机号', 'm.LoginName']];

    var grid = new Ext.grid.GridPanel({
        region: 'center',
        layout: 'fit',
        id: 'gg',
        store: store,
        stripeRows: true, //隔行颜色不同
        border: false,
        loadMask: { msg: '数据请求中，请稍后...' },
        columns: [
            new Ext.grid.RowNumberer({ header: '', width: 25 }), //设置行号
            {
                header: "广告地址",
                dataIndex: 'Url',
                width: 130,
                sortable: false,
                menuDisabled: true
            },
            {
                header: "广告图片",
                sortable: false,
                dataIndex: 'Img',
                menuDisabled: true,
                width: 100,
                renderer: formartImg
            },
         {
             header: '类型',
             sortable: false,
             dataIndex: 'Type',
             menuDisabled: true,
             width: 30,
             renderer: formartType
         },
            {
                header: "是否显示",
                dataIndex: 'IsShow',
                width: 20,
                sortable: false,
                menuDisabled: true,
                renderer: formartIsShow
            },
         {
             header: '点击量',
             sortable: false,
             dataIndex: 'Clicknum',
             menuDisabled: true,
             width: 40
         },
           {
               header: '用户',
               sortable: false,
               dataIndex: 'PUserName',
               menuDisabled: true,
               width: 40,
               renderer: formartPUserName
           },
         {
             header: '创建时间',
             sortable: false,
             dataIndex: 'CreateTime',
             menuDisabled: true,
             width: 40
         },
         {
             header: '状态',
             sortable: false,
             dataIndex: 'Status',
             menuDisabled: true,
             width: 40,
             renderer: formartStatus
         },
         {
             header: '审核人',
             sortable: false,
             dataIndex: 'MUserName',
             menuDisabled: true,
             width: 40,
             renderer: formartMUserName
         },
         {
             header: '审核间',
             sortable: false,
             dataIndex: 'AuditorTime',
             menuDisabled: true,
             width: 40
         },
         {
             header: '审批意见',
             sortable: false,
             dataIndex: 'Introduction',
             menuDisabled: true,
             width: 40
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











