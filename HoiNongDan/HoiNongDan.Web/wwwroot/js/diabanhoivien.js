
var DiaBanHoiVien = {}
DiaBanHoiVien.Init = function (controller) {
    DiaBanHoiVien.AddThanhVien(controller);
    DiaBanHoiVien.RoiHoiVien(controller);
}
DiaBanHoiVien.AddThanhVien = function (controller) {
    $("#btn-save-hoivien").click(function () {
        var frm = $("#frmEditChiTiet"),
            formData = new FormData(),
            formParams = frm.serializeArray();
        if (frm.valid()) {
            $.each(formParams, function (i, val) {
                formData.append(val.name, val.value);
            });
            console.log(formData.length);
            toastr.clear();
            $.ajax({
                type: "POST",
                url: "/" + controller + "/AddDiaBanHoiVien",
                data: formData,
                processData: false,
                contentType: false,
                success: function (jsonData) {

                    if (jsonData.success == true) {
                        if (jsonData.data != null) {
                            toastr.success(jsonData.data);
                            LoadDiaBanHoiVien("FC19C32D-E5E7-49AF-AEDE-E36C441050A8");
                            HoiNongDan.Table();
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
        }
    });
}
DiaBanHoiVien.LoadDiaBanHoiVien = function (id) {
    var data = {};
    data.id = id;
    $.ajax({
        type: "get",
        url: "/HoiVien/DiaBanHoatDong/_DiaBanHoiVien",
        data: data,
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
                $("#detailList").html(xhr);
                HoiNongDan.Table()
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
DiaBanHoiVien.RoiHoiVien = function (id) {
    $(".btn-hoivien-roidi").click(function () {
        alert("Hello");
    });
}