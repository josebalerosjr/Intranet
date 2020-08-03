var dataTable;

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("completed")) {
        loadDataTable("GetOrderList?status=completed");
    } else {
        if (url.includes("forapproval")) {
            loadDataTable("GetOrderList?status=forapproval");
        } else {
            if (url.includes("fordelivery")) {
                loadDataTable("GetOrderList?status=fordelivery");
            } else {
                if (url.includes("foracknowledge")) {
                    loadDataTable("GetOrderList?status=foracknowledgement");
                } else {
                    if (url.includes("forrating")) {
                        loadDataTable("GetOrderList?status=forrating");
                    } else {
                        if (url.includes("rejected")) {
                            loadDataTable("GetOrderList?status=rejected");
                        } else {
                            if (url.includes("approved")) {
                                loadDataTable("GetOrderList?status=approved");
                            } else {
                                if (url.includes("canceled")) {
                                    loadDataTable("GetOrderList?status=canceled");
                                } else {
                                    loadDataTable("GetOrderList?status=all");
                                }
                            }
                        }
                    }
                }
            }
        }
    }
});

function loadDataTable(url) {
    dataTable = $('#tblData').DataTable({
        "displayLength": 10,
        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
        dom: 'Bfrtip',
        "ajax": {
            "url": "/CorpComm/Order/" + url
        },
        "columns": [
            { "data": "id" },
            { "data": "orderDate" },
            { "data": "loginUser" },
            { "data": "eventName" },
            { "data": "stationEvent" },
            { "data": "eventDate" },
            { "data": "orderTotal" },
            { "data": "orderStatus" },
            { "data": "orderRating" },
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
                }
            }
        ]
    });
}