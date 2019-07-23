<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="UrlContent" runat="server">
 <!--百度编辑器-->
    <link href="../../Content/ueditor/themes/default/css/ueditor.css" rel="stylesheet"
        type="text/css" />
    <script src="<%=Url.Content("~/Content/ueditor/ueditor.all.js") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Content/ueditor/ueditor.config.js") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Content/ueditor/lang/zh-cn/zh-cn.js") %>" type="text/javascript"></script>

    <script language="javascript"  type="text/javascript">
        Ext.onReady(function () {
            var tb = new Ext.Toolbar();
            tb.add({
                text: '保存',
                iconCls: 'GTP_save',
                tooltip: '提交保存',
                handler: SaveDate
            });

            var form = new Ext.FormPanel({
                id: "formPanel",
                fileUpload: true, //有需文件上传的,需填写该属性
                border: false,
                labelAlign: 'right',
                labelWidth: 85,
                renderTo: Ext.getBody(),
                tbar: tb,
                items: [
                       {
                           xtype: "fieldset",
                           autoHeight: true,
                           title: "信息配置",
                           style: 'margin:10px',
                           items: [
                               {
                                   xytpe: 'panel',
                                   layout: 'form',
                                   border: false,
                                   defaultType: 'textfield',
                                   items: [
                                       {
                                           xtype: 'compositefield',
                                           fieldLabel: '抢单数',
                                           combineErrors: false,
                                           anchor: '90%',
                                           items: [
                                              {
                                                  name: 'GrabSingNum',
                                                  id: 'GrabSingNum',
                                                  xtype: 'numberfield',
                                                  minValue:1,
                                                  maxValue: 999999999,
                                                  allowBlank: false,
                                                  width: 240,
                                                  allowDecimals: false
                                              },
                                              {
                                                  xtype: 'tbtext',
                                                  style: 'margin-top:5px',
                                                  text: '（每天最多抢单数）'
                                              }
                                       ]
                                       },

                                     {
                                         xtype: 'compositefield',
                                         fieldLabel: '年检',
                                         combineErrors: false,
                                         anchor: '90%',
                                         items: [
                                              {
                                                  name: 'Year_NFC',
                                                  id: 'Year_NFC',
                                                  fieldLabel: '年检',
                                                  xtype: 'textfield',
                                                  maxLength: 100,
                                                  width: 440,
                                                  emptyText: '填写说明', ////textfield自己的属性
                                                  maxLengthText: '长度不能超过100个字符'
                                              },
                                              {
                                                  xtype: 'tbtext',
                                                  style: 'margin-top:5px',
                                                  text: '金额：'
                                              },
                                              {
                                                  name: 'Year_NFCPrice',
                                                  id: 'Year_NFCPrice',
                                                  xtype: 'numberfield',
                                                  maxValue: 999999999,
                                                  minValue: 0,
                                                  decimalPrecision:2,  //精确到小数点后2位 
                                                  allowDecimals:true
                                              },
                                              {
                                                  xtype: 'tbtext',
                                                  style: 'margin-top:5px',
                                                  text: '（元）'
                                              }
                                       ]
                                     },
                                     {
                                         xtype: 'compositefield',
                                         fieldLabel: 'A项',
                                         combineErrors: false,
                                         anchor: '90%',
                                         items: [
                                              {
                                                  name: 'A_NFC',
                                                  id: 'A_NFC',
                                                  xtype: 'textfield',
                                                  maxLength: 100,
                                                  width: 440,
                                                  emptyText: '填写说明', ////textfield自己的属性
                                                  maxLengthText: '长度不能超过100个字符'
                                              },
                                              {
                                                  xtype: 'tbtext',
                                                  align: 'right',
                                                  style: 'margin-top:5px',
                                                  text: '金额：'
                                              },
                                              {
                                                  name: 'A_NFCPrice',
                                                  id: 'A_NFCPrice',
                                                  xtype: 'numberfield',
                                                  maxValue: 999999999,
                                                  minValue: 0,
                                                  decimalPrecision: 2,  //精确到小数点后2位 
                                                  allowDecimals: true
                                              },
                                              {
                                                  xtype: 'tbtext',
                                                  style: 'margin-top:5px',
                                                  text: '（元）'
                                              }
                                       ]
                                     },
                                     {
                                         xtype: 'compositefield',
                                         fieldLabel: 'B项',
                                         combineErrors: false,
                                         anchor: '90%',
                                         items: [
                                              {
                                                  name: 'B_NFC',
                                                  id: 'B_NFC',
                                                  xtype: 'textfield',
                                                  maxLength: 100,
                                                  width: 440,
                                                  emptyText: '填写说明', ////textfield自己的属性
                                                  maxLengthText: '长度不能超过100个字符'
                                              },
                                              {
                                                  xtype: 'tbtext',
                                                  align: 'right',
                                                  style: 'margin-top:5px',
                                                  text: '金额：'
                                              },
                                              {
                                                  name: 'B_NFCPrice',
                                                  id: 'B_NFCPrice',
                                                  xtype: 'numberfield',
                                                  maxValue: 999999999,
                                                  minValue: 0,
                                                  decimalPrecision: 2,  //精确到小数点后2位 
                                                  allowDecimals: true
                                              },
                                              {
                                                  xtype: 'tbtext',
                                                  style: 'margin-top:5px',
                                                  text: '（元）'
                                              }
                                       ]
                                     },
                                     {
                                         xtype: 'compositefield',
                                         fieldLabel: 'C项',
                                         combineErrors: false,
                                         anchor: '90%',
                                         items: [
                                              {
                                                  name: 'C_NFC',
                                                  id: 'C_NFC',
                                                  xtype: 'textfield',
                                                  maxLength: 100,
                                                  width: 440,
                                                  emptyText: '填写说明', ////textfield自己的属性
                                                  maxLengthText: '长度不能超过100个字符'
                                              },
                                              {
                                                  xtype: 'tbtext',
                                                  align: 'right',
                                                  style: 'margin-top:5px',
                                                  text: '金额：'
                                              },
                                              {
                                                  name: 'C_NFCPrice',
                                                  id: 'C_NFCPrice',
                                                  xtype: 'numberfield',
                                                  maxValue: 999999999,
                                                  minValue: 0,
                                                  decimalPrecision: 2,  //精确到小数点后2位 
                                                  allowDecimals: true
                                              },
                                              {
                                                  xtype: 'tbtext',
                                                  style: 'margin-top:5px',
                                                  text: '（元）'
                                              }
                                       ]
                                     },
                                     {
                                         xtype: 'compositefield',
                                         fieldLabel: 'D项',
                                         combineErrors: false,
                                         anchor: '90%',
                                         items: [
                                              {
                                                  name: 'D_NFC',
                                                  id: 'D_NFC',
                                                  xtype: 'textfield',
                                                  maxLength: 100,
                                                  width: 440,
                                                  emptyText: '填写说明', ////textfield自己的属性
                                                  maxLengthText: '长度不能超过100个字符'
                                              },
                                              {
                                                  xtype: 'tbtext',
                                                  align: 'right',
                                                  style: 'margin-top:5px',
                                                  text: '金额：'
                                              },
                                              {
                                                  name: 'D_NFCPrice',
                                                  id: 'D_NFCPrice',
                                                  xtype: 'numberfield',
                                                  maxValue: 999999999,
                                                  minValue: 0,
                                                  decimalPrecision: 2,  //精确到小数点后2位 
                                                  allowDecimals: true
                                              },
                                              {
                                                  xtype: 'tbtext',
                                                  style: 'margin-top:5px',
                                                  text: '（元）'
                                              }
                                       ]
                                     },
                                  
                                     {
                                         xtype: 'compositefield',
                                         fieldLabel: '物业急修',
                                         combineErrors: false,
                                         anchor: '90%',
                                         items: [
                                              {
                                                  name: 'Repair',
                                                  id: 'Repair',
                                                  xtype: 'textfield',
                                                  maxLength: 100,
                                                  width: 440,
                                                  emptyText: '填写说明', ////textfield自己的属性
                                                  maxLengthText: '长度不能超过100个字符'
                                              },
                                              {
                                                  xtype: 'tbtext',
                                                  id: 'online',
                                                  align: 'right',
                                                  style: 'margin-top:5px',
                                                  text: '金额：'
                                              },
                                              {
                                                  name: 'RepairPrice',
                                                  id: 'RepairPrice',
                                                  xtype: 'numberfield',
                                                  maxValue: 999999999,
                                                  minValue: 0,
                                                  decimalPrecision: 2,  //精确到小数点后2位 
                                                  allowDecimals: true
                                              },
                                              {
                                                  xtype: 'tbtext',
                                                  style: 'margin-top:5px',
                                                  text: '（元）'
                                              }
                                       ]
                                          },
                                        {
                                            xtype: 'compositefield',
                                            fieldLabel: '急修项倍数',
                                            combineErrors: false,
                                            anchor: '90%',
                                            items: [
                                              {
                                                  name: 'RepairMber',
                                                  id: 'RepairMber',
                                                  xtype: 'numberfield',
                                                  maxValue: 999999999,
                                                  minValue: 0,
                                                  width: 240,
                                                  allowDecimals: false
                                              }
                                       ]
                                        },
                                     {
                                         xtype: 'compositefield',
                                         fieldLabel: '退单倍数',
                                         combineErrors: false,
                                         anchor: '90%',
                                         items: [
                                              {
                                                  name: 'ChargeBack',
                                                  id: 'ChargeBack',
                                                  xtype: 'numberfield',
                                                  maxValue: 999999999,
                                                  minValue: 0,
                                                  width: 240,
                                                  allowDecimals: false
                                              }
                                       ]
                                          },
                                      {
                                          xtype: 'compositefield',
                                          fieldLabel: '惩罚倍数',
                                          combineErrors: false,
                                          anchor: '90%',
                                          items: [
                                              {
                                                  name: 'RepairMberT',
                                                  id: 'RepairMberT',
                                                  xtype: 'numberfield',
                                                  maxValue: 999999999,
                                                  minValue: 0,
                                                  width: 240,
                                                  allowDecimals: false
                                              },
                                                {
                                                    xtype: 'tbtext',
                                                    style: 'margin-top:5px',
                                                    text: '（扣除上一个工程师的单）'
                                                }
                                       ]
                                          }
                                       
                            ]
                               }
        					]
                       }
                     ]
            });

            Init();
        });

        //初始化
        function Init() {
            var GrabSingNum = '<%=ViewData["GrabSingNum"] %>';
            var Year_NFC = '<%=ViewData["Year_NFC"] %>';
            var Year_NFCPrice = '<%=ViewData["Year_NFCPrice"] %>';
            var A_NFC = '<%=ViewData["A_NFC"] %>';
            var A_NFCPrice = '<%=ViewData["A_NFCPrice"] %>';
            var B_NFC = '<%=ViewData["B_NFC"] %>';
            var B_NFCPrice = '<%=ViewData["B_NFCPrice"] %>';
            var C_NFC = '<%=ViewData["C_NFC"] %>';
            var C_NFCPrice = '<%=ViewData["C_NFCPrice"] %>';
            var D_NFC = '<%=ViewData["D_NFC"] %>';
            var D_NFCPrice = '<%=ViewData["D_NFCPrice"] %>';
            var Repair = '<%=ViewData["Repair"] %>';
            var RepairPrice = '<%=ViewData["RepairPrice"] %>';
            var RepairMber = '<%=ViewData["RepairMber"] %>';
            var ChargeBack = '<%=ViewData["ChargeBack"] %>';
            var RepairMberT = '<%=ViewData["RepairMberT"] %>';
            if (GrabSingNum != "") {
                Ext.getCmp("GrabSingNum").setValue(GrabSingNum);
            }
            Ext.getCmp("Year_NFC").setValue(Year_NFC);
            if (Year_NFCPrice != 0) {
                Ext.getCmp("Year_NFCPrice").setValue(Year_NFCPrice);
            }
            Ext.getCmp("A_NFC").setValue(A_NFC);
            if (A_NFCPrice != 0) {
            Ext.getCmp("A_NFCPrice").setValue(A_NFCPrice);
            }
            Ext.getCmp("B_NFC").setValue(B_NFC);
            if (B_NFCPrice != 0) {
            Ext.getCmp("B_NFCPrice").setValue(B_NFCPrice);
            }
            Ext.getCmp("C_NFC").setValue(C_NFC);
            if (C_NFCPrice != 0) {
            Ext.getCmp("C_NFCPrice").setValue(C_NFCPrice);
            }
            Ext.getCmp("D_NFC").setValue(D_NFC);
            if (D_NFCPrice != 0) {
            Ext.getCmp("D_NFCPrice").setValue(D_NFCPrice);
            }
            Ext.getCmp("Repair").setValue(Repair);
            if (RepairPrice != 0) {
            Ext.getCmp("RepairPrice").setValue(RepairPrice);
            }
            if (RepairMber != 0) {
                Ext.getCmp("RepairMber").setValue(RepairMber);
            }
            if (ChargeBack != 0) {
                Ext.getCmp("ChargeBack").setValue(ChargeBack);
            }
            if (RepairMberT != 0) {
                Ext.getCmp("RepairMberT").setValue(RepairMberT);
            }
            
        }


        //保存
        function SaveDate() {
            var formPanel = Ext.getCmp("formPanel");
            if (formPanel.getForm().isValid()) {//如果验证通过
                formPanel.getForm().submit({
                    waitTitle: '系统提示', //标题
                    waitMsg: '正在提交数据,请稍后...', //提示信息  
                    submitEmptyText: false,
                    url: '/CompanySet/SavaSetting', //记录表单提交的路径
                    method: "POST",
                    success: function (form, action) {
                        var flag = action.result; //成功后
                        if (flag.success) {
                            MessageInfo("保存成功！", "right");
                        } else {
                            MessageInfo(flag.msg, "error");
                        }
                    },
                    failure: function (form, action) {
                        var rs = eval('(' + action.response.responseText + ')');
                        MessageInfo(rs.msg, "error");
                    }
                });
            }
        }
    </script>
</asp:Content>
