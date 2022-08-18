//给Jquery 扩展base64转文件下载的功能
jQuery.extend({
    downloadBase64File: function (base64Data, fileName, contentType) {
        var byteCharacters = atob(base64Data);
        var byteNumbers = new Array(byteCharacters.length);
        for (var i = 0; i < byteCharacters.length; i++) {
            byteNumbers[i] = byteCharacters.charCodeAt(i);
        }
        if (!contentType) {
            var extLastIndexOf = fileName.lastIndexOf('.');
            var ext = fileName.substring(extLastIndexOf + 1, fileName.length);//后缀名
            switch (ext.toLowerCase()) {
                case 'jpeg':
                case 'jpg':
                    contentType = 'image/jpeg';
                    break;
                case 'gif':
                    contentType = 'image/gif';
                    break;
                case 'png':
                    contentType = 'image/png';
                    break;
                case 'xls':
                case 'xlm':
                    contentType = 'application/vnd.ms-excel';
                    break;
                case 'xlsx':
                    contentType = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet';
                    break;
                case 'doc':
                    contentType = 'application/msword';
                    break;
                case 'docx':
                    contentType = 'application/vnd.openxmlformats-officedocument.wordprocessingml.document';
                    break;
                case 'pdf':
                    contentType = 'application/pdf';
                    break;
                case 'txt':
                    contentType = 'text/plain';
                    break;
                case 'zip':
                    contentType = 'application/zip';
                    break;
                default:
                    contentType = 'application/octet-stream';
                    break;
            }
        }
        var byteArray = new Uint8Array(byteNumbers);
        var blob = new Blob([byteArray], { type: contentType });
        if (navigator.msSaveOrOpenBlob) {
            navigator.msSaveBlob(blob, fileName);
        } else {
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = fileName;
            link.click();
            window.URL.revokeObjectURL(link.href);
        }
    }
});



//生成GUID
function uuid(len, radix) {
    var chars = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz'.split('');
    var uuid = [], i;
    radix = radix || chars.length;

    if (len) {
        // Compact form
        for (i = 0; i < len; i++) uuid[i] = chars[0 | Math.random() * radix];
    } else {
        // rfc4122, version 4 form
        var r;

        // rfc4122 requires these characters
        uuid[8] = uuid[13] = uuid[18] = uuid[23] = '-';
        uuid[14] = '4';

        // Fill in random data.  At i==19 set the high bits of clock sequence as
        // per rfc4122, sec. 4.1.5
        for (i = 0; i < 36; i++) {
            if (!uuid[i]) {
                r = 0 | Math.random() * 16;
                uuid[i] = chars[(i == 19) ? (r & 0x3) | 0x8 : r];
            }
        }
    }

    return uuid.join('');
}


function ajaxFun(action, data, callback, complete_callback, type, async) {
    if (async==null) {
        async = true;
    }
    $.ajax({
        async:async,
        type: type ? type : 'post',
        dataType: "json",
        contentType: "application/json",
        data: JSON.stringify(data),
        url: action,
        beforeSend: function (xre) {
            vueObj.loading = true;
        },
        complete: function () {
            vueObj.loading = false;
            if (complete_callback)
                complete_callback();
        },
        success: function (json) {
            if (callback)
                callback(json);
        },
        error: function (json) {
            alert("加载失败");
        }
    });

}

//通用导出的方法,调用时请将this修改为Vue对象 (请求回来base64)
function onExport(url, data) {
    var self = this;
    ajaxFun(url, data, function (msg) {
        if (msg.Ok) {
            $.downloadBase64File(msg.Data, uuid(8, 16) + ".xlsx")
        }
        else {
            self.$message.error(msg.Data || '下载出错!');
        }

    });
}

function onAdd() {
    var self = this;
    var em = vue_data.Dialog.EditModel;
    em.title = "添加";
    em.visible = true;
    self.$nextTick(function () {
        self.$refs.editform.resetFields();
    });
}

function onEdit(scope) {
    var rowData = scope.row;
    var self = this;
    self.onAdd();
    self.$nextTick(function () {
        var emf = vue_data.Dialog.EditModel.Form;
        for (var i in emf) {
            if (rowData[i] != null) {
                emf[i] = rowData[i];
            }
        }
    });
}

function onSave(url) {
    var self = this;
    var em = vue_data.Dialog.EditModel;
    em.loading = true;
    ajaxFun(url, [em.Form], function (re) {
        if (re.Ok) {
            self.$message.success("操作成功!");
            self.$refs.table_Main.reload();
        } else {
            self.$message.error(re.Data || "操作出错");
        }
    }, function () {
        em.loading = false;
    });
}


//删除一条数据, 注意主数据表的ref为 table_Main
function onDelete(url, pk) {
    var self = this;
    ajaxFun(url, { pk: pk }, function (msg) {
        self.$message.success("删除成功!");
        self.$refs.table_Main.reload();

    });
}