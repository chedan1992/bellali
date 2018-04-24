//批量导出
function BatchExport() {
    //判断是否超级管理员添加
    var creator = getLoginUser(); //获取用户信息
    var CreateCompanyId = '';
    if (creator.Attribute == 1)//系统管理员
    {
        var grid = Ext.getCmp("tr");
        var rows = grid.getSelectionModel().getSelectedNode();
        CreateCompanyId = rows.attributes.id;
    }
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length > 0) {
        var confirm = top.Ext.MessageBox.confirm('系统确认', '您确定要导出当前选中的信息吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push("'" + rows[i].data["Id"] + "'");
                }
                var url = '/SysAppointed/ImportOut?Cid=' + CreateCompanyId + '&IdList=' + ids.join(",") + '&date=' + new Date();
                $(".hideform").attr("action", url);
                $(".hideform").submit();
            }
        });
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}


var rightClick = new Ext.menu.Menu({
    id: 'rightClickCont',
    items: [
        {
            id: 'rMenu1',
            handler: DeleteDate,
            iconCls: 'GTP_delete',
            text: '删除'
        },
        {
            id: 'rMenu2',
            handler: DeleteAll,
            text: '清空全部',
            iconCls: 'GTP_clear'
        }, '-',
        {
            id: 'rMenu3',
            handler: CopyAll,
            text: '复制添加',
            iconCls: 'GTP_copy'
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
    var CreateCompanyId = '0';
    var grid = Ext.getCmp("tr");
    if (grid) {
        var rows = grid.getSelectionModel().getSelectedNode();
        if (rows) {
            if (rows.attributes.Attribute == 1) {
                CreateCompanyId = rows.attributes.id;
            }
            else {
                MessageInfo("请先选择单位！", "statusing");
                return false;
            }
        }
        else {
            MessageInfo("请先选择单位！", "statusing");
            return false;
        }
    }
    var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要清空列表所有设备吗?', function (e) {
        if (e == "yes") {
            Ext.Ajax.request({
                url: '/SysAppointed/DeleteAll?CreateCompanyId=' + CreateCompanyId,
                success: function (response) {
                    var rs = eval('(' + response.responseText + ')');
                    if (rs.success) {
                        MessageInfo("清空成功！", "right");
                        Ext.getCmp("gg").store.reload();
                    } else {
                        MessageInfo("清空失败！", "error");
                    }
                }
            });
        }
    });
}
//复制添加
function CopyAll() {
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要复制选中的设备基本信息吗?', function (e) {
            if (e == "yes") {
                var key = rows[0].get("Id");
                Ext.Ajax.request({
                    url: '/SysAppointed/CopyAll',
                    params: { id: key },
                    success: function (response) {
                        Ext.getCmp("gg").store.reload();
                    }
                });
            }
        });
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
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
        var QRCode = rows[0].data["QRCode"]; //二维码编码
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
                style: 'margin-left:100px;margin-top:20px',
                border:false,
                items: [{
                    xtype: 'box', //或者xtype: 'component',
                    id: 'PagLogo',
                    width: 200, //图片宽度
                    height: 200, //图片高度
                    autoEl: {
                        tag: 'img',    //指定为img标签
                        src: '/Content/Extjs/resources/images/default/s.gif'    //指定url路径
                    }
                }, {
                    style: 'margin-left:100px;',
                    xtype: 'tbtext',
                    html: '<br/>二维码编码:' + QRCode
                }]
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
    Ext.getCmp("gg").getStore().reload();
}

