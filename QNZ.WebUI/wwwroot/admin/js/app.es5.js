﻿"use strict";var app = (function () {
  var a = void 0,
      e = void 0,
      t = void 0,
      n = function n() {
    console.log(e), t.addEventListener("click", function () {
      return s(e, "wrap-nonav");
    }), a && (a.innerHTML = new Date());
  },
      s = function s(a, e) {
    a.classList.contains(e) ? a.classList.remove(e) : a.classList.add(e);
  };e = document.getElementById("wrapper"), t = document.getElementById("openav"), a = document.getElementById("sitetime"), n();
})();$(document).ready(function () {
  switch (($("#mainmenu .down-nav>a").click(function (a) {
    a.preventDefault();var e = $(this);e.next(".submenu").slideToggle(function () {
      e.closest("li.down-nav").toggleClass("open");
    });
  }), $("a.expand").click(function (a) {
    $(this).closest(".card").addClass("card-fixed"), a.preventDefault();
  }), $("a.compress").click(function (a) {
    $(this).closest(".card").removeClass("card-fixed"), a.preventDefault();
  }), location.pathname)) {case "/":case "/index.html":
      $(".mainmenu>li:nth-of-type(1) a").addClass("active");break;case "/table_basic.html":
      $(".mainmenu>li:nth-of-type(2)").addClass("nav-open"), $(".mainmenu>li:nth-of-type(2) .submenu li:nth-of-type(1) a").addClass("active");break;case "/table_adv.html":
      $(".mainmenu>li:nth-of-type(2)").addClass("nav-open"), $(".mainmenu>li:nth-of-type(2) .submenu li:nth-of-type(2) a").addClass("active");break;case "/customers.html":case "/customer-detail.html":case "/customer-edit.html":
      $(".mainmenu>li.customers a").addClass("active");break;case "/form_basic.html":
      $(".mainmenu>li:nth-of-type(3)").addClass("nav-open"), $(".mainmenu>li:nth-of-type(3) .submenu li:nth-of-type(1) a").addClass("active");break;case "/form_adv.html":
      $(".mainmenu>li:nth-of-type(3)").addClass("nav-open"), $(".mainmenu>li:nth-of-type(3) .submenu li:nth-of-type(2) a").addClass("active");break;case "/form_adv_pic.html":
      $(".mainmenu>li:nth-of-type(3)").addClass("nav-open"), $(".mainmenu>li:nth-of-type(3) .submenu li:nth-of-type(3) a").addClass("active");break;case "/pages.html":
      $(".mainmenu>li:nth-of-type(4) a").addClass("active");break;case "/noaccess.html":
      $(".mainmenu>li.ortherpage").addClass("nav-open"), $(".mainmenu>li.ortherpage .submenu li:nth-of-type(3) a").addClass("active");break;case "/404.html":
      $(".mainmenu>li.ortherpage").addClass("nav-open"), $(".mainmenu>li.ortherpage .submenu li:nth-of-type(4) a").addClass("active");break;case "/links.html":
      $(".mainmenu>li:nth-of-type(6)").addClass("nav-open"), $(".mainmenu>li:nth-of-type(6) .submenu li:nth-of-type(1) a").addClass("active");break;case "/link_categories.html":
      $(".mainmenu>li.links").addClass("nav-open"), $(".mainmenu>li.links .submenu li:nth-of-type(2) a").addClass("active");break;case "/siteinfo.html":
      $(".mainmenu>li.settings").addClass("nav-open"), $(".mainmenu>li.settings .submenu li:nth-of-type(1) a").addClass("active");break;case "/culture.html":case "/footprints.html":case "/awards.html":
      $(".mainav li:nth-of-type(4) a").addClass("active");break;case "/tech.html":
      $(".mainav li:nth-of-type(3) a").addClass("active");}$(window).scroll(function () {
    350 < $(this).scrollTop() ? $("#totop").fadeIn() : $("#totop").fadeOut();
  }), $("#totop").click(function () {
    return $("#totop").addClass("fly"), $("#totop").find("img").attr("src", "img/fly.png"), document.getElementById("myMusic").play(), $("html, body").animate({ scrollTop: 0 }, 1e3, function () {
      $("#totop").removeClass("fly"), $("#totop").find("img").attr("src", "img/totop.png");
    }), !1;
  });
});

