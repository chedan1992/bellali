<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
 <div id="split" style="height: 600px;" class="x-hide-display">
        <img src="../../Resource/Img/split.jpg" />
    </div>
    <div id="right" style="width: 300px;" class="x-hide-display">
        <img src="../../Resource/Img/right.jpg" />
    </div>
      <div id="form_panel" class="x-hide-display">
    </div>
     <form method="post" action="#" class="hide hideform"></form>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="UrlContent" runat="server">
 <script src="<%=Url.Content("~/Content/Extjs/ux/ComboBoxTree/form.ComboBoxTree.js") %>"
        type="text/javascript"></script>
    <!--百度编辑器-->
    <style type="text/css">
        .top
        {
            font-family: "微软雅黑";
            font-size: 24px;
            font-weight: bold;
            color: #c1c1c1;
        }
        .MicrositeTxt
        {
            margin-right: 50px;
            margin-top: 20px;
            float: right;
            width: 260px;
            min-height: 450px;
            text-align: center;
        }
        .footer
        {
            background-color: #f9f9f9;
            margin: 0 0 0 0;
            padding: 0px;
        }
        #pn td
        {
            padding: 0px;
        }
    </style>
     <style type="text/css">
        #ContentDiv ul
        {
            position: relative;
            display: block;
            height: auto;
            font-size: 85%;
        }
       #ContentDiv ul li img
        {
            margin-bottom: 1px;
        }
       #ContentDiv ul li
        {
            padding-left:0px;
            padding-bottom:10px;
            margin-top:5px;
            margin-right:5px;
            line-height: 1.25em;
            color: #333;
            font-family: "Helvetica Neue" ,sans-serif;
            overflow: hidden;
            border-top: 1px solid transparent;
        }
        #ContentDiv li strong
        {
            color: #000;
            display: block;
        }
    </style>
    <link href="../../Content/ueditor/themes/default/css/ueditor.css" rel="stylesheet"
        type="text/css" />
    <script src="<%=Url.Content("~/Content/ueditor/ueditor.all.js") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Content/ueditor/ueditor.config.js") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Content/ueditor/lang/zh-cn/zh-cn.js") %>" type="text/javascript"></script>
    <!--引用fileUpLoad-->
    <script src="<%=Url.Content("~/Content/Extjs/ux/fileuploadfield/FileUploadField.js") %>"
        type="text/javascript"></script>
    <link href="../../Content/Extjs/ux/fileuploadfield/css/fileuploadfield.css" rel="stylesheet"
        type="text/css" />
    <!--文件区域-->
    <script src="../../Content/Extjs/VTypes/ExtVTypes.js" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Common/grid.js?version=1.5") %>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Project/EDynamic/EDynamicEdit.js?version=1.5") %>"
        type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        Ext.onReady(function () {

            //切换模版,显示列表
            function CreateTemple(Num) {
                var response = [
            { ID: 1, Name: '大图', mark: '640*300高度可变', large: '640*300', TemplePic: '../../Resource/img/templePic/2.jpg', limit: 1, top: 50 },
            { ID: 2, Name: '大图加轮循', mark: '640*300高度可变', large: '640*300', TemplePic: '../../Resource/img/templePic/1.jpg', limit: 5, top: 50 },
            { ID: 3, Name: '三张小图', mark: '220*150三张图大小一样', large: '220*150', TemplePic: '../../Resource/img/templePic/3.jpg', limit: 3, top: 50 },
            { ID: 4, Name: '两张小图', mark: '320*150两张图大小一样', large: '320*150', TemplePic: '../../Resource/img/templePic/4.jpg', limit: 2, top: 60 },
            { ID: 5, Name: '左图右文', mark: '150*150宽高一样', large: '150*150', TemplePic: '../../Resource/img/templePic/5.jpg', limit: 1, top: 30 },
            { ID: 6, Name: '左文右图', mark: '150*150宽高一样', large: '150*150', TemplePic: '../../Resource/img/templePic/6.jpg', limit: 1, top: 30 },
            { ID: 7, Name: '视频', mark: '640*300高度可变', large: '640*300', TemplePic: '../../Resource/img/templePic/7.jpg', limit: 1, top: 70 }];

                var temple = '<span><input type="radio" class="Vote" name="Vote" id="Vote{ID}" value="{limit}" checked=true onclick=radioChk(this)> </span>';
                var tpl = new Ext.XTemplate(
                            '<ul>',
                            '<tpl for=".">',
                                '<li>',
                                    '<label for="Vote{ID}"><img title="{large}" width="300" src="{TemplePic}" style="float: left; border:1px solid #ccc;"/>',
                                    '<div style="margin-left: 310px;margin-top: {top}px;">', temple,
                                    '<span>{Name}</span><br/><span>{mark}</span></div></label>',
                                '</li>',
                            '</tpl>',
                            '</ul>'
                        );
                tpl.overwrite(Ext.getCmp("ContentDiv").body, response);

               
                Ext.getCmp("formPanel").doLayout();
            }

            var north = new Ext.Panel({
                region: 'north',
                tbar: Toolbar(),
                border: false
            });


            /*
            加载省
            */
            var comProvince = new Ext.data.Store({
                proxy: new Ext.data.HttpProxy({
                    url: '/Area/comProvince',
                    method: 'POST'
                }),
                reader: new Ext.data.JsonReader({},
                    Ext.data.Record.create(["Code", "Name"]
                ))
            });
            /*
            加载市区
            */
            var comCity = new Ext.data.Store({
                proxy: new Ext.data.HttpProxy({
                    url: '/Area/comCity',
                    method: 'POST'
                }),
                        reader: new Ext.data.JsonReader({},
                    Ext.data.Record.create(["Code", "Name"]
                ))
            });

            var form = new Ext.FormPanel({
                id: "formPanel",
                fileUpload: true, //有需文件上传的,需填写该属性
                border: false,
                labelAlign: 'right',
                autoScroll: true,
                labelWidth: 75,
                bodyStyle: 'margin:5px;padding:15px;',
                items: [
                 {
                     xtype: 'panel',
                     border: false,
                     layout: 'column',
                     items: [
                           {
                               columnWidth: .6,
                               layout: 'form',
                               border: false,
                               bodyStyle: 'margin:5px;padding:14px;',
                               items: [
                                  {
                                      name: 'Name',
                                      fieldLabel: '<span class="required">*</span>新闻标题',
                                      xtype: 'textfield',
                                      id: 'Name',
                                      maxLength: 50,
                                      allowBlank: false,
                                      emptyText: '请输入标题名称',
                                      maxLengthText: '标题长度不能超过50个字符',
                                      anchor: '80%'
                                  },
                                  {
                                     xtype: 'radiogroup',
                                     fieldLabel: '<span class="required">*</span>展示方式',
                                     id: 'ShowType',
                                     allowBlank: false,
                                     width: 220,
                                     items: [
						                    { boxLabel: '无时间限制', name: 'ShowType', id: 'ShowType1', inputValue: 1, checked: true },
                                            { boxLabel: '自动下架', name: 'ShowType', id: 'ShowType2', inputValue: 2 }
                                        ],
                                     listeners: {
                                         'change': function (checked) {
                                             ChangeType();
                                         }
                                     }
                                 },
                                  {
                         xtype: 'compositefield',
                         fieldLabel: '<span class="required">*</span>有效时间',
                         id: 'timeLimit',
                         hidden: true,
                         combineErrors: false,
                         items: [
							    {
							        xtype: 'textfield',
							        hidden: true
							    },
                                {
                                    name: 'StartDate',
                                    id: 'StartDate',
                                    xtype: 'datefield',
                                    allowBlank: false,
                                    value: new Date(),
                                    width: 130,
                                    emptyText: '选择开始时间',
                                    format: 'Y-m-d(周l)',
                                    vtype: 'daterange',
                                    endDateField: 'EndDate'
                                },
                                 new Ext.form.TimeField({
                                     width: 70,
                                     id: 'StartTime',
                                     maxValue: '23:59',                      //最大时间   
                                     maxText: '所选时间小于{0}',
                                     minValue: '00:00',                      //最小时间   
                                     minText: '所选时间大于{0}',
                                     maxHeight: 180,
                                     allowBlank: false,
                                     value: new Date().getHours() + 1,
                                     increment: 15,                          //时间间隔   
                                     format: 'G:i',                    //时间格式   
                                     invalidText: '时间格式无效'
                                 }),
                                {
                                    xtype: 'tbtext',
                                    text: '~'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'EndDate',
                                    id: 'EndDate',
                                    value: new Date((new Date() / 1000 + 86400 * 10) * 1000),
                                    allowBlank: false,
                                    format: 'Y-m-d(周l)',
                                    emptyText: '选择结束时间',
                                    width: 130,
                                    vtype: 'daterange',
                                    startDateField: 'StartDate'
                                },
                                    new Ext.form.TimeField({
                                        width: 70,
                                        id: 'EndTime',
                                        maxValue: '23:59',                      //最大时间   
                                        maxText: '所选时间小于{0}',
                                        minValue: '00:00',                      //最小时间   
                                        minText: '所选时间大于{0}',
                                        maxHeight: 180,
                                        allowBlank: false,
                                        value: new Date().getHours(),
                                        increment: 15,                          //时间间隔   
                                        format: 'G:i',                    //时间格式   
                                        invalidText: '时间格式无效'
                                    })
					            ]
                                },
                                 {
                                     xtype: 'textfield',
                                     id: 'GroupId',
                                     name: 'GroupId',
                                     hidden: true
                                 },
                               
                                    {
                                             xtype: 'compositefield',
                                             fieldLabel: '省市区',
                                             combineErrors: false,
                                             anchor: '100%',
                                             items: [
                                                 {
                                                     xtype: 'combo',
                                                     triggerAction: 'all',
                                                     id: 'comProvince',
                                                     emptyText: '所在省',
                                                     forceSelection: true,
                                                     editable: false,
                                                     displayField: 'Name',
                                                     valueField: 'Code',
                                                     hiddenName: 'Name',
                                                     width: 80,
                                                     store: comProvince,
                                                     listeners: {
                                                         select: {
                                                             fn: function (combo, record, index) {
                                                                 comCity.proxy = new Ext.data.HttpProxy({ url: '/Area/comCity?code=' + record.data["Code"], method: 'POST' });
                                                                 comCity.load();
                                                                 Ext.getCmp("comCity").setValue(""); //value
                                                                 Ext.getCmp("comCity").setRawValue(""); //text 

                                                             }
                                                         }
                                                     }
                                                 },
                                                 {
                                                     xtype: 'combo',
                                                     triggerAction: 'all',
                                                     id: 'comCity',
                                                     emptyText: '所在市',
                                                     forceSelection: true,
                                                     editable: false,
                                                     displayField: 'Name',
                                                     valueField: 'Code',
                                                     hiddenName: 'Name',
                                                     width: 75,
                                                     store: comCity,
                                                     listeners: {
                                                         select: {
                                                             fn: function (combo, record, index) {
                                                              
                                                             }
                                                         }
                                                     }
                                                 },
                                                 {
                                                     xtype: 'tbtext',
                                                     text: '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="required">*</span>新闻分类',
                                                     style:'margin-top:4px'
                                                 },
                                                  {
                                                      xtype: 'xcomboboxtree',
                                                      fieldLabel: '<span class="required">*</span>新闻分类',
                                                      id: 'GroupName',
                                                      enableClearValue: true, //显示清除值的trigger 
                                                      lines: true, //显示树形控件的前导线
                                                      autoScroll: true,
                                                      rootVisible: false,
                                                      useArrows: true,
                                                      animate: true,
                                                      anchor: '90%',
                                                      emptyText: '新闻分类',
                                                      allowBlank: false,
                                                      enableDD: true,
                                                      istWidth: 80, //下拉框的长度   
                                                      listHeight: 200, //下拉框的高度   
                                                      tree: new Ext.tree.TreePanel({
                                                          root: new Ext.tree.AsyncTreeNode({
                                                              id: '-1',
                                                              text: '新闻分类'
                                                          }),
                                                          id: 'treecombo',
                                                          rootVisible: true, //设为false将隐藏根节点，
                                                          loader: new Ext.tree.TreeLoader({
                                                              url: "/EDynamic/GetTree?id=0"
                                                          }),
                                                          listeners: {
                                                              click: itemclick
                                                          }
                                                      })
                                                  }
                                                 ]
                                             },
                                              
                                 {
                                 xtype: 'radiogroup',
                                 fieldLabel: '<span class="required">*</span>新闻类型',
                                 id: 'ActionType',
                                 allowBlank: false,
                                 width: 220,
                                 items: [
						                { boxLabel: '内部新闻', name: 'ActionType', id: 'ActionType1', inputValue: 1, checked: true },
                                        { boxLabel: '外部新闻', name: 'ActionType', id: 'ActionType2', inputValue: 2 }
                                    ],
                                 listeners: {
                                     'change': function (checked) {
                                         ChangeActionType();
                                     }
                                 }
                             },
                                 {
                         name: 'Link',
                         fieldLabel: '<span class="required">*</span>链接地址',
                         xtype: 'textfield',
                         id: 'Link',
                         hidden: true,
                         allowBlank: true,
                         emptyText: '请输入链接地址(完整链接地址需含http 如:http://www.baidu.com)',
                         anchor: '80%'
                     },
                                 {
                         fieldLabel: '<span class="required">*</span>描述信息',
                         xtype: 'textarea',
                         id: 'Content',
                         name: 'Content',
                         allowBlank: true,
                         height: 250,
                         anchor: '90%'
                     },
                 new Ext.form.Checkbox({
                            fieldLabel: '是否置顶',
                            boxLabel: '',
                            id: 'ChkIsTop',
                            name: 'ChkIsTop',
                            style:"margin-top:4px"
                        }),
                      {
                         name: 'Author',
                         fieldLabel: '来源',
                         xtype: 'textfield',
                         id: 'Author',
                         maxLength: 50,
                         emptyText: '请输入作者名称',
                         maxLengthText: '作者名称长度不能超过50个字符',
                         anchor: '80%'
                     }, {
                                     name: 'ReadNum',
                                         fieldLabel: '阅读量',
                                         xtype: 'numberfield',
                                         id: 'ReadNum',
                                         minValue: 0,
                                         maxValue: 99999999,
                                         width: 240,
                                         allowDecimals: false,
                                         anchor: '80%'
                                     },
                                     {
                                         name: 'Sort',
                                         fieldLabel: '排序',
                                         xtype: 'numberfield',
                                         id: 'Sort',
                                         minValue: 0,
                                         maxValue: 99999999,
                                         width: 240,
                                         allowDecimals: false,
                                         anchor: '80%'
                                     }
                                ]
                 },

                          {
                              width: 10,
                              height: 450,
                              border: false,
                              contentEl: 'split'
                          },
                    {
                        columnWidth: .4,
                        border: false,
                        layout: 'form',
                        labelWidth: 75,
                        bodyStyle: 'margin:5px;padding:15px;',
                        items: [
                            {
                              xtype: 'panel',
                              id: 'ContentDiv',
                              fieldLabel: '模块选择',
                              border: false,
                              items: []
                          },
                            {
                                xtype: 'panel',
                                fieldLabel: '<span class="required">*</span>广告图',
                                id: 'composCombin',
                                border: false,
                                layout: 'form',
                                items: [
                               {
                                   xtype: 'compositefield',
                                   combineErrors: false,
                                   hideLabel: true,
                                   width:330,
                                   items: [
                                         {
                                             xtype: 'textfield',
                                             value: '1', //1:新增 0:修改 -1:删除
                                             id: 'Ismodify1',
                                             hidden: true
                                         },
                                         {
                                             fieldLabel: '主键',
                                             xtype: 'textfield',
                                             id: 'hidKey1',
                                             hidden: true
                                         },
							           {
                                            xtype: 'fileuploadfield',
                                            emptyText: '可上传标识图',
                                            fieldLabel:"1列模块",
                                            id: 'winImg1',
                                            controlId: 1,
                                            allowBlank: false,
                                            buttonText: '',
                                            buttonCfg: {
                                                iconCls: 'image_add',
                                                tooltip: '图片选择'
                                            },
                                            width: 250,
                                            listeners: {
                                                'fileselected': {
                                                    fn: this.OneFileUploadAction,
                                                    scope: this
                                                }
                                            }
                                        },
								        new Ext.Button({
								            text: '+',
								            tip: '添加',
								            width: 40,
								            handler: AddCombin
								        }),
                                        {
                                            xtype: 'box', //或者xtype: 'component',
                                            id: 'browseImage1',
                                            width: 60, //图片宽度
                                            height: 25, //图片高度
                                            autoEl: {
                                                tag: 'div',    //指定为img标签
                                                style: "filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=scale);"
                                            }
                                        },
                                        {
                                            xtype: 'box', //或者xtype: 'component',
                                            id: 'browseImage_img1',
                                            width: 60, //图片宽度
                                            hidden: true,
                                            height: 25, //图片高度
                                            autoEl: {
                                                tag: 'img',    //指定为img标签
                                                src: '/Extjs/resources/images/default/s.gif'
                                            }
                                        }
						            ]
								    },
                                   {
                                       xtype: 'panel',
                                       hideLabel: true,
                                       border: false,
                                       id: 'ContentForm',
                                       contentEl: 'form_panel'
                                   }
                               
						]
                            }
                                ]
                    }
                  ]
                 }
                ]
            });

            var viewport = new Ext.Viewport({
                layout: 'border',
                id: 'viewMain',
                items: [north,
                        new Ext.Panel({
                            region: 'center',
                            border: false,
                            layout: 'fit',
                            deferredRender: false,
                            items: [form]
                        })
                      ]
            });

            var editor = new baidu.editor.ui.Editor({
                autoClearinitialContent: false,
                initialContent: '', //初始化编辑器的内容            
                minFrameHeight: 300, //设置高度            
                textarea: 'Content' //设置提交时编辑器内容的名字，之前我们用的名字是默认的editorValue       
            });
            editor.render('Content'); //渲染

            CreateTemple(7); //默认值

            //默认选中第一个
            var obj = Ext.select('.Vote').elements; //取得是name为aradio的radio组的值的数组。
            var aradioVal;
            if (obj.length > 0) {
                obj[0].checked = true;
            }

            initVisible(); //初始化
           
        });


        //初始化
        function initVisible() {
            var Id = '<%=ViewData["Id"] %>';
            var GroupId = '<%=ViewData["GroupId"] %>';
            if (Id) {//编辑
                url = '/EDynamic/SaveData?Id=' + Id + '&modify=1';
                var respon = Ext.lib.Ajax.getConnectionObject().conn;
                respon.open("post", "/EDynamic/LoadData?Id=" + Id, false);
                respon.send(null);
                var result = Ext.util.JSON.decode(respon.responseText);
                if (result) {
                    Ext.getCmp("Name").setValue(result.Name); //value
                    Ext.getCmp("Sort").setValue(result.Sort); //value
                    Ext.getCmp("ShowType").setValue(result.ShowType); //展示方式
                    Ext.getCmp("ActionType").setValue(result.ActionType); //新闻类型
                    Ext.getCmp("GroupId").setValue(result.GroupId); //text
                    Ext.getCmp("GroupName").setValue(result.GroupId); //text
                    Ext.getCmp("GroupName").setRawValue(result.GroupName);
                    switch (result.ShowType) {
                        case 1: //无时间限制
                            break;
                        case 2: //自动下架
                            var StartDate = new Date(formartTime(result.ActiveStartTime)).format('Y-m-d');
                            var StartTime = new Date(formartTime(result.ActiveStartTime)).format('H:i');
                            var EndDate = new Date(formartTime(result.ActiveEndTime)).format('Y-m-d');
                            var EndTime = new Date(formartTime(result.ActiveEndTime)).format('H:i');
                            Ext.getCmp("StartDate").setValue(StartDate);
                            Ext.getCmp("StartTime").setValue(StartTime);
                            Ext.getCmp("EndDate").setValue(EndDate);
                            Ext.getCmp("EndTime").setValue(EndTime);
                            break;
                    };

                    switch (result.ActionType) {
                        case 1:
                            Ext.getCmp("Link").hide(); //链接地址
                            Ext.getCmp("Content").show(); //内容
                            Ext.getCmp("Link").allowBlank = true;
                            Ext.getCmp("Content").allowBlank = false;

                            var editor = UE.getEditor('Content');
                            editor.ready(function () {//页面渲染完成后才能赋值
                                editor.setContent(result.Content);
                            });

                            break;
                        case 2:
                            Ext.getCmp("Content").hide(); //内容
                            Ext.getCmp("Link").show(); //链接地址
                            Ext.getCmp("Link").allowBlank = false;
                            Ext.getCmp("Content").allowBlank = true;
                            Ext.getCmp("Link").setValue(result.Content);
                            break;
                    }

                    //省
                    Ext.getCmp("comProvince").setValue(result.ProvienceId); //value
                    Ext.getCmp("comProvince").setRawValue(result.ProvienceName); //text
                    Ext.getCmp("comCity").setValue(result.CityId); //value
                    Ext.getCmp("comCity").setRawValue(result.CityName); //text

                    Ext.getCmp("Author").setValue(result.Author); //value
                    Ext.getCmp("ReadNum").setValue(result.ReadNum); //value
                    if (result.IsTop == true) {
                        Ext.getCmp("ChkIsTop").setValue(true); //value
                    }
                    
                    $("input[name=Gender][value=" + result.Template + "]").attr("checked", true); //设置当前性别选中项
                    $("input:radio[name='Vote']").eq(result.Template - 1).attr("checked", 'checked');
                    if (result.ImageList.length > 1) {
                        for (var i = 0; i < result.ImageList.length; i++) {
                            if (result.ImageList[i].Sort == 1) {
                                Ext.getCmp("winImg1").setValue(result.ImageList[i].PicUrl);
                                Ext.getCmp("hidKey1").setValue(result.ImageList[i].Id);
                                Ext.getCmp("browseImage_img1").getEl().dom.src = result.ImageList[i].PicUrl;
                            } else {
                                AddCombin();
                                Ext.getCmp("winImg" + controlid).setValue(result.ImageList[i].PicUrl);
                                Ext.getCmp("hidKey" + controlid).setValue(result.ImageList[i].Id);

                                Ext.getCmp("browseImage_img" + controlid).getEl().dom.src = result.ImageList[i].PicUrl;
                            }
                        }
                    }
                    else {
                        Ext.getCmp("winImg1").setValue(result.ImageList[0].PicUrl);
                        Ext.getCmp("browseImage_img1").getEl().dom.src = result.ImageList[0].PicUrl;
                    }
                }
            }
            else {
                var str = GroupId.split(",");
                if (str[0] != "0") {
                    Ext.getCmp("GroupId").setValue(str[0]); //text
                    Ext.getCmp("GroupName").setValue(str[0]); //text
                    Ext.getCmp("GroupName").setRawValue(str[1]);
                }
                url = '/EDynamic/SaveData?';
            }
            Ext.getCmp("formPanel").doLayout();
        }

        //保存数据
        function SaveDate() {
            var formPanel = Ext.getCmp("formPanel");
            if (formPanel.getForm().isValid()) {//如果验证通过
                var Content = [];
                var form = Ext.getCmp('ContentForm').items;
                var count = form.getCount();
                if (count == 0 && Ext.getCmp("winImg1").getValue()=="") {
                    top.Ext.Msg.show({ title: "信息提示", msg: "请添加图片!", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                    return false;
                }
                var sortIndex =2;
                for (var i = 0; i < count; i++) {
                    //验证图片是否上传正确
                    var num = form.items[i].num;
                    var Ismodify = Ext.getCmp("Ismodify" + num).getValue(); //标记模块图片是否修改过  1:新增 0:修改 -1:删除
                    var picValue = Ext.getCmp("winImg" + num).getValue(); //获取图片名称
                    var key = Ext.getCmp("hidKey" + num).getValue();

                    var num = parseInt(form.items[i].num); //当前控件主键序号
                    var order = parseInt(form.items[i].name); //当前记录的屏数
                    if (form.items[i].hidden == false) {//取消隐藏的控件 
                        if (picValue == "") {
                            top.Ext.Msg.show({ title: "信息提示", msg: "请上传标识图!", buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO });
                            return false;
                        }
                        var Main = {
                            Key: key,
                            picValue: picValue,
                            Modify: Ismodify, // 1:新增 0:修改 -1:删除
                            Sort: sortIndex //记录控件序号
                        };
                        sortIndex++;
                    }
                    else {
                        var Main = {
                            Key: key,
                            picValue: picValue,
                            Modify: Ismodify, // 1:新增 0:修改 -1:删除
                            Sort: sortIndex //记录控件序号
                        };
                    }
                    Content.push(Main);
                }
                var Main = {
                    Key: Ext.getCmp('hidKey1').getValue(),
                    picValue: Ext.getCmp('winImg1').getValue(),
                    Modify: Ext.getCmp("Ismodify1").getValue(),
                    Sort: 1 //记录控件序号
                };
                Content.push(Main);
                //新闻类型
                var ActionType = Ext.getCmp("ActionType").getValue().inputValue;
                //展示方式
                var ShowType = Ext.getCmp("ShowType").getValue().inputValue;

                //获取省市区
                var comProvince = Ext.getCmp("comProvince").getValue();
                var comCity = Ext.getCmp("comCity").getValue();
                var IsTop = Ext.getCmp("ChkIsTop").getValue();
                var Template = 1;
                var id = document.getElementsByName('Vote');
                var value = new Array();
                for (var i = 0; i < id.length; i++) {
                    if (id[i].checked)
                        Template = id[i].id.replace('Vote', '');
                }

                var para = {};
                if (ShowType == 2) {
                    //组合时间
                    var BeginTime = Ext.getCmp("StartTime").getValue();
                    var bHours = BeginTime.split(':')[0].length > 1 ? BeginTime.split(':')[0] : "0" + BeginTime.split(':')[0]; //小时
                    var bMiuns = BeginTime.split(':')[1]; //分钟
                    BeginTime = bHours + ":" + bMiuns;

                    var EndTime = Ext.getCmp("EndTime").getValue();
                    var eHours = EndTime.split(':')[0].length > 1 ? EndTime.split(':')[0] : "0" + EndTime.split(':')[0]; //小时
                    var eMiuns = EndTime.split(':')[1]; //分钟
                    EndTime = eHours + ":" + eMiuns;

                    var sbeginDate = Ext.getCmp("StartDate").getValue().format('Y-m-d') + ' ' + BeginTime;
                    var sendDate = Ext.getCmp("EndDate").getValue().format('Y-m-d') + ' ' + EndTime;
                    var beginDate = Date.parseDate(sbeginDate, "c"); //开始时间
                    var endDate = Date.parseDate(sendDate, "c"); //结束时间

                    if (endDate <= beginDate) {
                        MessageInfo("结束时间需要大于开始时间！", "statusing");
                        return false;
                    }
                    para = { ProvienceId: comProvince, CityId: comCity, ActiveStartTime: sbeginDate, ActiveEndTime: sendDate, Template: Template, IsTop: IsTop, AdModel: Ext.encode(Content) };
                }
                else {
                    para = { ProvienceId: comProvince, CityId: comCity, Template: Template, IsTop: IsTop, AdModel: Ext.encode(Content) }
                }

                formPanel.getForm().submit({
                    waitTitle: '系统提示', //标题
                    waitMsg: '正在提交数据,请稍后...', //提示信息  
                    submitEmptyText: false,
                    url: url, //记录表单提交的路径
                    method: "POST",
                    params: para,
                    success: function (form, action) {
                        var flag = action.result; //成功后
                        if (flag.success) {

                            url = flag.msg;
                            MessageInfo("提交成功！", "right");
                        } else {
                            if (url.indexOf('modify') > 0) {
                                var ID = '<%= ViewData["Id"] %>';
                                url = '/EDynamic/SaveData?Id=' + Id + '&modify=1';
                            } else {
                                url = '/Advertise/SaveData?1=1';
                            }
                            top.Ext.MessageBox.show({
                                title: '信息提示',
                                msg: flag.msg,
                                buttons: Ext.MessageBox.OK,
                                icon: Ext.MessageBox.INFO
                            });
                        }
                        CloseWindow();
                    },
                    failure: function (form, action) {
                        if (url.indexOf('modify') > 0) {
                            var ID = '<%= ViewData["Id"] %>';
                            url = '/Advertise/SaveData?Id=' + Id + '&modify=1';
                        } else {
                            url = '/Advertise/SaveData?1=1';
                        }
                        var rs = eval('(' + action.response.responseText + ')');
                        top.Ext.MessageBox.show({
                            title: '信息提示',
                            msg: rs.msg,
                            buttons: Ext.MessageBox.OK,
                            icon: Ext.MessageBox.INFO
                        });
                        return false
                    }
                });
            }
        }

    </script>
</asp:Content>
