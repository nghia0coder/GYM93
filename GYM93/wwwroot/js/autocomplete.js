$(document).ready(function () {

    $("#searchThanhVien").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/ThanhVien/SearchThanhVien',
                data: { term: request.term },
                success: function (data) {
                 
                    response($.map(data.value, function (item) {
                        
                        return {
                            label: item.ten,
                            value: item.thanhVienId,
                            img : item.hinhAnhTv
                        };
                    }));
                },
                error: function (xhr, status, error) {
                    console.error("AJAX Error:", status, error); // Xem lỗi nếu có
                }
            });
        },
        minLength: 1,
        delay: 100,
        select: function (event, ui) {
            $("#searchThanhVien").val(ui.item.label);
            $("input[name='ThanhVienId']").val(ui.item.value);
            return false;
        },
        focus: function (event, ui) {
            // Ngăn mặc định gán giá trị value khi dùng phím mũi tên
            $("#searchThanhVien").val(ui.item.label);
            return false;
        }

    }).autocomplete("instance")._renderItem = function (ul, item) {
        // Tạo giao diện tùy chỉnh cho mỗi item với hình ảnh
        return $("<li>")
            .append("<div><img src='/" + item.img + "' alt='Image' style='width:100px; height:100px;  object-fit: cover; margin-right:10px;' />" + item.label + "</div>")
            .appendTo(ul);
    };;
    // Ngăn chặn phím lên và phím xuống điều chỉnh giá trị ô nhập liệu
    $("#searchThanhVien").on('keydown', function (event) {
        // Phím lên
        if (event.key === 'ArrowUp' || event.key === 'ArrowDown') {
            event.preventDefault(); // Ngăn chặn hành động mặc định
        }
    });

    // Xử lý sự kiện khi người dùng xóa hoặc nhập dữ liệu khác
    $("#searchThanhVien").on('blur', function () {
        var currentVal = $(this).val();
        var selectedId = $("input[name='ThanhVienId']").val();

        $.ajax({
            url: '/ThanhVien/SearchThanhVien',
            data: { term: currentVal },
            success: function (data) {
                var isValid = data.value.some(function (item) {
                    return item.ten === currentVal;
                });
                if (!isValid) {
                    $("input[name='ThanhVienId']").val(''); // Xóa giá trị nếu không khớp
                }
            },
            error: function (xhr, status, error) {
                console.error("AJAX Error:", status, error);
            }
        });
    });



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
});