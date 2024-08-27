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


document.getElementById("tongTienInput").addEventListener("input", function (e) {
    let value = e.target.value.replace(/[^0-9]/g, '')
    if (value.length > 0) {
        value = parseFloat(value).toLocaleString();
    }
    e.target.value = value ? value + ' VNĐ' : '';
    document.getElementById("tongTienHidden").value = value.replace(/[^0-9]/g, '');
})
document.getElementById("tongTienInput").addEventListener("blur", function () {
    validateInput();
})

function validateInput() {

    let numericValue = document.getElementById("tongTienHidden").value;


    let numberValue = parseFloat(numericValue);
    let validationMessage = '';


    if (isNaN(numberValue)) {
        validationMessage = 'Vui Lòng Nhập Tổng Tiền';
    } else if (numberValue < 100000) {
        validationMessage = 'Giá trị phải lớn hơn hoặc bằng 100.000 VNĐ';
    } else if (numberValue > 5000000) {
        validationMessage = 'Giá trị phải nhỏ hơn hoặc bằng 5.000.000 VNĐ';
    }

    // Cập nhật thông báo lỗi
    document.getElementById("validationMessage").textContent = validationMessage;
}



