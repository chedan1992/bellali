//grid单击默认进行编辑操作
function gridrowclick(grid, rowindex, e) {
    if (Ext.getCmp("NowAll").pressed != true) {
        var grid = Ext.getCmp("gg");
        var SourceGrid = Ext.getCmp("rightGrid");
        if (grid.store.data.length > 0) {
            var rows = grid.getSelectionModel().getSelections();
            if (rows.length == 1) {
                treeNodeId = rows[0].data["CusterId"];
                SourceGrid.getStore().reload();
            }
            else {
                SourceGrid.getStore().reload();
            }
        }
        else {
            SourceGrid.getStore().reload();
        }
    }
}
function dbGridClick() {
    FileAttach();
}
//渲染入库状态
function renderInStatus(val, metadata, record, rowIndex, columnIndex, store) {
    switch (val) {
        case 0:
            return "<span style='color:silver'>未入库</span>";
            break;
        case 1:
            return "<span style='color:green'>已入库</span>";
            break;
    }
}
//渲染退货状态
function renderOutStatus(val, metadata, record, rowIndex, columnIndex, store) {
    switch (val) {
        case 0:
            return "<span style='color:silver'>未确认</span>";
            break;
        case 1:
            return "<span style='color:green'>已确认</span>";
            break;
    }
}
function renderMoney(val, metadata, record, rowIndex, columnIndex, store) {
    if (val > 0) {
        return "<span style='color:red'>" + val + "</span>";
    }
    else if (val == 0) {
        return val;
    } else {
        return "<span style='color:green'>" + val + "</span>";
    }
}

//渲染财务状态
function renderFinancialState(val, metadata, record, rowIndex, columnIndex, store) {
    switch (val) {
        case -1:
            return "<span style='color:red'>已驳回</span>";
            break;
        case 0:
            return "未提交";
            break;
        case 1:
            return "<span style='color:orange'>待提交</span>";
            break;
        default:
            return "未提交";
            break;
    }
}

//主表凭证管理
function FileAttach() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].get("Id");
        ShowAttach("InCome", key);
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}
//字表凭证管理
function FileAttach2() {
    var grid = Ext.getCmp("rightGrid");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].get("Id");
        ShowAttach("OutCome", key);
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}

//主表明细
function BtnMainDetail() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].get("Id");
        var InNumber = rows[0].get("InNumber");
        var shop = new top.Ext.Window({
            width: 950,
            id: 'FileAttachWin',
            height: 500,
            closable: false,
            buttonAlign: 'right', //左边
            shadow: false,
            stateful: false,
            border: false,
            modal: true,
            //draggable : false,//禁止拖动
            resizable: false, //禁止缩放
            layout: 'fit',
            plain: true,
            autoDestroy: true,
            closeAction: 'close',
            title: '入库单明细(入库单号：' + InNumber + ')',
            items: {
                autoScroll: true,
                border: false,
                params: { KeyId: key },
                autoLoad: { url: '/Areas/CACAI/Project/Html/InComeBill.htm', scripts: true, nocache: true }
            },
            buttons: [
                        {
                        text: '取消',
                        iconCls: 'GTP_cancel',
                        tooltip: '取消当前的操作',
                        handler: function () {
                            top.Ext.getCmp("FileAttachWin").close();
                        }
                    }

                      ]
        }).show();
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }

}

//子表明细
function BtnDetails() {
    var grid = Ext.getCmp("rightGrid");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].get("Id");
        var OutNumber = rows[0].get("OutNumber");
        var shop = new top.Ext.Window({
            width: 950,
            id: 'FileAttachWin',
            height: 500,
            closable: false,
            buttonAlign: 'right', //左边
            shadow: false,
            stateful: false,
            border: false,
            modal: true,
            //draggable : false,//禁止拖动
            resizable: false, //禁止缩放
            layout: 'fit',
            plain: true,
            autoDestroy: true,
            closeAction: 'close',
            title: '入库单明细(退货单号：' + OutNumber + ')',
            items: {
                autoScroll: true,
                border: false,
                params: { KeyId: key },
                autoLoad: { url: '/Areas/CACAI/Project/Html/HPurchase.htm', scripts: true, nocache: true }
            },
            buttons: [
                        {
                            text: '取消',
                            iconCls: 'GTP_cancel',
                            tooltip: '取消当前的操作',
                            handler: function () {
                                top.Ext.getCmp("FileAttachWin").close();
                            }
                        }

                      ]
        }).show();
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }

}

function CardDetail(val) {
    Ext.getCmp("NowCustomer").toggle();
    Ext.getCmp("NowAll").toggle();

    var SourceGrid = Ext.getCmp("rightGrid");
    if (SourceGrid) {
        SourceGrid.getStore().reload();
    }
}
//显示附件信息
function ShowAttach(code, key) {
    if (top.Ext.getCmp("ImgShowWin")) {
        top.Ext.getCmp("ImgShowWin").close();
    }
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].get("Id");
        var CusterName = rows[0].get("CusterName");
        var TotalPrice = rows[0].get("TotalPrice");
        var title = '凭证预览' + '  ( <span style="color:red"> 供应商：' + CusterName + ',退货金额：' + TotalPrice + '</span>)';
        if (code == "InCome") {
            var title = '凭证预览' + '  ( <span style="color:red"> 供应商：' + CusterName + ',供应商金额：' + TotalPrice + '</span>)';
        }

        var index = 0; //获取选中的索引
        var data = [];
        var respon = Ext.lib.Ajax.getConnectionObject().conn;
        respon.open("post", "/SysFileAttach/GetFileList?KeyId=" + key, false);
        respon.send(null);
        var result = Ext.util.JSON.decode(respon.responseText);
        if (result.length > 0) {
            for (var i = 0; i < result.length; i++) {
                data.push(result[i].FilePath);
            }
        }
        var img1 = new top.ImgView({
            imgindex: index,
            src: data
        });
        var shop = new top.Ext.Window({
            width:850,
            id: 'ImgShowWin',
            height: 750,
            closable: true,
            border: false,
            buttonAlign: 'center', //左边
            layout: 'fit',
            plain: true,
            autoDestroy: true,
            closeAction: 'close',
            title: title,
            items: [img1],
            buttons: [
                                {
                                    text: '取消',
                                    iconCls: 'GTP_cancel',
                                    tooltip: '取消当前的操作',
                                    handler: function () {
                                        top.Ext.getCmp("ImgShowWin").close();
                                    }
                                }]
        }).show();

    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}

