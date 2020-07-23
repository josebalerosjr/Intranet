var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "displayLength": 10,
        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
        dom: 'Bfrtip',
        "ajax": {
            "url": "/CorpComm/Collateral/GetAll"
        },
        "columns": [
            { "data": "name", "width": "20%" },
            { "data": "size.name", "width": "10%" },
            { "data": "unit.name", "width": "5%" },
            { "data": "brand.name", "width": "15%" },
            { "data": "location.name", "width": "10%" },
            { "data": "count", "width": "10%" },
            { "data": "price", "width": "10%" },
            {
                "data": "isActive",
                "render": function (data) {
                    if (data) {
                        return `<div class="text-center">
                                    <input type="checkbox" disabled checked class="form-check-input" />
                                </div>`
                    }
                    else {
                        return `<div class="text-center">
                                    <input type="checkbox" disabled class="form-check-input" />
                                </div>`
                    }
                },
                "width": "10%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href="/CorpComm/Collateral/Upsert/${data}" class="text-success" style="cursor:pointer">
                                    <i class="fas fa-edit" title="Edit"></i>
                                </a>

                                <a onclick=Delete("/CorpComm/Collateral/Delete/${data}") class="text-danger" style="cursor:pointer">
                                    <i class="fas fa-trash-alt" title="Delete"></i>
                                </a>

                                <a href="/CorpComm/Collateral/Transfer/${data}" asp-route-id="${data}" class="text-primary" style="cursor:pointer">
                                    <i class="fa fa-exchange" aria-hidden="true" title="Transfer"></i>
                                </a>

                                <a href="/CorpComm/History/GetAllId/${data}" asp-route-id="${data}" class="text-info" style="cursor:pointer">
                                    <i class="fas fa-history" aria-hidden="true" title="History"></i>
                                </a>
                                <a href="/CorpComm/History/GetAllIdAddHistory/${data}" asp-route-id="${data}" class="text-warning" style="cursor:pointer">
                                    <i class="fas fa-balance-scale" aria-hidden="true" title="Adjust"></i>
                                </a>
                            </div>
                           `;
                }, "width": "10%"
            }
        ]
    });
}

function Delete(url) {
    swal({
        title: "Are you sure you want to Delete?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}