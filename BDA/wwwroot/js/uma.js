$(document).ready(function () {
    var status = '@ViewBag.Status';

    if (status == 'Created' || status == 'Draft' || status == '') {
        $("#CreateGroupBtn").show();
    }
    else if (status == 'Submitted') {
        $("#ProcessGroupBtn").show();
    }
    else if (status == 'Processed') {
        $("#ReceiveGroupBtn").show();
    }
    else if (status == 'Received') {
        $("#ConfirmGroupBtn").show();
    }

    $("#btnCreate").on('click', function (e) {
        if ($("#formCreateUMA").valid()) {
            $("#Status").val("Submit");
            $("#formCreateUMA").submit();
        } else {
            ShowNoty("error", "Please fill all required fields!");
        }
    });

    $("#btnDraft").on('click', function (e) {
        $("#Status").val("Draft");
        $("#formCreateUMA").submit();
    });

    $("#btnProcess").on('click', function (e) {
        if ($("#formProcessUMA").valid()) {
            $("#Status").val("Process");
            $("#formProcessUMA").submit();
        } else {
            ShowNoty("error", "Please fill all required fields!");
        }
    });

    $("#btnReceive").on('click', function (e) {
        if ($("#formReceiveUMA").valid()) {
            $("#Status").val("Receive");
            $("#formReceiveUMA").submit();
        } else {
            ShowNoty("error", "Please fill all required fields!");
        }
    });

    $("#btnCompleteConfirm").on('click', function (e) {
        if ($("#formConfirmUMA").valid()) {
            $("#Status").val("Complete");
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