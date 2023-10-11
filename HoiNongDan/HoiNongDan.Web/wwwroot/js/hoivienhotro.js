$(function () {
    $("select#MaHinhThucHoTro").change(function () {
        let mahinhthuc = $(this).val().toUpperCase();
        if (mahinhthuc == "945C96EF-EA48-404D-9166-F41342EA48E6") {
            $("#div-SoTienVay").css("display", "block");
            $("#div-ThoiHangChoVay").css("display", "block");
            $("#div-LaiSuatVay").css("display", "block");
            $("#div-NgayTraNoCuoiCung").css("display", "block");
            $("#div-MaNguonVon").css("display", "block");
        }
        else {
            $("#div-SoTienVay").css("display", "none");
            $("#div-ThoiHangChoVay").css("display", "none");
            $("#div-LaiSuatVay").css("display", "none");
            $("#div-NgayTraNoCuoiCung").css("display", "none");
            $("#div-MaNguonVon").css("display", "none");
        }
    })
});