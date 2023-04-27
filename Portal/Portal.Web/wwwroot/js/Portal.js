var Portal = {};

Portal.SearchInitial = function (controller) {
    //set btn-search event
    $("#btn-search").click(function () {
        Portal.SearchDefault(controller);
    });
    //click btn-search button at first time
    $("#btn-search").trigger("click");
    //set default form submit => click btn-search button
    $("#frmSearch").submit(function (e) {
        e.preventDefault();
        $("#btn-search").trigger("click");
    });
    Portal.Delete();
    Portal.Table();
}
Portal.SearchDefault = function (controller) {
    var $btn = $("#btn-search");
    $.ajax({
        type: "get",
        url: "/" + controller + "/_Search",
        data: $("#frmSearch").serializeArray(),
        success: function (xhr, status, error) {
            if (xhr.Code == 500 || xhr.Success == false) {
               
            }
            else if (xhr.indexOf("from-login-error") > 0) {
                toastr.error('Hết thời gian thao tác xin đăng nhập lại');
                setTimeout(function () {
                    var url = window.location.href.toString().split(window.location.host)[1];
                    window.location.href = "/Permission/Auth/Login?returnUrl=" + url;
                }, 1000);
            }
            else {
                $("#divSearchResult").html(xhr);
                Portal.Table();
            }
        },
        error: function (xhr, status, error) {
            //$("#" + $btn[0].id + " i").toggleClass("d-none")
            //// hiện loading lên
            //$("#" + $btn[0].id + " span").toggleClass("d-none")
            //// disabled button
            //$("#" + $btn[0].id).toggleClass("disabled");
            toastr.error(xhr.responseText);
        }
    });
}

Portal.CreateInitial = function (controller) {
    $(document).on("click", "#btn-save-continue", function (e) {
        var isContinue = true;
        Portal.Create(controller, "#frmCreate", isContinue, this);
    });

    $(document).on("click", "#btn-save", function () {
        var isContinue = false;
        Portal.Create(controller, "#frmCreate", isContinue, this);
    });
}

Portal.Create = function (controller, frmCreate, isContinue, e) {
    var $btn = $(e);
    //var frm = $(frmCreate);
    var frm = $(frmCreate),
        formData = new FormData(),
        formParams = frm.serializeArray();

    if (frm.valid()) {
        $.each(frm.find('input[type="file"]'), function (i, tag) {
            $.each($(tag)[0].files, function (i, file) {
                formData.append(tag.name, file);
            });
        });

        $.each(formParams, function (i, val) {
            formData.append(val.name, val.value);
        });
        console.log(formData.length);
        toastr.clear();
        $.ajax({
            type: "POST",
            url: "/" + controller + "/Create",
            data: formData,
            processData: false,
            contentType: false,
            success: function (jsonData) {

                if (jsonData.success == true) {
                    if (isContinue == true) {
                        frm[0].reset();

                        if (jsonData.data != null) {
                            toastr.success(jsonData.data);
                        }
                    }
                    else {
                        //window.location.href = "/" + controller;
                        window.location.href = "/" + controller + "/index?message=" + jsonData.data;
                    }
                }
                else {
                    if (jsonData.data != null && jsonData.data != "") {
                        toastr.error(jsonData.data);
                    }
                    else if (jsonData.indexOf("from-login-error") > 0) {
                        toastr.error('Hết thời gian thao tác xin đăng nhập lại');
                        setTimeout(function () {
                            var url = window.location.href.toString().split(window.location.host)[1];
                            window.location.href = "/Permission/Auth/Login?returnUrl=" + url;
                        }, 1000);
                    }
                }
            },
            error: function (xhr, status, error) {
                toastr.error(xhr.responseText);
            }
        });
    }
    else {
        var validator = frm.validate();
        $.each(validator.errorMap, function (index, value) {
            console.log('Id: ' + index + ' Message: ' + value);
            //ShowToast.error('Id: ' + index + ' Message: ' + value, 10000);
        });
        $btn.button('reset');
    }
}


Portal.EditInitial = function (controller) {
    $(document).on("click", "#btn-save-continue", function (e) {
        var isContinue = true;
        Portal.Edit(controller, "#frmEdit", isContinue, this);
    });

    $(document).on("click", "#btn-save", function () {
        var isContinue = false;
        Portal.Edit(controller, "#frmEdit", isContinue, this);
    });
}

