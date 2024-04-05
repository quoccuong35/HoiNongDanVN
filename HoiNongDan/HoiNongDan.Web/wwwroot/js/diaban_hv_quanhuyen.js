$("select#MaQuanHuyen").change(function () {
    let maQuanHuyen = $(this).val();
    //console.log(maQuanHuyen);
    $("select#MaDiaBanHoiVien").empty();
    $.getJSON('/HoiVien/HVInfo/loadDiaBanHoatDong?maQuanHuyen=' + maQuanHuyen, function (data) {
        $("select#MaDiaBanHoiVien").append('<option>' + "--Vui lòng chọn danh mục--" + '</option>');
        $.each(data, function (i, item) {
            /*console.log(item);*/
            $("select#MaDiaBanHoiVien").append('<option value=' + item.maDiaBanHoatDong + '>' + item.name + '</option>');
        });
    });
})