﻿var Portal = {};

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
    Portal.UploadFile(controller);
    Portal.ImportModalHideHandler();
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
    Portal.SearchNhanSu();
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

                        if (jsonData.data != null) {
                            toastr.success(jsonData.data);
                            setTimeout(function () {
                                window.location.href = window.location.href = "/" + controller + "/Create";
                            }, 2000);
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
    Portal.DeleteFileDinhKem();
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
                       
                        if (jsonData.data != null) {
                            toastr.success(jsonData.data);

                            setTimeout(function () {
                                window.location.href = window.location.href = "/" + controller +"/Create";
                            }, 2000);
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
        let $btn = $(this);
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

Portal.UploadFile = function (controller) {
    $(document).on("click", "#btn-importExcel", function () {
        var file = document.getElementById('importexcelfile').files[0];
        if (file == null)
        {
            toastr.error('Chưa chọn file dữ liệu');
            return;
        }
        var formData = new FormData();
        formData.append("importexcelfile", file);
        $.ajax({
            type: "POST",
            url: "/" + controller + "/Import",
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (jsonData) {
                if (jsonData.success == true) {
                    //formData[0].reset();
                    let html = '<div class="alert alert-success">' +
                        '<button type="button" class="btn-close" data-bs-dismiss="alert" aria-hidden="true">×</button>' +
                        ' <span class=""><svg xmlns="http://www.w3.org/2000/svg" height="40" width="40" viewBox="0 0 24 24"><path fill="#13bfa6" d="M10.3125,16.09375a.99676.99676,0,0,1-.707-.293L6.793,12.98828A.99989.99989,0,0,1,8.207,11.57422l2.10547,2.10547L15.793,8.19922A.99989.99989,0,0,1,17.207,9.61328l-6.1875,6.1875A.99676.99676,0,0,1,10.3125,16.09375Z" opacity=".99"></path><path fill="#71d8c9" d="M12,2A10,10,0,1,0,22,12,10.01146,10.01146,0,0,0,12,2Zm5.207,7.61328-6.1875,6.1875a.99963.99963,0,0,1-1.41406,0L6.793,12.98828A.99989.99989,0,0,1,8.207,11.57422l2.10547,2.10547L15.793,8.19922A.99989.99989,0,0,1,17.207,9.61328Z"></path></svg></span>' +
                        '<strong>Thành công</strong>' +
                        '<hr class="message-inner-separator">' +
                        '<p>' + jsonData.data + '.</p>' +
                        '</div>'
                    $('#import-result').append(html);
                                
                    setTimeout(function () {
                        $("#importexcelfile").val("");
                        $('#import-result').html("");
                    }, 3000);
                }
                else if (jsonData.success == false) {
                    //formData[0].reset();
                    let html = '<div class="alert alert-danger">'+
                       '<button type="button" class="btn-close" data-bs-dismiss="alert" aria-hidden="true">×</button>'+
                        '<span class=""><svg xmlns="http://www.w3.org/2000/svg" height="40" width="40" viewBox="0 0 24 24"><path fill="#f07f8f" d="M20.05713,22H3.94287A3.02288,3.02288,0,0,1,1.3252,17.46631L9.38232,3.51123a3.02272,3.02272,0,0,1,5.23536,0L22.6748,17.46631A3.02288,3.02288,0,0,1,20.05713,22Z"></path><circle cx="12" cy="17" r="1" fill="#e62a45"></circle><path fill="#e62a45" d="M12,14a1,1,0,0,1-1-1V9a1,1,0,0,1,2,0v4A1,1,0,0,1,12,14Z"></path></svg></span>'+
                         '<strong>Lỗi</strong>'+
                         '<hr class="message-inner-separator">'+
                        '<p>' + jsonData.data +'.</p>' +
                        '</div>'
                    $('#import-result').append(html);
                    setTimeout(function () {
                        $("#importexcelfile").val("");

                    }, 3000);
                }
                else if (jsonData.indexOf("from-login-error") > 0) {
                    toastr.error('Hết thời gian thao tác xin đăng nhập lại');
                    setTimeout(function () {
                        var url = window.location.href.toString().split(window.location.host)[1];
                        window.location.href = "/Permission/Auth/Login?returnUrl=" + url;
                    }, 1000);
                }
            },
            error: function (xhr, status, error) {
                toastr.error(xhr.responseText);
            }
        });
    });
}
Portal.ImportModalHideHandler = function () {
    $('#modalImport').on('hidden.bs.modal', function (e) {
        document.getElementById("importexcelfile").value = "";
    })
}
Portal.Table = function () {   
    try {
        var table = $('#data-list').DataTable({
            autoFill: true,
            responsive: false,
            autoWidth: true,
            autoHeight: false,
            scrollCollapse: false,
            scrollX: true,
            scrollY: 300,
            iDisplayLength: 10,
            buttons: ['excel', 'pdf'],
            language: {
                emptyTable: "Không có dữ liệu",
                search: ""
            }
        });
        table.buttons().container()
            .appendTo('#data-list_wrapper .col-md-6:eq(0)');
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

Portal.SearchNhanSu = function () {
    $(document).on("click", "#btn-search-nhansu", function () {
        var maNhanSu = $("#txt-MaCanBo").val();
        var canBo = $("#txt-CanBo").val();
        formData = new FormData();
        formData.append("maNhanSu", maNhanSu);
        formData.append("canBo", canBo);
        $.ajax({
            type: "POST",
            url: "/NhanSu/NhanSuThongTin/SearchNhanSu",
            data: formData,
            processData: false,
            contentType: false,
            success: function (jsonData) {
                $("#Part_ThongTinNhanSu").html(jsonData);
            },
            error: function (xhr, status, error) {
            }
        });
    });
}

Portal.DeleteFileDinhKem = function (controller) {
    $(document).on("click", ".btn-deletefile", function () {
        let $btn = $(this);
        let id = $btn.data("id");
        formData = new FormData();
        formData.append("id", id);
        Swal.fire({
            title: 'Xóa thông tin',
            html: "Bạn có muốn xóa file đang chọn",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Có',
            cancelButtonText: "Không"
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: "POST",
                    url: "/NhanSu/FileDinhKem/DeleteFile",
                    data: formData,
                    processData: false,
                    contentType: false,
                    success: function (jsonData) {

                        if (jsonData.success == true) {
                            if (jsonData.data != null) {
                                toastr.success(jsonData.data);
                                var del_html = $("#" + id);
                                if (del_html != null) {
                                    del_html.remove();
                                }
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
                    }
                });
            }
        })
        
    });
}