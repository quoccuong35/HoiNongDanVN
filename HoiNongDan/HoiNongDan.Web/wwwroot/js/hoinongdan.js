
var HoiNongDan = {};

HoiNongDan.SearchInitial = function (controller, action = "_Search") {
    //set btn-search event
    $("#btn-search").click(function () {
        HoiNongDan.SearchDefault(controller, action);
    });
    //click btn-search button at first time
    $("#btn-search").trigger("click");
    //set default form submit => click btn-search button
    $("#frmSearch").submit(function (e) {
        e.preventDefault();
        $("#btn-search").trigger("click");
    });
    HoiNongDan.Delete();
    HoiNongDan.Table();
    HoiNongDan.Import(controller);
    HoiNongDan.UploadFile(controller);
    HoiNongDan.ImportModalHideHandler();
    HoiNongDan.ExportData(controller);
  
}
HoiNongDan.SearchDefault = function (controller, action) {
    var $btn = $("#btn-search");
    $btn.toggleClass("btn-loading");
    $.ajax({
        type: "get",
        url: "/" + controller + "/" + action,
        data: $("#frmSearch").serializeArray(),
        success: function (xhr, status, error) {
           
            if (xhr.code == 500 || xhr.success == false) {
                toastr.error(xhr.data);
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
                HoiNongDan.Table();
                $btn.toggleClass("btn-loading");
            }
        },
        error: function (xhr, status, error) {
            $btn.toggleClass("btn-loading");
            Exceptions(xhr, status, error);
           
        }
    });
}
HoiNongDan.SetDataSearch = function (data) {
    $("#divSearchResult").html(data);
    HoiNongDan.Table();
   
}

HoiNongDan.CreateInitial = function (controller) {
    $(document).on("click", "#btn-save-continue", function (e) {
        var isContinue = true;
        HoiNongDan.Create(controller, "#frmCreate", isContinue, this);
    });

    $(document).on("click", "#btn-save", function () {
        var isContinue = false;
        HoiNongDan.Create(controller, "#frmCreate", isContinue, this);
    });
    HoiNongDan.SearchNhanSu();
    HoiNongDan.SearchHoiVien();
    HoiNongDan.HoiVienShowModal();
}