function tbarDIY(PageName) {
    //同步查询页面权限按钮
    var tb = new Ext.Toolbar();
    tb.add({
        text: '当前供应商',
        tooltip: '当前供应商',
        id: 'NowCustomer',
        pressed: true,
        handler: CardDetail
    });
    tb.add({
        text: '显示全部',
        tooltip: '显示全部',
        id: 'NowAll',
        pressed: false,
        handler: CardDetail
    });
    tb.add('-');
    tb.add({
        text: '刷新',
        tooltip: '刷新',
        id: 'BtnRefeshDetail',
        iconCls: 'GTP_refresh',
        handler: BtnRefeshDetail //因为是字符串类型,找不到方法,所以需转义一下
    });
//    tb.add('-');
//    tb.add({
//        text: '价格拆分',
//        tooltip: '价格拆分',
//        id: 'PriceCut',
//        iconCls: 'GTP_cut',
//        handler: BtnCut //因为是字符串类型,找不到方法,所以需转义一下
//    });
    tb.add('-');
    tb.add({
        text: '驳回',
        tooltip: '驳回',
        id: 'intransit',
        iconCls: 'GTP_intransit',
        handler: GTP_Allintransit //因为是字符串类型,找不到方法,所以需转义一下
    });
    tb.add('-');
    tb.add({
        text: '批量备注',
        tooltip: '批量备注',
        id: 'AllEditDetail',
        iconCls: 'GTP_signnameadd',
        handler: GTP_AllEditDetail //因为是字符串类型,找不到方法,所以需转义一下
    });
    
    //                    tb.add('-');
    //                    tb.add({
    //                        text: '价格合并',
    //                        tooltip: '价格合并',
    //                        id: 'PriceCombine',
    //                        iconCls: 'GTP_combine',
    //                        handler: BtnCut //因为是字符串类型,找不到方法,所以需转义一下
    //                    });
    



    tb.add('->');

    tb.add({
        text: '多选统计：',
        id: 'waitBtn',
        iconCls: 'GTP_count',
        handler: function () {
        }
    });

    tb.add({
        xtype: 'tbtext',
        text: '<span id="txtChart" style="color:blue; font-weight:400;">' +
        '原退货金额：<span id="totalMoney1">0</span>&nbsp;&nbsp;'   +
         '剩余货金额：<span id="totalMoney2">0</span>&nbsp;&nbsp;'+
          '盈亏金额：<span id="totalMoney3">0</span>&nbsp;&nbsp;'
    });
    tb.add('-');
    //查询条件
    var comName = new Ext.form.ComboBox({
        store: new Ext.data.SimpleStore({
            fields: ['text', 'value'],
            data: [['全查询', ''], ['供应商', 'CusterName'], ['退货单号', 'OutNumber'], ['采购单号', 'GetNumber']]//, ['在途', 'IsOnLine']
        }),
        displayField: 'text',
        id: 'comName2',
        valueField: 'value',                                                                                                    
        mode: 'local',
        selectOnFocus: true,
        forceSelection: true,
        editable: false,
        triggerAction: 'all',
        allowBlank: false,
        value: this.seachkey,
        width: 90,
        enableKeyEvents: true,
        listeners: {
            select: function (mysel) {
                var itemstr = mysel.lastSelectionText;
                if (itemstr.trim() == "全查询") {
                    Ext.getCmp("comContent2").setValue("");
                    Ext.getCmp("comSign2").setValue('like');
                }
                if (itemstr.indexOf('时间') > -1) {
                    Ext.getCmp("comTimeContent2").show();
                    Ext.getCmp("comContent2").hide();
                    Ext.getCmp("IsOnlineEnum").hide();
                    Ext.getCmp("FlowEnum2").hide();
                    Ext.getCmp("comSign2").setValue('like');
                }
                else if (itemstr.indexOf('在途') > -1) {
                    Ext.getCmp("comTimeContent2").hide();
                    Ext.getCmp("comContent2").hide();
                    Ext.getCmp("IsOnlineEnum").show();
                    Ext.getCmp("comSign2").setValue('=');
                }

                else {
                    Ext.getCmp("comTimeContent2").hide();
                    Ext.getCmp("comContent2").show();
                    Ext.getCmp("IsOnlineEnum").hide();
                    //Ext.getCmp("FlowEnum2").hide();
                    Ext.getCmp("comSign2").setValue('like');
                }
            },
            //回车事件
            specialkey: function (field, e) {
                if (e.getKey() == Ext.EventObject.ENTER) {
                    if (comName.isValid() && comSign.isValid()) {
                        SearchDate2();
                    }
                }
            }
        }
    });
    //查询方式
    var comSign = new Ext.form.ComboBox({
        store: new Ext.data.SimpleStore({
            fields: ['text', 'value'],
            data: [['类似于', 'like'], ['等于', '='], ['不等于', '<>'], ['大于', '>'], ['大于等于', '>='], ['小于', '<'], ['小于等于', '<=']]
        }),
        displayField: 'text',
        valueField: 'value',
        mode: 'local',
        id: 'comSign2',
        selectOnFocus: true,
        orceSelection: true,
        editable: false,
        triggerAction: 'all',
        allowBlank: false,
        value: 'like',
        width: 60,
        enableKeyEvents: true,
        listeners: {
            //回车事件
            specialkey: function (field, e) {
                if (e.getKey() == Ext.EventObject.ENTER) {
                    if (comName.isValid() && comSign.isValid()) {
                        SearchDate2();
                    }
                }
            }
        }
    });
    //查询内容
    var comContent = new Ext.form.TextField({
        width: 140,
        id: 'comContent2',
        emptyText: '请输入搜索条件',
        enableKeyEvents: true,
        listeners: {
            //回车事件
            specialkey: function (field, e) {
                if (e.getKey() == Ext.EventObject.ENTER) {
                    if (comName.isValid() && comSign.isValid()) {
                        SearchDate2();
                    }
                }
            }
        }
    });
    //查询时间内容
    var comTimeContent = new Ext.form.DateField({
        width: 120,
        id: 'comTimeContent2',
        emptyText: '选择时间',
        format: 'Y-m-d',
        hidden: true ,
        enableKeyEvents: true,
        listeners: {
            //回车事件
            specialkey: function (field, e) {
                if (e.getKey() == Ext.EventObject.ENTER) {
                    if (comName.isValid() && comSign.isValid()) {
                        SearchDate2();
                    }
                }
            }
        }
    });
    //查询枚举内容
    var comEnum = new Ext.form.ComboBox({
        store: new Ext.data.SimpleStore({
            fields: ['text', 'value'],
            data: [['是', '1'], ['否', '0']]
        }),
        displayField: 'text',
        valueField: 'value',
        mode: 'local',
        id: 'IsOnlineEnum',
        selectOnFocus: true,
        orceSelection: true,
        editable: false,
        triggerAction: 'all',
        allowBlank: false,
        value: '1',
        hidden: true,
        width: 120 ,
        enableKeyEvents: true,
        listeners: {
            //回车事件
            specialkey: function (field, e) {
                if (e.getKey() == Ext.EventObject.ENTER) {
                    if (comName.isValid() && comSign.isValid()) {
                        SearchDate2();
                    }
                }
            }
        }
    });

    tb.add('快捷查询:');
    tb.add(comName);
    tb.add(comSign);
    tb.add(comContent); //文本输入
    tb.add(comTimeContent); //时间框输入
    tb.add(comEnum); //启用 禁用 枚举

    tb.add([{
        text: '搜索',
        iconCls: 'GTP_search',
        id: 'GTP_search2',
        tooltip: '搜索满足条件的数据',
        scope: this,
        handler: function () {
            if (comName.isValid() && comSign.isValid()) {
                SearchDate2();
            }
        },
        enableKeyEvents: true,
        listeners: {
            //回车事件
            specialkey: function (field, e) {
                if (e.getKey() == Ext.EventObject.ENTER) {
                    if (comName.isValid() && comSign.isValid()) {
                        SearchDate2();
                    }
                }
            }
        }
    }]);
    return tb;
}
//主表选中事件
function MainCellclick() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length >= 1) {
        //进项统计
        var TotalPrice = 0;
        var LossPrice = 0;

        for (var i = 0; i < rows.length; i++) {
            TotalPrice += parseFloat(rows[i].get("TotalPrice")); //供应商金额
            LossPrice += parseFloat(rows[i].get("LossPrice")); //盈亏金额
        }
        document.getElementById("chart1").innerHTML = TotalPrice;
        document.getElementById("chart2").innerHTML = LossPrice;
    }
    else {
        document.getElementById("chart1").innerHTML = 0;
        document.getElementById("chart2").innerHTML = 0;
    }
}
function SearchDate2() {
    var comName = Ext.getCmp("comName2").getValue(); //字段名称
    var comSign = Ext.getCmp("comSign2").getValue(); //查询方式

    var comContent = Ext.getCmp("comContent2").getValue(); //查询值
    var comTimeContent = Ext.getCmp("comTimeContent2").value; //查询时间值
    if (!Ext.getCmp("comTimeContent2").hidden) {//时间控件
        comContent = comTimeContent;
        //时间列需要转换
        comName = "Convert(varchar(20)," + comName + ",120)";
    }
    else if (!Ext.getCmp("IsOnlineEnum").hidden)//枚举控件
    {
        comContent = Ext.getCmp("IsOnlineEnum").getValue();
    }
    if (Ext.getCmp("rightGrid")) {//列表grid
        var _store = Ext.getCmp("rightGrid").store;
        Ext.apply(_store.baseParams, { conditionField: comName, condition: comSign, conditionValue: comContent });
        _store.reload({ params: { start: 0, limit: parseInt(Ext.getCmp("pagesize2").getValue())} });
    }
}

