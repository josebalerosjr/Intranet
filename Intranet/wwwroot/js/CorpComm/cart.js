function singleSelectChangeValue() {
    //Getting Value
    //var selValue = document.getElementById("singleSelectDD").value;
    var selObj = document.getElementById("singleSelectValueDDJS");
    var selValue = selObj.options[selObj.selectedIndex].value;
    var totalAmount = document.getElementById("txtOrderTotal").innerText;
    //Setting Value
    if (selObj.value === '') {
        document.getElementById("textFieldValueJS").innerText = 0;
    } else {
        document.getElementById("textFieldValueJS").innerText = selValue;
    }
    compareVal();
}

function compareVal() {
    var budgetLim = document.getElementById("singleSelectValueDDJS").value;
    var totalRequest = document.getElementById("txtOrderTotal").innerText;

    if (parseInt(budgetLim) > parseInt(totalRequest)) {
        document.getElementById("summaryButton").disabled = false;
    } else {
        document.getElementById("summaryButton").disabled = true;
    }
}