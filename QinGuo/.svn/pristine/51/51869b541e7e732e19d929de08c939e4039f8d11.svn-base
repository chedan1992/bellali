<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title>公司资质管理</title>
    <script src="../../Content/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        var idArray = new Array();
        var nameArray = new Array();
        var array = [];

        var dataview = "";
        Ext.onReady(function () {
            var tb = new Ext.Toolbar();
            tb.add({
                text: '删除',
                iconCls: 'GTP_delete',
                tooltip: '删除资质',
                handler: function (sender) {
                    alert(idArray.length);
                    if (idArray.length > 0) {
                        //删除服务器
                        Ext.Ajax.request({
                            url: '/BIZ/ImgDel',
                            params: { idArray: idArray.join(","), nameArray: nameArray.join(",") },
                            success: function (response) {
                                var rs = eval('(' + response.responseText + ')');
                                if (rs.success) {
                                    if (array.length == 1) { array = []; } else {
                                        for (var i = 0; i < length; i++) {
                                            var Name = new Array();
                                            Name = nameArray[i].split('/');
                                            var indresult = [idArray[i], Name[Name.length - 1], nameArray[i]];
                                            var index = array.indexOf(indresult);
                                            array.splice(index, 1);
                                        }
                                    }

                                    var store = new Ext.data.ArrayStore({
                                        proxy: new Ext.data.HttpProxy({
                                            url: "/BIZ/ListImg"
                                        }),
                                        fields: ['ID', 'Name', 'PicUrl'],
                                        sortInfo: {
                                            field: 'Name',
                                            direction: 'ASC'
                                        }
                                    });
                                    store.loadData(array);
                                    dataview.setStore(store);
                                    click();
                                } else {
                                    //MessageInfo("删除失败！", "error");
                                    // parent.parent.$('#logout').tip({ width: '240', status: 'error', content: "删除失败！", dis_time: 1000 });

                                }
                            },
                            failure: function (form, action) {
                                // MessageInfo("error！", "error");
                                //parent.parent.$('#logout').tip({ width: '240', status: 'error', content: "error！", dis_time: 1000 });

                            }
                        });
                        //top.Ext.getCmp('btnMangWin').close();
                    } else {
                        //parent.parent.$('#logout').tip({ width: '240', status: 'error', content: "请选择您要删除的项！", dis_time: 1000 });
                        //MessageInfo("请选择您要删除的项！", "error");
                        alert(1);
                    }
                }
            });

            tb.add('-');
            tb.add({
                text: '关闭',
                iconCls: 'GTP_cancel',
                tooltip: '关闭取消',
                handler: function (sender) {
                    top.Ext.getCmp('btnMangWin').close();
                }
            });

            var tpl = new Ext.XTemplate(
                '<ul>',
                '<tpl for=".">',
                '<li class="phone" title="{PicUrl}" id="{ID}" style="height:150px; border:1px dashed #ccc; float:left;margin-right:10px;margin-bottom:10px;margin-left:20px;width:160;text-align:center;padding:20px;">',
            //'<div style="width:150;"><img width="150" height="150" src="images/phones/{[values.name.replace(/ /g, "-")]}.png" /></div>',
                '<div style="width:160;text-align:center;"><img width="120" height="130" src="{PicUrl}" /></div>',
                '<div style="width:160;padding:20;text-align:center;"><strong style="font-size:12px;height:25px;width:160px;display:block;overflow:hidden;">{Name}</strong></div>',
                '</li>',
                '</tpl>',
                '</ul>'
            );

            dataview = new Ext.DataView({
                store: storeLoad(),
                tpl: tpl,
                autoHeight: true,
                id: 'phones',
                itemSelector: 'li.phone',
                overClass: 'phone-hover',
                singleSelect: true,
                style: 'margin:10px',
                multiSelect: true,
                autoScroll: true
            });
            var form = new Ext.FormPanel({
                id: "formPanelImg",
                renderTo: "divForm",
                height: 500,
                tbar: tb,
                border: false,
                layout: 'fit',
                style: 'margin:0px',
                items: dataview
            });
            //form.render("body");
            click();
        });

        function click(parameters) {
            $(".phone").click(function () {

                var id = $(this).attr("id");
                var name = $(this).attr("title");
                var doc = document.getElementById(id);
                if (doc.style.borderStyle == "dashed") {
                    doc.style.borderStyle = "inset";
                    doc.style.backgroundColor = "#ccd8e7";
                    //                    $("#" + id).css("borderStyle", "inset");
                    //                    $("#" + id).css("backgroundColor", "#ccd8e7");
                    idArray.push(id);
                    nameArray.push(name);

                } else {
                    doc.style.borderStyle = "dashed";
                    doc.style.backgroundColor = "#fff";
                    //                    $("#" + id).css("borderStyle", "dashed");
                    //                    $("#" + id).css("backgroundColor", "#fff");
                    idArray.splice(idArray.indexOf(id), 1);
                    nameArray.splice(nameArray.indexOf(name), 1);
                }
            });
        }

        function storeLoad() {
            var result = Ext.getCmp('btnMangWin').items.items[0].params.result;
            if (result.length > 0) {
                $.each(result, function (i, n) {
                    if (n != null) {
                        array.push([n.ID, n.Name, n.PicUrl]);
                    }
                });
            }
            var store = new Ext.data.ArrayStore({
                proxy: new Ext.data.HttpProxy({
                    url: "/BIZ/ListImg"
                }),
                fields: ['ID', 'Name', 'PicUrl'],
                sortInfo: {
                    field: 'Name',
                    direction: 'ASC'
                }
            });
            store.loadData(array);
            return store;
        }

    </script>
</head>
<body id="body" style="margin: 0px; padding: 0px">
    <div id="divForm">
    </div>
</body>
</html>
