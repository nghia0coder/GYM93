
    var currentPage = 1;
    var pageSize = 5;
    var sortBy;
    var sortAscending = true;
    var totalPages = 0;
    function fetchData(sortColumn) {
        // Cập nhật cột sắp xếp nếu có
        if (sortColumn) {
            sortBy = sortColumn;
            sortAscending = !sortAscending; // Đổi hướng sắp xếp
        }

        var searchQuery = document.getElementById('searchInput').value;

        var url = `/thanhvien/getmembers?searchQuery=${searchQuery}&pageNumber=${currentPage}&pageSize=${pageSize}&sortBy=${sortBy}&sortAscending=${sortAscending}`;

        fetch(url)
            .then(response => response.json())
            .then(data => {
                updateTable(data.value);
                updatePagination(data.value);
                updateSortIcon(sortBy);
            });
    }

    function updateTable(data) {
        var tableBody = document.querySelector('#tblData tbody');
        tableBody.innerHTML = '';
        totalPages = data.totalPages;
        data.members.forEach(function (member) {
            var hoVaTenDem = member.hoVaTenDem ? member.hoVaTenDem : 'trống';
            var sdt = member.sđt ? member.sđt : 'trống';
            var row = `<tr>
                              <td>${hoVaTenDem}</td>
                              <td>${member.ten}</td>
                              <td>${sdt}</td>
                              <td><img src="${member.hinhAnhTv}" alt="Gym93" class="img-thumbnail" /></td>
                              <td>
                                  <a href="/ThanhVien/Edit/${member.thanhVienId}" class="btn btn-warning"  >Chỉnh Sửa Thông Tin</a> |
                                  <a href="/ThanhVien/Details/${member.thanhVienId}" class="btn btn-info">Thông Tin Tiết</a> 
                             ${false ? `<a href="/ThanhVien/Delete/${member.thanhVienId}" class="btn btn-danger">Xóa Thành Viên</a>` : ''}
                              </td>
                           </tr>`;
            tableBody.innerHTML += row;
        });
    }

    function updatePagination(data) {
        
        document.getElementById('pageInfo').textContent = `Trang ${data.pageNumber} Trên ${data.totalPages}`;

        document.getElementById('prevPageBtn').style.display = (data.pageNumber > 1) ? 'inline-block' : 'none';
        document.getElementById('nextPageBtn').style.display = (data.pageNumber < data.totalPages) ? 'inline-block' : 'none';
    }

    function nextPage() {
        if (currentPage < totalPages) {
            currentPage++;
            fetchData();
        }
    }


    function prevPage() {
        if (currentPage > 1) {
            currentPage--;
            fetchData();
        }
    }

    // Tải dữ liệu lần đầu khi trang được load
    fetchData();

    function updateSortIcon(sortBy) {
        var sortIcon = document.getElementById('sortIcon');

        if (sortAscending) {
            sortIcon.className = "fa fa-arrow-up"; // Mũi tên hướng lên
        } else {
            sortIcon.className = "fa fa-arrow-down"; // Mũi tên hướng xuống
        }
    }
