$(document).ready(function () {
    var array = window.location.href.split('/');
    var activePage = "";
    var view = "";
    for (var i = 3; i < array.length; i++) {
        view = array[i];
        if (view.toLowerCase() == "edit" || view.toLowerCase() == "create" || view.toLowerCase() == "view" || view.toLowerCase() == "index" || view.toLowerCase() == "upsert") {
            break;
        }
        activePage = activePage + "/" + array[i];
    }
    $(".app-sidebar li a").each(function () {
        var $this = $(this);
        var pageUrl = $this.attr("href");
        if (activePage == pageUrl) {
            $(this).addClass("active");
            $(this).parent().addClass("is-expanded");
            $(this).parent().parent().prev().addClass("active");
            $(this).parent().parent().addClass("open");
            $(this).parent().parent().prev().addClass("is-expanded");
            $(this).parent().parent().parent().addClass("is-expanded");
            $(this).parent().parent().parent().parent().addClass("open");
            $(this).parent().parent().parent().parent().prev().addClass("active");
            $(this).parent().parent().parent().parent().parent().addClass("is-expanded");
            $(this).parent().parent().parent().parent().parent().prev().addClass("active");
            $(this).parent().parent().parent().parent().parent().prev().parent().addClass("is-expanded");
        }
    });
});

