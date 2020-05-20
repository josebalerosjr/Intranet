var dataTable;

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("inprocess")) {
        loadDataTable("GetOrderList?status=inprocess");
    } else {
        if (url.includes("pending")) {
            loadDataTable("GetOrderList?status=pending");
        } else {
            if (url.includes("completed")) {
                loadDataTable("GetOrderList?status=completed");
            } else {
                if (url.includes("rejected")) {
                    loadDataTable("GetOrderList?status=rejected");
                } else {
                    loadDataTable("GetOrderList?status=all");
                }
            }
        }
    }
});


function loadDataTable(url) {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/CorpComm/Order/" + url
        },
        "columns": [
            { "data": "id", "width": "10%" },
            { "data": "loginUser", "width": "25%" },
            { "data": "orderTotal", "width": "25%" },
            { "data": "orderStatus", "width": "25%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href="/CorpComm/Order/Details/${data}" class="text-success" style="cursor:pointer">
                                    <i class="fas fa-edit"></i> 
                                </a>
                            </div>
                           `;
                }, "width": "15%"
            }
        ]
    });
}