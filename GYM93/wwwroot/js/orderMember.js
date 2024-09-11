const giaTienMotThang = 200000;

$("#thangDangKy").on("input", function () {


    const thangDangKy = parseInt($(this).val()) || 0;

    const tongtien = thangDangKy * giaTienMotThang;

    const tongTienFormatted = tongtien.toLocaleString('vi-VN');

    document.getElementById('tongTienInput').value = tongTienFormatted;

    $("#tongTienHidden").val(tongtien);
});


$("#tongTienInput").on("input", function (e) {

    let value = e.target.value.replace(/[^0-9]/g, '');

    if (value.length > 0) {

        document.getElementById("tongTienHidden").value = value;


        e.target.value = parseInt(value).toLocaleString('vi-VN');
    } else {

        document.getElementById("tongTienHidden").value = '';
    }
});

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