//获取页面所有查询条件
function GetSearch2() {
    var param = GetParam();
    //高级查询
    var comName = Ext.getCmp("comName2").getValue(); //字段名称
    var comSign = Ext.getCmp("comSign2").getValue(); //查询方式
    var comContent = Ext.getCmp("comContent2").getValue(); //查询值
    var comTimeContent = Ext.getCmp("comTimeContent2").value; //查询时间值
    if (!Ext.getCmp("comTimeContent").hidden) {//时间控件
        comContent = comTimeContent;
        //时间列需要转换
        comName = "Convert(varchar(20)," + comName + ",120)";
    }
    else if (!Ext.getCmp("IsOnlineEnum").hidden)//枚举控件
    {
        comContent = Ext.getCmp("IsOnlineEnum").getValue();
    }
    param.conditionField = comName;
    param.condition = comSign;
    param.conditionValue = comContent;

    return urlEncode(param, "", true);
}

//刷新明细
function BtnRefeshDetail() {
    Ext.getCmp("rightGrid").store.reload();
}
//行选中事件
function cellclick() {
    var grid = Ext.getCmp("rightGrid");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length >= 1) {
        //进项统计
        var TotalPrice = 0;
        var PoolPrice = 0;
        var LossPrice = 0;
        for (var i = 0; i < rows.length; i++) {
            TotalPrice += parseFloat(rows[i].get("TotalPrice")); //数量
            PoolPrice += parseFloat(rows[i].get("PoolPrice"));
            LossPrice += parseFloat(rows[i].get("LossPrice")); 
        }
        document.getElementById("totalMoney1").innerHTML = TotalPrice;
        document.getElementById("totalMoney2").innerHTML = PoolPrice;
        document.getElementById("totalMoney3").innerHTML = LossPrice;
    }

    else {
        //货号：<span id="totalMoney1">0</span>&nbsp;&nbsp;，
        document.getElementById("txtChart").innerHTML = '' +
               '原退货金额：<span id="totalMoney1">0</span>&nbsp;&nbsp;'+
                '剩余退货金额：<span id="totalMoney2">0</span>&nbsp;&nbsp;'+
                 '盈亏金额：<span id="totalMoney3">0</span>&nbsp;&nbsp;'
    }
}

//提交财务
function GTPFinance() {
    //判断主表是单选还是多选
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        SingleFince();
    }
    else if (rows.length > 1) {
        AttachFince();
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
        return false;
    }
}

