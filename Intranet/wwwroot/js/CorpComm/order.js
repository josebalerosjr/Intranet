var dataTable;

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("requestsent")) {
        loadDataTable("GetOrderList?status=requestsent");
    } else {
        if (url.includes("forapproval")) {
            loadDataTable("GetOrderList?status=forapproval");
        } else {
            if (url.includes("fordelivery")) {
                loadDataTable("GetOrderList?status=fordelivery");
            } else {
                if (url.includes("foracknowledge")) {
                    loadDataTable("GetOrderList?status=foracknowledge");
                } else {
                    if (url.includes("forrating")) {
                        loadDataTable("GetOrderList?status=forrating");
                    } else {
                        if (url.includes("rejected")) {
                            loadDataTable("GetOrderList?status=rejected")
                        } else {
                            loadDataTable("GetOrderList?status=all");
                        }
                    }
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
            { "data": "orderTotal", "width": "15%" },
            { "data": "orderStatus", "width": "25%" },
            { "data": "orderRating", "width": "10%" },
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