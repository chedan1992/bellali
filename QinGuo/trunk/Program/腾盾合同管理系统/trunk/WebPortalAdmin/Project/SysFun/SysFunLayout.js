Ext.onReady(function () {
    /*
    * ================页面布局=======================
    */
    var tree = new Ext.tree.TreePanel({
        title: '分级列表',
        width: 200,
        layout: 'fit',
        id: 'tree',
        autoScroll: true,
        animate: true,
        enableDD: false,
        bodyStyle: 'padding-top:4px',
        lines: true, //显示树形控件的前导线
        containerScroll: true,
        root: {
            text: '分级列表',
            expanded: true,
            children: [
                        {
                            text: '菜单配置员',
                            id: '0',
                            leaf: true
                        },
                        {
                            text: '系统管理员',
                            id: '1',
                            leaf: true
                        },
                        {
                            text: '普通管理员',
                            id: '2',
                            leaf: true
                        },
                    ]
        },
        minSize: 175,
        maxWidth: 200,
        margins: '0 2 0 0',
        tbar: [
                {
                    xtype: 'tbtext',
                    height: 21,
                    text: ''
                }
            ],
        listeners: {
            afterrender: function (tree) {
                tree.getRootNode().childNodes[0].select(); //默认选中第一个节点
            },
            click: treeitemclick
        }
    });

    var west = new Ext.Panel({
        region: 'west',
        layoutConfig: {
            animate: true
        },
        animCollapse: true,
        split: true,
        width: 200,
        minSize: 175,
        maxSize: 400,
        margins: '0 5 0 0',
        layout: 'fit',
        bodyStyle: 'border-left:0px;border-top:0px;border-bottom:0px;',
        items: new Ext.TabPanel({
            border: false,
            activeTab: 0,
            tabPosition: 'bottom',
            items: [tree]
        })
    });


    //定义树的加载器 
    var treeloader = new Ext.ux.tree.TreeGridLoader({
        url: "/SysFun/InitMain?1=1"
    });

    //定义treegrid 
    var center = new Ext.ux.tree.TreeGrid({
        region: 'center',
        layout: 'fit',
        border: true,
        enableSort: false,
        id: 'tg',
        animate: true,
        columnLines: true, // 斑马线 ,  
        useArrows: true,
        loadMask: true, // 如何加载     
        lines: true, //显示树形控件的前导线
        stripeRows: true, //隔行颜色不同
        rootVisible: false,
        containerScroll: true,
        headersDisabled: true,
        columns: [
                    {
                        header: '导航名称',
                        dataIndex: 'text',
                        menuDisabled: true,
                        width: 170
                    },
                     {
                         header: "导航地址",
                         dataIndex: 'pageUrl',
                         width: 180
                     },
                     {
                         header: '导航图标',
                         dataIndex: 'iconCls',
                         menuDisabled: true,
                         width: 90
                     },
                     {
                         header: '类名称',
                         dataIndex: 'className',
                         menuDisabled: true,
                         width: 80
                     },
                    {
                        header: '排序',
                        dataIndex: 'funSort',
                        menuDisabled: true,
                        align: 'center',
                        width: 60
                    },
                      {
                          header: '状态',
                          dataIndex: 'Status',
                          menuDisabled: true,
                          align: 'center',
                          width: 60,
                          tpl: new Ext.XTemplate('{Status:this.Status}', {
                             Status: formartEnableOrDisable
                         })
                      },
                      {
                          header: '类型',
                          dataIndex: 'IsSystem',
                          align: 'center',
                          menuDisabled: true,
                          width: 120,
                          tpl: new Ext.XTemplate('{IsSystem:this.IsSystem}', {
                              IsSystem: function (val) {
                                  if (val) {
                                      return '系统定义';
                                  }
                                  else {
                                      return '用户创建';
                                  }

                              }
                          })
                      }
//                     {
//                         header: '添加时间',
//                         dataIndex: 'CreateTime',
//                         menuDisabled: true,
//                         width: 230,
//                         tpl: new Ext.XTemplate('{CreateTime:this.formatYear}', {
//                             formatYear: formartTreeGridTime
//                         })
//                     }
                 ],
        loader: treeloader,
        tbar: tbar('sysfun'),
        columnsText: "显示/隐藏列",
        viewConfig: {
            forceFit: true//True表示为自动展开/缩小列的宽度以适应grid的宽度，这样就不会出现水平的滚动条
        },
        //绑定加载器 
        listeners: {
            render: function () {
            }
        }

    });

    // 响应加载前事件，传递node参数 
    center.on('beforeload', function (node) {
        if (treeNodeId == -1) {
            treeNodeId = 0;
        }
        center.loader.dataUrl = "/SysFun/InitMain?id=" + treeNodeId; // 定义子节点的Loader 
    }, treeloader);

    //viewport
    var viewport = new Ext.Viewport({
        layout: 'border',
        id: 'viewport',
        items: [west, center],
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