let splitter = document.getElementsByClassName("splitter-head");
for (const sp of splitter) {
    sp.addEventListener('click', function handleClick() {
        let parent = sp.closest(".splitter").children[1].classList.toggle("splitter-toggle");
        // sp.closest(".splitter").requestFullscreen();
    });
}
let currency = document.querySelectorAll('input[data-type="currency"]');
for (const item of currency) {
    item.addEventListener("focus", (event) => {
        let value = item.value;
        let temp = value.replaceAll(",", "");
        item.value = value == "" ? null : Number.parseFloat(temp);
        item.setAttribute("step", "500000");
        item.setAttribute("type", "number");
    });
    item.addEventListener("blur", (event) => {
        item.setAttribute("type", "text")
        item.removeAttribute("step");
        let value = item.value;
        //console.log(value);
        item.value = value == "" ? "" : (Number.parseInt(value)).toLocaleString("en");
    });
}
 AjaxGet = function (link, data,func) {
    $.ajax({
        type: "get",
        url: link,
        data: data,
        success: function (xhr, status, error) {
            if (xhr.Code == 500 || xhr.Success == false) {

            }
            else if (xhr.indexOf("from-login-error") > 0) {
                toastr.error('Hết thời gian thao tác xin đăng nhập lại');
                setTimeout(function () {
                    var url = window.location.href.toString().split(window.location.host)[1];
                    window.location.href = "/Permission/Auth/Login?returnUrl=" + link;
                }, 1000);
            }
            else {
                func(xhr);
            }
        },
        error: function (xhr, status, error) {
            Exceptions
                (xhr, status, error)
        }
    });
}
AjaxUpdate = function (link, controller, formData, isContinue, func) {
    $.ajax({
        type: "POST",
        url: link,
        data: formData,
        processData: false,
        contentType: false,
        success: function (jsonData) {

            func({ link: controller, jsonData: jsonData, isContinue: isContinue });
        },
        error: function (xhr, status, error) {
            Exceptions
                (xhr, status, error)
        }
    });
}
$('.btn-userinfo').on('click', function () {
    let $btn = $(this);
    $.ajax({
        type: "GET",
        url: "/Permission/AccountInfo/Edit",
       /* data: { id: id },*/
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

                $("#content-infouser").html(xhr);
                $('#modal-userinfo').modal("show");
            }
        },
        error: function (xhr, status, error) {
            Exceptions(xhr, status, error);
        }
    });
});
$('.btn-doimatkhau').on('click', function () {
    if (!$("#txt-passwordold").val() || !$("#txt-passwordnew").val()) {
        toastr.error('Vui lòng nhâp mật khẩu cũ và mật khẩu mới');
        return;
    }
    Swal.fire({
        title: 'Đổi mật khẩu',
        html: "Bạn có muốn đổi thông tin mật khẩu. Có để tiếp tục",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Có',
        cancelButtonText: "Không"
    }).then((result) => {
        if (result.isConfirmed) {
            var frm = $('#frmDoiThongTinMatKhau'),
                formData = new FormData(),
                formParams = frm.serializeArray();
            $.each(frm.find('input[type="file"]'), function (i, tag) {
                $.each($(tag)[0].files, function (i, file) {
                    formData.append(tag.name, file);
                });
            });

            $.each(formParams, function (i, val) {
                formData.append(val.name, val.value);
            });
            $.ajax({
                type: "POST",
                url: "/Permission/AccountInfo/DoiMatKhau",
                data: formData,
                processData: false,
                contentType: false,
                success: function (jsonData) {
                    toastr.clear();
                    if (jsonData.success) {
                        toastr.success(jsonData.data);
                        setTimeout(function () {
                            window.location.href = "/Permission/Auth/LogOut";
                        }, 1000);
                    }
                    else {
                        toastr.error(jsonData.data);
                    }
                },
                error: function (xhr, status, error) {
                    Exceptions
                    (xhr, status, error)
                }
            });
        }
    });

});
/// ẩn hiện cột table columns
var dataTable;
function HideShowColumns(id) {
    dataTable = $(id).DataTable();
    var header = dataTable.columns().header();
    var html = "";
/*    html += '<li role="menuitem"><button class="ui-grid-menu-item"><i class="fa fa-save"></i> Lưu</button></li>';*/
    let gridName = $('.ui-grid-menu-items').attr('data-gridname');
    let localstorageGrid = JSON.parse(localStorage.getItem(gridName));
    let check = 'fa fa-check', uncheck = 'fa fa-close';
    for (var i = 0; i < header.length; i++) {
        if (header[i].innerHTML != '' && header[i].innerHTML != null) {
            if (localstorageGrid != null) {
                var check_hide_column = localstorageGrid.filter((item) => {
                    return item == i;
                });
                if (check_hide_column.length > 0) {
                    html += ' <li role="menuitem"><button class="ui-grid-menu-item" data-cv-idx="' + i + '" ><i class="' + uncheck + '"></i> ' + header[i].innerHTML + '</button></li>';
                    dataTable.column(check_hide_column).visible(false);
                }
                else {
                    html += ' <li role="menuitem"><button class="ui-grid-menu-item" data-cv-idx="' + i + '" ><i class="' + check + '"></i> ' + header[i].innerHTML + '</button></li>';
                }
            }
            else {
                html += ' <li role="menuitem"><button class="ui-grid-menu-item" data-cv-idx="' + i + '" ><i class="' + check + '"></i> ' + header[i].innerHTML + '</button></li>';
            }
        }
    }
    $('.ui-grid-menu-items').html('');
    $('.ui-grid-menu-items').append(html);
}
$(document).on("click", ".ui-grid-icon-container", function () {
    $('.ui-grid-menu').toggleClass('show');
});
$(document).on("click", "li[role='menuitem']>button.ui-grid-menu-item", function () {
    var $btn = $(this);
    let check = 'fa fa-check', uncheck = 'fa fa-close';
    let icon = $btn.children()[0].classList.value;
    let index = $btn.data("cv-idx");
    let gridName = $('.ui-grid-menu-items').attr('data-gridname');
    let localstorageGrid = JSON.parse(localStorage.getItem(gridName));
    if (localstorageGrid == null) {
        localstorageGrid = [];
    }
    if (icon == check) {
        // chuyen thanh ko hien thi
        localstorageGrid.push(index);
        localStorage.setItem(gridName, JSON.stringify(localstorageGrid));
        $btn.children()[0].classList = uncheck;
        dataTable.column(index).visible(false);
    }
    else {
        localstorageGrid = localstorageGrid.filter((item) => {
            return item != index;
        });
        localStorage.setItem(gridName, JSON.stringify(localstorageGrid));
        $btn.children()[0].classList = check;
        dataTable.column(index).visible(true);
    }
});
function Exceptions(xhr, status, error) {
    if (xhr.status == 401) {
        toastr.error('Hết thời gian thao tác xin đăng nhập lại');
        setTimeout(function () {
            var url = window.location.href.toString().split(window.location.host)[1];
            window.location.href = "/Permission/Auth/Login?returnUrl=" + url;
        }, 1000);
    }
    else if (xhr.status == 404) {
        setTimeout(function () {
            var url = window.location.href.toString().split(window.location.host)[1];
            window.location.href = "/Error/ErrorNotFound?returnUrl=" + url;
        }, 1000);
    }
    else if (xhr.status == 400) {
        toastr.error("Yêu cầu không hợp lệ");
    }
    else {
        toastr.error(error);
    }
}