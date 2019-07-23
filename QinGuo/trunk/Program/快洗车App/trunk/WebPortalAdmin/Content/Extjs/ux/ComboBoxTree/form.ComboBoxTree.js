Ext.ns('Ext.ux.form');
/**
 * customized field for ComboBox dropdown tree
 * 
 * @class Ext.ux.form.ComboBoxTree
 * @extends Ext.form.TwinTriggerField
 */
Ext.ux.form.ComboBoxTree = Ext.extend(Ext.form.TwinTriggerField,{

	//list dropdown Fx,boolean or object
	animate : {easing:'easeIn',duration:0.75},

	defaultAutoCreate : {tag: "input", type: "text", size: "24", autocomplete: "off"},
	
	editable : false,
	
	//true to display the clear value trigger, default to false
	enableClearValue : false,
	
	//this hiddenField's init value if exist
	hiddenValue : '',
	
	//this field's display init value
	displayValue : '',
	
	//dropdown list's width,default to this field's width
	listWidth : undefined,
	
	//dropdown list's height,if tree's height was speciallized ignore this
	listHeight : undefined,
	
	maxListHeight : 225,
	
	minListWidth : 70,
	
	handleHeight : 8,
	
	listClass : '',
	
	selectedClass : 'x-combo-selected',
	
	shadow : 'sides',
	
	listAlign : 'tl-bl?',
	
	listEmptyText : '',
	
	resizable : false,
	
	trigger1Class : 'x-form-clear-trigger',
	
	hideTrigger1 : true,
	
    trigger2Class : 'x-form-arrow-trigger',
	
	initComponent : function(){
	
		this.addEvents(
			/**
             * @event expand
             * Fires when the dropdown list is expanded
             * @param {Ext.ux.form.ComboBoxTree}
             */
			'expand',
			
			/**
             * @event collapse
             * Fires when the dropdown list is collapsed
             * @param {Ext.ux.form.ComboBoxTree}
             */
			'collapse',
			
			/**
             * @event treenodeselect
             * Fires when the tree node is selected
             * @param {Ext.ux.form.ComboBoxTree}
             * @param {Ext.tree.TreeNode}
             * @param {Ext.EventObject}
             */
			'treenodeselect',
			
			/**
             * @event beforecollapse
             * Fires when the dropdown list before collapsed
             * @param {Ext.ux.form.ComboBoxTree}
             */
			'beforecollapse',
			
			/**
             * @event beforeexpand
             * Fires when the dropdown list before expanded
             * @param {Ext.ux.form.ComboBoxTree}
             */
			'beforeexpand',
			
			/**
             * @event clearvalue
             * Fires when the clear value trigger is clicked
             * @param {Ext.ux.form.ComboBoxTree}
             * @param {String} the value will be cleared
             */
			'clearvalue'
		);
		
		Ext.ux.form.ComboBoxTree.superclass.initComponent.call(this);
		
		//user both do not specialized treePanel'height and comboboxtree's listHeight,it's means dropdown list auto fit height
    	this.tree.height = Ext.isDefined(this.tree.height) ? this.tree.height : this.listHeight;
    	
        //disabled autoHeight
        this.tree.autoHeight = false;
        
        //always autoScroll = true
        this.tree.autoScroll = true;
        
        this.tree.containerScroll = true;
        
		this.tree.border = false;
		
		//if this.tree.height == undefined ,force to expand tree'root if user do not specialized expanded = true
		this.tree.root.expanded = !Ext.isDefined(this.tree.height) ? true : this.tree.root.expanded;
		
		this.tplId = Ext.id();
		
		this.tpl = '<div id="'+this.tplId+'"></div>';
		
		this.onTrigger2Click = this.onTriggerClick;
        
        this.onTrigger1Click = this.clearValue;
		
	},
	
	//private
	onRender : function(ct, position){
		
		if(this.hiddenName && !Ext.isDefined(this.submitValue)){
            this.submitValue = false;
        }
		
		Ext.ux.form.ComboBoxTree.superclass.onRender.call(this,ct,position);

		if(this.hiddenName){
			var formItem = this.el.findParent('.x-form-item',4,true);
			if(formItem){
				//create a Ext.form.Hidden field as a sibling of this, in order to Ext.form.BasicForm.load(..)
				this.hiddenField = formItem.insertSibling({tag:'input', type:'hidden', name: this.hiddenName,
						id: (this.hiddenId||this.hiddenName)}, 'after', true);	
				var hf = new Ext.form.Hidden({applyTo:this.hiddenField});
			
				var basicForm = this.getOwnerForm(this);
				if(basicForm){
					basicForm.add(hf);
				}
			}else{
				this.hiddenField = this.el.insertSibling({tag:'input', type:'hidden', name: this.hiddenName,
                    id: (this.hiddenId||this.hiddenName)}, 'before', true);
			}
        }
	
        if(Ext.isGecko){
            this.el.dom.setAttribute('autocomplete', 'off');
        }
		
        this.initList();
	},
	
	//private
	initList : function(){
		if(!this.list){
			var cls = 'x-combo-list',
                listParent = Ext.getDom(this.getListParent() || Ext.getBody()),
                zindex = parseInt(Ext.fly(listParent).getStyle('z-index'), 10);
	
            if (!zindex) {
                zindex = this.getParentZIndex();
            }

            this.list = new Ext.Layer({
                parentEl: listParent,
                shadow: this.shadow,
                cls: [cls, this.listClass].join(' '),
                constrain:false,
                zindex: (zindex || 12000) + 5
            });
			
			var lw = this.listWidth || Math.max(this.wrap.getWidth(), this.minListWidth);
            this.list.setSize(lw, 0);
			
			//specialized the tree's width for scroll -x
			this.tree.width = lw - 1;
			
            this.list.swallowEvent('mousewheel');
			this.assetHeight = 0;
			if(this.syncFont !== false){
                this.list.setStyle('font-size', this.el.getStyle('font-size'));
            }
            if(this.title){
                this.header = this.list.createChild({cls:cls+'-hd', html: this.title});
                this.assetHeight += this.header.getHeight();
            }
			
			this.innerList = this.list.createChild({cls:cls+'-inner'});
			this.innerList.setWidth(lw - this.list.getFrameWidth('lr'));
			
			this.view = new Ext.DataView({
                applyTo: this.innerList,
                tpl: this.tpl,
                singleSelect: true,
                selectedClass: this.selectedClass,
                itemSelector: this.itemSelector || '.' + cls + '-item',
                emptyText: this.listEmptyText,
                deferEmptyText: false,
				listeners: {
					'afterrender' : function(){
						this.tpl.overwrite(this.el,[]);
					}
				}
            });
			
			this.mon(this.view, {
                containerclick : this.onViewClick,
                click : this.onViewClick,
                scope :this
            });
			
            if(this.resizable){
                this.resizer = new Ext.Resizable(this.list,  {
                   pinned:true, handles:'se'
                });
                this.mon(this.resizer, 'resize', function(r, w, h){
                    this.maxHeight = h-this.handleHeight-this.list.getFrameWidth('tb')-this.assetHeight;
                    this.listWidth = w;
                    this.innerList.setWidth(w - this.list.getFrameWidth('lr'));
                    this.restrictHeight();
                }, this);
            }
		}
	},
	
	//private
	initEvents : function(){
	
		Ext.ux.form.ComboBoxTree.superclass.initEvents.call(this);
		
		this.tree.on({
			scope:this,
    		'render' : function(){
    			this.tree.getTreeEl().on('click',function(e,t,o){
    				if(e.getTarget('.x-tree-ec-icon', 1)){//is click tree's collapse/expand trigger icon area?
    					this.isTreeNodeClicked = false;
    				}else{
    					this.isTreeNodeClicked = true;
    				}
    			},this);
    		},
    		'click' : function(n,e){
    			if(this.enableClearValue){
    				this.triggers[0].show();
    			}
				this.setValue(n.text);
				if(this.hiddenField){
					this.setHiddenValue(Ext.value(n.id,''));
				}
    			this.fireEvent('treenodeselect',this,n,e);
    		}
		});
		
		this.tree.root.on({
			scope:this,
			'expand' : {
				fn : function(){
					if(!Ext.isDefined(this.tree.height)){//dropdown list auto height
						this.restrictHeight();
					}
				},
				single: true
			}
		});
		
		this.on({
			'beforecollapse' : {
				fn : function() {
					if(!this.isExpanded()){
			            return;
			        }
					return this.isTreeNodeClicked;
				}
			},
			'beforedestroy' : {
	            fn : function() {
	                this.purgeListeners();
	                this.tree.purgeListeners();   
	            }   
	        },
			'beforeexpand' : {
	            fn : function() {
					if(this.tree.rendered && !this.tree.getRootNode().isExpanded()){
	                	this.tree.getRootNode().expand(false,true);
	                } 
	            },   
	            single : true
	        },
			'focus' : {
				fn : function(){
					if (!this.tree.rendered && this.tplId) {
						this.tree.render(this.tplId);
	                }
				},
				single : true
			}
		});
	},
	
	// private
    initValue : function(){
        Ext.ux.form.ComboBoxTree.superclass.initValue.call(this);
        if(this.hiddenField){
            this.setHiddenValue(Ext.value(Ext.isDefined(this.hiddenValue) ? this.hiddenValue : this.value, ''));
        }
    },
	
	//private, to get this field's owner BasicForm
	getOwnerForm : function(c){
		var formPanel = this.findDirectFormOwnerCt(c);
		if(formPanel && formPanel.getForm()){
			return formPanel.getForm();
		}else{
			return null;
		}
	},
	
	//private, to find this field's direct formPanel ownerCt
	findDirectFormOwnerCt : function(c){
		if(c.ownerCt){
			if(c.ownerCt.getXType() == 'form'){
				return c.ownerCt;
			}else{
				return this.findDirectFormOwnerCt(c.ownerCt);
			}
		}else{
			return null;
		}
	},
	
	//private
	getListParent : function(){
		return document.body;
	},
	
	getParentZIndex : function(){
        var zindex;
        if (this.ownerCt){
            this.findParentBy(function(ct){
                zindex = parseInt(ct.getPositionEl().getStyle('z-index'), 10);
                return !!zindex;
            });
        }
        return zindex;
    },
	
	// private
    restrictHeight : function(){
        this.innerList.dom.style.height = '';
        var inner = this.innerList.dom,
            pad = this.list.getFrameWidth('tb') + (this.resizable ? this.handleHeight : 0) + this.assetHeight,
            h = Math.max(inner.clientHeight, inner.offsetHeight, inner.scrollHeight),
            ha = this.getPosition()[1]-Ext.getBody().getScroll().top,
            hb = Ext.lib.Dom.getViewHeight()-ha-this.getSize().height,
            space = Math.max(ha, hb, this.minHeight || 0)-this.list.shadowOffset-pad;

        h = Math.min(h, space, this.maxListHeight);
	
        this.innerList.setHeight(h);
        this.list.beginUpdate();
        this.list.setHeight(h+pad);
        this.list.alignTo.apply(this.list, [this.el].concat(this.listAlign));
        this.list.endUpdate();
    },
	
	//private
	isExpanded : function(){
        return this.list && this.list.isVisible();
    },
	
	//private
	onViewClick : function(doFocus){
		var index = this.view.getSelectedIndexes()[0];
		if(!index){
            this.collapse();
        }
        if(doFocus == true){
            this.el.focus();
        }
	},
	
	// private
    onDestroy : function(){
        Ext.destroy(
            this.resizer,
            this.view,
            this.list
        );
        Ext.destroyMembers(this, 'hiddenField');
        Ext.ux.form.ComboBoxTree.superclass.onDestroy.call(this);
    },
	
    //override
    setValue : function(val){
    	val = Ext.value(val,'');
    	if(val != ''){
    		if(this.enableClearValue){
				//this.triggers[0].show();
			}
    	}else{
    		if(this.enableClearValue){
				//this.triggers[0].hide();
			}
    	}
    	Ext.ux.form.ComboBoxTree.superclass.setValue.call(this,val);
    },
    
    //private
    setHiddenValue : function(val){
    	val = Ext.value(val,'');
    	if(val != ''){
    		if(this.enableClearValue){
				//this.triggers[0].show();
			}
    	}else{
    		if(this.enableClearValue){
				//this.triggers[0].hide();
			}
    	}
    	if(this.hiddenField){
    		this.hiddenField.value = val;
			this.value = val;
    	}
    	return this;
    },
    
    //override
    reset : function(){
        Ext.ux.form.ComboBoxTree.superclass.reset.call(this);
        if(this.hiddenField){
            this.setHiddenValue('');
        }
    },
    
	//clear value by click clear value trigger
	clearValue : function(){
		if(this.enableClearValue){
			var v = this.value;
			if(this.hiddenField){
				v = this.hiddenField.value;
				this.hiddenField.value = '';
			}
			this.setValue('');
			this.validate();
			this.fireEvent('clearvalue', this, v);
		}
    },
	
	getTree : function(){
		return this.tree;
	},
	
	//override
    getValue : function(){
        if(this.hiddenField){
            return Ext.isDefined(this.value) ? this.value : '';
        }else{
            return Ext.form.ComboBox.superclass.getValue.call(this);
        }
    },
	
	collapse : function(){
		if(!this.isExpanded()){
			return;
		}
		
		if(this.enableClearValue && Ext.value(this.value,'') != ''){
			this.triggers[0].show();
		}
		
		if(this.fireEvent('beforecollapse',this) !== false){
	        this.list.hide();
	        Ext.getDoc().un('mousewheel', this.collapseIf, this);
	        Ext.getDoc().un('mousedown', this.collapseIf, this);
	        this.fireEvent('collapse', this);
    	}
    	
    },
	
    expand : function(){
		if(this.isExpanded() || !this.hasFocus){
			return;
		}
        if(this.fireEvent('beforeexpand', this) !== false){

	        this.list.alignTo.apply(this.list, [this.el].concat(this.listAlign));
	
	        // zindex can change, re-check it and set it if necessary
	        var listParent = Ext.getDom(this.getListParent() || Ext.getBody()),
	            zindex = parseInt(Ext.fly(listParent).getStyle('z-index') ,10);
	        if (!zindex){
	            zindex = this.getParentZIndex();
	        }
	        if (zindex) {
	            this.list.setZIndex(zindex + 5);
	        }
	        
	        this.triggers[0].hide();

	        this.list.show(this.animate);
	        
	        if(Ext.isGecko2){
	            this.innerList.setOverflow('auto'); // necessary for FF 2.0/Mac
	        }
	        this.mon(Ext.getDoc(), {
	            scope: this,
	            mousewheel: this.collapseIf,
	            mousedown: this.collapseIf
	        });
	        
	        this.fireEvent('expand', this);
        }
    },
	
    // private
    collapseIf : function(e){
        if(this.isExpanded() && !this.isDestroyed && !e.within(this.wrap) && !e.within(this.list)){
			this.isTreeNodeClicked = true;
            this.collapse();
        }
    },
	
	onTriggerClick : function(e){
		if(e.getTarget('#' + this.el.id, 1)){
			return;
		}
		if(this.readOnly || this.disabled){
            return;
        }
        if(this.isExpanded()){
            this.collapse();
            this.el.focus();
        }else {
			this.onFocus({});
            this.expand();
			this.restrictHeight();
			this.el.focus();
        }
	},
	
	//override, to prevent trigger hidden when set readOnly = true
    updateEditState: function(){
        if(this.rendered){
            if (this.readOnly) {
                this.el.dom.readOnly = true;
                this.el.addClass('x-trigger-noedit');
                this.mun(this.el, 'click', this.onTriggerClick, this);
            } else {
                if (!this.editable) {
                    this.el.dom.readOnly = true;
                    this.el.addClass('x-trigger-noedit');
                    this.mon(this.el, 'click', this.onTriggerClick, this);
                } else {
                    this.el.dom.readOnly = false;
                    this.el.removeClass('x-trigger-noedit');
                    this.mun(this.el, 'click', this.onTriggerClick, this);
                }
                this.trigger.setDisplayed(!this.hideTrigger);
            }
            this.onResize(this.width || this.wrap.getWidth());
        }
    },

    getSelectedNode:function(){
         var nodeId=this.getValue();
            var selectedNode=null;
            if(nodeId!=""){
            selectedNode=this.tree.getNodeById(nodeId);
            } 
            return selectedNode;
            },

    setSelectedNode:function(nodeId){
        var selectedNode=this.tree.getNodeById(nodeId);
        if(selectedNode)
        {
            selectedNode.select();
        } 
    }

});

Ext.reg('xcomboboxtree',Ext.ux.form.ComboBoxTree);