﻿/*
 * Ext JS Library 2.3.0
 * Copyright(c) 2006-2009, Ext JS, LLC.
 * licensing@extjs.com
 * 
 * http://extjs.com/license
 */
// Very simple plugin for adding a close context menu to tabs
Ext.ux.TabCloseMenu = function () {
    var tabs, menu, ctxItem;
    this.init = function (tp) {
        tabs = tp;
        tabs.on('contextmenu', onContextMenu);
    }

    function onContextMenu(ts, item, e) {
        if (!menu) { // create context menu on first right click
            menu = new Ext.menu.Menu([{
                id: tabs.id + '-close',
                text: '关闭选项卡',
                iconCls: 'GTP_sending',
                handler: function () {
                    tabs.remove(ctxItem);
                }
            }, {
                id: tabs.id + '-close-others',
                text: '关闭其他选项卡',
                iconCls: 'GTP_senderror',
                handler: function () {
                    tabs.items.each(function (item) {
                        if (item.closable && item != ctxItem) {
                            tabs.remove(item);
                        }
                    });
                }
            },
            {
                id: tabs.id + '-close-all',
                text: '关闭所有标签',
                iconCls: 'btnno',
                handler: function () {
                    tabs.items.each(function (item) {
                        if (item.closable) {
                            tabs.remove(item);
                        }
                    });
                }
            }
            ]);
        }
        ctxItem = item;
        var items = menu.items;
        items.get(tabs.id + '-close').setDisabled(!item.closable);

        var disableOthers = true;
        tabs.items.each(function () {
            if (this != item && this.closable) {
                disableOthers = false;
                return false;
            }
        });

        items.get(tabs.id + '-close-others').setDisabled(disableOthers);
        menu.showAt(e.getPoint());

        var disableAll = true;
        tabs.items.each(function () {
            if (this.closable) {
                disableAll = false;
                return false;
            }
        });
        items.get(tabs.id + '-close-all').setDisabled(disableAll);
        menu.showAt(e.getPoint());
    }
};