//流程审核列表
Ext.onReady(function () {

    var className = ''; //页面类名
    if (this.frameElement) {
        className = this.frameElement.name
    }
    //转义列
    var ETask = Ext.data.Record.create([
              { name: "Id", type: "string", mapping: "Id" },
              { name: "Title", type: "string", mapping: "Title" },
              { name: "FlowType", type: "Int", mapping: "FlowType" },
              { name: "FlowStatus", type: "Int", mapping: "FlowStatus" },
              { name: "ApprovalTime", type: "datetime", mapping: "ApprovalTime" },
              { name: "AuditTime", type: "datetime", mapping: "AuditTime" },
              { name: "Reamrk", type: "string", mapping: "Reamrk" },
              { name: "ApprovalUserName", type: "string", mapping: "ApprovalUserName" },
              { name: "CompanyId", type: "string", mapping: "CompanyId" },
               { name: "MasterId", type: "string", mapping: "MasterId" },
              { name: "AuditUserName", type: "string", mapping: "AuditUserName" }
            ]);
    //数据源（第三个参数不传，此页面不需要受数据权限控制）
    var store = GridStore(ETask, '/Flow/SearchDoWorkData');

    //快捷查询,如果不需要,可以不用定义
    searchData = [['全查询', ''], ['标题名称', 'Title']];

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
              header: "",
              dataIndex: 'Id',
              width:25,
              sortable: false,
              menuDisabled: true,
              renderer: function (value, meta, record, rowIdx, colIdx, store) {
                  var content = '<a href="#" style="vertical-align: middle;cursor:pointer" onclick="LookWork()"><img style="vertical-align: middle; width:18px; height:18px;border:0" class="GTP_view"  src="../../Content/Extjs/resources/images/default/s.gif"/>查看</a>';
                  return content;
              }
          },
            {
                header: "标题名称",
                dataIndex: 'Title',
                width: 130,
                sortable: false,
                menuDisabled: true
            },
         {
             header: '类型',
             sortable: false,
             dataIndex: 'FlowType',
             menuDisabled: true,
             width: 30,
             renderer: formartFlowType
         },
         {
             header: '状态',
             sortable: false,
             dataIndex: 'FlowStatus',
             menuDisabled: true,
             width: 30,
             renderer: formartFlowStatus
         },
           {
               header: '申请人',
               sortable: false,
               dataIndex: 'ApprovalUserName',
               menuDisabled: true,
               width: 40
           },
           {
               header: '审核人',
               sortable: false,
               dataIndex: 'AuditUserName',
               menuDisabled: true,
               width: 40
           },
         {
             header: '申请时间',
             sortable: false,
             dataIndex: 'ApprovalTime',
             menuDisabled: true,
              flex:1
         },
            {
                header: '审核时间',
                sortable: false,
                dataIndex: 'AuditTime',
                menuDisabled: true,
                width: 160
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













