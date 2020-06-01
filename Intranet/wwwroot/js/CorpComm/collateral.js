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
            { "data": "name", "width": "15%" },
            { "data": "description", "width": "38%" },
            { "data": "size.name", "width": "5%" },
            { "data": "unit.name", "width": "5%" },
            { "data": "brand.name", "width": "5%" },
            { "data": "location.name", "width": "10%" },
            { "data": "count", "width": "5%" },
            { "data": "price", "width": "5%" },
            { "data": "isActive", "width": "5%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href="/CorpComm/Collateral/Upsert/${data}" class="text-success" style="cursor:pointer">
                                    <i class="fas fa-edit"></i> 
                                </a>
                                <a onclick=Delete("/CorpComm/Collateral/Delete/${data}") class="text-danger" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i> 
                                </a>
                                <!--<a onclick=Delete("/CorpComm/Collateral/Withdraw/${data}") class="text-warning" style="cursor:pointer">
                                    <i class="fas fa-cart-arrow-down"></i>
                                </a>
                                <a onclick=Delete("/CorpComm/Collateral/AddStocks/${data}") class="text-info" style="cursor:pointer">
                                    <i class="fas fa-cart-plus"></i>
                                </a>-->
                            </div>
                           `;
                }, "width": "7%"
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