HoiNongDan.Create = function (controller, frmCreate, isContinue, e) {
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
                Exceptions(xhr, status, error);
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
HoiNongDan.EditInitial = function (controller) {
    $(document).on("click", "#btn-save-continue", function (e) {
        var isContinue = true;
        HoiNongDan.Edit(controller, "#frmEdit", isContinue, this);
    });

    $(document).on("click", "#btn-save", function () {
        var isContinue = false;
        HoiNongDan.Edit(controller, "#frmEdit", isContinue, this);
    });
    HoiNongDan.DeleteFileDinhKem();
    HoiNongDan.DeleteEdit();
    HoiNongDan.HoiVienShowModal();
}
HoiNongDan.Edit = function (controller, frmEdit, isContinue, e) {
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
                Exceptions(xhr, status, error);
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

HoiNongDan.UpsertInitial = function (controller) {
    $(document).on("click", "#btn-save-continue", function (e) {
        var isContinue = true;
        HoiNongDan.Upsert(controller, "#frmUpsert", isContinue, this);
    });

    $(document).on("click", "#btn-save", function () {
        var isContinue = false;
        HoiNongDan.Upsert(controller, "#frmUpsert", isContinue, this);
    });
}
HoiNongDan.Upsert = function (controller, frmUpsert, isContinue, e) {
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
                Exceptions(xhr, status, error);
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
HoiNongDan.Delete = function () {
    $(document).on("click", "#btn-delete", function () {
        let $btn = $(this);
        let controller = $btn.data("current-url");
        let itemName = $btn.data("item-name");
        let id = $btn.data("id");
        let textRequestion = "Bạn có muốn xóa thông tin <b> " + itemName + "</b> đang chọn?";
      /*  let token = $('input[name="__RequestVerificationToken"]').val();*/
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
                var token = $("input[name='__RequestVerificationToken']").val();
                $.ajax({
                    url: "/" + controller + "/Delete",
                    data: { id: id, '__RequestVerificationToken': token },
                    type: 'DELETE',
                    
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
                    error: function (xhr, status, error) {
                        Exceptions(xhr, status, error);
                    }
                })
            }
        })
    });
}
HoiNongDan.DeleteEdit = function () {
    $(document).on("click", "#btn-delete", function () {
        let $btn = $(this);
        let controller = $btn.data("current-url");
        let itemName = $btn.data("item-name");
        let id = $btn.data("id");
        let textRequestion = "Bạn có muốn xóa thông tin <b> " + itemName + "</b> đang chọn?";
        /*  let token = $('input[name="__RequestVerificationToken"]').val();*/
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
                var token = $("input[name='__RequestVerificationToken']").val();
                $.ajax({
                    url: "/" + controller + "/Delete",
                    data: { id: id, '__RequestVerificationToken': token },
                    type: 'DELETE',
                    success: function (data) {
                        toastr.clear();
                        if (data.success) {
                            toastr.success(data.data);
                            setTimeout(function () {
                                window.location.href ='/'+ controller +"/Index";
                            }, 1000);
                        }
                        else {
                            toastr.error(data.data);
                        }
                    },
                    error: function (xhr, status, error) {
                        Exceptions(xhr, status, error);
                    }
                })
            }
        })
    });
}
HoiNongDan.DuyetHoiVienInitial = function (controller) {
    $(document).on("click", ".duyet-hoivien", function (e) {
        let $btn = $(this);
        let id = $btn.data("id");
        var lid = [];
        lid.push(id);
        var formData = {};
        formData.lid = lid;
        HoiNongDan.DuyetHoiVien(formData, controller);
    });

    $(document).on("click", "#duyet-hoivienall", function () {
        var lid = [];
        $('#data-list tbody tr').each(function () {
            lid.push(this.id);
        });
        var formData = {};
        formData.lid = lid;
        HoiNongDan.DuyetHoiVien(formData, controller);
    });
    $(document).on("click", ".btn-reject", function (e) {
        let $btn = $(this);
        let id = $btn.data("id");
        var lid = [];
        lid.push(id);
        var formData = {};
        formData.lid = lid;
        HoiNongDan.TuChoiDuyetHoiVien(formData, controller);
    });
}
HoiNongDan.DuyetHoiVien = function (data, controller)
{
    let textRequestion = "Bạn có muốn duyệt các hội viên đang chọn?";
    Swal.fire({
        title: 'Duyệt hội viên mới',
        html: textRequestion,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Có',
        cancelButtonText: "Không"
    }).then((result) => {
        if (result.isConfirmed) {
            var token = $("input[name='__RequestVerificationToken']").val();
            data.__RequestVerificationToken = token;
            $.ajax({
                url: "/" + controller + "/Edit",
                type: 'POST',
                data: data,
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
}

HoiNongDan.TuChoiDuyetHoiVien = function (data, controller) {
    var lydo = "";
    Swal.fire({
        title: "Lý do từ chối",
        input: "text",
        showCancelButton: true,
        confirmButtonText: "Từ chối",
        cancelButtonText: "Hủy",
        showLoaderOnConfirm: true,
        preConfirm: async (value) => {
            try {
                lydo = value;
            } catch (error) {
            }
        },
    }).then((result) => {
        if (result.isConfirmed) {
            if (lydo == null || lydo == "" || lydo === undefined) {
                toastr.error('Chưa nhập lý do');
                return;
            }
            data.lyDo = lydo;
            data.__RequestVerificationToken = $("input[name='__RequestVerificationToken']").val();
            $.ajax({
                url: "/" + controller + "/TuChoi",
                type: 'POST',
                data: data,
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
    });
}
HoiNongDan.Import = function (controller) {
    $(document).on("click", ".btn-import", function () {
        $.ajax({
            type: "get",
            url: "/" + controller + "/_Import",
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
                   
                    $("#iframe-import").html(xhr);
                    $('#modalImport').modal("show");
                }
            },
            error: function (xhr, status, error) {
                Exceptions(xhr, status, error);
            }
        });
       
    });
  
}
HoiNongDan.UploadFile = function (controller) {
    $(document).on("click", "#btn-importExcel", function () {
        var frm = $("#frmImport");
        var formData = new FormData();
        formParams = frm.serializeArray();
        var $btn = $("#btn-importExcel");
        $btn.toggleClass("btn-loading");
        $.each(frm.find('input[type="file"]'), function (i, tag) {
            $.each($(tag)[0].files, function (i, file) {
                formData.append(tag.name, file);
            });
        });
        $.each(formParams, function (i, val) {
            formData.append(val.name, val.value);
        });
        console.log(formData);
        $.ajax({
            type: "POST",
            url: "/" + controller + "/Import",
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (jsonData) {
                $btn.toggleClass("btn-loading");
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
                    $btn.toggleClass("btn-loading");
                    toastr.error('Hết thời gian thao tác xin đăng nhập lại');
                    setTimeout(function () {
                        var url = window.location.href.toString().split(window.location.host)[1];
                        window.location.href = "/Permission/Auth/Login?returnUrl=" + url;
                    }, 1000);
                }
            },
            error: function (xhr, status, error) {
                Exceptions(xhr, status, error);
                $btn.toggleClass("btn-loading");
            }
        });
    });
}
HoiNongDan.ImportModalHideHandler = function () {
    $('#modalImport').on('hidden.bs.modal', function (e) {
        /*document.parentElement.getElementById("importexcelfile").value = "";*/
        $("#iframe-import").html("");
    })
}
HoiNongDan.ExportData = function (controller) {
    $(document).on("click", ".btn-exporttoexcel", function () {
        var formParams = $("#frmSearch").serializeArray();
        var para = "";
        $.each(formParams, function (i, val) {
            if (val.value) {
                if (para == "") {
                    para = para + val.name + "=" + val.value;
                }
                else {
                    para = para +"&"+ val.name + "=" + val.value;
                }
               
            }
        });
        self.location = "/" + controller + "/ExportEdit?" + para;
    });
}
HoiNongDan.Table = function (id_table = "#data-list") {   
    var height = document.documentElement.clientHeight*0.4;
    try {
        var table = $(id_table).DataTable({
            autoFill: true,
            responsive: false,
            autoWidth: false,
            autoHeight: false,
            scrollCollapse: false,
            scrollX: true,
/*            scrollY: height,*/
            iDisplayLength: 10,
            order: [],
            buttons: ['excel'],
            //buttons: ['colvis'],
            language: {
                emptyTable: "Không có dữ liệu",
                search: ""
            }
        });
        table.buttons().container()
            .appendTo(id_table + '_wrapper .col-md-6:eq(0)');
        HideShowColumns(id_table);
      /*  table.column(0).visible(false);*/
    } catch (e) {
        console.log(e);
    }
};
HoiNongDan.Selected = function (id) {
    $('#'+id).on('click', 'tr', function () {
        $(this).toggleClass('selected');
        //var pos = oTable.row(this).index();
        //var row = oTable.row(pos).data();
        //console.log(row);
        //console.log(oTable);
    });
}
//get message from current url
HoiNongDan.GetQueryString = function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, '\\$&');
    var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, ' '));
}
HoiNongDan.ShowMessage = function (url) {
    var text = HoiNongDan.GetQueryString("message", url);
    if (text != null) {
        //alertPopup(true, text);
        toastr.info(text);
        history.pushState(null, '', window.location.href.split("?")[0]);
    }
}

HoiNongDan.SearchNhanSu = function () {
    $(document).on("click", "#btn-search-nhansu", function () {
        var maNhanSu = $("#txt-MaCanBo").val();
        let $btn = $(this);
        var canBo = $btn.data("canbo");;
       
        $.ajax({
            type: "GET",
            url: "/NhanSu/NhanSuThongTin/SearchNhanSu",
            data: { maNhanSu: maNhanSu, canBo: canBo },
            success: function (jsonData) {
                $("#Part_ThongTinNhanSu").html(jsonData);
            },
            error: function (xhr, status, error) {
                Exceptions(xhr, status, error);
            }
        });
    });
}

HoiNongDan.SearchHoiVien = function () {
    $(document).on("click", "#btn-search-hoivien", function () {
        var mahoivien = $("#txt-mahoivien").val();
        $.ajax({
            type: "GET",
            url: "/HoiVien/HVInfo/SearchHV",
            data: { maHV: mahoivien},
            success: function (jsonData) {
                $("#part-hoivien").html(jsonData);
               
            },
            error: function (xhr, status, error) {
                Exceptions(xhr, status, error);
            }
        });
    });
}

HoiNongDan.HoiVienShowModal = function (controller) {
    HoiNongDan.HoiVienSearch();
    HoiNongDan.ChonHoiViens();
    HoiNongDan.ChonHoiVien();
    HoiNongDan.ChonNhanhHV();
    $(document).on("click", ".btn-hoivien-modal", function () {
        $.ajax({
            type: "get",
            url: "/HoiVien/HVInfo/_HoiVien",
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
                    $("#modalHoiVien").appendTo("body");
                    $("#container-hoivien").html(xhr);
                    $('#modalHoiVien').modal("show");
                }
            },
            error: function (xhr, status, error) {
                Exceptions(xhr, status, error);
            }
        });

    });

}
HoiNongDan.HoiVienSearch = function () {
    HoiNongDan.CheckedAll();
    $(document).on("click", "#btn-searchhoivien", function () {
        $.ajax({
            type: "get",
            url: "/HoiVien/HVInfo/_HoiVienSearch",
            data: $("#frmSearchHoiVien").serializeArray(),
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
                    $("#divDSSearchHoiVien").html(xhr);
                    $('#divDSSearchHoiVien>#list-dshoivien').dataTable();
                }
            },
            error: function (xhr, status, error) {
                Exceptions(xhr, status, error);
            }
        });

    });

}