//导入
function ImportDate1() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].data["Id"];
        var Title = rows[0].data["Name"];
        var shop = new top.Ext.Window({
            width: 680,
            id: 'shoper',
            height:510,
            closable: false,
            shadow: false,
            stateful: false,
            border: false,
            modal: true,
            layout: 'fit',
            plain: true,
            autoDestroy: true,
            closeAction: 'close',
            title: Title + '巡检记录',
            items: {
                autoScroll: true,
                border: false,
                params: { CEId: key },
                autoLoad: { url: '../../Project/Html/selectMasterTree.htm', scripts: true, nocache: true }
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
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}
//导入
function ImportDate() {
    //判断是否超级管理员添加
    var creator = getLoginUser(); //获取用户信息
    var CreateCompanyId = '';
    if (creator.Attribute == 1)//系统管理员
    {
        var grid = Ext.getCmp("tr");
        var rows = grid.getSelectionModel().getSelectedNode();
        var CreateCompanyId = '0';
        if (rows) {
            if (rows.attributes.Attribute == 1) {
                CreateCompanyId = rows.attributes.id;
            }
            else {
                MessageInfo("请先选择单位！", "statusing");
                return false;
            }
        }
        else {
            MessageInfo("请先选择单位！", "statusing");
            return false;
        }
        var grid = Ext.getCmp("tr");
        var rows = grid.getSelectionModel().getSelectedNode();
        CreateCompanyId = rows.attributes.id;
    }

    //下载地址
    var downMoBanUlr = "/Project/Template/设备模版.xls";


    var panel = new top.Ext.Panel({
        frame: true,
        layout: "card",
        activeItem: 0,
        defaults: {
            bodyStyle: "padding:3px; background-color: #FFFFFF"
        },
        items: [
            {
                id: "c1",
                xtype: 'panel',
                items: [
                new top.Ext.FormPanel({
                    xtype: 'panel',
                    layout: 'form',
                    labelAlign: 'right',
                    fileUpload: true, //有需文件上传的,需填写该属性
                    id: 'formPanel1',
                    labelWidth: 75,
                    bodyStyle: 'padding:15px;',
                    border: false,
                    defaultType: 'textfield',
                    items: [
                                {
                                    id: 'moban',
                                    name: 'moban',
                                    bodyStyle: 'border:0px;',
                                    fieldLabel: '导入模板',
                                    style: 'padding-top:3px;',
                                    xtype: 'panel',
                                    width: 300,
                                    anchor: '90%',
                                    html: '<a target=blank href="' + downMoBanUlr + '">模板下载</a>'
                                },
                                {
                                    xtype: 'tbtext',
                                    text: '<br/>'
                                },
                                {
                                    xtype: 'compositefield',
                                    fieldLabel: '附件文件',
                                    id: 'compositefieldPic',
                                    combineErrors: false,
                                    anchor: '85%',
                                    items: [

                                                    {
                                                        xtype: 'fileuploadfield',
                                                        id: 'excel',
                                                        emptyText: '请上传Excel格式文件',
                                                        name: 'excel',
                                                        buttonText: '',
                                                        allowBlank: false,
                                                        buttonCfg: {
                                                            iconCls: 'image_add',
                                                            tooltip: '附件选择'
                                                        },
                                                        width: 400,
                                                        listeners: {
                                                            'fileselected': {
                                                                fn: top.ExcelUploadAction,
                                                                scope: this
                                                            }
                                                        }
                                                    }
						                        ]
                                },
                                {
                                    xtype: 'tbtext',
                                    text: '<br/>'
                                },
                                {
                                    fieldLabel: '提示说明',
                                    xtype: 'panel',
                                    border: false,
                                    width: 300,
                                    style: 'padding-top:4px;color:#acacac',
                                    anchor: '90%',
                                    html:'1  设备编号：必填<br/>'
                                       + '2  设备名称：必填<br/>'
                                       + '3  设备位置：必填<br/>'
                                       + '4  设备型号：必填<br/>'
                                       + '5  设备规格：必填<br/>'
                                       + '6  设备数量：选填(数量，为空默认为0)<br/>'
                                       + '7  生产日期：必填(日期类型)<br/>'
                                       + '8  责任部门：必填<br/>'
                                       + '9    责任人：必填<br/>'
                                       + '10 联系电话：必填(手机号码)<br/>'
                                       + '11 设备分类：必填<br/>'
                                }

                            ]
                })
                ]
            },
            { id: "c2",
              xtype: 'panel',
              items: [
               new top.Ext.FormPanel({
                     xtype: 'panel',
                     layout: 'form',
                     id: 'formPanel4',
                     labelAlign: 'right',
                     labelWidth: 75,
                     bodyStyle: 'padding:15px;',
                     border: false,
                     defaultType: 'textfield',
                     items: [
                         {
                             name: 'downerror',
                             style: 'color:Red',
                             bodyStyle: 'border:0px;',
                             id: 'downerror',
                             fieldLabel: '信息提示',
                             style: 'padding-left:5px;padding-top:3px;',
                             xtype: 'panel',
                             width: 300,
                             anchor: '90%',
                             html: ''
                         }
                     ]
            })
            ]
            }
        ],
        buttons: [
        {
            text: "上一步",
            handler: changePage
        },
        {
            text: "下一步",
            handler: changePage
        },
         {
             text: "取消",
             handler: function () {
                 top.Ext.getCmp("Export").close();
             }
         }
    ]
    });

    var shop = new top.Ext.Window({
        width: 620,
        id: 'Export',
        height: 428,
        closable: false,
        shadow: false,
        stateful: false,
        border: false,
        modal: true,
        layout: 'fit',
        plain: true,
        autoDestroy: true,
        closeAction: 'close',
        title: '批量导入 1:文件上传',
        items: [panel]
    }).show();

    function changePage(btn) {
        var index = Number(panel.layout.activeItem.id.substring(1));
        if (btn.text == "上一步") {
            index -= 1;
            if (index == 1) {
                top.Ext.getCmp("Export").setTitle("批量导入 1:文件上传");
            }
            if (index == 2) {
                top.Ext.getCmp("Export").setTitle("批量导入 2:开始导入");
            }
            if (index < 1) index = 1;
        }
         else {
             index += 1;
            if (index == 1) {
                var formPanel = top.Ext.getCmp("formPanel1");
                 if (formPanel.getForm().isValid()) {//如果验证通过
                     top.Ext.getCmp("Export").setTitle("批量导入 1:文件上传");
                 }
                 else {
                     return false;
                 }
            }
             if (index == 2) {
                var formPanel = top.Ext.getCmp("formPanel1");
                if (formPanel.getForm().isValid()) {//如果验证通过
                    //判断是否超级管理员添加
                    var creator = getLoginUser(); //获取用户信息
                    var CreateCompanyId = '';
                    if (creator.Attribute == 1)//系统管理员
                    {
                        var grid = Ext.getCmp("tr");
                        var rows = grid.getSelectionModel().getSelectedNode();
                        CreateCompanyId = rows.attributes.id;
                    }
                    var para = {CompanyId: CreateCompanyId };
                    formPanel.getForm().submit({
                        waitTitle: '系统提示', //标题
                        waitMsg: '正在导入,请稍后...', //提示信息
                        submitEmptyText: false,
                        method: "POST",
                        url: '/SysAppointed/ImportDate',
                        params: para,
                        success: function (form, action) {
                            var flag = action.result; //成功后
                            var a = '';
                            if (flag.success) {
                                MessageInfo("导入成功！", "right");
                                a += '(' + flag.msg + ')<br/>';
                                if (flag.data.length > 0) {
                                    a += '<a style="color:Red" target=blank href="' + flag.data + '">下载错误数据</a><br/>';
                                }
                                Ext.getCmp("gg").getStore().reload();
                            } else {
                                var a = '<span style="color:Red">导入失败,文件有误.</span>';
                                MessageInfo("导入失败,文件有误", "error");
                            }
                            top.Ext.getCmp('downerror').show();
                            top.Ext.getCmp("downerror").update(a);
                            //导入数据
                            top.Ext.getCmp("Export").setTitle("批量导入 2:开始导入");
                        },
                        failure: function (form, action) {
                            MessageInfo("保存失败！", "error");
                        }
                    });
                }
                else {
                    return false;
                }
            }
            if (index > 3) index =3;
        }
        panel.layout.setActiveItem("c" + index);
    }
}


//加载信息表单
function load(title, key, CreateCompanyId) {
    /*
    设备类型
    */
    var AppoinedType = new Ext.data.Store({
        proxy: new Ext.data.HttpProxy({
            url: '/SysGroup/GetSysGroup?CompanyId=' + CreateCompanyId,
            method: 'POST'
        }),
        reader: new Ext.data.JsonReader({},
            Ext.data.Record.create(["Id", "Name"]
        ))
    });
    /*
    责任人
    */
    var comBrand = new Ext.data.Store({
        proxy: new Ext.data.HttpProxy({
            url: '/UserStaff/GetSysMaster?CompanyId=' + CreateCompanyId,
            method: 'POST'
        }),
        reader: new Ext.data.JsonReader({},
            Ext.data.Record.create(["Id", "UserName"]
        ))
    });
    if (title == "编辑") {
        var grid = Ext.getCmp("gg");
        //得到选后的数据   
        var rows = grid.getSelectionModel().getSelections();
        key = rows[0].get("Id");

        //设备类型
        if (rows[0].get("CityId") != "") {
            AppoinedType.proxy = new Ext.data.HttpProxy({ url: '/SysGroup/GetSysGroup?CompanyId=' + rows[0].get("Cid"), method: 'POST' });
            AppoinedType.load();
        }
        //责任人
        if (rows[0].get("AreaId") != "") {
            comBrand.proxy = new Ext.data.HttpProxy({ url: '/UserStaff/GetSysMaster?CompanyId=' + rows[0].get('Cid'), method: 'POST' });
            comBrand.load();
        }
    }
    var form = new top.Ext.FormPanel({
        id: "formPanel",
        labelAlign: 'right',
        frame: true,
        bodyStyle: 'padding:5px 5px 0',
        width: 650,
        labelWidth: 80,
        fileUpload: true,
        autoScroll: true,
        border: false,
        items: [
            {
                xtype: "fieldset",
                autoHeight: true,
                title: "基本信息",
                layout: 'column',
                items: [
                    {
                        columnWidth: .5,
                        layout: 'form',
                        items: [
                        {
                            name: 'Name',
                            fieldLabel: '<span class="required">*</span>设备名称',
                            xtype: 'textfield',
                            id: 'Name',
                            maxLength: 50,
                            allowBlank: false,
                            emptyText: '填写设备名称',
                            maxLengthText: '设备名称长度不能超过50个字符',
                            anchor: '90%',
                            enableKeyEvents: true
                        },
                         {
                             name: 'Places',
                             fieldLabel: '<span class="required">*</span>设备位置',
                             xtype: 'textfield',
                             id: 'Places',
                             maxLength: 50,
                             allowBlank: false,
                             maxLengthText: '位置长度不能超过50个字符',
                             anchor: '90%',
                             enableKeyEvents: true
                         },
                        {
                            name: 'Model',
                            fieldLabel: '<span class="required">*</span>设备型号',
                            xtype: 'textfield',
                            id: 'Model',
                            flex: 2,
                            maxLength: 50,
                            maxLengthText: '型号长度不能超过300个字符',
                            anchor: '90%'
                        },
                        {
                            fieldLabel: '生产日期',
                            xtype: 'datefield',
                            allowBlank: false,
                            value: new Date((new Date() / 1000 + 86400 * 1) * 1000),
                            width: 130,
                            id: 'ProductionDate',
                            name: 'ProductionDate',
                            emptyText: '选择开始时间',
                            format: 'Y-m-d(周l)',
                            anchor: '90%'
                        },
                         {
                             xtype: 'compositefield',
                             fieldLabel: '二维码',
                             combineErrors: false,
                             anchor: '90%',
                             items: [
                                    {
                                        xtype: 'textfield',
                                        height: 22, //
                                        id: "QRCode",
                                        name: "QRCode",
                                        hidden:true,
                                        emptyText: 'Y',
                                        readOnly: true
                                    },
                                    {
                                        xtype: 'textfield',
                                        height: 22, //
                                        id: "QrName",
                                        name: "QrName",
                                       width:155,
                                       emptyText: '若设备已有二维码,请关联',
                                       readOnly: true
                                    },
                                    new top.Ext.Button({
                                        text: '选择',
                                        width: 45,
                                        handler: function () {
                                            selectQrCode();
                                        }
                                    })
                                 ]
                         }
                        ]
                    },
                    {
                        columnWidth: .5,
                        layout: 'form',
                        items: [
                                {
                                    xtype: 'combo',
                                    triggerAction: 'all',
                                    id: 'comGid',
                                    name: 'comGid',
                                    fieldLabel: '<span class="required">*</span>设备类型',
                                    emptyText: '设备类型',
                                    forceSelection: true,
                                    editable: true,
                                    typeAhead: true, //模糊查询
                                    allowBlank: false,
                                    displayField: 'Name',
                                    valueField: 'Id',
                                    hiddenName: 'Name',
                                    anchor: '90%',
                                    store: AppoinedType,
                                    listeners: {
                                        beforequery: function (e) {
                                            var combo = e.combo;
                                            if (!e.forceAll) {
                                                var input = e.query;
                                                // 检索的正则  
                                                var regExp = new RegExp(".*" + input + ".*");
                                                // 执行检索  
                                                combo.store.filterBy(function (record, id) {
                                                    // 得到每个record的项目名称值  
                                                    var text = record.get(combo.displayField);
                                                    return regExp.test(text);
                                                });
                                                combo.expand();
                                                return false;
                                            }
                                        }
                                    }
                                }, {
                                    xtype: 'combo',
                                    triggerAction: 'all',
                                    fieldLabel: '<span class="required">*</span>责任人',
                                    emptyText: '责任人',
                                    forceSelection: true,
                                    editable: true,
                                    id: 'comResponsibleId',
                                    name: 'comResponsibleId',
                                    typeAhead: true, //模糊查询
                                    allowBlank: false,
                                    displayField: 'UserName',
                                    valueField: 'Id',
                                    hiddenName: 'UserName',
                                    anchor: '90%',
                                    store: comBrand,
                                    listeners: {
                                        beforequery: function (e) {
                                            var combo = e.combo;
                                            if (!e.forceAll) {
                                                var input = e.query;
                                                // 检索的正则  
                                                var regExp = new RegExp(".*" + input + ".*");
                                                // 执行检索  
                                                combo.store.filterBy(function (record, id) {
                                                    // 得到每个record的项目名称值  
                                                    var text = record.get(combo.displayField);
                                                    return regExp.test(text);
                                                });
                                                combo.expand();
                                                return false;
                                            }
                                        }
                                    }
                                }, {
                                    name: 'Specifications',
                                    xtype: 'textfield',
                                    id: 'Specifications',
                                    fieldLabel: '<span class="required">*</span>设备规格',
                                    allowBlank: false,
                                    anchor: '90%',
                                    maxLength: 50,
                                    maxLengthText: '规格长度不能超过100个字符'
                                },
                                {
                                    fieldLabel: '过期日期',
                                    xtype: 'datefield',
                                    id: 'LostTime',
                                    hidden: true,
                                    value: new Date((new Date() / 1000 + 86400 * 1) * 1000),
                                    width: 130,
                                    emptyText: '选择开始时间',
                                    format: 'Y-m-d(周l)',
                                    anchor: '90%'
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'StoreNum',
                                    id: 'StoreNum',
                                    fieldLabel: '数量',
                                    allowBlank: false,
                                    value: 0,
                                    minValue: 0,
                                    maxValue: 9999,
                                    anchor: '90%'
                                },
                                   {
                                       xtype: 'combo',
                                       triggerAction: 'all',
                                       emptyText: '过期年限',
                                       forceSelection: true,
                                       displayField: 'text',
                                       fieldLabel: '过期年限',
                                       valueField: 'value',
                                       id: 'comLostTime',
                                       mode: 'local',
                                       editable: false,
                                       anchor: '90%',
                                       store: new Ext.data.SimpleStore({
                                           fields: ['text', 'value'],
                                           data: [['1', '1'], ['2', '2'], ['3', '3'], ['4', '4'], ['5', '5'], ['6', '6'], ['7', '7'], ['8', '8'], ['9', '9'], ['10', '10']]
                                       }),
                                       value: '5'
                                   }
                           ]
                    },
                    {
                        columnWidth: 1,
                        layout: 'form',
                        items: [
                         {
                             fieldLabel: '设备介绍',
                             xtype: 'htmleditor',
                             id: 'Mark',
                             name: 'Mark',
                             height: 150,
                             emptyText: '可输入对设备的介绍信息', ////textfield自己的属性
                             anchor: '96%'
                         }
                        ]
                    }
                ]
            }
        //              {
        //                  xtype: "fieldset",
        //                  autoHeight: true,
        //                  title: "二维码",
        //                  id: 'ImgShowDiv',
        //                  hidden:true,
        //                  layout: 'column',
        //                  items: [
        //                  {
        //                      xtype: 'box', //或者xtype: 'component',
        //                      width: 100, //图片宽度
        //                      id:'ImgShow',
        //                      height: 100, //图片高度
        //                      autoEl: {
        //                          tag: 'img',    //指定为img标签
        //                          src: '/Content/Extjs/resources/images/default/s.gif'    //指定url路径
        //                      }
        //                  }
        //                ]
        //              }
        ]
    });

    //窗体
    var win = Window("window", title, form);
    win.width = 700;
    win.height =440;
    return win;
}
/**************************方法************************************/
//启用
function EnableUse() {
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        if (rows[0].get("Status") == "1") {
            MessageInfo("该设备已经启用！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要启用该设备吗?', function (e) {
            if (e == "yes") {
                var key = rows[0].get("Id");
                Ext.Ajax.request({
                    url: '/SysAppointed/EnableUse',
                    params: { id: key },
                    success: function (response) {
                        Ext.getCmp("gg").store.reload();
                    }
                });
            }
        });
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}
//禁用
function DisableUse() {
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        if (rows[0].get("Status") == "0") {
            MessageInfo("该设备已经禁用！", "statusing");
            return;
        }
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要禁用该设备吗?', function (e) {
            if (e == "yes") {
                var key = rows[0].get("Id");
                Ext.Ajax.request({
                    url: '/SysAppointed/DisableUse',
                    params: { id: key },
                    success: function (response) {
                        Ext.getCmp("gg").store.reload();
                    }
                });
            }
        });
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}
//新增
function AddDate() {
    var CreateCompanyId = '0';
    var grid = Ext.getCmp("tr");
    if (grid) {
        var rows = grid.getSelectionModel().getSelectedNode();
        if (rows) {
            if (rows.attributes.Attribute == 1) {
                CreateCompanyId = rows.attributes.id;
            }
            else {
                MessageInfo("请先选择单位！", "statusing");
                return false;
            }
        }
        else {
            MessageInfo("请先选择单位！", "statusing");
            return false;
        }
    }
    result = [];
    parent.idArray = [];
    url = '/SysAppointed/SaveData?CreateCompanyId=' + CreateCompanyId;
    var win = load("新增", "", CreateCompanyId);
    win.show();
}
//编辑
function EditDate() {
    result = [];
    parent.idArray = [];
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();

    if (rows.length == 1) {
        var key = rows[0].get("Id");
        var win = load("编辑", key,"");
        var form = top.Ext.getCmp('formPanel');
        win.show();

        var respon = Ext.lib.Ajax.getConnectionObject().conn;
        respon.open("post", "/SysAppointed/GetData?id=" + key, false);
        respon.send(null);
        var response = Ext.util.JSON.decode(respon.responseText);
        form.form.loadRecord(response);
        //设备类型
        top.Ext.getCmp("comGid").setValue(response.data.Gid); //value
        top.Ext.getCmp("comGid").setRawValue(response.data.GroupName); //text
        //责任人
        top.Ext.getCmp("comResponsibleId").setValue(response.data.ResponsibleId); //value
        top.Ext.getCmp("comResponsibleId").setRawValue(response.data.Responsible); //text

        //格式化时间
        var ProductionDate = new Date(formartTime(rows[0].data.ProductionDate).format('Y-m-d'));
        if (ProductionDate == "NaN") {
            ProductionDate = formartTime(rows[0].data.ProductionDate).format('Y-m-d');
        }
//        top.Ext.getCmp("ImgShow").getEl().dom.src = rows[0].data.Img;
//        top.Ext.getCmp("ImgShowDiv").show();

        top.Ext.getCmp("ProductionDate").setValue(ProductionDate);
        win.show();
        url = '/SysAppointed/SaveData?Id=' + key + '&modify=1';
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}
//删除
function DeleteDate() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要删除设备吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].data["Id"]);
                }
                Ext.Ajax.request({
                    url: '/SysAppointed/DeleteData',
                    params: { id: ids.join(",") },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            MessageInfo("删除成功！", "right");
                            Ext.getCmp("gg").store.reload();
                        } else {
                            MessageInfo("删除失败！", "error");
                        }
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

        //获取设备类型和责任人
        var comGid = top.Ext.getCmp("comGid").getValue();
        var comResponsibleId = top.Ext.getCmp("comResponsibleId").getValue();
        var ProductionDate = top.Ext.getCmp("ProductionDate").getValue().format('Y-m-d'); //生产日期
        //过期年限
        var LostTime = top.Ext.getCmp("comLostTime").getValue();
        //获取
        var para = { Gid: comGid, ResponsibleId: comResponsibleId, Time: ProductionDate, LostYear: LostTime };
        if (win.title == "编辑") {
            //得到选后的数据   
            var rows = grid.getSelectionModel().getSelections();
            para = { Id: rows[0].get("Id"), Gid: comGid, ResponsibleId: comResponsibleId, Time: ProductionDate, LostYear: LostTime };
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
                    Ext.getCmp("gg").store.reload();
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
//查看设备维护情况
function clickDetail() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var key = rows[0].data["Id"];
        var Title = rows[0].data["Name"];
        var shop = new top.Ext.Window({
            width: 680,
            id: 'shoper',
            height: 498,
            closable: false,
            shadow: false,
            stateful: false,
            border: false,
            modal: true,
            layout: 'fit',
            plain: true,
            autoDestroy: true,
            closeAction: 'close',
            title: Title + '巡检记录',
            items: {
                autoScroll: true,
                border: false,
                params: { CEId: key },
                autoLoad: { url: '../../Project/Html/RepairRecordGrid.htm', scripts: true, nocache: true }
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
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}

//选择二维码列表
function selectQrCode() {
    //判断是否超级管理员添加
    var creator = getLoginUser(); //获取用户信息
    var CreateCompanyId = '';
    if (creator.Attribute == 1)//系统管理员
    {
        var grid = Ext.getCmp("tr");
        var rows = grid.getSelectionModel().getSelectedNode();
         CreateCompanyId = rows.attributes.id;
    }
    var shop = new top.Ext.Window({
        width: 680,
        id: 'shoper',
        height: 500,
        closable: false,
        shadow: false,
        stateful: false,
        border: false,
        modal: true,
        layout: 'fit',
        plain: true,
        autoDestroy: true,
        closeAction: 'close',
        title: '二维码选择',
        items: {
            autoScroll: true,
            border: false,
            params: { CompanyId: CreateCompanyId },
            autoLoad: { url: '../../Project/Html/selectGroup.htm', scripts: true, nocache: true }
        },
        buttons: [
                    {
                        text: '确定',
                        iconCls: 'GTP_submit',
                        handler: function () {
                            SaveMaster();
                        }
                    },
                    {
                        text: '取消',
                        iconCls: 'GTP_cancel',
                        handler: function () {
                            top.Ext.getCmp("shoper").close();
                        }
                    }]
    }).show();
}

//确定选择二维码
function SaveMaster() {
    var grid = top.Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var Name = rows[0].data["Name"]; //二维码编号
        var QrCode = rows[0].data["QrCode"]; //二维码编码
        top.Ext.getCmp("QRCode").setValue(QrCode);
        top.Ext.getCmp("QrName").setValue(Name);
        top.Ext.getCmp("shoper").close();
    }
    else {
        top.Ext.Msg.show({ title: "信息提示", msg: '请选中一条记录', buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
    }

}




























