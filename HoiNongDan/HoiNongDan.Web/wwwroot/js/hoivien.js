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
var table;
//load default and set event
$(document).ready(function () {
    LoadThongTin()
    //HideShowColumns();
    $('#dt-hoivien tbody').on('dblclick', 'tr', function () {
        var row = table.row(this).data();
        let link = '/HoiVien/HoiVien/Edit/' + row['idCanBo'];
        window.open(link, '_blank').focus();
    });
});
function LoadThongTin() {
    var $btn = $("#btn-search");
    $btn.toggleClass("btn-loading");
    $.ajax({
        type: "get",
        url: "/HoiVien/HoiVien/_Search",
        data: $("#frmSearch").serializeArray(),
        success: function (xhr, status, error) {
            table = $('#dt-hoivien').DataTable({
                data: xhr,
                autoFill: true,
                responsive: false,
                autoWidth: false,
                autoHeight: false,
                scrollCollapse: false,
                scrollX: true,
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
                    { data: "hoNgheo" },
                    { data: "canNgheo" },
                    { data: "giaDinhChinhSach" },
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
                    { data: "hoTrovayVon" },
                    { data: "hoTroKhac" },
                    { data: "hoTroDaoTaoNghe" },
                    { data: "kkAnToanThucPham" },
                    { data: "dkMauNguoiNongDanMoi" },
                    { data: "ghiChu" },

                ]
            });
            table.on('order.dt search.dt', function () {
                table.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
            }).draw();
            $btn.toggleClass("btn-loading");
            HideShowColumns('#example');
        },
        error: function (xhr, status, error) {
            $btn.toggleClass("btn-loading");
            Exceptions(xhr, status, error);
        }
    });
}
$("#btn-search").click(function () {
    var $btn = $("#btn-search");
    $btn.toggleClass("btn-loading");
    table.clear();
    $.ajax({
        type: "get",
        url: "/HoiVien/HoiVien/_Search",
        data: $("#frmSearch").serializeArray(),
        success: function (xhr, status, error) {

            table.rows.add(xhr);
            table.draw();
            $btn.toggleClass("btn-loading");
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