function singleSelectChangeValue() {
    //Getting Value
    var selObj = document.getElementById("singleSelectValueDDJS");
    var selValue = selObj.options[selObj.selectedIndex].value;
    var seltext = selObj.options[selObj.selectedIndex].innerText;

    //Setting Value
    if (selObj.value === '') {
        document.getElementById("textFieldValueJS").innerText = 0;
        $('#eventType').val('');
    } else {
        document.getElementById("textFieldValueJS").innerText = selValue;
        $('#eventType').val(seltext);
    }

    compareVal();
}

function compareVal() {
    var budgetLim = document.getElementById("singleSelectValueDDJS").value;
    var totalRequest = document.getElementById("txtOrderTotal").innerText;
    var totalRequest = document.getElementById("txtOrderTotal").innerText;

    if (parseInt(budgetLim) > parseInt(totalRequest)) {
        document.getElementById("summaryButton").disabled = false;
    } else {
        document.getElementById("summaryButton").disabled = true;
    }
}