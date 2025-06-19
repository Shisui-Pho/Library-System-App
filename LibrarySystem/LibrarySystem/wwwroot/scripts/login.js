document.querySelector("#LogIn").addEventListener("click", function () {
    document.querySelector(".popup").classList.add("active")
});

document.querySelector(".button-close .btn-close").addEventListener("click", function () {
    document.querySelector(".popup").classList.remove("active")
});