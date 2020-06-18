function pointsCheckers() {
    var pointschecker = document.getElementById('pointsChecker').value;

    if (pointschecker.toString() == '-Points') {
        swal("Error", "Please select a number");
        return false;
    } else {
        return true;
    }
}