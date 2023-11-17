$("select#MaNgachLuong").change(function () {
    let maNgachLuong = $(this).val();

    $("select#MaBacLuong").empty();

    var para = {};
    para.maNgachLuong = maNgachLuong;
    AjaxGet("/NhanSu/CanBo/LoadBacLuong", para, SetBaLuong);

    //$.getJSON('/NhanSu/CanBo/LoadBacLuong?maNgachLuong=' + maNgachLuong, function (data) {
    //    //console.log(data);
    //    $.each(data, function (i, item) { 
    //        /*console.log(item);*/
    //        $("select#MaBacLuong").append('<option value=' + item.maBacLuong + '>' + item.tenBacLuong +'</option>');
    //    });
    //});
})
const SetBaLuong = function (data) {
    $.each(data, function (i, item) {
        /*console.log(item);*/
        $("select#MaBacLuong").append('<option value=' + item.maBacLuong + '>' + item.tenBacLuong + '</option>');
    });
}
$("select#MaBacLuong").change(function () {
    let mabacluong = $(this).val();
    let textValue = $('select#MaBacLuong option:selected').text().split(':')[1].trim();
    $("#HeSoLuong").val(Number.parseFloat(textValue));
    //$.getJSON('/NhanSu/CanBo/GetHoSoLuong?id=' + mabacluong, function (data) {
    //    if (data != null) {
    //        $("#HeSoLuong").val(data.heSo);
    //    }
    //});
})



$("select#IdCoSo").change(function () {
    let idCoSo = $(this).val();

    $("select#IdDepartment").empty();

    $.getJSON('/NhanSu/CanBo/LoadDonVi?idCoSo=' + idCoSo, function (data) {
        //console.log(data);
        $.each(data, function (i, item) {
            /*console.log(item);*/
            $("select#IdDepartment").append('<option value=' + item.idDepartment + '>' + item.name + '</option>');
        });
    });
})