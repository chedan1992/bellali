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
        url: "/ShopCategory/SearchData?id=0&className=ShopCategory"
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
                    width:300
                },
                  {
                      header: '编码',
                      dataIndex: 'OrderNum',
                      menuDisabled: true,
                      width: 150
                  },
//         {
//             header: "标识图",
//             dataIndex: 'PicUrl',
//             width: 80,
//             align: 'center',
//             menuDisabled: true,
//             tpl: new Ext.XTemplate('{PicUrl:this.PicUrl}', {
//                 PicUrl: function (value, meta, record, rowIdx, colIdx, store) {
//                     var content = '';
//                     if (value) {
//                         content = '<div><span style="vertical-align: middle;"><img style="vertical-align: middle; width:20px; height:20px;border:0"  src="../..' + value + '"/></span></div>';
//                     }
//                     return content;
//                 }
//             })
//         },
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
              header: '备注',
              dataIndex: 'Remark',
              menuDisabled: true,
              width:350
          }
//        {
//            header: '添加时间',
//            dataIndex: 'CreateTime',
//            menuDisabled: true,
//            width: 230,
//            tpl: new Ext.XTemplate('{CreateTime:this.formatYear}', {
//                formatYear: formartTreeGridTime
//            })
//        }
        ],
        loader: treeloader,
        tbar: tbar('ShopCategory'),
        columnsText: "显示/隐藏列",
        //绑定加载器 
        listeners: {
           
        }

    });

    // 响应加载前事件，传递node参数 
    center.on('beforeload', function (node) {
        var Id = (!node.attributes["Id"] == true) ? "0" : node.attributes["Id"];
        center.loader.dataUrl = "/ShopCategory/SearchData?className=ShopCategory&id=" + Id; // 定义子节点的Loader 
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
                    //Ext.getCmp("tg").columns[3].hidden=true; //隐藏某列索引从1开始
                }
            }
        }
    });

});