//批量提交财务
function AttachFince() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    var ids = [];
    for (var i = 0; i < rows.length; i++) {
        ids.push("'" + rows[i].data["RelationId"] + "'");
    }

    var ComfirmMsg = "确认批量提交单据到财务吗？";
    var confirm = top.Ext.MessageBox.confirm('系统确认', ComfirmMsg, function (e) {
        if (e == "yes") {
            top.Ext.MessageBox.wait('', '系统正在执行中,请稍后...'); //显示等待信息框   
            var GetNumber = rows[0].get("GetNumber"); //采购单号
            Ext.Ajax.request({
                url: '/CACAI/Finance/SubFinance',
                params: {
                    IsAll:true,//是否批量
                    MainId: ids.join(","),
                    MainDetailId:'',
                    MainMoney: '',
                    MainsMoney: '',
                    CusterId: '',
                    GetNumber:''
                },
                success: function (response) {
                    var rs = eval('(' + response.responseText + ')');
                    if (rs.success) {
                        MessageInfo("提交成功！", "right");
                        Ext.getCmp("gg").store.reload();
                    } else {
                        MessageInfo("提交失败！", "error");
                    }
                    top.Ext.MessageBox.hide(); //完成后隐藏消息框   
                },
                failure: function (response) {
                    top.Ext.MessageBox.hide(); //完成后隐藏消息框   
                    top.Ext.Msg.show({ title: "系统错误提示", msg: '操作失败，请联系开发人员!', buttons: Ext.Msg.OK, icon: Ext.MessageBox.ERROR });
                }
            });
        }
    });
}
//单独提交财务
function SingleFince() {
    var MainMoney = 0;
    var MainsMoney = 0;
    var MainId = [];
    var MainDetailId = [];
    //获取主表金额
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        MainMoney = parseFloat(rows[0].get("TotalPrice"));
        MainId.push(rows[0].get("RelationId"));
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
        return false;
    }
    //获取子表金额
    var grid = Ext.getCmp("rightGrid");
    var rowsDetail = grid.getSelectionModel().getSelections();
    //判断明细选中是否相同的供应商
    var CusterId = rows[0].get("CusterId");

    if (rowsDetail.length >= 1) {
        //进项统计
        for (var i = 0; i < rowsDetail.length; i++) {
            var Id = rowsDetail[i].get("CusterId");
            if (CusterId != Id) {
                top.Ext.Msg.show({ title: "信息提示", msg: "明细选中的供应商与入库单不一致，无法提交，请重新选择。", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                return false;
            }
            MainsMoney += parseFloat(rowsDetail[i].get("PoolPrice")); //数量
            MainDetailId.push(rowsDetail[i].get("RelationId"));
        }
    }

    var ComfirmMsg = "确认要提交单据到财务吗？";
    if (MainId.length <= 0) {
        top.Ext.Msg.show({ title: "信息提示", msg: "请先选中一条记录。", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
        return false;
    }
    if (MainDetailId.length <= 0) {
        ComfirmMsg = "当前没有选择明细，确认要提交单据到财务吗?";
        //top.Ext.Msg.show({ title: "信息提示", msg: "明细未选择，不能进行提交。", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
        //return false;
    }
    else if (MainsMoney < MainMoney && MainsMoney != 0) {
        ComfirmMsg = "所选明细金额小于入库金额，确认要提交单据到财务吗?";
    }
    else if (MainsMoney > MainMoney) {
        ComfirmMsg = "所选明细金额大于入库金额，多出" + (MainsMoney - MainMoney) + "元,系统将会自动拆分金额，确认要提交单据到财务吗？";
    }

    var confirm = top.Ext.MessageBox.confirm('系统确认', ComfirmMsg, function (e) {
        if (e == "yes") {
            //供应商金额   MainMoney
            //退款金额   MainsMoney
            top.Ext.MessageBox.wait('', '系统正在执行中,请稍后...'); //显示等待信息框
            var GetNumber = rows[0].get("GetNumber"); //采购单号
            Ext.Ajax.request({
                url: '/CACAI/Finance/SubFinance',
                params: {
                    IsAll: false, //是否批量
                    MainId: MainId.join(","),
                    MainDetailId: MainDetailId.join(','),
                    MainMoney: MainMoney,
                    MainsMoney: MainsMoney,
                    CusterId: CusterId,
                    GetNumber: GetNumber
                },
                success: function (response) {
                    var rs = eval('(' + response.responseText + ')');
                    if (rs.success) {
                        MessageInfo("提交成功！", "right");
                        Ext.getCmp("gg").store.reload();
                    } else {
                        MessageInfo("提交失败！", "error");
                    }
                    top.Ext.MessageBox.hide(); //完成后隐藏消息框   
                },
                failure: function (response) {
                    top.Ext.MessageBox.hide(); //完成后隐藏消息框   
                    top.Ext.Msg.show({ title: "系统错误提示", msg: '操作失败，请联系开发人员!', buttons: Ext.Msg.OK, icon: Ext.MessageBox.ERROR });
                }
            });
        }
    });
}

//价格拆分
function BtnCut() {
    var grid = Ext.getCmp("rightGrid");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var TotalPrice = parseFloat(rows[0].get("PoolPrice"));
        var key = rows[0].get("RelationId");
        var form = new top.Ext.FormPanel({
            labelWidth: 65,
            frame: true,
            border: false,
            labelAlign: 'right',
            bodyStyle: 'padding:25px 25px 0',
            id: 'formPanel',
            defaultType: 'textfield',
            items: [
                {
                    name: 'TotalPrice',
                    id: 'TotalPrice',
                    xtype: 'label',
                    readOnly: true,
                    fieldLabel: '原始金额',
                    text: TotalPrice,
                    anchor: '95%'
                },
                {
                    xtype: 'numberfield',
                    name: 'DecomposePrice',
                    id: 'DecomposePrice',
                    fieldLabel: '拆分金额',
                    anchor: '95%',
                    allowBlank: false,
                    value: 0,
                    minValue: 0,
                    maxValue: 999999999999,
                    enableKeyEvents: true,
                    listeners: {
                        'keyup': function (e) {
                            //设置剩余金额
                            var CheckNum = parseFloat(top.Ext.getCmp("TotalPrice").text);
                            var Money = top.Ext.getCmp("DecomposePrice").getValue();
                            top.Ext.getCmp("CutTotalPrice").setText(CheckNum - Money);
                        }
                    }
                },
               {
                   xtype: 'label',
                   id: 'CutTotalPrice',
                   fieldLabel: '剩余金额',
                   text: '0'
               }
        ]
        });

        //窗体
        var win = Window("window",'价格拆分', form);
        win.width = 300;
        win.height = 300;
        win.show();

        url = '/CACAI/Finance/BtnCut?Id=' + key + '&modify=1';
    }
    else {
        MessageInfo("请选中一条记录进行拆分！", "statusing");
    }
}



var rightClick = new Ext.menu.Menu({
    id: 'rightClickCont',
    items: [
//        {
//            id: 'rMenu1',
//            handler: EditDate,
//            iconCls: 'GTP_edit',
//            text: '编辑'
//        },'-',
        {
            id: 'rMenu2',
            handler: DeleteDate,
            iconCls: 'GTP_delete',
            text: '删除'
        },
        {
            id: 'rMenu3',
            handler: DeleteAll,
            text: '清空全部',
            iconCls: 'GTP_clear'
        }
    ]
});
//右键菜单代码关键部分 
function rightClickFn(grid, rowindex, e) {
    e.preventDefault();
    rightClick.showAt(e.getXY());
}
//情况全部
function DeleteAll() {
    if (Ext.getCmp('gg').store.data.length > 0) {
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要清空列表所有信息吗?', function (e) {
            if (e == "yes") {
                top.Ext.MessageBox.wait('', '系统正在执行中,请稍后...'); //显示等待信息框   
                Ext.Ajax.request({
                    url: '/CACAI/InComeBill/DeleteAll',
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            MessageInfo("清空成功！", "right");
                            Ext.getCmp("gg").store.reload();
                        } else {
                            MessageInfo("清空失败！", "error");
                        }
                        top.Ext.MessageBox.hide(); //完成后隐藏消息框   
                    },
                    failure: function (response) {
                        top.Ext.MessageBox.hide(); //完成后隐藏消息框   
                        top.Ext.Msg.show({ title: "系统错误提示", msg: '操作失败，请联系开发人员!', buttons: Ext.Msg.OK, icon: Ext.MessageBox.ERROR });
                    }
                });
            }
        });
    }
    else {
        top.Ext.Msg.show({ title: "信息提示", msg: '请选中一条记录', buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO }); 
    }
}
//JS日期函数 加几天 减几天 推后天数日期计算
function getthedate(dd, dadd) {
    //可以加上错误处理
    var a = new Date(dd)
    a = a.valueOf()
    a = a + dadd * 24 * 60 * 60 * 1000
    a = new Date(a);
    var m = a.getMonth() + 1;
    if (m.toString().length == 1) {
        m = '0' + m;
    }
    var d = a.getDate();
    if (d.toString().length == 1) {
        d = '0' + d;
    }
    return a.getFullYear() + "-" + m + "-" + d;
}
//渲染设备状态
function renderMaintenanceStatus(val, metadata, record, rowIndex, columnIndex, store) {
    if (val == -1) {
        return "<span style='color:red'>已过期</span>";
    }
    else if (val == 0)
        return "<span style='color:green'>设备正常</span>";
    else if (val == 1) {
        return "<span style='color:orange'>设备异常</span>";
    }
}
//展示图片
function showImg(val) {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].data["Id"];
        var Title = rows[0].data["Name"];
        var QRCode = rows[0].data["QRCode"];//二维码编码
        var shop = new top.Ext.Window({
            width: 450,
            id: 'shoper',
            height: 350,
            closable: false,
            shadow: false,
            stateful: false,
            border: false,
            modal: true,
            layout: 'fit',
            plain: true,
            autoDestroy: true,
            closeAction: 'close',
            title: Title + '二维码展示',
            items: {
                xtype: 'panel', //或者xtype: 'component',
                border: false,
                items: [{
                    xtype: 'box', //或者xtype: 'component',
                    id: 'PagLogo',
                    style: 'margin-left:100px;margin-top:20px',
                    width: 200, //图片宽度
                    height: 200, //图片高度
                    autoEl: {
                        tag: 'img',    //指定为img标签
                        src: '/Content/Extjs/resources/images/default/s.gif'    //指定url路径
                    }
                },
                 {
                     style: 'margin-left:100px;',
                     xtype: 'tbtext',
                     html: '<br/>二维码编码:' + QRCode
                 }
                ]
               
            },
            buttons: [
                    {
                        text: '取消',
                        iconCls: 'GTP_cancel',
                        tooltip: '取消当前的操作',
                        handler: function () {
                            top.Ext.getCmp("shoper").close();
                        }
                    }]
        }).show();

        top.Ext.getCmp("PagLogo").getEl().dom.src = rows[0].data["Img"];
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}

