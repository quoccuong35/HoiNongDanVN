(function () {
    let splitter = document.getElementsByClassName("splitter-head");
    for (const sp of splitter) {
        sp.addEventListener('click', function handleClick() {
            let parent = sp.closest(".splitter").children[1].classList.toggle("splitter-toggle");
            // sp.closest(".splitter").requestFullscreen();
        });
    }
})();