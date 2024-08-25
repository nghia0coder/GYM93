// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function previewImage(event) {
    var file = event.target.files[0];
    var reader = new FileReader();

    reader.onload = function () {
        var output = document.getElementById("preview");
        output.src = reader.result;
        output.style.display = 'block';
    };
    reader.readAsDataURL(file);
}