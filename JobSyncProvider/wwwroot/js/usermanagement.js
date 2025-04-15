
var dataTable;

//On Page Load
$(document).ready(function () {
    loadDataTable();

    // Filter by status
    $('#statusFilter').on('change', function () {
        // Reload the table with the selected filter
        dataTable.ajax.reload();
    });

    // Filter by role
    $('#roleFilter').on('change', function () {
        // Reload the table with the selected filter
        dataTable.ajax.reload();
    });
});


//Loading Records in Datatable
function loadDataTable(selectedStatus,selectedRole) {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            url: '/admin/user/getallusers',
            type: 'GET',
            data: function (d) {
                // Get the selected status from the dropdown and add it to the AJAX request
                var selectedStatus = $('#statusFilter').val();
                var selectedRole = $('#roleFilter').val();
                
                if (selectedStatus) {
                    d.selectedStatus = selectedStatus;
                }

                if (selectedRole) {
                    d.selectedRole = selectedRole;
                }
            }
        },
        "columns": [
            { data: 'fullName' },
            { data: 'email', "width": "30%" },
            {
                data: 'role',
                "width": "10%",
                "render": function (data, type, row) {
                    let roleClass = ''
                    if (data === 'Admin') {
                        roleClass = 'text-bg-warning'
                    }
                    else if (data === 'Employer') {
                        roleClass = 'text-bg-primary'
                    }
                    else {
                        roleClass = 'text-bg-info'
                    }

                    return `<span class="badge rounded-pill ${roleClass} w-100">${data}</span>`;
                }
            },
            {
                data: 'status',
                width: '2%',
                "render": function (data, type, row) {
                    if (data === 'Valid') {
                        return `<div class="d-flex justify-content-between">
                                    <a class="btn btn-sm btn-success mb-sm-2 mb-md-0" onclick=UpdateStatus('${row.id}','${data}') data-bs-toggle="tooltip" title="Lock User">
                                        <i class="bi bi-unlock-fill"></i>
                                    </a>
                                    <a class="btn btn-sm btn-primary mb-sm-2 mb-md-0" href="/admin/user/ManageRole?id=${row.id}" data-bs-toggle="tooltip" title="Manage User Role">
                                        <i class="bi bi-person-gear"></i>
                                    </a>
                                </div>` 
                    }
                    else
                    {
                        return `<div class="d-flex justify-content-between">
                                    <a class="btn btn-sm btn-danger mb-sm-2 mb-md-0" onclick=UpdateStatus('${row.id}','${data}') data-bs-toggle="tooltip" title="Unlock User">
                                        <i class="bi bi-lock-fill"></i>
                                    </a>
                                    <a class="btn btn-sm btn-primary mb-sm-2 mb-md-0" href="/admin/user/ManageRole?id=${row.id}" data-bs-toggle="tooltip" title="Manage User Role">
                                        <i class="bi bi-person-gear"></i>
                                    </a>
                                </div>`
                    }
                }
            },
        ]
    });
}


//Update User Status "Valid / Invalid" using IdentityUser Id
function UpdateStatus(id,status) {

    // Determine the message based on the status
    var title = '';
    var text = '';
    var confirmText = '';

    if (status === 'Valid') {
        title = "Are you sure?";
        text = "This will \"LOCK\" the user's record!";
        confirmText = "Proceed Lock";
    } else {
        title = "Are you sure?";
        text = "This will \"UNLOCK\" the user's record!";
        confirmText = "Proceed Unlock";
    }

    Swal.fire({
        title: title,
        text: text,
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: confirmText
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/admin/user/updateuserstatus',
                type: 'POST',
                data: JSON.stringify(id),
                contentType: 'application/json',
                success: function (data) {
                    dataTable.ajax.reload();

                    if (data.success === true) {
                        toastr.success(data.message);
                    }
                    else {
                        dataTable.ajax.reload();
                        toastr.error(data.message);
                    }
                },
                error: function (xhr, status, error) {
                    // Check if xhr.responseJSON or xhr.responseText contains an error message
                    const errorMessage = xhr.responseJSON?.message || xhr.responseText || "Something went wrong.";
                    toastr.error(errorMessage);
                }
            })
        }
    });
}

