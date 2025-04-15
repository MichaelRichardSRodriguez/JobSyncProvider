
var dataTable;

$(document).ready(function () {
    loadDataTable();

    // Filter by status
    $('#statusFilter').on('change', function () {
        // Reload the table with the selected filter
        dataTable.ajax.reload();
    });
});


function loadDataTable(selectedStatus) {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            url: '/admin/category/getallcategories',
            type: 'GET',
            data: function (d) {
                // Get the selected status from the dropdown and add it to the AJAX request
                var selectedStatus = $('#statusFilter').val();
                
                if (selectedStatus) {
                    d.selectedStatus = selectedStatus;
                }
            }
        },
        "columns": [
            { data: 'name'},
            {
                data: 'status',
                "render": function (data, type, row) {
                    if (data === 'Valid') {
                        return `<a class="btn btn-sm btn-success w-100" onclick=UpdateStatus('${row.categoryId}','${data}')>${data}</a>`
                    }
                    else {
                        return `<a class="btn btn-sm btn-danger w-100" onclick=UpdateStatus('${row.categoryId}','${data}')>${data}</a>`
                    }
                },
                "width": "20%"
            },
            {
                data: 'categoryId',
                "render": function (data) {
                    return `<div class="d-flex justify-content-between">
                                <a class="btn btn-sm btn-warning mb-sm-2 mb-md-0" href="/admin/category/edit/${data}" data-bs-toggle="tooltip" title="Edit Category">
                                    <i class="bi bi-pencil-square"></i>
                                </a>
							    <a class="btn btn-sm btn-info mb-sm-2 mb-md-0" href="/admin/category/details?id=${data}" data-bs-toggle="tooltip" title="Category Details">
                                    <i class="bi bi-info-square"></i>
                                </a>
							    <a class="btn btn-sm btn-danger mb-sm-2 mb-md-0" onclick=Delete('/admin/category/delete/${data}') data-bs-toggle="tooltip" title="Delete Category">
                                    <i class="bi bi-trash"></i>
                                </a>
                           </div>`
                },
                "width": "15%"
            }
        ]
    });
}

function UpdateStatus(id,status) {

    // Determine the message based on the status
    var title = '';
    var text = '';
    var confirmText = '';

    if (status === 'Valid') {
        title = "Are you sure?";
        text = "This will mark the category as \"INVALID\"!";
        confirmText = "Proceed to Invalidate";
    } else {
        title = "Are you sure?";
        text = "This will mark the category as \"VALID\"!";
        confirmText = "Proceed to Validate";
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
                url: '/admin/category/updatestatus',
                type: 'POST',
                data: JSON.stringify(id),
                contentType: 'application/json',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    });
}

function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Proceed Delete"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    Swal.fire({
                        title: "Deleted!",
                        text: "Category has been deleted.",
                        icon: "success",
                        showConfirmButton: false,
                        timer: 1500
                    });
                }
            })
        }
    });
}