//点击组织架构
function treeitemclick(node, e) {
    if (node.attributes.Attribute == 1) {
        treeNodeId = node.attributes.id;
    }
    else {
        treeNodeId = "-1";
    }
    reload();
}

/**************************方法************************************/
//创建表单弹框
function CreateFromWindow(title) {
    var form = new top.Ext.FormPanel({
        labelWidth: 65,
        frame: true,
        border: false,
        labelAlign: 'right',
        bodyStyle: 'padding:5px 5px 0',
        id: 'formPanel',
        defaultType: 'textfield',
        items: [
                {
                fieldLabel: '备注信息',
                xtype: 'textarea',
                id: 'Remark',
                name: 'Remark',
                height: 150,
                allowBlank: false,
                anchor: '95%',
                emptyText: '可输入备注的描述信息', ////textfield自己的属性
                maxLength: 200,
                maxLengthText: '备注长度不能超过200个字符'
            }
        ]
    });

    //窗体
    var win = Window("window", title, form);
    win.width = 500;
    win.height = 300;
    return win;
}
//批量主表备注
function GTP_AllEdit() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length >= 1) {
        var ids = [];
        for (var i = 0; i < rows.length; i++) {
            ids.push("'" + rows[i].data["Id"] + "'");
        }
        var window = CreateFromWindow("批量备注");
        var form = top.Ext.getCmp('formPanel');
        form.form.reset();
        url = '/CACAI/Finance/EditRemark?ids=' + ids.join(',');
        window.show();
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}
//批量子表备注
function GTP_AllEditDetail() {
    var grid = Ext.getCmp("rightGrid");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length >= 1) {
        var ids = [];
        for (var i = 0; i < rows.length; i++) {
            ids.push("'" + rows[i].data["Id"] + "'");
        }
        var window = CreateFromWindow("批量备注");
        var form = top.Ext.getCmp('formPanel');
        form.form.reset();
        url = '/CACAI/Finance/EditDetailRemark?ids=' + ids.join(',');
        window.show();
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}
//驳回主表
function BtnBackMain() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length > 0) {
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要驳回该信息吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push("'"+rows[i].data["Id"]+"'");
                }
                top.Ext.MessageBox.wait('', '系统正在执行中,请稍后...'); //显示等待信息框
                Ext.Ajax.request({
                    url: '/CACAI/Finance/BtnBackMain',
                    params: { id: ids.join(",") },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            MessageInfo("驳回成功！", "right");
                            Ext.getCmp("gg").store.reload();
                        } else {
                            MessageInfo("驳回失败！", "error");
                        }
                        top.Ext.MessageBox.hide(); //完成后隐藏消息框   
                    },
                    failure: function (response) {
                        top.Ext.MessageBox.hide(); //完成后隐藏消息框   
                        top.Ext.Msg.show({ title: "系统错误提示", msg: '操作失败，请联系开发人员!', buttons: Ext.Msg.OK, icon: Ext.MessageBox.ERROR });
                    }
                });
            }
        });
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}
//批量驳回主表
function GTP_cancelagent() {
    BtnBackMain();
}
//批量驳回子表
function GTP_Allintransit() {
    BtnBackDetail();
}

