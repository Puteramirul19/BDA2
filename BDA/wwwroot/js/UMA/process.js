$(document).ready(function () {

    // Initialize dropdowns
    $("#ddlBankProcessType").select2({
        minimumResultsForSearch: -1,
        ajax: {
            url: "/UMA/GetAllBankProcessType",
            dataType: 'json',
            type: "GET",
            processResults: function (data) {
                return {
                    results: $.map(data, function (item) {
                        return {
                            text: item.name,
                            id: item.id
                        }
                    })
                };
            }
        }
    });

    // Bank process type change event
    $("#ddlBankProcessType").on('change', function () {
        if (this.value == "1") { // Maybank
            $('#pnl_less_oneyear').show();
            $('#pnl_more_oneyear').hide();
        } else if (this.value == "2") { // UMA
            $('#pnl_less_oneyear').hide();
            $('#pnl_more_oneyear').show();
        }
    });

    // Submit to Bank button
    $("#formProcessUMA").on('submit', function (e) {
        e.preventDefault();

        var formData = new FormData(this);

        $.ajax({
            url: $(this).attr('action'),
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (data) {
                if (data.response.statusCode == 200) {
                    ShowNoty("success", data.message);
                    setTimeout(function () {
                        window.location.reload();
                    }, 2000);
                } else {
                    ShowNoty("error", data.message);
                }
            },
            error: function () {
                ShowNoty("error", "An error occurred while processing the request.");
            }
        });
    });
});