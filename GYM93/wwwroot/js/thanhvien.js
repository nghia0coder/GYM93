var dataTable;

$(document).ready(function () {

    loadDataTable();
    
});

function loadDataTable() {
    console.log("Hello");
    dataTable = $('#tblData').DataTable({
        "ajax": { url: "/thanhvien/GetAllThanhVien" },
        error: function (xhr, error, code) {
            console.error("Error loading data:", error, code);
        },
        "columns": [
            { data: 'hoVaTenDem', "width": '20%' },
            { data: 'ten', "width": '10%' },
            {
                data: 'hinhAnhTv',
                "render": function (data) {
                    if (data) {
                        return `<img src="/${data}" alt="Hình ảnh" class="img-thumbnail" />`;
                    } else {
                        return `<img src="/default-image.jpg" alt="Không có hình ảnh" class="img-thumbnail" />`;
                    }
                },
                "width": '15%',
                "defaultContent": ""
            },
            {
                data: 'sđt', "width": '10%'
            },
            {
                data: null, // Cột này không lấy dữ liệu từ server
                "render": function (data, type, row) {
                    return `<a href="/thanhvien/edit/${row.thanhVienId}" class="btn btn-primary">Sửa</a>
                            <a href="/thanhvien/delete/${row.thanhVienId}" class="btn btn-danger">Xóa</a>`;
                },
                "width": '20%'
            }
        ]
    })
}