//驳回子表
function BtnBackDetail() {
    var grid = Ext.getCmp("rightGrid");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length >=1) {
        //获取是否拆分
        var IsDecompose = rows[0].data["IsDecompose"];
        if (IsDecompose == 1) {
            top.Ext.Msg.show({ title: "信息提示", msg: "当前单据已被拆分，不能进行驳回操作。", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
            return false;
        }
        var OutNumberS = rows[0].data["OutNumberS"];
        if (OutNumberS.split(',').length > 1) {
            top.Ext.Msg.show({ title: "信息提示", msg: "当前单据是拆分子项，不能进行驳回操作。", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
            return false;  
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要驳回该信息吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push("'" + rows[i].data["Id"] + "'");
                }
                top.Ext.MessageBox.wait('', '系统正在执行中,请稍后...'); //显示等待信息框
                Ext.Ajax.request({
                    url: '/CACAI/Finance/BtnBackDetail',
                    params: { id: ids.join(",") },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            MessageInfo("驳回成功！", "right");
                            Ext.getCmp("rightGrid").store.reload();
                        } else {
                            MessageInfo("驳回失败！", "error");
                        }
                        top.Ext.MessageBox.hide(); //完成后隐藏消息框   
                    },
                    failure: function (response) {
                        top.Ext.MessageBox.hide(); //完成后隐藏消息框   
                        top.Ext.Msg.show({ title: "系统错误提示", msg: '操作失败，请联系开发人员!', buttons: Ext.Msg.OK, icon: Ext.MessageBox.ERROR });
                    }
                });
            }
        });
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}
//提交财务
function GTPsubmit() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length > 0) {
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要提交选中的记录信息吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push("'" + rows[i].data["Id"] + "'");
                }
                top.Ext.MessageBox.wait('', '系统正在执行中,请稍后...'); //显示等待信息框 
                Ext.Ajax.request({
                    url: '/CACAI/InComeBill/SubmitData',
                    params: { id: ids.join(",") },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            MessageInfo("提交成功！", "right");
                            Ext.getCmp("gg").store.reload();
                        } else {
                            MessageInfo("提交失败！", "error");
                        }
                        top.Ext.MessageBox.hide(); //完成后隐藏消息框   
                    },
                    failure: function (response) {
                        top.Ext.MessageBox.hide(); //完成后隐藏消息框   
                        top.Ext.Msg.show({ title: "系统错误提示", msg: '操作失败，请联系开发人员!', buttons: Ext.Msg.OK, icon: Ext.MessageBox.ERROR });
                    }
                });
            }
        });
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}

//保存
var SaveDate = function () {
    var grid = Ext.getCmp("gg");
    var formPanel = top.Ext.getCmp("formPanel");
    var win = top.Ext.getCmp("window");
    if (formPanel.getForm().isValid()) {//如果验证通过
        //获取
        var para = {};
        if (win.title == "编辑") {
            //得到选后的数据   
            var rows = grid.getSelectionModel().getSelections();
            para = { Id: rows[0].get("Id") };
        }
        formPanel.getForm().submit({
            waitTitle: '系统提示', //标题
            waitMsg: '正在提交数据,请稍后...', //提示信息
            submitEmptyText: false,
            method: "POST",
            url: url,
            params: para,
            success: function (form, action) {
                //成功后
                var flag = action.result;
                if (flag.success) {
                    MessageInfo("保存成功！", "right");
                    win.close();
                    if (url.indexOf('EditRemark') > -1) {
                        Ext.getCmp("gg").store.reload();
                    }
                    else {
                        Ext.getCmp("rightGrid").getStore().proxy.conn.url = '/CACAI/InComeBill/SearchDataDetail?MainId=' + treeNodeId;
                        Ext.getCmp("rightGrid").getStore().reload();  
                    }
                } else {
                    MessageInfo(flag.msg, "error");
                }
            },
            failure: function (form, action) {
                MessageInfo("保存失败！", "error");
            }
        });
    }
};


