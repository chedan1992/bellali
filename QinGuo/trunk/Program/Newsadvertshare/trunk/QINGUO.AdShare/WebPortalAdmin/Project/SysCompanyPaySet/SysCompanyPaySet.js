//创建表单弹框
function CreateFromWindow(title) {

    var store = new Ext.data.Store({
        proxy: new Ext.data.HttpProxy({
            url: "/SysCompanyPaySet/SearchAuthData",
            method: 'POST'
        }),
        reader: new Ext.data.JsonReader({},
            Ext.data.Record.create(["Id", "PayName"]
        ))
    });


    var form = new top.Ext.FormPanel({
        id: "formPanel",
        labelAlign: 'right',
        frame: true,
        bodyStyle: 'padding:5px 5px 0',
        autoScroll: true,
        items: [
			{
			    xtype: 'fieldset',
			    title: '支付类型',
			    items: [
				{
				    layout: 'column',
				    items: [
				{
				    columnWidth: .7,
				    layout: 'form',
				    items: [
								{
								    name: 'TypeName',
								    fieldLabel: '<span class="required">*</span>支付方式名称',
								    xtype: 'textfield',
								    id: 'TypeName',
								    maxLength: 50,
								    allowBlank: false,
								    emptyText: '填写支付方式名称',
								    maxLengthText: '方式名称长度不能超过50个字符',
								    anchor: '95%'
								},
								{
								    xtype: 'combo',
								    fieldLabel: '<span class="required">*</span>接口类型',
								    triggerAction: 'all',
								    id: 'PayName',
								    emptyText: '选择接口类型...',
								    forceSelection: true,
								    selectOnFocus: true,
								    hiddenValue: '',
								    typeAhead: true, // 设置true，完成自动提示 
								    editable: false,
								    allowBlank: false,
								    displayField: 'PayName',
								    valueField: 'Id',
								    anchor: '95%',
								    store: store,
								    listeners: {
								        select: select
								    }
								},
                                  {
                                      fieldLabel: '关联字段',
                                      xtype: 'textfield',
                                      id: 'PayType',
                                      name: 'PayType',
                                      hidden: true
                                  }
						]
				},
				{
				    columnWidth: .3,
				    layout: 'form',
				    items: [{
				        xtype: 'box', //或者xtype: 'component',
				        id: 'PagLogo',
				        width: 150, //图片宽度
				        height: 50, //图片高度
				        autoEl: {
				            tag: 'img',    //指定为img标签
				            src: '/Content/Extjs/resources/images/default/s.gif'    //指定url路径
				        }
				    }]
				}
					]
				},
            {
                name: 'Sex',
                xtype: 'radiogroup',
                fieldLabel: '接口类型',
                id: 'PayFit',
                border: false,
                width: 150,
                items: [
                                 { boxLabel: '电脑版', name: 'PayFit', id: 'PayFit1', inputValue: 1, disabled: true, width: 100, height: 25 },
                                 { boxLabel: '手机版', name: 'PayFit', id: 'PayFit2', inputValue: 2, disabled: true, width: 100, height: 25 }
                               ]

            }
            ]
			},

			{
			    xtype: 'fieldset',
			    title: '商户信息',
			    id: 'ShopInfo',
			    items: [
				{
				    xtype: 'compositefield',
				    fieldLabel: '<span class="required">*</span>商户号',
				    combineErrors: false,
				    items: [
							{
							    name: 'Partner',
							    fieldLabel: '',
							    xtype: 'textfield',
							    id: 'Partner',
							    allowBlank: true,
							    maxLength: 50,
							    emptyText: '输入商户号',
							    flex: 1
							},
							{
							    xtype: 'box',
							    width: 20, //图片宽度
							    flex: 1,
							    style: {
							        marginLeft: '-5px',
							        cursor: 'pointer'
							    },
							    height: 20, //图片高度
							    autoEl: {
							        tag: 'img',    //指定为img标签
							        src: '/Resource/css/icons/validate_normal.gif',    //指定url路径
							        title: '商户号一般是您在支付公司注册的账号，您也可以登陆支付公司后按照说明查找您的商户号'
							    }
							}
						]
				},
                {
                    xtype: 'compositefield',
                    fieldLabel: '<span class="required">*</span>商户密钥',
                    combineErrors: false,
                    items: [
							{
							    name: 'RsaPrivate',
							    fieldLabel: '',
							    xtype: 'textfield',
							    id: 'RsaPrivate',
							    allowBlank: true,
							    emptyText: '输入商户密钥',
							    flex: 1
							},
							{
							    xtype: 'box',
							    width: 20, //图片宽度
							    flex: 1,
							    style: {
							        marginLeft: '-5px',
							        cursor: 'pointer'
							    },
							    height: 20, //图片高度
							    autoEl: {
							        tag: 'img',    //指定为img标签
							        src: '/Resource/css/icons/validate_normal.gif',    //指定url路径
							        title: '为了保证支付的安全性和真实性，很多支付公司都为公司提供了一个密钥并要求使用此密钥验证支付信息，也有的支付公司称之为校验码或验证码'
							    }
							}
						]
                },
					{
					    name: 'Seller',
					    fieldLabel: '<span class="required">*</span>支付宝账号',
					    xtype: 'textfield',
					    allowBlank: true,
					    id: 'Seller',
					    maxLength: 2000,
					    emptyText: '输入支付宝账号',
					    anchor: '95%'
					},
					{
					    name: 'RsaAlipayPublic',
					    fieldLabel: '<span class="required">*</span>支付宝公钥',
					    xtype: 'textfield',
					    allowBlank: true,
					    id: 'RsaAlipayPublic',
					    emptyText: '输入支付宝公钥',
					    anchor: '95%'
					},
                    {
                        name: 'Key',
                        fieldLabel: '<span class="required">*</span>支付宝key',
                        xtype: 'textfield',
                        allowBlank: true,
                        id: 'Key',
                        emptyText: '输入支付宝key',
                        anchor: '95%'
                    }
				]
			},
            {
                xtype: 'fieldset',
                title: '账户信息',
                items: [
//                    {
//				    name: 'Account',
//				    fieldLabel: '<span class="required">*</span>开户名',
//				    xtype: 'textfield',
//				    allowBlank: true,
//				    id: 'Account',
//				    emptyText: '输入真实姓名',
//				    anchor: '95%'
//				},
				{
				    name: 'Name',
				    fieldLabel: '<span class="required">*</span>开户名',
				    xtype: 'textfield',
				    allowBlank: true,
				    id: 'Name',
				    emptyText: '输入账户开户名',
				    anchor: '95%'
				}
                ]
            }
		]

    });
    //窗体
    var win = Window("window", title, form);
    win.width = 570;
    win.height =480;
    return win;
}