HoiNongDan.CheckedAll = function () {
    $(document).on("change", "#checkAll", function (e) {
       
        const items = document.querySelectorAll('#divDSSearchHoiVien input[name="chk-item[]"]');
        for (let i = 0; i < items.length; i++) {
            items[i].checked = this.checked;
            var tr = items[i].parentNode.parentNode;
            var id = items[i].getAttribute('id')
            if (this.checked) {
                HoiNongDan.InsertRowTable(tr, id);
            }
            else {
                HoiNongDan.RemoveRowTable(id);
            }
        }
        HoiVienDaChon();
        //if (checkAll.checked) {
        //    $("#selected-info").html("Đã chọn" + items.length)
        //}
        //else {
        //    $("#selected-info").html("Chưa chọn")
        //}
        
    });
}
function HoiVienDaChon() {
    let so_item_chon = document.querySelectorAll("#tbodyDSChonHoiVien>tr").length;
    $("#selected-info").html("Đã chọn " + so_item_chon)
}
HoiNongDan.ChonHoiViens = function () {
    $(document).on("click", "#btn-chon-hv", function (e) {
       
        var data_selected = document.querySelectorAll("#tbodyDSChonHoiVien>tr"); 
        var data = document.querySelectorAll("#tbodyHoiViens>tr");
        var cout = data.length;
        for (var i = 0; i < data_selected.length; i++) {
            data
            var table = document.getElementById("list-dshoivien").getElementsByTagName('tbody')[0];
            var row = table.insertRow(0);
            let chon = row.insertCell(0);
            let maNV = row.insertCell(1);
            let hoten = row.insertCell(2);
            let diaban = row.insertCell(3);
            let ngaysinh = row.insertCell(4);
            let socccd = row.insertCell(5);
            let hokhau = row.insertCell(6);
            chon.innerHTML = "<input type ='checkbox' name = 'HoiViens[" + cout + "].Chon' value='true' checked>";
            maNV.innerHTML = data_selected[i].children[0].innerText + "<input type='text' hidden name = 'HoiViens[" + cout + "].IdCanbo' value='" + data_selected[i].children[6].innerText.toString().trim() + "' >";
            hoten.innerHTML = data_selected[i].children[1].innerText + "<input type='text' hidden name = 'HoiViens[" + cout + "].HoVaTen' value='" + data_selected[i].children[1].innerText.toString().trim() + "' >";
            diaban.innerText = data_selected[i].children[2].innerText 
            ngaysinh.innerText = data_selected[i].children[3].innerText 
            socccd.innerText = data_selected[i].children[4].innerText 
            hokhau.innerText = data_selected[i].children[5].innerText 
            cout++;
        }
        $('#modalHoiVien').modal("hide");
    });
}
HoiNongDan.ChonNhanhHV = function () {
    // khi double click
    $(document).on("dblclick", "#list-dshoivien>tbody>tr", function (e) {
        document.getElementById("HoiVien-IdCanbo").value = this.children[0].children[0].getAttribute("id");

        document.getElementById("MaCanBo").value = this.children[2].innerText;
        document.getElementById("txt-mahoivien").value = this.children[2].innerText;

        document.getElementById("HoVaTen").value = this.children[3].innerText;
        document.getElementById("txt-HoVaTen").value = this.children[3].innerText;

        document.getElementById("SoCCCD").value = this.children[6].innerText;
        document.getElementById("txt-SoCCCD").value = this.children[6].innerText;

        document.getElementById("DiaBan").value = this.children[4].innerText;
        document.getElementById("txt-DiaBan").value = this.children[4].innerText;

        document.getElementById("HoKhauThuongTru").value = this.children[7].innerText;
        document.getElementById("txt-HoKhauThuongTru").value = this.children[7].innerText;

        document.getElementById("NgaySinh").value = this.children[5].innerText;
        document.getElementById("txt-NgaySinh").value = this.children[5].innerText;

        $('#modalHoiVien').modal("hide");
    });
}
HoiNongDan.ChonHoiVien = function () {
    $(document).on("click", 'input[name="chk-item[]"]', function (e) {
        let id = this.getAttribute('id');
        let tr = document.getElementById(id).parentElement.parentElement;
        if (this.checked) {
            HoiNongDan.InsertRowTable(tr, id);
        }
        else {
            HoiNongDan.RemoveRowTable(id);
        }
        HoiVienDaChon();
    });
}

