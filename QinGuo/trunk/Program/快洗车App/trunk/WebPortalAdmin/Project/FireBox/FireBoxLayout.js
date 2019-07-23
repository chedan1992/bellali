Ext.onReady(function () {

    //快捷查询,如果不需要,可以不用定义
    var className = ''; //页面类名
    if (this.frameElement) {
        className = this.frameElement.name
    }

    // searchData = [['全查询', ''], ['箱体位置', 'Address'], ['查询简码', 'Name']];

    DistinBar = 2; //右键显示切换按钮显示
    PageSize = 15;
    //转义列
    var Member = Ext.data.Record.create([
              { name: "Id", type: "string", mapping: "Id" },
              { name: "Name", type: "string", mapping: "Name" },
              { name: "Address", type: "string", mapping: "Address" },
              { name: "QrCode", type: "string", mapping: "QrCode" },
              { name: "Status", type: "Int", mapping: "Status" },
              { name: "EquipmentCount", type: "Int", mapping: "EquipmentCount" },
              { name: "Img", type: "string", mapping: "Img" },
              { name: "SysId", type: "string", mapping: "SysId" },
              { name: "CreateTime", type: "datetime", mapping: "CreateTime" }
    ]);
    //数据源
    var store = GridStore(Member, '/FireBox/SearchData');

    var sm = new Ext.grid.CheckboxSelectionModel();

    var grid = new Ext.grid.GridPanel({
        layout: 'fit',
        id: 'gg',
        region: 'center',
        style: 'border-left:0px;',
        height: 300,
        sm: sm,
        store: store,
        stripeRows: true, //隔行颜色不同
        defult: {
            sortable: false
        },
        loadMask: { msg: '数据请求中，请稍后...' },
        columns: [
                new Ext.grid.RowNumberer({ header: '', width: 25 }), //设置行号
                 sm, //设置复选框
         {
         header: "",
         dataIndex: 'Img',
         width: 20,
         align: 'center',
         menuDisabled: true,
         renderer: function (data, metadata, record, rowIndex, columnIndex, store) {
             var content = '<div><span style="vertical-align: middle;"><img  onclick="showImg(this)" style="vertical-align: middle;width:15px; height:15px;border:0"  src="../..' + data + '"/></span></div>';
             return content;
         }
     },
                 {
                     header: "二维码编码",
                     dataIndex: 'QrCode',
                     flex: 1,
                     menuDisabled: true
                 },
                {
                    header: "箱体位置",
                    dataIndex: 'Address',
                    flex: 1,
                    menuDisabled: true
                },
             {
                 header: "查询简码",
                 dataIndex: 'Name',
                 flex: 1,
                 menuDisabled: true
             },
               {
                   header: "设备数量",
                   dataIndex: 'EquipmentCount',
                   flex: 1,
                   menuDisabled: true,
                   renderer: function (data, metadata, record, rowIndex, columnIndex, store) {
                       if (data > 0) {
                           var content = '<a href="#" onclick="showEqument()">' + data + '</a>';
                           return content;
                       }
                       return data;
                   }
               },

                    {
                        header: '添加时间',
                        dataIndex: 'CreateTime',
                        flex: 1,
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

    grid.store.on("load", function () {
        grid.getSelectionModel().selectFirstRow(); //用这个就可以了，前提是你要有SelectionModel的实现
    });
    grid.store.on('beforeload', function (node) {
        grid.getStore().proxy.conn.url = '/FireBox/SearchData?treeNodeId=' + treeNodeId;
    });
    //单机事件
    //grid.addListener('rowclick', Gridrowclick);
    //    grid.addListener('rowdblclick', dbGridClick);



    var north = new Ext.FormPanel({
        id: "formPanel",
        border: false,
        region: 'north',
        labelAlign: 'right',
        autoScroll: true,
        height: 35,
        items: [
                  {
                      layout: "column", // 从左往右的布局
                      bodyStyle: 'padding-top:5px;padding-left:10px;padding-right:10px;',
                      border: false,
                      items: [
                                              {
                                                  layout: "form", // 从上往下的布局
                                                  labelWidth: 75,
                                                  border: false,
                                                  items: [
                                                  {
                                                      name: 'txtAddress',
                                                      xtype: 'textfield',
                                                      id: 'txtAddress',
                                                      maxLength: 50,
                                                      fieldLabel: '箱体位置',
                                                      maxLengthText: '箱体位置长度不能超过50个字符',
                                                      anchor: '90%'
                                                  }]
                                              },
                                              {
                                                  layout: "form", // 从上往下的布局
                                                  labelWidth: 75,
                                                  border: false,
                                                  items: [
                                                  {
                                                      name: 'txtName',
                                                      xtype: 'textfield',
                                                      id: 'txtName',
                                                      maxLength: 50,
                                                      fieldLabel: '查询简码',
                                                      maxLengthText: '简码长度不能超过50个字符',
                                                      anchor: '90%'
                                                  }]
                                              },
                                                {
                                                    layout: "form",
                                                    border: false,
                                                    style: 'margin-left:10px',
                                                    labelWidth: 150,
                                                    items: [
                                                    new Ext.Button({
                                                        text: '搜索',
                                                        iconCls: 'GTP_search',
                                                        id: 'GTP_search',
                                                        tooltip: '搜索满足条件的数据',
                                                        scope: this,
                                                        handler: ChangeSearch
                                                    })
                                                ]
                                                }
                            ]
                  }
                ]

    });



    //viewport
    var viewport = new Ext.Viewport({
        layout: 'border',
        items: [north, grid],
        listeners: {
            afterrender: function () {

            }
        }
    });
});
