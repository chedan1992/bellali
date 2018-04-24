Ext.onReady(function () {
    /*
    * ================页面布局=======================
    */
    var north = new Ext.Panel({
        id: 'infoPanel',
        border: false,
        contentEl: 'info',
        region: 'north',
        bodyStyle: 'margin:0'
    });

    //定义树的加载器 
    var treeloader = new Ext.ux.tree.TreeGridLoader({
        url: "/SysCategory/SearchData?id=0&className=SysCategory"
    });

    //定义treegrid 
    var center = new Ext.ux.tree.TreeGrid({
        region: 'center',
        layout: 'fit',
        border: false,
        enableSort: false,
        bodyStyle: 'margin:0',
        id: 'tg',
        animate: true,
        lines: true, //显示树形控件的前导线
        stripeRows: true, //隔行颜色不同
        rootVisible: false,
        containerScroll: true,
        enableDD: false,
        headersDisabled: true,
        columns: [
        {
            header: '分类名称',
            dataIndex: 'Name',
            menuDisabled: true,
            width: 200
        },
         {
             header: '显示顺序',
             dataIndex: 'OrderNum',
             menuDisabled: true,
             width: 150
         },
         {
             header: '状态',
             dataIndex: 'Status',
             menuDisabled: true,
             width: 80,
             tpl: new Ext.XTemplate('{Status:this.Status}', {
                 Status: formartEnableOrDisable
             }
              )
         },
          {
              header: '节点类型',
              dataIndex: 'Depth',
              menuDisabled: true,
              width: 150,
              tpl: new Ext.XTemplate('{Depth:this.Depth}', {
                  Depth: formartDepth
              }
             )
          },
        {
            header: '添加时间',
            dataIndex: 'CreateTime',
            menuDisabled: true,
            width: 230,
            tpl: new Ext.XTemplate('{CreateTime:this.formatYear}', {
                formatYear: formartTreeGridTime
            })
        }
        ],
        loader: treeloader,
        tbar: tbar('SysCategory'),
        columnsText: "显示/隐藏列",
        //绑定加载器 
        listeners: {
        }

    });

    // 响应加载前事件，传递node参数 
    center.on('beforeload', function (node) {
        var ID = (!node.attributes["Id"] == true) ? "0" : node.attributes["Id"];
        center.loader.dataUrl = "/SysCategory/SearchData?className=SysCategory&id=" + ID; // 定义子节点的Loader 
    }, treeloader);

    //viewport
    var viewport = new Ext.Viewport({
        layout: 'border',
        id: 'viewport',
        items: [north, center],
        listeners: {
            afterrender: function (t) {
                var creator = getLoginUser(); //获取用户信息
                if (!creator.IsMain) {
                   
                }
            }
        }
    });

});

