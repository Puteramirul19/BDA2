$(document).ready(function () {
    // Show appropriate buttons based on status
    $("#ConfirmGroupBtn").show();

    $("#btnConfirmPayment").on('click', function (e) {
        if ($("#formConfirmUMA").valid()) {
            $("#Status").val("PaymentReceived");
            $("#formConfirmUMA").submit();
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