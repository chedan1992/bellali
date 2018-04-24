<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>分类管理</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="initial-scale=1.0, user-scalable=no" />
</head>
<body>
    <div id="body">
    </div>
    <input id="hcid" type="hidden" value="<%=ViewData["cid"] %>" />
    <script type="text/javascript">

        var rmCategoryTree;

        //设置节点子级全部选中
        function SetChildNodeChecked(node) {
            var isChecked = node.attributes.checked; //父节点选中,子节点不一定选中,子节点取消,父节点必须取消
//            if (isChecked) {
//                return;
//            }
            var childCount = node.childNodes.length;
            if (childCount > 0) {
                for (var i = 0; i < childCount; i++) {
                    var child = node.childNodes[i];
                    var checkBox = child.getUI().checkbox;
                    child.attributes.checked = isChecked;
                    checkBox.checked = isChecked;
                    checkBox.indeterminate = false;
                    this.SetChildNodeChecked(child);
                }
            }
        }

        //设置节点父节点选中状态
        function SetParentNodeCheckState(node) {
            var parentNode = node.parentNode;
            if (parentNode != null) {
                var checkBox = parentNode.getUI().checkbox;
                var isAllChildChecked = true;
                var someChecked = false;
                var childCount = parentNode.childNodes.length;
                for (var i = 0; i < childCount; i++) {
                    var child = parentNode.childNodes[i];
                    if (child.attributes.checked) {
                        someChecked = true;
                    }
                    else if (!child.getUI().checkbox) {
                        someChecked = true;
                        break;
                    }
                    else if (child.getUI().checkbox.indeterminate == true && child.getUI().checkbox.checked == false) {
                        someChecked = true;
                        isAllChildChecked = false;
                        break;
                    }
                    else {
                        isAllChildChecked = false;
                    }
                }
                if (isAllChildChecked && someChecked) {
                    parentNode.attributes.checked = true;
                    if (checkBox != null) {
                        checkBox.indeterminate = false;
                        checkBox.checked = true;
                    }
                }
                else if (someChecked) {
                    parentNode.attributes.checked = false;
                    if (checkBox != null) {
                        checkBox.indeterminate = true;
                        checkBox.checked = false;
                    }
                }
                else {
                    parentNode.attributes.checked = false;
                    if (checkBox != null) {
                        checkBox.indeterminate = false;
                        checkBox.checked = false;
                    }
                }
                this.SetParentNodeCheckState(parentNode);
            }
        }

        function NodeCheckChange(node, checked) {
            SetChildNodeChecked(node);
            SetParentNodeCheckState(node);
        }

        function ExpandNode(node) {
            SetChildNodeChecked(node);
        }

        //页面加载完成后看是否选中
        function IsChecked(node) {
            if (node.attributes.checked) {
                var child = node.childNodes;
                var childCount = child.length;
                var indeterminate = false;
                var count = 0;
                for (var i = 0; i < childCount; i++) {
                    if (child[i].attributes.checked) {
                        count++; //没有被选中的  
                        // indeterminate = true;
                    }
                }

                if (count < childCount && count != 0) {
                    indeterminate = true;
                }
                else if (count == childCount) {
                    indeterminate = false;
                }
                if (indeterminate) {
                    var checkBox = node.getUI().checkbox;
                    checkBox.indeterminate = indeterminate;
                    checkBox.checked = false;
                }
            }
        }

        //全选
        function selectAll() {
            var roonodes = rmCategoryTree.getRootNode();
            findchildnode(roonodes);
            function findchildnode(node) {
                var childnodes = node.childNodes;
                for (var i = 0; i < childnodes.length; i++) {
                    var rootnode = childnodes[i];
                    if (rootnode.leaf) {
                        rootnode.getUI().toggleCheck(true);
                        rootnode.attributes.checked = true;
                    }
                    if (rootnode.childNodes.length > 0) {
                        findchildnode(rootnode);
                    }
                }
            }
        }

        //全不选
        function noSelect() {
            var nodes = rmCategoryTree.getChecked();
            if (nodes && nodes.length) {
                for (var i = 0; i < nodes.length; i++) {
                    nodes[i].getUI().toggleCheck(false);
                    nodes[i].attributes.checked = false;
                }
            }
        }


        Ext.onReady(function () {
            var tbar = new Ext.Toolbar();
            tbar.add({
                id: 'rMenu1',
                text: '全选',
                tooltip: '全部选择',
                handler: function () {
                    selectAll();
                }

            });
            tbar.add('-');
            tbar.add({
                id: 'rMenu2',
                text: '全不选',
                tooltip: '取消选择状态',
                handler: function () {
                    //事件函数调用
                    noSelect();
                }
            });
            tbar.add('-');
            tbar.add({
                iconCls: 'GTP_allfold', //收缩按钮
                tooltip: '收缩全部',
                handler: function () {
                    rmCategoryTree.collapseAll();
                }
            });
            tbar.add('-');
            tbar.add({
                iconCls: 'GTP_allunfold', //展开按钮
                tooltip: '展开全部',
                handler: function () {
                    rmCategoryTree.expandAll();
                }
            });
            rmCategoryTree = new Ext.tree.TreePanel({
                id: 'treepanel',
                layout: 'fit',
                autoScroll: true,
                height:460,
                width:634,
                rootVisible: true,
                animate: false,
                renderTo: 'body',
                border: false,
                tbar: tbar,
                loader: new Ext.tree.TreeLoader(
                        {
                            dataUrl: '/ShopCategory/SearchDataBL?cid=' + $("#hcid").val()
                        }
                    ),
                root: new Ext.tree.AsyncTreeNode(
                        {
                            text: '分类管理',
                            draggable: false,
                            leaf: false,
                            check: false,
                            expanded: true
                        }
                    ),
                listeners: {
                    "checkchange": function (node, checked) {
                        NodeCheckChange(node);
                    }, "expand": function (node, checked) {
                        ExpandNode(node);
                    }, "load": function (node) {
                        IsChecked(node);
                    }
                  
                }
            });
        }
        );


    </script>
</body>
</html>
