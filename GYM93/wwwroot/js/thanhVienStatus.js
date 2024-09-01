
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

        var url = `/thanhvien/getmembersstatus?searchQuery=${searchQuery}&pageNumber=${currentPage}&pageSize=${pageSize}&sortBy=${sortBy}&sortAscending=${sortAscending}`;

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

            var startDate = new Date(member.ngayBatDau);
            var endDate = new Date(member.ngayKetThuc);
            var currentDate = new Date();
            var remainingDays;
            if (startDate && endDate) {
                // Tính toán số ngày đã trôi qua và tổng số ngày
                var totalDays = (endDate - startDate) / (1000 * 60 * 60 * 24); // Tổng số ngày
                var passedDays = (currentDate - startDate) / (1000 * 60 * 60 * 24); // Số ngày đã trôi qua
                remainingDays = Math.ceil((endDate - currentDate) / (1000 * 60 * 60 * 24)); // Số ngày còn lại
                // Tính phần trăm tiến trình
                var progressPercent = (passedDays / totalDays) * 100;

                // Nếu thời gian đã qua vượt quá ngày kết thúc, thanh tiến trình sẽ đầy (100%)
                if (progressPercent > 100) {
                    progressPercent = 100;
                } else if (progressPercent < 0) {
                    progressPercent = 0; // Trước ngày bắt đầu
                }
            } else {
                var progressPercent = 0; // Mặc định là 0 nếu không có ngày bắt đầu hoặc ngày kết thúc
            }

            // Chọn màu sắc cho thanh tiến trình dựa trên số ngày còn lại
            var progressBarClass = '';
            if (remainingDays > 10) {
                progressBarClass = 'bg-success'; // Xanh lá
            } else if (remainingDays <= 10 && remainingDays > 5) {
                progressBarClass = 'bg-warning'; // Vàng
            } else if (remainingDays <= 5) {
                progressBarClass = 'bg-danger'; // Đỏ
            }

            // Tạo nút gia hạn nếu còn dưới 10 ngày
            var renewButton = '';
            if (remainingDays <= 10) {
                renewButton = `<a href="/hoadon/create/${member.thanhVienId}" class="btn btn-primary" style="margin-top:15px;">Gia Hạn</a>`;
            }

            var row = `<tr>
                              <td>${member.ten}</td>
                              <td>${member.sđt}</td>
                              <td><img src="/${member.hinhAnhTv}" alt="Gym93" class="img-thumbnail" /></td>
                                <td><div class="progress custom-progress">
                              <div class="progress-bar ${progressBarClass} custom-progress-bar" role="progressbar" style="width: ${progressPercent}%" aria-valuenow="${progressPercent}" aria-valuemin="0" aria-valuemax="100">Còn ${remainingDays} ngày</div>
                          </div>
                            ${renewButton}
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
