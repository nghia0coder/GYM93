var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: "/thanhvien/GetAllThanhVien" },
        "columns": [
            { data: 'hoVaTenDem', "width": '25%' },
            { data: 'ten', "width": '5%' }
        ]
    })
}