//支付类型选择
function select(combo, record, index) {
    if (!record.data.Id) {
        return;
    }
    var respon = Ext.lib.Ajax.getConnectionObject().conn;
    respon.open("post", "/SysCompanyPaySet/SearchAuthData?Id=" + record.data.Id, false);
    respon.send(null);
    var result = Ext.util.JSON.decode(respon.responseText);
    if (result.length > 0) {
        //绑定信息
        if (result[0].PayLogo) {
            top.Ext.getCmp("PagLogo").getEl().dom.src = result[0].PayLogo;
        }
        else {
            top.Ext.getCmp("PagLogo").getEl().dom.src = '/Content/Extjs/resources/images/default/s.gif';
        }
        //设置接口类型
        top.Ext.getCmp("PayFit" + result[0].PayFit).setValue(result[0].PayFit);
        top.Ext.getCmp("PayType").setValue(record.data.Id); //combox选中值Id

        setControll(result[0].PayTypeName);
    }
}

//设置控件隐藏显示
function setControll(val) {
    switch (val) {
        case "1": //银联
            top.Ext.getCmp("ShopInfo").show();
            top.Ext.getCmp("Seller").hide(); //支付宝账号
            top.Ext.getCmp("RsaAlipayPublic").hide(); //支付宝密钥
            top.Ext.getCmp("Partner").allowBlank = false; //商户号
            top.Ext.getCmp("RsaPrivate").allowBlank = false; //商户号
            break;
        case "2": //支付宝
            top.Ext.getCmp("ShopInfo").show();
            top.Ext.getCmp("Seller").show();
            top.Ext.getCmp("RsaAlipayPublic").show();
            top.Ext.getCmp("Seller").allowBlank = false;
            top.Ext.getCmp("RsaAlipayPublic").allowBlank = false;
            top.Ext.getCmp("Partner").allowBlank = false; //商户号
            top.Ext.getCmp("RsaPrivate").allowBlank = false; //商户号
            break;
        case "3": //货到付款
            top.Ext.getCmp("ShopInfo").hide();
            top.Ext.getCmp("Seller").allowBlank = true;
            top.Ext.getCmp("RsaAlipayPublic").allowBlank = true;
            top.Ext.getCmp("Partner").allowBlank = true; //商户号
            top.Ext.getCmp("RsaPrivate").allowBlank = true; //商户号
            break;
        case "4": //货到付款--电脑版
            top.Ext.getCmp("ShopInfo").hide();
            top.Ext.getCmp("Seller").allowBlank = true;
            top.Ext.getCmp("RsaAlipayPublic").allowBlank = true;
            top.Ext.getCmp("Partner").allowBlank = true; //商户号
            top.Ext.getCmp("RsaPrivate").allowBlank = true; //商户号
            break;
        case "6": //货到付款--电脑版
            //alert('微信支付');
            break;
        default:
            top.Ext.getCmp("ShopInfo").show();
            top.Ext.getCmp("Seller").hide(); //支付宝账号
            top.Ext.getCmp("RsaAlipayPublic").hide(); //支付宝密钥
            top.Ext.getCmp("Partner").allowBlank = false; //商户号
            top.Ext.getCmp("RsaPrivate").allowBlank = false; //商户号
            break;
    }
    top.Ext.getCmp("formPanel").doLayout();
}

