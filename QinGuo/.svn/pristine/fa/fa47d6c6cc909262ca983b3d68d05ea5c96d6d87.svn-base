/*
列表页 grid 通用
*/
var PageSize = 20; //每页显示记录条数
var signdata = [['类似于', 'like'], ['等于', '='], ['不等于', '<>'], ['大于', '>'], ['大于等于', '>='], ['小于', '<'], ['小于等于', '<=']]; //提供查询比较符号

//多选列
//分页数据集
//model:模型名称 url:后台请求地址 className:页面类名称
function GridStore(ReaderModel, url, className, SortField, direction) {

    var sort = "CreateTime"; //排序字段
    if (SortField != null && SortField != 'undefined' && SortField!="") {
        sort = SortField;
    }
    var direct = "desc"; //排序字段
    if (direction != null && direction != 'undefined' && direction != "") {
        direct = direction;
    }
    var GridStore = new Ext.data.Store({
        remoteSort: true, //服务器排序
        autoLoad: { params: { start: 0, limit: PageSize} },
        baseParams: { className: className }, //此参数控制页面数据查看权限
        reader: new Ext.data.JsonReader({
            totalProperty: "total",
            root: "rows",
            record: 'record',
        }, ReaderModel),
        proxy: new Ext.data.HttpProxy({
            url: url
        }),
        sortInfo: {
            field: sort,
            direction: direct
        },
        listeners: {
            beforeload: function (param) {
            }
        }
    });
    return GridStore;
}

