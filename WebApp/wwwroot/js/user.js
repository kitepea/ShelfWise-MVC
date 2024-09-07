var dataTable;
$(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#userTable').DataTable({
        "ajax": { url: '/admin/user/getall' },
        "columns": [
            { data: "name", "width": "15%" },
            { data: "email", "width": "15%" },
            { data: "phoneNumber", "width": "15%" },
            { data: "company.name", "width": "15%" },
            { data: "role", "width": "10%" },
            {
                data: { id: "id", lockoutEnd: "lockoutEnd" },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime(); // return time value in milliseconds

                    if (lockout > today) {
                        return `
                        <div class="w-100 btn-group d-flex justify-content-center" role="group">
                             <a onclick=LockUnlock('${data.id}') class="btn btn-danger text-white w-25 px-0" style="cursor:pointer;">
                                    <i class="bi bi-lock-fill"></i>  Locked
                                </a> 
                                <a href="/admin/user/RoleManagement?userId=${data.id}" class="btn btn-danger text-white w-25 px-0" style="cursor:pointer">
                                     <i class="bi bi-pencil-square"></i> Permission
                                </a>
                        </div>
                        `
                    }
                    else {
                        return `
                        <div class="w-100 btn-group d-flex justify-content-center" role="group">
                              <a onclick=LockUnlock('${data.id}') class="btn btn-success text-white w-25 px-0" style="cursor:pointer; width:100px;">
                                    <i class="bi bi-unlock-fill"></i>  UnLocked
                                </a>
                                <a href="/admin/user/RoleManagement?userId=${data.id}" class="btn btn-danger text-white w-25 px-0" style="cursor:pointer">
                                     <i class="bi bi-pencil-square"></i> Permission
                                </a>
                        </div>
                        `
                    }
                },
                "width": "25%"
            }
        ]
    });
}

function LockUnlock(id) {
    $.ajax({
        type: "POST",
        url: '/Admin/User/LockUnlock',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            }
        }
    });
}