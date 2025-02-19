function DuyetHoiVienInitial(controller) {
    $(document).on("click", ".duyet-hoivien", function (e) {
        let $btn = $(this);
        let id = $btn.data("id");
        var lid = [];
        lid.push(id);
        var formData = {};
        formData.lid = lid;
        DuyetHoiVien(formData, controller);
    });

    $(document).on("click", "#duyet-hoivienall", function () {
        var lid = [];
        $('#data-list tbody tr').each(function () {
            lid.push(this.id);
        });
        var formData = {};
        formData.lid = lid;
        DuyetHoiVien(formData, controller);
    });
    $(document).on("click", ".btn-reject", function (e) {
        let $btn = $(this);
        let id = $btn.data("id");
        var lid = [];
        lid.push(id);
        var formData = {};
        formData.lid = lid;
        TuChoiDuyetHoiVien(formData, controller);
    });
    $(document).on("click", "#btn-duyettudong", function (e) {
        var lid = [];
        $('#data-list tbody tr').each(function () {
            lid.push(this.id);
        });
        var formData = {};
        formData.lid = lid;
        DuyetTuDong(formData, controller);
    });
}
function DuyetHoiVien(data, controller) {
    let textRequestion = "Bạn có muốn duyệt các hội viên đang chọn?";
    Swal.fire({
        title: 'Duyệt hội viên mới',
        html: '<input id="soQuyetDinh" type="text" class="form-control my-1" placeholder="Số quyết định">' +
            '<input id="ngayQuyetDinh" type="date" class="form-control" placeholder="Ngày quyết định">',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Có',
        cancelButtonText: "Không"
    }).then((result) => {
        var soQuyetDinh = $("#soQuyetDinh").val();
        var ngayQuyetDinh = $("#ngayQuyetDinh").val();

        data.soQuyetDinh = soQuyetDinh;
        data.ngayQuyetDinh = ngayQuyetDinh;

        if (result.isConfirmed) {
            if (!soQuyetDinh || !ngayQuyetDinh) {
                toastr.error("Chưa nhập số quyết định hoặc ngày quyết định");
                return;
            }
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

function DuyetTuDong(data, controller) {
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
        let soQuyetDinh = $("#hoiVienDuyetCapNhatVM_SoQuyetDinh").val();
        let ngayQuyetDinh = $("#hoiVienDuyetCapNhatVM_NgayVaoHoi").val();
        let ngayCapThe = $("#hoiVienDuyetCapNhatVM_NgayCapThe").val();
        let soThe = $("#SoThe").val();

        data.SoQuyetDinh = soQuyetDinh;
        data.NgayQuyetDinh = ngayQuyetDinh;
        data.SoThe = soThe;

        data.NgayCapThe = ngayCapThe;

        if (result.isConfirmed) {
            if (!ngayCapThe) {
                toastr.error("Chưa nhập ngày cấp thẻ");
                return;
            }
            var token = $("input[name='__RequestVerificationToken']").val();
            data.__RequestVerificationToken = token;
            $.ajax({
                url: "/" + controller + "/DuyetTuDong",
                type: 'POST',
                data: data,
                success: function (data) {
                    toastr.clear();
                    if (data.success) {
                        $("#btn-search").trigger("click");
                        toastr.success(data.data);
                        $("#SoThe").val(data.sothe);
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

function TuChoiDuyetHoiVien(data, controller) {
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