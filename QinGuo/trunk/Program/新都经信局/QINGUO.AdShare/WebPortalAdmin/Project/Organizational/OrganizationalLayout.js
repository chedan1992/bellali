Ext.onReady(function () {
//    /*
//    * ================有组织架构页面布局=======================
//    */
//  
//    //定义树的加载器 
//    var treeloader = new Ext.ux.tree.TreeGridLoader({
//        url: "/Organizational/SearchData"
//    });
//    //数据源
//    var className = ''; //页面类名
//    if (this.frameElement) {
//        className = this.frameElement.name
//    }
//    //定义treegrid 
//    var center = new Ext.ux.tree.TreeGrid({
//        region: 'center',
//        layout: 'fit',
//        border: false,
//        enableSort: false,
//        id: 'tg',
//        animate: true,
//        columnLines: true, // 斑马线 ,  
//        useArrows: true,
//        loadMask: true, // 如何加载     
//        lines: true, //显示树形控件的前导线
//        stripeRows: true, //隔行颜色不同
//        rootVisible: false,
//        containerScroll: true,
//        headersDisabled: true,
//        columns: [
//                    {
//                        header: '名称',
//                        dataIndex: 'text',
//                        menuDisabled: true,
//                        width:250
//                    },
//                     {
//                         header: "编码",
//                         dataIndex: 'Code',
//                         width: 180
//                     },
////                     {
////                         header: '简称',
////                         dataIndex: 'NameTitle',
////                         menuDisabled: true,
////                         width:120
////                     },
//                     {
//                         header: '分类',
//                         dataIndex: 'Attribute',
//                         menuDisabled: true,
//                         width:100,
//                         tpl: new Ext.XTemplate('{Attribute:this.Attribute}', {//1:集团 2:公司 3:分公司 4:子公司
//                             Attribute: function (val) {
//                                 if (val==1) {
//                                     return '集团';
//                                 }
//                                 else if (val ==2) {
//                                     return '公司';
//                                 }
//                                 else if (val == 3) {
//                                     return '分公司';
//                                 }
//                                 else if (val == 4) {
//                                     return '子公司';
//                                 }
//                             }
//                         })
//                     },
//                    {
//                        header: '联系人',
//                        dataIndex: 'LinkUser',
//                        menuDisabled: true,
//                        align: 'center',
//                        width: 100
//                    },
//                     {
//                         header: '联系电话',
//                         dataIndex: 'Phone',
//                         menuDisabled: true,
//                         align: 'center',
//                         width: 100
//                     },
//                      {
//                          header: '状态',
//                          dataIndex: 'Status',
//                          menuDisabled: true,
//                          align: 'center',
//                          width: 60,
//                          tpl: new Ext.XTemplate('{Status:this.Status}', {
//                             Status: formartEnableOrDisable
//                         })
//                      }
////                      {
////                          header: '类型',
////                          dataIndex: 'IsSystem',
////                          align: 'center',
////                          menuDisabled: true,
////                          width: 120,
////                          tpl: new Ext.XTemplate('{IsSystem:this.IsSystem}', {
////                              IsSystem: function (val) {
////                                  if (val) {
////                                      return '系统定义';
////                                  }
////                                  else {
////                                      return '用户创建';
////                                  }

////                              }
////                          })
////                      },
////                     {
////                         header: '添加时间',
////                         dataIndex: 'CreateTime',
////                         menuDisabled: true,
////                         width:300,
////                         tpl: new Ext.XTemplate('{CreateTime:this.formatYear}', {
////                             formatYear: formartTreeGridTime
////                         })
////                     }
//                 ],
//        loader: treeloader,
//        tbar: tbar(className),
//        columnsText: "显示/隐藏列",
//        viewConfig: {
//            forceFit: true//True表示为自动展开/缩小列的宽度以适应grid的宽度，这样就不会出现水平的滚动条
//        },
//        //绑定加载器 
//        listeners: {
//            render: function () {
//            }
//        }

//    });

//    center.expandAll();  
//    //viewport
//    var viewport = new Ext.Viewport({
//        layout: 'border',
//        id: 'viewport',
//        items: [center],
//        listeners: {
//            afterrender: function (t) {
//                var creator = getLoginUser(); //获取用户信息
//                if (!creator.IsMain) {
//                    //Ext.getCmp("tg").columns[3].hidden=true; //隐藏某列索引从1开始
//                }
//            }
//        }
//    });

    /*
    * ================无组织架构页面布局=======================
    */

    //定义树的加载器 
    var treeloader = new Ext.ux.tree.TreeGridLoader({
        url: "/Organizational/SearchData"
    });
    //数据源
    var className = ''; //页面类名
    if (this.frameElement) {
        className = this.frameElement.name
    }
    //定义treegrid 
    var center = new Ext.ux.tree.TreeGrid({
        region: 'center',
        layout: 'fit',
        border: false,
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
                        header: '名称',
                        dataIndex: 'text',
                        menuDisabled: true,
                        width: 250
                    },
                     {
                         header: "编码",
                         dataIndex: 'Code',
                         width: 180
                     },
        //                     {
        //                         header: '简称',
        //                         dataIndex: 'NameTitle',
        //                         menuDisabled: true,
        //                         width:120
        //                     },
                     {
                     header: '分类',
                     dataIndex: 'Attribute',
                     menuDisabled: true,
                     width: 100,
                     tpl: new Ext.XTemplate('{Attribute:this.Attribute}', {//1:集团 2:公司 3:分公司 4:子公司
                         Attribute: function (val) {
                             if (val == 1) {
                                 return '集团';
                             }
                             else if (val == 2) {
                                 return '公司';
                             }
                             else if (val == 3) {
                                 return '分公司';
                             }
                             else if (val == 4) {
                                 return '子公司';
                             }
                         }
                     })
                 },
                    {
                        header: '联系人',
                        dataIndex: 'LinkUser',
                        menuDisabled: true,
                        align: 'center',
                        width: 100
                    },
                     {
                         header: '联系电话',
                         dataIndex: 'Phone',
                         menuDisabled: true,
                         align: 'center',
                         width: 100
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
                      }
        //                      {
        //                          header: '类型',
        //                          dataIndex: 'IsSystem',
        //                          align: 'center',
        //                          menuDisabled: true,
        //                          width: 120,
        //                          tpl: new Ext.XTemplate('{IsSystem:this.IsSystem}', {
        //                              IsSystem: function (val) {
        //                                  if (val) {
        //                                      return '系统定义';
        //                                  }
        //                                  else {
        //                                      return '用户创建';
        //                                  }

        //                              }
        //                          })
        //                      },
        //                     {
        //                         header: '添加时间',
        //                         dataIndex: 'CreateTime',
        //                         menuDisabled: true,
        //                         width:300,
        //                         tpl: new Ext.XTemplate('{CreateTime:this.formatYear}', {
        //                             formatYear: formartTreeGridTime
        //                         })
        //                     }
                 ],
        loader: treeloader,
        tbar: tbar(className),
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

    center.expandAll();
    //viewport
    var viewport = new Ext.Viewport({
        layout: 'border',
        id: 'viewport',
        items: [center],
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