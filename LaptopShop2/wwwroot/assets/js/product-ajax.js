$(document).ready(function () {
    // Biến tạm để theo dõi trạng thái checked của từng bộ lọc
    var lastSelected = {
        colorFilter: '',
        cpuCompanyFilter: '',
        ramSizeFilter: '',
        screenSizeFilter: '',
        driveMemoryFilter: ''
    };

    function updateProducts() {
        var categoryId = $('#categorySelect').val();
        var color = $('input[name="colorFilter"]:checked').val();
        var cpuCompany = $('input[name="cpuCompanyFilter"]:checked').val();
        var ramSize = $('input[name="ramSizeFilter"]:checked').val();
        var screenSize = $('input[name="screenSizeFilter"]:checked').val();
        var driveMemory = $('input[name="driveMemoryFilter"]:checked').val();

        // Chỉ gửi các giá trị cần thiết nếu chúng tồn tại
        var data = {};
        if (categoryId && categoryId !== "0") data.categoryId = categoryId;
        if (color) data.color = color;
        if (cpuCompany) data.cpuCompany = cpuCompany;
        if (ramSize) data.ramSize = ramSize;
        if (screenSize) data.screenSize = screenSize;
        if (driveMemory) data.driveMemory = driveMemory;

        var url = $('#productListUrl').val(); // Trường này sẽ chứa URL động được truyền từ View

        $.ajax({
            url: url,
            type: 'GET',
            data: data,
            success: function (result) {
                $('#product_item').html(result);
            },
            error: function (xhr, status, error) {
                console.error('AJAX request failed:', status, error);
                $('#product_item').html('<p>Unable to load products. Please try again later.</p>');
            }
        });
    }

    // Phương pháp để xử lý hủy lựa chọn khi click lần thứ hai
    $('input[type="radio"]').on('click', function (e) {
        var name = $(this).attr('name');
        if (lastSelected[name] === $(this).val()) {
            $(this).prop('checked', false);
            lastSelected[name] = '';
        } else {
            lastSelected[name] = $(this).val();
        }

        updateProducts(); // Gọi lại updateProducts để cập nhật kết quả lọc
    });

    // Xử lý sự kiện cho bộ lọc danh mục
    $('#categorySelect').change(updateProducts);
});