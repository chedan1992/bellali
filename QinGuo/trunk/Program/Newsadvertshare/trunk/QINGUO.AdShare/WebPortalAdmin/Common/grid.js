/*
    列表页 grid 通用
*/
var PageSize =20; //每页显示记录条数
var signdata = [['类似于', 'like'], ['等于', '='], ['不等于', '<>'], ['大于', '>'], ['大于等于', '>='], ['小于', '<'], ['小于等于', '<=']]; //提供查询比较符号

//多选列
//分页数据集
//model:模型名称 url:后台请求地址 className:页面类名称
function GridStore(ReaderModel, url, className, SortField, direction) {

    var sort = "CreateTime";//排序字段
    if (SortField != null && SortField != 'undefined') {
        sort = SortField;
    }
    var direct = "desc"; //排序字段
    if (direction != null && direction != 'undefined') {
        direct = direction;
    }
    var GridStore = new Ext.data.Store({
        remoteSort: true,//服务器排序
        autoLoad: { params: { start: 0, limit: PageSize} },
        baseParams: { className: className }, //此参数控制页面数据查看权限
        reader: new Ext.data.JsonReader({
            totalProperty: "total",
            root: "rows",
            record: 'record'
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
//store:数据集
function bbar(store) {
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
            width:50
    });
    comboxshow.on("select", function (comboxshow) {
        PagingToolbar.pageSize = parseInt(comboxshow.getValue());
        store.load({ params: { start: 0, limit: PagingToolbar.pageSize} });
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
        items: ['-', '&nbsp;&nbsp;每页显示', comboxshow, '条']
    });
    return PagingToolbar;
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
            width:50
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


