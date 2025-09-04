$(document).ready(function () {
    // Show create buttons
    $("#CreateGroupBtn").show();

    $("#btnCreate").on('click', function (e) {
        if ($("#formCreateUMA").valid()) {
            $("#Status").val("Submit");
            $("#formCreateUMA").submit();
        } else {
            ShowNoty("error", "Please fill all required fields!");
        }
    });

    $("#btnDraft").on('click', function (e) {
        if ($("#formCreateUMA").valid()) {
            $("#Status").val("Draft");
            $("#formCreateUMA").submit();
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