//新增
function AddDate() {
    var win = CreateFromWindow("新增");
    url = '/SysCompanyPaySet/SaveData?1=1';
    win.show();
}

//编辑
function EditDate() {
    var grid = Ext.getCmp("gg");
    //得到选后的数据   
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var PayType = rows[0].get("PayType");
//        //根据编号查询是否有编辑权限
//        var respon = Ext.lib.Ajax.getConnectionObject().conn;
//        respon.open("post", "/SysCompanyPaySet/LoadData?key=" + PayType, false);
//        respon.send(null);
//        var result = Ext.util.JSON.decode(respon.responseText);
//        if (result.success) {
//            MessageInfo("暂未分配该类型,无法编辑！", "statusing");
//            return false;
        //        }
        var win = CreateFromWindow("编辑");
        var form = top.Ext.getCmp('formPanel');
        var key = rows[0].get("Id");

        //设置接口类型
        if (rows[0].json.PayFit) {
            top.Ext.getCmp("PayFit").items[rows[0].json.PayFit - 1].checked = true;
        }
        setControll(rows[0].json.PayTypeName);
        url = '/SysCompanyPaySet/SaveData?Id=' + key + '&modify=1';
        top.Ext.getCmp("formPanel").form.loadRecord(rows[0]); //再加载数据

        top.Ext.getCmp("Seller").setValue(rows[0].json.Seller);
        top.Ext.getCmp("RsaAlipayPublic").setValue(rows[0].json.RsaAlipayPublic);
        top.Ext.getCmp("Partner").setValue(rows[0].json.Partner);
        top.Ext.getCmp("RsaPrivate").setValue(rows[0].json.RsaPrivate);

        //combox设置默认选中
        top.Ext.getCmp("PayName").setValue(rows[0].json.PayName); //value
        top.Ext.getCmp("PayName").setRawValue(rows[0].json.PayName); //text
        top.Ext.getCmp("PayType").setValue(rows[0].json.PayType); //combox选中值Id

        if (rows[0].json.PayLogo) {
            form.findById("PagLogo").autoEl.src = rows[0].json.PayLogo;
        }
        else {
            form.findById("PagLogo").autoEl.src = '/Content/Extjs/resources/images/default/s.gif';
        }
        form.doLayout();
        win.show();
    }
    else {
        MessageInfo("请选中一条记录！", "statusing");
    }
}

//保存
function SaveDate() {
    var formPanel = top.Ext.getCmp("formPanel");
    if (formPanel.getForm().isValid()) {//如果验证通过
        var PayTypeID = top.Ext.getCmp("PayType").getValue(); //取得display值
        url += "&PayTypeID=" + PayTypeID;
        formPanel.getForm().submit({
            waitTitle: '系统提示', //标题
            waitMsg: '正在提交数据,请稍后...', //提示信息
            submitEmptyText: false,                            
            url: url, //记录表单提交的路径
            method: "POST",
            success: function (form, action) {
                //成功后
                var flag = action.result;
                if (flag.success) {
                    Ext.getCmp("gg").store.reload();
                    MessageInfo("保存成功！", "right");
                } else {
                    top.Ext.Msg.show({ title: "信息提示", msg: "保存失败！", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                    return false;
                }
                top.Ext.getCmp('window').close();
            },
            failure: function (form, action) {
                top.Ext.Msg.show({ title: "信息提示", msg: action.result.msg, buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                return false;
            }
        });
    }
}

//删除
function DeleteDate() {
    var grid = Ext.getCmp("gg");
    var rows = grid.getSelectionModel().getSelections();
    if (rows.length == 1) {
        var confirm = top.Ext.MessageBox.confirm('系统确认', '确认要删除该信息吗?', function (e) {
            if (e == "yes") {
                var ids = [];
                for (var i = 0; i < rows.length; i++) {
                    ids.push("'"+rows[i].data["Id"]+"'");
                }
                Ext.Ajax.request({
                    url: '/SysCompanyPaySet/DeleteData',
                    params: { id: ids.join(",") },
                    success: function (response) {
                        var rs = eval('(' + response.responseText + ')');
                        if (rs.success) {
                            //判断是否在grid最后一条的时候删除,如果删除,重新加载
                            Ext.getCmp("gg").store.reload();
                            MessageInfo("删除成功！", "right");
                        } else {
                            top.Ext.Msg.show({ title: "信息提示", msg:"删除失败！", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                            return false;
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

