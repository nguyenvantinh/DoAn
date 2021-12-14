//tham số 1: fileUpload or input or this, tham số 2: thẻ img (đang link đến ảnh cũ(preimage))
//function ShowImagePreview(imageUploader, previewImage) {
//    if (imageUploader.files && imageUploader.files[0]) {//kt image muốn up lên có tồn tại
//        var reader = new FileReader();
//        reader.onload = function (e) {
//            $(previewImage).attr('src', e.target.result);//set lại src của thẻ img 
//        }
//        reader.readAsDataURL(imageUploader.files[0]); // đọc nội dung của file dc tải lên vào biến = vs input's name
//    }
//}
function previewFile() {
    const preview = document.getElementById('preview');
    const file = document.querySelector('input[type=file]').files[0];
    const reader = new FileReader();

    reader.addEventListener("load", function () {
        // convert image file to base64 string
        preview.src = reader.result;
    }, false);

    if (file) {//nếu có thẻ input type = file thì / nếu file dc load lên có nội dung
        reader.readAsDataURL(file); //đọc nội dung của file dc tải lên vào biến = vs input's name
    }
}
function JqueryAjaxPost(form) {

    $.validator.unobtrusive.parse(form);
    if ($(form).valid()) {
        var ajaxConfig = {
            url: form.action,
            type: "POST",
            data: new FormData(form),
            success: function (response) {
                if (response.success) {
                    $("#firstTab").html(response.html);
                    refreshAddNewTab($(form).attr('data-restUrl'), true);
                    $.notify(response.message, "success");
                    if (typeof activatejQueryTable !== 'undefined' && $.isFunction(activatejQueryTable))
                        activatejQueryTable();
                }
                else {
                    $.notify(response.message, "error");
                }
            }
        }
        if ($(form).attr('enctype') == 'multipart/form-data') { //nếu form có thuộc tính enctype = ... thì tức là nó có file upload >> phải có contentType và processData
            ajaxConfig["contentType"] = false;
            ajaxConfig["processData"] = false;
        }
        $.ajax(ajaxConfig);
    }
    return false;
}

function refreshAddNewTab(resetUrl, showViewTab) {
    $.ajax({
        type: 'GET',
        url: resetUrl,
        success: function (response) {
            $("#secondTab").html(response);
            $('ul.nav.nav-tabs a:eq(1)').html('Add New');
            if (showViewTab)
                $('ul.nav.nav-tabs a:eq(0)').tab('show');
        }

    });
}

function Edit(url) {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (response) {
            $("#secondTab").html(response);
            $('ul.nav.nav-tabs a:eq(1)').html('Edit');
            $('ul.nav.nav-tabs a:eq(1)').tab('show');
        }

    });
}

function Delete(url) {
    if (confirm('Are you sure to delete this record ?') == true) {
        $.ajax({
            type: 'POST',
            url: url,
            success: function (response) {
                if (response.success) {
                    $("#firstTab").html(response.html);
                    $.notify(response.message, "warn");
                    if (typeof activatejQueryTable !== 'undefined' && $.isFunction(activatejQueryTable))
                        activatejQueryTable();
                }
                else {
                    $.notify(response.message, "error");
                }
            }

        });

    }
}