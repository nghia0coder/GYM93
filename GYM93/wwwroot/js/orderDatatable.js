var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: "/hoadon/getallhoadon" },
        "columns": [
            { data: "hoaDonId", "width": "10%", "className": "text-center" },
            { data: "tenThanhVien", "width": "15%", "className": "text-center" },
            { data: "thangDangKy", "width": "15%", "className": "text-center" },
            {
                data: "ngayThanhToan", "width": "20%", "className": "text-center",
                render: function (data, type, row) {
                    return moment(data).format('DD/MM/YYYY HH:mm'); // sử dụng moment.js để định dạng ngày tháng
                }
            },
            {
                data: "tongTien", "width": "20%", "className": "text-center",
                render: function (data) {
                    return new Intl.NumberFormat('vi-VN').format(data) + ' VNĐ';
                }
            }
        ],
        "order": [[0, 'desc']],
        "language": {
            "emptyTable": "Hiện Tại Không Có Dữ Liệu Nào", // Thay đổi thông báo khi bảng không có dữ liệu
            "info": "Hiển thị từ _START_ đến _END_ của _TOTAL_ mục", // Thay đổi thông báo số lượng mục
            "infoEmpty": "Hiển thị 0 đến 0 của 0 mục", // Thay đổi thông báo khi không có mục nào
            "infoFiltered": "(Lọc từ _MAX_ mục)", // Thay đổi thông báo khi có lọc dữ liệu
            "lengthMenu": "Hiển thị _MENU_ mục mỗi trang", // Thay đổi thông báo số mục trên mỗi trang
            "loadingRecords": "Đang tải...", // Thay đổi thông báo khi đang tải dữ liệu
            "processing": "Đang xử lý...", // Thay đổi thông báo khi đang xử lý dữ liệu
            "search": "Tìm kiếm:", // Thay đổi tiêu đề tìm kiếm
            "zeroRecords": "Không tìm thấy kết quả phù hợp", // Thay đổi thông báo khi không tìm thấy dữ liệu
            "paginate": {
                "first": "Trang Đầu", // Thay đổi tiêu đề nút đầu tiên
                "last": "Trang Cuối", // Thay đổi tiêu đề nút cuối cùng
                "next": "Kế tiếp", // Thay đổi tiêu đề nút kế tiếp
                "previous": "Trước đó" // Thay đổi tiêu đề nút trước đó
            }
        }
    });



}
