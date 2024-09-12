const giaTienNgay = 25000;

$("#soNgayDangKy").on("input", function () {


    const soNgay = parseInt($(this).val()) || 0;

    const tongtien = soNgay * giaTienNgay;

    const tongTienFormatted = tongtien.toLocaleString('vi-VN');

    document.getElementById('totalInput').value = tongTienFormatted;

    $("#totalHidden").val(tongtien);
});


$("#totalInput").on("input", function (e) {

    let value = e.target.value.replace(/[^0-9]/g, '');

    if (value.length > 0) {

        document.getElementById("totalHidden").value = value;


        e.target.value = parseInt(value).toLocaleString('vi-VN');
    } else {

        document.getElementById("totalHidden").value = '';
    }
});

document.getElementById("totalInput").addEventListener("blur", function () {
    validateInput();
})

function validateInput() {

    let numericValue = document.getElementById("totalHidden").value;


    let numberValue = parseFloat(numericValue);
    let validationMessage = '';


    if (isNaN(numberValue)) {
        validationMessage = 'Vui Lòng Nhập Tổng Tiền';
    } else if (numberValue < 25000) {
        validationMessage = 'Giá trị phải lớn hơn hoặc bằng 25.000 VNĐ';
    } else if (numberValue > 5000000) {
        validationMessage = 'Giá trị phải nhỏ hơn hoặc bằng 5.000.000 VNĐ';
    }

    // Cập nhật thông báo lỗi
    document.getElementById("validationMessage").textContent = validationMessage;
}