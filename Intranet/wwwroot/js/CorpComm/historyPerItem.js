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
            "url": "/CorpComm/History/GetAllItemHistory"
        },
        "columns": [
            { "data": "requestId" },
            { "data": "requestDate" },
            { "data": "loginUser" },
            { "data": "collateralName" },
            { "data": "eventType" },
            { "data": "quantity" },
            { "data": "stationEvent" },
            { "data": "eventDate" },
            { "data": "shippingDate" },
            { "data": "dropOffPoint" },
            { "data": "rating" }
        ]
    });
}