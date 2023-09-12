(function () {
    let splitter = document.getElementsByClassName("splitter-head");
    for (const sp of splitter) {
        sp.addEventListener('click', function handleClick() {
            let parent = sp.closest(".splitter").children[1].classList.toggle("splitter-toggle");
            // sp.closest(".splitter").requestFullscreen();
        });
    }
    let currency = document.querySelectorAll('input[data-type="currency"]');
    for (const item of currency) {
        item.addEventListener("focus", (event) => {
            let value = item.value;
            let temp = value.replaceAll(",", "");
            item.value = value == "" ? null : Number.parseFloat(temp);
            item.setAttribute("step", "500000");
            item.setAttribute("type", "number");
        });
        item.addEventListener("blur", (event) => {
            item.setAttribute("type", "text")
            item.removeAttribute("step");
            let value = item.value;
            //console.log(value);
            item.value = value == "" ? "" : (Number.parseInt(value)).toLocaleString("en");
        });
    }
    $('.btn-userinfo').on('click', function () {
        let $btn = $(this);
        var id = $btn.data("id");
        $.ajax({
            type: "get",
            url: "/Permission/AccountInfo/Edit",
            data: { id: id },
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

                    $("#content-infouser").html(xhr);
                    $('#modal-userinfo').modal("show");
                }
            },
            error: function (xhr, status, error) {

            }
        });
    });
    $('.btn-doimatkhau').on('click', function () {
        var id = $("#AccountId").val();
        var passWord = $("#txt-password").val();
        if (!passWord) {
            toastr.info("Bạn chưa nhập thông tin mật khẩu");
            return;
        }
        Swal.fire({
            title: 'Đổi mật khẩu',
            html: "Bạn có muốn đổi thông tin mật khẩu. Có để tiếp tục",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Có',
            cancelButtonText: "Không"
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: "/Permission/AccountInfo/DoiMatKhau",
                    type: 'post',
                    data: { id: id, passWord: passWord },
                    success: function (data) {
                        toastr.clear();
                        if (data.success) {
                            toastr.success(data.data);
                            setTimeout(function () {
                                window.location.href = "/Permission/Auth/LogOut"; 
                            }, 1000);
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
        
    });
})();
