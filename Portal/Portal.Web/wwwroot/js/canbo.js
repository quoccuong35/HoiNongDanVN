$(function () {
    $("select#MaNgachLuong").change(function () {
        let maNgachLuong = $(this).val();

        $("select#MaBacLuong").empty();

        $.getJSON('/NhanSu/CanBo/LoadBacLuong?maNgachLuong=' + maNgachLuong, function (data) {
            //console.log(data);
            $.each(data, function (i, item) { 
                /*console.log(item);*/
                $("select#MaBacLuong").append('<option value=' + item.maBacLuong + '>' + item.tenBacLuong +'</option>');
            });
        });
    })
    $("select#MaTrinhDoHocVan").change(function () {
        let maTrinhDoHV = $(this).val();
        if (maTrinhDoHV == "SĐH") {
            $("#cbxHocHam").css("display", "block")
        }
        else {
            $("#cbxHocHam").css("display", "none")
        }
    })
    $("select#MaBacLuong").change(function () {
        let mabacluong = $(this).val();
        $.getJSON('/NhanSu/CanBo/GetHoSoLuong?id=' + mabacluong, function (data) {
            if (data != null) {
                $("#HeSoLuong").val(data.heSo);
            }
        });
    })
});