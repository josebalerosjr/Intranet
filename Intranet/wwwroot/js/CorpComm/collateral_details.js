$(document).ready(function () {
    $('#itemCount').keyup(function () {
        var itemcount = parseInt($('#itemCount').val());
        var itemqty = parseInt($('#itemQty').val());
        var txtwarning = $('#txtWarning').text("");

        //if (itemcount.toString().length == 0 || itemcount == 0) {
        //    $('#add2cart').attr('disabled', true);
        //    txtwarning.text('Count should not be NULL or 0.'); 
        if (itemqty >= itemcount) {
            $('#add2cart').attr('disabled', false);
        } else {
            txtwarning.text('Count should not be NULL or exeed available quantity.'); 
            $('#add2cart').attr('disabled', true);
        }
    });
})