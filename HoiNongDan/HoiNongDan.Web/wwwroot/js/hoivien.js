$(function (e) {
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
                toastr.error(xhr.responseText);
            }
        });
    }

    $("select#MaQuanHuyen").change(function () {
        let maQuanHuyen = $(this).val();
        console.log(maQuanHuyen);
        $("select#MaDiaBanHoatDong").empty();
        $.getJSON('/HoiVien/HoiVien/loadDiaBanHoatDong?maQuanHuyen=' + maQuanHuyen, function (data) {
            $("select#MaDiaBanHoatDong").append('<option>' + "--Vui lòng chọn danh mục--" + '</option>');
            $.each(data, function (i, item) {
                /*console.log(item);*/
                $("select#MaDiaBanHoatDong").append('<option value=' + item.maDiaBanHoatDong + '>' + item.name + '</option>');
            });
        });
    })

});