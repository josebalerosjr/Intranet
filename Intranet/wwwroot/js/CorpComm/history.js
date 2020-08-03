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
            "url": "/CorpComm/History/GetAll"
        },
        "columns": [
            { "data": "requestId" },
            { "data": "reconRemarks" },
            { "data": "requestDate" },
            { "data": "loginUser" },
            { "data": "collateralName" },
            { "data": "quantity" },
            { "data": "eventType" },
            { "data": "stationEvent" },
            { "data": "eventDate" },
            { "data": "shippingDate" },
            { "data": "dropOffPoint" },
            { "data": "rating" }
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