//创建查询面板
function CreateSearchWindow(title, key) {

    var form = new top.Ext.form.FormPanel({
        layout: "form", // 整个大的表单是form布局
        id: 'SearchPanel',
        border: false,
        labelWidth: 65,
        autoScroll: true,
        bodyStyle: 'padding:10px',
        labelAlign: "right",
        items: [
             {
                 xtype: "fieldset",
                 title: "主表信息",
                 items: [
                    { fieldLabel: '供应商', xtype: 'textfield', id: 'CusterName', name: 'CusterName', maxLength: 50,
                        maxLengthText: '供应商长度不能超过50个字符', anchor: '95%', enableKeyEvents: true,
                        listeners: {
                            //回车事件
                            specialkey: function (field, e) {
                                if (e.getKey() == Ext.EventObject.ENTER) {
                                    SaveSearch();
                                }
                            }
                        }
                    },
                    { fieldLabel: '采购单号', xtype: 'textfield', id: 'GetNumber', name: 'GetNumber', maxLength: 50,
                        maxLengthText: '采购单号长度不能超过50个字符', anchor: '95%', enableKeyEvents: true,
                        listeners: {
                            //回车事件
                            specialkey: function (field, e) {
                                if (e.getKey() == Ext.EventObject.ENTER) {
                                    SaveSearch();
                                }
                            }
                        }
                    },
                    { fieldLabel: '入库单号', xtype: 'textfield', id: 'InNumber', name: 'InNumber', maxLength: 50,
                        maxLengthText: '入库仓号长度不能超过50个字符', anchor: '95%', enableKeyEvents: true,
                        listeners: {
                            //回车事件
                            specialkey: function (field, e) {
                                if (e.getKey() == Ext.EventObject.ENTER) {
                                    SaveSearch();
                                }
                            }
                        }
                    },
                    { fieldLabel: '主表备注', xtype: 'textfield', id: 'Remark', name: 'Remark', maxLength: 50,
                        maxLengthText: '备注长度不能超过50个字符', anchor: '95%', enableKeyEvents: true,
                        listeners: {
                            //回车事件
                            specialkey: function (field, e) {
                                if (e.getKey() == Ext.EventObject.ENTER) {
                                    SaveSearch();
                                }
                            }
                        }
                    },
                    new top.Ext.form.ComboBox({
                        store: new Ext.data.SimpleStore({
                            fields: ['text', 'value'],
                            data: [['全部', '-1'], ['月结', '1'], ['日结', '2']]
                        }),
                        fieldLabel: '结账方式',
                        displayField: 'text',
                        valueField: 'value',
                        mode: 'local',
                        id: 'CheckoutType',
                        name: 'CheckoutType',
                        selectOnFocus: true,
                        orceSelection: true,
                        editable: false,
                        triggerAction: 'all',
                        value: '-1',
                        anchor: '95%',
                        enableKeyEvents: true,
                        listeners: {
                            //回车事件
                            specialkey: function (field, e) {
                                if (e.getKey() == Ext.EventObject.ENTER) {
                                    SaveSearch();
                                }
                            }
                        }
                    }),
                             new top.Ext.form.ComboBox({
                                 store: new Ext.data.SimpleStore({
                                     fields: ['text', 'value'],
                                     data: [['全部', '-1'], ['支付宝', '1'], ['工行', '2'], ['农行', '3']]
                                 }),
                                 fieldLabel: '付款方式',
                                 displayField: 'text',
                                 valueField: 'value',
                                 mode: 'local',
                                 id: 'PaymentType',
                                 name: 'PaymentType',
                                 selectOnFocus: true,
                                 orceSelection: true,
                                 value: '-1',
                                 editable: false,
                                 triggerAction: 'all',
                                 anchor: '95%',
                                 enableKeyEvents: true,
                                 listeners: {
                                     //回车事件
                                     specialkey: function (field, e) {
                                         if (e.getKey() == Ext.EventObject.ENTER) {
                                             SaveSearch();
                                         }
                                     }
                                 }
                             }),
                          {
                              xtype: 'compositefield',
                              fieldLabel: '单据日期',
                              combineErrors: false,
                              items: [
							    {
							        xtype: 'textfield',
							        hidden: true
							    },
                                {
                                    id: 'BegBillDate',
                                    xtype: 'datefield',
                                    width: 95,
                                    emptyText: '开始时间',
                                    format: 'Y-m-d',
                                    vtype: 'daterange',
                                    endDateField: 'EndBillDate', enableKeyEvents: true,
                                    listeners: {
                                        //回车事件
                                        specialkey: function (field, e) {
                                            if (e.getKey() == Ext.EventObject.ENTER) {
                                                SaveSearch();
                                            }
                                        }
                                    }
                                },
                                {
                                    xtype: 'tbtext',
                                    text: '~'
                                },
                                { xtype: 'datefield', id: 'EndBillDate', emptyText: '', emptyText: '结束日期', format: 'Y-m-d', anchor: '90%', enableKeyEvents: true,
                                    listeners: {
                                        //回车事件
                                        specialkey: function (field, e) {
                                            if (e.getKey() == Ext.EventObject.ENTER) {
                                                SaveSearch();
                                            }
                                        }
                                    }
                                }
					          ]
                          },
                 {
                     xtype: 'compositefield',
                     fieldLabel: '供应商金额',
                     combineErrors: false,
                     items: [
							     {
							         xtype: 'textfield',
							         hidden: true
							     },
                                { xtype: 'numberfield', id: 'BegTotalPrice', name: 'BegTotalPrice', maxLength: 50,
                                    emptyText: '起始金额', width: 95, maxValue: 999999999, enableKeyEvents: true,
                                    listeners: {
                                        //回车事件
                                        specialkey: function (field, e) {
                                            if (e.getKey() == Ext.EventObject.ENTER) {
                                                SaveSearch();
                                            }
                                        }
                                    }
                                },
                                {
                                    xtype: 'tbtext',
                                    text: '~'
                                },
                                { xtype: 'numberfield', id: 'EndTotalPrice', name: 'EndTotalPrice', maxLength: 50,
                                    emptyText: '结束金额', width: 100, maxValue: 999999999, enableKeyEvents: true,
                                    listeners: {
                                        //回车事件
                                        specialkey: function (field, e) {
                                            if (e.getKey() == Ext.EventObject.ENTER) {
                                                SaveSearch();
                                            }
                                        }
                                    }
                                }
					          ]
                 },
                 {
                     xtype: 'compositefield',
                     fieldLabel: '盈亏金额',
                     combineErrors: false,
                     items: [
							     {
							         xtype: 'textfield',
							         hidden: true
							     },
                                { xtype: 'numberfield', id: 'BegLossPrice', name: 'BegLossPrice', maxLength: 50,
                                    emptyText: '起始金额', width: 95, maxValue: 999999999, enableKeyEvents: true,
                                    listeners: {
                                        //回车事件
                                        specialkey: function (field, e) {
                                            if (e.getKey() == Ext.EventObject.ENTER) {
                                                SaveSearch();
                                            }
                                        }
                                    }
                                },
                                {
                                    xtype: 'tbtext',
                                    text: '~'
                                },
                                { xtype: 'numberfield', id: 'EndLossPrice', name: 'EndLossPrice', maxLength: 50,
                                    emptyText: '结束金额', width: 100, maxValue: 999999999, enableKeyEvents: true,
                                    listeners: {
                                        //回车事件
                                        specialkey: function (field, e) {
                                            if (e.getKey() == Ext.EventObject.ENTER) {
                                                SaveSearch();
                                            }
                                        }
                                    }
                                }

					          ]
                 },
                  {
                      xtype: 'compositefield',
                      fieldLabel: '提交时间',
                      combineErrors: false,
                      items: [
							    {
							        xtype: 'textfield',
							        hidden: true
							    },
                                {
                                    id: 'BegCreateTime',
                                    xtype: 'datefield',
                                    width: 95,
                                    emptyText: '开始时间',
                                    format: 'Y-m-d',
                                    vtype: 'daterange',
                                    endDateField: 'EndCreateTime', enableKeyEvents: true,
                                    listeners: {
                                        //回车事件
                                        specialkey: function (field, e) {
                                            if (e.getKey() == Ext.EventObject.ENTER) {
                                                SaveSearch();
                                            }
                                        }
                                    }
                                },
                                {
                                    xtype: 'tbtext',
                                    text: '~'
                                },
                                { xtype: 'datefield', id: 'EndCreateTime', emptyText: '', emptyText: '结束日期', format: 'Y-m-d', anchor: '90%', enableKeyEvents: true,
                                    listeners: {
                                        //回车事件
                                        specialkey: function (field, e) {
                                            if (e.getKey() == Ext.EventObject.ENTER) {
                                                SaveSearch();
                                            }
                                        }
                                    }
                                }
					          ]
                  },
                   new top.Ext.form.ComboBox({
                       store: new Ext.data.SimpleStore({
                           fields: ['text', 'value'],
                           data: [['全部', '-1'], ['在途', '1'], ['未在途', '2']]
                       }),
                       fieldLabel: '退货在途',
                       displayField: 'text',
                       valueField: 'value',
                       mode: 'local',
                       id: 'IsOnLine',
                       name: 'IsOnLine',
                       selectOnFocus: true,
                       orceSelection: true,
                       value: '-1',
                       editable: false,
                       triggerAction: 'all',
                       anchor: '95%',
                       enableKeyEvents: true,
                       listeners: {
                           //回车事件
                           specialkey: function (field, e) {
                               if (e.getKey() == Ext.EventObject.ENTER) {
                                   SaveSearch();
                               }
                           },
                           select: function (mysel) {
                               var itemstr = mysel.lastSelectionText;
                               if (itemstr.trim() == "在途") {
                                   top.Ext.getCmp("OnlineTime").show();
                               }
                               else {
                                   top.Ext.getCmp("OnlineTime").hide();
                                   top.Ext.getCmp("BegOnLine").setValue('');
                                   top.Ext.getCmp("EndOnLine").setValue('');
                               }
                           }
                       }
                   }),
                    {
                        xtype: 'compositefield',
                        fieldLabel: '在途时间',
                        hidden: true,
                        id: 'OnlineTime',
                        combineErrors: false,
                        items: [
							    {
							        xtype: 'textfield',
							        hidden: true
							    },
                                {
                                    id: 'BegOnLine',
                                    xtype: 'datefield',
                                    width: 95,
                                    emptyText: '开始时间',
                                    format: 'Y-m-d',
                                    vtype: 'daterange',
                                    endDateField: 'EndOnLine', enableKeyEvents: true,
                                    listeners: {
                                        //回车事件
                                        specialkey: function (field, e) {
                                            if (e.getKey() == Ext.EventObject.ENTER) {
                                                SaveSearch();
                                            }
                                        }
                                    }
                                },
                                {
                                    xtype: 'tbtext',
                                    text: '~'
                                },
                                { xtype: 'datefield', id: 'EndOnLine', emptyText: '', emptyText: '结束日期', format: 'Y-m-d', anchor: '90%', enableKeyEvents: true,
                                    listeners: {
                                        //回车事件
                                        specialkey: function (field, e) {
                                            if (e.getKey() == Ext.EventObject.ENTER) {
                                                SaveSearch();
                                            }
                                        }
                                    }
                                }
					          ]
                    }
               ]
             }
          ]
    });

    return form;
}