HoiNongDan.InsertRowTable = function (tr, id) {
    var table = document.getElementById("divDSChonHoiVien").getElementsByTagName('tbody')[0];
    
    var row = table.insertRow(0);
    row.id = "tr_"+id;
    let manv = row.insertCell(0);
    let hoten = row.insertCell(1);
    let diaban = row.insertCell(2);
    let ngaysinh = row.insertCell(3);
    let socccd = row.insertCell(4);
    let hokhau = row.insertCell(5);
    let idCanBo = row.insertCell(6);
  
    manv.innerText = tr.children[2].innerText;
    hoten.innerText = tr.children[3].innerText;
    diaban.innerText = tr.children[4].innerText;
    ngaysinh.innerText = tr.children[5].innerText;
    socccd.innerText = tr.children[6].innerText;
    hokhau.innerText = tr.children[7].innerText;
    idCanBo.innerText = id;
}
HoiNongDan.RemoveRowTable = function (rowid) {
    var tr = document.getElementById("tr_"+rowid);
    tr.parentNode.removeChild(tr);
}
HoiNongDan.DeleteFileDinhKem = function (controller) {
    $(document).on("click", ".btn-deletefile", function () {
        let $btn = $(this);
        let id = $btn.data("id");
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
                var token = $("input[name='__RequestVerificationToken']").val();
                $.ajax({
                    url: "/NhanSu/FileDinhKem/DeleteFile",
                    data: { id: id, '__RequestVerificationToken': token },
                    type: 'DELETE',
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
                        Exceptions(xhr, status, error);
                    }
                });
            }
        })
        
    });
}