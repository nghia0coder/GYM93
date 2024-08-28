var dataTable;

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: "/thanhvien/index" },
        "columns": [
            { data: 'hovatendem', "width": '5%' },
            { data: 'ten', "width" : '25%'}
        ]
    })
}