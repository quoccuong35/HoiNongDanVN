$(document).on("click", 'input[type="checkbox"]', function (e) {
    let name = $(this).attr("name");

    if (name == "chk-all") {
        var checkboxs = document.querySelectorAll('li>input[type="checkbox"]');
        checkboxs.forEach((item) => {
            const sl = item.getAttribute("data-sl");
            item.checked = this.checked;
            if (item.name == "chk-quanhuyen") {
                if (this.checked) {
                    item.parentElement.children[2].textContent = sl + "/" + sl;
                }
                else {
                    item.parentElement.children[2].textContent = "0/" + sl;
                }
            }
        });
    }
    else if (name == "chk-quanhuyen") {
        //CheckQuanHuyen($(this), this.checked);
        const checkboxs = this.parentElement.querySelectorAll('ul>li>input[type="checkbox"]');
        checkboxs.forEach((item) => {
            item.checked = this.checked;
        });
        const sl = this.getAttribute("data-sl");
        if (this.checked) {
            this.parentElement.children[2].textContent = sl + "/" + sl;
        }
        else {
            this.parentElement.children[2].textContent = "0/" + sl;
        }

    }
    else {
        const parent = this.parentElement.parentElement.parentElement;

        let checkbox = parent.children[3].querySelectorAll('ul>li>input[type="checkbox"]').length;
        let checked = parent.children[3].querySelectorAll('ul>li>input[type="checkbox"]:checked').length;
        const span = parent.children[2];

        span.textContent = checked + "/" + checkbox;
    }
})