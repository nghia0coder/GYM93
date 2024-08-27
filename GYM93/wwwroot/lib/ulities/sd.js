$(document).ready(function () {
    $("input").on("blur", function () {
        $(this).valid();
    });
});