$(document).on("click", ".btn-answer", function () {
    let $btn = $(this);
    let id = $btn.data("id");
    let status = $btn.data("status");
    if (status == "01") {
        formData = new FormData();
        formData.append("id", id);
        $.ajax({
            type: "POST",
            url: "/HoiVien/HVDap/Edit",
            data: formData,
            processData: false,
            contentType: false,
            success: function (jsonData) {

                if (jsonData.success == true) {
                    
                }
                else {
                    toastr.error(jsonData.data);
                }
            },
            error: function (xhr, status, error) {
                toastr.error(xhr.responseText);
            }
        });
    }
    var parent = event.target.parentNode;
    var answer = parent.querySelector('#cs_answer');
    answer.style.display = "block";
});
$(document).on("click", ".btn-answercancel", function () {
    let $btn = $(this);
    let index = $btn.data("index");
    $("#txt_noidung_" + index).val("");
    var parent = event.target.parentNode.parentNode.parentNode.parentNode;
    var answer = parent.querySelector('#cs_answer');
    answer.style.display = "none";
});
$(document).on("click", ".btn-guitraloi", function () {
    let $btn = $(this);
    let id = $btn.data("id");
    let index = $btn.data("index");
    let noidung = $("#txt_noidung_" + index).val();
    if (!noidung)
    {
        toastr.error('Nội dung trã lời chưa nhập');
        return;
    }
    formData = new FormData();
    formData.append("id", id);
    formData.append("NoiDung", noidung);
    $.ajax({
        type: "POST",
        url: "/HoiVien/HVDap/Create",
        data: formData,
        processData: false,
        contentType: false,
        success: function (jsonData) {

            if (jsonData.success == true) {
                toastr.success(jsonData.data);
            }
            else {
                toastr.error(jsonData.data);
            }
        },
        error: function (xhr, status, error) {
            toastr.error(xhr.responseText);
        }
    });
});