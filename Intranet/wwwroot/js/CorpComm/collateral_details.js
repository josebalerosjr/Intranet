$(document).ready(function () {
    $('#itemCount').keyup(function () {
        var itemcount = parseInt($('#itemCount').val());
        var itemqty = parseInt($('#itemQty').val());

        if (itemqty >= itemcount) {
            $('#add2cart').attr('disabled', false);
        } else {
            swal("Error","Count should not be NULL or exeed available quantity."); 
            $('#add2cart').attr('disabled', true);
        }
    });
})