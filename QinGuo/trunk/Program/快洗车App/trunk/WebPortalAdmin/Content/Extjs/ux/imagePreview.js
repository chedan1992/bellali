/**

* 继承Panel组件，用于图像预览

*

* @author centre

* @class ImgView

* @extends Ext.Panel

*/

ImgView = Ext.extend(Ext.Panel, {

    height: 800,

    width: 800,

    //cls: "background:blue;",

    img_index: 0,

    img_view_id: this.id + '_img',

    /**

    * 设置显示图像的偏移量，根据传入的参数选择应该显示的图片的id

    *

    * @author centre

    * @param {}

    *            offset 偏移量为整数

    */

    setImg: function (offset) {
        if (offset == 1 && Ext.getCmp(this.id + '_next_btn').disabled != true) {

            this.img_index = this.img_index + offset;

        }

        if (offset == -1 && Ext.getCmp(this.id + '_pre_btn').disabled != true) {

            this.img_index = this.img_index + offset;

        }

        if (this.img_index <= 0) {

            Ext.get(this.img_view_id).dom.src = this.src[0];

            // this.img_index =this.src.length-1;

        } else if (this.img_index >= this.src.length) {

            Ext.get(this.img_view_id).dom.src = this.src[this.src.length - 1];

            // this.img_index = 0;

        } else {

            Ext.get(this.img_view_id).dom.src = this.src[this.img_index];

        }

        //控制按钮可用
        if (this.img_index == 0 && this.img_index != this.src.length) {
            //首张
            Ext.getCmp(this.id + '_next_btn').disabled = false; //下一张
            Ext.getCmp(this.id + '_pre_btn').disabled = true; //上一张
        }
        else if (this.img_index < this.src.length) {
            Ext.getCmp(this.id + '_next_btn').disabled = false; //下一张
            Ext.getCmp(this.id + '_pre_btn').disabled = false; //上一张
        }
        else if (this.img_index == this.src.length) {
            Ext.getCmp(this.id + '_next_btn').disabled = true; //下一张
            Ext.getCmp(this.id + '_pre_btn').disabled = false; //上一张
        }
        // this.img_index = this.img_index + offset;

        Ext.get(this.img_view_id).center();
        Ext.get(this.img_view_id).dom.style = "width:95%;height:95%;padding:20px;";
    },

    /**

    * 构造函数，根据传入的json对象初始化组件，新增了一个src的配置参数

    *

    * @author centre

    */

    initComponent: function () {
        var cmp = this;
        var length = this.src.length;
        this.img_index = this.imgindex;
        this.html = '<img id=\'' + this.img_view_id + '\' src=\'' + this.src[this.imgindex] + '\' ></img>';

        this.tbar = [{
            text: "上一张",
            id: this.id + '_pre_btn',
            handler: function (e, b) {
                cmp.setImg(-1);
            }

        }, '-', {
            text: "下一张",
            id: this.id + '_next_btn',
            handler: function (e, b) {
                cmp.setImg(1);
            }
        }];

        if (this.src.length == 0) {
            Ext.getCmp(this.id + '_next_btn').disabled = false; //下一张
            Ext.getCmp(this.id + '_pre_btn').disabled = false; //上一张
        }

        ImgView.superclass.initComponent.call(this);

    },

    /**

    * ImageView渲染后，通过监听图片上的鼠标滚轮事件将图片放大或者缩小

    *

    * @author centre

    * @return null

    */

    afterRender: function () {
        ImgView.superclass.afterRender.call(this);

        Ext.get(this.img_view_id).parent = this;

        Ext.get(this.img_view_id).center();
        new Ext.dd.DD(Ext.get(this.img_view_id), 'pic'); //拖拽
        Ext.get(this.img_view_id).dom.title = '鼠标滚轮可以控制图片的放大和缩小。';
        //Ext.get(this.img_view_id).dom.style = "position: relative; left:0px; top:0px;";
        Ext.get(this.img_view_id).dom.style = "width:95%;height:95%;padding:20px;";
        Ext.get(this.img_view_id).on({
            'dblclick': {
                fn: function () {
                    Ext.get(this).parent.zoom(Ext.get(this), 1.1, true);
                }
            },
            'mousewheel': {
                fn: function (e) {
                    var delta = e.getWheelDelta();
                    if (delta > 0) {
                       // Ext.get(this).parent.zoom(Ext.get(this), 1.1, true);
                    } else {
                       // Ext.get(this).parent.zoom(Ext.get(this), 1.1, false);
                    }
                }
            }

        });

    },

    /**

    * 图片放大和缩小

    *

    * @param {}

    *            el 一个dom对象

    * @param {}

    *            offset 放大或者缩小的偏移量

    * @param {}

    *            type true为放大，false为缩小

    */

    zoom: function (el, offset, type) {
        var width = el.getWidth();

        var height = el.getHeight();

        var nwidth = type ? (width * offset) : (width / offset);

        var nheight = type ? (height * offset) : (height / offset);

        var left = type ? -((nwidth - width) / 2) : ((width - nwidth) / 2);

        var top = type ? -((nheight - height) / 2) : ((height - nheight) / 2);

        el.animate({

            height: {

                to: nheight,

                from: height

            },

            width: {

                to: nwidth,

                from: width

            },

            left: {

                by: left

            },

            top: {

                by: top

            }

        }, null, null, 'backBoth', 'motion');

    }

});