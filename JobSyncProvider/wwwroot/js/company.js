
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
        responsive: true,
        "ajax": {
            url: '/employer/company/getallcompanies',
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
            /*            { data: 'name' },*/
            {
                data: 'logoUrl',  // We now expect the logoUrl as part of the data from the server
                "render": function (data, type, row) {
                    // If the logoUrl exists, render the image alongside the name
                    var logoSrc = data && data !== '' ? data : '/images/NoImage.png'; // Use default image if no logo URL
                    return `<img src="${logoSrc}" alt="Logo" class="img-fluid" style="max-width: 30px; max-height: 30px; margin-right: 10px; vertical-align: middle;" />${row.name}`;
                }
            },
            { data: 'website', "width": "30%" },
            {
                data: 'isActive',
                "render": function (data, type, row) {
                    if (data === true) {
                        return `<a class="btn btn-sm btn-success w-100" onclick=UpdateStatus('${row.companyId}','${data}')>Active</a>`
                    }
                    else {
                        return `<a class="btn btn-sm btn-danger w-100" onclick=UpdateStatus('${row.companyId}','${data}')>Inactive</a>`
                    }
                },
                "width": "10%"
            },
            {
                data: 'companyId',
                "render": function (data) {
                    return `<div class="d-flex justify-content-between"><a class="btn btn-sm btn-warning ms-1" href="/employer/company/edit/${data}" data-bs-toggle="tooltip" title="Edit Company">
                                    <i class="bi bi-pencil-square"></i>
                                </a>
							    <a class="btn btn-sm btn-info ms-1"  href="/employer/company/details?id=${data}" data-bs-toggle="tooltip" title="Company Details">
                                    <i class="bi bi-info-square"></i>
                                </a>
							    <a class="btn btn-sm btn-danger ms-1" onclick=Delete('/employer/company/delete/${data}') data-bs-toggle="tooltip" title="Delete Company">
                                    <i class="bi bi-trash"></i>
                                </a></div>`
                },
                "width":"10%"
            }
        ]
    });
}

function UpdateStatus(id, status) {

    // Determine the message based on the status
    var title = '';
    var text = '';
    var confirmText = '';

    if (status === true) {
        title = "Are you sure?";
        text = "This will mark the company as \"INACTIVE\"!";
        confirmText = "Proceed to Deactivate";
    } else {
        title = "Are you sure?";
        text = "This will mark the company as \"ACTIVE\"!";
        confirmText = "Proceed to Activate";
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
                url: '/employer/company/updatestatus',
                type: 'POST',
                data: JSON.stringify(id),
                contentType: 'application/json',
                success: function (data) {
                    dataTable.ajax.reload();
                    if (data.success) {
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message || 'Update Failed, An Error Encountered.');
                    }
                },
                error: function (xhr, status, error) {
                    // In case of AJAX failure
                    Swal.fire({
                        icon: "error",
                        title: "Error Encountered",
                        text: 'There was an error with the request. Please try again.',
                        showConfirmButton: true,
                    });
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
                    if (data.success) {
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message || 'Deletion Failed, An Error Encountered.');
                    }
                },
                error: function (xhr, status, error) {
                    // In case of AJAX failure
                    Swal.fire({
                        icon: "error",
                        title: "Error Encountered",
                        text: 'There was an error with the request. Please try again.',
                        showConfirmButton: true,
                    });
                }
            })
        }
    });
}