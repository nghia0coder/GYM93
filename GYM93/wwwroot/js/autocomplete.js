$(document).ready(function () {

    $("#searchThanhVien").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/ThanhVien/SearchThanhVien',
                data: { term: request.term },
                success: function (data) {
                    // Giới hạn kết quả trả về chỉ 9 item
                    response($.map(data.value.slice(0, 9), function (item) {
                        return {
                            label: item.ten,
                            value: item.thanhVienId,
                            img: item.hinhAnhTv
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


      
            // Tạo thẻ img chứa hình ảnh
            var imgTag = `
            <img src="${ui.item.img}" alt="Hình Ảnh Thành Viên Gym93" class="img-fluid" />
        `;

            // Chèn hình ảnh vào bên trong div với id displayImage
            $("#displayImage").html(imgTag);

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
            .append("<div><img src='" + item.img + "' alt='Image' style='width:100px; height:100px;  object-fit: cover; margin-right:10px;' />" + item.label + "</div>")
            .appendTo(ul);
    };;

    // Thiết lập giá trị ban đầu của ô input nếu có
    var initialValue = $("#ThanhVienId").val(); // Lấy giá trị từ thẻ hidden
    if (initialValue) {
        // Gửi AJAX request để lấy thông tin thành viên dựa trên ThanhVienId
        $.ajax({
            url: '/ThanhVien/GetThanhVienById', // API để lấy thông tin thành viên theo id
            data: { thanhVienid: initialValue }, // Gửi ThanhVienId trong request
            success: function (data) {
                if (data) {
                    var imgTag = `
            <img src="${data.hinhAnhTv}" alt="Hình Ảnh Thành Viên Gym93" class="img-fluid" />
        `;
                    // Gán tên thành viên vào input search
                    $("#searchThanhVien").val(data.ten); // Hiển thị tên thành viên
                    $("#displayImage").html(imgTag);
                }
            },
            error: function (xhr, status, error) {
                console.error("AJAX Error:", status, error); // Xử lý lỗi nếu có
            }
        });
    }



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



   
});

// Thêm CSS để giới hạn chiều cao của menu gợi ý
$("<style>")
    .prop("type", "text/css")
    .html(`
        .ui-autocomplete {
            max-height: 300px; /* Điều chỉnh chiều cao tùy theo kích thước mong muốn */
            overflow-y: auto;
            overflow-x: hidden;
        }
    `)
    .appendTo("head");