Portal.Edit = function (controller, frmEdit, isContinue, e) {
    var $btn = $(e);
    //var frm = $(frmCreate);
    var frm = $(frmEdit),
        formData = new FormData(),
        formParams = frm.serializeArray();

    if (frm.valid()) {
        $.each(frm.find('input[type="file"]'), function (i, tag) {
            $.each($(tag)[0].files, function (i, file) {
                formData.append(tag.name, file);
            });
        });

        $.each(formParams, function (i, val) {
            formData.append(val.name, val.value);
        });
        toastr.clear();
        $.ajax({
            type: "POST",
            url: "/" + controller + "/Edit",
            data: formData,
            processData: false,
            contentType: false,
            success: function (jsonData) {

                if (jsonData.success == true) {
                    if (isContinue == true) {
                        frm[0].reset();

                        if (jsonData.data != null) {
                            toastr.success(jsonData.data);
                        }
                    }
                    else {
                        //window.location.href = "/" + controller;
                        window.location.href = "/" + controller + "/index?message=" + jsonData.data;
                    }
                }
                else {
                    if (jsonData.data != null && jsonData.data != "") {
                        toastr.error(jsonData.data);
                    }
                    else if (jsonData.indexOf("from-login-error") > 0) {
                        toastr.error('Hết thời gian thao tác xin đăng nhập lại');
                        setTimeout(function () {
                            var url = window.location.href.toString().split(window.location.host)[1];
                            window.location.href = "/Permission/Auth/Login?returnUrl=" + url;
                        }, 1000);
                    }
                }
            },
            error: function (xhr, status, error) {
                toastr.error(xhr.responseText);
            }
        });
    }
    else {
        var validator = frm.validate();
        $.each(validator.errorMap, function (index, value) {
            console.log('Id: ' + index + ' Message: ' + value);
            //ShowToast.error('Id: ' + index + ' Message: ' + value, 10000);
        });
        $btn.button('reset');
    }
}

Portal.UpsertInitial = function (controller) {
    $(document).on("click", "#btn-save-continue", function (e) {
        var isContinue = true;
        Portal.Upsert(controller, "#frmUpsert", isContinue, this);
    });

    $(document).on("click", "#btn-save", function () {
        var isContinue = false;
        Portal.Upsert(controller, "#frmUpsert", isContinue, this);
    });
}

Portal.Upsert = function (controller, frmUpsert, isContinue, e) {
    var $btn = $(e);
    //var frm = $(frmCreate);
    var frm = $(frmUpsert),
        formData = new FormData(),
        formParams = frm.serializeArray();

    if (frm.valid()) {
        $.each(frm.find('input[type="file"]'), function (i, tag) {
            $.each($(tag)[0].files, function (i, file) {
                formData.append(tag.name, file);
            });
        });

        $.each(formParams, function (i, val) {
            formData.append(val.name, val.value);
        });
        toastr.clear();
        $.ajax({
            type: "POST",
            url: "/" + controller + "/Upsert",
            data: formData,
            processData: false,
            contentType: false,
            success: function (jsonData) {
               
                if (jsonData.success== true) {
                    if (isContinue == true) {
                        frm[0].reset();

                        if (jsonData.data != null) {
                            toastr.success(jsonData.data);
                        }
                    }
                    else {
                        //window.location.href = "/" + controller;
                        window.location.href = "/" + controller + "/index?message=" + jsonData.data;
                    }
                }
                else {
                    if (jsonData.data != null && jsonData.data != "") {
                        toastr.error(jsonData.data);
                    }
                    else if (jsonData.indexOf("from-login-error") > 0) {
                        toastr.error('Hết thời gian thao tác xin đăng nhập lại');
                        setTimeout(function () {
                            var url = window.location.href.toString().split(window.location.host)[1];
                            window.location.href = "/Permission/Auth/Login?returnUrl=" + url;
                        }, 1000);
                    }
                }
            },
            error: function (xhr, status, error) {
                toastr.error(xhr.responseText);
            }
        });
    }
    else {
        var validator = frm.validate();
        $.each(validator.errorMap, function (index, value) {
            console.log('Id: ' + index + ' Message: ' + value);
            //ShowToast.error('Id: ' + index + ' Message: ' + value, 10000);
        });
        $btn.button('reset');
    }
}

Portal.Delete = function () {
    $(document).on("click", "#btn-delete", function () {
        let $btn = $(this);;
        let controller = $btn.data("current-url");
        let itemName = $btn.data("item-name");
        let id = $btn.data("id");
        let textRequestion = "Bạn có muốn xóa thông tin <b> " + itemName + "</b> đang chọn?";
        Swal.fire({
            title: 'Xóa thông tin',
            html: textRequestion,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Có',
            cancelButtonText: "Không"
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: "/" + controller + "/Delete",
                    type: 'DELETE',
                    data: {id:id},
                    success: function (data) {
                        toastr.clear();
                        if (data.success) {
                            $("#btn-search").trigger("click");
                            toastr.success(data.data);
                        }
                        else {
                            toastr.error(data.data);
                        }
                    },
                    error: function (xhr, status, data) {

                        toastr.error(data);
                    }
                })
            }
        })
    });
}

Portal.Table = function () {
    
    try {
        $('#data-list').DataTable({
            responsive: false,
            autoWidth: true,
            autoHeight: false,
            scrollCollapse: false,
            scrollX: true,
            scrollY: 250,
            iDisplayLength: 10,
            buttons: ['excel', 'pdf',],
            language: {
                emptyTable: "Không có dữ liệu",
                search: ""
            }
        });
    } catch (e) {
        console.log(e);
    }
};
//get message from current url
Portal.GetQueryString = function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, '\\$&');
    var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, ' '));
}
Portal.ShowMessage = function (url) {
    var text = Portal.GetQueryString("message", url);
    if (text != null) {
        //alertPopup(true, text);
        toastr.info(text);
        history.pushState(null, '', window.location.href.split("?")[0]);
    }
}