//分页工具条
//store:数据集 gridId:列表控件ID
function bbar(store, gridId, className) {
    var comboxshow = new Ext.form.ComboBox({
        store: new Ext.data.SimpleStore({
            fields: ['text', 'value'],
            data: [['15', '15'], ['20', '20'], ['30', '30'], ['40', '40'], ['60', '60'], ['80', '80']]
        }),
        displayField: 'text',
        valueField: 'value',
        mode: 'local',
        id: 'pagesize',
        selectOnFocus: true,
        orceSelection: true,
        editable: false,
        triggerAction: 'all',
        allowBlank: false,
        value: PageSize,
        width: 50
    });
    comboxshow.on("select", function (comboxshow) {
        PagingToolbar.pageSize = parseInt(comboxshow.getValue());
        store.load({ params: { start: 0, limit: PagingToolbar.pageSize} });
    });
    //添加自定义显示列按钮操作
    var clums = new Ext.Button({
        text: '列设置',
        width: 40,
        iconCls: 'GTP_catalog'
    })

    clums.on("click", function (comboxshow) {
        ClumnHide(gridId, className);
    });

    var PagingToolbar = new Ext.PagingToolbar({
        pageSize: PageSize, //公共变量
        store: store,
        displayInfo: true,
        beforePageText: "第 ",
        afterPageText: "页，共 {0} 页",
        firstText: "第一页",
        prevText: "前一页",
        lastText: "末页",
        nextText: "下一页",
        refreshText: "刷新",
        emptyMsg: "没有要显示的数据",
        displayMsg: "<span style='font-size:13px;'>显示第{0}到{1}条,共{2}条</span>",
        items: ['-', '&nbsp;&nbsp;每页显示', comboxshow, '条', '-', clums]
    });
    return PagingToolbar;
}
//显示设置列隐藏弹框
function ClumnHide(gridId, className) {
    var grid = Ext.getCmp(gridId);
    if (grid) {
        //保存列设置
        function SaveColumnDate() {
            var xqCheck = top.Ext.getCmp('unitItems').items;
            var xq = '';
            for (var i = 0; i < xqCheck.length; i++) {
                //获取列的索引
                var index = getGridIndex(xqCheck.get(i).inputValue);
                //记录取消显示的列
                if (xqCheck.get(i).checked == false) {
                    xq += ',' + xqCheck.get(i).inputValue;
                    grid.getColumnModel().setHidden(index, true);
                }
                else {
                    grid.getColumnModel().setHidden(index, false);
                
                }
            }

            function getGridIndex(dataIndex) {
                var column = grid.getColumnModel();
                for (var i = 0; i < column.config.length; i++) {
                    if (column.config[i].dataIndex != "") {
                        if (column.config[i].dataIndex == dataIndex) {
                            return i;
                        }
                    }
                }
            }
            //保存后台设置
            Ext.Ajax.request({
                url: '/RoleManage/UpdateGridColumn',
                params: { ColumnIndex: xq, moudel: className },
                success: function (response) {
                    var rs = eval('(' + response.responseText + ')');
                    if (rs.success) {
                        columnHide = xq;
                    } else {
                        top.Ext.Msg.show({ title: "信息提示", msg: rs.msg, buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                    }
                    win.close();
                }
            });

        }
        //判断是否选中
        function checked(name) {
            var flag = true;
            var column = columnHide.split(',');
            for (var i = 0; i < column.length; i++) {
                if (column[i] == name) {
                    flag = false;
                    break;
                }
            }
            return flag;
        }
        var column = grid.getColumnModel();
        var unitColumns = [];
        for (var i = 0; i < column.config.length; i++) {
            if (column.config[i].dataIndex != "") {
                var flag = true;
                if (columnHide != "") {
                    flag = checked(column.config[i].dataIndex);
                }
                unitColumns.push({
                    boxLabel: column.config[i].header,
                    name: 'checkColumn',
                    width:80,
                    id:'chk'+column.config[i].dataIndex,
                    inputValue: column.config[i].dataIndex,
                    checked: flag
                });
            }
        }
        var win = new top.Ext.Window({
            title: '列设置(隐藏/显示)',
            id: 'ColumnSet',
            width:400,
            minHeight: 200,
            layout: 'fit',
            modal: true,
            shadow: false,
            stateful: false,
            items: new top.Ext.FormPanel({
                labelWidth: 65,
                frame: true,
                id:'columnForm',
                border: false,
                layout: 'fit',
               // autoScroll: true,
                bodyStyle:'overflow-y:auto;overflow-x:hidden',
                labelAlign: 'right',
                items: new top.Ext.form.CheckboxGroup({
                    id: 'unitItems',
                    fieldLabel: '选项',
                    labelWidth: 65,
                    columns:3,
                    items: unitColumns
                })
            }),
            border: false,
            closeAction: 'close',
            buttons: [{
                text: '确定',
                iconCls: 'GTP_save',
                id: 'GTP_save',
                handler: SaveColumnDate
            }, {
                text: '取消',
                iconCls: 'GTP_cancel',
                id: 'GTP_cancel',
                handler: function () {
                    top.Ext.getCmp('ColumnSet').close(); //直接销毁
                }
            }]
        });

        win.addListener('beforeshow', function (o) {
            win.center(); //始终居中显示
        });

        win.width =400;
        win.height = (270 + (unitColumns.length /3) *16);
        win.show();
    }
}



//弹框小窗体分页工具条
function smallbbar(store) {
    var PagingToolbar = new Ext.PagingToolbar({
        pageSize: PageSize, //公共变量
        store: store,
        displayInfo: true,
        beforePageText: "第 ",
        afterPageText: "页，共 {0} 页",
        firstText: "第一页",
        prevText: "前一页",
        lastText: "末页",
        nextText: "下一页",
        refreshText: "刷新",
        emptyMsg: "没有要显示的数据",
        displayMsg: "<span style='font-size:13px;'>显示第{0}到{1}条,共{2}条</span>"
    });
    return PagingToolbar;
}

//针对弹框的列表分页工具条
function bbarTop(store) {
    var combo = new top.Ext.form.ComboBox({
        store: new Ext.data.SimpleStore({
            fields: ['text', 'value'],
            data: [['15', '15'], ['20', '20'], ['30', '30'], ['40', '40'], ['60', '60'], ['80', '80']]
        }),
        displayField: 'text',
        valueField: 'value',
        mode: 'local',
        id: 'pagesize',
        selectOnFocus: true,
        orceSelection: true,
        editable: false,
        triggerAction: 'all',
        allowBlank: false,
        value: PageSize,
        width: 50
    });
    combo.on("select", function (comboBox) {
        PagingToolbar.pageSize = parseInt(comboBox.getValue());
        store.load({ params: { start: 0, limit: PagingToolbar.pageSize} });
    });
    var PagingToolbar = new top.Ext.PagingToolbar({
        pageSize: PageSize, //公共变量
        store: store,
        displayInfo: true,
        beforePageText: "第 ",
        afterPageText: "页，共 {0} 页",
        firstText: "第一页",
        prevText: "前一页",
        lastText: "末页",
        nextText: "下一页",
        refreshText: "刷新",
        emptyMsg: "没有要显示的数据",
        displayMsg: "<span style='font-size:13px;'>显示第{0}到{1}条,共{2}条</span>",
        items: ['-', '&nbsp;&nbsp;每页显示', combo, '条']
    });
    return PagingToolbar;
}


