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
    
})();
