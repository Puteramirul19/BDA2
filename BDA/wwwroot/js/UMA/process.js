$(document).ready(function () {
    // Show appropriate buttons based on status
    $("#ProcessGroupBtn").show();

    $("#btnSubmitToBank").on('click', function (e) {
        if ($("#formProcessUMA").valid()) {
            $("#Status").val("Processed");
            $("#formProcessUMA").submit();
        } else {
            ShowNoty("error", "Please fill all required fields!");
        }
    });

    $("#btnDraft").on('click', function (e) {
        if ($("#formProcessUMA").valid()) {
            $("#Status").val("Draft");
            $("#formProcessUMA").submit();
        } else {
            ShowNoty("error", "Please fill all required fields!");
        }
    });
});

function ShowNoty(type, message) {
    new Noty({
        type: type,
        layout: 'topRight',
        text: message,
        timeout: 3000
    }).show();
}