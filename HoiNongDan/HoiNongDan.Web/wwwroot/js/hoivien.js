
var table;
var MaCanBo = "", HoVaTen = "", MaQuanHuyen = "", MaDiaBanHoiVien = "";
//load default and set event
$(function () {
    LoadThongTin();
  
    $('#dt-hoivien tbody').on('dblclick', 'tr', function () {
        var row = table.row(this).data();
        let link = '/HoiVien/HoiVien/Edit/' + row['idCanBo'];
        window.open(link, '_blank').focus();
    });
});
$('.btn-print').on('click', function () {
    var lid = [];
    $('#data-list tbody tr.selected').each(function () {
        lid.push(this.id);
    });
    var formData = {};
    formData.lid = lid;
    if (lid.length == 0) {
        toastr.info('Chưa chọn thông tin hội viên cần in thẻ');
        return;
    }
    ShowInTheHoiVien(formData);
});

$("select#MaDiaBanHoiVien").change(function () {
    let maHoiNongDan = $(this).val();
    //console.log(maQuanHuyen);
    $("select#MaChiHoi").empty();
    $.getJSON('/HoiVien/HoiVien/loadChiHoi?ma=' + maHoiNongDan, function (data) {
        $("select#MaChiHoi").append('<option>' + "--Vui lòng chọn danh mục--" + '</option>');
        $.each(data, function (i, item) {
            /*console.log(item);*/
            $("select#MaChiHoi").append('<option value=' + item.maChiHoi + '>' + item.name + '</option>');
        });
    });
})
function ShowInTheHoiVien(formData) {
    toastr.clear();
    $.ajax({
        type: "POST",
        url: "/HoiVien/HoiVien/Print",
        data: formData,
        success: function (jsonData) {
            if (jsonData.success == true) {
                window.open("/HoiVien/HoiVien/ShowInTheHoiVien", "_blank");
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

function LoadThongTin() {
    CheckSearchChange();
    var $btn = $("#btn-searchhv");
    $btn.toggleClass("btn-loading");
    $.ajax({
        type: "get",
        url: "/HoiVien/HoiVien/_Search",
        data: $("#frmSearch").serializeArray(),
        success: function (res, status, error) {
            table = $('#dt-hoivien').DataTable({
                data: res.data,
                autoFill: true,
                responsive: false,
                autoWidth: false,
                autoHeight: false,
                scrollCollapse: false,
                scrollX: true,
                destroy: true,
                /*            scrollY: height,*/
                iDisplayLength: 10,
                order: [],
                //buttons: ['excel', 'pdf'],
                language: {
                    emptyTable: "Không có dữ liệu",
                    search: ""
                },
                columns: [
                    { data: null },
                    { data: 'tenDiaBanHoatDong' },
                    {
                        data: "maCanBo"
                        //render: function (data, type, row) {
                        //    let link = '/HoiVien/HoiVien/Edit/' + row['idCanBo'];
                        //    return '<a href="' + link + '" target="_blank" >' + data + '</a>';
                        //}
                    },
                    { data: "hoVaTen" },
                    { data: "ngaySinh" },
                    { data: "gioiTinh" },
                    { data: "soCCCD" },
                    { data: "hoKhauThuongTru" },
                    { data: "choOHienNay" },
                    { data: "soDienThoai" },
                    { data: "dangVien" },
                    { data: "ngayVaoDangChinhThuc" },
                    { data: "danToc" },
                    { data: "tonGiao" },
                    { data: "trinhDoHocvan" },
                    { data: "maTrinhDoChuyenMon" },
                    { data: "maTrinhDoChinhTri" },
                    { data: "ngayVaoHoi" },
                    { data: "ngayThamGiaCapUyDang" },
                    { data: "ngayThamGiaHDND" },
                    { data: "vaiTro" },
                    { data: "vaiTroKhac" },
                    { data: "giaDinhThuocDien" },
                    { data: "giaDinhThuocDienKhac" },
                    { data: "ngheNghiepHienNay" },
                    { data: "loai_DV_SX_ChN" },
                    { data: "soLuong" },
                    { data: "dienTich_QuyMo" },
                    { data: "thamGia_SH_DoanThe_HoiDoanKhac" },
                    { data: "thamGia_SH_DoanThe_HoiDoanKhac" },
                    { data: "thamGia_CLB_DN_MH_HTX_THT" },
                    { data: "thamGia_THNN_CHNN" },
                    { data: "hoiVienNongCot" },
                    { data: "hoiVienUuTuNam" },
                    { data: "hoiVienDanhDu" },
                    { data: "ndsxkdg" },
                    { data: "ndTieuBieu" },
                    { data: "ndVietnamXS" },
                    { data: "kncgcnd" },
                    { data: "canBoHoiCoSoGioi" },
                    { data: "sangTaoNhaNong" },
                    { data: "guongDiemHinh" },
                    { data: "guongDanVanKheo" },
                    { data: "guongDiemHinhHocTapLamTheoBac" },
                    { data: "sanPhamNongNghiepTieuBieu_OCOP" },
                    { data: "hoTrovayVon" },
                    { data: "hoTroKhac" },
                    { data: "hoTroDaoTaoNghe" },
                    { data: "ghiChu" },

                ]
            });
            table.on('order.dt search.dt', function () {
                table.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
            }).draw();
            var btn_loaddata = $("#btn-loadthem");
            let total = Number(res.total);
            if (total > 1000) {
                btn_loaddata.html("Hiển thị thêm<br> 1,000/" + total.toLocaleString('hi-IN', { minimumFractionDigits: 0 }));
                btn_loaddata.css("display", "block");
                btn_loaddata.attr("data-total", total);
                $("#txt-noteload").css("display", "block");
            }
            else {
                btn_loaddata.css("display", "none");
                $("#txt-noteload").css("display", "none");
            }
            $btn.toggleClass("btn-loading");
            HideShowColumns('#dt-hoivien');
            var phuongXa = document.getElementById("TenPhuongXa");
            if (phuongXa.value != "") {
                phuongXa.value = "";
            }
        },
        error: function (xhr, status, error) {
            $btn.toggleClass("btn-loading");
            Exceptions(xhr, status, error);
        }
    });
}
function AddButtonLoad() {
    const pagination = document.getElementById("dt-hoivien_paginate");
    const btn = document.getElementById("btn-loadthem");
    if (pagination != null && btn == null) {
        const new_bttom = document.createElement("button");
        new_bttom.style = "display:block";
        new_bttom.textContent = "texthu"
        new_bttom.setAttribute("class", "btn btn-sm btn-primary ms-1");
        new_bttom.setAttribute("data-total", "0");
        new_bttom.setAttribute("onclick", "LoadThem()");
        new_bttom.setAttribute("id", "btn-loadthem");
        //new_bttom.css("btn btn-sm btn-primary");
        pagination.appendChild(new_bttom);
    }
}
function LoadThem() {
    var btn_loaddata = $("#btn-loadthem");
    var count = table.data().count();
    let _total = Number(btn_loaddata.data("total"));
    if (count == _total)
        return;
    btn_loaddata.text("Đang tải dữ liệu");
    if (CheckSearchChange()) {
        LoadThongTin();
    }
    else
    {
        var  search = {};
        search.Skip = count;
        search.MaCanBo = $("#MaCanBo").val();
        search.HoVaTen = $("#HoVaTen").val();
        search.MaQuanHuyen = $("#MaQuanHuyen").val();
        search.MaDiaBanHoiVien = $("#MaDiaBanHoiVien").val();
        let page = document.querySelector(".paginate_button.page-item.active>a").textContent;
      
        $.ajax({
            type: "get",
            url: "/HoiVien/HoiVien/_Search",
            data: search,
            success: function (res, status, error) {
              
                table.rows.add(res.data);
                table.draw();
                let total = Number(res.total);
                count = table.data().count();
                table.page(Number(page-1)).draw(false);
                if (total > count) {
                    btn_loaddata.html("Hiển thị thêm<br> " + count.toLocaleString('hi-IN', { minimumFractionDigits: 0 }) + "/" + total.toLocaleString('hi-IN', { minimumFractionDigits: 0 }));
                    btn_loaddata.css("display", "block");
                    $("#txt-noteload").css("display", "block");
                }
                else {
                    btn_loaddata.css("display", "none");
                    $("#txt-noteload").css("display", "none");
                }
            },
            error: function (xhr, status, error) {
                Exceptions(xhr, status, error);
            }
        });
    }
}
function CheckSearchChange() {
    var _macanbo = $("#MaCanBo").val();
    var _hovaten = $("#HoVaTen").val();
    var _maquanhuyen = $("#MaQuanHuyen").val();
    var _diabanhoatdong = $("#MaDiaBanHoiVien").val();
    if (_macanbo != MaCanBo || _hovaten != HoVaTen || _maquanhuyen != MaQuanHuyen || _diabanhoatdong != MaDiaBanHoiVien) {
        MaCanBo = _macanbo;
        HoVaTen = _hovaten;
        MaQuanHuyen = _maquanhuyen;
        MaDiaBanHoiVien = _diabanhoatdong;
        return true;
    }
      
    else
        return false;
}
$(document).on("click", ".btn-import", function () {
    $.ajax({
        type: "get",
        url: "/HoiVien/HoiVien/_Import",
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
$(document).on("click", ".btn-import", function () {
    $.ajax({
        type: "get",
        url: "/HoiVien/HoiVien/_Import",
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

$(document).on("click", "#btn-importExcel", function () {
    var frm = $("#frmImport");
    var formData = new FormData(),
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
    $.ajax({
        type: "POST",
        url: "/HoiVien/HoiVien/Import",
        data: formData,
        processData: false,
        contentType: false,
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
                let html = '<div class="alert alert-danger">' +
                    '<button type="button" class="btn-close" data-bs-dismiss="alert" aria-hidden="true">×</button>' +
                    '<span class=""><svg xmlns="http://www.w3.org/2000/svg" height="40" width="40" viewBox="0 0 24 24"><path fill="#f07f8f" d="M20.05713,22H3.94287A3.02288,3.02288,0,0,1,1.3252,17.46631L9.38232,3.51123a3.02272,3.02272,0,0,1,5.23536,0L22.6748,17.46631A3.02288,3.02288,0,0,1,20.05713,22Z"></path><circle cx="12" cy="17" r="1" fill="#e62a45"></circle><path fill="#e62a45" d="M12,14a1,1,0,0,1-1-1V9a1,1,0,0,1,2,0v4A1,1,0,0,1,12,14Z"></path></svg></span>' +
                    '<strong>Lỗi</strong>' +
                    '<hr class="message-inner-separator">' +
                    '<p>' + jsonData.data + '.</p>' +
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
$('#modalImport').on('hidden.bs.modal', function (e) {
    /*document.parentElement.getElementById("importexcelfile").value = "";*/
    $("#iframe-import").html("");
})
$(document).on("click", ".btn-exporttoexcel", function () {
    var formParams = $("#frmSearch").serializeArray();
    var para = "";
    $.each(formParams, function (i, val) {
        if (val.value) {
            if (para == "") {
                para = para + val.name + "=" + val.value;
            }
            else {
                para = para + "&" + val.name + "=" + val.value;
            }

        }
    });
    self.location = "/HoiVien/HoiVien/ExportEdit?" + para;
});