//更多查询面板
function BtnMore() {
    if (top.Ext.getCmp("SearchMore")) {
        top.Ext.getCmp("SearchMore").close();
    }
    var form = CreateSearchWindow("组合查询", "");
    var shop = new top.Ext.Window({
        width: 340,
        id: 'SearchMore',
        constrainHeader: true, //视图区域中显示
        height: 600,
        closable: true,
        //shadow: false,
        //stateful: false,
        border: false,
        // modal: true,
        layout: 'fit',
        plain: true,
        autoDestroy: true,
        closeAction: 'close',
        title: '组合查询面板',
        items: form,
        buttons: [
                    {
                        text: '查询',
                        iconCls: 'GTP_query',
                        handler: function () {
                            try {
                                SaveSearch();   
                            }
                            catch (e) {
                                Ext.MessageBox.alert("提示", "操作错误，请手动关闭窗体，右上角点击退出重新打开！");
                            }
                        }
                    },
                    {
                        text: '重置',
                        iconCls: 'GTP_eraser',
                        handler: function () {
                            try {
                                top.Ext.getCmp("SearchPanel").getForm().reset();
                                SaveSearch();
                            } catch (e) {
                                Ext.MessageBox.alert("提示", "操作错误，请手动关闭窗体，右上角点击退出重新打开！");
                            }
                        }
                    },
                    {
                        text: '取消',
                        iconCls: 'GTP_cancel',
                        handler: function () {
                            try {
                                top.Ext.getCmp("SearchMore").close();
                            } catch (e) {
                                Ext.MessageBox.alert("提示", "操作错误，请手动关闭窗体，右上角点击退出重新打开！");
                            }
                        }
                    }]
    })
    shop.show();
    //光标定位
    top.Ext.getCmp('CusterName').focus(false, 200); //11为对应字段的id
}

var param = {};

//获取页面所有查询条件
function GetSearch() {
    var param = GetParam();
    //高级查询
    var comName = Ext.getCmp("comName").getValue(); //字段名称
    var comSign = Ext.getCmp("comSign").getValue(); //查询方式
    var comContent = Ext.getCmp("comContent").getValue(); //查询值
    var comTimeContent = Ext.getCmp("comTimeContent").value; //查询时间值
    if (!Ext.getCmp("comTimeContent").hidden) {//时间控件
        comContent = comTimeContent;
        //时间列需要转换
        comName = "Convert(varchar(20)," + comName + ",120)";
    }
    else if (!Ext.getCmp("comEnum").hidden)//枚举控件
    {
        comContent = Ext.getCmp("comEnum").getValue();
    }
    else if (Ext.getCmp("ConSysDir") != null)//字典类型
    {
        if (!Ext.getCmp("ConSysDir").hidden) {
            comContent = Ext.getCmp("ConSysDir").getValue();
        }
    }
    param.conditionField = comName;
    param.condition = comSign;
    param.conditionValue = comContent;

    return urlEncode(param, "", true);
}

//获取组合查询面板数据
function GetParam() {
    if (top.Ext.getCmp("SearchPanel")) {
        var IsOnLine = top.Ext.getCmp("IsOnLine").getValue(); //显示在途       
        param = {
            InNumber: top.Ext.getCmp("InNumber").getValue().trim(), //入库单号
            GetNumber: top.Ext.getCmp("GetNumber").getValue().trim(), //采购单号
            CusterName: top.Ext.getCmp("CusterName").getValue().trim(), //供应商
            Remark: top.Ext.getCmp("Remark").getValue().trim(), //备注
            BegBillDate: top.Ext.getCmp("BegBillDate").value, //单据日期
            EndBillDate: top.Ext.getCmp("EndBillDate").value, //
            BegCreateTime: top.Ext.getCmp("BegCreateTime").value, //单据日期
            EndCreateTime: top.Ext.getCmp("EndCreateTime").value, //
            BegTotalPrice: top.Ext.getCmp("BegTotalPrice").getValue(), //供应商金额
            EndTotalPrice: top.Ext.getCmp("EndTotalPrice").getValue(), //
            BegLossPrice: top.Ext.getCmp("BegLossPrice").getValue(), //盈亏金额
            EndLossPrice: top.Ext.getCmp("EndLossPrice").getValue(), //
            PaymentType: top.Ext.getCmp("PaymentType").getValue(), //付款方式
            CheckoutType: top.Ext.getCmp("CheckoutType").getValue(), //结账方式
            IsOnLine: IsOnLine,
            BegOnLine: top.Ext.getCmp("BegOnLine").value, //在途日期
            EndOnLine: top.Ext.getCmp("EndOnLine").value //
        };
    } else {
        param = {
            InNumber: '', //入库单号
            GetNumber: '', //采购单号
            CusterName: '', //供应商
            Remark: '', //备注
            DetailRemark: '',
            BegBillDate: '', //单据日期
            EndBillDate: '', //
            BegCreateTime:'', //单据日期
            EndCreateTime:'', //
            BegTotalPrice: '', //供应商金额
            EndTotalPrice: '', //
            BegLossPrice: '', //盈亏金额
            EndLossPrice: '', //
            PaymentType: -1,
            CheckoutType: -1,
            IsOnLine:-1,
            BegOnLine:'', //在途日期
            EndOnLine:'' //
        };
    }
    return param;
}
//查询
var SaveSearch = function () {
    var grid = Ext.getCmp("gg");
    var formPanel = top.Ext.getCmp("SearchPanel");
    if (formPanel.getForm().isValid()) {//如果验证通过
        var _store = Ext.getCmp("gg").store;
        Ext.apply(_store.baseParams, GetParam());
        _store.reload({ params: { start: 0, limit: parseInt(Ext.getCmp("pagesize").getValue())} });
    }
    else {
        Ext.MessageBox.alert("提示", "请按提示填写数据！");
        return "";
    }
}



















