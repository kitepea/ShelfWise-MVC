var dataTable;
$(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#userTable').DataTable({
        "ajax": { url: '/admin/user/getall' },
        "columns": [
            { data: "name", "width": "25%" },
            { data: "email", "width": "15%" },
            { data: "phoneNumber", "width": "10%" },
            { data: "company.name", "width": "15%" },
            { data: "role", "width": "10%" },
            {
                data: "id",
                "render": function (data) {
                    return `<div class="w-100 btn-group d-flex justify-content-center" role="group">
                                <a href="/admin/user/upsert?id=${data}" class="btn btn-primary mx-2 w-25 px-0"> <i class="bi bi-pencil-square"></i> Edit</a>
                            </div>`
                },
                "width": "25%"
            }
        ]
    });
}