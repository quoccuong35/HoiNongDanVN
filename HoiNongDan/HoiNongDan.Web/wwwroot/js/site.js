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
$('.btn-userinfo').on('click', function () {
    let $btn = $(this);
    var id = $btn.data("id");
    $.ajax({
        type: "get",
        url: "/Permission/AccountInfo/Edit",
        data: { id: id },
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

        }
    });
});
$('.btn-doimatkhau').on('click', function () {
    var id = $("#AccountId").val();
    var passWord = $("#txt-password").val();
    if (!passWord) {
        toastr.info("Bạn chưa nhập thông tin mật khẩu");
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
            $.ajax({
                url: "/Permission/AccountInfo/DoiMatKhau",
                type: 'post',
                data: { id: id, passWord: passWord },
                success: function (data) {
                    toastr.clear();
                    if (data.success) {
                        toastr.success(data.data);
                        setTimeout(function () {
                            window.location.href = "/Permission/Auth/LogOut";
                        }, 1000);
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