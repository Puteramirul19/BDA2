$(document).ready(function () {
    // Show appropriate buttons based on status
    $("#ReceiveGroupBtn").show();

    $("#btnSubmitToANM").on('click', function (e) {
        if ($("#formReceiveUMA").valid()) {
            $("#Status").val("SubmittedToANM");
            $("#formReceiveUMA").submit();
        } else {
            ShowNoty("error", "Please fill all required fields!");
        }
    });

    $("#btnDraft").on('click', function (e) {
        if ($("#formReceiveUMA").valid()) {
            $("#Status").val("Draft");
            $("#formReceiveUMA").submit();
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