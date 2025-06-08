// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.getElementById("service-button-div").addEventListener("click", function() {
    document.getElementById("container-login-div").style.display = "grid";
});
document.getElementsByClassName("header-button-div")[0].addEventListener("click", function() {
    document.getElementById("container-login-div").style.display = "grid";
});
document.getElementsByClassName("header-logo-div")[0].addEventListener("click", function() {
    document.getElementById("container-login-div").style.display = "none";
});