function LuuCauHoi() {
    var id = $("#txt-id").val();
    var noidung = $("#txt-noidung").val();
    if (!id) {
        alert("Bạn chưa tìm kiếm thông tin hội viên");
        return;
    }
    if (!noidung) {
        alert("Bạn chưa nhập nội dung câu hỏi");
        return;
    }
    formData = new FormData();
    formData.append("IDHoiVien", id);
    formData.append("NoiDung", noidung);
    $.ajax({
        type: "POST",
        url: "/HoiVien/HVHoi/Create",
        data: formData,
        processData: false,
        contentType: false,
        success: function (jsonData) {

            if (jsonData.success == true) {
                alert(jsonData.data);
                Search(id);
            }
            else {
                alert(jsonData.data);
            }
        },
        error: function (xhr, status, error) {
            Exceptions(xhr, status, error);
        }
    });
}
function Search(id) {
    var data = {};
    data.id = id;
    $.ajax({
        type: "get",
        url: "/HoiVien/HVHoi/_Search",
        data: data,
        success: function (xhr, status, error) {
            if (xhr.Code == 500 || xhr.Success == false) {

            }
            else {
                $("#div-thongtinhoidap").html(xhr);
            }
        }
    });
}
$(document).on("click", ".btn-delcauhoi", function () {
    let $btn = $(this);
    let id = $btn.data("id");
    var idcanbo = $("#txt-id").val();
    $.ajax({
        url: "/HoiVien/HVHoi/Delete",
        type: 'DELETE',
        data: { id: id },
        success: function (data) {
            if (data.success) {
                Search(idcanbo);
            }
            else {
                alert(data.data);
            }
        },
        error: function (xhr, status, data) {

            alert(data);
        }
    });
});