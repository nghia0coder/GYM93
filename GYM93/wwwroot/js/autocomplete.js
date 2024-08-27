$(document).ready(function () {

    $("#searchThanhVien").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/ThanhVien/SearchThanhVien',
                data: { term: request.term },
                success: function (data) {
                 
                    response($.map(data.value, function (item) {
                        console.log(item.ten)
                        return {
                            label: item.ten,
                            value: item.thanhVienId
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
        }
    });

});