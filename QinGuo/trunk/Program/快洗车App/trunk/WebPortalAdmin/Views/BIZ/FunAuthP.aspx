<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<html>
<head>
    <title>权限配置</title>
    <script type="text/javascript">
        var rmtreeFun;

        //设置节点子级全部选中
        function SetChildNodeCheckedFun(node) {
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
                    this.SetChildNodeCheckedFun(child);
                }
            }
        }

        //设置节点父节点选中状态
        function SetParentNodeCheckStateFun(node) {
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

        function NodeCheckChangeFun(node, checked) {
            SetChildNodeCheckedFun(node);
            SetParentNodeCheckStateFun(node);
        }

        function ExpandNodeFun(node) {
            SetChildNodeCheckedFun(node);
        }

        //页面加载完成后看是否选中
        function IsCheckedFun(node) {
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
        function selectAllFun() {
            var roonodes = rmtreeFun.getRootNode();
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
        function noSelectFun() {
            var nodes = rmtreeFun.getChecked();
            if (nodes && nodes.length) {
                for (var i = 0; i < nodes.length; i++) {
                    nodes[i].getUI().toggleCheck(false);
                    nodes[i].attributes.checked = false;
                }
            }
        }


        Ext.onReady(function () {
            //            var grid = Ext.getCmp("gg");
            //            //得到选后的数据   
            //            var rows = grid.getSelectionModel().getSelections();
            var key = $("#idHid").val();
            var tbar = new Ext.Toolbar();
            tbar.add({
                xtype: 'textfield',
                emptyText: '查找菜单....',
                id: 'keyfield',
                enableKeyEvents: true,
                listeners: {
                    //                    'keyup': function (val) {
                    //                        if (val.getValue()) {
                    //                            rmtreeFun.loader.dataUrl = "/RoleManage/funTree?TypeID=" + $("#attrVal").val() + "&key=" + key + "&name=" + encodeURIComponent(val.getValue().trim());
                    //                        } else {
                    //                            rmtreeFun.loader.dataUrl = "/RoleManage/funTree?TypeID=" + $("#attrVal").val() + "&key=" + key;
                    //                        }
                    //                        rmtreeFun.root.reload();
                    //                    }
                    'keyup': function (node, event) {
                        findByKeyWordFiler(node, event);
                    }
                }
            });
            tbar.add('-');
            tbar.add({
                id: 'rMenu1Fun',
                text: '全选',
                tooltip: '全部选择',
                handler: function () {
                    //事件函数调用
                    selectAllFun();
                }

            });
            tbar.add('-');
            tbar.add({
                id: 'rMenu2Fun',
                text: '全不选',
                tooltip: '取消选择状态',
                handler: function () {
                    //事件函数调用
                    noSelectFun();
                }
            });
            tbar.add('-');
            tbar.add({
                iconCls: 'GTP_allfold', //收缩按钮
                tooltip: '收缩全部',
                handler: function () {
                    rmtreeFun.collapseAll();
                }
            });
            tbar.add('-');
            tbar.add({
                iconCls: 'GTP_allunfold', //展开按钮
                tooltip: '展开全部',
                handler: function () {
                    rmtreeFun.expandAll();
                }
            });

            rmtreeFun = new Ext.tree.TreePanel({
                id: 'treepanelFun',
                layout: 'fit',
                autoScroll: true,
                height:465,
                width:634,
                rootVisible: true,
                animate: false,
                renderTo: 'divrmtreeFun',
                border: false,
                tbar: tbar,
                loader: new Ext.tree.TreeLoader(
                            {
                                dataUrl: '/RoleManage/funTree',
                                baseParams: { TypeID: $("#attrVal").val(), key: key} //用户类型 角色编号
                            }
                        ),
                root: new Ext.tree.AsyncTreeNode(
                            {
                                text: '权限配置',
                                draggable: false,
                                leaf: false,
                                check: false,
                                expanded: true
                            }
                        ),

                listeners: {
                    "checkchange": function (node, flag) {
                        NodeCheckChangeFun(node);
                    },
                    "expand": function (node, checked) {
                        ExpandNodeFun(node);
                    },
                    "load": function (node) {
                        IsCheckedFun(node);
                    }
                }
            });


            rmtreeFun.getRootNode().collapse(true);
        });

        var timeOutId = null;

        var treeFilter = new Ext.tree.TreeFilter(rmtreeFun, {
            clearBlank: true,
            autoClear: true
        });

        // 保存上次隐藏的空节点  
        var hiddenPkgs = [];
        var findByKeyWordFiler = function (node, event) {
            clearTimeout(timeOutId); // 清除timeOutId  
            rmtreeFun.expandAll(); // 展开树节点  
            // 为了避免重复的访问后台，给服务器造成的压力，采用timeOutId进行控制，如果采用treeFilter也可以造成重复的keyup  
            timeOutId = setTimeout(function () {
                // 获取输入框的值  
                var text = node.getValue();
                // 根据输入制作一个正则表达式，'i'代表不区分大小写  
                var re = new RegExp(Ext.escapeRe(text), 'i');
                // 先要显示上次隐藏掉的节点  
                Ext.each(hiddenPkgs, function (n) {
                    n.ui.show();
                });
                hiddenPkgs = [];
                if (text != "") {
                    treeFilter.filterBy(function (n) {
                        // 只过滤叶子节点，这样省去枝干被过滤的时候，底下的叶子都无法显示  
                        return !n.isLeaf() || re.test(n.text);
                    });
                    // 如果这个节点不是叶子，而且下面没有子节点，就应该隐藏掉  
                    rmtreeFun.root.cascade(function (n) {
                        if (n.id != '0') {
                            if (!n.isLeaf() && judge(n, re) == false && !re.test(n.text)) {
                                hiddenPkgs.push(n);
                                n.ui.hide();
                            }
                        }
                    });
                } else {
                    treeFilter.clear();
                    return;
                }
            }, 500);
        }
        // 过滤不匹配的非叶子节点或者是叶子节点  
        var judge = function (n, re) {
            var str = false;
            n.cascade(function (n1) {
                if (n1.isLeaf()) {
                    if (re.test(n1.text)) { str = true; return; }
                } else {
                    if (re.test(n1.text)) { str = true; return; }
                }
            });
            return str;
        };  
    </script>
</head>
<body>
    <input type="hidden" id="hid_roleid" value='' />
    <input type="hidden" id="attrVal" value='<%= ViewData["key"] %>' />
    <input type="hidden" id="idHid" value='<%= ViewData["id"] %>' />
    <div id="divrmtreeFun" style="margin: 0px">
    </div>
</body>
</html>
