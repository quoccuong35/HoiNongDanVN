$(function () {
    $("select#MaTinhThanhPho").change(function () {
        let maTihThanhPho = $(this).val();

        $("select#MaQuanHuyen").empty();

        $.getJSON('/HoiVien/DiaBanHoatDong/LoadQuanHuyen?maTinhThanhPho=' + maTihThanhPho, function (data) {
            //console.log(data);
            $.each(data, function (i, item) {
                /*console.log(item);*/
                $("select#MaQuanHuyen").append('<option value=' + item.maQuanHuyen + '>' + item.tenQuanHuyen + '</option>');
            });
        });
    })
    $("select#MaQuanHuyen").change(function () {
        let maQuanHuyen = $(this).val();

        $("select#MaPhuongXa").empty();

        $.getJSON('/HoiVien/DiaBanHoatDong/LoadPhuongXa?maQuanHuyen=' + maQuanHuyen, function (data) {
            //console.log(data);
            $.each(data, function (i, item) {
                /*console.log(item);*/
                $("select#MaPhuongXa").append('<option value=' + item.maPhuongXa + '>' + item.tenPhuongXa + '</option>');
            });
        });
    })
    
});