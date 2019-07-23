Ext.apply(Ext.form.VTypes, {
    daterange: function (val, field) {
        var date = field.parseDate(val);

        if (!date) {
            return false;
        }
        if (field.startDateField) {
            var start = Ext.getCmp(field.startDateField);
            if (!start.maxValue || (date.getTime() != start.maxValue.getTime())) {
                start.setMaxValue(date);
                start.validate();
            }
        }
        else if (field.endDateField) {
            var end = Ext.getCmp(field.endDateField);
            if (!end.minValue || (date.getTime() != end.minValue.getTime())) {
                end.setMinValue(date);
                end.validate();
            }
        }
        /*
        * Always return true since we're only using this vtype to set the
        * min/max allowed values (these are tested for after the vtype test)
        */
        return true;
    },
    daterangeTop: function (val, field) {
        var date = field.parseDate(val);
        if (!date) {
            return false;
        }
        if (field.startDateField) {
            var start = top.Ext.getCmp(field.startDateField);
            if (!start.maxValue || (date.getTime() != start.maxValue.getTime())) {
                start.setMaxValue(date);
                start.validate();
            }
        }
        else if (field.endDateField) {
            var end = top.Ext.getCmp(field.endDateField);
            if (!end.minValue || (date.getTime() != end.minValue.getTime())) {
                end.setMinValue(date);
                end.validate();
            }
        }
        /*
        * Always return true since we're only using this vtype to set the
        * min/max allowed values (these are tested for after the vtype test)
        */
        return true;
    },
    myemail: function (val, field) {
        return val == '' ? true : /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/.test(val);
    },
    password: function (val, field) {////val指这里的文本框值，field指这个文本框组件
        if (field.confirmTo) {//confirmTo是我们自定义的配置参数，一般用来保存另外的组件的id值
            var pwd = Ext.get(field.confirmTo); //取得confirmTo指向组件Id的值
            return (val == pwd.getValue());
        }
        return true;
    },
    mobile: function (v) {//规则区号（3-4位数字）-电话号码（7-8位数字）
        // alert(/^(((13[0-9]{1})|159|153)+\d{8})$/.test(v));
        if (v.indexOf('-') > 0) {
            return /^(([0\+]\d{2,3}-)?(0\d{2,3})-)?(\d{7,8})(-(\d{3,}))?$/.test(v);
        } else {
            return /^1[4,3,5,8]\d{9}$/.test(v);
        }
    },
    mobileText: '手机格式不正确',
    money: function (val) {
        return /^([1-9]\d{0,7}|0)(\.\d{1,2})?$/.test(val);
    },
    moneyText: '请输入正确的金额金额' 

});