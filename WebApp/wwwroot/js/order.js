var dataTable;
$(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#orderTable').DataTable({
        "ajax": { url: '/admin/order/getall' },
        "columns": [
            { data: "id", "width": "5%" },
            { data: "name", "width": "15%" },
            { data: "phoneNumber", "width": "20%" },
            { data: "applicationUser.email", "width": "15%" },
            { data: "orderStatus", "width": "10%" },
            { data: "orderTotal", "width": "10%" },
            {
                data: "id",
                "render": function (data) {
                    return `<div class="w-100 btn-group d-flex justify-content-center" role="group">
                                <a href="/admin/order/details?orderId=${data}" class="btn btn-primary mx-2 w-25 px-0"> <i class="bi bi-pencil-square"></i></a>
                            </div>`
                },
                "width": "25%"
            }
        ]
    });
}

