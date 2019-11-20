console.clear(); var app = function () { var t = void 0, o = void 0, n = function () { o.addEventListener("click", function () { return e(t, "nav-active") }) }, e = function (t, o) { t.classList.contains(o) ? t.classList.remove(o) : t.classList.add(o) }; t = document.querySelector("body"), o = document.querySelector(".menu-icon"), document.querySelectorAll(".nav__list-item"), n() }(); $(document).ready(function () { $(window).scroll(function () { 350 < $(this).scrollTop() ? $("#totop").fadeIn() : $("#totop").fadeOut() }), $("#totop").click(function () { return $("#totop").addClass("fly"), $("#totop").find("img").attr("src", "/img/fly.png"), document.getElementById("myMusic").play(), $("html, body").animate({ scrollTop: 0 }, 1e3, function () { $("#totop").removeClass("fly"), $("#totop").find("img").attr("src", "/img/totop.png") }), !1 }) });

$(function () {
    $("#openlangs").click(function (e) {
        e.preventDefault();
        $(this).next("dl").slideToggle();

    });
})